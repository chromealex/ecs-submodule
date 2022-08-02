namespace ME.ECS.Transform {

    [ComponentGroup(typeof(TransformComponentConstants.GroupInfo))]
    [ComponentOrder(4)]
    public struct Container : IComponent, IVersioned, IFilterConnect {

        public Entity entity;

        Entity IFilterConnect.entity => this.entity;

    }

    [ComponentGroup(typeof(TransformComponentConstants.GroupInfo))]
    [ComponentOrder(5)]
    public struct Nodes : IComponent, IVersioned, IComponentDisposable {

        public ME.ECS.Collections.MemoryAllocator.List<Entity> items;

        public void OnDispose(ref ME.ECS.Collections.V3.MemoryAllocator allocator) {
            if (this.items.isCreated == true) this.items.Dispose(ref allocator);
        }

    }

}