namespace ME.ECS.Collections.LowLevel {
    
    using Unsafe;
    
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

    public unsafe struct UnsafeMemArraySlicedAllocator {
        
        public static readonly Unity.Burst.SharedStatic<int> arrayVersion = Unity.Burst.SharedStatic<int>.GetOrCreate<UnsafeMemArrayAllocator>();
        
        [ME.ECS.Serializer.SerializeField]
        public int version;

        internal const int BUCKET_SIZE = 4;

        [ME.ECS.Serializer.SerializeField]
        public UnsafeMemArrayAllocator data;
        [ME.ECS.Serializer.SerializeField]
        private MemArrayAllocator<UnsafeMemArrayAllocator> tails;
        [ME.ECS.Serializer.SerializeField]
        private int tailsLength;

        public bool isCreated {
            [INLINE(256)]
            get => this.data.isCreated;
        }

        public int Length {
            [INLINE(256)]
            get => this.data.Length + this.tailsLength;
        }

        [INLINE(256)]
        public UnsafeMemArraySlicedAllocator(int sizeOf, ref MemoryAllocator allocator, int length, ClearOptions options = ClearOptions.ClearMemory) {

            this.data = new UnsafeMemArrayAllocator(sizeOf, ref allocator, length, options);
            this.tails = default;
            this.tailsLength = 0;
            this.version = ++UnsafeMemArraySlicedAllocator.arrayVersion.Data;

        }

        [INLINE(256)]
        public void Dispose(ref MemoryAllocator allocator) {
            
            this.data.Dispose(ref allocator);
                
            for (int i = 0, length = this.tails.Length; i < length; ++i) {

                ref var tail = ref this.tails[in allocator, i];
                tail.Dispose(ref allocator);

            }

            this.tails.Dispose(ref allocator);
            this = default;

        }

        [INLINE(256)]
        public UnsafeMemArraySlicedAllocator Merge(int sizeOf, ref MemoryAllocator allocator) {
            
            if (this.tails.isCreated == false || this.tails.Length == 0) {

                return this;

            }

            //var arr = PoolArrayNative<T>.Spawn(this.Length);
            //if (this.data.isCreated == true) NativeArrayUtils.Copy(this.data, 0, ref arr, 0, this.data.Length);
            var elementSize = sizeOf;
            var ptr = this.data.Length * elementSize;
            this.data.Resize(sizeOf, ref allocator, this.Length);
            for (int i = 0, length = this.tails.Length; i < length; ++i) {

                ref var tail = ref this.tails[in allocator, i];
                if (tail.isCreated == false) continue;

                allocator.MemMove(this.data.arrPtr, ptr, tail.arrPtr, 0, tail.Length * elementSize);
                ptr += tail.Length * elementSize;
                tail.Dispose(ref allocator);

            }
            
            this.version = ++UnsafeMemArraySlicedAllocator.arrayVersion.Data;

            this.tails = default;
            this.tailsLength = 0;
            
            return this;
            
        }

        [INLINE(256)]
        public ref T Get<T>(in MemoryAllocator allocator, int index) where T : unmanaged {
            
            var data = this.data;
            if (index >= data.Length) {

                // Look into tails
                var tails = this.tails;
                index -= data.Length;
                for (int i = 0, length = tails.Length; i < length; ++i) {

                    ref var tail = ref tails[in allocator, i];
                    var len = tail.Length;
                    if (index < len) return ref tail.Get<T>(in allocator, index);
                    index -= len;

                }

            }

            return ref data.Get<T>(in allocator, index);
            
        }

        [INLINE(256)]
        public UnsafeMemArraySlicedAllocator Resize(int sizeOf, ref MemoryAllocator allocator, int newLength, out bool result, ClearOptions options = ClearOptions.ClearMemory) {

            if (this.isCreated == false) {

                result = true;
                this = new UnsafeMemArraySlicedAllocator(sizeOf, ref allocator, newLength, options);
                return this;

            }
            
            var index = newLength - 1;
            if (index >= this.Length) {

                // Do we need any tail?
                var tails = this.tails;
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
                var idx = tails.isCreated == false ? 0 : tails.Length;
                var size = this.Length;
                tails.Resize(ref allocator, idx + 1, options);
                var bucketSize = index + UnsafeMemArraySlicedAllocator.BUCKET_SIZE - size;
                var newTail = new UnsafeMemArrayAllocator(sizeOf, ref allocator, bucketSize);
                tails[in allocator, idx] = newTail;
                this.tails = tails;
                this.tailsLength += bucketSize;
                this.version = ++UnsafeMemArraySlicedAllocator.arrayVersion.Data;
                result = true;
                return this;

            }

            // We dont need to resize any
            result = false;
            return this;

        }

        [INLINE(256)]
        public void Clear(int sizeOf, ref MemoryAllocator allocator) {

            this.Clear(sizeOf, ref allocator, 0, this.Length);

        }

        [INLINE(256)]
        public void Clear(int sizeOf, ref MemoryAllocator allocator, int index, int length) {

            this.Merge(sizeOf, ref allocator);
            var size = sizeOf;
            allocator.MemClear(this.data.arrPtr, index * size, length * size);
            
        }

    }
    
    public struct MemArraySlicedAllocator<T> where T : struct {
        
        public static readonly Unity.Burst.SharedStatic<int> arrayVersion = Unity.Burst.SharedStatic<int>.GetOrCreate<MemArraySlicedAllocator<T>>();

        [ME.ECS.Serializer.SerializeField]
        public MemArrayAllocator<T> data;
        [ME.ECS.Serializer.SerializeField]
        public int version;
        [ME.ECS.Serializer.SerializeField]
        private MemArrayAllocator<MemArrayAllocator<T>> tails;
        [ME.ECS.Serializer.SerializeField]
        private int tailsLength;
        
        public bool isCreated {
            [INLINE(256)]
            get => this.data.isCreated;
        }

        public int Length {
            [INLINE(256)]
            get => this.data.Length + this.tailsLength;
        }

        [INLINE(256)]
        public MemArraySlicedAllocator(ref MemoryAllocator allocator, int length, ClearOptions options = ClearOptions.ClearMemory) {

            this.data = new MemArrayAllocator<T>(ref allocator, length, options);
            this.tails = default;
            this.tailsLength = 0;
            this.version = ++MemArraySlicedAllocator<T>.arrayVersion.Data;

        }

        [INLINE(256)]
        public void Dispose(ref MemoryAllocator allocator) {
            
            this.data.Dispose(ref allocator);
                
            for (int i = 0, length = this.tails.Length; i < length; ++i) {

                ref var tail = ref this.tails[in allocator, i];
                tail.Dispose(ref allocator);

            }

            this.tails.Dispose(ref allocator);
            this = default;

        }

        [INLINE(256)]
        public readonly bool HasMerge(in MemoryAllocator allocator) {
            
            if (this.tails.isCreated == false || this.tails.Length == 0) {

                return false;

            }

            return true;

        }

        [INLINE(256)]
        public MemArraySlicedAllocator<T> Merge(ref MemoryAllocator allocator) {
            
            if (this.tails.isCreated == false || this.tails.Length == 0) {

                return this;

            }

            //var arr = PoolArrayNative<T>.Spawn(this.Length);
            //if (this.data.isCreated == true) NativeArrayUtils.Copy(this.data, 0, ref arr, 0, this.data.Length);
            var elementSize = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>();
            var ptr = this.data.Length * elementSize;
            this.data.Resize(ref allocator, this.Length);
            for (int i = 0, length = this.tails.Length; i < length; ++i) {

                ref var tail = ref this.tails[in allocator, i];
                if (tail.isCreated == false) continue;

                allocator.MemMove(this.data.arrPtr, ptr, tail.arrPtr, 0, tail.Length * elementSize);
                ptr += tail.Length * elementSize;
                tail.Dispose(ref allocator);

            }

            this.tails = default;
            this.tailsLength = 0;
            this.version = ++MemArraySlicedAllocator<T>.arrayVersion.Data;
            
            return this;
            
        }

        [INLINE(256)]
        public readonly unsafe void* GetUnsafePtr(in MemoryAllocator allocator, int index) {
            
            ref readonly var data = ref this.data;
            if (index >= data.Length) {

                // Look into tails
                ref readonly var tails = ref this.tails;
                index -= data.Length;
                for (int i = 0, length = tails.Length; i < length; ++i) {

                    ref var tail = ref tails[in allocator, i];
                    var len = tail.Length;
                    if (index < len) return allocator.GetUnsafePtr(tail.GetAllocPtr(in allocator, index));
                    index -= len;

                }

            }

            return allocator.GetUnsafePtr(data.GetAllocPtr(in allocator, index));
            
        }

        [INLINE(256)]
        public readonly MemPtr GetAllocPtr(in MemoryAllocator allocator, int index) {
            
            ref readonly var data = ref this.data;
            if (index >= data.Length) {

                // Look into tails
                ref readonly var tails = ref this.tails;
                index -= data.Length;
                for (int i = 0, length = tails.Length; i < length; ++i) {

                    ref var tail = ref tails[in allocator, i];
                    var len = tail.Length;
                    if (index < len) return tail.GetAllocPtr(in allocator, index);
                    index -= len;

                }

            }

            return data.GetAllocPtr(in allocator, index);
            
        }

        public readonly ref T this[in MemoryAllocator allocator, int index] {
            [INLINE(256)]
            get {
                ref readonly var data = ref this.data;
                if (index >= data.Length) {

                    // Look into tails
                    ref readonly var tails = ref this.tails;
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

        [INLINE(256)]
        public MemArraySlicedAllocator<T> Resize(ref MemoryAllocator allocator, int newLength, out bool result, ClearOptions options = ClearOptions.ClearMemory) {

            if (this.isCreated == false) {

                result = true;
                this = new MemArraySlicedAllocator<T>(ref allocator, newLength, options);
                return this;

            }
            
            var index = newLength - 1;
            if (index >= this.Length) {

                // Do we need any tail?
                var tails = this.tails;
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
                var idx = tails.isCreated == false ? 0 : tails.Length;
                var size = this.Length;
                tails.Resize(ref allocator, idx + 1, options);
                var bucketSize = index + UnsafeMemArraySlicedAllocator.BUCKET_SIZE - size;
                var newTail = new MemArrayAllocator<T>(ref allocator, bucketSize);
                tails[in allocator, idx] = newTail;
                this.tails = tails;
                this.tailsLength += bucketSize;
                this.version = ++MemArraySlicedAllocator<T>.arrayVersion.Data;
                result = true;
                return this;

            }

            // We dont need to resize any
            result = false;
            return this;

        }

        [INLINE(256)]
        public void Clear(ref MemoryAllocator allocator) {

            this.Clear(ref allocator, 0, this.Length);

        }

        [INLINE(256)]
        public void Clear(ref MemoryAllocator allocator, int index, int length) {

            this.Merge(ref allocator);
            var size = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>();
            allocator.MemClear(this.data.arrPtr, index * size, length * size);
            
        }

    }

}