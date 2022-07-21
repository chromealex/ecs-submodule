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
        public static void Copy<T>(ListCopyable<T> fromArr, ref ListCopyable<T> arr) {

            if (fromArr == null) {

                if (arr != null) {

                    PoolListCopyable<T>.Recycle(ref arr);

                }

                arr = null;
                return;

            }

            if (arr == null) {

                arr = PoolListCopyable<T>.Spawn(fromArr.Count);

            }

            arr.CopyFrom(fromArr);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(ListCopyable<T> fromArr, ref ListCopyable<T> arr, TCopy copy) where TCopy : IArrayElementCopy<T> {

            if (fromArr == null) {

                if (arr != null) {

                    for (int i = 0; i < arr.Count; ++i) {

                        copy.Recycle(ref arr[i]);

                    }

                    PoolListCopyable<T>.Recycle(ref arr);

                }

                arr = null;
                return;

            }

            if (arr == null) {

                arr = PoolListCopyable<T>.Spawn(fromArr.Count);

            }
            
            arr.CopyFrom(fromArr, copy);
            
            /*
            if (arr == null || fromArr.Count != arr.Count) {
                
                //if (arr != null) PoolListCopyable<T>.Recycle(ref arr);
                //arr = PoolListCopyable<T>.Spawn(fromArr.Count);
                if (arr == null) {
                    arr = PoolListCopyable<T>.Spawn(fromArr.Count);
                } else {
                    if (arr.Length > fromArr.Length) {
                        // Clamp to fromArr.Length
                        arr = arr.Clamp(fromArr.Length, copy);
                    } else {
                        // Length changed - resize
                        arr.EnsureCapacity(fromArr.Count);
                    }
                }

            }

            var cnt = arr.Count;
            for (int i = 0; i < fromArr.Count; ++i) {

                var isDefault = i >= cnt;
                T item = (isDefault ? default : arr[i]);
                copy.Copy(fromArr[i], ref item);
                if (isDefault == true) {

                    arr.Add(item);

                } else {

                    arr[i] = item;

                }

            }*/

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in ListCopyable<T> fromArr, int sourceIndex, ref ME.ECS.Collections.BufferArray<T> arr, int destIndex, int length) {

            if (fromArr == null) {

                if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                arr = BufferArray<T>.Empty;
                return;

            }

            if (arr.arr == null) {

                if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                arr = PoolArray<T>.Spawn(destIndex + length);

            }

            System.Array.Copy(fromArr.innerArray, sourceIndex, arr.arr, destIndex, length);

        }

    }

}