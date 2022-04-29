namespace ME.ECS {
    
    using Collections;

    public struct EntitiesIndexer {

        [ME.ECS.Serializer.SerializeField]
        private HashSetCopyable<System.Collections.Generic.KeyValuePair<int, int>> data;
        [ME.ECS.Serializer.SerializeField]
        private HashSetCopyable<long> index;

        public void Initialize(int capacity) {

            if (this.data == null) this.data = PoolHashSetCopyable<System.Collections.Generic.KeyValuePair<int, int>>.Spawn(capacity);
            if (this.index == null) this.index = PoolHashSetCopyable<long>.Spawn(capacity);

        }

        public void Validate(int capacity) {

            //ArrayUtils.Resize(capacity + 1, ref this.data);
            
        }

        public bool Has(int entityId, int componentId) {

            var key = MathUtils.GetKey(entityId, componentId);
            return this.index.Contains(key);

        }

        public HashSetCopyable<System.Collections.Generic.KeyValuePair<int, int>> Get() {

            return this.data;

        }

        public void Set(int entityId, int componentId) {

            var key = MathUtils.GetKey(entityId, componentId);
            if (this.index.Add(key) == true) {
                
                this.data.Add(new System.Collections.Generic.KeyValuePair<int, int>(entityId, componentId));
                
            }

        }

        public void Remove(int entityId, int componentId) {
            
            var key = MathUtils.GetKey(entityId, componentId);
            if (this.index.Remove(key) == true) {

                this.data.RemoveWhere(entityId, (e, kv) => kv.Key == e);
                
            }

        }

        public void CopyFrom(in EntitiesIndexer other) {
            
            ArrayUtils.Copy(other.data, ref this.data);
            ArrayUtils.Copy(other.index, ref this.index);
            
        }

        public void Recycle() {
            
            PoolHashSetCopyable<System.Collections.Generic.KeyValuePair<int, int>>.Recycle(ref this.data);
            PoolHashSetCopyable<long>.Recycle(ref this.index);
            
        }

    }

    public struct CopyItem : IArrayElementCopy<HashSetCopyable<int>> {
        
        public void Copy(HashSetCopyable<int> @from, ref HashSetCopyable<int> to) {
            
            ArrayUtils.Copy(from, ref to);
            
        }

        public void Recycle(HashSetCopyable<int> item) {
            
            PoolHashSetCopyable<int>.Recycle(ref item);
            
        }

    }

}