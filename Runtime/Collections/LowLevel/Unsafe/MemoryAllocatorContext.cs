using System;

namespace ME.ECS.Collections.LowLevel.Unsafe {

    public struct MemoryAllocatorContext : IDisposable {

        public static readonly Unity.Burst.SharedStatic<MemoryAllocator> burstAllocator = Unity.Burst.SharedStatic<MemoryAllocator>.GetOrCreate<MemoryAllocatorContext, MemoryAllocator>();
        
        public MemoryAllocator allocator;

        public MemoryAllocatorContext Create() {
            
            MemoryAllocatorContext.burstAllocator.Data = this.allocator;
            return this;

        }

        public void Dispose() {

            MemoryAllocatorContext.burstAllocator.Data = default;

        }

    }

}