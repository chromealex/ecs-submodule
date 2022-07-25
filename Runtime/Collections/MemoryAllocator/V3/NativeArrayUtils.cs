namespace ME.ECS.Collections.V3 {

    public unsafe class NativeArrayUtils {
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(ref MemoryAllocator allocator,
                                   in MemArrayAllocator<T> fromArr,
                                   ref MemArrayAllocator<T> arr) where T : unmanaged {
            
            NativeArrayUtils.Copy(ref allocator, fromArr, 0, ref arr, 0, fromArr.Length);
            
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(ref MemoryAllocator allocator,
                                   in MemArrayAllocator<T> fromArr,
                                   int sourceIndex,
                                   ref MemArrayAllocator<T> arr,
                                   int destIndex,
                                   int length) where T : unmanaged {

            switch (fromArr.isCreated) {
                case false when arr.isCreated == false:
                    return;

                case false when arr.isCreated == true:
                    arr.Dispose(ref allocator);
                    arr = default;
                    return;
            }

            if (arr.isCreated == false || arr.Length < fromArr.Length) {

                if (arr.isCreated == true) arr.Dispose(ref allocator);
                arr = new MemArrayAllocator<T>(ref allocator, fromArr.Length);//new Unity.Collections.NativeArray<T>(fromArr.Length, allocator);
                
            }

            var size = sizeof(T);
            allocator.MemCopy(arr.GetMemPtr(), destIndex * size, fromArr.GetMemPtr(), sourceIndex * size, length * size);

        }
        
    }

}