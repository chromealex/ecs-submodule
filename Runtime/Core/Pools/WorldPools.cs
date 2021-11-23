
namespace ME.ECS {

    public partial class World {

        private IPoolImplementation prevPools;
        private IPoolImplementation currentPools;
        private IPoolImplementation currentThreadPools;

        partial void InitializePools() {

            this.prevPools = Pools.current;
            this.currentPools = new PoolImplementation(isNull: false);
            this.currentThreadPools = new PoolImplementationThread(isNull: false);
            Pools.current = this.currentPools;
            
        }

        partial void DeInitializePools() {

            if (this.currentPools != null) {
                this.currentPools.Clear();
                Pools.current = this.prevPools;
            }
            
            if (this.currentThreadPools != null) {
                this.currentThreadPools.Clear();
            }
            
            foreach (var pool in ME.ECS.Buffers.ArrayPools.pools) {
                
                pool.Clear();
                
            }
            ME.ECS.Buffers.ArrayPools.pools.Clear();

            this.currentThreadPools = null;
            this.prevPools = null;
            this.currentPools = null;

        }

    }

}