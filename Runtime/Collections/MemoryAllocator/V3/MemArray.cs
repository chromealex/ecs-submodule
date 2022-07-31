namespace ME.ECS.Collections.V3 {
    
    using MemPtr = System.Int64;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(MemArrayAllocatorProxy<>))]
    public struct MemArrayAllocator<T>: System.Collections.Generic.IEnumerable<T> where T : struct {

        public struct Enumerator : System.Collections.Generic.IEnumerator<T> {
            
            private readonly State state;
            private readonly MemArrayAllocator<T> list;
            private int index;

            internal Enumerator(State state, MemArrayAllocator<T> list) {
                this.state = state;
                this.list = list;
                this.index = -1;
            }

            public void Dispose() {
            }

            public bool MoveNext() {
                ++this.index;
                return this.index < this.list.Length(in this.state.allocator);
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
            private readonly MemArrayAllocator<T> list;
            private int index;

            internal EnumeratorNoState(in MemoryAllocator allocator, MemArrayAllocator<T> list) {
                this.allocator = allocator;
                this.list = list;
                this.index = -1;
            }

            public void Dispose() {
            }

            public bool MoveNext() {
                ++this.index;
                return this.index < this.list.Length(in this.allocator);
            }

            public T Current => this.list[in this.allocator, this.index];

            object System.Collections.IEnumerator.Current => this.Current;

            void System.Collections.IEnumerator.Reset() {
                this.index = -1;
            }
            
        }

        private struct InternalData {

            public int length;
            public int growFactor;
            public MemPtr arr;

            public void Dispose(ref MemoryAllocator allocator) {

                allocator.Free(this.arr);

            }
            
        }
        
        [ME.ECS.Serializer.SerializeField]
        private readonly MemPtr ptr;

        private readonly ref MemPtr arrPtr(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).arr;
        private readonly ref int size(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).length;
        private readonly ref int growFactor(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).growFactor;
        
        public readonly int Length(in MemoryAllocator allocator) => this.size(in allocator);
        public readonly bool isCreated => this.ptr != 0L;

        public MemArrayAllocator(ref MemoryAllocator allocator, int length, ClearOptions clearOptions = ClearOptions.ClearMemory, int growFactor = 1) {

            this.ptr = allocator.AllocData<InternalData>(default);
            this.arrPtr(in allocator) = length > 0 ? allocator.AllocArrayUnmanaged<T>(length) : 0;
            this.growFactor(in allocator) = growFactor;
            this.size(in allocator) = length;

            if (clearOptions == ClearOptions.ClearMemory) {
                this.Clear(in allocator);
            }

        }

        public MemArrayAllocator(ref MemoryAllocator allocator, BufferArray<T> arr) {

            this = new MemArrayAllocator<T>(ref allocator, arr.Length, ClearOptions.UninitializedMemory);
            NativeArrayUtils.Copy(in allocator, (T[])arr.arr, 0, ref this, 0, arr.Length);
            
        }

        public MemArrayAllocator(ref MemoryAllocator allocator, ListCopyable<T> arr) {

            this = new MemArrayAllocator<T>(ref allocator, arr.Count, ClearOptions.UninitializedMemory);
            NativeArrayUtils.Copy(in allocator, arr.innerArray, 0, ref this, 0, arr.Count);

        }

        public MemArrayAllocator(ref MemoryAllocator allocator, T[] arr) {

            this = new MemArrayAllocator<T>(ref allocator, arr.Length, ClearOptions.UninitializedMemory);
            NativeArrayUtils.Copy(in allocator, arr, 0, ref this, 0, arr.Length);

        }

        public void Dispose(ref MemoryAllocator allocator) {

            if (this.arrPtr(in allocator) != 0) allocator.Ref<InternalData>(this.ptr).Dispose(ref allocator);
            allocator.Free(this.ptr);
            this = default;

        }

        public readonly MemPtr GetMemPtr(in MemoryAllocator allocator) {
            
            return this.arrPtr(in allocator);
            
        }

        public readonly unsafe void* GetUnsafePtr(in MemoryAllocator allocator) {

            return allocator.GetUnsafePtr(this.arrPtr(in allocator));

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

        public ref T this[in MemoryAllocator allocator, int index] {
            get {
                E.RANGE(index, 0, this.Length(in allocator));
                return ref allocator.RefArrayUnmanaged<T>(this.arrPtr(in allocator), index);
            }
        }

        public ref T this[MemoryAllocator allocator, int index] {
            get {
                E.RANGE(index, 0, this.Length(in allocator));
                return ref allocator.RefArrayUnmanaged<T>(this.arrPtr(in allocator), index);
            }
        }

        public bool Resize(ref MemoryAllocator allocator, int newLength, ClearOptions options = ClearOptions.ClearMemory, int growFactor = 1) {

            if (this.isCreated == false) {

                this = new MemArrayAllocator<T>(ref allocator, newLength, options, growFactor);
                return true;

            }
            
            if (newLength <= this.Length(in allocator)) {

                return false;
                
            }

            newLength *= this.growFactor(in allocator);

            var prevLength = this.Length(in allocator);
            this.arrPtr(in allocator) = allocator.ReAllocArrayUnmanaged<T>(this.arrPtr(in allocator), newLength);
            if (options == ClearOptions.ClearMemory) {
                this.Clear(in allocator, prevLength, newLength - prevLength);
            }
            this.size(in allocator) = newLength;
            return true;

        }

        public void Clear(in MemoryAllocator allocator) {

            this.Clear(in allocator, 0, this.Length(in allocator));

        }

        public void Clear(in MemoryAllocator allocator, int index, int length) {

            var size = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>();
            allocator.MemClear(this.arrPtr(in allocator), index * size, length * size);
            
        }

    }

}