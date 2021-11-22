namespace ME.ECS {
    
    using ME.ECS.Collections;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class PoolRefList<TValue> {

        public static RefList<TValue> Spawn(int capacity) {

            return Pools.current.PoolSpawn(capacity, (c) => new RefList<TValue>(c), (x) => ((RefList<TValue>)x).Clear());
			
        }

        public static void Recycle(ref RefList<TValue> dic) {

            Pools.current.PoolRecycle(ref dic);
			
        }

    }

}