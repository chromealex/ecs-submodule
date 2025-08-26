using Unity.Collections.LowLevel.Unsafe;

namespace ME.ECS.Collections.LowLevel {

    using ME.ECS.Collections.LowLevel.Unsafe;
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(ME.ECS.DebugUtils.ListProxyDebugger<>))]
    public struct List<T> : IIsCreated where T : unmanaged {

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
                return this.index < this.list.Count;
            }

            public ref T Current => ref this.list[in this.state.allocator, this.index];

            T System.Collections.Generic.IEnumerator<T>.Current => this.Current;

            object System.Collections.IEnumerator.Current => this.Current;

            void System.Collections.IEnumerator.Reset() {
                this.index = -1;
            }
            
        }

        public struct EnumeratorNoState : System.Collections.Generic.IEnumerator<T> {
            
            private MemoryAllocator allocator;
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
                return this.index < this.list.Count;
            }

            public T Current => this.list[in this.allocator, this.index];

            object System.Collections.IEnumerator.Current => this.Current;

            public void SetAllocator(in MemoryAllocator allocator) => this.allocator = allocator;

            void System.Collections.IEnumerator.Reset() {
                this.index = -1;
            }
            
        }

        [ME.ECS.Serializer.SerializeField]
        private MemArrayAllocator<T> arr;
        [ME.ECS.Serializer.SerializeField]
        public int Count;

        public readonly bool isCreated {
            [INLINE(256)]
            get => this.arr.isCreated;
        }

        [INLINE(256)]
        public readonly int Capacity(in MemoryAllocator allocator) {
            ECS.E.IS_CREATED(this);
            return this.arr.Length;
        }
        
        [INLINE(256)]
        public List(ref MemoryAllocator allocator, int capacity) {

            if (capacity <= 0) capacity = 1;
            this.arr = new MemArrayAllocator<T>(ref allocator, capacity);
            this.Count = 0;
            this.EnsureCapacity(ref allocator, capacity);

        }

        [INLINE(256)]
        public void ReplaceWith(ref MemoryAllocator allocator, in List<T> other) {
            
            if (other.arr.arrPtr == this.arr.arrPtr) {
                return;
            }
            
            this.Dispose(ref allocator);
            this = other;
            
        }

        [INLINE(256)]
        public void CopyFrom(ref MemoryAllocator allocator, in List<T> other) {

            if (other.arr.arrPtr == this.arr.arrPtr) return;
            if (this.arr.arrPtr == MemPtr.Invalid && other.arr.arrPtr == MemPtr.Invalid) return;
            if (this.arr.arrPtr != MemPtr.Invalid && other.arr.arrPtr == MemPtr.Invalid) {
                this.Dispose(ref allocator);
                return;
            }
            if (this.arr.arrPtr == MemPtr.Invalid) this = new List<T>(ref allocator, other.Capacity(in allocator));
            
            NativeArrayUtils.Copy(ref allocator, in other.arr, ref this.arr);
            this.Count = other.Count;

        }

        [INLINE(256)]
        public readonly MemPtr GetMemPtr(in MemoryAllocator allocator) {
            
            ECS.E.IS_CREATED(this);
            return this.arr.arrPtr;
            
        }

        [INLINE(256)]
        public readonly unsafe void* GetUnsafePtr(in MemoryAllocator allocator) {

            ECS.E.IS_CREATED(this);
            return this.arr.GetUnsafePtr(in allocator);

        }

        [INLINE(256)]
        public void Dispose(ref MemoryAllocator allocator) {

            ECS.E.IS_CREATED(this);
            this.arr.Dispose(ref allocator);
            this = default;
            
        }

        [INLINE(256)]
        public readonly Enumerator GetEnumerator(State state) {
            
            ECS.E.IS_CREATED(this);
            return new Enumerator(state, this);
            
        }
        
        [INLINE(256)]
        public readonly Enumerator GetEnumerator() {
            
            ECS.E.IS_CREATED(this);
            return this.GetEnumerator(Worlds.current.GetState());
            
        }

        [INLINE(256)]
        public readonly EnumeratorNoState GetEnumerator(in MemoryAllocator allocator) {
            
            ECS.E.IS_CREATED(this);
            return new EnumeratorNoState(in allocator, this);
            
        }
        
        [INLINE(256)]
        public void Clear(in MemoryAllocator allocator) {

            ECS.E.IS_CREATED(this);
            this.Count = 0;

        }

        public readonly ref T this[in MemoryAllocator allocator, int index] {
            [INLINE(256)]
            get {
                ECS.E.IS_CREATED(this);
                E.RANGE(index, 0, this.Count);
                return ref this.arr[in allocator, index];
            }
        }

        [INLINE(256)]
        public bool EnsureCapacity(ref MemoryAllocator allocator, int capacity) {

            ECS.E.IS_CREATED(this);
            capacity = Helpers.NextPot(capacity);
            return this.arr.Resize(ref allocator, capacity, ClearOptions.ClearMemory);
            
        }
        
        [INLINE(256)]
        public void Add(ref MemoryAllocator allocator, T obj) {

            ECS.E.IS_CREATED(this);
            ++this.Count;
            this.EnsureCapacity(ref allocator, this.Count);

            this.arr[in allocator, this.Count - 1] = obj;

        }

        [INLINE(256)]
        public readonly bool Contains<U>(in MemoryAllocator allocator, U obj) where U : unmanaged, System.IEquatable<T> {
            
            ECS.E.IS_CREATED(this);
            for (int i = 0, cnt = this.Count; i < cnt; ++i) {

                if (obj.Equals(this.arr[in allocator, i]) == true) {

                    return true;

                }
                
            }

            return false;

        }

        [INLINE(256)]
        public bool Remove<U>(ref MemoryAllocator allocator, U obj) where U : unmanaged, System.IEquatable<T> {

            ECS.E.IS_CREATED(this);
            for (int i = 0, cnt = this.Count; i < cnt; ++i) {

                if (obj.Equals(this.arr[in allocator, i]) == true) {

                    this.RemoveAt(ref allocator, i);
                    return true;

                }
                
            }

            return false;

        }

        [INLINE(256)]
        public bool RemoveFast<U>(ref MemoryAllocator allocator, U obj) where U : unmanaged, System.IEquatable<T> {

            ECS.E.IS_CREATED(this);
            for (int i = 0, cnt = this.Count; i < cnt; ++i) {

                if (obj.Equals(this.arr[in allocator, i]) == true) {

                    this.RemoveAtFast(ref allocator, i);
                    return true;

                }

            }

            return false;

        }

        [INLINE(256)]
        public unsafe bool RemoveAt(ref MemoryAllocator allocator, int index) {
            
            ECS.E.IS_CREATED(this);
            if (index < 0 || index >= this.Count) return false;

            if (index == this.Count - 1) {

                --this.Count;
                this.arr[in allocator, this.Count] = default;
                return true;

            }
            
            var ptr = this.arr.arrPtr;
            var size = sizeof(T);
            allocator.MemMove(ptr, size * index, ptr, size * (index + 1), (this.Count - index - 1) * size);
            
            --this.Count;
            this.arr[in allocator, this.Count] = default;
            
            return true;

        }

        [INLINE(256)]
        public bool RemoveAtFast(ref MemoryAllocator allocator, int index) {
            
            ECS.E.IS_CREATED(this);
            if (index < 0 || index >= this.Count) return false;
            
            --this.Count;
            var last = this.arr[in allocator, this.Count];
            this.arr[in allocator, index] = last;
            
            return true;

        }

        [INLINE(256)]
        public bool Resize(ref MemoryAllocator allocator, int newLength, ClearOptions options = ClearOptions.ClearMemory) {

            ECS.E.IS_CREATED(this);
            if (this.isCreated == false) {
                
                this = new List<T>(ref allocator, newLength);
                return true;

            }
            
            if (newLength <= this.Count) {

                return false;
                
            }

            this.arr.Resize(ref allocator, newLength, options);
            this.Count = newLength;
            return true;

        }

        [INLINE(256)]
        public void AddRange(ref MemoryAllocator allocator, ListCopyable<T> list) {

            ECS.E.IS_CREATED(this);
            foreach (var item in list) {
                
                this.Add(ref allocator, item);
                
            }
            
        }

        [INLINE(256)]
        public unsafe void AddRange(ref MemoryAllocator allocator, Unity.Collections.NativeArray<T> collection) {

            ECS.E.IS_CREATED(this);
            var index = this.Count;
            if (collection.IsCreated == false)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
            if ((uint) index > (uint)this.Count)
                throw new System.IndexOutOfRangeException();
            int count = collection.Length;
            if (count > 0) {
                this.EnsureCapacity(ref allocator, this.Count + count);
                var size = sizeof(T);
                if (index < this.Count) {
                    allocator.MemMove(this.arr.arrPtr, (index + count) * size, this.arr.arrPtr, index * size, (this.Count - index) * size);
                }

                Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemMove(allocator.GetUnsafePtr(this.arr.arrPtr).ptr, collection.GetUnsafePtr(), this.Count * size);
                
                this.Count += count;
            }
            
        }

        [INLINE(256)]
        public unsafe void AddRange(ref MemoryAllocator allocator, List<T> collection) {

            ECS.E.IS_CREATED(this);
            var index = this.Count;
            if (collection.isCreated == false)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
            if ((uint) index > (uint)this.Count)
                throw new System.IndexOutOfRangeException();
            int count = collection.Count;
            if (count > 0) {
                this.EnsureCapacity(ref allocator, this.Count + count);
                var size = sizeof(T);
                if (index < this.Count) {
                    allocator.MemMove(this.arr.arrPtr, (index + count) * size, this.arr.arrPtr, index * size, (this.Count - index) * size);
                }

                if (this.arr.arrPtr == collection.arr.arrPtr) {
                    allocator.MemMove(this.arr.arrPtr, index * size, this.arr.arrPtr, 0, index * size);
                    allocator.MemMove(this.arr.arrPtr, (index * 2) * size, this.arr.arrPtr, (index + count) * size, (this.Count - index) * size);
                } else {
                    collection.CopyTo(ref allocator, this.arr, index);
                }

                this.Count += count;
            }
            
        }

        [INLINE(256)]
        public unsafe void AddRange(ref MemoryAllocator allocator, MemArrayAllocator<T> collection) {

            ECS.E.IS_CREATED(this);
            var index = this.Count;
            if (collection.isCreated == false)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
            if ((uint) index > (uint)this.Count)
                throw new System.IndexOutOfRangeException();
            int count = collection.Length;
            if (count > 0) {
                this.EnsureCapacity(ref allocator, this.Count + count);
                var size = sizeof(T);
                if (index < this.Count) {
                    allocator.MemMove(this.arr.arrPtr, (index + count) * size, this.arr.arrPtr, index * size, (this.Count - index) * size);
                }

                if (this.arr.arrPtr == collection.arrPtr) {
                    allocator.MemMove(this.arr.arrPtr, index * size, this.arr.arrPtr, 0, index * size);
                    allocator.MemMove(this.arr.arrPtr, (index * 2) * size, this.arr.arrPtr, (index + count) * size, (this.Count - index) * size);
                } else {
                    CopyFrom(ref allocator, collection, index);
                }

                this.Count += count;
            }
        }

        [INLINE(256)]
        public readonly unsafe void CopyTo(ref MemoryAllocator allocator, MemArrayAllocator<T> arr, int index) {
            
            ECS.E.IS_CREATED(this);
            if (arr.isCreated == false) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
            }

            var size = sizeof(T);
            allocator.MemMove(arr.arrPtr, index * size, this.arr.arrPtr, 0, this.Count * size);
            
        }

        [INLINE(256)]
        public readonly unsafe void CopyFrom(ref MemoryAllocator allocator, MemArrayAllocator<T> arr, int index) {

            ECS.E.IS_CREATED(this);
            if (arr.isCreated == false) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
            }

            var size = sizeof(T);
            allocator.MemMove(this.arr.arrPtr, index * size, arr.arrPtr, 0, arr.Length * size);

        }
        
    }

}