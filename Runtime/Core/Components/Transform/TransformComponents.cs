namespace ME.ECS.Transform {

    public struct Container : IComponent, IVersioned {

        public Entity entity;

    }

    public struct Nodes : IComponent, IVersioned {

        public ME.ECS.Collections.IntrusiveList items;

    }

}