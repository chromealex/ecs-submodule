namespace ME.ECS.Essentials.GOAP {
    
    using Modules;

    public struct GOAPEntityGroup : IComponent, IComponentRuntime {

        public GOAPGroupId groupId;

    }

    public struct GOAPEntityGoal : IComponent {

        public Goal goal;

    }

    public struct GOAPEntityPrevPlan : IComponent {

        public GOAPGroupId groupId;
        public int nextActionIdx;

    }

    public struct GOAPEntityPlan : IComponent {

        public GOAPGroupId groupId;
        public int nextActionIdx;
        
        public GOAPAction nextAction => Worlds.current.GetModule<GOAPModule>().GetAction(this.groupId, this.nextActionIdx);

    }

}