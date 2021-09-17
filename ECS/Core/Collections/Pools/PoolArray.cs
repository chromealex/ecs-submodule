#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {
    
    using System.Diagnostics;
    using System.Threading;

    using Collections;

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

            var arrSize = PoolArray<T>.GetSize(length);
            var arr = new Unity.Collections.NativeArray<T>(arrSize, Unity.Collections.Allocator.Persistent);
            var size = (realSize == true ? arr.Length : length);
            var buffer = new NativeBufferArray<T>(arr, length, realSize == true ? arr.Length : -1);
            NativeArrayUtils.Clear(buffer, 0, size);

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

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class PoolArray<T> {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal static int GetSize(int minimumLength) {

            var bucketIndex = 0;
            while (1 << bucketIndex < minimumLength && bucketIndex < 30) {
                ++bucketIndex;
            }
            if (bucketIndex == 30) {
                throw new System.ArgumentException("Too high minimum length");
            }
            return 1 << bucketIndex;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static T[] Claim(int length) {

            return ME.ECS.Buffers.ArrayPool<T>.Shared.Rent(length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Release(ref T[] arr) {

            if (Pools.isActive == false) {
                arr = default;
                return;
            }

            if (arr == null) return;
            ME.ECS.Buffers.ArrayPool<T>.Shared.Return(arr, true);
            arr = null;
            
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static BufferArray<T> Spawn(int length, bool realSize = false) {

            if (Pools.isActive == false) return new BufferArray<T>(new T[length], length);
            
            var arr = PoolArray<T>.Claim(length);
            var size = (realSize == true ? arr.Length : length);
            var buffer = new BufferArray<T>(arr, length, realSize == true ? arr.Length : -1);
            System.Array.Clear(buffer.arr, 0, size);

            return buffer;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle(ref BufferArray<T> buffer) {

            if (Pools.isActive == false) {
                buffer = default;
                return;
            }

            T[] arr = buffer.arr;
            if (arr != null) System.Array.Clear(arr, 0, arr.Length);
            PoolArray<T>.Release(ref arr);
            buffer = new BufferArray<T>(null, 0);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle(BufferArray<T> buffer) {

            if (Pools.isActive == false) {
                buffer = default;
                return;
            }

            T[] arr = buffer.arr;
            if (arr != null) System.Array.Clear(arr, 0, arr.Length);
            PoolArray<T>.Release(ref arr);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle(ref T[] buffer) {

            buffer = null;

        }

    }

}