namespace ME.ECS.Collections.V3 {
    
    using MemPtr = System.Int64;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(MemArrayAllocatorProxy<>))]
    public struct MemArrayAllocator<T> where T : struct {

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
            for (int i = 0; i < arr.Length; ++i) {
                this[in allocator, i] = arr[i];
            }

        }

        public MemArrayAllocator(ref MemoryAllocator allocator, ListCopyable<T> arr) {

            this = new MemArrayAllocator<T>(ref allocator, arr.Count, ClearOptions.UninitializedMemory);
            for (int i = 0; i < arr.Count; ++i) {
                this[in allocator, i] = arr[i];
            }

        }

        public void Dispose(ref MemoryAllocator allocator) {

            if (this.arrPtr(in allocator) != 0) allocator.Ref<InternalData>(this.ptr).Dispose(ref allocator);
            allocator.Free(this.ptr);
            this = default;

        }

        public readonly MemPtr GetMemPtr(in MemoryAllocator allocator) {
            
            return this.arrPtr(in allocator);
            
        }

        public unsafe void* GetUnsafePtr(ref MemoryAllocator allocator) {

            return allocator.GetUnsafePtr(this.arrPtr(in allocator));

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