
namespace ME.ECS.Debug {

    public class EntityProxyDebugger {

        private Entity entity;
        public EntityProxyDebugger(Entity entity) {

            this.entity = entity;
            this.id = entity.id;
            this.generation = entity.generation;
            this.version = entity.GetVersion();

        }

        public int id;
        public ushort generation;
        public uint version;
        
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

    }

}