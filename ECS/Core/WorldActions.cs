
namespace ME.ECS {

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

            PoolClass<InnerAction>.Recycle(ref this.onAdd);
            PoolClass<InnerAction>.Recycle(ref this.onRemove);
            PoolClass<FilterAction>.Recycle(this);

        }

        public FilterAction Push() {
            
            this.filter.Push();
            return this;

        }

        public FilterAction WithComponent<TComponent>() where TComponent : class, IComponent {

            this.filter.WithComponent<TComponent>();
            return this;

        }
        
        public FilterAction WithoutComponent<TComponent>() where TComponent : class, IComponent {

            this.filter.WithoutComponent<TComponent>();
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
