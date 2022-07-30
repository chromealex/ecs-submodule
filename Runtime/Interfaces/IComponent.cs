namespace ME.ECS {

    public interface IComponentBase { }

    public interface IStructComponent : IComponentBase { }

    public interface IComponent : IStructComponent { }

    public interface IComponentRuntime { }
    
    public interface IComponentOneShot : IComponentBase, IComponentRuntime { }

    public interface IVersioned : IComponentBase { }

    public interface IComponentBlittable : IComponentBase {}
    
    #if !COMPONENTS_VERSION_NO_STATE_DISABLED
    public interface IVersionedNoState : IComponentBase { }
    #endif

    [System.Obsolete("Use IComponent")]
    public interface IComponentDisposable : IComponentBase {

        void OnDispose();

    }

    public interface ICopyableBase { }

    #if COMPONENTS_COPYABLE
    public interface IStructCopyable<T> : IComponent, ICopyableBase where T : IStructCopyable<T> {

        void CopyFrom(in T other);
        void OnRecycle();

    }

    public interface ICopyable<T> : IStructCopyable<T> where T : IStructCopyable<T> {

    }
    #endif

}