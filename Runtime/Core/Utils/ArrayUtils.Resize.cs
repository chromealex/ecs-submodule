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
        public static bool WillResize<T>(int index, ref T[] arr) {

            if (arr == null) return true;
            if (index < arr.Length) return false;
            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool WillResize<T>(int index, ref BufferArray<T> arr) {

            if (arr.arr == null) return true;
            if (index < arr.Length) return false;

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool Resize<T>(int index, ref BufferArray<T> arr, bool resizeWithOffset = false) {

            if (index < arr.Length) return false;

            var newLength = index + 1;
            var offset = (resizeWithOffset == true ? 2 : 1);
            if (arr.arr == null) {

                arr = PoolArray<T>.Spawn(index * offset + 1);
                arr = new BufferArray<T>(arr.arr, newLength);
                return true;

            }

            // If new length <= inner real array length
            if (newLength <= arr.arr.Length) {

                // Soft resize
                var clearLength = arr.arr.Length - newLength;
                if (clearLength > 0) System.Array.Clear(arr.arr, arr.Length, clearLength);
                arr = new BufferArray<T>(arr.arr, newLength);
                return false;

            }

            // Hard resize
            var newArr = PoolArray<T>.Spawn(newLength);
            System.Array.Copy(arr.arr, newArr.arr, arr.Length);
            if (arr != newArr) PoolArray<T>.Recycle(ref arr);
            arr = newArr;

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool Resize<T>(int index, ref BufferArraySliced<T> arr, bool resizeWithOffset = false) where T : struct {

            arr = arr.Resize(index, resizeWithOffset, out var result);
            return result;

        }

    }

}