namespace ME.ECS {

    using ME.ECS.Collections;
    
    public class PoolDisposeSentinel<T, TProvider> where TProvider : struct, ME.ECS.Collections.IDataObjectProvider<T> {

        public static IPoolImplementation current = new PoolImplementation(isNull: true);
        
        public struct Data {}
        
        public static DisposeSentinel<T, TProvider> Spawn() {

            return current.PoolSpawn(new Data(), (data) => new DisposeSentinel<T, TProvider>(), null);
			
        }

        public static void Recycle(ref DisposeSentinel<T, TProvider> system) {

            current.PoolRecycle(ref system);
			
        }

        public static void Recycle(DisposeSentinel<T, TProvider> system) {

            current.PoolRecycle(ref system);
			
        }

    }

}