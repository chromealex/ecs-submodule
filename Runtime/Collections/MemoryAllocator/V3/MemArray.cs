namespace ME.ECS.Collections.V3 {
    
    using MemPtr = System.Int64;

    public unsafe class MemArrayAllocatorProxy<T> where T : struct {

        private MemArrayAllocator<T> arr;
        private MemoryAllocator allocator;
        
        public MemArrayAllocatorProxy(ref MemoryAllocator allocator, MemArrayAllocator<T> arr) {

            this.arr = arr;
            this.allocator = allocator;

        }

        public MemArrayAllocatorProxy(MemArrayAllocator<T> arr) {

            this.arr = arr;
            if (Worlds.current == null || Worlds.current.currentState == null) {
                return;
            }
            this.allocator = Worlds.current.currentState.allocator;

        }

        public T[] items {
            get {
                var arr = new T[this.arr.Length];
                for (int i = 0; i < this.arr.Length; ++i) {
                    if (this.allocator.zone != null) arr[i] = this.arr[in this.allocator, i];
                }

                return arr;
            }
        }

    }

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(MemArrayAllocatorProxy<>))]
    public struct MemArrayAllocator<T> where T : struct {

        [ME.ECS.Serializer.SerializeField]
        private MemPtr ptr;
        [ME.ECS.Serializer.SerializeField]
        public int Length;
        [ME.ECS.Serializer.SerializeField]
        private int growFactor;
        public bool isCreated => this.ptr != 0L;

        public MemArrayAllocator(ref MemoryAllocator allocator, int length, ClearOptions clearOptions = ClearOptions.ClearMemory, int growFactor = 1) {
            
            this.ptr = length > 0 ? allocator.AllocArrayUnmanaged<T>(length) : 0;
            this.growFactor = growFactor;
            this.Length = length;

            if (clearOptions == ClearOptions.ClearMemory) {
                this.Clear(in allocator);
            }

        }

        public void Dispose(ref MemoryAllocator allocator) {

            allocator.Free(this.ptr);
            this = default;

        }

        public readonly MemPtr GetMemPtr() {
            return this.ptr;
        }

        public unsafe void* GetUnsafePtr(ref MemoryAllocator allocator) {

            return allocator.GetUnsafePtr(this.ptr);

        }

        public ref T this[in MemoryAllocator allocator, int index] {
            get {
                E.RANGE(index, 0, this.Length);
                return ref allocator.RefArrayUnmanaged<T>(this.ptr, index);
            }
        }

        public ref T this[MemoryAllocator allocator, int index] {
            get {
                E.RANGE(index, 0, this.Length);
                return ref allocator.RefArrayUnmanaged<T>(this.ptr, index);
            }
        }

        public bool Resize(ref MemoryAllocator allocator, int newLength, ClearOptions options = ClearOptions.ClearMemory, int growFactor = 1) {

            if (newLength <= this.Length) {

                return false;
                
            }

            if (this.isCreated == false) this.growFactor = growFactor;
            newLength *= this.growFactor;

            var prevLength = this.Length;
            this.ptr = allocator.ReAllocArrayUnmanaged<T>(this.ptr, newLength);
            if (options == ClearOptions.ClearMemory) {
                this.Clear(in allocator, prevLength, newLength - prevLength);
            }
            this.Length = newLength;
            return true;

        }

        public void Clear(in MemoryAllocator allocator) {

            this.Clear(in allocator, 0, this.Length);

        }

        public void Clear(in MemoryAllocator allocator, int index, int length) {

            var size = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>();
            allocator.MemClear(this.ptr, index * size, length * size);
            
        }

    }

}