namespace ME.ECS {
    
    using ME.ECS.Collections;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class PoolPriorityQueue<TValue> {

        private static int capacity;
        private static PoolInternalBase pool = new PoolInternalBase(typeof(PriorityQueue<TValue>), () => new PriorityQueue<TValue>(PoolPriorityQueue<TValue>.capacity), (x) => ((PriorityQueue<TValue>)x).Clear());

        public static PriorityQueue<TValue> Spawn(int capacity) {

            PoolPriorityQueue<TValue>.capacity = capacity;
            return (PriorityQueue<TValue>)PoolPriorityQueue<TValue>.pool.Spawn();
		    
        }

        public static void Recycle(ref PriorityQueue<TValue> dic) {

            PoolPriorityQueue<TValue>.pool.Recycle(dic);
            dic = null;

        }

        public static void Recycle(PriorityQueue<TValue> dic) {

            PoolPriorityQueue<TValue>.pool.Recycle(dic);

        }

    }

}