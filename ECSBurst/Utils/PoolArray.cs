
namespace ME.ECSBurst {
    
    using Collections;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class PoolArrayUtilities {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int GetArrayLengthPot(int length) {

            var bucketIndex = 0;
            while (1 << bucketIndex < length && bucketIndex < 30) {
                ++bucketIndex;
            }

            return 1 << bucketIndex;

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class PoolArrayNative<T> where T : struct {
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static NativeBufferArray<T> Spawn(int length, bool realSize = false) {

            var arrSize = PoolArrayUtilities.GetArrayLengthPot(length);
            var arr = new NativeArrayBurst<T>(arrSize, Unity.Collections.Allocator.Persistent);
            var size = (realSize == true ? arr.Length : length);
            var buffer = new NativeBufferArray<T>(arr, length, realSize == true ? arr.Length : -1);
            ArrayUtils.Clear(buffer, 0, size);

            return buffer;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle(ref NativeBufferArray<T> buffer) {

            if (buffer.isCreated == true) buffer.Dispose();
            buffer = NativeBufferArray<T>.Empty;

        }
        
    }

}