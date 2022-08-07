
using System.Linq;
using System.Reflection;

namespace ME.ECS.DebugUtils {

    public class EntityProxyDebugger {

        private Entity entity;
        private World world;
        
        public EntityProxyDebugger(Entity entity) {

            this.entity = entity;
            this.world = Worlds.current;

        }
        
        public EntityProxyDebugger(Entity entity, World world) {

            this.entity = entity;
            this.world = world;

        }

        public void SetEntity(Entity entity) {
            
            this.entity = entity;
            
        }

        public void SetComponent(int index, IComponentBase data) {
            
            var world = this.world;
            var components = world.GetStructComponents();
            var registries = components.GetAllRegistries();
            registries.arr[index].SetObject(this.entity, data, StorageType.Default);
            
        }

        #if !SHARED_COMPONENTS_DISABLED
        public void SetSharedComponent(int index, IComponentBase data, uint groupId) {
            
            var world = this.world;
            var components = world.GetStructComponents();
            var registries = components.GetAllRegistries();
            registries.arr[index].SetSharedObject(this.entity, data, groupId);

        }

        public struct SharedGroup {

            public uint groupId;
            public IComponentBase data;

        }
        #endif

        public string actor {
            get { return this.entity.ToString(); }
        }

        public bool alive {
            get { return this.entity.IsAlive(); }
        }
        
        public IComponentBase[] components {
            
            get {
            
                var world = this.world;
                var components = world.GetStructComponents();
                var registries = components.GetAllRegistries();
                var list = new System.Collections.Generic.List<IComponentBase>();
                foreach (var reg in registries) {

                    if (reg == null) continue;
                    if (reg.Has(this.entity) == false) continue;
                    var comp = reg.GetObject(this.entity);
                    list.Add(comp);

                }

                return list.ToArray();
                
            }
            
        }

        public IComponentBase[] oneShotComponents {
            
            get {
            
                var world = this.world;
                var components = world.GetNoStateData();
                var registries = components.storage.GetAllRegistries();
                var list = new System.Collections.Generic.List<IComponentBase>();
                foreach (var reg in registries) {

                    if (reg == null) continue;
                    if (reg.Has(this.entity) == false) continue;
                    var comp = reg.GetObject(this.entity);
                    list.Add(comp);

                }

                return list.ToArray();
                
            }
            
        }

        #if !SHARED_COMPONENTS_DISABLED
        public SharedGroup[] sharedComponents {
            
            get {
            
                var world = this.world;
                var components = world.GetStructComponents();
                var registries = components.GetAllRegistries();
                var list = new System.Collections.Generic.List<SharedGroup>();
                foreach (var reg in registries) {
                    
                    if (reg == null) continue;
                    var groups = reg.GetSharedGroups(this.entity);
                    if (groups != null) {

                        foreach (var group in groups) {

                            list.Add(new SharedGroup() {
                                groupId = group,
                                data = reg.GetSharedObject(this.entity, group),
                            });

                        }

                    }

                }

                return list.ToArray();
                
            }
            
        }
        #endif

        [System.Serializable]
        public struct ComponentData {

            public int dataIndex;
            public uint groupId;
            [UnityEngine.SerializeReference]
            public IComponentBase data;

        }

        public class DuplicateKeyComparer<TKey> : System.Collections.Generic.IComparer<TKey> where TKey : System.IComparable {
            
            public int Compare(TKey x, TKey y) {
                int result = x.CompareTo(y);
                if (result == 0) {
                    return 1; // Handle equality as being greater. Note: this will break Remove(key) or
                } else { // IndexOfKey(key) since the comparer never returns 0 to signal key equality
                    return result;
                }
            }
            
        }
        
        private class TempGroup {

            public string name;
            public int order;
            public System.Collections.Generic.SortedList<int, ComponentData> data;

        }
        public ComponentData[] GetComponentsList() {
            
            if (this.alive == false) return new ComponentData[0];
            
            var curGroup = new TempGroup() {
                name = "Default",
                order = 0,
                data = new System.Collections.Generic.SortedList<int, ComponentData>(new DuplicateKeyComparer<int>()),
            };
            var sorted = new System.Collections.Generic.SortedList<int, string>(new DuplicateKeyComparer<int>());
            sorted.Add(curGroup.order, curGroup.name);
            var groups = new System.Collections.Generic.Dictionary<string, TempGroup>();
            groups.Add(curGroup.name, curGroup);
            var world = this.world;
            var components = world.GetStructComponents();
            var registries = components.GetAllRegistries();
            for (int i = 0; i < registries.Length; ++i) {

                var reg = registries.arr[i];
                if (reg == null) continue;
                if (reg.Has(this.entity) == false) continue;

                var data = reg.GetObject(this.entity);
                var componentData = new ComponentData() {
                    dataIndex = i,
                    data = data,
                };
                var hasGroup = false;
                var type = data.GetType();

                var componentOrder = 0;
                var orderAttr = type.GetCustomAttribute<ComponentOrderAttribute>(true);
                if (orderAttr != null) {

                    componentOrder = orderAttr.order;

                }
                
                var groupAttr = type.GetCustomAttribute<ComponentGroupAttribute>(true);
                if (groupAttr != null) {

                    if (curGroup.name != groupAttr.name) {

                        if (groups.TryGetValue(groupAttr.name, out var group) == true) {
                            
                            group.data.Add(componentOrder, componentData);
                            
                        } else {
                            
                            group = new TempGroup() {
                                name = groupAttr.name,
                                order = groupAttr.order,
                                data = new System.Collections.Generic.SortedList<int, ComponentData>(new DuplicateKeyComparer<int>()),
                            };
                            group.data.Add(componentOrder, componentData);
                            groups.Add(groupAttr.name, group);
                            sorted.Add(groupAttr.order, groupAttr.name);
                            
                        }
                        
                    }

                    hasGroup = true;

                }

                if (hasGroup == false) {
                    
                    curGroup.data.Add(componentOrder, componentData);
                    
                }
                
            }

            var list = new System.Collections.Generic.List<ComponentData>();
            foreach (var kv in sorted) {

                if (groups.TryGetValue(kv.Value, out var group) == true) {
                    
                    list.AddRange(group.data.Values);
                    
                }
                
            }

            return list.ToArray();

        }

        #if !SHARED_COMPONENTS_DISABLED
        public ComponentData[] GetSharedComponentsList() {
            
            if (this.alive == false) return new ComponentData[0];
            
            var world = this.world;
            var components = world.GetStructComponents();
            var registries = components.GetAllRegistries();
            var list = new System.Collections.Generic.List<ComponentData>();
            for (int i = 0; i < registries.Length; ++i) {

                var reg = registries.arr[i];
                if (reg == null) continue;
                var groups = reg.GetSharedGroups(this.entity);
                if (groups != null) {

                    foreach (var group in groups) {

                        list.Add(new ComponentData() {
                            dataIndex = i,
                            groupId = group,
                            data = reg.GetSharedObject(this.entity, group),
                        });

                    }

                }

            }

            return list.ToArray();
            
        }
        #endif

    }

}