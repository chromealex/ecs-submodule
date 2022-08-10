namespace ME.ECS.GlobalEvents {
    
    using MemPtr = System.Int64;
    using Collections.MemoryAllocator;
    using ME.ECS.Collections.V3;

    public struct GlobalEventStorage : IPlugin {

        public static int key;
        
        public struct GlobalEventFrameItem {

            public uint globalEvent;
            public Entity data;

        }

        public List<GlobalEventFrameItem> globalEventLogicItems;
        public HashSet<long> globalEventLogicEvents;

        public void Initialize(int key, ref MemoryAllocator allocator) {

            GlobalEventStorage.key = GlobalEventStorage.key;
            
            if (this.globalEventLogicItems.isCreated == false) this.globalEventLogicItems = new List<GlobalEventFrameItem>(ref allocator, 10);
            if (this.globalEventLogicEvents.isCreated == false) this.globalEventLogicEvents = new HashSet<long>(ref allocator, 10);

        }

        public bool Remove(ref MemoryAllocator allocator, GlobalEvent globalEvent, in Entity entity) {
            
            var id = globalEvent.id;
            if (id <= 0u) return false;

            var key = MathUtils.GetKey(globalEvent.GetHashCode(), entity.GetHashCode());
            if (this.globalEventLogicEvents.Contains(in allocator, key) == true) {

                for (int i = 0; i < this.globalEventLogicItems.Count; ++i) {

                    var item = this.globalEventLogicItems[in allocator, i];
                    if (item.globalEvent == id && item.data == entity) {

                        this.globalEventLogicEvents.Remove(ref allocator, key);
                        this.globalEventLogicItems.RemoveAt(ref allocator, i);
                        return true;

                    }

                }

            }
            
            return false;

        }

        public bool Add(ref MemoryAllocator allocator, GlobalEvent globalEvent, in Entity entity) {

            var id = globalEvent.id;
            if (id <= 0u) return false;
            
            var key = MathUtils.GetKey(globalEvent.GetHashCode(), entity.GetHashCode());
            if (this.globalEventLogicEvents.Contains(in allocator, key) == false) {

                this.globalEventLogicEvents.Add(ref allocator, key);
                this.globalEventLogicItems.Add(ref allocator, new GlobalEventFrameItem() {
                    globalEvent = id,
                    data = entity,
                });
                
                return true;

            }
            
            return false;

        }

    }

}