namespace ME.ECS.Collections.MemoryAllocator {

    using ME.ECS.Collections.V3;
    
    public struct List<T> where T : unmanaged {

        public struct Enumerator : System.Collections.Generic.IEnumerator<T> {
            
            private readonly MemoryAllocator allocator;
            private readonly List<T> list;
            private int index;

            internal Enumerator(in MemoryAllocator allocator, List<T> list) {
                this.allocator = allocator;
                this.list = list;
                this.index = -1;
            }

            public void Dispose() {
            }

            public bool MoveNext() {
                ++this.index;
                return this.index < this.list.count;
            }

            public T Current => this.list[in this.allocator, this.index];

            object System.Collections.IEnumerator.Current => this.Current;

            void System.Collections.IEnumerator.Reset() {
                this.index = -1;
            }
            
        }
        
        [ME.ECS.Serializer.SerializeField]
        private MemArrayAllocator<T> arr;
        [ME.ECS.Serializer.SerializeField]
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

        public readonly Enumerator GetEnumerator(in MemoryAllocator allocator) {
            
            return new Enumerator(in allocator, this);
            
        }
        
        public void Clear(in MemoryAllocator allocator) {

            this.arr.Clear(in allocator);
            this.count = 0;

        }
        
        public ref T this[in MemoryAllocator allocator, int index] {
            get {
                if (index < 0 || index >= this.count) {
                    throw new System.IndexOutOfRangeException();
                }
                return ref this.arr[in allocator, index];
            }
        }

        public bool EnsureCapacity(ref MemoryAllocator allocator, int capacity) {

            capacity = Helpers.NextPot(capacity);
            return this.arr.Resize(ref allocator, capacity, ClearOptions.UninitializedMemory);
            
        }
        
        public void Add(ref MemoryAllocator allocator, T obj) {

            ++this.count;
            this.EnsureCapacity(ref allocator, this.count);

            this.arr[in allocator, this.count - 1] = obj;

        }

        public readonly bool Contains<U>(in MemoryAllocator allocator, U obj) where U : unmanaged, System.IEquatable<U> {
            
            for (int i = 0; i < this.count; ++i) {

                var asU = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.As<T, U>(ref this.arr[in allocator, i]);
                if (asU.Equals(obj) == true) {

                    return true;

                }
                
            }

            return false;

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
            var last = this.arr[in allocator, this.count];
            this.arr[in allocator, index] = last;
            
            return true;

        }

        public bool Resize(ref MemoryAllocator allocator, int newLength, ClearOptions options = ClearOptions.ClearMemory) {

            if (this.isCreated == false) {
                
                this = new List<T>(ref allocator, newLength);
                
            }
            
            if (newLength <= this.Count) {

                return false;
                
            }

            this.arr.Resize(ref allocator, newLength, options);
            this.count = newLength;
            return true;

        }

        public unsafe void AddRange(ref MemoryAllocator allocator, List<T> collection) {

            var index = this.count;
            if (collection.isCreated == false)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
            if ((uint) index > (uint) this.count)
                throw new System.IndexOutOfRangeException();
            int count = collection.Count;
            if (count > 0) {
                this.EnsureCapacity(ref allocator, this.count + count);
                var size = sizeof(T);
                if (index < this.count) {
                    allocator.MemCopy(this.arr.GetMemPtr(), (index + count) * size, this.arr.GetMemPtr(), index * size, (this.count - index) * size);
                }

                if (this.arr.GetMemPtr() == collection.arr.GetMemPtr()) {
                    allocator.MemCopy(this.arr.GetMemPtr(), index * size, this.arr.GetMemPtr(), 0, index * size);
                    allocator.MemCopy(this.arr.GetMemPtr(), (index * 2) * size, this.arr.GetMemPtr(), (index + count) * size, (this.count - index) * size);
                } else {
                    collection.CopyTo(ref allocator, this.arr, index);
                }

                this.count += count;
            }
            
        }

        public unsafe void CopyTo(ref MemoryAllocator allocator, MemArrayAllocator<T> arr, int index) {
            
            if (arr.isCreated == false) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
            }

            var size = sizeof(T);
            allocator.MemCopy(arr.GetMemPtr(), index * size, this.arr.GetMemPtr(), 0, this.count * size);
            
        }

    }

}