using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Internal;

namespace ME.ECS.Collections {

    public static class NativeExt {

        public static ref T GetRef<T>(this Unity.Collections.NativeSlice<T> array, int index) where T : struct {
            if (index < 0 || index >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            unsafe {
                var ptr = array.GetUnsafePtr();
                return ref UnsafeUtility.ArrayElementAsRef<T>(ptr, index);
            }
        }

        public static ref T GetRef<T>(this Unity.Collections.NativeArray<T> array, int index) where T : struct {
            if (index < 0 || index >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            unsafe {
                var ptr = array.GetUnsafePtr();
                return ref UnsafeUtility.ArrayElementAsRef<T>(ptr, index);
            }
        }

    }
    
}