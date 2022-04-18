namespace ME.ECS.Transform {

    public struct Container : IComponent, IVersioned, IFilterConnect {

        public Entity entity;

        Entity IFilterConnect.entity => this.entity;

    }

    public struct Nodes : IComponent, IVersioned {

        public ME.ECS.Collections.IntrusiveList items;

    }

}