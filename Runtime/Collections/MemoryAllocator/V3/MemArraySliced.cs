namespace ME.ECS.Collections.V3 {

    using MemPtr = System.Int64;
    
    public struct MemArraySlicedAllocator<T> where T : struct {

        private struct InternalData {

            public MemArrayAllocator<T> data;
            public MemArrayAllocator<MemArrayAllocator<T>> tails;
            public int tailsLength;

            public void Dispose(ref MemoryAllocator allocator) {
                
                this.data.Dispose(ref allocator);
                
                for (int i = 0, length = this.tails.Length(in allocator); i < length; ++i) {

                    ref var tail = ref this.tails[in allocator, i];
                    tail.Dispose(ref allocator);

                }

                this.tails.Dispose(ref allocator);

                this = default;

            }

        }
        
        private const int BUCKET_SIZE = 4;

        [ME.ECS.Serializer.SerializeField]
        private readonly MemPtr ptr;
        
        internal readonly ref MemArrayAllocator<T> data(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).data;
        private readonly ref MemArrayAllocator<MemArrayAllocator<T>> tails(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).tails;
        private readonly ref int tailsLength(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).tailsLength;
        
        public bool isCreated => this.ptr != 0;

        public int Length(in MemoryAllocator allocator) {
            return this.data(in allocator).Length(in allocator) + this.tailsLength(in allocator);
        }

        public MemArraySlicedAllocator(ref MemoryAllocator allocator, int length, ClearOptions options = ClearOptions.ClearMemory) {

            this.ptr = allocator.AllocData<InternalData>(default);
            this.data(in allocator) = new MemArrayAllocator<T>(ref allocator, length, options);
            this.tails(in allocator) = default;
            this.tailsLength(in allocator) = 0;

        }

        public void Dispose(ref MemoryAllocator allocator) {

            allocator.Ref<InternalData>(this.ptr).Dispose(ref allocator);
            allocator.Free(this.ptr);
            this = default;

        }

        public unsafe void* GetUnsafePtr(in MemoryAllocator allocator) {

            return this.data(in allocator).GetUnsafePtr(in allocator);

        }

        public MemArraySlicedAllocator<T> Merge(ref MemoryAllocator allocator) {
            
            if (this.tails(in allocator).isCreated == false || this.tails(in allocator).Length(in allocator) == 0) {

                return this;

            }

            //var arr = PoolArrayNative<T>.Spawn(this.Length);
            //if (this.data.isCreated == true) NativeArrayUtils.Copy(this.data, 0, ref arr, 0, this.data.Length);
            var elementSize = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>();
            var ptr = this.data(in allocator).Length(in allocator) * elementSize;
            this.data(in allocator).Resize(ref allocator, this.Length(in allocator));
            for (int i = 0, length = this.tails(in allocator).Length(in allocator); i < length; ++i) {

                ref var tail = ref this.tails(in allocator)[in allocator, i];
                if (tail.isCreated == false) continue;

                allocator.MemCopy(this.data(in allocator).GetMemPtr(in allocator), ptr, tail.GetMemPtr(in allocator), 0, tail.Length(in allocator) * elementSize);
                ptr += tail.Length(in allocator) * elementSize;
                tail.Dispose(ref allocator);

            }

            this.tails(in allocator) = default;
            this.tailsLength(in allocator) = 0;
            
            return this;
            
        }

        public ref T this[in MemoryAllocator allocator, int index] {
            get {
                var data = this.data(in allocator);
                if (index >= data.Length(in allocator)) {

                    // Look into tails
                    var tails = this.tails(in allocator);
                    index -= data.Length(in allocator);
                    for (int i = 0, length = tails.Length(in allocator); i < length; ++i) {

                        ref var tail = ref tails[in allocator, i];
                        var len = tail.Length(in allocator);
                        if (index < len) return ref tail[in allocator, index];
                        index -= len;

                    }

                }

                return ref data[in allocator, index];
            }
        }

        public MemArraySlicedAllocator<T> Resize(ref MemoryAllocator allocator, int newLength, out bool result, ClearOptions options = ClearOptions.ClearMemory) {

            if (this.isCreated == false) {

                result = true;
                this = new MemArraySlicedAllocator<T>(ref allocator, newLength, options);
                return this;

            }
            
            var index = newLength - 1;
            if (index >= this.Length(in allocator)) {

                // Do we need any tail?
                var tails = this.tails(in allocator);
                if (tails.isCreated == true) {
                    // Look into tails
                    var ptr = this.data(in allocator).Length(in allocator);
                    for (int i = 0, length = this.tails(in allocator).Length(in allocator); i < length; ++i) {

                        // For each tail determine do we need to resize any tail to store index?
                        var tail = tails[in allocator, i];
                        ptr += tail.Length(in allocator);
                        if (index >= ptr) continue;

                        // We have found tail without resize needed
                        // Tail was found - we do not need to resize
                        result = false;
                        return this;

                    }
                }

                // Need to add new tail and resize tails container
                var idx = tails.isCreated == false ? 0 : tails.Length(in allocator);
                var size = this.Length(in allocator);
                tails.Resize(ref allocator, idx + 1, options);
                var bucketSize = index + MemArraySlicedAllocator<T>.BUCKET_SIZE - size;
                var newTail = new MemArrayAllocator<T>(ref allocator, bucketSize);
                tails[in allocator, idx] = newTail;
                this.tails(in allocator) = tails;
                this.tailsLength(in allocator) += bucketSize;
                result = true;
                return this;

            }

            // We dont need to resize any
            result = false;
            return this;

        }

        public void Clear(ref MemoryAllocator allocator) {

            this.Clear(ref allocator, 0, this.Length(in allocator));

        }

        public void Clear(ref MemoryAllocator allocator, int index, int length) {

            this.Merge(ref allocator);
            var size = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>();
            allocator.MemClear(this.data(in allocator).GetMemPtr(in allocator), index * size, length * size);
            
        }

    }

}