namespace ME.ECS {

    [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
    public interface IComponentBase { }

    [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
    public interface IComponentPredicate<in TComponent> where TComponent : IComponentBase {

        bool Execute(TComponent data);

    }

    [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
    public interface IComponent : IComponentBase {

    }

    [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
    public interface IComponentCopyable : IComponent, IPoolableRecycle {

        void CopyFrom(IComponentCopyable other);
        
    }

    [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
    public interface IComponentSharedBase { }
    [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
    public interface IComponentShared : IComponent, IComponentSharedBase { }
    [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
    public interface IComponentSharedCopyable : IComponentCopyable { }

}