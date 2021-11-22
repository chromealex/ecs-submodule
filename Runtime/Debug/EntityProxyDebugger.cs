
namespace ME.ECS.Debug {

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

        public void SetComponent(int index, IStructComponentBase data) {
            
            var world = this.world;
            var components = world.GetStructComponents();
            var registries = components.GetAllRegistries();
            registries.arr[index].SetObject(this.entity, data);
            
        }

        public void SetSharedComponent(int index, IStructComponentBase data, uint groupId) {
            
            var world = this.world;
            var components = world.GetStructComponents();
            var registries = components.GetAllRegistries();
            registries.arr[index].SetSharedObject(this.entity, data, groupId);

        }

        public struct SharedGroup {

            public uint groupId;
            public IStructComponentBase data;

        }

        public string actor {
            get { return this.entity.ToString(); }
        }

        public bool alive {
            get { return this.entity.IsAlive(); }
        }
        
        public IStructComponentBase[] components {
            
            get {
            
                var world = this.world;
                var components = world.GetStructComponents();
                var registries = components.GetAllRegistries();
                var list = new System.Collections.Generic.List<IStructComponentBase>();
                foreach (var reg in registries) {

                    if (reg == null) continue;
                    if (reg.Has(this.entity) == false) continue;
                    var comp = reg.GetObject(this.entity);
                    list.Add(comp);

                }

                return list.ToArray();
                
            }
            
        }

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

        [System.Serializable]
        public struct ComponentData {

            public int dataIndex;
            public uint groupId;
            [UnityEngine.SerializeReference]
            public IStructComponentBase data;

        }
        
        public ComponentData[] GetComponentsList() {
            
            if (this.alive == false) return new ComponentData[0];
            
            var world = this.world;
            var components = world.GetStructComponents();
            var registries = components.GetAllRegistries();
            var list = new System.Collections.Generic.List<ComponentData>();
            for (int i = 0; i < registries.Length; ++i) {

                var reg = registries.arr[i];
                if (reg == null) continue;
                if (reg.Has(this.entity) == false) continue;
                list.Add(new ComponentData() {
                    dataIndex = i,
                    data = reg.GetObject(this.entity),
                });

            }

            return list.ToArray();

        }

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

    }

}