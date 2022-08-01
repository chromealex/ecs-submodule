namespace ME.ECS.Collections.V3 {
    
    using MemPtr = System.Int64;
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(MemArrayAllocatorProxy<>))]
    public struct MemArrayAllocator<T> where T : struct {

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
                return this.index < this.list.Length;
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
                return this.index < this.list.Length;
            }

            public T Current => this.list[in this.allocator, this.index];

            object System.Collections.IEnumerator.Current => this.Current;

            void System.Collections.IEnumerator.Reset() {
                this.index = -1;
            }
            
        }

        [ME.ECS.Serializer.SerializeField]
        public MemPtr arrPtr;
        [ME.ECS.Serializer.SerializeField]
        public int Length;
        [ME.ECS.Serializer.SerializeField]
        public int growFactor;

        public readonly bool isCreated {
            [INLINE(256)]
            get => this.arrPtr != 0L;
        }

        [INLINE(256)]
        public MemArrayAllocator(ref MemoryAllocator allocator, int length, ClearOptions clearOptions = ClearOptions.ClearMemory, int growFactor = 1) {

            this.arrPtr = length > 0 ? allocator.AllocArrayUnmanaged<T>(length) : 0;
            this.Length = length;
            this.growFactor = growFactor;

            if (clearOptions == ClearOptions.ClearMemory) {
                this.Clear(in allocator);
            }

        }

        [INLINE(256)]
        public MemArrayAllocator(ref MemoryAllocator allocator, BufferArray<T> arr) {

            this = new MemArrayAllocator<T>(ref allocator, arr.Length, ClearOptions.UninitializedMemory);
            NativeArrayUtils.Copy(in allocator, (T[])arr.arr, 0, ref this, 0, arr.Length);
            
        }

        [INLINE(256)]
        public MemArrayAllocator(ref MemoryAllocator allocator, ListCopyable<T> arr) {

            this = new MemArrayAllocator<T>(ref allocator, arr.Count, ClearOptions.UninitializedMemory);
            NativeArrayUtils.Copy(in allocator, arr.innerArray, 0, ref this, 0, arr.Count);

        }

        [INLINE(256)]
        public MemArrayAllocator(ref MemoryAllocator allocator, T[] arr) {

            this = new MemArrayAllocator<T>(ref allocator, arr.Length, ClearOptions.UninitializedMemory);
            NativeArrayUtils.Copy(in allocator, arr, 0, ref this, 0, arr.Length);

        }

        [INLINE(256)]
        public void Dispose(ref MemoryAllocator allocator) {

            if (this.arrPtr != 0) {
                allocator.Free(this.arrPtr);
            }
            this = default;

        }

        [INLINE(256)]
        public readonly unsafe void* GetUnsafePtr(in MemoryAllocator allocator) {

            return allocator.GetUnsafePtr(this.arrPtr);

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

        public ref T this[in MemoryAllocator allocator, int index] {
            [INLINE(256)]
            get {
                E.RANGE(index, 0, this.Length);
                return ref allocator.RefArrayUnmanaged<T>(this.arrPtr, index);
            }
        }

        public ref T this[MemoryAllocator allocator, int index] {
            [INLINE(256)]
            get {
                E.RANGE(index, 0, this.Length);
                return ref allocator.RefArrayUnmanaged<T>(this.arrPtr, index);
            }
        }

        [INLINE(256)]
        public bool Resize(ref MemoryAllocator allocator, int newLength, ClearOptions options = ClearOptions.ClearMemory, int growFactor = 1) {

            if (this.isCreated == false) {

                this = new MemArrayAllocator<T>(ref allocator, newLength, options, growFactor);
                return true;

            }
            
            if (newLength <= this.Length) {

                return false;
                
            }

            newLength *= this.growFactor;

            var prevLength = this.Length;
            this.arrPtr = allocator.ReAllocArrayUnmanaged<T>(this.arrPtr, newLength);
            if (options == ClearOptions.ClearMemory) {
                this.Clear(in allocator, prevLength, newLength - prevLength);
            }
            this.Length = newLength;
            return true;

        }

        [INLINE(256)]
        public void Clear(in MemoryAllocator allocator) {

            this.Clear(in allocator, 0, this.Length);

        }

        [INLINE(256)]
        public void Clear(in MemoryAllocator allocator, int index, int length) {

            var size = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>();
            allocator.MemClear(this.arrPtr, index * size, length * size);
            
        }

    }

}