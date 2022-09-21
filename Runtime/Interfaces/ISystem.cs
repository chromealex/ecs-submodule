namespace ME.ECS {

    public interface ISystemBase : IContext {
        
        World world { get; set; }
        
        void OnConstruct();
        void OnDeconstruct();

    }

    public interface ISystemFilter : ISystem, IAdvanceTickBase {

        #if CSHARP_8_OR_NEWER
        [System.ObsoleteAttribute("Use ISystemJobs")]
        bool jobs => false;
        [System.ObsoleteAttribute("Use ISystemJobs")]
        int jobsBatchCount => 64;
        #else
        [System.ObsoleteAttribute("Use ISystemJobs")]
        bool jobs { get; }
        [System.ObsoleteAttribute("Use ISystemJobs")]
        int jobsBatchCount { get; }
        #endif
        
        Filter filter { get; set; }
        Filter CreateFilter();

        void AdvanceTick(in Entity entity, in float deltaTime);

    }

    public interface ISystemJobs : ISystem {

        #if CSHARP_8_OR_NEWER
        int jobsBatchCount => 64;
        #else
        int jobsBatchCount { get; }
        #endif

    }

    public interface ISystem : ISystemBase { }

    public interface ISystemValidation : IContext {

        bool CanBeAdded();

    }

}