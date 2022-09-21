#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Collections;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static partial class ArrayUtils {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in ME.ECS.Collections.BufferArray<T> fromArr, ref ME.ECS.Collections.BufferArray<T> arr) {

            if (fromArr.arr == null) {

                if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                arr = BufferArray<T>.Empty;
                return;

            }

            if (arr.arr == null || fromArr.Length != arr.Length) {

                //if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                //arr = PoolArray<T>.Spawn(fromArr.Count);
                
                if (arr.arr == null) {
                    arr = PoolArray<T>.Spawn(fromArr.Length);
                } else {
                    if (arr.Length > fromArr.Length) {
                        // Clamp to fromArr.Length
                        arr = arr.Clamp(fromArr.Length);
                    } else {
                        // Length changed - resize
                        ArrayUtils.Resize(fromArr.Length - 1, ref arr);
                    }
                }

            }

            System.Array.Copy(fromArr.arr, arr.arr, fromArr.Length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(ME.ECS.Collections.BufferArray<T> fromArr, ref ME.ECS.Collections.BufferArray<T> arr, TCopy copy) where TCopy : IArrayElementCopy<T> {

            if (fromArr.arr == null) {

                if (arr.arr != null) ArrayUtils.Recycle(ref arr, copy);
                arr = BufferArray<T>.Empty;
                return;

            }

            if (arr.arr == null || fromArr.Length != arr.Length) {

                //if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                //arr = PoolArray<T>.Spawn(fromArr.Count);
                
                if (arr.arr == null) {
                    arr = PoolArray<T>.Spawn(fromArr.Length);
                } else {
                    if (arr.Length > fromArr.Length) {
                        // Clamp to fromArr.Length
                        arr = arr.Clamp(fromArr.Length, copy);
                    } else {
                        // Length changed - resize
                        ArrayUtils.Resize(fromArr.Length - 1, ref arr);
                    }
                }
                
            }

            for (int i = 0; i < fromArr.Length; ++i) {

                copy.Copy(fromArr.arr[i], ref arr.arr[i]);

            }

        }

    }

}