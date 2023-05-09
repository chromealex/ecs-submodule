using System.Runtime.CompilerServices;

namespace ME.ECS.Collections.LowLevel.Unsafe {

    public static unsafe class MemUtils {

        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MemCpy(void* destination, void* source, long size) {
            
            byte* d = (byte*)destination;
            byte* s = (byte*)source;

            for (long i = 0; i < size; i++) {
                d[i] = s[i];
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MemCpyStride(void* destination, int destinationStride, void* source, int sourceStride, int elementSize, int count) {
            
            for (int i = 0; i != count; i++) {
                MemUtils.MemCpy(destination, source, elementSize);
                destination = (byte*)destination + destinationStride;
                source = (byte*)source + sourceStride;
            }
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MemMove(void* destination, void* source, long size) {
            
            if ((ulong)destination < (ulong)source) {
                MemUtils.MemCpy(destination, source, size);
                return;
            }

            byte* d = (byte*)destination;
            byte* s = (byte*)source;

            for (long i = size; i > 0; i--) {
                d[i-1] = s[i-1];
            }
            
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MemSet(void* destination, byte value, long size) {

            byte* d = (byte*)destination;
            
            for (long i = 0; i < size; i++) {
                d[i] = value;
            }
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MemClear(void* destination, long size) => MemUtils.MemSet(destination, 0, size);*/

    }

}