using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Internal;

namespace ME.ECS.Collections {

    public static class NativeExt {

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private static void CheckArray(int index, int length) {
            
            if (index < 0 || index >= length)
                throw new ArgumentOutOfRangeException(nameof(index));

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref T GetRef<T>(this Unity.Collections.NativeSlice<T> array, int index) where T : struct {
            CheckArray(index, array.Length);
            unsafe {
                var ptr = array.GetUnsafePtr();
                #if UNITY_2020_1_OR_NEWER
                return ref UnsafeUtility.ArrayElementAsRef<T>(ptr, index);
                #else
                throw new Exception("UnsafeUtility.ArrayElementAsRef");
                #endif
            }
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref T GetRefRead<T>(this Unity.Collections.NativeSlice<T> array, int index) where T : struct {
            CheckArray(index, array.Length);
            unsafe {
                var ptr = array.GetUnsafeReadOnlyPtr();
                #if UNITY_2020_1_OR_NEWER
                return ref UnsafeUtility.ArrayElementAsRef<T>(ptr, index);
                #else
                throw new Exception("UnsafeUtility.ArrayElementAsRef");
                #endif
            }
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref T GetRef<T>(this Unity.Collections.NativeArray<T> array, int index) where T : struct {
            CheckArray(index, array.Length);
            unsafe {
                var ptr = array.GetUnsafePtr();
                #if UNITY_2020_1_OR_NEWER
                return ref UnsafeUtility.ArrayElementAsRef<T>(ptr, index);
                #else
                throw new Exception("UnsafeUtility.ArrayElementAsRef");
                #endif
            }
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref readonly T GetRefRead<T>(this Unity.Collections.NativeArray<T> array, int index) where T : struct {
            CheckArray(index, array.Length);
            unsafe {
                var ptr = array.GetUnsafeReadOnlyPtr();
                #if UNITY_2020_1_OR_NEWER
                return ref UnsafeUtility.ArrayElementAsRef<T>(ptr, index);
                #else
                throw new Exception("UnsafeUtility.ArrayElementAsRef");
                #endif
            }
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref T GetRef<T>(this Unity.Collections.NativeList<T> array, int index) where T : unmanaged {
            CheckArray(index, array.Length);
            unsafe {
                var ptr = array.GetUnsafePtr();
                #if UNITY_2020_1_OR_NEWER
                return ref UnsafeUtility.ArrayElementAsRef<T>(ptr, index);
                #else
                throw new Exception("UnsafeUtility.ArrayElementAsRef");
                #endif
            }
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref T GetRefRead<T>(this Unity.Collections.NativeList<T> array, int index) where T : unmanaged {
            CheckArray(index, array.Length);
            unsafe {
                var ptr = array.GetUnsafeReadOnlyPtr();
                #if UNITY_2020_1_OR_NEWER
                return ref UnsafeUtility.ArrayElementAsRef<T>(ptr, index);
                #else
                throw new Exception("UnsafeUtility.ArrayElementAsRef");
                #endif
            }
        }

        #if NATIVE_ARRAY_BURST
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref T GetRef<T>(this NativeArrayBurst<T> array, int index) where T : struct {
            CheckArray(index, array.Length);
            unsafe {
                var ptr = array.GetUnsafePtr();
                #if UNITY_2020_1_OR_NEWER
                return ref UnsafeUtility.ArrayElementAsRef<T>(ptr, index);
                #else
                throw new Exception("UnsafeUtility.ArrayElementAsRef");
                #endif
            }
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref T GetRefRead<T>(this NativeArrayBurst<T> array, int index) where T : struct {
            CheckArray(index, array.Length);
            unsafe {
                var ptr = array.GetUnsafePtr();
                #if UNITY_2020_1_OR_NEWER
                return ref UnsafeUtility.ArrayElementAsRef<T>(ptr, index);
                #else
                throw new Exception("UnsafeUtility.ArrayElementAsRef");
                #endif
            }
        }
        #endif

    }
    
}
