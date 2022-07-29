namespace ME.ECS {
    
    using Collections.V3;
    using Collections.MemoryAllocator;

    public struct EntitiesIndexer {

        [ME.ECS.Serializer.SerializeField]
        private ME.ECS.Collections.V3.MemArrayAllocator<ME.ECS.Collections.MemoryAllocator.HashSet<int>> data;

        internal void Initialize(ref MemoryAllocator allocator, int capacity) {

            if (this.data.isCreated == false) this.data = new ME.ECS.Collections.V3.MemArrayAllocator<ME.ECS.Collections.MemoryAllocator.HashSet<int>>(ref allocator, capacity);

        }

        internal void Validate(ref MemoryAllocator allocator, int entityId) {

            this.data.Resize(ref allocator, entityId + 1);

        }

        public readonly int GetCount(in MemoryAllocator allocator, int entityId) {

            var arr = this.data[in allocator, entityId];
            if (arr.isCreated == false) return 0;
            
            return arr.Count(in allocator);

        }

        public readonly bool Has(in MemoryAllocator allocator, int entityId, int componentId) {

            var arr = this.data[in allocator, entityId];
            if (arr.isCreated == false) return false;

            return arr.Contains(in allocator, componentId);

        }
        
        public readonly ref HashSet<int> Get(in MemoryAllocator allocator, int entityId) {

            return ref this.data[in allocator, entityId];

        }

        internal void Set(ref MemoryAllocator allocator, int entityId, int componentId) {

            ref var item = ref this.data[in allocator, entityId];
            if (item.isCreated == false) item = new HashSet<int>(ref allocator, 64);
            item.Add(ref allocator, componentId);

        }

        internal void Remove(ref MemoryAllocator allocator, int entityId, int componentId) {
            
            ref var item = ref this.data[in allocator, entityId];
            if (item.isCreated == true) item.Remove(ref allocator, componentId);
            
        }

        internal void RemoveAll(ref MemoryAllocator allocator, int entityId) {
            
            ref var item = ref this.data[in allocator, entityId];
            if (item.isCreated == true) item.Clear(in allocator);
            
        }

        internal void Dispose(ref MemoryAllocator allocator) {

            for (int i = 0; i < this.data.Length; ++i) {

                ref var set = ref this.data[in allocator, i];
                if (set.isCreated == true) {
                    
                    set.Dispose(ref allocator);
                    
                }

            }
            
            this.data.Dispose(ref allocator);
            
        }

    }

}