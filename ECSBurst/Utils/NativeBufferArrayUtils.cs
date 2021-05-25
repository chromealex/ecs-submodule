#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECSBurst {
    
    using Collections;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static unsafe partial class ArrayUtils {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Clear<T>(NativeBufferArray<T> arr) where T : struct {

            arr.Clear();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Clear<T>(NativeBufferArray<T> arr, int index, int length) where T : struct {

            arr.Clear(index, length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle<T, TCopy>(ref NativeBufferArray<T> item, TCopy copy) where TCopy : IArrayElementCopy<T> where T : struct {

            for (int i = 0; i < item.Length; ++i) {

                copy.Recycle(item.arr[i]);

            }

            item = item.Dispose();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void RecycleWithIndex<T, TCopy>(ref NativeBufferArray<T> item, TCopy copy) where TCopy : IArrayElementCopyWithIndex<T> where T : struct {

            for (int i = 0; i < item.Length; ++i) {

                copy.Recycle(i, ref item.arr.GetRef(i));

            }

            item = item.Dispose();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(NativeBufferArray<T> fromArr, ref NativeBufferArray<T> arr, TCopy copy) where TCopy : IArrayElementCopy<T> where T : struct {

            if (fromArr.isCreated == false) {

                if (arr.isCreated == true) ArrayUtils.Recycle(ref arr, copy);
                arr = NativeBufferArray<T>.Empty;
                return;

            }

            if (arr.isCreated == false || fromArr.Length != arr.Length) {

                if (arr.isCreated == true) ArrayUtils.Recycle(ref arr, copy);
                arr = PoolArrayNative<T>.Spawn(fromArr.Length);

            }

            for (int i = 0; i < fromArr.Length; ++i) {

                copy.Copy(fromArr.arr[i], ref arr.arr.GetRef(i));

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void CopyWithIndex<T, TCopy>(NativeBufferArray<T> fromArr, ref NativeBufferArray<T> arr, TCopy copy) where TCopy : IArrayElementCopyWithIndex<T> where T : struct {

            if (fromArr.isCreated == false) {

                if (arr.isCreated == true) ArrayUtils.RecycleWithIndex(ref arr, copy);
                arr = NativeBufferArray<T>.Empty;
                return;

            }

            if (arr.isCreated == false || fromArr.Length != arr.Length) {

                if (arr.isCreated == true) ArrayUtils.RecycleWithIndex(ref arr, copy);
                arr = PoolArrayNative<T>.Spawn(fromArr.Length);

            }

            for (int i = 0; i < fromArr.Length; ++i) {

                copy.Copy(i, fromArr.arr[i], ref arr.arr.GetRef(i));

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in NativeBufferArray<T> fromArr, ref NativeBufferArray<T> arr) where T : struct {

            if (fromArr.isCreated == false) {

                if (arr.isCreated == true) PoolArrayNative<T>.Recycle(ref arr);
                arr = NativeBufferArray<T>.Empty;
                return;

            }

            if (arr.isCreated == false || fromArr.Length != arr.Length) {

                if (arr.isCreated == true) PoolArrayNative<T>.Recycle(ref arr);
                arr = PoolArrayNative<T>.Spawn(fromArr.Length);

            }

            var newArr = arr.arr;
            ArrayUtils.Copy(fromArr.arr, ref newArr, fromArr.Length);
            arr = new NativeBufferArray<T>(newArr, fromArr.Length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in NativeBufferArray<T> fromArr, int sourceIndex, ref NativeBufferArray<T> arr, int destIndex, int length) where T : struct {

            if (fromArr.isCreated == false) {

                if (arr.isCreated == true) PoolArrayNative<T>.Recycle(ref arr);
                arr = NativeBufferArray<T>.Empty;
                return;

            }

            if (arr.isCreated == false) {

                if (arr.isCreated == true) PoolArrayNative<T>.Recycle(ref arr);
                arr = PoolArrayNative<T>.Spawn(destIndex + length);

            }

            var newArr = arr.arr;
            ArrayUtils.Copy(fromArr.arr, sourceIndex, ref newArr, destIndex, length);
            arr = new NativeBufferArray<T>(newArr, fromArr.Length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool WillResize<T>(int index, ref NativeBufferArray<T> arr) where T : struct {

            if (arr.isCreated == false) return true;
            if (index < arr.Length) return false;

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool WillResizeWithBuffer<T>(int index, ref NativeBufferArray<T> arr) where T : struct {

            if (arr.isCreated == false) return true;
            if (index < arr.Length) return false;
            var newLength = index + 1;
            if (newLength <= arr.arr.Length) return false;

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool Resize<T>(int index, ref NativeBufferArray<T> arr, bool resizeWithOffset = false) where T : struct {

            if (index < arr.Length) return false;

            var offset = (resizeWithOffset == true ? 2 : 1);
            if (arr.isCreated == false) {

                arr = PoolArrayNative<T>.Spawn(index * offset + 1);
                arr = new NativeBufferArray<T>(arr.arr, index + 1);

            }

            var newLength = index + 1;
            if (newLength <= arr.arr.Length) {

                ArrayUtils.Clear(arr.arr, arr.Length, newLength - arr.Length);
                arr = new NativeBufferArray<T>(arr.arr, newLength);
                return false;

            }

            var newArr = PoolArrayNative<T>.Spawn(newLength);
            var copyArr = newArr.arr;
            ArrayUtils.Copy(arr.arr, ref copyArr, arr.Length);
            arr = new NativeBufferArray<T>(copyArr, arr.Length);
            if (arr != newArr) PoolArrayNative<T>.Recycle(ref arr);
            arr = newArr;

            return true;

        }

    }

}