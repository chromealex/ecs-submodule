using System.Reflection;

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

    public enum GroupColor {

        Default = 0, 
        Red,
        Green,
        Yellow,
        Cyan,
        Grey,
        Magenta,
        White,

    }
    
    public class ComponentGroupAttribute : System.Attribute {

        public string name;
        public UnityEngine.Color color;
        public int order = -1;

        public ComponentGroupAttribute(System.Type referenceType) {

            var attr = referenceType.GetCustomAttribute<ComponentGroupAttribute>();
            if (attr != null) {

                this.name = attr.name;
                this.color = attr.color;
                this.order = attr.order;

            }

        }

        public ComponentGroupAttribute(string name, int order = -1) {

            this.name = name;
            this.order = order;
            this.color = default;

        }

        public ComponentGroupAttribute(string name, byte r, byte g, byte b, int order = -1) {

            this.name = name;
            this.order = -1;
            this.color = new UnityEngine.Color32(r, g, b, 0);

        }

        public ComponentGroupAttribute(string name, GroupColor color, int order = -1) {

            this.name = name;
            this.order = order;
            
            switch (color) {
                case GroupColor.Default:
                    this.color = default;
                    break;
                case GroupColor.Red:
                    this.color = UnityEngine.Color.red;
                    break;
                case GroupColor.Green:
                    this.color = UnityEngine.Color.green;
                    break;
                case GroupColor.Yellow:
                    this.color = UnityEngine.Color.yellow;
                    break;
                case GroupColor.Cyan:
                    this.color = UnityEngine.Color.cyan;
                    break;
                case GroupColor.Grey:
                    this.color = UnityEngine.Color.grey;
                    break;
                case GroupColor.Magenta:
                    this.color = UnityEngine.Color.magenta;
                    break;
                case GroupColor.White:
                    this.color = UnityEngine.Color.white;
                    break;
            }

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

    public interface ICopyable<T> : IStructCopyable<T> where T : IStructCopyable<T> {

    }

    public interface ISharedGroups {

        System.Collections.Generic.ICollection<uint> GetGroups();

    }

    public interface IStructComponentsContainer {

        BufferArray<StructRegistryBase> GetAllRegistries();

    }

}
