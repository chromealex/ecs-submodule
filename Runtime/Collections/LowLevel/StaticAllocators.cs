namespace ME.ECS.Collections.LowLevel {
    
    using Unsafe;
    
    public enum AllocatorType {

        Invalid = 0,
        Persistent,
        Temp,

    }

    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadAttribute]
    #endif
    public static class StaticAllocatorInitializer {

        private static readonly Destructor finalize = new Destructor();

        static StaticAllocatorInitializer() {

            // 4 MB of persistent memory + no max size
            StaticAllocators.persistent.Data = new MemoryAllocator().Initialize(4 * 1024 * 1024, -1);

            // 256 KB of temp memory + max size = 256 KB
            StaticAllocators.temp.Data = new MemoryAllocator().Initialize(256 * 1024, 256 * 1024);

        }

        private sealed class Destructor {

            ~Destructor() {
                StaticAllocators.persistent.Data.Dispose();
                StaticAllocators.temp.Data.Dispose();
            }

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