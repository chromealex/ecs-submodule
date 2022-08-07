namespace ME.ECS {
    
    using MemPtr = System.Int64;
    using ME.ECS.Collections.V3;
    using ME.ECS.Collections.MemoryAllocator;

    public partial class World {
        
        #region GlobalEvents
        public enum GlobalEventType : byte {

            Logic,
            Visual,

        }

        private System.Collections.Generic.List<GlobalEventStorage.GlobalEventFrameItem> globalEventFrameItems;
        private HashSet<long> globalEventFrameEvents;

        internal void InitializeGlobalEvents() {

            this.globalEventFrameItems = PoolList<GlobalEventStorage.GlobalEventFrameItem>.Spawn(10);
            this.globalEventFrameEvents = new HashSet<long>(ref this.tempAllocator, 10);

        }

        internal void DisposeGlobalEvents() {

            GlobalEvent.ResetCache();
            
            PoolList<GlobalEventStorage.GlobalEventFrameItem>.Recycle(ref this.globalEventFrameItems);
            this.globalEventFrameEvents.Dispose(ref this.tempAllocator);
            
        }

        public void ProcessGlobalEvents(GlobalEventType globalEventType) {

            if (globalEventType == GlobalEventType.Visual) {

                try {

                    for (int i = 0; i < this.globalEventFrameItems.Count; ++i) {

                        var item = this.globalEventFrameItems[i];
                        GlobalEvent.GetEventById(item.globalEvent).Run(in item.data);

                    }

                } catch (System.Exception ex) {

                    UnityEngine.Debug.LogException(ex);

                }

                this.globalEventFrameItems.Clear();
                this.globalEventFrameEvents.Clear(in this.tempAllocator);

            } else if (globalEventType == GlobalEventType.Logic) {

                for (int i = 0; i < this.currentState.globalEvents.globalEventLogicItems.Count; ++i) {

                    var item = this.currentState.globalEvents.globalEventLogicItems[in this.currentState.allocator, i];
                    GlobalEvent.GetEventById(item.globalEvent).Run(in item.data);

                }

                this.currentState.globalEvents.globalEventLogicItems.Clear(in this.currentState.allocator);
                this.currentState.globalEvents.globalEventLogicEvents.Clear(in this.currentState.allocator);

            }

        }

        public bool CancelGlobalEvent(GlobalEvent globalEvent, in Entity entity, GlobalEventType globalEventType) {

            var key = MathUtils.GetKey(globalEvent.GetHashCode(), entity.GetHashCode());
            if (globalEventType == GlobalEventType.Visual) {

                if (this.globalEventFrameEvents.Contains(in this.tempAllocator, key) == true) {

                    for (int i = 0; i < this.globalEventFrameItems.Count; ++i) {

                        var item = this.globalEventFrameItems[i];
                        if (item.globalEvent == globalEvent.id && item.data == entity) {

                            this.globalEventFrameEvents.Remove(ref this.tempAllocator, key);
                            this.globalEventFrameItems.RemoveAt(i);
                            return true;

                        }

                    }

                }

            } else if (globalEventType == GlobalEventType.Logic) {

                this.currentState.globalEvents.Remove(ref this.currentState.allocator, globalEvent, in entity);
                
            }

            return false;

        }

        public void RegisterGlobalEvent(GlobalEvent globalEvent, in Entity entity, GlobalEventType globalEventType) {

            var key = MathUtils.GetKey(globalEvent.GetHashCode(), entity.GetHashCode());
            if (globalEventType == GlobalEventType.Visual) {

                if (this.globalEventFrameEvents.Contains(in this.tempAllocator, key) == false) {

                    this.globalEventFrameEvents.Add(ref this.tempAllocator, key);
                    this.globalEventFrameItems.Add(new GlobalEventStorage.GlobalEventFrameItem() {
                        globalEvent = globalEvent.id,
                        data = entity,
                    });

                }

            } else if (globalEventType == GlobalEventType.Logic) {

                this.currentState.globalEvents.Add(ref this.currentState.allocator, globalEvent, in entity);
                
            }

        }
        #endregion

    }

}