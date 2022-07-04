namespace ME.ECS.Collections.V2 {

    using word_t = System.UIntPtr;
    using size_t = System.Int64;
    using ptr = System.Int64;
    using MemPtr = System.Int64;

    public struct MemArray<T> where T : struct {

        private MemPtr ptr;
        public int Length;
        private readonly AllocatorType allocator;
        public bool isCreated => this.ptr != 0L;
        
        public MemArray(int length, AllocatorType allocator) {

            this.allocator = allocator;
            var memoryAllocator = StaticAllocators.GetAllocator(allocator);
            this.ptr = memoryAllocator.AllocArrayUnmanaged<T>(length);
            this.Length = length;

        }

        public void Dispose() {

            var memoryAllocator = StaticAllocators.GetAllocator(this.allocator);
            memoryAllocator.Free(this.ptr);
            this = default;

        }

        public ref T this[int index] => ref StaticAllocators.GetAllocator(this.allocator).RefArrayUnmanaged<T>(this.ptr, index);

        public bool Resize(int newLength) {

            if (newLength <= this.Length) {

                return false;
                
            }
            
            var memoryAllocator = StaticAllocators.GetAllocator(this.allocator);
            this.ptr = memoryAllocator.ReAllocArrayUnmanaged<T>(this.ptr, newLength, ClearOptions.ClearMemory);
            this.Length = newLength;
            return true;

        }
        
    }

    public class MemArrayAllocatorProxy<T> where T : struct {

        private MemArrayAllocator<T> arr;
        private MemoryAllocator allocator;
        
        public MemArrayAllocatorProxy(ref MemoryAllocator allocator, MemArrayAllocator<T> arr) {

            this.arr = arr;
            this.allocator = allocator;

        }

        public T[] items {
            get {
                var arr = new T[this.arr.Length];
                for (int i = 0; i < this.arr.Length; ++i) {
                    arr[i] = this.arr[in this.allocator, i];
                }

                return arr;
            }
        }

    }

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(MemArrayAllocatorProxy<>))]
    public struct MemArrayAllocator<T> where T : struct {

        private MemPtr ptr;
        public int Length;
        public bool isCreated => this.ptr != 0L;

        public MemArrayAllocator(ref MemoryAllocator allocator, int length) {

            this.ptr = allocator.AllocArrayUnmanaged<T>(length);
            this.Length = length;

        }

        public void Dispose(ref MemoryAllocator allocator) {

            allocator.Free(this.ptr);
            this = default;

        }

        public MemPtr GetMemPtr() {
            return this.ptr;
        }

        public unsafe void* GetUnsafePtr(ref MemoryAllocator allocator) {

            return allocator.GetUnsafePtr(this.ptr);

        }

        public ref T this[in MemoryAllocator allocator, int index] {
            get => ref allocator.RefArrayUnmanaged<T>(this.ptr, index); 
        }

        public bool Resize(ref MemoryAllocator allocator, int newLength, ClearOptions options = ClearOptions.ClearMemory) {

            if (newLength <= this.Length) {

                return false;
                
            }

            this.ptr = allocator.ReAllocArrayUnmanaged<T>(this.ptr, newLength, options);
            this.Length = newLength;
            return true;

        }

        public void Clear(ref MemoryAllocator allocator) {

            this.Clear(ref allocator, 0, this.Length);

        }

        public void Clear(ref MemoryAllocator allocator, int index, int length) {

            var size = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>();
            allocator.MemClear(this.ptr, index * size, length * size);
            
        }

    }

}