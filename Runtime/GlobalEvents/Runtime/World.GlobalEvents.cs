namespace ME.ECS.GlobalEvents {

    public enum GlobalEventType : byte {

        Logic,
        Visual,

    }

    public static class WorldExt {
        
        public static void ProcessGlobalEvents(this ME.ECS.World world, GlobalEventType globalEventType) {

            if (globalEventType == GlobalEventType.Visual) {

                ref var allocator = ref world.GetNoStateData().allocator;
                ref var storage = ref world.GetNoStateData().pluginsStorage.Get<ME.ECS.GlobalEvents.WorldStorage>(ref allocator, ME.ECS.GlobalEvents.WorldStorage.key);
                try {

                    for (int i = 0; i < storage.globalEventFrameItems.Count; ++i) {

                        var item = storage.globalEventFrameItems[in allocator, i];
                        GlobalEvent.GetEventById(item.globalEvent).Run(in item.data);

                    }

                } catch (System.Exception ex) {

                    UnityEngine.Debug.LogException(ex);

                }

                storage.globalEventFrameItems.Clear(in allocator);
                storage.globalEventFrameEvents.Clear(in allocator);

            } else if (globalEventType == GlobalEventType.Logic) {

                ref var allocator = ref world.GetState().allocator;
                ref var storage = ref world.GetNoStateData().pluginsStorage.Get<ME.ECS.GlobalEvents.GlobalEventStorage>(ref allocator, ME.ECS.GlobalEvents.GlobalEventStorage.key);
                for (int i = 0; i < storage.globalEventLogicItems.Count; ++i) {

                    var item = storage.globalEventLogicItems[in allocator, i];
                    GlobalEvent.GetEventById(item.globalEvent).Run(in item.data);

                }

                storage.globalEventLogicItems.Clear(in allocator);
                storage.globalEventLogicEvents.Clear(in allocator);

            }

        }

        public static bool CancelGlobalEvent(this ME.ECS.World world, GlobalEvent globalEvent, in Entity entity, GlobalEventType globalEventType) {

            var key = MathUtils.GetKey(globalEvent.GetHashCode(), entity.GetHashCode());
            if (globalEventType == GlobalEventType.Visual) {

                ref var allocator = ref world.GetNoStateData().allocator;
                ref var storage = ref world.GetNoStateData().pluginsStorage.Get<ME.ECS.GlobalEvents.WorldStorage>(ref allocator, ME.ECS.GlobalEvents.WorldStorage.key);
                if (storage.globalEventFrameEvents.Contains(in allocator, key) == true) {

                    for (int i = 0; i < storage.globalEventFrameItems.Count; ++i) {

                        var item = storage.globalEventFrameItems[in allocator, i];
                        if (item.globalEvent == globalEvent.id && item.data == entity) {

                            storage.globalEventFrameEvents.Remove(ref allocator, key);
                            storage.globalEventFrameItems.RemoveAt(ref allocator, i);
                            return true;

                        }

                    }

                }

            } else if (globalEventType == GlobalEventType.Logic) {

                ref var allocator = ref world.GetState().allocator;
                ref var storage = ref world.GetNoStateData().pluginsStorage.Get<ME.ECS.GlobalEvents.GlobalEventStorage>(ref allocator, ME.ECS.GlobalEvents.GlobalEventStorage.key);
                storage.Remove(ref allocator, globalEvent, in entity);
                
            }

            return false;

        }

        public static void RegisterGlobalEvent(this ME.ECS.World world, GlobalEvent globalEvent, in Entity entity, GlobalEventType globalEventType) {

            var key = MathUtils.GetKey(globalEvent.GetHashCode(), entity.GetHashCode());
            if (globalEventType == GlobalEventType.Visual) {

                ref var allocator = ref world.noStateData.allocator;
                ref var storage = ref world.noStateData.pluginsStorage.Get<ME.ECS.GlobalEvents.WorldStorage>(ref allocator, ME.ECS.GlobalEvents.WorldStorage.key);
                if (storage.globalEventFrameEvents.Contains(in allocator, key) == false) {

                    storage.globalEventFrameEvents.Add(ref allocator, key);
                    storage.globalEventFrameItems.Add(ref allocator, new GlobalEventStorage.GlobalEventFrameItem() {
                        globalEvent = globalEvent.id,
                        data = entity,
                    });

                }

            } else if (globalEventType == GlobalEventType.Logic) {

                ref var allocator = ref world.GetState().allocator;
                ref var storage = ref world.GetNoStateData().pluginsStorage.Get<ME.ECS.GlobalEvents.GlobalEventStorage>(ref allocator, ME.ECS.GlobalEvents.GlobalEventStorage.key);
                storage.Add(ref allocator, globalEvent, in entity);
                
            }

        }

    }

}

namespace ME.ECS.GlobalEvents {
    
    using MemPtr = System.Int64;
    using ME.ECS.Collections.V3;
    using ME.ECS.Collections.MemoryAllocator;

    public struct WorldStorage : IPlugin {

        public static int key;
        
        #region GlobalEvents
        internal List<GlobalEventStorage.GlobalEventFrameItem> globalEventFrameItems;
        internal HashSet<long> globalEventFrameEvents;

        public void Initialize(int key, ref MemoryAllocator allocator) {
            
            WorldStorage.key = key;
            this.globalEventFrameItems = new List<GlobalEventStorage.GlobalEventFrameItem>(ref allocator, 10);
            this.globalEventFrameEvents = new HashSet<long>(ref allocator, 10);

        }
        #endregion

    }

}