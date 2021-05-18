namespace ME.ECS {

    using System.Collections.Generic;
    using ME.ECS.Collections;

    #if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class PoolDictionaryCopyable<TKey, TValue> {

        private static IEqualityComparer<TKey> customComparer;
        private static int capacity;
        private static PoolInternalBase pool = new PoolInternalBase(typeof(DictionaryCopyable<TKey, TValue>), () => new DictionaryCopyable<TKey, TValue>(PoolDictionaryCopyable<TKey, TValue>.capacity, PoolDictionaryCopyable<TKey, TValue>.customComparer), (x) => ((DictionaryCopyable<TKey, TValue>)x).Clear());

        public static DictionaryCopyable<TKey, TValue> Spawn(int capacity, IEqualityComparer<TKey> customComparer = null) {

            PoolDictionaryCopyable<TKey, TValue>.capacity = capacity;
            PoolDictionaryCopyable<TKey, TValue>.customComparer = customComparer;
            return (DictionaryCopyable<TKey, TValue>)PoolDictionaryCopyable<TKey, TValue>.pool.Spawn();
		    
        }

        public static void Recycle(ref DictionaryCopyable<TKey, TValue> dic) {

            PoolDictionaryCopyable<TKey, TValue>.pool.Recycle(dic);
            dic = null;

        }

        public static void Recycle(DictionaryCopyable<TKey, TValue> dic) {

            PoolDictionaryCopyable<TKey, TValue>.pool.Recycle(dic);

        }

    }

}