#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using ME.ECS.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static partial class NativeArrayUtils {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static unsafe void Clear<T>(Unity.Collections.NativeArray<T> arr) where T : struct {

            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemClear(arr.GetUnsafePtr(), UnsafeUtility.SizeOf<T>() * arr.Length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static unsafe void Clear<T>(Unity.Collections.NativeArray<T> arr, int index, int length) where T : struct {

            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemClear((void*)((System.IntPtr)arr.GetUnsafePtr() + (int)((ulong)UnsafeUtility.SizeOf<T>() * (ulong)index)), (long)((ulong)UnsafeUtility.SizeOf<T>() * (ulong)length));

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void RecycleWithIndex<T, TCopy>(ref Unity.Collections.NativeArray<T> item, TCopy copy) where TCopy : IArrayElementCopyWithIndex<T> where T : struct {

            for (int i = 0; i < item.Length; ++i) {

                copy.Recycle(i, ref item.GetRef(i));

            }

            item.Dispose();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void CopyWithIndex<T, TCopy>(Unity.Collections.NativeArray<T> fromArr, ref Unity.Collections.NativeArray<T> arr, TCopy copy)
            where TCopy : IArrayElementCopyWithIndex<T> where T : struct {

            if (fromArr.IsCreated == false) {

                if (arr.IsCreated == true) NativeArrayUtils.RecycleWithIndex(ref arr, copy);
                arr = default;
                return;

            }

            if (arr.IsCreated == false || fromArr.Length != arr.Length) {

                if (arr.IsCreated == true) NativeArrayUtils.RecycleWithIndex(ref arr, copy);
                arr = new Unity.Collections.NativeArray<T>(fromArr.Length, Unity.Collections.Allocator.Persistent);

            }

            for (int i = 0; i < fromArr.Length; ++i) {

                copy.Copy(i, fromArr[i], ref arr.GetRef(i));

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool Resize<T>(int index, ref Unity.Collections.NativeArray<T> arr, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent, bool resizeWithOffset = false) where T : struct {

            int offset = 1;
            if (resizeWithOffset == true) {

                offset *= 2;

            }

            if (arr.IsCreated == false) arr = new Unity.Collections.NativeArray<T>(index * offset + 1, allocator);
            if (index < arr.Length) return false;

            var newLength = arr.Length * offset + 1;
            if (newLength == 0 || newLength <= index) newLength = index * offset + 1;

            var newArr = new Unity.Collections.NativeArray<T>(newLength, allocator);
            Unity.Collections.NativeArray<T>.Copy(arr, 0, newArr, 0, arr.Length);
            arr.Dispose();
            arr = newArr;

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static unsafe bool Resize<T>(int index, ref Unity.Collections.NativeList<T> arr, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : unmanaged {

            const int offset = 1;

            if (arr.IsCreated == false) arr = new Unity.Collections.NativeList<T>(index * offset + 1, allocator);
            if (index < arr.Length) return false;

            var newLength = arr.Length * offset + 1;
            if (newLength == 0 || newLength <= index) newLength = index * offset + 1;

            var elements = newLength - arr.Length;
            //var ptr = (void*)((System.IntPtr)arr.GetUnsafePtr() + UnsafeUtility.SizeOf<T>() * arr.Length);
            for (int i = 0; i < elements; ++i) arr.Add(default);
            //UnsafeUtility.MemClear(ptr, UnsafeUtility.SizeOf<T>() * elements);
            
            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in Unity.Collections.NativeList<T> fromArr, ref Unity.Collections.NativeList<T> arr) where T : unmanaged {

            switch (fromArr.IsCreated) {
                case false when arr.IsCreated == false:
                    return;

                case false when arr.IsCreated == true:
                    arr.Dispose();
                    arr = default;
                    return;
            }
            
            if (arr.IsCreated == false) {

                if (arr.IsCreated == true) arr.Dispose();
                arr = new Unity.Collections.NativeList<T>(fromArr.Length, Unity.Collections.Allocator.Persistent);

            }
            
            arr.CopyFrom(fromArr);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in Unity.Collections.NativeArray<T> fromArr, ref Unity.Collections.NativeArray<T> arr) where T : struct {

            switch (fromArr.IsCreated) {
                case false when arr.IsCreated == false:
                    return;

                case false when arr.IsCreated == true:
                    arr.Dispose();
                    arr = default;
                    return;
            }

            if (arr.IsCreated == false || arr.Length != fromArr.Length) {

                if (arr.IsCreated == true) arr.Dispose();
                arr = new Unity.Collections.NativeArray<T>(fromArr.Length, Unity.Collections.Allocator.Persistent);
                
            }

            Unity.Collections.NativeArray<T>.Copy(fromArr, arr);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in Unity.Collections.NativeArray<T> fromArr, ref Unity.Collections.NativeArray<T> arr, int length)
            where T : struct {

            NativeArrayUtils.Copy<T>(in fromArr, 0, ref arr, 0, length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in BufferArray<T> fromArr, int sourceIndex, ref Unity.Collections.NativeArray<T> arr, int destIndex, int length) where T : struct {

            switch (fromArr.isCreated) {
                case false when arr.IsCreated == false:
                    return;

                case false when arr.IsCreated == true:
                    arr.Dispose();
                    arr = default;
                    return;
            }

            if (arr.IsCreated == false || arr.Length < fromArr.Length) {

                if (arr.IsCreated == true) arr.Dispose();
                arr = new Unity.Collections.NativeArray<T>(fromArr.Length, Unity.Collections.Allocator.Persistent);
                
            }

            Unity.Collections.NativeArray<T>.Copy(fromArr.arr, sourceIndex, arr, destIndex, length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in Unity.Collections.NativeArray<T> fromArr, int sourceIndex, ref Unity.Collections.NativeArray<T> arr, int destIndex, int length) where T : struct {

            switch (fromArr.IsCreated) {
                case false when arr.IsCreated == false:
                    return;

                case false when arr.IsCreated == true:
                    arr.Dispose();
                    arr = default;
                    return;
            }

            if (arr.IsCreated == false || arr.Length < fromArr.Length) {

                if (arr.IsCreated == true) arr.Dispose();
                arr = new Unity.Collections.NativeArray<T>(fromArr.Length, Unity.Collections.Allocator.Persistent);
                
            }

            Unity.Collections.NativeArray<T>.Copy(fromArr, sourceIndex, arr, destIndex, length);

        }
        
        #if NATIVE_ARRAY_BURST
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in NativeArrayBurst<T> fromArr, ref NativeArrayBurst<T> arr, int length)
            where T : struct {

            NativeArrayUtils.Copy<T>(in fromArr, 0, ref arr, 0, length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static unsafe void Clear<T>(NativeArrayBurst<T> arr) where T : struct {

            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemClear(arr.GetUnsafePtr(), UnsafeUtility.SizeOf<T>() * arr.Length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static unsafe void Clear<T>(NativeArrayBurst<T> arr, int index, int length) where T : struct {

            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemClear((void*)((System.IntPtr)arr.GetUnsafePtr() + (int)((ulong)UnsafeUtility.SizeOf<T>() * (ulong)index)), (long)((ulong)UnsafeUtility.SizeOf<T>() * (ulong)length));

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void RecycleWithIndex<T, TCopy>(ref NativeArrayBurst<T> item, TCopy copy) where TCopy : IArrayElementCopyWithIndex<T> where T : struct {

            for (int i = 0; i < item.Length; ++i) {

                copy.Recycle(i, ref item.GetRef(i));

            }

            item.Dispose();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void CopyWithIndex<T, TCopy>(NativeArrayBurst<T> fromArr, ref NativeArrayBurst<T> arr, TCopy copy)
            where TCopy : IArrayElementCopyWithIndex<T> where T : struct {

            if (fromArr.IsCreated == false) {

                if (arr.IsCreated == true) NativeArrayUtils.RecycleWithIndex(ref arr, copy);
                arr = default;
                return;

            }

            if (arr.IsCreated == false || fromArr.Length != arr.Length) {

                if (arr.IsCreated == true) NativeArrayUtils.RecycleWithIndex(ref arr, copy);
                arr = new NativeArrayBurst<T>(fromArr.Length, Unity.Collections.Allocator.Persistent);

            }

            for (int i = 0; i < fromArr.Length; ++i) {

                copy.Copy(i, fromArr[i], ref arr.GetRef(i));

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(NativeArrayBurst<T> fromArr, ref NativeArrayBurst<T> arr, TCopy copy)
            where TCopy : IArrayElementCopy<T> where T : struct {

            if (fromArr.IsCreated == false) {

                if (arr.IsCreated == true) NativeArrayUtils.Recycle(ref arr, copy);
                arr = default;
                return;

            }

            if (arr.IsCreated == false || fromArr.Length != arr.Length) {

                if (arr.IsCreated == true) NativeArrayUtils.Recycle(ref arr, copy);
                arr = new NativeArrayBurst<T>(fromArr.Length, Unity.Collections.Allocator.Persistent);

            }

            for (int i = 0; i < fromArr.Length; ++i) {

                copy.Copy(fromArr[i], ref arr.GetRef(i));

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle<T, TCopy>(ref NativeArrayBurst<T> item, TCopy copy) where TCopy : IArrayElementCopy<T> where T : struct {

            for (int i = 0; i < item.Length; ++i) {

                copy.Recycle(item[i]);

            }

            item.Dispose();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool Resize<T>(int index, ref NativeArrayBurst<T> arr, bool resizeWithOffset = true,
                                     Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : struct {

            var offset = (resizeWithOffset == true ? 2 : 1);

            if (arr.IsCreated == false) arr = new NativeArrayBurst<T>(index * offset + 1, allocator);
            if (index < arr.Length) return false;

            var newLength = arr.Length * offset + 1;
            if (newLength == 0 || newLength <= index) newLength = index * offset + 1;

            var newArr = new NativeArrayBurst<T>(newLength, allocator);
            NativeArrayUtils.Copy(arr, 0, ref newArr, 0, arr.Length);
            arr.Dispose();
            arr = newArr;

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static unsafe void Copy<T>(in NativeArrayBurst<T> fromArr, int srcIndex, ref Unity.Collections.NativeArray<T> arr, int dstIndex, int length) where T : struct {

            switch (fromArr.IsCreated) {
                case false when arr.IsCreated == false:
                    return;

                case false when arr.IsCreated == true:
                    arr.Dispose();
                    arr = default;
                    return;
            }

            if (arr.IsCreated == false || arr.Length < fromArr.Length) {

                if (arr.IsCreated == true) arr.Dispose();
                arr = new Unity.Collections.NativeArray<T>(fromArr.Length, Unity.Collections.Allocator.Persistent);
                
            }

            UnsafeUtility.MemCpy((void*) ((System.IntPtr) fromArr.m_Buffer + dstIndex * UnsafeUtility.SizeOf<T>()), (void*) ((System.IntPtr) arr.GetUnsafePtr() + srcIndex * UnsafeUtility.SizeOf<T>()), (long) (length * UnsafeUtility.SizeOf<T>()));
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static unsafe void Copy<T>(in NativeArrayBurst<T> fromArr, int srcIndex, ref NativeArrayBurst<T> arr, int dstIndex, int length) where T : struct {

            switch (fromArr.IsCreated) {
                case false when arr.IsCreated == false:
                    return;

                case false when arr.IsCreated == true:
                    arr.Dispose();
                    arr = default;
                    return;
            }

            if (arr.IsCreated == false || arr.Length < fromArr.Length) {

                if (arr.IsCreated == true) arr.Dispose();
                arr = new NativeArrayBurst<T>(fromArr.Length, Unity.Collections.Allocator.Persistent);
                
            }
            
            UnsafeUtility.MemCpy((void*) ((System.IntPtr) arr.m_Buffer + dstIndex * UnsafeUtility.SizeOf<T>()), (void*) ((System.IntPtr) fromArr.m_Buffer + srcIndex * UnsafeUtility.SizeOf<T>()), (long) (length * UnsafeUtility.SizeOf<T>()));
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static unsafe void Copy<T>(in NativeArrayBurst<T> fromArr, ref NativeArrayBurst<T> arr) where T : struct {
            
            NativeArrayUtils.Copy<T>(in fromArr, 0, ref arr, 0, fromArr.Length);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static unsafe void Copy<T>(in Unity.Collections.NativeArray<T> fromArr, ref NativeArrayBurst<T> arr) where T : struct {
            
            NativeArrayUtils.Copy<T>(in fromArr, 0, ref arr, 0, fromArr.Length);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static unsafe void Copy<T>(in Unity.Collections.NativeArray<T> fromArr, int srcIndex, ref NativeArrayBurst<T> arr, int dstIndex, int length) where T : struct {

            switch (fromArr.IsCreated) {
                case false when arr.IsCreated == false:
                    return;

                case false when arr.IsCreated == true:
                    arr.Dispose();
                    arr = default;
                    return;
            }

            if (arr.IsCreated == false || arr.Length < fromArr.Length) {

                if (arr.IsCreated == true) arr.Dispose();
                arr = new NativeArrayBurst<T>(fromArr.Length, Unity.Collections.Allocator.Persistent);
                
            }

            UnsafeUtility.MemCpy((void*) ((System.IntPtr) arr.GetUnsafePtr() + dstIndex * UnsafeUtility.SizeOf<T>()), (void*) ((System.IntPtr) fromArr.GetUnsafePtr() + srcIndex * UnsafeUtility.SizeOf<T>()), (long) (length * UnsafeUtility.SizeOf<T>()));
            
        }
        #endif

    }

}