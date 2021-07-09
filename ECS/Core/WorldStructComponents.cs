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

    public interface IComponentRuntime { }
    
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

    public interface IStructRegistryBase {

        IStructComponentBase GetObject(Entity entity);
        bool SetObject(Entity entity, IStructComponentBase data);
        IStructComponentBase GetSharedObject(Entity entity, uint groupId);
        bool SetSharedObject(Entity entity, IStructComponentBase data, uint groupId);
        bool RemoveObject(Entity entity);
        bool HasType(System.Type type);

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract class StructRegistryBase : IStructRegistryBase, IPoolableRecycle {

        public World world {
            get {
                return Worlds.currentWorld;
            }
        }

        public abstract int GetTypeBit();
        public abstract int GetAllTypeBit();

        public abstract void UpdateVersion(in Entity entity);
        public abstract void UpdateVersionNoState(in Entity entity);
        
        public abstract bool HasType(System.Type type);
        public abstract IStructComponentBase GetObject(Entity entity);
        public abstract bool SetObject(Entity entity, IStructComponentBase data);
        public abstract System.Collections.Generic.ICollection<uint> GetSharedGroups(Entity entity);
        public abstract IStructComponentBase GetSharedObject(Entity entity, uint groupId);
        public abstract bool SetSharedObject(Entity entity, IStructComponentBase data, uint groupId);
        public abstract bool RemoveObject(Entity entity);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void Merge();

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void UseLifetimeStep(World world, byte step, float deltaTime);

        public abstract void CopyFrom(StructRegistryBase other);

        public abstract void CopyFrom(in Entity from, in Entity to);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract bool Validate(int capacity);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract bool Validate(in Entity entity);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract bool Has(in Entity entity);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract bool HasShared(in Entity entity, uint groupId);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract bool Remove(in Entity entity, bool clearAll = false);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void OnRecycle();

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void Recycle();

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract StructRegistryBase Clone();

        public abstract int GetCustomHash();

    }
    
    public interface ISharedGroups {

        System.Collections.Generic.ICollection<uint> GetGroups();

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class StructComponents<TComponent> : StructRegistryBase where TComponent : struct, IStructComponentBase {

        public struct SharedGroupData {

            [ME.ECS.Serializer.SerializeField]
            internal BufferArray<bool> states;
            [ME.ECS.Serializer.SerializeField]
            internal TComponent data;

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Validate(int entityId) {
                
                ArrayUtils.Resize(entityId, ref this.states, true);
                
            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void CopyFrom(in Entity from, in Entity to) {

                this.states.arr[to.id] = this.states.arr[from.id];

            }

        }

        public struct SharedGroups : ISharedGroups {

            #if MULTITHREAD_SUPPORT
            [ME.ECS.Serializer.SerializeField]
            internal CCDictionary<uint, SharedGroupData> sharedGroups;
            #else
            [ME.ECS.Serializer.SerializeField]
            internal DictionaryCopyable<uint, SharedGroupData> sharedGroups;
            #endif
            private static bool alwaysFalse;

            public System.Collections.Generic.ICollection<uint> GetGroups() {
                
                if (this.sharedGroups == null) return null;
                return this.sharedGroups.Keys;
                
            }

            public void Validate(in Entity entity) {
                
                this.Initialize();

            }

            public void Validate(int capacity) {
                
                this.Initialize();
                
            }

            public void Initialize() {

                if (this.sharedGroups == null) {
                    
                    #if MULTITHREAD_SUPPORT
                    this.sharedGroups = PoolCCDictionary<uint, SharedGroupData>.Spawn(1);
                    #else
                    this.sharedGroups = PoolDictionaryCopyable<uint, SharedGroupData>.Spawn(1);
                    #endif

                }

            }

            public void OnRecycle() {

                if (this.sharedGroups != null) {
                    
                    #if MULTITHREAD_SUPPORT
                    PoolCCDictionary<uint, SharedGroupData>.Recycle(ref this.sharedGroups);
                    #else
                    PoolDictionaryCopyable<uint, SharedGroupData>.Recycle(ref this.sharedGroups);
                    #endif
                    
                }
                
            }

            public void CopyFrom<TElementCopy>(SharedGroups other, TElementCopy copy) where TElementCopy : IArrayElementCopy<SharedGroupData> {

                if (this.sharedGroups == null && other.sharedGroups == null) {

                    return;

                } else if (this.sharedGroups == null && other.sharedGroups != null) {

                    #if MULTITHREAD_SUPPORT
                    this.sharedGroups = PoolCCDictionary<uint, SharedGroupData>.Spawn(other.sharedGroups.Count);
                    #else
                    this.sharedGroups = PoolDictionaryCopyable<uint, SharedGroupData>.Spawn(other.sharedGroups.Count);
                    #endif

                } else if (this.sharedGroups != null && other.sharedGroups == null) {

                    foreach (var kv in this.sharedGroups) {
                        
                        copy.Recycle(kv.Value);
                        
                    }
                    #if MULTITHREAD_SUPPORT
                    PoolCCDictionary<uint, SharedGroupData>.Recycle(ref this.sharedGroups);
                    #else
                    PoolDictionaryCopyable<uint, SharedGroupData>.Recycle(ref this.sharedGroups);
                    #endif
                    return;

                }
                
                this.sharedGroups.CopyFrom(other.sharedGroups, copy);
                
            }

            public void CopyFrom(in Entity from, in Entity to) {

                foreach (var kv in this.sharedGroups) {

                    kv.Value.CopyFrom(in from, in to);

                }
                
            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public ref bool Has(int entityId, uint groupId) {

                ref var group = ref this.sharedGroups.Get(groupId);
                if (entityId < group.states.Length) return ref group.states.arr[entityId];
                SharedGroups.alwaysFalse = false;
                return ref SharedGroups.alwaysFalse;

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public ref TComponent Get(int entityId, uint groupId) {

                return ref this.sharedGroups.Get(groupId).data;

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Set(int entityId, uint groupId, TComponent data) {

                ref var group = ref this.sharedGroups.Get(groupId);
                group.Validate(entityId);
                group.states.arr[entityId] = true;
                group.data = data;

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Set(int entityId, uint groupId) {

                ref var group = ref this.sharedGroups.Get(groupId);
                group.Validate(entityId);
                group.states.arr[entityId] = true;

            }

        }

        public struct LifetimeData {

            public int entityId;
            public float lifetime;

        }

        [ME.ECS.Serializer.SerializeField]
        internal BufferArraySliced<TComponent> components;
        [ME.ECS.Serializer.SerializeField]
        internal BufferArray<byte> componentsStates;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<LifetimeData> lifetimeData;
        [ME.ECS.Serializer.SerializeField]
        internal BufferArray<long> versions;
        
        // We don't need to serialize this field
        internal BufferArray<uint> versionsNoState;
        
        // Shared data
        [ME.ECS.Serializer.SerializeField]
        internal SharedGroups sharedGroups;
        /*[ME.ECS.Serializer.SerializeField]
        internal BufferArray<bool> sharedStates;
        [ME.ECS.Serializer.SerializeField]
        internal TComponent sharedData;*/

        public long GetVersion(in Entity entity) {

            return this.versions.arr[entity.id];

        }

        public uint GetVersionNotStated(in Entity entity) {

            return this.versionsNoState.arr[entity.id];

        }

        public override int GetTypeBit() {

            return WorldUtilities.GetComponentTypeId<TComponent>();

        }

        public override int GetAllTypeBit() {

            return WorldUtilities.GetAllComponentTypeId<TComponent>();

        }

        public override int GetCustomHash() {

            var hash = 0;
            if (typeof(TComponent) == typeof(ME.ECS.Transform.Position)) {

                for (int i = 0; i < this.components.Length; ++i) {

                    var p = (ME.ECS.Transform.Position)(object)this.components[i];
                    hash ^= (int)(p.x * 100000f);
                    hash ^= (int)(p.y * 100000f);
                    hash ^= (int)(p.z * 100000f);

                }

            }

            return hash;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void UpdateVersion(in Entity entity) {

            if (AllComponentTypes<TComponent>.isVersioned == true) this.versions.arr[entity.id] = this.world.GetCurrentTick();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void UpdateVersionNoState(in Entity entity) {

            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++this.versionsNoState.arr[entity.id];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Merge() {

            if (AllComponentTypes<TComponent>.isTag == false) this.components = this.components.Merge();

        }

        public override void Recycle() {

            PoolRegistries.Recycle(this);

        }

        public override StructRegistryBase Clone() {

            var reg = this.SpawnInstance();
            this.Merge();
            reg.CopyFrom(this);
            return reg;

        }

        protected virtual StructRegistryBase SpawnInstance() {

            return PoolRegistries.Spawn<TComponent>();

        }

        public override void OnRecycle() {

            this.components = this.components.Dispose();
            PoolArray<byte>.Recycle(ref this.componentsStates);
            if (AllComponentTypes<TComponent>.isVersioned == true) PoolArray<long>.Recycle(ref this.versions);
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) PoolArray<uint>.Recycle(ref this.versionsNoState);
            if (this.lifetimeData != null) PoolListCopyable<LifetimeData>.Recycle(ref this.lifetimeData);
            
            if (AllComponentTypes<TComponent>.isShared == true) this.sharedGroups.OnRecycle();
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void UseLifetimeStep(World world, byte step, float deltaTime) {

            if (this.lifetimeData != null) {

                for (int i = 0, cnt = this.lifetimeData.Count; i < cnt; ++i) {

                    ref var data = ref this.lifetimeData[i];
                    data.lifetime -= deltaTime;
                    if (data.lifetime <= 0f) {

                        this.UseLifetimeStep(data.entityId, world, step);

                    }

                }

                this.lifetimeData.Clear();

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void UseLifetimeStep(int id, World world, byte step) {

            ref var state = ref this.componentsStates.arr[id];
            if (state - 1 == step) {

                var entity = world.GetEntityById(id);
                if (entity.generation == 0) return;
                
                state = 0;
                if (AllComponentTypes<TComponent>.isTag == false) this.components[id] = default;
                if (world.currentState.filters.HasInAnyFilter<TComponent>() == true) {

                    world.currentState.storage.archetypes.Remove<TComponent>(in entity);
                    world.UpdateFilterByStructComponent<TComponent>(in entity);

                }

                --world.currentState.structComponents.count;
                #if ENTITY_ACTIONS
                world.RaiseEntityActionOnRemove<TComponent>(in entity);
                #endif

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Validate(int capacity) {

            var result = false;
            if (AllComponentTypes<TComponent>.isTag == false) {

                if (ArrayUtils.Resize(capacity, ref this.components, true) == true) {

                    // Add into dirty map
                    result = true;

                }

            }

            if (ArrayUtils.WillResize(capacity, ref this.componentsStates) == true) {

                ArrayUtils.Resize(capacity, ref this.componentsStates, true);

            }

            if (AllComponentTypes<TComponent>.isShared == true) this.sharedGroups.Validate(capacity);

            if (AllComponentTypes<TComponent>.isVersioned == true) ArrayUtils.Resize(capacity, ref this.versions, true);
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ArrayUtils.Resize(capacity, ref this.versionsNoState, true);
            
            this.world.currentState.storage.archetypes.Validate(capacity);

            return result;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Validate(in Entity entity) {

            var result = false;
            var index = entity.id;
            if (AllComponentTypes<TComponent>.isTag == false) {

                if (ArrayUtils.Resize(index, ref this.components) == true) {

                    ArrayUtils.Resize(index, ref this.componentsStates, true);
                    // Add into dirty map
                    result = true;

                }

            }

            if (result == false && ArrayUtils.WillResize(index, ref this.componentsStates) == true) {

                ArrayUtils.Resize(index, ref this.componentsStates, true);

            }

            if (AllComponentTypes<TComponent>.isShared == true) this.sharedGroups.Validate(in entity);
            
            if (AllComponentTypes<TComponent>.isVersioned == true) ArrayUtils.Resize(index, ref this.versions, true);
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ArrayUtils.Resize(index, ref this.versionsNoState, true);

            this.world.currentState.storage.archetypes.Validate(in entity);

            if (this.world.currentState.filters.HasInAnyFilter<TComponent>() == true && this.world.HasData<TComponent>(in entity) == true) {

                this.world.currentState.storage.archetypes.Set<TComponent>(in entity);

            }

            return result;

        }

        public override bool HasType(System.Type type) {

            return typeof(TComponent) == type;

        }

        public override IStructComponentBase GetObject(Entity entity) {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            //var bucketId = this.GetBucketId(in entity.id, out var index);
            var index = entity.id;
            //this.CheckResize(in index);
            var bucketState = this.componentsStates.arr[index];
            if (bucketState > 0) {

                if (AllComponentTypes<TComponent>.isTag == false) {

                    return this.components[index];

                } else {

                    return AllComponentTypes<TComponent>.empty;

                }

            }

            return null;

        }

        public override System.Collections.Generic.ICollection<uint> GetSharedGroups(Entity entity) {

            return this.sharedGroups.GetGroups();

        }

        public override IStructComponentBase GetSharedObject(Entity entity, uint groupId) {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            if (this.sharedGroups.Has(entity.id, groupId) == false) return null;
            return this.sharedGroups.Get(entity.id, groupId);

        }

        public override bool SetSharedObject(Entity entity, IStructComponentBase data, uint groupId) {
            
            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            this.sharedGroups.Set(entity.id, groupId, (TComponent)data);
            
            var componentIndex = ComponentTypes<TComponent>.typeId;
            if (componentIndex >= 0) this.world.currentState.storage.archetypes.Set<TComponent>(in entity);

            return true;
            
        }

        public override bool SetObject(Entity entity, IStructComponentBase data) {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            //var bucketId = this.GetBucketId(in entity.id, out var index);
            var index = entity.id;
            //this.CheckResize(in index);
            if (AllComponentTypes<TComponent>.isTag == false) {

                ref var bucket = ref this.components[index];
                bucket = (TComponent)data;

            }

            ref var bucketState = ref this.componentsStates.arr[index];
            if (bucketState == 0) {

                bucketState = 1;

                var componentIndex = ComponentTypes<TComponent>.typeId;
                if (componentIndex >= 0) this.world.currentState.storage.archetypes.Set<TComponent>(in entity);

                return true;

            }

            return false;

        }

        public override bool RemoveObject(Entity entity) {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            //var bucketId = this.GetBucketId(in entity.id, out var index);
            var index = entity.id;
            //this.CheckResize(in index);
            ref var bucketState = ref this.componentsStates.arr[index];
            if (bucketState > 0) {

                if (AllComponentTypes<TComponent>.isTag == false) this.RemoveData(in entity);
                bucketState = 0;

                var componentIndex = ComponentTypes<TComponent>.typeId;
                if (componentIndex >= 0) this.world.currentState.storage.archetypes.Remove<TComponent>(in entity);

                return true;

            }

            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public virtual void RemoveData(in Entity entity) {

            this.components[entity.id] = default;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Has(in Entity entity) {

            #if WORLD_EXCEPTIONS
            if (entity.generation == 0) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            return this.componentsStates.arr[entity.id] > 0;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool HasShared(in Entity entity, uint groupId) {

            #if WORLD_EXCEPTIONS
            if (entity.generation == 0) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            return this.sharedGroups.Has(entity.id, groupId);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Remove(in Entity entity, bool clearAll = false) {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif
            
            var index = entity.id;
            if (index >= this.componentsStates.Length) return false;
            ref var bucketState = ref this.componentsStates.arr[index];
            if (bucketState > 0) {

                if (AllComponentTypes<TComponent>.isTag == false) this.RemoveData(in entity);
                bucketState = 0;

                if (clearAll == false) {

                    this.world.currentState.storage.archetypes.Remove<TComponent>(in entity);

                }

                return true;

            }

            return false;

        }

        public override void CopyFrom(in Entity from, in Entity to) {

            if (typeof(TComponent) == typeof(ME.ECS.Views.ViewComponent)) {

                var view = from.Read<ME.ECS.Views.ViewComponent>();
                if (view.viewInfo.entity == from) {

                    to.InstantiateView(view.viewInfo.prefabSourceId);

                }

                return;

            }

            this.componentsStates.arr[to.id] = this.componentsStates.arr[from.id];
            if (AllComponentTypes<TComponent>.isTag == false) this.components[to.id] = this.components[from.id];
            if (this.componentsStates.arr[from.id] > 0) {

                if (this.world.currentState.filters.HasInAnyFilter<TComponent>() == true) this.world.currentState.storage.archetypes.Set<TComponent>(in to);

            } else {

                if (this.world.currentState.filters.HasInAnyFilter<TComponent>() == true) this.world.currentState.storage.archetypes.Remove<TComponent>(in to);

            }

        }

        private struct ElementCopy : IArrayElementCopy<SharedGroupData> {

            public void Copy(SharedGroupData @from, ref SharedGroupData to) {
                
                to.data = from.data;
                
            }

            public void Recycle(SharedGroupData item) {
                
            }

        }

        public override void CopyFrom(StructRegistryBase other) {

            var _other = (StructComponents<TComponent>)other;
            ArrayUtils.Copy(in _other.componentsStates, ref this.componentsStates);
            ArrayUtils.Copy(_other.lifetimeData, ref this.lifetimeData);
            if (AllComponentTypes<TComponent>.isVersioned == true) ArrayUtils.Copy(_other.versions, ref this.versions);
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) _other.versionsNoState = this.versionsNoState;
            if (AllComponentTypes<TComponent>.isTag == false) ArrayUtils.Copy(in _other.components, ref this.components);

            if (AllComponentTypes<TComponent>.isShared == true) this.sharedGroups.CopyFrom(_other.sharedGroups, new ElementCopy());
            
        }

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class StructComponentsDisposable<TComponent> : StructComponents<TComponent> where TComponent : struct, IStructComponentBase, IComponentDisposable {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        protected override StructRegistryBase SpawnInstance() {

            return PoolRegistries.SpawnDisposable<TComponent>();

        }

        /*public override void OnRecycle() {

            if (this.sharedGroups.sharedGroups != null) {

                foreach (var kv in this.sharedGroups.sharedGroups) {

                    kv.Value.data.OnDispose();

                }

            }

            for (int i = 0; i < this.componentsStates.Length; ++i) {
                
                if (this.componentsStates.arr[i] > 0) this.components[i].OnDispose();
                
            }
            
            base.OnRecycle();
            
        }*/
        
        public override void RemoveData(in Entity entity) {

            this.components[entity.id].OnDispose();
            base.RemoveData(in entity);
            
        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class StructComponentsCopyable<TComponent> : StructComponents<TComponent> where TComponent : struct, IStructComponentBase, IStructCopyable<TComponent> {

        private struct CopyItem : IArrayElementCopyWithIndex<TComponent> {

            public BufferArray<byte> states;
            public BufferArray<byte> otherStates;

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Copy(int index, TComponent @from, ref TComponent to) {

                var hasFrom = (this.otherStates.isCreated == true && index >= 0 && index < this.otherStates.Length && this.otherStates.arr[index] > 0);
                var hasTo = (this.states.isCreated == true && index >= 0 && index < this.states.Length && this.states.arr[index] > 0);

                if (hasFrom == false && hasTo == false) {
                    
                    from.OnRecycle();
                    to.OnRecycle();
                    
                } else {

                    to.CopyFrom(in from);

                }

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Recycle(int index, ref TComponent item) {

                if (this.otherStates.isCreated == true && index >= 0 && index < this.otherStates.Length && this.otherStates.arr[index] > 0) {

                    item.OnRecycle();
                    item = default;

                }

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void RemoveData(in Entity entity) {

            this.components[entity.id].OnRecycle();
            this.components[entity.id] = default;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        protected override StructRegistryBase SpawnInstance() {

            return PoolRegistries.SpawnCopyable<TComponent>();

        }

        public override void OnRecycle() {

            if (this.sharedGroups.sharedGroups != null) {

                foreach (var kv in this.sharedGroups.sharedGroups) {

                    kv.Value.data.OnRecycle();

                }

            }

            for (int i = 0; i < this.componentsStates.Length; ++i) {
                
                if (this.componentsStates.arr[i] > 0) this.components[i].OnRecycle();
                
            }
            
            base.OnRecycle();
            
        }

        private struct ElementCopy : IArrayElementCopy<SharedGroupData> {

            public void Copy(SharedGroupData @from, ref SharedGroupData to) {
                
                to.data.CopyFrom(from.data);
                
            }

            public void Recycle(SharedGroupData item) {
                
                item.data.OnRecycle();
                
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void CopyFrom(StructRegistryBase other) {

            var _other = (StructComponents<TComponent>)other;
            ArrayUtils.Copy(in _other.componentsStates, ref this.componentsStates);
            ArrayUtils.Copy(_other.lifetimeData, ref this.lifetimeData);
            if (AllComponentTypes<TComponent>.isVersioned == true) ArrayUtils.Copy(_other.versions, ref this.versions);
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) _other.versionsNoState = this.versionsNoState;
            if (AllComponentTypes<TComponent>.isTag == false) {
                ArrayUtils.CopyWithIndex(_other.components, ref this.components, new CopyItem() {
                    states = this.componentsStates,
                    otherStates = _other.componentsStates,
                });
            }

            if (AllComponentTypes<TComponent>.isShared == true) this.sharedGroups.CopyFrom(_other.sharedGroups, new ElementCopy());
            
        }

        public override void CopyFrom(in Entity from, in Entity to) {

            if (typeof(TComponent) == typeof(ME.ECS.Views.ViewComponent)) {

                var view = from.Read<ME.ECS.Views.ViewComponent>();
                if (view.viewInfo.entity == from) {

                    to.InstantiateView(view.viewInfo.prefabSourceId);

                }

                return;

            }

            this.componentsStates.arr[to.id] = this.componentsStates.arr[from.id];
            if (AllComponentTypes<TComponent>.isTag == false) this.components[to.id].CopyFrom(this.components[from.id]);
            if (AllComponentTypes<TComponent>.isVersioned == true) this.versions.arr[to.id] = this.versions.arr[from.id];
            if (AllComponentTypes<TComponent>.isShared == true) this.sharedGroups.CopyFrom(in from, in to);
            if (this.componentsStates.arr[from.id] > 0) {

                if (this.world.currentState.filters.HasInAnyFilter<TComponent>() == true) this.world.currentState.storage.archetypes.Set<TComponent>(in to);

            } else {

                if (this.world.currentState.filters.HasInAnyFilter<TComponent>() == true) this.world.currentState.storage.archetypes.Remove<TComponent>(in to);

            }

        }

    }

    public interface IStructComponentsContainer {

        BufferArray<StructRegistryBase> GetAllRegistries();

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct StructComponentsContainer : IStructComponentsContainer {

        internal interface ITask {

            Entity GetEntity();
            void Execute();
            void Recycle();
            ITask Clone();
            void CopyFrom(ITask other);

        }

        internal class NextFrameTask<TComponent> : ITask, System.IEquatable<NextFrameTask<TComponent>> where TComponent : struct, IStructComponent {

            public Entity entity;
            public TComponent data;
            public ComponentLifetime lifetime;
            public float secondsLifetime;

            public Entity GetEntity() {

                return this.entity;

            }

            public void Execute() {

                if (this.entity.IsAlive() == true) {

                    Worlds.currentWorld.SetData(this.entity, in this.data, this.lifetime, this.secondsLifetime);

                }

            }

            public void CopyFrom(ITask other) {

                var _other = (NextFrameTask<TComponent>)other;
                this.entity = _other.entity;
                this.data = _other.data;
                this.lifetime = _other.lifetime;
                this.secondsLifetime = _other.secondsLifetime;

            }

            public void Recycle() {

                this.data = default;
                this.entity = default;
                this.lifetime = default;
                this.secondsLifetime = default;
                PoolClass<NextFrameTask<TComponent>>.Recycle(this);

            }

            public ITask Clone() {

                var copy = PoolClass<NextFrameTask<TComponent>>.Spawn();
                copy.CopyFrom(this);
                return copy;

            }

            public bool Equals(NextFrameTask<TComponent> other) {

                if (other == null) return false;
                return this.entity == other.entity && this.lifetime == other.lifetime;

            }

        }

        [ME.ECS.Serializer.SerializeField]
        internal CCList<ITask> nextFrameTasks;
        [ME.ECS.Serializer.SerializeField]
        internal CCList<ITask> nextTickTasks;

        [ME.ECS.Serializer.SerializeField]
        internal BufferArray<StructRegistryBase> list;
        [ME.ECS.Serializer.SerializeField]
        internal int count;
        [ME.ECS.Serializer.SerializeField]
        private bool isCreated;

        [ME.ECS.Serializer.SerializeField]
        internal HashSetCopyable<int> listLifetimeTick;
        [ME.ECS.Serializer.SerializeField]
        internal HashSetCopyable<int> listLifetimeFrame;
        [ME.ECS.Serializer.SerializeField]
        private ListCopyable<int> dirtyMap;

        public bool IsCreated() {

            return this.isCreated;

        }

        public void Initialize(bool freeze) {

            this.nextFrameTasks = PoolCCList<ITask>.Spawn();
            this.nextTickTasks = PoolCCList<ITask>.Spawn();
            this.dirtyMap = PoolListCopyable<int>.Spawn(10);
            this.listLifetimeTick = PoolHashSetCopyable<int>.Spawn(10);
            this.listLifetimeFrame = PoolHashSetCopyable<int>.Spawn(10);

            ArrayUtils.Resize(100, ref this.list, false);
            this.count = 0;
            this.isCreated = true;

        }

        public void Merge() {

            if (this.dirtyMap == null) return;

            for (int i = 0, count = this.dirtyMap.Count; i < count; ++i) {

                var idx = this.dirtyMap[i];
                if (idx < 0 || idx >= this.list.Count) {
                    UnityEngine.Debug.LogError($"DirtyMap: Idx {idx} is out of range [0..{this.list.Count})");
                    continue;
                }
                this.list.arr[idx].Merge();

            }

            this.dirtyMap.Clear();

        }

        public BufferArray<StructRegistryBase> GetAllRegistries() {

            return this.list;

        }

        public int Count {
            get {
                return this.count;
            }
        }

        public int GetCustomHash() {

            var hash = 0;
            for (int i = 0; i < this.list.Length; ++i) {

                hash ^= this.list.arr[i].GetCustomHash();

            }

            return hash;

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void SetEntityCapacity(int capacity) {

            // Update all known structs
            for (int i = 0, length = this.list.Length; i < length; ++i) {

                var item = this.list.arr[i];
                if (item != null) {

                    if (item.Validate(capacity) == true) {

                        this.dirtyMap.Add(i);

                    }

                }

            }

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void OnEntityCreate(in Entity entity) {

            // Update all known structs
            for (int i = 0, length = this.list.Length; i < length; ++i) {

                var item = this.list.arr[i];
                if (item != null) {

                    if (item.Validate(in entity) == true) {

                        this.dirtyMap.Add(i);

                    }

                }

            }

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void RemoveAll(in Entity entity) {

            for (int i = 0, cnt = this.nextFrameTasks.Count; i < cnt; ++i) {

                if (this.nextFrameTasks[i] == null) continue;

                if (this.nextFrameTasks[i].GetEntity() == entity) {

                    this.nextFrameTasks[i].Recycle();
                    this.nextFrameTasks.RemoveAt(i);
                    --i;
                    --cnt;

                }

            }

            for (int i = 0, cnt = this.nextTickTasks.Count; i < cnt; ++i) {

                if (this.nextTickTasks[i] == null) continue;

                if (this.nextTickTasks[i].GetEntity() == entity) {

                    this.nextTickTasks[i].Recycle();
                    this.nextTickTasks.RemoveAt(i);
                    --i;
                    --cnt;

                }

            }

            for (int i = 0, length = this.list.Length; i < length; ++i) {

                var item = this.list.arr[i];
                if (item != null && item.Has(in entity) == true) {

                    //var sw = new System.Diagnostics.Stopwatch();
                    //sw.Start();
                    //item.Validate(in entity);
                    item.Remove(in entity, clearAll: true);
                    //sw.Stop();
                    //if (sw.ElapsedMilliseconds > 10) UnityEngine.Debug.Log("REMOVE " + sw.ElapsedMilliseconds + " :: " + item.GetType());

                }

            }
            
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void Validate<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IStructComponentBase {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            this.Validate<TComponent>(code, isTag);
            var reg = (StructComponents<TComponent>)this.list.arr[code];
            reg.Validate(in entity);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void Validate<TComponent>(bool isTag = false) where TComponent : struct, IStructComponentBase {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            if (isTag == true) WorldUtilities.SetComponentAsTag<TComponent>();
            this.Validate<TComponent>(code, isTag);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void Validate<TComponent>(int code, bool isTag) where TComponent : struct, IStructComponentBase {

            if (ArrayUtils.WillResize(code, ref this.list) == true) {

                ArrayUtils.Resize(code, ref this.list, true);

            }

            if (this.list.arr[code] == null) {

                var instance = PoolRegistries.Spawn<TComponent>();
                this.list.arr[code] = instance;

            }

        }

        #region Copyable
        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateCopyable<TComponent>(bool isTag = false) where TComponent : struct, IStructComponentBase, IStructCopyable<TComponent> {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            if (isTag == true) WorldUtilities.SetComponentAsTag<TComponent>();
            this.ValidateCopyable<TComponent>(code, isTag);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateCopyable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IStructComponentBase, IStructCopyable<TComponent> {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            this.ValidateCopyable<TComponent>(code, isTag);
            var reg = (StructComponentsCopyable<TComponent>)this.list.arr[code];
            reg.Validate(in entity);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void ValidateCopyable<TComponent>(int code, bool isTag) where TComponent : struct, IStructComponentBase, IStructCopyable<TComponent> {

            if (ArrayUtils.WillResize(code, ref this.list) == true) {

                ArrayUtils.Resize(code, ref this.list, true);

            }

            if (this.list.arr[code] == null) {

                var instance = PoolRegistries.SpawnCopyable<TComponent>();
                this.list.arr[code] = instance;

            }

        }
        #endregion

        #region Disposable
        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateDisposable<TComponent>(bool isTag = false) where TComponent : struct, IStructComponentBase, IComponentDisposable {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            if (isTag == true) WorldUtilities.SetComponentAsTag<TComponent>();
            this.ValidateDisposable<TComponent>(code, isTag);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateDisposable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IStructComponentBase, IComponentDisposable {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            this.ValidateDisposable<TComponent>(code, isTag);
            var reg = (StructComponentsDisposable<TComponent>)this.list.arr[code];
            reg.Validate(in entity);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void ValidateDisposable<TComponent>(int code, bool isTag) where TComponent : struct, IStructComponentBase, IComponentDisposable {

            if (ArrayUtils.WillResize(code, ref this.list) == true) {

                ArrayUtils.Resize(code, ref this.list, true);

            }

            if (this.list.arr[code] == null) {

                var instance = PoolRegistries.SpawnDisposable<TComponent>();
                this.list.arr[code] = instance;

            }

        }
        #endregion

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasBit(in Entity entity, int bit) {

            if (bit < 0 || bit >= this.list.Length) return false;
            var reg = this.list.arr[bit];
            if (reg == null) return false;
            return reg.Has(in entity);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasSharedBit(in Entity entity, int bit, uint groupId) {

            if (bit < 0 || bit >= this.list.Length) return false;
            var reg = this.list.arr[bit];
            if (reg == null) return false;
            return reg.HasShared(in entity, groupId);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void OnRecycle() {

            for (int i = 0; i < this.nextFrameTasks.array.Length; ++i) {

                if (this.nextFrameTasks.array[i] == null) continue;

                for (int j = 0; j < this.nextFrameTasks.array[i].Length; ++j) {

                    if (this.nextFrameTasks.array[i][j] == null) continue;

                    this.nextFrameTasks.array[i][j].Recycle();

                }

            }

            PoolCCList<ITask>.Recycle(ref this.nextFrameTasks);

            for (int i = 0; i < this.nextTickTasks.array.Length; ++i) {

                if (this.nextTickTasks.array[i] == null) continue;

                for (int j = 0; j < this.nextTickTasks.array[i].Length; ++j) {

                    if (this.nextTickTasks.array[i][j] == null) continue;

                    this.nextTickTasks.array[i][j].Recycle();

                }

            }

            PoolCCList<ITask>.Recycle(ref this.nextTickTasks);

            for (int i = 0; i < this.list.Length; ++i) {

                if (this.list.arr[i] != null) {

                    PoolRegistries.Recycle(this.list.arr[i]);
                    this.list.arr[i] = null;

                }

            }

            PoolArray<StructRegistryBase>.Recycle(ref this.list);

            if (this.dirtyMap != null) PoolListCopyable<int>.Recycle(ref this.dirtyMap);
            if (this.listLifetimeTick != null) PoolHashSetCopyable<int>.Recycle(ref this.listLifetimeTick);
            if (this.listLifetimeFrame != null) PoolHashSetCopyable<int>.Recycle(ref this.listLifetimeFrame);

            this.count = default;
            this.isCreated = default;

        }

        public void CopyFrom(in Entity from, in Entity to) {

            for (int i = 0; i < this.list.Count; ++i) {

                var reg = this.list.arr[i];
                if (reg != null) reg.CopyFrom(in from, in to);

            }

        }

        private struct CopyTask : IArrayElementCopy<ITask> {

            public void Copy(ITask @from, ref ITask to) {

                if (from == null && to == null) return;

                if (from == null && to != null) {

                    to.Recycle();
                    to = null;

                } else if (to == null) {

                    to = from.Clone();

                } else {

                    to.CopyFrom(from);

                }

            }

            public void Recycle(ITask item) {

                item.Recycle();

            }

        }

        private struct CopyRegistry : IArrayElementCopy<StructRegistryBase> {

            public void Copy(StructRegistryBase @from, ref StructRegistryBase to) {

                if (from == null && to == null) return;

                if (from == null && to != null) {

                    to.Recycle();
                    to = null;

                } else if (to == null) {

                    to = from.Clone();

                } else {

                    from.Merge();
                    to.CopyFrom(from);

                }

            }

            public void Recycle(StructRegistryBase item) {

                PoolRegistries.Recycle(item);

            }

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void CopyFrom(StructComponentsContainer other) {

            //this.OnRecycle();

            ArrayUtils.Copy(other.dirtyMap, ref this.dirtyMap);
            ArrayUtils.Copy(other.listLifetimeTick, ref this.listLifetimeTick);
            ArrayUtils.Copy(other.listLifetimeFrame, ref this.listLifetimeFrame);
            
            this.count = other.count;
            this.isCreated = other.isCreated;

            {

                for (int i = 0; i < this.nextFrameTasks.array.Length; ++i) {

                    if (this.nextFrameTasks.array[i] == null) continue;

                    for (int j = 0; j < this.nextFrameTasks.array[i].Length; ++j) {

                        if (this.nextFrameTasks.array[i][j] == null) continue;

                        this.nextFrameTasks.array[i][j].Recycle();

                    }

                }

                PoolCCList<ITask>.Recycle(ref this.nextFrameTasks);

                for (int i = 0; i < this.nextTickTasks.array.Length; ++i) {

                    if (this.nextTickTasks.array[i] == null) continue;

                    for (int j = 0; j < this.nextTickTasks.array[i].Length; ++j) {

                        if (this.nextTickTasks.array[i][j] == null) continue;

                        this.nextTickTasks.array[i][j].Recycle();

                    }

                }

                PoolCCList<ITask>.Recycle(ref this.nextTickTasks);

                this.nextFrameTasks = PoolCCList<ITask>.Spawn();
                this.nextFrameTasks.InitialCopyOf(other.nextFrameTasks);
                for (int i = 0; i < other.nextFrameTasks.array.Length; ++i) {

                    if (other.nextFrameTasks.array[i] == null) {

                        this.nextFrameTasks.array[i] = null;
                        continue;

                    }

                    for (int j = 0; j < other.nextFrameTasks.array[i].Length; ++j) {

                        var item = other.nextFrameTasks.array[i][j];
                        if (item == null) {

                            this.nextFrameTasks.array[i][j] = null;
                            continue;

                        }

                        var copy = item.Clone();
                        this.nextFrameTasks.array[i][j] = copy;

                    }

                }

                this.nextTickTasks = PoolCCList<ITask>.Spawn();
                this.nextTickTasks.InitialCopyOf(other.nextTickTasks);
                for (int i = 0; i < other.nextTickTasks.array.Length; ++i) {

                    if (other.nextTickTasks.array[i] == null) {

                        this.nextTickTasks.array[i] = null;
                        continue;

                    }

                    for (int j = 0; j < other.nextTickTasks.array[i].Length; ++j) {

                        var item = other.nextTickTasks.array[i][j];
                        if (item == null) {

                            this.nextTickTasks.array[i][j] = null;
                            continue;

                        }

                        var copy = item.Clone();
                        this.nextTickTasks.array[i][j] = copy;

                    }

                }

            }

            //ArrayUtils.Copy(other.nextFrameTasks, ref this.nextFrameTasks, new CopyTask());
            //ArrayUtils.Copy(other.nextTickTasks, ref this.nextTickTasks, new CopyTask());
            ArrayUtils.Copy(other.list, ref this.list, new CopyRegistry());

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public partial class World {

        public ref StructComponentsContainer GetStructComponents() {

            return ref this.currentState.structComponents;

        }

        partial void OnSpawnStructComponents() { }

        partial void OnRecycleStructComponents() { }

        public void IncrementEntityVersion(in Entity entity) {

            this.currentState.storage.versions.Increment(in entity);
            
        }

        partial void SetEntityCapacityPlugin1(int capacity) {

            this.currentState.structComponents.SetEntityCapacity(capacity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        partial void CreateEntityPlugin1(Entity entity) {

            this.currentState.structComponents.OnEntityCreate(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        partial void DestroyEntityPlugin1(Entity entity) {

            this.currentState.structComponents.RemoveAll(in entity);
            this.currentState.storage.archetypes.Clear(in entity);

        }

        public void Register(ref StructComponentsContainer componentsContainer, bool freeze, bool restore) {

            if (componentsContainer.IsCreated() == false) {

                //componentsContainer = new StructComponentsContainer();
                componentsContainer.Initialize(freeze);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasDataBit(in Entity entity, int bit) {

            return this.currentState.structComponents.HasBit(in entity, bit);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasSharedDataBit(in Entity entity, int bit, uint groupId) {

            return this.currentState.structComponents.HasSharedBit(in entity, bit, groupId);

        }

        public void ValidateData<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IStructComponentBase {

            this.currentState.structComponents.Validate<TComponent>(in entity, isTag);

        }

        public void ValidateDataCopyable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IStructComponentBase, IStructCopyable<TComponent> {

            this.currentState.structComponents.ValidateCopyable<TComponent>(in entity, isTag);

        }

        public void ValidateDataDisposable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentDisposable {

            this.currentState.structComponents.ValidateDisposable<TComponent>(in entity, isTag);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void PlayTasksForFrame() {

            if (this.currentState.structComponents.nextFrameTasks.Count > 0) {

                for (int i = 0; i < this.currentState.structComponents.nextFrameTasks.Count; ++i) {

                    var task = this.currentState.structComponents.nextFrameTasks[i];
                    if (task == null) continue;

                    task.Execute();
                    task.Recycle();

                }

                this.currentState.structComponents.nextFrameTasks.ClearNoCC();

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void PlayTasksForTick() {

            if (this.currentState.structComponents.nextTickTasks.Count > 0) {

                for (int i = 0; i < this.currentState.structComponents.nextTickTasks.Count; ++i) {

                    var task = this.currentState.structComponents.nextTickTasks[i];
                    if (task == null) continue;

                    task.Execute();
                    task.Recycle();

                }

                this.currentState.structComponents.nextTickTasks.ClearNoCC();

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void UseLifetimeStep(ComponentLifetime step, float deltaTime) {

            var list = this.currentState.structComponents.listLifetimeTick;
            if (step == ComponentLifetime.NotifyAllModules || step == ComponentLifetime.NotifyAllModulesBelow) {

                list = this.currentState.structComponents.listLifetimeFrame;

            }
            
            if (list.Count > 0) {

                var bStep = (byte)step;
                foreach (var idx in list) {

                    ref var reg = ref this.currentState.structComponents.list.arr[idx];
                    if (reg == null) continue;

                    reg.UseLifetimeStep(this, bStep, deltaTime);

                }

                list.Clear();

            }
            
        }

        private void AddToLifetimeIndex<TComponent>(in Entity entity, ComponentLifetime lifetime, float secondsLifetime) where TComponent : struct, IStructComponentBase {
            
            var list = this.currentState.structComponents.listLifetimeTick;
            if (lifetime == ComponentLifetime.NotifyAllModules || lifetime == ComponentLifetime.NotifyAllModulesBelow) {

                list = this.currentState.structComponents.listLifetimeFrame;

            }
            
            var idx = WorldUtilities.GetAllComponentTypeId<TComponent>();
            if (list.Contains(idx) == false) list.Add(idx);
            
            ref var r = ref this.currentState.structComponents.list.arr[idx];
            var reg = (StructComponents<TComponent>)r;
            if (reg.lifetimeData == null) reg.lifetimeData = PoolListCopyable<StructComponents<TComponent>.LifetimeData>.Spawn(10);
            reg.lifetimeData.Add(new StructComponents<TComponent>.LifetimeData() {
                entityId = entity.id,
                lifetime = secondsLifetime,
            });
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public long GetDataVersion<TComponent>(in Entity entity) where TComponent : struct, IVersioned {
            
            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            if (AllComponentTypes<TComponent>.isVersioned == false) return 0L;
            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            return reg.versions.arr[entity.id];
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public uint GetDataVersionNoState<TComponent>(in Entity entity) where TComponent : struct, IVersionedNoState {
            
            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            if (AllComponentTypes<TComponent>.isVersionedNoState == false) return 0u;
            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            return reg.versionsNoState.arr[entity.id];
            
        }

        #region COMMON SHARED
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasSharedData<TComponent>() where TComponent : struct, IStructComponent {

            return this.HasData<TComponent>(in this.sharedEntity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref 
            //#if UNITY_EDITOR
            readonly
            //#endif
            TComponent ReadSharedData<TComponent>() where TComponent : struct, IStructComponent {

            return ref this.ReadData<TComponent>(in this.sharedEntity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref TComponent GetSharedData<TComponent>(bool createIfNotExists = true) where TComponent : struct, IStructComponent {

            return ref this.GetData<TComponent>(in this.sharedEntity, createIfNotExists);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetSharedData<TComponent>(in TComponent data) where TComponent : struct, IStructComponent {

            this.SetData(in this.sharedEntity, data);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetSharedData<TComponent>(in TComponent data, ComponentLifetime lifetime) where TComponent : struct, IStructComponent {

            this.SetData(in this.sharedEntity, data, lifetime);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveSharedData<TComponent>() where TComponent : struct, IStructComponent {

            this.RemoveData<TComponent>(in this.sharedEntity);

        }
        #endregion

        #region SHARED
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref readonly TComponent ReadSharedData<TComponent>(in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {
            
            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            if (groupId == 0 && entity.Has<ME.ECS.DataConfigs.DataConfig.SharedData>() == true) {

                ref readonly var sharedData = ref entity.Read<ME.ECS.DataConfigs.DataConfig.SharedData>();
                if (sharedData.archetypeToId.TryGetValue(AllComponentTypes<TComponent>.typeId, out var innerGroupId) == true && innerGroupId != 0) {

                    return ref this.ReadSharedData<TComponent>(in entity, innerGroupId);

                }

            }
            
            // Inline all manually
            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            if (reg.sharedGroups.Has(entity.id, groupId) == false) return ref AllComponentTypes<TComponent>.empty;
            return ref reg.sharedGroups.Get(entity.id, groupId);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasSharedData<TComponent>(in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {
            
            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            if (groupId == 0 && entity.Has<ME.ECS.DataConfigs.DataConfig.SharedData>() == true) {

                ref readonly var sharedData = ref entity.Read<ME.ECS.DataConfigs.DataConfig.SharedData>();
                if (sharedData.archetypeToId.TryGetValue(AllComponentTypes<TComponent>.typeId, out var innerGroupId) == true && innerGroupId != 0) {

                    return this.HasSharedData<TComponent>(in entity, innerGroupId);

                }

            }

            // Inline all manually
            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            if (reg.sharedGroups.Has(entity.id, groupId) == false) return false;
            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref TComponent GetSharedData<TComponent>(in Entity entity, uint groupId = 0u, bool createIfNotExists = true) where TComponent : struct, IComponentShared {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            if (groupId == 0 && entity.Has<ME.ECS.DataConfigs.DataConfig.SharedData>() == true) {

                ref readonly var sharedData = ref entity.Read<ME.ECS.DataConfigs.DataConfig.SharedData>();
                if (sharedData.archetypeToId.TryGetValue(AllComponentTypes<TComponent>.typeId, out var innerGroupId) == true && innerGroupId != 0) {

                    return ref this.GetSharedData<TComponent>(in entity, innerGroupId);

                }

            }

            // Inline all manually
            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            ref var state = ref reg.sharedGroups.Has(entity.id, groupId);
            var incrementVersion = (this.HasStep(WorldStep.LogicTick) == true || this.HasResetState() == false);
            if (state == false && createIfNotExists == true) {

                #if WORLD_EXCEPTIONS
                if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                    OutOfStateException.ThrowWorldStateCheck();

                }
                #endif

                incrementVersion = true;
                reg.sharedGroups.Set(entity.id, groupId);
                if (this.currentState.filters.HasInAnyFilter<TComponent>() == true) {

                    this.currentState.storage.archetypes.Set<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

                System.Threading.Interlocked.Increment(ref this.currentState.structComponents.count);
                #if ENTITY_ACTIONS
                this.RaiseEntityActionOnAdd<TComponent>(in entity);
                #endif

            }

            if (AllComponentTypes<TComponent>.isTag == true) return ref AllComponentTypes<TComponent>.empty;
            if (incrementVersion == true) {
                
                // Increment versions for all entities stored this group
                ref var states = ref reg.sharedGroups.sharedGroups.Get(groupId).states;
                for (int i = 0; i < states.Length; ++i) {
                    if (states.arr[i] == true) {
                        this.currentState.storage.versions.Increment(i);
                    }
                }

                if (AllComponentTypes<TComponent>.isVersioned == true) reg.versions.arr[entity.id] = this.GetCurrentTick();
                if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
                if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);

            }

            return ref reg.sharedGroups.Get(entity.id, groupId);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveSharedData<TComponent>(in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {
            
            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {
                
                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            if (groupId == 0 && entity.Has<ME.ECS.DataConfigs.DataConfig.SharedData>() == true) {

                ref readonly var sharedData = ref entity.Read<ME.ECS.DataConfigs.DataConfig.SharedData>();
                if (sharedData.archetypeToId.TryGetValue(AllComponentTypes<TComponent>.typeId, out var innerGroupId) == true && innerGroupId != 0) {

                    this.RemoveSharedData<TComponent>(in entity, innerGroupId);
                    return;

                }

            }

            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            ref var state = ref reg.sharedGroups.Has(entity.id, groupId);
            if (state == true) {
                
                // Increment versions for all entities stored this group
                ref var states = ref reg.sharedGroups.sharedGroups.Get(groupId).states;
                for (int i = 0; i < states.Length; ++i) {
                    if (states.arr[i] == true) {
                        this.currentState.storage.versions.Increment(i);
                    }
                }

                state = false;
                if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);
                if (this.currentState.filters.HasInAnyFilter<TComponent>() == true) {

                    this.currentState.storage.archetypes.Remove<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

                System.Threading.Interlocked.Decrement(ref this.currentState.structComponents.count);
                #if ENTITY_ACTIONS
                this.RaiseEntityActionOnRemove<TComponent>(in entity);
                #endif

            }

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetSharedData<TComponent>(in Entity entity, in TComponent data, uint groupId = 0u) where TComponent : struct, IComponentShared {

            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            // Inline all manually
            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            if (AllComponentTypes<TComponent>.isTag == false) {
                
                reg.sharedGroups.Set(entity.id, groupId, data);
                
                // Increment versions for all entities stored this group
                ref var states = ref reg.sharedGroups.sharedGroups.Get(groupId).states;
                for (int i = 0; i < states.Length; ++i) {
                    if (states.arr[i] == true) {
                        this.currentState.storage.versions.Increment(i);
                    }
                }

            }
            
            ref var state = ref reg.sharedGroups.Has(entity.id, groupId);
            if (state == false) {

                state = true;
                if (this.currentState.filters.HasInAnyFilter<TComponent>() == true) {

                    this.currentState.storage.archetypes.Set<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

                System.Threading.Interlocked.Increment(ref this.currentState.structComponents.count);
                
            }
            #if ENTITY_ACTIONS
            this.RaiseEntityActionOnAdd<TComponent>(in entity);
            #endif
            if (AllComponentTypes<TComponent>.isVersioned == true) reg.versions.arr[entity.id] = this.GetCurrentTick();
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
            if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetSharedData<TComponent>(in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {

            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            // Inline all manually
            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            if (AllComponentTypes<TComponent>.isTag == false) reg.sharedGroups.Set(entity.id, groupId);
            ref var state = ref reg.sharedGroups.Has(entity.id, groupId);
            if (state == false) {

                state = true;
                if (this.currentState.filters.HasInAnyFilter<TComponent>() == true) {

                    this.currentState.storage.archetypes.Set<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

                System.Threading.Interlocked.Increment(ref this.currentState.structComponents.count);
                
            }
            #if ENTITY_ACTIONS
            this.RaiseEntityActionOnAdd<TComponent>(in entity);
            #endif
            this.currentState.storage.versions.Increment(in entity);
            if (AllComponentTypes<TComponent>.isVersioned == true) reg.versions.arr[entity.id] = this.GetCurrentTick();
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
            if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);

        }
        
        public void SetSharedData(in Entity entity, in IStructComponentBase data, int dataIndex, uint groupId = 0u) {
            
            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            // Inline all manually
            ref var reg = ref this.currentState.structComponents.list.arr[dataIndex];
            if (reg.SetSharedObject(entity, data, groupId) == true) {

                this.currentState.storage.versions.Increment(in entity);
                reg.UpdateVersion(in entity);
                reg.UpdateVersionNoState(in entity);
                System.Threading.Interlocked.Increment(ref this.currentState.structComponents.count);

            }
            
        }
        #endregion

        #region GET/SET/READ
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasData<TComponent>(in Entity entity) where TComponent : struct, IStructComponentBase {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            return ((StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId]).componentsStates.arr[entity.id] > 0;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref 
            //#if UNITY_EDITOR
            readonly
            //#endif
            TComponent ReadData<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            // Inline all manually
            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            if (AllComponentTypes<TComponent>.isTag == true) return ref AllComponentTypes<TComponent>.empty;
            return ref reg.components[entity.id];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public IStructComponentBase ReadData(in Entity entity, int registryIndex) {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            // Inline all manually
            var reg = this.currentState.structComponents.list.arr[registryIndex];
            return reg.GetObject(entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref TComponent GetData<TComponent>(in Entity entity, bool createIfNotExists = true) where TComponent : struct, IStructComponent {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            // Inline all manually
            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            ref var state = ref reg.componentsStates.arr[entity.id];
            var incrementVersion = (this.HasStep(WorldStep.LogicTick) == true || this.HasResetState() == false);
            if (state == 0 && createIfNotExists == true) {

                #if WORLD_EXCEPTIONS
                if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                    OutOfStateException.ThrowWorldStateCheck();

                }
                #endif

                incrementVersion = true;
                state = 1;
                if (this.currentState.filters.HasInAnyFilter<TComponent>() == true) {

                    this.currentState.storage.archetypes.Set<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

                System.Threading.Interlocked.Increment(ref this.currentState.structComponents.count);
                #if ENTITY_ACTIONS
                this.RaiseEntityActionOnAdd<TComponent>(in entity);
                #endif

            }

            if (AllComponentTypes<TComponent>.isTag == true) return ref AllComponentTypes<TComponent>.empty;
            if (incrementVersion == true) {

                this.currentState.storage.versions.Increment(in entity);
                if (AllComponentTypes<TComponent>.isVersioned == true) reg.versions.arr[entity.id] = this.GetCurrentTick();
                if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
                if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);

            }

            return ref reg.components[entity.id];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref byte SetData<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            // Inline all manually
            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            if (AllComponentTypes<TComponent>.isTag == false) reg.RemoveData(in entity);
            ref var state = ref reg.componentsStates.arr[entity.id];
            if (state == 0) {

                state = 1;
                if (this.currentState.filters.HasInAnyFilter<TComponent>() == true) {

                    this.currentState.storage.archetypes.Set<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

                System.Threading.Interlocked.Increment(ref this.currentState.structComponents.count);

            }
            #if ENTITY_ACTIONS
            this.RaiseEntityActionOnAdd<TComponent>(in entity);
            #endif
            this.currentState.storage.versions.Increment(in entity);
            if (AllComponentTypes<TComponent>.isVersioned == true) reg.versions.arr[entity.id] = this.GetCurrentTick();
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
            if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);

            return ref state;

        }
        
        /// <summary>
        /// Lifetime default is Infinite
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="data"></param>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref byte SetData<TComponent>(in Entity entity, in TComponent data) where TComponent : struct, IStructComponent {

            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            // Inline all manually
            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            if (AllComponentTypes<TComponent>.isTag == false) reg.components[entity.id] = data;
            ref var state = ref reg.componentsStates.arr[entity.id];
            if (state == 0) {

                state = 1;
                if (this.currentState.filters.HasInAnyFilter<TComponent>() == true) {

                    this.currentState.storage.archetypes.Set<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

                System.Threading.Interlocked.Increment(ref this.currentState.structComponents.count);
                //this.AddComponentToFilter(entity);

            }
            #if ENTITY_ACTIONS
            this.RaiseEntityActionOnAdd<TComponent>(in entity);
            #endif
            this.currentState.storage.versions.Increment(in entity);
            if (AllComponentTypes<TComponent>.isVersioned == true) reg.versions.arr[entity.id] = this.GetCurrentTick();
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
            if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);

            return ref state;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetData<TComponent>(in Entity entity, ComponentLifetime lifetime) where TComponent : struct, IStructComponent {

            TComponent data = default;
            this.SetData<TComponent>(in entity, in data, lifetime);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetData<TComponent>(in Entity entity, in TComponent data, ComponentLifetime lifetime) where TComponent : struct, IStructComponent {

            this.SetData<TComponent>(in entity, in data, lifetime, 0f);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetData<TComponent>(in Entity entity, in TComponent data, ComponentLifetime lifetime, float secondsLifetime) where TComponent : struct, IStructComponent {
            
            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            if (lifetime == ComponentLifetime.NotifyAllModules ||
                lifetime == ComponentLifetime.NotifyAllSystems) {

                if (this.HasData<TComponent>(in entity) == true) return;

                var task = PoolClass<StructComponentsContainer.NextFrameTask<TComponent>>.Spawn();
                task.entity = entity;
                task.data = data;
                task.secondsLifetime = secondsLifetime;

                if (lifetime == ComponentLifetime.NotifyAllModules) {

                    task.lifetime = ComponentLifetime.NotifyAllModulesBelow;

                    this.currentState.structComponents.nextFrameTasks.Add(task);

                } else if (lifetime == ComponentLifetime.NotifyAllSystems) {

                    task.lifetime = ComponentLifetime.NotifyAllSystemsBelow;

                    if (this.currentState.structComponents.nextTickTasks.Contains(task) == false) {
                        
                        this.currentState.structComponents.nextTickTasks.Add(task);
                        
                    } else {
                        
                        task.Recycle();
                        
                    }

                }

            } else {

                ref var state = ref this.SetData(in entity, in data);

                if (lifetime == ComponentLifetime.Infinite) return;
                state = (byte)(lifetime + 1);

                this.AddToLifetimeIndex<TComponent>(in entity, lifetime, secondsLifetime);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveData(in Entity entity) {

            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {
                
                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            var changed = false;
            for (int i = 0; i < this.currentState.structComponents.list.Length; ++i) {

                var reg = this.currentState.structComponents.list.arr[i];
                if (reg != null && reg.Remove(in entity, false) == true) {

                    var bit = reg.GetTypeBit();
                    if (bit >= 0) this.currentState.storage.archetypes.Remove(in entity, bit);
                    System.Threading.Interlocked.Decrement(ref this.currentState.structComponents.count);
                    changed = true;

                }

            }

            if (changed == true) {

                this.currentState.storage.versions.Increment(in entity);
                this.RemoveComponentFromFilter(in entity);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveData<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {
                
                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            ref var state = ref reg.componentsStates.arr[entity.id];
            if (state > 0) {

                state = 0;
                this.currentState.storage.versions.Increment(in entity);
                if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);
                if (AllComponentTypes<TComponent>.isTag == false) reg.RemoveData(in entity);
                if (this.currentState.filters.HasInAnyFilter<TComponent>() == true) {

                    this.currentState.storage.archetypes.Remove<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

                System.Threading.Interlocked.Decrement(ref this.currentState.structComponents.count);
                #if ENTITY_ACTIONS
                this.RaiseEntityActionOnRemove<TComponent>(in entity);
                #endif

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetData(in Entity entity, in IStructComponentBase data, int dataIndex, int componentIndex) {

            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            // Inline all manually
            ref var reg = ref this.currentState.structComponents.list.arr[dataIndex];
            if (reg.SetObject(entity, data) == true) {

                this.currentState.storage.versions.Increment(in entity);
                reg.UpdateVersion(in entity);
                reg.UpdateVersionNoState(in entity);
                if (componentIndex >= 0) {

                    this.currentState.storage.archetypes.Set(in entity, componentIndex);
                    this.UpdateFilterByStructComponent(in entity, componentIndex);

                }
                System.Threading.Interlocked.Increment(ref this.currentState.structComponents.count);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveData(in Entity entity, int dataIndex, int componentIndex) {

            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {
                
                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            var reg = this.currentState.structComponents.list.arr[dataIndex];
            if (reg.RemoveObject(entity) == true) {

                this.currentState.storage.versions.Increment(in entity);
                if (componentIndex >= 0) {
                    
                    this.currentState.storage.archetypes.Remove(in entity, componentIndex);
                    this.UpdateFilterByStructComponent(in entity, componentIndex);
                    
                }
                System.Threading.Interlocked.Decrement(ref this.currentState.structComponents.count);

            }

        }
        #endregion

    }

}