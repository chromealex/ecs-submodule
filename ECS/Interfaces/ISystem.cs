namespace ME.ECS {

    public interface IContext {}
    
    public interface ILoadableSystem : IContext {

        void Load(System.Action onComplete);

    }
    
    public interface ISystemBase : IContext {
        
        World world { get; set; }
        
        void OnConstruct();
        void OnDeconstruct();

    }

    public interface IAdvanceTickBase : IContext { }
    
    public interface IAdvanceTickStep : IContext {

        Tick step { get; }

    }

    public interface IAdvanceTick : IAdvanceTickBase {

        void AdvanceTick(in float deltaTime);

    }

    public interface IAdvanceTickPost : IContext {

        void AdvanceTickPost(in float deltaTime);

    }

    public interface IAdvanceTickPre : IContext {

        void AdvanceTickPre(in float deltaTime);

    }

    public interface IUpdate : IContext {

        void Update(in float deltaTime);

    }

    public interface IUpdatePost : IContext {

        void UpdatePost(in float deltaTime);

    }

    public interface ISystemFilter : ISystem, IAdvanceTickBase {

        bool jobs { get; }
        int jobsBatchCount { get; }
        Filter filter { get; set; }
        Filter CreateFilter();

        void AdvanceTick(in Entity entity, in float deltaTime);
        
    }

    /*
    public interface IAdvanceTickBurst {

        World.SystemFilterAdvanceTick GetAdvanceTickForBurst();

    }*/
    
    public interface ISystem : ISystemBase { }

    public interface ISystemValidation : IContext {

        bool CouldBeAdded();

    }

}