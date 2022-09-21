namespace ME.ECS {

    using System.Collections.Generic;
    using ME.ECS.Collections;

    #if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class PoolDictionaryULong<TValue> {

        public struct Data {

            public int capacity;

        }

        public static DictionaryULong<TValue> Spawn(int capacity) {

            return Pools.current.PoolSpawn(new Data() {
                capacity = capacity,
            }, (data) => new DictionaryULong<TValue>(data.capacity), x => x.Clear());
			
        }

        public static void Recycle(ref DictionaryULong<TValue> dic) {

            Pools.current.PoolRecycle(ref dic);
			
        }

    }

}