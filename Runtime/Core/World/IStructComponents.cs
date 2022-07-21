#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif
using System.Reflection;

namespace ME.ECS {

    using ME.ECS.Collections;

    public enum ComponentLifetime : byte {

        Infinite = 0,

        NotifyAllSystemsBelow = 1,
        NotifyAllSystems = 2,

    }

    public enum StorageType : byte {

        Default = 0,
        NoState = 1,

    }

    public interface IStructComponentsContainer {

        BufferArray<StructRegistryBase> GetAllRegistries();

    }

}
