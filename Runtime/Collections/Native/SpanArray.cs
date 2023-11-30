namespace ME.ECS.Collections {

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(ME.ECS.DebugUtils.SpanArrayProxyDebugger<>))]
    public unsafe struct SpanArray<T> where T : unmanaged {

        private T* ptr;
        public int Length;
        private Unity.Collections.Allocator allocator;

        public bool isCreated => this.ptr != null;

        public Unity.Collections.Allocator GetAllocator() => this.allocator;

        public SpanArray(SpanArray<T> source, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Temp) : this(source.Length, allocator) {

            for (int i = 0; i < source.Length; ++i) {
                this[i] = source[i];
            }
            
        }

        public static SpanArray<T> Copy<TCopy>(SpanArray<T> source, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Temp) where TCopy : struct, IArrayElementCopy<T> {
            
            TCopy copy = default;
            var arr = new SpanArray<T>(source.Length, allocator);
            for (int i = 0; i < source.Length; ++i) {
                copy.Copy(source[i], ref arr[i]);
            }

            return arr;

        }
        
        public SpanArray(int length, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Temp) {

            if (length == 0) {
                this = default;
                return;
            }

            var size = length * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>();
            this.allocator = allocator;
            this.ptr = (T*)Unity.Collections.LowLevel.Unsafe.UnsafeUtility.Malloc(size, Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AlignOf<T>(), allocator);
            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemClear(this.ptr, size);
            this.Length = length;

        }

        public SpanArray(T[] arr, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Temp) : this(arr?.Length ?? 0, allocator) {

            if (arr == null) {
                this = default;
                return;
            }

            const int dstIndex = 0;
            const int srcIndex = 0;
            var length = this.Length;
            
            System.Runtime.InteropServices.GCHandle gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(arr, System.Runtime.InteropServices.GCHandleType.Pinned);
            System.IntPtr num = gcHandle.AddrOfPinnedObject();
            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemMove((void*)((System.IntPtr)this.ptr + dstIndex * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>()), (void*) ((System.IntPtr) (void*) num + srcIndex * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>()), (long) (length * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>()));
            gcHandle.Free();
            
        }

        public SpanArray(ListCopyable<T> list, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Temp) : this(list?.Count ?? 0, allocator) {

            if (list == null) {
                this = default;
                return;
            }

            const int dstIndex = 0;
            const int srcIndex = 0;
            var length = this.Length;
            
            System.Runtime.InteropServices.GCHandle gcHandle = System.Runtime.InteropServices.GCHandle.Alloc((System.Array)list.innerArray, System.Runtime.InteropServices.GCHandleType.Pinned);
            System.IntPtr num = gcHandle.AddrOfPinnedObject();
            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemMove((void*)((System.IntPtr)this.ptr + dstIndex * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>()), (void*) ((System.IntPtr) (void*) num + srcIndex * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>()), (long) (length * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>()));
            gcHandle.Free();
            
        }

        public SpanArray(Unity.Collections.NativeList<T> list, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Temp) : this(list.Length, allocator) {

            const int dstIndex = 0;
            const int srcIndex = 0;
            var length = this.Length;
            var num = list.GetUnsafeList()->Ptr;
            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemMove((void*)((System.IntPtr)this.ptr + dstIndex * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>()), (void*) ((System.IntPtr) (void*) num + srcIndex * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>()), (long) (length * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>()));
            
        }

        public ref T this[int index] => ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.ArrayElementAsRef<T>(this.ptr, index);

        public void Dispose() {

            if (this.allocator == Unity.Collections.Allocator.Invalid) return;
            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.Free(this.ptr, this.allocator);
            this.allocator = default;
            this.ptr = null;
            this.Length = 0;

        }

        public static implicit operator SpanArray<T>(T[] arr) {

            return new SpanArray<T>(arr);
            
        }
        
    }

}