namespace ME.ECS.Collections.MemoryAllocator {

    using ME.ECS.Collections.V3;
    
    public struct List<T> where T : unmanaged {

        public MemArrayAllocator<T> arr;
        private int count;

        public bool isCreated => this.arr.isCreated;
        public int Count => this.count;

        public List(ref MemoryAllocator allocator, int capacity) {

            this = default;
            this.EnsureCapacity(ref allocator, capacity);

        }

        public void Dispose(ref MemoryAllocator allocator) {
            
            this.arr.Dispose(ref allocator);
            this.count = default;

        }
        
        public ref T this[in MemoryAllocator allocator, int index] {
            get {
                if (index < 0 || index >= this.count) {
                    throw new System.IndexOutOfRangeException();
                }
                return ref this.arr[in allocator, index];
            }
        }

        private void EnsureCapacity(ref MemoryAllocator allocator, int capacity) {

            this.arr.Resize(ref allocator, capacity, ClearOptions.UninitializedMemory);
            
        }
        
        public void Add(ref MemoryAllocator allocator, T obj) {

            ++this.count;
            this.EnsureCapacity(ref allocator, this.count);

            this.arr[in allocator, this.count - 1] = obj;

        }

        public bool Remove<U>(ref MemoryAllocator allocator, U obj) where U : unmanaged, System.IEquatable<U> {

            for (int i = 0; i < this.count; ++i) {

                var asU = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.As<T, U>(ref this.arr[in allocator, i]);
                if (asU.Equals(obj) == true) {

                    this.RemoveAt(ref allocator, i);
                    return true;

                }
                
            }

            return false;

        }

        public unsafe bool RemoveAt(ref MemoryAllocator allocator, int index) {
            
            if (index < 0 || index >= this.count) return false;

            if (index == this.count - 1) {

                --this.count;
                this.arr[in allocator, this.count] = default;
                return true;

            }
            
            var ptr = this.arr.GetMemPtr();
            var size = sizeof(T);
            allocator.MemCopy(ptr, size * index, ptr, size * (index + 1), (this.count - index - 1) * size);
            
            --this.count;
            this.arr[in allocator, this.count] = default;
            
            return true;

        }

        public bool RemoveAtFast(ref MemoryAllocator allocator, int index) {
            
            if (index < 0 || index >= this.count) return false;
            
            --this.count;
            var last = this.arr[in allocator, this.arr.Length - 1];
            this.arr[in allocator, index] = last;
            
            return true;

        }

    }

}