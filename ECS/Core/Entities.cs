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

        public static int typeId = -1;
        public static bool isFilterVersioned = false;

    }

    public struct AllComponentTypes<TComponent> {

        public static int typeId = -1;
        public static bool isTag = false;
        public static bool isVersioned = false;
        public static bool isVersionedNoState = false;
        public static bool isCopyable = false;
        public static bool isShared = false;
        public static bool isDisposable = false;
        public static bool isOneShot = false;
        public static bool isInHash = true;
        public static TComponent empty = default;

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
        public static long GetDataVersion<TComponent>(this in Entity entity) where TComponent : struct, IVersioned {

            return Worlds.currentWorld.GetDataVersion<TComponent>(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static uint GetDataVersionNoState<TComponent>(this in Entity entity) where TComponent : struct, IVersionedNoState {

            return Worlds.currentWorld.GetDataVersionNoState<TComponent>(in entity);

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
        public static Entity ValidateData<TComponent>(this in Entity entity, bool isTag = false) where TComponent : struct, IStructComponentBase {

            Worlds.currentWorld.ValidateData<TComponent>(in entity, isTag);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity ValidateDataOneShot<TComponent>(this in Entity entity, bool isTag = false) where TComponent : struct, IComponentOneShot {

            Worlds.currentWorld.ValidateDataOneShot<TComponent>(in entity, isTag);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity ValidateDataCopyable<TComponent>(this in Entity entity, bool isTag = false) where TComponent : struct, IStructCopyable<TComponent> {

            Worlds.currentWorld.ValidateDataCopyable<TComponent>(in entity, isTag);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity ValidateDataDisposable<TComponent>(this in Entity entity, bool isTag = false) where TComponent : struct, IComponentDisposable {

            Worlds.currentWorld.ValidateDataDisposable<TComponent>(in entity, isTag);
            return entity;

        }

    }
    
    #if !ENTITY_API_VERSION1_TURN_OFF
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static partial class EntityExtensionsV1 {

        #region Set/Has/Remove/Read
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public static Entity RemoveData<TComponent>(this Entity entity) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.RemoveData<TComponent>(entity);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public static bool HasData<TComponent>(this Entity entity) where TComponent : struct, IStructComponent {

            return Worlds.currentWorld.HasData<TComponent>(entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public static ref
            //#if UNITY_EDITOR
            readonly
            //#endif
            TComponent ReadData<TComponent>(this in Entity entity) where TComponent : struct, IStructComponent {

            return ref Worlds.currentWorld.ReadData<TComponent>(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public static ref TComponent GetData<TComponent>(this in Entity entity, bool createIfNotExists = true) where TComponent : struct, IStructComponent {

            return ref Worlds.currentWorld.GetData<TComponent>(in entity, createIfNotExists);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public static Entity SetData<TComponent>(this in Entity entity) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData<TComponent>(in entity);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public static Entity SetData<TComponent>(this in Entity entity, ComponentLifetime lifetime) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData<TComponent>(in entity, lifetime);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public static Entity SetData<TComponent>(this in Entity entity, in TComponent data) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData(in entity, in data);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public static Entity SetData<TComponent>(this in Entity entity, in TComponent data, ComponentLifetime lifetime) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData(in entity, in data, lifetime);
            return entity;

        }
        #endregion

    }
    #endif
    
    #if !ENTITY_API_VERSION2_TURN_OFF
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static partial class EntityExtensionsV2 {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity SetTimer(this in Entity entity, int index, float time) {

            Worlds.currentWorld.SetTimer(in entity, index, time);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static float ReadTimer(this in Entity entity, int index) {

            return Worlds.currentWorld.ReadTimer(in entity, index);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref float GetTimer(this in Entity entity, int index) {

            return ref Worlds.currentWorld.GetTimer(in entity, index);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool RemoveTimer(this in Entity entity, int index) {

            return Worlds.currentWorld.RemoveTimer(in entity, index);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity Remove<TComponent>(this in Entity entity) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.RemoveData<TComponent>(in entity);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool Has<TComponent>(this in Entity entity) where TComponent : struct, IStructComponent {

            return Worlds.currentWorld.HasData<TComponent>(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref
            //#if UNITY_EDITOR
            readonly
            //#endif
            TComponent Read<TComponent>(this in Entity entity) where TComponent : struct, IStructComponent {

            return ref Worlds.currentWorld.ReadData<TComponent>(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref TComponent Get<TComponent>(this in Entity entity, bool createIfNotExists = true) where TComponent : struct, IStructComponent {

            return ref Worlds.currentWorld.GetData<TComponent>(in entity, createIfNotExists);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity SetAs<TComponent>(this in Entity entity, in Entity source) where TComponent : struct, IStructComponent {

            if (source.Has<TComponent>() == true) {

                entity.Set(source.Read<TComponent>());

            } else {
                
                entity.Remove<TComponent>();
                
            }
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity Set<TComponent>(this in Entity entity) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData<TComponent>(in entity);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity Set<TComponent>(this in Entity entity, ComponentLifetime lifetime) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData<TComponent>(in entity, lifetime);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity Set<TComponent>(this in Entity entity, in TComponent data) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData(in entity, in data);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity Set<TComponent>(this in Entity entity, in TComponent data, ComponentLifetime lifetime) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData(in entity, in data, lifetime);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity Set<TComponent>(this in Entity entity, in TComponent data, ComponentLifetime lifetime, float customLifetime) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData(in entity, in data, lifetime, customLifetime);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool HasShared<TComponent>(this in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {

            return Worlds.currentWorld.HasSharedData<TComponent>(in entity, groupId);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref readonly TComponent ReadShared<TComponent>(this in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {

            return ref Worlds.currentWorld.ReadSharedData<TComponent>(in entity, groupId);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref TComponent GetShared<TComponent>(this in Entity entity, uint groupId = 0u, bool createIfNotExists = true) where TComponent : struct, IComponentShared {

            return ref Worlds.currentWorld.GetSharedData<TComponent>(in entity, groupId, createIfNotExists);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity SetShared<TComponent>(this in Entity entity, in TComponent data, uint groupId = 0u) where TComponent : struct, IComponentShared {

            Worlds.currentWorld.SetSharedData(in entity, in data, groupId);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity RemoveShared<TComponent>(this in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {

            Worlds.currentWorld.RemoveSharedData<TComponent>(in entity, groupId);
            return entity;

        }
        
    }
    #endif

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    #if MESSAGE_PACK_SUPPORT
    [MessagePack.MessagePackObjectAttribute()]
    #endif
    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(ME.ECS.Debug.EntityProxyDebugger))]
    public readonly struct Entity : System.IEquatable<Entity>, System.IComparable<Entity> {

        public const ushort GENERATION_ZERO = 0;
        public static readonly Entity Empty = new Entity(0, Entity.GENERATION_ZERO);

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

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsAliveWithBoundsCheck() {

            // Inline manually
            if (Worlds.currentWorld == null || Worlds.currentWorld.currentState == null) return false;
            var arr = Worlds.currentWorld.currentState.storage.cache;
            if (this.id >= arr.Length) return false;
            return arr.arr[this.id].generation == this.generation;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref Entity Create(string name = null) {

            return ref Worlds.currentWorld.AddEntity(name);

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

            if (obj is Entity ent) {
                
                return this.Equals(ent);
                
            }
            
            return false;

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

            if (Worlds.currentWorld == null || this == Entity.Empty) {
                
                return 0u;
                
            }

            return Worlds.currentWorld.currentState.storage.versions.Get(this);

        }

        public string ToStringNoVersion() {

            return $"Entity Id: {this.id} Gen: {this.generation}";

        }

        public override string ToString() {

            return $"Entity Id: {this.id} Gen: {this.generation} Ver: {this.GetVersion().ToString()}";

        }

        public string ToSmallString() {

            return $"Id: {this.id}#{this.generation} ({this.GetVersion().ToString()})";

        }
        
    }

}