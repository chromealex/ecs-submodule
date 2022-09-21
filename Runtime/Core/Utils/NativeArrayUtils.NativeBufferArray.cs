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
    public static unsafe partial class NativeArrayUtils {

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

                copy.Recycle(ref item.arr.GetRef(i));

            }

            item = item.Dispose();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle<T, TCopy>(ref NativeBufferArraySliced<T> item, TCopy copy) where TCopy : IArrayElementCopy<T> where T : struct {

            for (int i = 0; i < item.Length; ++i) {

                copy.Recycle(ref item[i]);

            }

            item = item.Dispose();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(NativeBufferArray<T> fromArr, ref NativeBufferArray<T> arr, TCopy copy) where TCopy : IArrayElementCopy<T> where T : struct {

            if (fromArr.isCreated == false) {

                if (arr.isCreated == true) NativeArrayUtils.Recycle(ref arr, copy);
                arr = NativeBufferArray<T>.Empty;
                return;

            }

            if (arr.isCreated == false || fromArr.Length != arr.Length) {

                //if (arr.isCreated == true) NativeArrayUtils.Recycle(ref arr, copy);
                //arr = PoolArrayNative<T>.Spawn(fromArr.Length, options: Unity.Collections.NativeArrayOptions.UninitializedMemory);

                if (arr.arr == null) {
                    arr = PoolArrayNative<T>.Spawn(fromArr.Length, options: Unity.Collections.NativeArrayOptions.UninitializedMemory);
                } else {
                    if (arr.Length > fromArr.Length) {
                        // Clamp to fromArr.Length
                        arr = arr.Clamp(fromArr.Length, copy);
                    } else {
                        // Length changed - resize
                        NativeArrayUtils.Resize(fromArr.Length - 1, ref arr, options: Unity.Collections.NativeArrayOptions.UninitializedMemory);
                    }
                }
                
            }

            for (int i = 0; i < fromArr.Length; ++i) {

                copy.Copy(fromArr.arr[i], ref arr.arr.GetRef(i));

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

                if (arr.arr == null) {
                    arr = PoolArrayNative<T>.Spawn(fromArr.Length, options: Unity.Collections.NativeArrayOptions.UninitializedMemory);
                } else {
                    if (arr.Length > fromArr.Length) {
                        // Clamp to fromArr.Length
                        arr = arr.Clamp(fromArr.Length);
                    } else {
                        // Length changed - resize
                        NativeArrayUtils.Resize(fromArr.Length - 1, ref arr, options: Unity.Collections.NativeArrayOptions.UninitializedMemory);
                    }
                }

            }

            /*
            if (arr.isCreated == false || fromArr.Length != arr.Length) {

                if (arr.isCreated == true) PoolArrayNative<T>.Recycle(ref arr);
                arr = PoolArrayNative<T>.Spawn(fromArr.Length, options: Unity.Collections.NativeArrayOptions.UninitializedMemory);

            }
            */

            Unity.Collections.NativeArray<T>.Copy(fromArr.arr, 0, arr.arr, 0, fromArr.Length);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in BufferArray<T> fromArr, int sourceIndex, ref NativeBufferArray<T> arr, int destIndex, int length) where T : struct {

            if (fromArr.isCreated == false) {

                if (arr.isCreated == true) PoolArrayNative<T>.Recycle(ref arr);
                arr = NativeBufferArray<T>.Empty;
                return;

            }

            if (arr.isCreated == false) {

                if (arr.isCreated == true) PoolArrayNative<T>.Recycle(ref arr);
                arr = PoolArrayNative<T>.Spawn(destIndex + length, options: Unity.Collections.NativeArrayOptions.UninitializedMemory);

            }

            Unity.Collections.NativeArray<T>.Copy(fromArr.arr, sourceIndex, arr.arr, destIndex, length);

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
                arr = PoolArrayNative<T>.Spawn(destIndex + length, options: Unity.Collections.NativeArrayOptions.UninitializedMemory);

            }

            Unity.Collections.NativeArray<T>.Copy(fromArr.arr, sourceIndex, arr.arr, destIndex, length);

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
        public static void Copy<T, TCopy>(NativeBufferArraySliced<T> fromArr, ref NativeBufferArraySliced<T> arr, TCopy copy)
            where TCopy : IArrayElementCopy<T> where T : struct {

            if (fromArr.isCreated == false) {

                if (arr.isCreated == true) NativeArrayUtils.Recycle(ref arr, copy);
                arr = default;
                return;

            }

            if (arr.isCreated == false || fromArr.Length != arr.Length) {

                if (arr.isCreated == false) {
                    arr = new NativeBufferArraySliced<T>(fromArr.Length, Unity.Collections.NativeArrayOptions.UninitializedMemory);
                } else {
                    if (arr.Length > fromArr.Length) {
                        // Clamp to fromArr.Length
                        arr = arr.Clamp(fromArr.Length, copy);
                    } else {
                        // Length changed - resize
                        arr.Resize(fromArr.Length - 1, resizeWithOffset: false, out _, options: Unity.Collections.NativeArrayOptions.UninitializedMemory);
                    }
                }
                
            }

            for (int i = 0; i < fromArr.Length; ++i) {

                copy.Copy(fromArr[i], ref arr[i]);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool Resize<T>(int index, ref NativeBufferArray<T> arr, bool resizeWithOffset = true, Unity.Collections.NativeArrayOptions options = Unity.Collections.NativeArrayOptions.ClearMemory) where T : struct {

            if (index < arr.Length) return false;

            /*var newArr = PoolArrayNative<T>.Spawn(index + 1);
            if (arr.isCreated == true) {
                
                Unity.Collections.NativeArray<T>.Copy(arr.arr, 0, newArr.arr, 0, arr.Length);
                arr.Dispose();
                
            }
            
            arr = newArr;*/
            
            var newLength = index + 1;
            var offset = (resizeWithOffset == true ? 2 : 1);
            if (arr.isCreated == false) {

                arr = PoolArrayNative<T>.Spawn(index * offset + 1, options: Unity.Collections.NativeArrayOptions.UninitializedMemory);
                if (options == Unity.Collections.NativeArrayOptions.ClearMemory) NativeArrayUtils.Clear(arr.arr, 0, newLength);
                arr = new NativeBufferArray<T>(arr.arr, newLength);
                return true;

            }

            if (newLength <= arr.arr.Length) {

                if (options == Unity.Collections.NativeArrayOptions.ClearMemory) {
                    var delta = newLength - arr.Length;
                    if (delta > 0) NativeArrayUtils.Clear(arr.arr, arr.Length, delta);
                }

                arr = new NativeBufferArray<T>(arr.arr, newLength);
                return false;

            }

            {
                var newArr = PoolArrayNative<T>.Spawn(newLength, options: Unity.Collections.NativeArrayOptions.UninitializedMemory);
                Unity.Collections.NativeArray<T>.Copy(arr.arr, 0, newArr.arr, 0, arr.Length);
                if (options == Unity.Collections.NativeArrayOptions.ClearMemory) {
                    var delta = newLength - arr.Length;
                    if (delta > 0) NativeArrayUtils.Clear(newArr.arr, arr.Length, delta);
                }

                arr.Dispose();
                arr = newArr;
            }

            return true;

        }

    }

}