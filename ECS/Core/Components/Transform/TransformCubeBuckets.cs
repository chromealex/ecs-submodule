namespace ME.ECS.Transform {

    using Unity.Mathematics;
    using Unity.Collections;

    public struct TransformCubeBuckets {

        /// <summary>
        /// Key   = Value Hash
        /// Value = Entity Id
        /// </summary>
        private NativeMultiHashMap<int, int> data;
        /// <summary>
        /// Key   = Entity Id
        /// Value = Value Hash
        /// </summary>
        private NativeHashMap<int, int> contains;

        /// <summary>
        /// Block size for storing cubes
        /// </summary>
        public float blockSize;

        public void SetBlockSize(float value) {

            this.blockSize = value;

        }

        public NativeMultiHashMap<int, int>.Enumerator GetInRange(float3 position, float range, NativeArray<Entity> cache) {

            var rangeSqr = range * range;
            var hash = position.GetHash(this.blockSize);
            var enumerator = this.data.GetValuesForKey(hash);
            foreach (var entityId in enumerator) {

                var pos = (float3)cache[entityId].GetPosition();
                if (math.distancesq(pos, position) <= rangeSqr) {
                    
                }

            }
            
            return default;

        }
        
        /// <summary>
        /// Called on data update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="prevValue"></param>
        /// <param name="newValue"></param>
        public void UpdateData(in Entity entity, in float3 prevValue, in float3 newValue) {

            var newHash = newValue.GetHash(this.blockSize);
            if (this.contains.ContainsKey(entity.id) == true) {
                this.data.Remove(prevValue.GetHash(this.blockSize), entity.id);
                this.contains[entity.id] = newHash;
            } else {
                this.contains.Add(entity.id, newHash);
            }

            this.data.Add(newHash, entity.id);
            
        }

        /// <summary>
        /// Called on component remove
        /// </summary>
        /// <param name="entity"></param>
        public void OnRemoveData(in Entity entity) {

            this.contains.Remove(entity.id);
            
        }

        /// <summary>
        /// Called on entity destroy
        /// </summary>
        /// <param name="entity"></param>
        public void OnDestroyEntity(in Entity entity) {

            if (this.contains.TryGetValue(entity.id, out var key) == true) {

                this.data.Remove(key);
                this.contains.Remove(entity.id);

            }
            
        }

    }

    public static class Utils {
        
        public static int GetHash(this in float3 value, float blockSize) {

            return (int)(value.x / blockSize) ^ (int)(value.y / blockSize) ^ (int)(value.z / blockSize);

        }

    }

}