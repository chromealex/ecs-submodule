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
    
    public interface IComponentDisposableBase : IComponentBase {}

    public interface IComponentDisposable<T> : IComponentDisposableBase where T : IComponentDisposable<T> {

        void OnDispose(ref ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator);
        void ReplaceWith(ref ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator, in T other);

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

    /// <summary>
    /// Used in data configs
    /// If component has this interface - it would be ignored in DataConfig::Apply method
    /// </summary>
    public interface IComponentStatic : IComponentBase { }

}
