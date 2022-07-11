using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace ME.ECS.Collections.V3 {

    public unsafe partial struct MemoryAllocator {

        private const int ZONE_ID = 0x1d4a11;

        private const int MIN_FRAGMENT = 64;

        public struct MemZone {

            public int size;           // total bytes malloced, including header
            public MemBlock blocklist; // start / end cap for linked list
            public MemBlockOffset rover;

        }

        public struct MemBlock {

            public int size;    // including the header and possibly tiny fragments
            public void** user; // NULL if a free block
            public int id;      // should be ZONEID
            public MemBlockOffset next;
            public MemBlockOffset prev;

        };

        public struct MemBlockOffset {

            public readonly long value;

            public MemBlockOffset(void* block, MemZone* zone) {
                this.value = (byte*)block - (byte*)zone;
            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public readonly MemBlock* Ptr(void* zone) {
                return (MemBlock*)((byte*)zone + this.value);
            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static bool operator ==(MemBlockOffset a, MemBlockOffset b) => a.value == b.value;

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static bool operator !=(MemBlockOffset a, MemBlockOffset b) => a.value != b.value;

        }

        public static void ZmClearZone(MemZone* zone) {
            var block = (MemBlock*)((byte*)zone + sizeof(MemZone));
            var blockOffset = new MemBlockOffset(block, zone);

            // set the entire zone to one free block
            zone->blocklist.next = zone->blocklist.prev = blockOffset;

            zone->blocklist.user = (void**)zone;
            zone->rover = blockOffset;

            block->prev = block->next = new MemBlockOffset(&zone->blocklist, zone);

            // NULL indicates a free block.
            block->user = null;

            block->size = zone->size - sizeof(MemZone);
        }

        public static MemZone* ZmCreateZone(int size) {
            size = MemoryAllocator.ZmGetMemBlockSize(size) + sizeof(MemZone);

            var zone = (MemZone*)UnsafeUtility.Malloc(size, UnsafeUtility.AlignOf<byte>(), Allocator.Persistent);

            UnsafeUtility.MemSet(zone, 0, size);
            zone->size = size;

            MemoryAllocator.ZmClearZone(zone);

            return zone;
        }

        public static MemZone* ZmReallocZone(MemZone* zone, int newSize) {
            if (zone->size >= newSize) return zone;

            var newZone = MemoryAllocator.ZmCreateZone(newSize);
            var extra = newZone->size - zone->size;

            UnsafeUtility.MemCpy(newZone, zone, zone->size);

            newZone->size = zone->size + extra;

            var top = newZone->rover.Ptr(newZone);

            for (var block = newZone->blocklist.next.Ptr(newZone); block != &newZone->blocklist; block = block->next.Ptr(newZone)) {
                if (block > top) {
                    top = block;
                }
            }

            if (top->user == null) {
                top->size += extra;
            } else {
                var newblock = (MemBlock*)((byte*)top + top->size);
                var newblockOffset = new MemBlockOffset(newblock, newZone);
                newblock->size = extra;

                newblock->user = null;
                newblock->prev = new MemBlockOffset(top, newZone);
                newblock->next = top->next;
                newblock->next.Ptr(newZone)->prev = newblockOffset;

                top->next = newblockOffset;
                newZone->rover = newblockOffset;
            }

            MemoryAllocator.ZmFreeZone(zone);

            return newZone;
        }

        public static void ZmFreeZone(MemZone* zone) {
            UnsafeUtility.Free(zone, Allocator.Persistent);
        }

        public static bool ZmFree(MemZone* zone, void* ptr) {
            var block = (MemBlock*)((byte*)ptr - sizeof(MemBlock));
            var blockOffset = new MemBlockOffset(block, zone);

            if (block->id != MemoryAllocator.ZONE_ID) {
                return false;
                //throw new System.ArgumentException("Free: freed a pointer without ZONEID");
            }

            if (block->user > (void**)0x100) {
                // smaller values are not pointers
                // Note: OS-dependend?

                // clear the user's mark
                *block->user = null;
            }

            // mark as free
            block->user = null;
            block->id = 0;

            var other = block->prev.Ptr(zone);
            var otherOffset = block->prev;

            if (other->user == null) {
                // merge with previous free block
                other->size += block->size;
                other->next = block->next;
                other->next.Ptr(zone)->prev = otherOffset;

                if (blockOffset == zone->rover) zone->rover = otherOffset;

                block = other;
                blockOffset = otherOffset;
            }

            other = block->next.Ptr(zone);
            otherOffset = block->next;
            if (other->user == null) {
                // merge the next free block onto the end
                block->size += other->size;
                block->next = other->next;
                block->next.Ptr(zone)->prev = blockOffset;

                if (otherOffset == zone->rover) zone->rover = blockOffset;
            }

            return true;
        }

        private static int ZmGetMemBlockSize(int size) {
            return ((size + 3) & ~3) + sizeof(MemBlock);
        }

        public static void* ZmMalloc(MemZone* zone, int size, void* user) {
            size = MemoryAllocator.ZmGetMemBlockSize(size);

            // scan through the block list,
            // looking for the first free block
            // of sufficient size,
            // throwing out any purgable blocks along the way.

            // if there is a free block behind the rover,
            //  back up over them
            var @base = zone->rover.Ptr(zone);

            if (@base->prev.Ptr(zone)->user != null) @base = @base->prev.Ptr(zone);

            var rover = @base;
            var start = @base->prev.Ptr(zone);

            do {
                if (rover == start) {
                    // scanned all the way around the list
                    return null;
                    //throw new System.OutOfMemoryException($"Malloc: failed on allocation of {size} bytes");
                }

                if (rover->user != null) {
                    // hit a block that can't be purged,
                    // so move base past it
                    @base = rover = rover->next.Ptr(zone);
                } else
                    rover = rover->next.Ptr(zone);
            } while (@base->user != null || @base->size < size);


            // found a block big enough
            var extra = @base->size - size;

            if (extra > MemoryAllocator.MIN_FRAGMENT) {
                // there will be a free fragment after the allocated block
                var newblock = (MemBlock*)((byte*)@base + size);
                var newblockOffset = new MemBlockOffset(newblock, zone);
                newblock->size = extra;

                // NULL indicates free block.
                newblock->user = null;
                newblock->prev = new MemBlockOffset(@base, zone);
                newblock->next = @base->next;
                newblock->next.Ptr(zone)->prev = newblockOffset;

                @base->next = newblockOffset;
                @base->size = size;

            }

            if (user != null) {
                // mark as an in use block
                @base->user = (void**)user;
                *(void**)user = (void*)((byte*)@base + sizeof(MemBlock));
            } else {
                // mark as in use, but unowned	
                @base->user = (void**)2;
            }

            @base->id = MemoryAllocator.ZONE_ID;

            // next allocation will start looking here
            zone->rover = @base->next;

            return (void*)((byte*)@base + sizeof(MemBlock));
        }

        public static void ZmDumpHeap(MemZone* zone) {
            UnityEngine.Debug.Log($"zone size: {zone->size}; location: {new IntPtr(zone)}; rover block offset: {zone->rover.value}");

            for (var block = zone->blocklist.next.Ptr(zone);; block = block->next.Ptr(zone)) {

                UnityEngine.Debug.Log($"block offset: {(byte*)block - (byte*)@zone}; size: {block->size}; user: {new IntPtr(block->user)}");

                if (block->next.Ptr(zone) == &zone->blocklist) break;

                MemoryAllocator.ZmCheckBlock(zone, block);
            }
        }

        public static void ZmCheckHeap(MemZone* zone) {
            for (var block = zone->blocklist.next.Ptr(zone);; block = block->next.Ptr(zone)) {
                if (block->next.Ptr(zone) == &zone->blocklist) {
                    // all blocks have been hit
                    break;
                }

                MemoryAllocator.ZmCheckBlock(zone, block);
            }
        }

        private static void ZmCheckBlock(MemZone* zone, MemBlock* block) {
            if ((byte*)block + block->size != (byte*)block->next.Ptr(zone)) {
                UnityEngine.Debug.LogError("CheckHeap: block size does not touch the next block\n");
            }

            if (block->next.Ptr(zone)->prev.Ptr(zone) != block) {
                UnityEngine.Debug.LogError("CheckHeap: next block doesn't have proper back link\n");
            }

            if (block->user == null && block->next.Ptr(zone)->user == null) {
                UnityEngine.Debug.LogError("CheckHeap: two consecutive free blocks\n");
            }
        }

        public static int ZmFreeMemory(MemZone* zone) {
            var free = 0;

            for (var block = zone->blocklist.next.Ptr(zone); block != &zone->blocklist; block = block->next.Ptr(zone)) {
                if (block->user == null) free += block->size;
            }

            return free;
        }

        public static bool ZmHasFreeBlock(MemZone* zone, int size) {
            size = MemoryAllocator.ZmGetMemBlockSize(size);

            for (var block = zone->blocklist.next.Ptr(zone); block != &zone->blocklist; block = block->next.Ptr(zone)) {
                if (block->user == null && block->size > size) {
                    return true;
                }

            }

            return false;
        }

    }

}