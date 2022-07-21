namespace ME.ECS {
    
    public static class PoolClass<T> where T : class, new() {

        public struct Data {

        }

        public static T Spawn() {

            return Pools.current.PoolSpawn(new Data(), (data) => new T(), null);
			
        }

        public static void Recycle(ref T system) {

            Pools.current.PoolRecycle(ref system);
			
        }

        public static void Recycle(T system) {

            Pools.current.PoolRecycle(ref system);
			
        }

    }
    
}