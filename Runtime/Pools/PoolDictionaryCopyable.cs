namespace ME.ECS {

    using System.Collections.Generic;
    using ME.ECS.Collections;

    #if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class PoolDictionaryCopyable<TKey, TValue> {

        public struct Data {

            public IEqualityComparer<TKey> customComparer;
            public int capacity;

        }

        public static DictionaryCopyable<TKey, TValue> Spawn(int capacity, IEqualityComparer<TKey> customComparer = null) {

            return Pools.current.PoolSpawn(new Data() {
                capacity = capacity,
                customComparer = customComparer,
            }, (data) => new DictionaryCopyable<TKey, TValue>(data.capacity, data.customComparer), x => x.Clear());
			
        }

        public static void Recycle(ref DictionaryCopyable<TKey, TValue> dic) {

            Pools.current.PoolRecycle(ref dic);
			
        }

    }

}