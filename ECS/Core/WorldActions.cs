
namespace ME.ECS {

    public abstract class EntityActionBase {

        internal System.Action<Entity> onAdd;
        internal System.Action<Entity> onRemove;

        public abstract void ExecuteOnAdd(in Entity entity);
        public abstract void ExecuteOnRemove(in Entity entity);

        public abstract void Dispose(World world);

    }
    
    #if ENTITY_ACTIONS
    public class EntityAction<TComponent> : EntityActionBase where TComponent : struct, IStructComponent {

        public static EntityAction<TComponent> Create(World world) {

            var instance = PoolClass<EntityAction<TComponent>>.Spawn();
            world.RegisterEntityAction(instance);
            return instance;

        }

        public override void ExecuteOnAdd(in Entity entity) {
            
            if (this.onAdd != null) this.onAdd.Invoke(entity);
            
        }

        public override void ExecuteOnRemove(in Entity entity) {
            
            if (this.onRemove != null) this.onRemove.Invoke(entity);
            
        }

        public override void Dispose(World world) {

            this.onAdd = null;
            this.onRemove = null;
            world.UnRegisterEntityAction(this);
            
        }

    }
    
    public class EntityActions : System.IDisposable {

        private World world;
        private ME.ECS.Collections.ListCopyable<EntityActionBase> list;

        public static EntityActions Create(World world) {

            var instance = PoolClass<EntityActions>.Spawn();
            instance.world = world;
            instance.list = PoolList<EntityActionBase>.Spawn(10);
            return instance;

        }

        public EntityActions Any<TComponent>() where TComponent : struct, IStructComponent {

            this.list.Add(EntityAction<TComponent>.Create(this.world));
            return this;
            
        }

        public void ExecuteOnAdd(in Entity entity) {
            
            for (int i = 0; i < this.list.Count; ++i) {

                this.list[i].onAdd.Invoke(entity);

            }
            
        }

        public void ExecuteOnRemove(in Entity entity) {
            
            for (int i = 0; i < this.list.Count; ++i) {

                this.list[i].onRemove.Invoke(entity);

            }
            
        }
        
        public EntityActions OnAdd(System.Action<Entity> onAdd) {

            for (int i = 0; i < this.list.Count; ++i) {

                this.list[i].onAdd = onAdd;

            }
            
            return this;

        }

        public EntityActions OnRemove(System.Action<Entity> onRemove) {

            for (int i = 0; i < this.list.Count; ++i) {

                this.list[i].onRemove = onRemove;

            }

            return this;

        }

        public void Dispose() {
            
            for (int i = 0; i < this.list.Count; ++i) {

                this.list[i].Dispose(this.world);

            }
            
        }

    }
    #endif
    
    public class FilterAction : System.IDisposable {

        private class InnerAction : IFilterAction {

            internal System.Action<Entity> callback;
            
            public void Execute(in Entity entity) {
                
                this.callback.Invoke(entity);
                
            }

        }

        private Filter filter;
        private InnerAction onAdd;
        private InnerAction onRemove;
        
        public static FilterAction Create(System.Action<Entity> onAdd = null, System.Action<Entity> onRemove = null, string customName = null) {

            var instance = PoolClass<FilterAction>.Spawn();
            instance.filter = Filter.Create(customName);
            
            if (onAdd != null) {

                instance.onAdd = PoolClass<InnerAction>.Spawn();
                instance.onAdd.callback = onAdd;
                instance.filter.SetOnEntityAdd(instance.onAdd);

            }

            if (onRemove != null) {

                instance.onRemove = PoolClass<InnerAction>.Spawn();
                instance.onRemove.callback = onRemove;
                instance.filter.SetOnEntityRemove(instance.onRemove);

            }

            return instance;

        }

        public void Dispose() {

            if (this.onAdd != null) PoolClass<InnerAction>.Recycle(ref this.onAdd);
            if (this.onRemove != null) PoolClass<InnerAction>.Recycle(ref this.onRemove);
            PoolClass<FilterAction>.Recycle(this);

        }

        public FilterAction Push() {
            
            this.filter.Push();
            this.filter.world.Register(this);
            return this;

        }

        public FilterAction WithStructComponent<TComponent>() where TComponent : struct, IStructComponent {

            this.filter.WithStructComponent<TComponent>();
            return this;

        }

        public FilterAction WithoutStructComponent<TComponent>() where TComponent : struct, IStructComponent {

            this.filter.WithoutStructComponent<TComponent>();
            return this;

        }

    }

}
