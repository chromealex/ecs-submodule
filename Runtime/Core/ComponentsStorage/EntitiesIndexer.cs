namespace ME.ECS {
    
    using Collections;

    public struct EntitiesIndexer {

        public struct CopyItem : IArrayElementCopy<HashSetCopyable<int>> {
        
            public void Copy(HashSetCopyable<int> @from, ref HashSetCopyable<int> to) {
            
                ArrayUtils.Copy(from, ref to);
            
            }

            public void Recycle(HashSetCopyable<int> item) {
            
                PoolHashSetCopyable<int>.Recycle(ref item);
            
            }

        }

        [ME.ECS.Serializer.SerializeField]
        private BufferArray<HashSetCopyable<int>> data;

        public void Initialize(int capacity) {

            if (this.data.isCreated == false) this.data = PoolArray<HashSetCopyable<int>>.Spawn(capacity);

        }

        public void Validate(int entityId) {

            ArrayUtils.Resize(entityId, ref this.data);

        }

        public int GetCount(int entityId) {

            var arr = this.data.arr[entityId];
            if (arr == null) return 0;
            
            return arr.Count;

        }

        public bool Has(int entityId, int componentId) {

            var arr = this.data.arr[entityId];
            if (arr == null) return false;

            return arr.Contains(componentId);

        }
        
        public HashSetCopyable<int> Get(int entityId) {

            return this.data[entityId];

        }

        public void Set(int entityId, int componentId) {

            ref var item = ref this.data[entityId];
            if (item == null) item = PoolHashSetCopyable<int>.Spawn(64);
            item.Add(componentId);

        }

        public void Remove(int entityId, int componentId) {
            
            ref var item = ref this.data[entityId];
            if (item != null) item.Remove(componentId);
            
        }

        public void RemoveAll(int entityId) {
            
            ref var item = ref this.data[entityId];
            if (item != null) item.Clear();
            
        }

        public void CopyFrom(in EntitiesIndexer other) {
            
            ArrayUtils.Copy(other.data, ref this.data, new CopyItem());
            
        }

        public void Recycle() {
            
            ArrayUtils.Recycle(ref this.data, new CopyItem());
            
        }

    }

}