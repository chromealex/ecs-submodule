namespace ME.ECS.Transform {

    public struct Container : IComponent {

        public Entity entity;

    }

    public struct Nodes : IComponent {

        public ME.ECS.Collections.IntrusiveList items;

    }

}