namespace ME.ECS.Collections.V3 {

    public unsafe class NativeArrayUtils {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in MemoryAllocator allocator,
                                   in T[] src,
                                   int srcIndex,
                                   ref MemArrayAllocator<T> dst,
                                   int dstIndex,
                                   int length) where T : struct {
        
            var gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(src, System.Runtime.InteropServices.GCHandleType.Pinned);
            var num = gcHandle.AddrOfPinnedObject();
            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemCpy((void*)((System.IntPtr)dst.GetMemPtr(in allocator) + dstIndex * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>()), (void*)((System.IntPtr)(void*)num + srcIndex * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>()), (long)(length * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>()));
            gcHandle.Free();
            
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(ref MemoryAllocator allocator,
                                   in MemArrayAllocator<T> fromArr,
                                   ref MemArrayAllocator<T> arr) where T : unmanaged {
            
            NativeArrayUtils.Copy(ref allocator, fromArr, 0, ref arr, 0, fromArr.Length(in allocator));
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void CopyExact<T>(ref MemoryAllocator allocator,
                                   in MemArrayAllocator<T> fromArr,
                                   ref MemArrayAllocator<T> arr) where T : unmanaged {
            
            NativeArrayUtils.Copy(ref allocator, fromArr, 0, ref arr, 0, fromArr.Length(in allocator), true);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(ref MemoryAllocator allocator,
                                   in MemArrayAllocator<T> fromArr,
                                   int sourceIndex,
                                   ref MemArrayAllocator<T> arr,
                                   int destIndex,
                                   int length,
                                   bool copyExact = false) where T : unmanaged {

            switch (fromArr.isCreated) {
                case false when arr.isCreated == false:
                    return;

                case false when arr.isCreated == true:
                    arr.Dispose(ref allocator);
                    arr = default;
                    return;
            }

            if (arr.isCreated == false || (copyExact == false ? arr.Length(in allocator) < fromArr.Length(in allocator) : arr.Length(in allocator) != fromArr.Length(in allocator))) {

                if (arr.isCreated == true) arr.Dispose(ref allocator);
                arr = new MemArrayAllocator<T>(ref allocator, fromArr.Length(in allocator));
                
            }

            var size = sizeof(T);
            allocator.MemCopy(arr.GetMemPtr(in allocator), destIndex * size, fromArr.GetMemPtr(in allocator), sourceIndex * size, length * size);

        }
        
    }

}