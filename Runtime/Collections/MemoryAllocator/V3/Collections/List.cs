namespace ME.ECS.Collections.MemoryAllocator {

    using ME.ECS.Collections.V3;
    using MemPtr = System.Int64;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(ListProxy<>))]
    public struct List<T>: System.Collections.Generic.IEnumerable<T> where T : unmanaged {

        private struct InternalData {

            public MemArrayAllocator<T> arr;
            public int count;

            public void Dispose(ref MemoryAllocator allocator) {
                
                this.arr.Dispose(ref allocator);
                this = default;

            }

        }
        
        public struct Enumerator : System.Collections.Generic.IEnumerator<T> {
            
            private readonly State state;
            private readonly List<T> list;
            private int index;

            internal Enumerator(State state, List<T> list) {
                this.state = state;
                this.list = list;
                this.index = -1;
            }

            public void Dispose() {
            }

            public bool MoveNext() {
                ++this.index;
                return this.index < this.list.GetCount(in this.state.allocator);
            }

            public ref T Current => ref this.list[in this.state.allocator, this.index];

            T System.Collections.Generic.IEnumerator<T>.Current => this.Current;

            object System.Collections.IEnumerator.Current => this.Current;

            void System.Collections.IEnumerator.Reset() {
                this.index = -1;
            }
            
        }

        public struct EnumeratorNoState : System.Collections.Generic.IEnumerator<T> {
            
            private readonly MemoryAllocator allocator;
            private readonly List<T> list;
            private int index;

            internal EnumeratorNoState(in MemoryAllocator allocator, List<T> list) {
                this.allocator = allocator;
                this.list = list;
                this.index = -1;
            }

            public void Dispose() {
            }

            public bool MoveNext() {
                ++this.index;
                return this.index < this.list.GetCount(in this.allocator);
            }

            public T Current => this.list[in this.allocator, this.index];

            object System.Collections.IEnumerator.Current => this.Current;

            void System.Collections.IEnumerator.Reset() {
                this.index = -1;
            }
            
        }

        [ME.ECS.Serializer.SerializeField]
        private readonly MemPtr ptr;
        
        private readonly ref MemArrayAllocator<T> GetArray(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).arr;
        private readonly ref int GetCount(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).count;

        public bool isCreated => this.ptr != 0;

        public readonly int Capacity(in MemoryAllocator allocator) {
            return this.GetArray(in allocator).Length(in allocator);
        }
        
        public readonly int Count(in MemoryAllocator allocator) {
            if (this.ptr == 0) throw new System.NullReferenceException();
            return this.GetCount(in allocator);
        }

        public List(ref MemoryAllocator allocator, int capacity) {

            this.ptr = allocator.AllocData<InternalData>(default);
            this.EnsureCapacity(ref allocator, capacity);

        }

        public readonly MemPtr GetMemPtr(in MemoryAllocator allocator) {
            
            return this.GetArray(in allocator).GetMemPtr(in allocator);
            
        }

        public readonly unsafe void* GetUnsafePtr(in MemoryAllocator allocator) {

            return this.GetArray(in allocator).GetUnsafePtr(in allocator);

        }

        public void Dispose(ref MemoryAllocator allocator) {

            allocator.Ref<InternalData>(this.ptr).Dispose(ref allocator);
            allocator.Free(this.ptr);
            this = default;
            
        }

        public readonly Enumerator GetEnumerator(State state) {
            
            return new Enumerator(state, this);
            
        }
        
        public readonly System.Collections.Generic.IEnumerator<T> GetEnumerator() {
            
            return GetEnumerator(Worlds.current.GetState());
            
        }

        readonly System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            
            return GetEnumerator();
            
        }

        public readonly EnumeratorNoState GetEnumerator(in MemoryAllocator allocator) {
            
            return new EnumeratorNoState(in allocator, this);
            
        }
        
        public void Clear(in MemoryAllocator allocator) {

            this.GetCount(in allocator) = 0;

        }
        
        public ref T this[in MemoryAllocator allocator, int index] {
            get {
                if (index < 0 || index >= this.GetCount(in allocator)) {
                    throw new System.IndexOutOfRangeException($"index {index} out of range 0..{this.GetCount(in allocator)}");
                }
                return ref this.GetArray(in allocator)[in allocator, index];
            }
        }

        public bool EnsureCapacity(ref MemoryAllocator allocator, int capacity) {

            capacity = Helpers.NextPot(capacity);
            return this.GetArray(in allocator).Resize(ref allocator, capacity, ClearOptions.UninitializedMemory);
            
        }
        
        public void Add(ref MemoryAllocator allocator, T obj) {

            ++this.GetCount(in allocator);
            this.EnsureCapacity(ref allocator, this.GetCount(in allocator));

            this.GetArray(in allocator)[in allocator, this.GetCount(in allocator) - 1] = obj;

        }

        public readonly bool Contains<U>(in MemoryAllocator allocator, U obj) where U : unmanaged, System.IEquatable<T> {
            
            for (int i = 0, cnt = this.GetCount(in allocator); i < cnt; ++i) {

                if (obj.Equals(this.GetArray(in allocator)[in allocator, i]) == true) {

                    return true;

                }
                
            }

            return false;

        }

        public bool Remove<U>(ref MemoryAllocator allocator, U obj) where U : unmanaged, System.IEquatable<T> {

            for (int i = 0, cnt = this.GetCount(in allocator); i < cnt; ++i) {

                if (obj.Equals(this.GetArray(in allocator)[in allocator, i]) == true) {

                    this.RemoveAt(ref allocator, i);
                    return true;

                }
                
            }

            return false;

        }

        public unsafe bool RemoveAt(ref MemoryAllocator allocator, int index) {
            
            if (index < 0 || index >= this.GetCount(in allocator)) return false;

            if (index == this.GetCount(in allocator) - 1) {

                --this.GetCount(in allocator);
                this.GetArray(in allocator)[in allocator, this.GetCount(in allocator)] = default;
                return true;

            }
            
            var ptr = this.GetArray(in allocator).GetMemPtr(in allocator);
            var size = sizeof(T);
            allocator.MemCopy(ptr, size * index, ptr, size * (index + 1), (this.GetCount(in allocator) - index - 1) * size);
            
            --this.GetCount(in allocator);
            this.GetArray(in allocator)[in allocator, this.GetCount(in allocator)] = default;
            
            return true;

        }

        public bool RemoveAtFast(ref MemoryAllocator allocator, int index) {
            
            if (index < 0 || index >= this.GetCount(in allocator)) return false;
            
            --this.GetCount(in allocator);
            var last = this.GetArray(in allocator)[in allocator, this.GetCount(in allocator)];
            this.GetArray(in allocator)[in allocator, index] = last;
            
            return true;

        }

        public bool Resize(ref MemoryAllocator allocator, int newLength, ClearOptions options = ClearOptions.ClearMemory) {

            if (this.isCreated == false) {
                
                this = new List<T>(ref allocator, newLength);
                return true;

            }
            
            if (newLength <= this.GetCount(in allocator)) {

                return false;
                
            }

            this.GetArray(in allocator).Resize(ref allocator, newLength, options);
            this.GetCount(in allocator) = newLength;
            return true;

        }

        public void AddRange(ref MemoryAllocator allocator, ListCopyable<T> list) {

            foreach (var item in list) {
                
                this.Add(ref allocator, item);
                
            }
            
        }

        public unsafe void AddRange(ref MemoryAllocator allocator, List<T> collection) {

            var index = this.GetCount(in allocator);
            if (collection.isCreated == false)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
            if ((uint) index > (uint) this.GetCount(in allocator))
                throw new System.IndexOutOfRangeException();
            int count = collection.GetCount(in allocator);
            if (count > 0) {
                this.EnsureCapacity(ref allocator, this.GetCount(in allocator) + count);
                var size = sizeof(T);
                if (index < this.GetCount(in allocator)) {
                    allocator.MemCopy(this.GetArray(in allocator).GetMemPtr(in allocator), (index + count) * size, this.GetArray(in allocator).GetMemPtr(in allocator), index * size, (this.GetCount(in allocator) - index) * size);
                }

                if (this.GetArray(in allocator).GetMemPtr(in allocator) == collection.GetArray(in allocator).GetMemPtr(in allocator)) {
                    allocator.MemCopy(this.GetArray(in allocator).GetMemPtr(in allocator), index * size, this.GetArray(in allocator).GetMemPtr(in allocator), 0, index * size);
                    allocator.MemCopy(this.GetArray(in allocator).GetMemPtr(in allocator), (index * 2) * size, this.GetArray(in allocator).GetMemPtr(in allocator), (index + count) * size, (this.GetCount(in allocator) - index) * size);
                } else {
                    collection.CopyTo(ref allocator, this.GetArray(in allocator), index);
                }

                this.GetCount(in allocator) += count;
            }
            
        }

        public unsafe void CopyTo(ref MemoryAllocator allocator, MemArrayAllocator<T> arr, int index) {
            
            if (arr.isCreated == false) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
            }

            var size = sizeof(T);
            allocator.MemCopy(arr.GetMemPtr(in allocator), index * size, this.GetArray(in allocator).GetMemPtr(in allocator), 0, this.GetCount(in allocator) * size);
            
        }
        
    }

}