namespace ME.ECS {
    
    using Collections;

    public struct EntitiesIndexer {

        [ME.ECS.Serializer.SerializeField]
        private BufferArray<HashSetCopyable<int>> data;

        public void Initialize(int capacity) {

            if (this.data.isCreated == false) this.data = PoolArray<HashSetCopyable<int>>.Spawn(capacity);

        }

        public void Validate(int capacity) {

            ArrayUtils.Resize(capacity + 1, ref this.data);

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

    public struct CopyItem : IArrayElementCopy<HashSetCopyable<int>> {
        
        public void Copy(HashSetCopyable<int> @from, ref HashSetCopyable<int> to) {
            
            ArrayUtils.Copy(from, ref to);
            
        }

        public void Recycle(HashSetCopyable<int> item) {
            
            PoolHashSetCopyable<int>.Recycle(ref item);
            
        }

    }

}