namespace ME.ECS.Collections.V2 {

    using Unity.Collections.LowLevel.Unsafe;
    using ptr = System.Int64;
    using MemPtr = System.Int64;
    
    public unsafe partial struct MemoryAllocator {

        //
        // Arrays
        //
        
        public readonly ref T RefArray<T>(MemPtr ptr, int index) where T : unmanaged {
            var size = (ptr)sizeof(T);
            return ref this.GetBlockData<T>((ptr)ptr, index * size);
        }

        public MemPtr ReAllocArray<T>(MemPtr ptr, int newLength, ClearOptions options) where T : unmanaged {
            var size = (ptr)sizeof(T);
            return this.ReAlloc(ptr, size * newLength, options);
        }

        public MemPtr AllocArray<T>(int length) where T : unmanaged {
            var size = (ptr)sizeof(T);
            return this.Alloc(size * length);
        }

        public readonly ref T RefArrayUnmanaged<T>(MemPtr ptr, int index) where T : struct {
            var size = (ptr)UnsafeUtility.SizeOf<T>();
            return ref this.GetBlockData<T>((ptr)ptr, index * size);
        }

        public MemPtr ReAllocArrayUnmanaged<T>(MemPtr ptr, int newLength, ClearOptions options) where T : struct {
            var size = (ptr)UnsafeUtility.SizeOf<T>();
            return this.ReAlloc(ptr, size * newLength, options);
        }

        public MemPtr AllocArrayUnmanaged<T>(int length) where T : struct {
            var size = (ptr)UnsafeUtility.SizeOf<T>();
            return this.Alloc(size * length);
        }

    }

}