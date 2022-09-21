namespace ME.ECS {

    public interface IContext {}
    
    public interface ILoadableSystem : IContext {

        void Load(System.Action onComplete);

    }
    
    public interface ILoadableSync {}

    public interface ISystemConstructLate {

        void OnConstructLate();

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

    public interface IUpdateLate : IContext {

        void UpdateLate(in float deltaTime);

    }

    public interface IUpdatePost : IContext {

        void UpdatePost(in float deltaTime);

    }

    public interface IDrawGizmos {

        void OnDrawGizmos();

    }

}