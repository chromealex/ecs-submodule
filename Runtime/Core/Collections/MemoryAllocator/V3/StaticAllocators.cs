namespace ME.ECS.Collections.V3 {

    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadAttribute]
    #endif
    public static class StaticAllocators {

        private static readonly Destructor finalize = new Destructor();

        private static MemoryAllocator persistent;
        private static MemoryAllocator temp;

        public static ref MemoryAllocator GetAllocator(AllocatorType type) {

            switch (type) {
                case AllocatorType.Persistent:
                    return ref StaticAllocators.persistent;

                case AllocatorType.Temp:
                    return ref StaticAllocators.temp;
            }

            throw new System.Exception($"Allocator type {type} is unknown");

        }

        static StaticAllocators() {

            // 4 MB of persistent memory + no max size
            StaticAllocators.persistent = new MemoryAllocator().Initialize(4 * 1024 * 1024, -1);

            // 256 KB of temp memory + max size = 256 KB
            StaticAllocators.temp = new MemoryAllocator().Initialize(256 * 1024, 256 * 1024);

        }

        private sealed class Destructor {

            ~Destructor() {
                StaticAllocators.persistent.Dispose();
                StaticAllocators.temp.Dispose();
            }

        }

    }

}