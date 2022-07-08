namespace ME.ECS.Collections.V1 {

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(MemArraySlicedAllocator<>))]
    public class MemArraySlicedAllocatorProxy<T> where T : struct {

        private MemArraySlicedAllocator<T> arr;
        private MemoryAllocator allocator;
        
        public MemArraySlicedAllocatorProxy(ref MemoryAllocator allocator, MemArraySlicedAllocator<T> arr) {

            this.arr = arr;
            this.allocator = allocator;

        }

        public T[] items {
            get {
                var arr = new T[this.arr.Length];
                for (int i = 0; i < this.arr.Length; ++i) {
                    arr[i] = this.arr[in this.allocator, i];
                }

                return arr;
            }
        }

    }

    public struct MemArraySlicedAllocator<T> where T : struct {

        private const int BUCKET_SIZE = 4;

        public MemArrayAllocator<T> data;
        public MemArrayAllocator<MemArrayAllocator<T>> tails;
        public int tailsLength;
        public bool isCreated => this.data.isCreated;

        public int Length {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get => this.data.Length + this.tailsLength;
        }

        public MemArraySlicedAllocator(ref MemoryAllocator allocator, int length) {

            this.data = new MemArrayAllocator<T>(ref allocator, length);
            this.tails = default;
            this.tailsLength = 0;

        }

        public void Dispose(ref MemoryAllocator allocator) {

            this.data.Dispose(ref allocator);
            this.data = default;
            
            for (int i = 0, length = this.tails.Length; i < length; ++i) {

                ref var tail = ref this.tails[in allocator, i];
                tail.Dispose(ref allocator);

            }

            this.tails.Dispose(ref allocator);
            this.tails = default;

        }

        public unsafe void* GetUnsafePtr(ref MemoryAllocator allocator) {

            return this.data.GetUnsafePtr(ref allocator);

        }

        public MemArraySlicedAllocator<T> Merge(ref MemoryAllocator allocator) {
            
            if (this.tails.isCreated == false || this.tails.Length == 0) {

                return this;

            }

            //var arr = PoolArrayNative<T>.Spawn(this.Length);
            //if (this.data.isCreated == true) NativeArrayUtils.Copy(this.data, 0, ref arr, 0, this.data.Length);
            var ptr = this.data.Length;
            this.data.Resize(ref allocator, this.Length);
            for (int i = 0, length = this.tails.Length; i < length; ++i) {

                ref var tail = ref this.tails[in allocator, i];
                if (tail.isCreated == false) continue;

                allocator.MemCopy(this.data.GetMemPtr(), ptr, tail.GetMemPtr(), 0, tail.Length);
                ptr += tail.Length;
                tail.Dispose(ref allocator);

            }

            this.tails = default;
            this.tailsLength = 0;
            
            return this;
            
        }

        public ref T this[in MemoryAllocator allocator, int index] {
            get {
                var data = this.data;
                if (index >= data.Length) {

                    // Look into tails
                    var tails = this.tails;
                    index -= data.Length;
                    for (int i = 0, length = tails.Length; i < length; ++i) {

                        ref var tail = ref tails[in allocator, i];
                        var len = tail.Length;
                        if (index < len) return ref tail[in allocator, index];
                        index -= len;

                    }

                }

                return ref data[in allocator, index];
            }
        }

        public MemArraySlicedAllocator<T> Resize(ref MemoryAllocator allocator, int newLength, out bool result, ClearOptions options = ClearOptions.ClearMemory) {

            var index = newLength - 1;
            if (index >= this.Length) {

                // Do we need any tail?
                ref var tails = ref this.tails;
                if (tails.isCreated == true) {
                    // Look into tails
                    var ptr = this.data.Length;
                    for (int i = 0, length = this.tails.Length; i < length; ++i) {

                        // For each tail determine do we need to resize any tail to store index?
                        var tail = tails[in allocator, i];
                        ptr += tail.Length;
                        if (index >= ptr) continue;

                        // We have found tail without resize needed
                        // Tail was found - we do not need to resize
                        result = false;
                        return this;

                    }
                }

                // Need to add new tail and resize tails container
                var idx = tails.Length;
                var size = this.Length;
                tails.Resize(ref allocator, idx + 1, options);
                var bucketSize = index + MemArraySlicedAllocator<T>.BUCKET_SIZE - size;
                tails[in allocator, idx] = new MemArrayAllocator<T>(ref allocator, bucketSize);
                this.tailsLength += bucketSize;
                result = true;
                return this;

            }

            // We dont need to resize any
            result = false;
            return this;

        }

        public void Clear(ref MemoryAllocator allocator) {

            this.Clear(ref allocator, 0, this.Length);

        }

        public void Clear(ref MemoryAllocator allocator, int index, int length) {

            this.Merge(ref allocator);
            var size = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>();
            allocator.MemClear(this.data.GetMemPtr(), index * size, length * size);
            
        }

    }

}