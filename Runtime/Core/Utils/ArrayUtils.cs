#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Collections;

    public interface IArrayElementCopy<T> {

        void Copy(in T from, ref T to);
        void Recycle(ref T item);

    }

    public interface IArrayElementCopyUnmanaged<T> {

        void Copy(ref ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator, in T from, ref T to);
        void Recycle(ref ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator, ref T item);

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static partial class ArrayUtils {

        [return: Unity.Burst.CompilerServices.AssumeRangeAttribute(0, int.MaxValue)]
        public static int AssumePositive(int value) {
            return value;
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Clear(System.Array arr) {

            if (arr != null) System.Array.Clear(arr, 0, arr.Length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Clear<T>(BufferArray<T> arr) {

            if (arr.arr != null) System.Array.Clear(arr.arr, 0, arr.Length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle<T, TCopy>(ref ListCopyable<T> item, TCopy copy) where TCopy : IArrayElementCopy<T> {

            if (item != null) {

                for (int i = 0; i < item.Count; ++i) {

                    copy.Recycle(ref item[i]);

                }

                PoolListCopyable<T>.Recycle(ref item);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle<T, TCopy>(ref HashSetCopyable<T> list, TCopy copy) where TCopy : IArrayElementCopy<T> {

            if (list != null) {

                foreach (var item in list) {

                    var val = item;
                    copy.Recycle(ref val);

                }

                PoolHashSetCopyable<T>.Recycle(ref list);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle<T, TCopy>(ref System.Collections.Generic.List<T> item, TCopy copy) where TCopy : IArrayElementCopy<T> {

            if (item != null) {

                for (int i = 0; i < item.Count; ++i) {

                    var val = item[i];
                    copy.Recycle(ref val);

                }

                PoolList<T>.Recycle(ref item);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle<T, TCopy>(ref DictionaryInt<T> list, TCopy copy) where TCopy : IArrayElementCopy<T> {

            if (list != null) {

                foreach (var item in list) {

                    var val = item.Value;
                    copy.Recycle(ref val);
                    
                }
                
                PoolDictionaryInt<T>.Recycle(ref list);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle<T, TCopy>(ref ME.ECS.Collections.BufferArray<T> item, TCopy copy) where TCopy : IArrayElementCopy<T> {

            for (int i = 0; i < item.Length; ++i) {

                copy.Recycle(ref item.arr[i]);

            }

            PoolArray<T>.Recycle(ref item);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(System.Collections.Generic.List<T> fromArr, ref System.Collections.Generic.List<T> arr, TCopy copy) where TCopy : IArrayElementCopy<T> {

            if (fromArr == null) {

                if (arr != null) {

                    for (int i = 0; i < arr.Count; ++i) {

                        var val = arr[i];
                        copy.Recycle(ref val);

                    }

                    PoolList<T>.Recycle(ref arr);

                }

                arr = null;
                return;

            }

            if (arr == null || fromArr.Count != arr.Count) {

                if (arr != null) ArrayUtils.Recycle(ref arr, copy);
                arr = PoolList<T>.Spawn(fromArr.Count);

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

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(System.Collections.Generic.List<T> fromArr, ref System.Collections.Generic.List<T> arr) where T : struct {

            if (fromArr == null) {

                if (arr != null) {

                    PoolList<T>.Recycle(ref arr);

                }

                arr = null;
                return;

            }

            if (arr == null || fromArr.Count != arr.Count) {

                if (arr != null) PoolList<T>.Recycle(ref arr);
                arr = PoolList<T>.Spawn(fromArr.Count);

            }

            var cnt = arr.Count;
            for (int i = 0; i < fromArr.Count; ++i) {

                var isDefault = i >= cnt;
                var item = fromArr[i];
                if (isDefault == true) {

                    arr.Add(item);

                } else {

                    arr[i] = item;

                }

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(HashSetCopyable<T> fromArr, ref HashSetCopyable<T> arr) where T : struct {

            if (fromArr == null) {

                if (arr != null) {

                    PoolHashSetCopyable<T>.Recycle(ref arr);

                }

                arr = null;
                return;

            }

            if (arr == null) {

                arr = PoolHashSetCopyable<T>.Spawn();

            }

            arr.CopyFrom(fromArr);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(HashSetCopyable<T> fromArr, ref HashSetCopyable<T> arr, TCopy copy) where T : struct where TCopy : IArrayElementCopy<T> {

            if (fromArr == null) {

                if (arr != null) {

                    arr.Clear(copy);
                    PoolHashSetCopyable<T>.Recycle(ref arr);

                }

                arr = null;
                return;

            }

            if (arr == null) {

                arr = PoolHashSetCopyable<T>.Spawn();

            }

            arr.CopyFrom(fromArr, copy);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<TKey, T>(DictionaryCopyable<TKey, T> fromDic, ref DictionaryCopyable<TKey, T> dic) {
            
            if (fromDic == null) {
            
                if (dic != null) {
                
                    PoolDictionaryCopyable<TKey, T>.Recycle(ref dic);
                    
                }

                dic = null;
                return;
                
            }

            if (dic == null || fromDic.Count != dic.Count) {
            
                if (dic != null) {
                
                    PoolDictionaryCopyable<TKey, T>.Recycle(ref dic);
                }

                dic = PoolDictionaryCopyable<TKey, T>.Spawn(fromDic.Count);
                
            }

            dic.CopyFrom(fromDic);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<TKey, T, TCopy>(DictionaryCopyable<TKey, T> fromDic, ref DictionaryCopyable<TKey, T> dic, TCopy copy) where TCopy : IArrayElementCopy<T> {
            
            if (fromDic == null) {
            
                if (dic != null) {
                
                    foreach (var kv in dic) {
                        
                        var val = kv.Value;
                        copy.Recycle(ref val);
                        
                    }

                    PoolDictionaryCopyable<TKey, T>.Recycle(ref dic);
                    
                }

                dic = null;
                return;
                
            }

            if (dic == null || fromDic.Count != dic.Count) {
            
                if (dic != null) {
                
                    foreach (var kv in dic) {
                    
                        var val = kv.Value;
                        copy.Recycle(ref val);
                        
                    }

                    PoolDictionaryCopyable<TKey, T>.Recycle(ref dic);
                }

                dic = PoolDictionaryCopyable<TKey, T>.Spawn(fromDic.Count);
                
            }

            dic.CopyFrom(fromDic, copy);
            
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(DictionaryInt<T> fromDic, ref DictionaryInt<T> dic) {
            
            if (fromDic == null) {
            
                if (dic != null) {
                
                    PoolDictionaryInt<T>.Recycle(ref dic);
                    
                }

                dic = null;
                return;
                
            }

            if (dic == null || fromDic.Count != dic.Count) {
            
                if (dic != null) {
                
                    PoolDictionaryInt<T>.Recycle(ref dic);
                }

                dic = PoolDictionaryInt<T>.Spawn(fromDic.Count);
                
            }

            dic.CopyFrom(fromDic);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(DictionaryInt<T> fromDic, ref DictionaryInt<T> dic, TCopy copy) where TCopy : IArrayElementCopy<T> {
            
            if (fromDic == null) {
            
                if (dic != null) {
                
                    foreach (var kv in dic) {
                        
                        var val = kv.Value;
                        copy.Recycle(ref val);
                        
                    }

                    PoolDictionaryInt<T>.Recycle(ref dic);
                    
                }

                dic = null;
                return;
                
            }

            if (dic == null || fromDic.Count != dic.Count) {
            
                if (dic != null) {
                
                    foreach (var kv in dic) {
                    
                        var val = kv.Value;
                        copy.Recycle(ref val);
                        
                    }

                    PoolDictionaryInt<T>.Recycle(ref dic);
                }

                dic = PoolDictionaryInt<T>.Spawn(fromDic.Count);
                
            }

            dic.CopyFrom(fromDic, copy);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(DictionaryULong<T> fromDic, ref DictionaryULong<T> dic) {
            
            if (fromDic == null) {
            
                if (dic != null) {
                
                    PoolDictionaryULong<T>.Recycle(ref dic);
                    
                }

                dic = null;
                return;
                
            }

            if (dic == null || fromDic.Count != dic.Count) {
            
                if (dic != null) {
                
                    PoolDictionaryULong<T>.Recycle(ref dic);
                }

                dic = PoolDictionaryULong<T>.Spawn(fromDic.Count);
                
            }

            dic.CopyFrom(fromDic);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(DictionaryULong<T> fromDic, ref DictionaryULong<T> dic, TCopy copy) where TCopy : IArrayElementCopy<T> {
            
            if (fromDic == null) {
            
                if (dic != null) {
                
                    foreach (var kv in dic) {

                        var val = kv.Value;
                        copy.Recycle(ref val);
                        
                    }

                    PoolDictionaryULong<T>.Recycle(ref dic);
                    
                }

                dic = null;
                return;
                
            }

            if (dic == null || fromDic.Count != dic.Count) {
            
                if (dic != null) {
                
                    foreach (var kv in dic) {
                    
                        var val = kv.Value;
                        copy.Recycle(ref val);
                        
                    }

                    PoolDictionaryULong<T>.Recycle(ref dic);
                }

                dic = PoolDictionaryULong<T>.Spawn(fromDic.Count);
                
            }

            dic.CopyFrom(fromDic, copy);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(BufferArraySliced<T> fromArr, ref BufferArraySliced<T> arr, TCopy copy)
            where TCopy : IArrayElementCopy<T> where T : struct {

            arr = arr.CopyFrom(in fromArr, copy);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in ME.ECS.Collections.BufferArraySliced<T> fromArr, ref ME.ECS.Collections.BufferArraySliced<T> arr) where T : struct {

            arr = arr.CopyFrom(in fromArr);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in ME.ECS.Collections.BufferArray<T> fromArr, int sourceIndex, ref ME.ECS.Collections.BufferArray<T> arr, int destIndex, int length) {

            if (fromArr.arr == null) {

                if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                arr = BufferArray<T>.Empty;
                return;

            }

            if (arr.arr == null) {

                if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                arr = PoolArray<T>.Spawn(destIndex + length);

            }

            System.Array.Copy(fromArr.arr, sourceIndex, arr.arr, destIndex, length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in T[] fromArr, int sourceIndex, ref ME.ECS.Collections.BufferArray<T> arr, int destIndex, int length) {

            if (fromArr == null) {

                if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                arr = BufferArray<T>.Empty;
                return;

            }

            if (arr.arr == null) {

                if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                arr = PoolArray<T>.Spawn(destIndex + length);

            }

            System.Array.Copy(fromArr, sourceIndex, arr.arr, destIndex, length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(System.Collections.Generic.IList<T> fromArr, ref ME.ECS.Collections.BufferArray<T> arr) {

            if (fromArr == null) {

                if (arr != null) PoolArray<T>.Recycle(ref arr);
                arr = new BufferArray<T>(null, 0);
                return;

            }

            if (arr.arr == null || fromArr.Count != arr.Length) {

                if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                arr = PoolArray<T>.Spawn(fromArr.Count);

            }

            fromArr.CopyTo(arr.arr, 0);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(T[] fromArr, ref T[] arr) {

            if (fromArr == null) {

                arr = null;
                return;

            }

            if (arr == null || fromArr.Length != arr.Length) {

                arr = new T[fromArr.Length];

            }

            fromArr.CopyTo(arr, 0);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(T[] fromArr, ref T[] arr, TCopy copy) where TCopy : IArrayElementCopy<T> {

            if (fromArr == null) {

                arr = null;
                return;

            }

            if (arr == null || fromArr.Length != arr.Length) {

                arr = new T[fromArr.Length];

            }

            for (int i = 0; i < fromArr.Length; ++i) {
                copy.Copy(fromArr[i], ref arr[i]);
            }
            
        }

    }

}