namespace ME.ECS {
    
    using ME.ECS.Collections;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class PoolPriorityQueue<TValue> {

        public static PriorityQueue<TValue> Spawn(int capacity) {

            return Pools.current.PoolSpawn(capacity, (c) => new PriorityQueue<TValue>(c), (x) => ((PriorityQueue<TValue>)x).Clear());
			
        }

        public static void Recycle(ref PriorityQueue<TValue> dic) {

            Pools.current.PoolRecycle(ref dic);
			
        }

    }

}