using ME.ECS.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace ME.ECS.Collections.LowLevel {
    
    public enum AllocatorType {

        Invalid = 0,
        Persistent,
        Temp,

    }

    public static class StaticAllocatorInitializer {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init() {

            if (StaticAllocators.persistent.Data.isValid) {
                StaticAllocators.persistent.Data.Dispose();
                Debug.LogWarning("StaticAllocatorInitializer: disposing old persistent allocator");
            }

            if (StaticAllocators.temp.Data.isValid) {
                StaticAllocators.temp.Data.Dispose();
                Debug.LogWarning("StaticAllocatorInitializer: disposing old temp allocator");
            }

            // 4 MB of persistent memory + no max size
            StaticAllocators.persistent.Data = new MemoryAllocator().Initialize(4 * 1024 * 1024);

            // 256 KB of temp memory + max size = 256 KB
            StaticAllocators.temp.Data = new MemoryAllocator().Initialize(256 * 1024);

        }

    }
    
    public struct StaticAllocators {

        internal static readonly Unity.Burst.SharedStatic<MemoryAllocator> persistent = Unity.Burst.SharedStatic<MemoryAllocator>.GetOrCreate<StaticAllocators, MemoryAllocator>();
        internal static readonly Unity.Burst.SharedStatic<MemoryAllocator> temp = Unity.Burst.SharedStatic<MemoryAllocator>.GetOrCreate<StaticAllocators, MemoryAllocator>();

        public static ref MemoryAllocator GetAllocator(AllocatorType type) {

            switch (type) {
                case AllocatorType.Persistent:
                    return ref StaticAllocators.persistent.Data;

                case AllocatorType.Temp:
                    return ref StaticAllocators.temp.Data;
            }

            throw new System.Exception($"Allocator type {type} is unknown");

        }
        
        internal static unsafe MemoryAllocator* GetAllocatorUnsafe(AllocatorType type) {

            switch (type) {
                case AllocatorType.Persistent:
                    return (MemoryAllocator*)StaticAllocators.persistent.UnsafeDataPointer;

                case AllocatorType.Temp:
                    return (MemoryAllocator*)StaticAllocators.temp.UnsafeDataPointer;
            }

            throw new System.Exception($"Allocator type {type} is unknown");

        }

    }

}