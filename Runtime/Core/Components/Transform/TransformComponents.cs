namespace ME.ECS.Transform {

    public struct Container : IComponent {

        public Entity entity;

    }

    [System.ObsoleteAttribute("Use Nodes instead.")]
    public struct Childs : IStructComponent {

        public ME.ECS.Collections.IntrusiveList childs;

    }

    public struct Nodes : IComponent {

        public ME.ECS.Collections.IntrusiveList items;

    }

}