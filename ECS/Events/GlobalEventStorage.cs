namespace ME.ECS {
    
    using Collections;

    public struct GlobalEventStorage {

        public struct GlobalEventFrameItem {

            public uint globalEvent;
            public Entity data;

        }

        public ListCopyable<GlobalEventFrameItem> globalEventLogicItems;
        public HashSetCopyable<long> globalEventLogicEvents;

        public void Initialize() {
            
            this.globalEventLogicItems = PoolListCopyable<GlobalEventFrameItem>.Spawn(10);
            this.globalEventLogicEvents = PoolHashSetCopyable<long>.Spawn();

        }

        public void DeInitialize() {

            PoolListCopyable<GlobalEventFrameItem>.Recycle(ref this.globalEventLogicItems);
            PoolHashSetCopyable<long>.Recycle(ref this.globalEventLogicEvents);

        }

        public bool Remove(GlobalEvent globalEvent, in Entity entity) {
            
            var id = globalEvent.id;
            if (id <= 0u) return false;

            var key = MathUtils.GetKey(globalEvent.GetHashCode(), entity.GetHashCode());
            if (this.globalEventLogicEvents.Contains(key) == true) {

                for (int i = 0; i < this.globalEventLogicItems.Count; ++i) {

                    var item = this.globalEventLogicItems[i];
                    if (item.globalEvent == id && item.data == entity) {

                        this.globalEventLogicEvents.Remove(key);
                        this.globalEventLogicItems.RemoveAt(i);
                        return true;

                    }

                }

            }
            
            return false;

        }

        public bool Add(GlobalEvent globalEvent, in Entity entity) {

            var id = globalEvent.id;
            if (id <= 0u) return false;
            
            var key = MathUtils.GetKey(globalEvent.GetHashCode(), entity.GetHashCode());
            if (this.globalEventLogicEvents.Contains(key) == false) {

                this.globalEventLogicEvents.Add(key);
                this.globalEventLogicItems.Add(new GlobalEventFrameItem() {
                    globalEvent = id,
                    data = entity,
                });
                
                return true;

            }
            
            return false;

        }

        public void CopyFrom(in GlobalEventStorage other) {
            
            ArrayUtils.Copy(other.globalEventLogicItems, ref this.globalEventLogicItems);
            ArrayUtils.Copy(other.globalEventLogicEvents, ref this.globalEventLogicEvents);
            
        }
        
    }

}