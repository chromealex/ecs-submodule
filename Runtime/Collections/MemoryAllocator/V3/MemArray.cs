namespace ME.ECS.Collections.V3 {
    
    using MemPtr = System.Int64;

    public struct MemArray<T> where T : struct {

        [ME.ECS.Serializer.SerializeField]
        private MemPtr ptr;
        [ME.ECS.Serializer.SerializeField]
        public int Length;
        [ME.ECS.Serializer.SerializeField]
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

    public unsafe class MemArrayAllocatorProxy<T> where T : struct {

        private MemArrayAllocator<T> arr;
        private MemoryAllocator allocator;
        
        public MemArrayAllocatorProxy(ref MemoryAllocator allocator, MemArrayAllocator<T> arr) {

            this.arr = arr;
            this.allocator = allocator;

        }

        public MemArrayAllocatorProxy(MemArrayAllocator<T> arr) {

            this.arr = arr;
            if (Worlds.current == null || Worlds.current.currentState == null) return;
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
        public bool isCreated => this.ptr != 0L;

        public MemArrayAllocator(ref MemoryAllocator allocator, int length, ClearOptions clearOptions = ClearOptions.ClearMemory) {
            
            this.ptr = length > 0 ? allocator.AllocArrayUnmanaged<T>(length) : 0;
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
            get => ref allocator.RefArrayUnmanaged<T>(this.ptr, index); 
        }

        public ref T this[MemoryAllocator allocator, int index] {
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

        public void Clear(in MemoryAllocator allocator) {

            this.Clear(in allocator, 0, this.Length);

        }

        public void Clear(in MemoryAllocator allocator, int index, int length) {

            var size = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>();
            allocator.MemClear(this.ptr, index * size, length * size);
            
        }

    }

}