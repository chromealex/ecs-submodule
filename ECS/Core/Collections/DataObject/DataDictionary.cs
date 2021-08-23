#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS.Collections {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct DataDictionaryProvider<TKey, TValue> : IDataObjectProvider<DictionaryCopyable<TKey, TValue>> {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Clone(DictionaryCopyable<TKey, TValue> from, ref DictionaryCopyable<TKey, TValue> to) {

            to = PoolDictionaryCopyable<TKey, TValue>.Spawn(from.Count);
            ArrayUtils.Copy(from, ref to);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Recycle(ref DictionaryCopyable<TKey, TValue> value) {

            if (value != null) {
                PoolDictionaryCopyable<TKey, TValue>.Recycle(ref value);
            }

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [GeneratorIgnoreManagedType]
    public struct DataDictionary<TKey, TValue> : IDataObject<DictionaryCopyable<TKey, TValue>> {

        [ME.ECS.Serializer.SerializeField]
        private DataObject<DictionaryCopyable<TKey, TValue>, DataDictionaryProvider<TKey, TValue>> dataObject;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsCreated() => this.dataObject.IsCreated();

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public DataDictionary(int capacity = 0) : this(PoolDictionaryCopyable<TKey, TValue>.Spawn(capacity)) {}

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public DataDictionary(DictionaryCopyable<TKey, TValue> data) {

            this.dataObject = new DataObject<DictionaryCopyable<TKey, TValue>, DataDictionaryProvider<TKey, TValue>>(data);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref readonly DictionaryCopyable<TKey, TValue> Read() {

            return ref this.dataObject.Read();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref DictionaryCopyable<TKey, TValue> Get() {

            return ref this.dataObject.Get();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(DictionaryCopyable<TKey, TValue> value) {

            this.dataObject.Set(value);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {

            this.dataObject.Dispose();

        }

    }

}
