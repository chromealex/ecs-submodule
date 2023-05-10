//#define MEMORY_ALLOCATOR_BOUNDS_CHECK
//#define MEMORY_ALLOCATOR_LOGS

using System;

namespace ME.ECS.Collections.LowLevel.Unsafe {

    public static unsafe class MemoryAllocatorExtensions {

        #if !MEMORY_ALLOCATOR_LOGS && !MEMORY_ALLOCATOR_BOUNDS_CHECK
        //[BURST(CompileSynchronously = true)]
        #endif
        public static MemPtr Alloc(this ref MemoryAllocator allocator, int size) {

            void* ptr = null;

            for (int i = 0; i < allocator.zonesListCount; i++) {
                var zone = allocator.zonesList[i];
                
                if (zone == null) continue;
                
                ptr = MemoryAllocator.ZmMalloc(zone, (int)size);

                if (ptr != null) {
                    var memPtr = allocator.GetSafePtr(ptr, i);
                    #if MEMORY_ALLOCATOR_LOGS
                    MemoryAllocator.LogAdd(memPtr, size);
                    #endif
                    return memPtr;
                }
            }

            {
                var zone = MemoryAllocator.ZmCreateZone((int)Math.Max(size, MemoryAllocator.MIN_ZONE_SIZE));
                var zoneIndex = allocator.AddZone(zone);

                ptr = MemoryAllocator.ZmMalloc(zone, (int)size);

                var memPtr = allocator.GetSafePtr(ptr, zoneIndex);
                #if MEMORY_ALLOCATOR_LOGS
                MemoryAllocator.LogAdd(memPtr, size);
                #endif
                return memPtr;
            }

        }

        #if !MEMORY_ALLOCATOR_LOGS && !MEMORY_ALLOCATOR_BOUNDS_CHECK
        //[BURST(CompileSynchronously = true)]
        #endif
        public static bool Free(this ref MemoryAllocator allocator, MemPtr ptr) {

            if (ptr == MemPtr.Null) return false;

            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            if (ptr.zoneId >= allocator.zonesListCount || allocator.zonesList[ptr.zoneId] == null || allocator.zonesList[ptr.zoneId]->size < ptr.offset) {
                throw new OutOfBoundsException();
            }
            #endif
            
            var zone = allocator.zonesList[ptr.zoneId];

            #if MEMORY_ALLOCATOR_LOGS
            if (startLog == true) {
                MemoryAllocator.LogRemove(ptr);
            }
            #endif

            var success = false;

            if (zone != null) {
                success = MemoryAllocator.ZmFree(zone, allocator.GetUnsafePtr(ptr));

                if (MemoryAllocator.IsEmptyZone(zone) == true) {
                    MemoryAllocator.ZmFreeZone(zone);
                    allocator.zonesList[ptr.zoneId] = null;
                }
            }

            return success;
        }

    }

}