
namespace ME.ECS.Debug {

    public class EntityProxyDebugger {

        private Entity entity;
        public EntityProxyDebugger(Entity entity) {

            this.entity = entity;

        }

        public struct SharedGroup {

            public uint groupId;
            public IStructComponent data;

        }

        public string actor {
            get { return this.entity.ToString(); }
        }
        
        public IStructComponent[] components {
            
            get {
            
                var world = Worlds.currentWorld;
                var components = world.GetStructComponents();
                var registries = components.GetAllRegistries();
                var list = new System.Collections.Generic.List<IStructComponent>();
                foreach (var reg in registries) {

                    var comp = reg.GetObject(this.entity);
                    if (comp != null) list.Add(comp);

                }

                return list.ToArray();
                
            }
            
        }

        public SharedGroup[] sharedComponents {
            
            get {
            
                var world = Worlds.currentWorld;
                var components = world.GetStructComponents();
                var registries = components.GetAllRegistries();
                var list = new System.Collections.Generic.List<SharedGroup>();
                foreach (var reg in registries) {
                    
                    var groups = reg.GetSharedGroups(this.entity);
                    foreach (var group in groups) {
                        
                        list.Add(new SharedGroup() {
                            groupId = group,
                            data = reg.GetSharedObject(this.entity, group),
                        });
                        
                    }

                }

                return list.ToArray();
                
            }
            
        }

    }

}