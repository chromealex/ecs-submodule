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

    }

    public enum StorageType : byte {

        Default = 0,
        NoState = 1,

    }

    public class ComponentOrderAttribute : System.Attribute {

        public int order;

        public ComponentOrderAttribute(int order) {

            this.order = order;

        }

    }

    public interface IComponentBase { }

    public interface IStructComponent : IComponentBase { }

    public interface IComponent : IStructComponent { }

    public interface IComponentRuntime { }
    
    public interface IComponentOneShot : IComponentBase, IComponentRuntime { }

    public interface IVersioned : IComponentBase { }

    public interface IVersionedNoState : IComponentBase { }

    public interface IComponentShared : IComponentBase { }

    public interface IComponentDisposable : IComponentBase {

        void OnDispose();

    }

    public interface ICopyableBase { }

    public interface IStructCopyable<T> : IComponent, ICopyableBase where T : IStructCopyable<T> {

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
