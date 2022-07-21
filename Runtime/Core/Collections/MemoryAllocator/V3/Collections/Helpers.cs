namespace ME.ECS.Collections.MemoryAllocator {

    public static class Helpers {

        public static int NextPot(int n) {

            --n;
            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            ++n;
            return n;

        }

        public static unsafe void Copy<T>(ref ME.ECS.Collections.V3.MemoryAllocator allocator,
                                          in ME.ECS.Collections.V3.MemArrayAllocator<T> sourceArr,
                                          int sourceIndex,
                                          in ME.ECS.Collections.V3.MemArrayAllocator<T> targetArr,
                                          int targetIndex,
                                          int length) where T : unmanaged {

            var size = sizeof(T);
            allocator.MemCopy(targetArr.GetMemPtr(), size * targetIndex, sourceArr.GetMemPtr(), size * sourceIndex, size * length);
            
        }

    }

}