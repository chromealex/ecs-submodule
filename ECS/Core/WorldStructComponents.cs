#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using ME.ECS.Collections;

    public class IsBitmask : System.Attribute { }

    public enum ComponentLifetime : byte {

        Infinite = 0,

        NotifyAllSystemsBelow = 1,
        NotifyAllSystems = 2,

        NotifyAllModulesBelow = 3,
        NotifyAllModules = 4,

    }

    public class ComponentOrderAttribute : System.Attribute {

        public int order;

        public ComponentOrderAttribute(int order) {

            this.order = order;

        }

    }

    public interface IStructComponentBase { }

    public interface IStructComponent : IStructComponentBase { }

    public interface IComponent : IStructComponent { }

    public interface IComponentRuntime { }
    
    public interface IComponentOneShot : IStructComponentBase, IComponentRuntime { }

    public interface IVersioned : IStructComponentBase { }

    public interface IVersionedNoState : IStructComponentBase { }

    public interface IComponentShared : IStructComponentBase { }

    public interface IComponentDisposable : IStructComponentBase {

        void OnDispose();

    }

    public interface IStructCopyableBase { }

    public interface IStructCopyable<T> : IStructComponent, IStructCopyableBase where T : IStructCopyable<T> {

        void CopyFrom(in T other);
        void OnRecycle();

    }

    public interface ISharedGroups {

        System.Collections.Generic.ICollection<uint> GetGroups();

    }

    public interface IStructComponentsContainer {

        BufferArray<StructRegistryBase> GetAllRegistries();

    }

}
