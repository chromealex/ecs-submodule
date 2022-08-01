//#define MEMORY_ALLOCATOR_BOUNDS_CHECK

using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

namespace ME.ECS.Collections.V3 {

    using MemPtr = System.Int64;

    public unsafe partial struct MemoryAllocator : IMemoryAllocator<MemoryAllocator, MemPtr> {

        private const long OFFSET_MASK = 0xFFFFFFFF;
        private const long MIN_ZONE_SIZE = 512 * 1024;
        private const int MIN_ZONES_LIST_CAPACITY = 20;

        [NativeDisableUnsafePtrRestriction]
        internal MemZone** zonesList;
        internal int zonesListCount;
        internal int zonesListCapacity;
        internal long maxSize;
        
        public bool isValid => this.zonesList != null;

        /// 
        /// Constructors
        /// 
        public MemoryAllocator Initialize(long initialSize, long maxSize = -1L) {

            this.AddZone(MemoryAllocator.ZmCreateZone((int)Math.Max(initialSize, MemoryAllocator.MIN_ZONE_SIZE)));
            
            this.maxSize = maxSize;

            return this;
        }

        public void Dispose() {

            this.FreeZones();
            
            if (this.zonesList != null) {
                UnsafeUtility.Free(this.zonesList, Allocator.Persistent);
            }

            this.zonesListCapacity = 0;
            this.maxSize = default;

        }

        public void CopyFrom(in MemoryAllocator other) {

            if (other.zonesList == null && this.zonesList == null) {
                
            } else if (other.zonesList == null && this.zonesList != null) {
                this.FreeZones();
            } else {
                this.FreeZones();

                for (int i = 0; i < other.zonesListCount; i++) {
                    var otherZone = other.zonesList[i];
                    var zone = MemoryAllocator.ZmCreateZone(otherZone->size);
                    UnsafeUtility.MemCpy(zone, otherZone, otherZone->size);

                    this.AddZone(zone);
                }

            }
            
            this.maxSize = other.maxSize;
        }

        private void FreeZones() {
            if (this.zonesList != null) {
                for (int i = 0; i < this.zonesListCount; i++) {
                    MemoryAllocator.ZmFreeZone(this.zonesList[i]);
                }
            }

            this.zonesListCount = 0;
        }

        private int AddZone(MemZone* zone) {

            if (this.zonesListCapacity <= this.zonesListCount) {
                var capacity = Math.Max(MemoryAllocator.MIN_ZONES_LIST_CAPACITY, this.zonesListCapacity * 2);
                var list = (MemZone**)UnsafeUtility.Malloc(capacity * sizeof(MemZone*), UnsafeUtility.AlignOf<byte>(), Allocator.Persistent);

                if (this.zonesList != null) {
                    for (int i = 0; i < this.zonesListCount; i++) {
                        list[i] = this.zonesList[i];
                    }
                    
                    UnsafeUtility.Free(this.zonesList, Allocator.Persistent);
                }
                
                this.zonesList = list;
                this.zonesListCapacity = capacity;
            }

            this.zonesList[this.zonesListCount++] = zone;

            return this.zonesListCount - 1;
        }

        /// 
        /// Base
        ///
        [INLINE(256)]
        public readonly ref T Ref<T>(MemPtr ptr) where T : unmanaged {
            return ref UnsafeUtility.AsRef<T>(this.GetUnsafePtr(ptr));
        }

        [INLINE(256)]
        public readonly ref T RefUnmanaged<T>(MemPtr ptr) where T : struct {
            return ref UnsafeUtility.AsRef<T>(this.GetUnsafePtr(ptr));
        }

        [INLINE(256)]
        public MemPtr AllocData<T>(T data) where T : unmanaged {
            var ptr = this.Alloc<T>();
            this.Ref<T>(ptr) = data;
            return ptr;
        }

        [INLINE(256)]
        public MemPtr Alloc<T>() where T : unmanaged {
            var size = sizeof(T);
            var alignOf = UnsafeUtility.AlignOf<T>();
            return this.Alloc(size + alignOf);
        }

        [INLINE(256)]
        public MemPtr AllocUnmanaged<T>() where T : struct {
            var size = UnsafeUtility.SizeOf<T>();
            var alignOf = UnsafeUtility.AlignOf<T>();
            return this.Alloc(size + alignOf);
        }

        [INLINE(256)]
        public MemPtr Alloc(long size) {

            void* ptr = null;

            for (int i = 0; i < this.zonesListCount; i++) {
                ptr = MemoryAllocator.ZmMalloc(this.zonesList[i], (int)size, null);

                if (ptr != null) {
                    return this.GetSafePtr(ptr, i);
                }
            }
            
            var zone = MemoryAllocator.ZmCreateZone((int)Math.Max(size, MemoryAllocator.MIN_ZONE_SIZE));
            var zoneIndex = this.AddZone(zone);
            
            ptr = MemoryAllocator.ZmMalloc(zone, (int)size, null);

            return this.GetSafePtr(ptr, zoneIndex);
            
        }

        [INLINE(256)]
        public bool Free(MemPtr ptr) {

            if (ptr == 0) return false;
            
            var zoneIndex = ptr >> 32;
            
            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            if (zoneIndex >= this.zonesListCount || this.zonesList[zoneIndex]->size < (ptr & MemoryAllocator.OFFSET_MASK)) {
                throw new OutOfBoundsException();
            }
            #endif
            
            return MemoryAllocator.ZmFree(this.zonesList[zoneIndex], this.GetUnsafePtr(ptr));
        }

        [INLINE(256)]
        public MemPtr ReAlloc(MemPtr ptr, long size) {
            
            if (ptr == 0L) return this.Alloc(size);;

            var blockSize = ((MemBlock*)((byte*)this.GetUnsafePtr(ptr) - sizeof(MemBlock)))->size;
            var blockDataSize = blockSize - sizeof(MemBlock);
            if (blockDataSize > size) {
                return ptr;
            }

            if (blockDataSize < 0) {
                throw new Exception();
            }

            var newPtr = this.Alloc(size);
            this.MemCopy(newPtr, 0, ptr, 0, blockDataSize);
            this.Free(ptr);

            return newPtr;
            
        }

        [INLINE(256)]
        public readonly void MemCopy(MemPtr dest, long destOffset, MemPtr source, long sourceOffset, long length) {
            
            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            var destZoneIndex = dest >> 32;
            var sourceZoneIndex = source >> 32;
            var destMaxOffset = (dest & MemoryAllocator.OFFSET_MASK) + destOffset + length;
            var sourceMaxOffset = (source & MemoryAllocator.OFFSET_MASK) + sourceOffset + length;
            
            if (destZoneIndex >= this.zonesListCount || sourceZoneIndex >= this.zonesListCount) {
                throw new OutOfBoundsException();
            }
            
            if (this.zonesList[destZoneIndex]->size < destMaxOffset || this.zonesList[sourceZoneIndex]->size < sourceMaxOffset) {
                throw new OutOfBoundsException();
            }
            #endif
            
            UnsafeUtility.MemCpy(this.GetUnsafePtr(dest + destOffset), this.GetUnsafePtr(source + sourceOffset), length);
        }

        [INLINE(256)]
        public readonly void MemClear(MemPtr dest, long destOffset, long length) {
            
            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            var zoneIndex = dest >> 32;
            
            if (zoneIndex >= this.zonesListCount || this.zonesList[zoneIndex]->size < ((dest & MemoryAllocator.OFFSET_MASK) + destOffset + length)) {
                throw new OutOfBoundsException();
            }
            #endif

            UnsafeUtility.MemClear(this.GetUnsafePtr(dest + destOffset), length);
        }

        [INLINE(256)]
        public void Prepare(long size) {

            for (int i = 0; i < this.zonesListCount; i++) {
                if (MemoryAllocator.ZmHasFreeBlock(this.zonesList[i], (int)size) == true) {
                    return;
                }
            }
 
            this.AddZone(MemoryAllocator.ZmCreateZone((int)Math.Max(size, MemoryAllocator.MIN_ZONE_SIZE)));
                
        }

        [INLINE(256)]
        public readonly void* GetUnsafePtr(in MemPtr ptr) {

            var zoneIndex = ptr >> 32;
            var offset = (ptr & MemoryAllocator.OFFSET_MASK);
            
            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            if (zoneIndex < this.zonesListCount && this.zonesList[zoneIndex]->size < offset) {
                throw new OutOfBoundsException();
            }
            #endif

            return (byte*)this.zonesList[zoneIndex] + offset;
        }

        [INLINE(256)]
        private readonly MemPtr GetSafePtr(void* ptr, int zoneIndex) {
            var index = (long)zoneIndex << 32;
            var offset = ((byte*)ptr - (byte*)this.zonesList[zoneIndex]);

            return index | offset;
        }

        /// 
        /// Arrays
        /// 
        [INLINE(256)]
        public readonly ref T RefArray<T>(MemPtr ptr, int index) where T : unmanaged {
            var size = sizeof(T);
            return ref UnsafeUtility.AsRef<T>(this.GetUnsafePtr(ptr + index * size));
        }

        [INLINE(256)]
        public MemPtr ReAllocArray<T>(MemPtr ptr, int newLength) where T : unmanaged {
            var size = sizeof(T);
            return this.ReAlloc(ptr, size * newLength);
        }

        [INLINE(256)]
        public MemPtr AllocArray<T>(int length) where T : unmanaged {
            var size = sizeof(T);
            return this.Alloc(size * length);
        }

        [INLINE(256)]
        public readonly ref T RefArrayUnmanaged<T>(MemPtr ptr, int index) where T : struct {
            var size = UnsafeUtility.SizeOf<T>();
            return ref UnsafeUtility.AsRef<T>(this.GetUnsafePtr(ptr + index * size));
        }

        [INLINE(256)]
        public MemPtr ReAllocArrayUnmanaged<T>(MemPtr ptr, int newLength) where T : struct {
            var size = UnsafeUtility.SizeOf<T>();
            return this.ReAlloc(ptr, size * newLength);
        }

        public MemPtr AllocArrayUnmanaged<T>(int length) where T : struct {
            var size = UnsafeUtility.SizeOf<T>();
            return this.Alloc(size * length);
        }

    }

}