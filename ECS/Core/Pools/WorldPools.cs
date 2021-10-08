
namespace ME.ECS {

    public partial class World {

        private IPoolImplementation prevPools;
        private IPoolImplementation currentPools;

        partial void InitializePools() {

            this.prevPools = Pools.current;
            this.currentPools = new PoolImplementation(isNull: false);
            Pools.current = this.currentPools;
            
        }

        partial void DeInitializePools() {

            if (this.currentPools != null) {
                this.currentPools.Clear();
                Pools.current = this.prevPools;
            }

            foreach (var pool in ME.ECS.Buffers.ArrayPools.pools) {
                
                pool.Clear();
                
            }
            
            this.prevPools = null;
            this.currentPools = null;

        }

    }

}