#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

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

            return System.Buffers.ArrayPool<T>.Shared.Rent(length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Release(ref T[] arr) {

            if (arr == null) return;
            System.Buffers.ArrayPool<T>.Shared.Return(arr);
            arr = null;
            
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static BufferArray<T> Spawn(int length, bool realSize = false) {

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

            T[] arr = buffer.arr;
            if (arr != null) System.Array.Clear(arr, 0, arr.Length);
            PoolArray<T>.Release(ref arr);
            buffer = new BufferArray<T>(null, 0);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle(BufferArray<T> buffer) {

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
    
    /*
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class PoolArray<T> {

        /// <summary>
        /// Maximum length of an array pooled using ClaimWithExactLength.
        /// Arrays with lengths longer than this will silently not be pooled.
        /// </summary>
        private const int MaximumExactArrayLength = 256;
        private const int MAX_STACK_SIZE = 31;

        /// <summary>
        /// Internal pool.
        /// The arrays in each bucket have lengths of 2^i
        /// </summary>
        private static readonly System.Collections.Generic.Stack<T[]>[] pool = new System.Collections.Generic.Stack<T[]>[PoolArray<T>.MAX_STACK_SIZE];
        //private static readonly System.Collections.Generic.Stack<T[]>[] pool = new System.Collections.Generic.Stack<T[]>[PoolArray<T>.MAX_STACK_SIZE];
        private static readonly System.Collections.Generic.Stack<T[]>[] exactPool = new System.Collections.Generic.Stack<T[]>[PoolArray<T>.MaximumExactArrayLength + 1];

        private static readonly System.Collections.Generic.HashSet<T[]> outArrays = new System.Collections.Generic.HashSet<T[]>();

        private static readonly T[] empty = new T[0];

        public static void Initialize() {

            if (PoolArray<T>.pool[0] == null) {

                for (int i = 0; i < PoolArray<T>.MAX_STACK_SIZE; ++i) {

                    var bucketIndex = i;
                    PoolArray<T>.pool[bucketIndex] = new System.Collections.Generic.Stack<T[]>();
                    
                }

            }

        }

        /// <summary>
        /// Returns an array with at least the specified length.
        /// Warning: Returned arrays may contain arbitrary data.
        /// You cannot rely on it being zeroed out.
        /// </summary>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal static T[] Claim(int minimumLength) {

            //return new T[minimumLength];

            if (minimumLength <= 0) {
                return PoolArray<T>.empty; //PoolArray<T>.ClaimWithExactLength(0);
            }

            var bucketIndex = 0;
            while (1 << bucketIndex < minimumLength && bucketIndex < 30) {
                ++bucketIndex;
            }

            if (bucketIndex == 30) {
                throw new System.ArgumentException("Too high minimum length");
            }

            if (PoolArray<T>.pool[0] == null) PoolArray<T>.Initialize();

            var pool = PoolArray<T>.pool[bucketIndex];
            if (pool.Count > 0) {

                var arrPooled = pool.Pop();
                #if UNITY_EDITOR
                if (PoolArray<T>.outArrays.Contains(arrPooled) == true) {

                    UnityEngine.Debug.LogError("You are trying to pool array that has been already in pool");

                }

                PoolArray<T>.outArrays.Add(arrPooled);
                #endif

                //UnityEngine.Debug.Log("Spawn array: " + arrPooled + " :: " + arrPooled.GetHashCode());
                return arrPooled;

            }

            var arr = new T[1 << bucketIndex];
            #if UNITY_EDITOR
            PoolArray<T>.outArrays.Add(arr);
            #endif
            //UnityEngine.Debug.Log("Spawn new array: " + arr + " :: " + arr.GetHashCode());
            return arr;
        }

        /// <summary>
        /// Returns an array with the specified length.
        /// Use with caution as pooling too many arrays with different lengths that
        /// are rarely being reused will lead to an effective memory leak.
        ///
        /// Use <see cref="Claim"/> if you just need an array that is at least as large as some value.
        ///
        /// Warning: Returned arrays may contain arbitrary data.
        /// You cannot rely on it being zeroed out.
        /// </summary>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal static T[] ClaimWithExactLength(int length) {
            var isPowerOfTwo = length != 0 && (length & (length - 1)) == 0;
            if (isPowerOfTwo) {
                // Will return the correct array length
                return PoolArray<T>.Claim(length);
            }

            if (length <= PoolArray<T>.MaximumExactArrayLength) {
                var stack = PoolArray<T>.exactPool[length];
                return stack.Pop();
            }

            return new T[length];
        }

        /// <summary>
        /// Pool an array.
        /// If the array was got using the <see cref="ClaimWithExactLength"/> method then the allowNonPowerOfTwo parameter must be set to true.
        /// The parameter exists to make sure that non power of two arrays are not pooled unintentionally which could lead to memory leaks.
        /// </summary>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal static void Release(ref T[] array, bool allowNonPowerOfTwo = false) {

            //array = null;
            //return;

            if (array == null || array.Length == 0) {
                return;
            }

            var isPowerOfTwo = array.Length != 0 && (array.Length & (array.Length - 1)) == 0;
            if (!isPowerOfTwo && !allowNonPowerOfTwo && array.Length != 0) {

                throw new System.ArgumentException("Length is not a power of 2");
                //array = null;
                //return;

            }

            if (isPowerOfTwo == true) {

                var bucketIndex = 0;
                while (1 << bucketIndex < array.Length && bucketIndex < 30) {
                    bucketIndex++;
                }

                if (PoolArray<T>.pool[bucketIndex] == null) return;

                #if UNITY_EDITOR
                if (PoolArray<T>.outArrays.Contains(array) == false) {

                    if (PoolArray<T>.pool[bucketIndex].Contains(array) == true) {

                        UnityEngine.Debug.LogError("You are trying to push array that already in pool!");

                    } else {

                        UnityEngine.Debug.LogWarning("You are trying to push array was created without pool!");

                    }

                }

                PoolArray<T>.outArrays.Remove(array);
                #endif

                PoolArray<T>.pool[bucketIndex].Push(array);

            } else if (array.Length <= PoolArray<T>.MaximumExactArrayLength) {

                #if MULTITHREAD_SUPPORT
				lock (PoolArray<T>.pool) {
                #endif

                var stack = PoolArray<T>.exactPool[array.Length];
                if (stack == null) {
                    stack = PoolArray<T>.exactPool[array.Length] = new System.Collections.Generic.Stack<T[]>();
                }

                #if UNITY_EDITOR
                if (PoolArray<T>.outArrays.Contains(array) == false) {

                    if (stack.Contains(array) == true) {

                        UnityEngine.Debug.LogError("You are trying to push array that already in pool!");

                    }

                }

                PoolArray<T>.outArrays.Remove(array);
                #endif

                //UnityEngine.Debug.Log("Recycle array " + array + " :: " + array.GetHashCode());
                stack.Push(array);

                #if MULTITHREAD_SUPPORT
				}
                #endif

            }

            array = null;
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static BufferArray<T> Spawn(int length, bool realSize = false) {

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

            T[] arr = buffer.arr;
            if (arr != null) System.Array.Clear(arr, 0, arr.Length);
            PoolArray<T>.Release(ref arr);
            buffer = new BufferArray<T>(null, 0);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle(BufferArray<T> buffer) {

            //buffer = new BufferArray<T>(null, 0);
            //return;

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

    }*/

}