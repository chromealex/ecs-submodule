#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Collections;

    public struct ComponentTypesCounter {

        public static int counter = -1;

    }

    public struct AllComponentTypesCounter {

        public static int counter = -1;

    }

    public struct ComponentTypesRegistry {

        public static System.Collections.Generic.Dictionary<System.Type, int> allTypeId = new System.Collections.Generic.Dictionary<System.Type, int>();
        public static System.Collections.Generic.Dictionary<System.Type, int> typeId = new System.Collections.Generic.Dictionary<System.Type, int>();
        public static System.Action reset;

    }

    public struct ComponentTypes<TComponent> {

        public static readonly byte _;
        public static int typeId = -1;

    }

    public struct AllComponentTypes<TComponent> {

        public static readonly byte _;
        public static int typeId = -1;
        public static bool isTag = false;
        public static bool isCopyable = false;
        public static bool isInHash = true;

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static partial class EntityExtensions {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Destroy(this in Entity entity) {

            Worlds.currentWorld.RemoveEntity(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void DestroyAllViews(this in Entity entity) {

            Worlds.currentWorld.DestroyAllViews(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity RemoveData<TComponent>(this Entity entity) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.RemoveData<TComponent>(entity);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool HasData<TComponent>(this Entity entity) where TComponent : struct, IStructComponent {

            return Worlds.currentWorld.HasData<TComponent>(entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref readonly TComponent ReadData<TComponent>(this in Entity entity) where TComponent : struct, IStructComponent {

            return ref Worlds.currentWorld.ReadData<TComponent>(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref TComponent GetData<TComponent>(this in Entity entity, bool createIfNotExists = true) where TComponent : struct, IStructComponent {

            return ref Worlds.currentWorld.GetData<TComponent>(in entity, createIfNotExists);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity CopyFrom(this in Entity entity, in Entity fromEntity) {

            Worlds.currentWorld.CopyFrom(in fromEntity, in entity);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity SetData<TComponent>(this in Entity entity) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData<TComponent>(in entity);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity SetData<TComponent>(this in Entity entity, ComponentLifetime lifetime) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData<TComponent>(in entity, lifetime);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity SetData<TComponent>(this in Entity entity, in TComponent data) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData(in entity, in data);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity SetData<TComponent>(this in Entity entity, in TComponent data, ComponentLifetime lifetime) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData(in entity, in data, lifetime);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity ValidateData<TComponent>(this in Entity entity, bool isTag = false) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.ValidateData<TComponent>(in entity, isTag);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity ValidateDataCopyable<TComponent>(this in Entity entity, bool isTag = false) where TComponent : struct, IStructComponent, IStructCopyable<TComponent> {

            Worlds.currentWorld.ValidateDataCopyable<TComponent>(in entity, isTag);
            return entity;

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    #if MESSAGE_PACK_SUPPORT
    [MessagePack.MessagePackObjectAttribute()]
    #endif
    public readonly struct Entity : System.IEquatable<Entity>, System.IComparable<Entity> {

        public const ushort GENERATION_ZERO = 0;
        public static Entity Empty = new Entity(0, Entity.GENERATION_ZERO);

        #if MESSAGE_PACK_SUPPORT
        [MessagePack.Key(0)]
        #endif
        public readonly int id;
        #if MESSAGE_PACK_SUPPORT
        [MessagePack.Key(1)]
        #endif
        public readonly ushort generation;

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsEmpty() {

            return this.generation == Entity.GENERATION_ZERO;

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsAlive() {

            // Inline manually
            return Worlds.currentWorld.currentState.storage.cache.arr[this.id].generation == this.generation;
            //return Worlds.currentWorld.IsAlive(this.id, this.generation);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Entity(string name) {

            ref var entity = ref Worlds.currentWorld.AddEntity(name);
            this.id = entity.id;
            this.generation = entity.generation;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal Entity(int id, ushort generation) {

            this.id = id;
            this.generation = generation;

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool operator ==(Entity e1, Entity e2) {

            return e1.id == e2.id && e1.generation == e2.generation;

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool operator !=(Entity e1, Entity e2) {

            return !(e1 == e2);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Equals(Entity other) {

            return this == other;

        }

        public override bool Equals(object obj) {

            throw new AllocationException();

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int CompareTo(Entity other) {

            return this.id.CompareTo(other.id);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override int GetHashCode() {

            return this.id ^ this.generation;

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public uint GetVersion() {

            if (Worlds.currentWorld == null) {
                return 0u;
            }

            return Worlds.currentWorld.currentState.storage.versions.Get(this);

        }

        public override string ToString() {

            return "Entity Id: " + this.id.ToString() + " Gen: " + this.generation.ToString() + " Ver: " + this.GetVersion().ToString();

        }

        public string ToSmallString() {

            return "Id: " + this.id.ToString() + "#" + this.generation.ToString() + " (" + this.GetVersion().ToString() + ")";

        }

    }

}