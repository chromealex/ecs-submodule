namespace ME.ECS.Collections.MemoryAllocator {

    using ME.ECS.Collections.V3;
    using MemPtr = System.Int64;
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(ListProxy<>))]
    public struct List<T> where T : unmanaged {

        public struct InternalData {

            public MemArrayAllocator<T> arr;
            public int count;

            [INLINE(256)]
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
        
        [INLINE(256)]
        private readonly ref MemArrayAllocator<T> GetArray(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).arr;

        [INLINE(256)]
        private readonly ref int GetCount(in MemoryAllocator allocator) {
            return ref allocator.Ref<InternalData>(this.ptr).count;
        }

        public bool isCreated {
            [INLINE(256)]
            get => this.ptr != 0;
        }

        [INLINE(256)]
        public readonly int Capacity(in MemoryAllocator allocator) {
            return this.GetArray(in allocator).Length;
        }
        
        [INLINE(256)]
        public readonly int Count(in MemoryAllocator allocator) {
            return this.GetCount(in allocator);
        }

        [INLINE(256)]
        public List(ref MemoryAllocator allocator, int capacity) {

            this.ptr = allocator.AllocData<InternalData>(default);
            this.EnsureCapacity(ref allocator, capacity);

        }

        [INLINE(256)]
        public readonly MemPtr GetMemPtr(in MemoryAllocator allocator) {
            
            return this.GetArray(in allocator).arrPtr;
            
        }

        [INLINE(256)]
        public readonly unsafe void* GetUnsafePtr(in MemoryAllocator allocator) {

            return this.GetArray(in allocator).GetUnsafePtr(in allocator);

        }

        [INLINE(256)]
        public void Dispose(ref MemoryAllocator allocator) {

            allocator.Ref<InternalData>(this.ptr).Dispose(ref allocator);
            allocator.Free(this.ptr);
            this = default;
            
        }

        [INLINE(256)]
        public readonly Enumerator GetEnumerator(State state) {
            
            return new Enumerator(state, this);
            
        }
        
        [INLINE(256)]
        public readonly Enumerator GetEnumerator() {
            
            return this.GetEnumerator(Worlds.current.GetState());
            
        }

        [INLINE(256)]
        public readonly EnumeratorNoState GetEnumerator(in MemoryAllocator allocator) {
            
            return new EnumeratorNoState(in allocator, this);
            
        }
        
        [INLINE(256)]
        public void Clear(in MemoryAllocator allocator) {

            this.GetCount(in allocator) = 0;

        }

        [INLINE(256)]
        public ref InternalData GetInternalData(in MemoryAllocator allocator) {
            return ref allocator.Ref<InternalData>(this.ptr);
        }
        
        public ref T this[in MemoryAllocator allocator, int index] {
            [INLINE(256)]
            get {
                E.RANGE(index, 0, this.GetCount(in allocator));
                return ref this.GetArray(in allocator)[in allocator, index];
            }
        }

        public ref T this[in InternalData internalData, in MemoryAllocator allocator, int index] {
            [INLINE(256)]
            get {
                return ref internalData.arr[in allocator, index];
            }
        }

        [INLINE(256)]
        public bool EnsureCapacity(ref MemoryAllocator allocator, int capacity) {

            capacity = Helpers.NextPot(capacity);
            return this.GetArray(in allocator).Resize(ref allocator, capacity, ClearOptions.UninitializedMemory);
            
        }
        
        [INLINE(256)]
        public void Add(ref MemoryAllocator allocator, T obj) {

            ++this.GetCount(in allocator);
            this.EnsureCapacity(ref allocator, this.GetCount(in allocator));

            this.GetArray(in allocator)[in allocator, this.GetCount(in allocator) - 1] = obj;

        }

        [INLINE(256)]
        public readonly bool Contains<U>(in MemoryAllocator allocator, U obj) where U : unmanaged, System.IEquatable<T> {
            
            for (int i = 0, cnt = this.GetCount(in allocator); i < cnt; ++i) {

                if (obj.Equals(this.GetArray(in allocator)[in allocator, i]) == true) {

                    return true;

                }
                
            }

            return false;

        }

        [INLINE(256)]
        public bool Remove<U>(ref MemoryAllocator allocator, U obj) where U : unmanaged, System.IEquatable<T> {

            for (int i = 0, cnt = this.GetCount(in allocator); i < cnt; ++i) {

                if (obj.Equals(this.GetArray(in allocator)[in allocator, i]) == true) {

                    this.RemoveAt(ref allocator, i);
                    return true;

                }
                
            }

            return false;

        }

        [INLINE(256)]
        public unsafe bool RemoveAt(ref MemoryAllocator allocator, int index) {
            
            if (index < 0 || index >= this.GetCount(in allocator)) return false;

            if (index == this.GetCount(in allocator) - 1) {

                --this.GetCount(in allocator);
                this.GetArray(in allocator)[in allocator, this.GetCount(in allocator)] = default;
                return true;

            }
            
            var ptr = this.GetArray(in allocator).arrPtr;
            var size = sizeof(T);
            allocator.MemCopy(ptr, size * index, ptr, size * (index + 1), (this.GetCount(in allocator) - index - 1) * size);
            
            --this.GetCount(in allocator);
            this.GetArray(in allocator)[in allocator, this.GetCount(in allocator)] = default;
            
            return true;

        }

        [INLINE(256)]
        public bool RemoveAtFast(ref MemoryAllocator allocator, int index) {
            
            if (index < 0 || index >= this.GetCount(in allocator)) return false;
            
            --this.GetCount(in allocator);
            var last = this.GetArray(in allocator)[in allocator, this.GetCount(in allocator)];
            this.GetArray(in allocator)[in allocator, index] = last;
            
            return true;

        }

        [INLINE(256)]
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

        [INLINE(256)]
        public void AddRange(ref MemoryAllocator allocator, ListCopyable<T> list) {

            foreach (var item in list) {
                
                this.Add(ref allocator, item);
                
            }
            
        }

        [INLINE(256)]
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
                    allocator.MemCopy(this.GetArray(in allocator).arrPtr, (index + count) * size, this.GetArray(in allocator).arrPtr, index * size, (this.GetCount(in allocator) - index) * size);
                }

                if (this.GetArray(in allocator).arrPtr == collection.GetArray(in allocator).arrPtr) {
                    allocator.MemCopy(this.GetArray(in allocator).arrPtr, index * size, this.GetArray(in allocator).arrPtr, 0, index * size);
                    allocator.MemCopy(this.GetArray(in allocator).arrPtr, (index * 2) * size, this.GetArray(in allocator).arrPtr, (index + count) * size, (this.GetCount(in allocator) - index) * size);
                } else {
                    collection.CopyTo(ref allocator, this.GetArray(in allocator), index);
                }

                this.GetCount(in allocator) += count;
            }
            
        }

        [INLINE(256)]
        public unsafe void CopyTo(ref MemoryAllocator allocator, MemArrayAllocator<T> arr, int index) {
            
            if (arr.isCreated == false) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
            }

            var size = sizeof(T);
            allocator.MemCopy(arr.arrPtr, index * size, this.GetArray(in allocator).arrPtr, 0, this.GetCount(in allocator) * size);
            
        }
        
    }

}