using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Internal;

namespace ME.ECSBurst.Collections {

    public static class NativeExt {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref T GetRef<T>(this Unity.Collections.NativeSlice<T> array, int index) where T : struct {
            if (index < 0 || index >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
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
        public static ref T GetRef<T>(this Unity.Collections.NativeArray<T> array, int index) where T : struct {
            if (index < 0 || index >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
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
        public static ref T GetRef<T>(this Unity.Collections.NativeList<T> array, int index) where T : struct {
            if (index < 0 || index >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
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
        public static ref T GetRef<T>(this NativeArrayBurst<T> array, int index) where T : struct {
            if (index < 0 || index >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            unsafe {
                var ptr = array.GetUnsafePtr();
                #if UNITY_2020_1_OR_NEWER
                return ref UnsafeUtility.ArrayElementAsRef<T>(ptr, index);
                #else
                throw new Exception("UnsafeUtility.ArrayElementAsRef");
                #endif
            }
        }

    }
    
}
