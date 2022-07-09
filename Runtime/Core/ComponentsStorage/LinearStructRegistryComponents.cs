#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS {

    using ME.ECS.Collections;

    public interface IStructRegistryBase {

        IComponentBase GetObject(Entity entity);
        bool SetObject(in Entity entity, IComponentBase data, StorageType storageType);
        #if !SHARED_COMPONENTS_DISABLED
        IComponentBase GetSharedObject(Entity entity, uint groupId);
        bool SetSharedObject(in Entity entity, IComponentBase data, uint groupId);
        bool RemoveObject(in Entity entity, StorageType storageType);
        #endif
        bool HasType(System.Type type);

    }

    public struct Component<TComponent> where TComponent : struct, IComponentBase {

        public TComponent data;
        public byte state;
        public long version;

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract partial class StructRegistryBase : IStructRegistryBase, IPoolableRecycle {

        public World world {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                return Worlds.currentWorld;
            }
        }

        public abstract bool IsNeedToDispose();

        public abstract int GetTypeBit();
        public abstract int GetAllTypeBit();
        public abstract bool IsTag();

        public abstract long GetVersion(int entityId);
        public abstract bool HasChanged(int entityId);

        public abstract void UpdateVersion(in Entity entity);
        
        #if !COMPONENTS_VERSION_NO_STATE_DISABLED
        public abstract void UpdateVersionNoState(in Entity entity);
        #endif
        
        public abstract bool HasType(System.Type type);
        public abstract IComponentBase GetObject(Entity entity);
        public abstract bool SetObject(in Entity entity, IComponentBase data, StorageType storageType);
        public abstract bool SetObject(in Entity entity, UnsafeData buffer, StorageType storageType);
        public abstract bool RemoveObject(in Entity entity, StorageType storageType);
        #if !SHARED_COMPONENTS_DISABLED
        public abstract System.Collections.Generic.ICollection<uint> GetSharedGroups(Entity entity);
        public abstract IComponentBase GetSharedObject(Entity entity, uint groupId);
        public abstract bool SetSharedObject(in Entity entity, IComponentBase data, uint groupId);
        #endif

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void Merge();

        public abstract void CopyFrom(StructRegistryBase other);

        public abstract void CopyFrom(in Entity from, in Entity to);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract bool Validate(int capacity);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract bool Has(in Entity entity);

        #if !SHARED_COMPONENTS_DISABLED
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract bool HasShared(in Entity entity, uint groupId);
        #endif

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

        public virtual UnsafeData CreateObjectUnsafe(in Entity entity) {
            throw new System.Exception("Object must be unmanaged to use CreateObjectUnsafe");
        }

    }
    
    #if !SHARED_COMPONENTS_DISABLED
    public static class SharedGroupDataAPI<TComponent> where TComponent : struct, IComponentBase {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Validate(ref SharedDataStorage<TComponent> sharedStorage, int entityId) {
            
            ArrayUtils.Resize(entityId, ref sharedStorage.states, true);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void CopyFrom(in SharedDataStorage<TComponent> sharedStorage, in Entity from, in Entity to) {

            sharedStorage.states.arr[to.id] = sharedStorage.states.arr[from.id];

        }

    }

    internal static class SharedGroupsFalse {

        internal static bool alwaysFalse;

    }
    
    public static class SharedGroupsAPI<TComponent> where TComponent : struct, IComponentBase {

        public static System.Collections.Generic.ICollection<uint> GetGroups(ref SharedDataStorageGroup<TComponent> sharedStorage) {
            
            if (sharedStorage.sharedGroups == null) return null;
            return sharedStorage.sharedGroups.Keys;
            
        }

        public static void Validate(ref SharedDataStorageGroup<TComponent> sharedStorage, in Entity entity) {
            
            Initialize(ref sharedStorage);

        }

        public static void Validate(ref SharedDataStorageGroup<TComponent> sharedStorage, int capacity) {
            
            Initialize(ref sharedStorage);
            
        }

        public static void Initialize(ref SharedDataStorageGroup<TComponent> sharedStorage) {

            if (sharedStorage.sharedGroups == null) {
                
                #if MULTITHREAD_SUPPORT
                sharedStorage.sharedGroups = PoolCCDictionary<uint, SharedDataStorage<TComponent>>.Spawn(1);
                #else
                sharedStorage.sharedGroups = PoolDictionaryCopyable<uint, SharedDataStorage<TComponent>>.Spawn(1);
                #endif

            }

        }

        public static void OnRecycle(ref SharedDataStorageGroup<TComponent> sharedStorage) {

            if (sharedStorage.sharedGroups != null) {
                
                #if MULTITHREAD_SUPPORT
                PoolCCDictionary<uint, SharedDataStorage<TComponent>>.Recycle(ref this.sharedGroups);
                #else
                PoolDictionaryCopyable<uint, SharedDataStorage<TComponent>>.Recycle(ref sharedStorage.sharedGroups);
                #endif
                
            }
            
        }

        public static void CopyFrom<TElementCopy>(ref SharedDataStorageGroup<TComponent> sharedStorage, SharedDataStorageGroup<TComponent> other, TElementCopy copy) where TElementCopy : IArrayElementCopy<SharedDataStorage<TComponent>> {

            if (sharedStorage.sharedGroups == null && other.sharedGroups == null) {

                return;

            } else if (sharedStorage.sharedGroups == null && other.sharedGroups != null) {

                #if MULTITHREAD_SUPPORT
                sharedStorage.sharedGroups = PoolCCDictionary<uint, SharedDataStorage<TComponent>>.Spawn(other.sharedGroups.Count);
                #else
                sharedStorage.sharedGroups = PoolDictionaryCopyable<uint, SharedDataStorage<TComponent>>.Spawn(other.sharedGroups.Count);
                #endif

            } else if (sharedStorage.sharedGroups != null && other.sharedGroups == null) {

                foreach (var kv in sharedStorage.sharedGroups) {
                    
                    var item = kv.Value;
                    copy.Recycle(ref item);
                    
                }
                #if MULTITHREAD_SUPPORT
                PoolCCDictionary<uint, SharedDataStorage<TComponent>>.Recycle(ref sharedStorage.sharedGroups);
                #else
                PoolDictionaryCopyable<uint, SharedDataStorage<TComponent>>.Recycle(ref sharedStorage.sharedGroups);
                #endif
                return;

            }
            
            sharedStorage.sharedGroups.CopyFrom(other.sharedGroups, copy);
            
        }

        public static void CopyFrom(ref SharedDataStorageGroup<TComponent> sharedStorage, in Entity from, in Entity to) {

            foreach (var kv in sharedStorage.sharedGroups) {

                SharedGroupDataAPI<TComponent>.CopyFrom(kv.Value, in from, in to);
                
            }
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref bool Has(ref SharedDataStorageGroup<TComponent> sharedStorage, int entityId, uint groupId) {

            ref var group = ref sharedStorage.sharedGroups.Get(groupId);
            if (entityId < group.states.Length) return ref group.states.arr[entityId];
            SharedGroupsFalse.alwaysFalse = false;
            return ref SharedGroupsFalse.alwaysFalse;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref TComponent Get(ref SharedDataStorageGroup<TComponent> sharedStorage, int entityId, uint groupId) {

            return ref sharedStorage.sharedGroups.Get(groupId).data;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref SharedDataStorage<TComponent> GetGroup(ref SharedDataStorageGroup<TComponent> sharedStorage, uint groupId) {

            return ref sharedStorage.sharedGroups.Get(groupId);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Set(ref SharedDataStorageGroup<TComponent> sharedStorage, int entityId, uint groupId, TComponent data) {

            ref var group = ref sharedStorage.sharedGroups.Get(groupId);
            SharedGroupDataAPI<TComponent>.Validate(ref group, entityId);
            group.states.arr[entityId] = true;
            group.data = data;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Set(ref SharedDataStorageGroup<TComponent> sharedStorage, int entityId, uint groupId) {

            ref var group = ref sharedStorage.sharedGroups.Get(groupId);
            SharedGroupDataAPI<TComponent>.Validate(ref group, entityId);
            group.states.arr[entityId] = true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Remove(ref SharedDataStorageGroup<TComponent> sharedStorage, int entityId, uint groupId) {

            ref var group = ref sharedStorage.sharedGroups.Get(groupId);
            SharedGroupDataAPI<TComponent>.Validate(ref group, entityId);
            group.states.arr[entityId] = false;

        }

    }

    public struct SharedDataStorage<TComponent> where TComponent : struct, IComponentBase {

        [ME.ECS.Serializer.SerializeField]
        internal BufferArray<bool> states;
        [ME.ECS.Serializer.SerializeField]
        internal TComponent data;

    }

    public struct SharedDataStorageGroup<TComponent> where TComponent : struct, IComponentBase {

        #if MULTITHREAD_SUPPORT
        [ME.ECS.Serializer.SerializeField]
        internal CCDictionary<uint, SharedGroupData> sharedGroups;
        #else
        [ME.ECS.Serializer.SerializeField]
        internal DictionaryCopyable<uint, SharedDataStorage<TComponent>> sharedGroups;
        #endif

    }
    #endif

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract partial class StructComponentsBase<TComponent> : StructRegistryBase where TComponent : struct, IComponentBase {

        #if !COMPONENTS_VERSION_NO_STATE_DISABLED
        // We don't need to serialize this field
        internal BufferArray<uint> versionsNoState;
        #endif
        #if !SHARED_COMPONENTS_DISABLED
        // Shared data
        [ME.ECS.Serializer.SerializeField]
        internal SharedDataStorageGroup<TComponent> sharedStorage;
        #endif
        
        public override int GetCustomHash() => 0;

        public virtual void UpdateVersion(ref Component<TComponent> bucket) {
            
            if (AllComponentTypes<TComponent>.isVersioned == true) {
                bucket.version = this.world.GetCurrentTick();
            }

        }
        
        #if !COMPONENTS_VERSION_NO_STATE_DISABLED
        public uint GetVersionNotStated(in Entity entity) {

            return this.versionsNoState.arr[entity.id];

        }
        #endif

        public override bool IsTag() {

            return WorldUtilities.IsComponentAsTag<TComponent>();

        }

        public override int GetTypeBit() {

            return WorldUtilities.GetComponentTypeId<TComponent>();

        }

        public override int GetAllTypeBit() {

            return WorldUtilities.GetAllComponentTypeId<TComponent>();

        }

        #if !COMPONENTS_VERSION_NO_STATE_DISABLED
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void UpdateVersionNoState(in Entity entity) {

            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++this.versionsNoState.arr[entity.id];

        }
        #endif

        public override StructRegistryBase Clone() {

            var reg = this.SpawnInstance();
            ME.WeakRef.Reg(reg);
            this.Merge();
            reg.CopyFrom(this);
            return reg;

        }

        protected virtual StructRegistryBase SpawnInstance() {

            return PoolRegistries.Spawn<TComponent>();

        }

        public override void OnRecycle() {

            #if !COMPONENTS_VERSION_NO_STATE_DISABLED
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) PoolArray<uint>.Recycle(ref this.versionsNoState);
            #endif
            #if !SHARED_COMPONENTS_DISABLED
            if (AllComponentTypes<TComponent>.isShared == true) SharedGroupsAPI<TComponent>.OnRecycle(ref this.sharedStorage);
            #endif
            
        }

        public override bool Validate(int capacity) {
            
            #if !SHARED_COMPONENTS_DISABLED
            if (AllComponentTypes<TComponent>.isShared == true) SharedGroupsAPI<TComponent>.Validate(ref this.sharedStorage, capacity);
            #endif
            #if !COMPONENTS_VERSION_NO_STATE_DISABLED
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ArrayUtils.Resize(capacity, ref this.versionsNoState, true);
            #endif
            
            #if FILTERS_STORAGE_LEGACY
            this.world.currentState.storage.archetypes.Validate(capacity);
            #endif
            
            return false;

        }

        public override bool HasType(System.Type type) {

            return typeof(TComponent) == type;

        }

        #if !SHARED_COMPONENTS_DISABLED
        public override System.Collections.Generic.ICollection<uint> GetSharedGroups(Entity entity) {

            return SharedGroupsAPI<TComponent>.GetGroups(ref this.sharedStorage);

        }

        public override IComponentBase GetSharedObject(Entity entity, uint groupId) {

            if (SharedGroupsAPI<TComponent>.Has(ref this.sharedStorage, entity.id, groupId) == false) return null;
            return SharedGroupsAPI<TComponent>.Get(ref this.sharedStorage, entity.id, groupId);

        }

        public override bool SetSharedObject(in Entity entity, IComponentBase data, uint groupId) {
            
            E.IS_ALIVE(in entity);

            SharedGroupsAPI<TComponent>.Set(ref this.sharedStorage, entity.id, groupId, (TComponent)data);
            
            var componentIndex = ComponentTypes<TComponent>.typeId;
            if (componentIndex >= 0) this.world.currentState.storage.archetypes.Set<TComponent>(in entity);

            return true;
            
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool HasShared(in Entity entity, uint groupId) {

            E.IS_ALIVE(in entity);

            return SharedGroupsAPI<TComponent>.Has(ref this.sharedStorage, entity.id, groupId);

        }
        #endif

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void Replace(ref Component<TComponent> bucket, in TComponent data);
        
        #if !SHARED_COMPONENTS_DISABLED
        private struct ElementCopy : IArrayElementCopy<SharedDataStorage<TComponent>> {

            public void Copy(in SharedDataStorage<TComponent> @from, ref SharedDataStorage<TComponent> to) {
                
                to.data = from.data;
                ArrayUtils.Copy(from.states, ref to.states);
                
            }

            public void Recycle(ref SharedDataStorage<TComponent> item) {
                
                PoolArray<bool>.Recycle(ref item.states);
                item.data = default;
                item = default;
                
            }

        }
        #endif

        public override void CopyFrom(in Entity from, in Entity to) {

            if (typeof(TComponent) == typeof(ME.ECS.Views.ViewComponent)) {

                var view = from.Read<ME.ECS.Views.ViewComponent>();
                if (view.viewInfo.entity == from) {

                    to.ReplaceView(view.viewInfo.prefabSourceId);

                }

                return;

            }

            var taskList = PoolListCopyable<StructComponentsContainer.NextTickTask>.Spawn(2);
            foreach (var task in this.world.currentState.structComponents.nextTickTasks) {
                if (task.entity == from) {
                    var item = task;
                    item.entity = to;
                    taskList.Add(item);
                }
            }

            foreach (var task in taskList) {
                this.world.currentState.structComponents.nextTickTasks.Add(task);
            }
            PoolListCopyable<StructComponentsContainer.NextTickTask>.Recycle(ref taskList);
            
            #if !SHARED_COMPONENTS_DISABLED
            if (AllComponentTypes<TComponent>.isShared == true) SharedGroupsAPI<TComponent>.CopyFrom(ref this.sharedStorage, in from, in to);
            #endif
            if (this.CopyFromState(in from, in to) > 0) {

                if (ComponentTypes<TComponent>.typeId >= 0) this.world.currentState.storage.archetypes.Set<TComponent>(in to);

            } else {

                if (ComponentTypes<TComponent>.typeId >= 0) this.world.currentState.storage.archetypes.Remove<TComponent>(in to);

            }

        }

        protected abstract byte CopyFromState(in Entity from, in Entity to);

        public override void CopyFrom(StructRegistryBase other) {

            var _other = (StructComponentsBase<TComponent>)other;
            #if !COMPONENTS_VERSION_NO_STATE_DISABLED
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) _other.versionsNoState = this.versionsNoState;
            #endif
            #if !SHARED_COMPONENTS_DISABLED
            if (AllComponentTypes<TComponent>.isShared == true) SharedGroupsAPI<TComponent>.CopyFrom(ref this.sharedStorage, _other.sharedStorage, new ElementCopy());
            #endif
            
        }

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct StructComponentsContainer : IStructComponentsContainer {

        internal struct NextTickTask : System.IEquatable<NextTickTask> {

            public Entity entity;
            public UnsafeData data;
            public int dataIndex;
            public ComponentLifetime lifetime;
            public StorageType storageType;
            public tfloat secondsLifetime;
            public bool destroyEntity;
            
            public ComponentLifetime GetStep() => this.lifetime;
            
            public void NextStep() {

                if (this.entity.IsAlive() == true) {
                    
                    if (this.lifetime == ComponentLifetime.NotifyAllSystemsBelow) return;

                    if (this.dataIndex >= 0) Worlds.currentWorld.SetData(this.entity, this.data, this.dataIndex, this.storageType);
                    
                    if (this.lifetime == ComponentLifetime.NotifyAllSystems) {
                        this.lifetime = ComponentLifetime.NotifyAllSystemsBelow;
                    }
                    
                }

            }

            public bool Update(tfloat deltaTime) {

                if (this.entity.IsAlive() == false) return true;
                
                this.secondsLifetime -= deltaTime;
                if (this.secondsLifetime <= 0f) {

                    if (this.destroyEntity == false) {

                        Worlds.currentWorld.RemoveData(this.entity, this.dataIndex, this.storageType);

                    }

                    return true;

                }

                return false;

            }

            public bool IsValid() {

                return this.entity.IsEmpty() == false;

            }
            
            public void CopyFrom(in NextTickTask other) {

                this.entity = other.entity;
                this.storageType = other.storageType;
                this.dataIndex = other.dataIndex;
                this.data.CopyFrom(in other.data);
                this.lifetime = other.lifetime;
                this.secondsLifetime = other.secondsLifetime;
                this.destroyEntity = other.destroyEntity;

            }
            
            public void Recycle() {

                this.dataIndex = default;
                this.storageType = default;
                this.entity = default;
                this.lifetime = default;
                this.secondsLifetime = default;
                this.destroyEntity = default;
                this.data.Dispose();

            }
            
            public NextTickTask Clone() {

                var copy = new NextTickTask();
                copy.CopyFrom(this);
                return copy;

            }

            public bool Equals(NextTickTask other) {
                return this.entity.Equals(other.entity) &&
                       this.dataIndex == other.dataIndex &&
                       this.lifetime == other.lifetime &&
                       this.storageType == other.storageType &&
                       this.secondsLifetime == other.secondsLifetime &&
                       this.destroyEntity == other.destroyEntity;
            }

            public override bool Equals(object obj) {
                return obj is NextTickTask other && this.Equals(other);
            }

            public override int GetHashCode() {
                unchecked {
                    return (this.entity.GetHashCode() * 397) ^ this.dataIndex ^ (int)this.lifetime ^ (int)this.storageType ^ (int)(this.destroyEntity ? 1 : 0) ^ (int)(this.secondsLifetime * 1000f);
                }
            }

        }

        [ME.ECS.Serializer.SerializeField]
        internal HashSetCopyable<NextTickTask> nextTickTasks;

        [ME.ECS.Serializer.SerializeField]
        internal BufferArray<StructRegistryBase> list;
        [ME.ECS.Serializer.SerializeField]
        public EntitiesIndexer entitiesIndexer;
        [ME.ECS.Serializer.SerializeField]
        private bool isCreated;

        [ME.ECS.Serializer.SerializeField]
        private ListCopyable<int> dirtyMap;

        public bool IsCreated() {

            return this.isCreated;

        }

        public void Initialize(bool freeze) {

            this.nextTickTasks = PoolHashSetCopyable<NextTickTask>.Spawn();
            this.dirtyMap = PoolListCopyable<int>.Spawn(10);
            
            this.entitiesIndexer = new EntitiesIndexer();
            this.entitiesIndexer.Initialize(100);

            ArrayUtils.Resize(100, ref this.list, false);
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

        public int GetHash() {

            return this.list.Length;

        }

        public int GetCustomHash() {

            var hash = 0;
            for (int i = 0; i < this.list.Length; ++i) {

                if (this.list.arr[i] == null) continue;
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

            this.entitiesIndexer.Validate(capacity);
            
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

            this.entitiesIndexer.Validate(entity.id);
            
            // Update all known structs
            for (int i = 0, length = this.list.Length; i < length; ++i) {

                var item = this.list.arr[i];
                if (item != null) {

                    if (item.Validate(entity.id) == true) {

                        //this.dirtyMap.Add(i);

                    }

                }

            }

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public unsafe void RemoveAll(in Entity entity) {

            E.IS_ALIVE(in entity);

            var list = this.entitiesIndexer.Get(entity.id);
            if (list != null) {

                foreach (var index in list) {

                    var item = this.list.arr[index];
                    if (item != null) {

                        item.Remove(in entity, clearAll: true);

                    }

                }

                list.Clear();

            }

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void Validate<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            this.Validate<TComponent>(code, isTag);
            this.list.arr[code].Validate(entity.id);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void Validate<TComponent>(bool isTag = false) where TComponent : struct, IComponentBase {

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
        private void Validate<TComponent>(int code, bool isTag) where TComponent : struct, IComponentBase {

            if (ArrayUtils.WillResize(code, ref this.list) == true) {

                ArrayUtils.Resize(code, ref this.list, true);

            }

            if (this.list.arr[code] == null) {

                var instance = PoolRegistries.Spawn<TComponent>();
                ME.WeakRef.Reg(instance);
                this.list.arr[code] = instance;

            }

        }
        
        #region OneShot
        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateOneShot<TComponent>(bool isTag = false) where TComponent : struct, IComponentBase, IComponentOneShot {

            var code = WorldUtilities.GetOneShotComponentTypeId<TComponent>();
            if (isTag == true) WorldUtilities.SetComponentAsTag<TComponent>();
            this.ValidateOneShot<TComponent>(code, isTag);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateOneShot<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase, IComponentOneShot {

            var code = WorldUtilities.GetOneShotComponentTypeId<TComponent>();
            this.ValidateOneShot<TComponent>(code, isTag);
            this.list.arr[code].Validate(entity.id);

        }
        
        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void ValidateOneShot<TComponent>(int code, bool isTag) where TComponent : struct, IComponentBase, IComponentOneShot {

            if (ArrayUtils.WillResize(code, ref this.list) == true) {

                ArrayUtils.Resize(code, ref this.list, true);

            }

            if (this.list.arr[code] == null) {

                var instance = PoolRegistries.SpawnOneShot<TComponent>();
                this.list.arr[code] = instance;

            }

        }
        #endregion

        #region Blittable
        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateBlittable<TComponent>(bool isTag = false) where TComponent : struct, IComponentBase {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            if (isTag == true) WorldUtilities.SetComponentAsTag<TComponent>();
            this.ValidateBlittable<TComponent>(code, isTag);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateBlittable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            this.ValidateBlittable<TComponent>(code, isTag);
            var reg = (StructComponentsBlittable<TComponent>)this.list.arr[code];
            reg.Validate(entity.id);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void ValidateBlittable<TComponent>(int code, bool isTag) where TComponent : struct, IComponentBase {

            if (ArrayUtils.WillResize(code, ref this.list) == true) {

                ArrayUtils.Resize(code, ref this.list, true);

            }

            if (this.list.arr[code] == null) {

                var instance = PoolRegistries.SpawnBlittable<TComponent>();
                this.list.arr[code] = instance;

            }

        }
        #endregion

        #region BlittableCopyable
        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateBlittableCopyable<TComponent>(bool isTag = false) where TComponent : struct, IComponentBase, IStructCopyable<TComponent> {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            if (isTag == true) WorldUtilities.SetComponentAsTag<TComponent>();
            this.ValidateBlittableCopyable<TComponent>(code, isTag);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateBlittableCopyable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase, IStructCopyable<TComponent> {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            this.ValidateBlittableCopyable<TComponent>(code, isTag);
            this.list.arr[code].Validate(entity.id);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void ValidateBlittableCopyable<TComponent>(int code, bool isTag) where TComponent : struct, IComponentBase, IStructCopyable<TComponent> {

            if (ArrayUtils.WillResize(code, ref this.list) == true) {

                ArrayUtils.Resize(code, ref this.list, true);

            }

            if (this.list.arr[code] == null) {

                var instance = PoolRegistries.SpawnBlittableCopyable<TComponent>();
                this.list.arr[code] = instance;

            }

        }
        #endregion

        #region Copyable
        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateCopyable<TComponent>(bool isTag = false) where TComponent : struct, IComponentBase, IStructCopyable<TComponent> {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            if (isTag == true) WorldUtilities.SetComponentAsTag<TComponent>();
            this.ValidateCopyable<TComponent>(code, isTag);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateCopyable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase, IStructCopyable<TComponent> {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            this.ValidateCopyable<TComponent>(code, isTag);
            this.list.arr[code].Validate(entity.id);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void ValidateCopyable<TComponent>(int code, bool isTag) where TComponent : struct, IComponentBase, IStructCopyable<TComponent> {

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
        public void ValidateDisposable<TComponent>(bool isTag = false) where TComponent : struct, IComponentBase, IComponentDisposable {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            if (isTag == true) WorldUtilities.SetComponentAsTag<TComponent>();
            this.ValidateDisposable<TComponent>(code, isTag);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateDisposable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase, IComponentDisposable {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            this.ValidateDisposable<TComponent>(code, isTag);
            this.list.arr[code].Validate(entity.id);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void ValidateDisposable<TComponent>(int code, bool isTag) where TComponent : struct, IComponentBase, IComponentDisposable {

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

            return this.entitiesIndexer.Has(entity.id, bit);
            
        }

        #if !SHARED_COMPONENTS_DISABLED
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
        #endif

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void OnRecycle() {

            this.entitiesIndexer.Recycle();
            ArrayUtils.Recycle(ref this.nextTickTasks, new CopyTask());
            
            if (this.list.arr != null) {

                for (int i = 0; i < this.list.Length; ++i) {

                    if (this.list.arr[i] != null) {

                        PoolRegistries.Recycle(this.list.arr[i]);
                        this.list.arr[i] = null;

                    }

                }

                PoolArray<StructRegistryBase>.Recycle(ref this.list);

            }

            if (this.dirtyMap != null) PoolListCopyable<int>.Recycle(ref this.dirtyMap);

            this.isCreated = default;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void CopyFrom(in Entity from, in Entity to) {

            for (int i = 0; i < this.list.Count; ++i) {

                var reg = this.list.arr[i];
                if (reg != null) reg.CopyFrom(in from, in to);

            }

        }

        private struct CopyTask : IArrayElementCopy<NextTickTask> {

            public void Copy(in NextTickTask @from, ref NextTickTask to) {

                to.CopyFrom(from);

            }

            public void Recycle(ref NextTickTask item) {

                item.Recycle();
                item = default;

            }

        }

        private struct CopyRegistry : IArrayElementCopy<StructRegistryBase> {

            public void Copy(in StructRegistryBase @from, ref StructRegistryBase to) {

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

            public void Recycle(ref StructRegistryBase item) {

                PoolRegistries.Recycle(item);
                item = default;

            }

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void CopyFrom(StructComponentsContainer other) {

            //this.OnRecycle();

            this.entitiesIndexer.CopyFrom(in other.entitiesIndexer);

            ArrayUtils.Copy(other.dirtyMap, ref this.dirtyMap);

            this.isCreated = other.isCreated;

            {

                ArrayUtils.Copy(other.nextTickTasks, ref this.nextTickTasks, new CopyTask());
                
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

        private Filter entitiesOneShotFilter;
        
        public ref StructComponentsContainer GetStructComponents() {

            return ref this.currentState.structComponents;

        }

        public ref StructComponentsContainer GetNoStateStructComponents() {

            return ref this.structComponentsNoState;

        }

        partial void OnSpawnStructComponents() { }

        partial void OnRecycleStructComponents() { }

        public void IncrementEntityVersion(in Entity entity) {

            this.currentState.storage.versions.Increment(in entity);
            
        }

        partial void SetEntityCapacityPlugin1(int capacity) {

            this.structComponentsNoState.SetEntityCapacity(capacity);
            this.currentState.structComponents.SetEntityCapacity(capacity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        partial void CreateEntityPlugin1(Entity entity, bool isNew) {

            if (isNew == true) {
                
                this.structComponentsNoState.SetEntityCapacity(entity.id);
                this.currentState.structComponents.OnEntityCreate(in entity);
                
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        partial void DestroyEntityPlugin1(Entity entity) {

            this.structComponentsNoState.RemoveAll(in entity);
            this.currentState.structComponents.RemoveAll(in entity);
            this.currentState.storage.archetypes.Clear(in entity);

        }

        public void Register(ref StructComponentsContainer componentsContainer, bool freeze, bool restore) {

            if (componentsContainer.IsCreated() == false) {

                //componentsContainer = new StructComponentsContainer();
                componentsContainer.Initialize(freeze);

            }

            if (freeze == false) {

                if (this.sharedEntity.generation == 0 && this.sharedEntityInitialized == false) {

                    // Create shared entity which should store shared components
                    this.sharedEntity = this.AddEntity();

                }

                this.sharedEntityInitialized = true;

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasDataBit(in Entity entity, int bit) {

            return this.currentState.structComponents.HasBit(in entity, bit);

        }

        #if !SHARED_COMPONENTS_DISABLED
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasSharedDataBit(in Entity entity, int bit, uint groupId) {

            return this.currentState.structComponents.HasSharedBit(in entity, bit, groupId);

        }
        #endif

        public void ValidateData<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase {

            this.currentState.structComponents.Validate<TComponent>(in entity, isTag);

        }

        public void ValidateDataOneShot<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase, IComponentOneShot {

            this.structComponentsNoState.ValidateOneShot<TComponent>(in entity, isTag);

        }

        public void ValidateDataCopyable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase, IStructCopyable<TComponent> {

            this.currentState.structComponents.ValidateCopyable<TComponent>(in entity, isTag);

        }

        public void ValidateDataBlittable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase {

            this.currentState.structComponents.ValidateBlittable<TComponent>(in entity, isTag);

        }

        public void ValidateDataBlittableCopyable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase, IStructCopyable<TComponent> {

            this.currentState.structComponents.ValidateBlittableCopyable<TComponent>(in entity, isTag);

        }

        public void ValidateDataDisposable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentDisposable {

            this.currentState.structComponents.ValidateDisposable<TComponent>(in entity, isTag);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void UseLifetimeStep(ComponentLifetime step, tfloat deltaTime) {

            #if !ENTITY_TIMERS_DISABLED
            if (step == ComponentLifetime.NotifyAllSystemsBelow) {
                
                this.currentState.timers.Update(deltaTime);
                
            }
            #endif
        
            this.UseLifetimeStep(step, deltaTime, ref this.currentState.structComponents);
            this.UseLifetimeStep(step, deltaTime, ref this.structComponentsNoState);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void UseEntityFlags() {

            foreach (var entity in this.entitiesOneShotFilter) {

                entity.Destroy();

            }
            
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private unsafe void UseLifetimeStep(ComponentLifetime step, tfloat deltaTime, ref StructComponentsContainer structComponentsContainer) {

            var list = structComponentsContainer.nextTickTasks;
            if (list.Count > 0) {

                // We need to allocate temporary list to store entities
                // because on entity.Destroy we clean up all data including tasks list
                var tempList = stackalloc Entity[list.Count];
                var k = 0;
                var cnt = 0;
                foreach (ref var task in list) {
                    
                    var taskStep = task.GetStep();
                    if (taskStep != step) continue;
                    
                    if (step == ComponentLifetime.NotifyAllSystemsBelow) {

                        if (task.Update(deltaTime) == true) {
                            
                            // Remove task on complete
                            if (task.destroyEntity == true) tempList[k++] = task.entity;
                            task.Recycle();
                            task = default;
                            ++cnt;

                        }

                    } else if (step == ComponentLifetime.NotifyAllSystems) {
                        
                        task.NextStep();
                        
                    }
                    
                }

                for (int i = 0; i < k; ++i) {
                    tempList[i].Destroy();
                }
                
                if (cnt == list.Count) {
                    
                    // All is null
                    list.Clear();
                    
                }

            }
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public long GetDataVersion<TComponent>(in Entity entity) where TComponent : struct, IVersioned {
            
            E.IS_ALIVE(in entity);

            if (AllComponentTypes<TComponent>.isVersioned == false) return 0L;
            var reg = (StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            return reg.GetVersion(entity.id);
            
        }

        #if !COMPONENTS_VERSION_NO_STATE_DISABLED
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public uint GetDataVersionNoState<TComponent>(in Entity entity) where TComponent : struct, IVersionedNoState {
            
            E.IS_ALIVE(in entity);

            if (AllComponentTypes<TComponent>.isVersionedNoState == false) return 0u;
            var reg = (StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            return reg.versionsNoState.arr[entity.id];
            
        }
        #endif

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
        public bool HasSharedDataBit(int bit) {

            return this.HasDataBit(in this.sharedEntity, bit);

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
        public ref TComponent GetSharedData<TComponent>() where TComponent : struct, IStructComponent {

            return ref this.GetData<TComponent>(in this.sharedEntity);

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
        public void SetSharedData<TComponent>(in TComponent data, ComponentLifetime lifetime) where TComponent : unmanaged, IStructComponent {

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
        #if !SHARED_COMPONENTS_DISABLED
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref readonly TComponent ReadSharedData<TComponent>(in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {
            
            E.IS_ALIVE(in entity);
            E.IS_TAG<TComponent>(in entity);

            if (groupId == 0 && entity.Has<ME.ECS.DataConfigs.DataConfig.SharedData>() == true) {

                ref readonly var sharedData = ref entity.Read<ME.ECS.DataConfigs.DataConfig.SharedData>();
                if (sharedData.archetypeToId.TryGetValue(AllComponentTypes<TComponent>.typeId, out var innerGroupId) == true && innerGroupId != 0) {

                    return ref this.ReadSharedData<TComponent>(in entity, innerGroupId);

                }

            }
            
            // Inline all manually
            var reg = (StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            #if WORLD_EXCEPTIONS
            if (SharedGroupsAPI<TComponent>.Has(ref reg.sharedStorage, entity.id, groupId) == false) {
                
                EmptyDataException.Throw(entity);
                
            }
            #endif
            return ref SharedGroupsAPI<TComponent>.Get(ref reg.sharedStorage, entity.id, groupId);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasSharedData<TComponent>(in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {
            
            E.IS_ALIVE(in entity);

            if (groupId == 0 && entity.Has<ME.ECS.DataConfigs.DataConfig.SharedData>() == true) {

                ref readonly var sharedData = ref entity.Read<ME.ECS.DataConfigs.DataConfig.SharedData>();
                if (sharedData.archetypeToId.TryGetValue(AllComponentTypes<TComponent>.typeId, out var innerGroupId) == true && innerGroupId != 0) {

                    return this.HasSharedData<TComponent>(in entity, innerGroupId);

                }

            }

            // Inline all manually
            var reg = (StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            if (SharedGroupsAPI<TComponent>.Has(ref reg.sharedStorage, entity.id, groupId) == false) return false;
            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref TComponent GetSharedData<TComponent>(in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {

            E.IS_ALIVE(in entity);
            E.IS_TAG<TComponent>(in entity);

            if (groupId == 0 && entity.Has<ME.ECS.DataConfigs.DataConfig.SharedData>() == true) {

                ref readonly var sharedData = ref entity.Read<ME.ECS.DataConfigs.DataConfig.SharedData>();
                if (sharedData.archetypeToId.TryGetValue(AllComponentTypes<TComponent>.typeId, out var innerGroupId) == true && innerGroupId != 0) {

                    return ref this.GetSharedData<TComponent>(in entity, innerGroupId);

                }

            }

            // Inline all manually
            var reg = (StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            ref var state = ref SharedGroupsAPI<TComponent>.Has(ref reg.sharedStorage, entity.id, groupId);
            var incrementVersion = (this.HasStep(WorldStep.LogicTick) == true || this.HasResetState() == false);
            if (state == false) {

                E.IS_LOGIC_STEP(this);

                if (this.currentState.structComponents.entitiesIndexer.GetCount(entity.id) == 0 &&
                    (this.currentState.storage.flags.Get(entity.id) & (byte)EntityFlag.DestroyWithoutComponents) != 0) {
                    entity.RemoveOneShot<IsEntityEmptyOneShot>();
                }
                
                incrementVersion = true;
                SharedGroupsAPI<TComponent>.Set(ref reg.sharedStorage, entity.id, groupId);
                this.currentState.structComponents.entitiesIndexer.Set(entity.id, AllComponentTypes<TComponent>.typeId);
                if (ComponentTypes<TComponent>.typeId >= 0) {

                    this.currentState.storage.archetypes.Set<TComponent>(in entity);
                    this.AddFilterByStructComponent<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

            }

            if (ComponentTypes<TComponent>.isFilterLambda == true && ComponentTypes<TComponent>.typeId >= 0) {

                this.ValidateFilterByStructComponent<TComponent>(in entity, true);
                
            }
            
            if (incrementVersion == true) {
                
                // Increment versions for all entities stored this group
                ref var states = ref SharedGroupsAPI<TComponent>.GetGroup(ref reg.sharedStorage, groupId).states;
                for (int i = 0; i < states.Length; ++i) {
                    if (states.arr[i] == true) {
                        this.currentState.storage.versions.Increment(i);
                    }
                }

                if (AllComponentTypes<TComponent>.isBlittable == true) {

                    var regType = (StructComponentsBlittable<TComponent>)reg;
                    ref var bucket = ref regType.components[entity.id];
                    reg.UpdateVersion(ref bucket);

                } else {
                
                    var regType = (StructComponents<TComponent>)reg;
                    ref var bucket = ref regType.components[entity.id];
                    reg.UpdateVersion(ref bucket);

                }
                
                if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
                if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);

            }

            return ref SharedGroupsAPI<TComponent>.Get(ref reg.sharedStorage, entity.id, groupId);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveSharedData<TComponent>(in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {
            
            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);
            E.IS_TAG<TComponent>(in entity);
            
            if (groupId == 0 && entity.Has<ME.ECS.DataConfigs.DataConfig.SharedData>() == true) {

                ref readonly var sharedData = ref entity.Read<ME.ECS.DataConfigs.DataConfig.SharedData>();
                if (sharedData.archetypeToId.TryGetValue(AllComponentTypes<TComponent>.typeId, out var innerGroupId) == true && innerGroupId != 0) {

                    this.RemoveSharedData<TComponent>(in entity, innerGroupId);
                    return;

                }

            }

            var reg = (StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            ref var state = ref SharedGroupsAPI<TComponent>.Has(ref reg.sharedStorage, entity.id, groupId);
            if (state == true) {
                
                // Increment versions for all entities stored this group
                ref var states = ref SharedGroupsAPI<TComponent>.GetGroup(ref reg.sharedStorage, groupId).states;
                for (int i = 0; i < states.Length; ++i) {
                    if (states.arr[i] == true) {
                        this.currentState.storage.versions.Increment(i);
                    }
                }

                state = false;
                this.currentState.structComponents.entitiesIndexer.Remove(entity.id, AllComponentTypes<TComponent>.typeId);
                if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);
                if (ComponentTypes<TComponent>.typeId >= 0) {

                    this.currentState.storage.archetypes.Remove<TComponent>(in entity);
                    this.RemoveFilterByStructComponent<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

            }

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetSharedData<TComponent>(in Entity entity, in TComponent data, uint groupId = 0u) where TComponent : struct, IComponentShared {
            
            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);
            
            // Inline all manually
            var reg = (StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            if (AllComponentTypes<TComponent>.isTag == false) {
                
                SharedGroupsAPI<TComponent>.Set(ref reg.sharedStorage, entity.id, groupId, data);
                
                // Increment versions for all entities stored this group
                ref var states = ref SharedGroupsAPI<TComponent>.GetGroup(ref reg.sharedStorage, groupId).states;
                for (int i = 0; i < states.Length; ++i) {
                    if (states.arr[i] == true) {
                        this.currentState.storage.versions.Increment(i);
                    }
                }

            }
            
            ref var state = ref SharedGroupsAPI<TComponent>.Has(ref reg.sharedStorage, entity.id, groupId);
            if (state == false) {

                state = true;
                this.currentState.structComponents.entitiesIndexer.Set(entity.id, AllComponentTypes<TComponent>.typeId);
                if (ComponentTypes<TComponent>.typeId >= 0) {

                    this.currentState.storage.archetypes.Set<TComponent>(in entity);
                    this.AddFilterByStructComponent<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

            }
            
            if (ComponentTypes<TComponent>.isFilterLambda == true && ComponentTypes<TComponent>.typeId >= 0) {

                this.ValidateFilterByStructComponent<TComponent>(in entity);
                
            }
            
            if (AllComponentTypes<TComponent>.isBlittable == true) {

                var regType = (StructComponentsBlittable<TComponent>)reg;
                ref var bucket = ref regType.components[entity.id];
                reg.UpdateVersion(ref bucket);

            } else {
                
                var regType = (StructComponents<TComponent>)reg;
                ref var bucket = ref regType.components[entity.id];
                reg.UpdateVersion(ref bucket);

            }

            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
            if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetSharedData<TComponent>(in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {

            TComponent data = default;
            this.SetSharedData<TComponent>(in entity, in data, groupId);
            
        }
        
        public void SetSharedData(in Entity entity, in IComponentBase data, int dataIndex, uint groupId = 0u) {
            
            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);
            
            // Inline all manually
            ref var reg = ref this.currentState.structComponents.list.arr[dataIndex];
            if (reg.SetSharedObject(entity, data, groupId) == true) {

                this.currentState.storage.versions.Increment(in entity);
                reg.UpdateVersion(in entity);
                reg.UpdateVersionNoState(in entity);

            }
            
        }
        #endif
        #endregion

        #region TIMERS
        #if !ENTITY_TIMERS_DISABLED
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetTimer(in Entity entity, uint index, tfloat time) {

            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);
            
            this.currentState.timers.Set(in entity, index, time);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref tfloat GetTimer(in Entity entity, uint index) {

            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);

            return ref this.currentState.timers.Get(in entity, index);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public tfloat ReadTimer(in Entity entity, uint index) {
            
            E.IS_ALIVE(in entity);

            return this.currentState.timers.Read(in entity, index);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool RemoveTimer(in Entity entity, uint index) {
            
            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);

            return this.currentState.timers.Remove(in entity, index);

        }
        #endif
        #endregion

        #region HAS/GET/SET/READ/REMOVE
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool TryReadData<TComponent>(in Entity entity, out TComponent component) where TComponent : struct, IStructComponent {

            E.IS_ALIVE(in entity);
            E.IS_TAG<TComponent>(in entity);

            if (AllComponentTypes<TComponent>.isBlittable == true) {
                
                // Inline all manually
                var reg = (StructComponentsBlittable<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                var c = reg.components[entity.id];
                component = c.data;
                return c.state > 0;

            } else {

                var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                var c = reg.components[entity.id];
                component = c.data;
                return c.state > 0;

            }

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasData<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            E.IS_ALIVE(in entity);

            return this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId].Has(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref readonly TComponent ReadData<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            E.IS_ALIVE(in entity);
            E.IS_TAG<TComponent>(in entity);

            if (AllComponentTypes<TComponent>.isBlittable == true) {
                
                // Inline all manually
                var reg = (StructComponentsBlittable<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                return ref reg.components[entity.id].data;

            } else {

                // Inline all manually
                var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                return ref reg.components[entity.id].data;

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public IComponentBase ReadData(in Entity entity, int registryIndex) {

            E.IS_ALIVE(in entity);

            return this.currentState.structComponents.list.arr[registryIndex].GetObject(entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public UnsafeData ReadDataUnsafe(in Entity entity, int registryIndex) {

            E.IS_ALIVE(in entity);

            var reg = this.currentState.structComponents.list.arr[registryIndex];
            return reg.CreateObjectUnsafe(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref TComponent GetData<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);
            E.IS_TAG<TComponent>(in entity);
            
            if (AllComponentTypes<TComponent>.isBlittable == true) {
                
                // Inline all manually
                var reg = (StructComponentsBlittable<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                return ref DataBlittableBufferUtils.PushGet_INTERNAL(this, in entity, reg, StorageType.Default);

            } else {

                // Inline all manually
                var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                return ref DataBufferUtils.PushGet_INTERNAL(this, in entity, reg, StorageType.Default);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetData<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            TComponent data = default;
            this.SetData(in entity, data);
            
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
        public void SetData<TComponent>(in Entity entity, in TComponent data) where TComponent : struct, IStructComponent {

            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);

            if (AllComponentTypes<TComponent>.isBlittable == true) {
                
                // Inline all manually
                var reg = (StructComponentsBlittable<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                DataBlittableBufferUtils.PushSet_INTERNAL(this, in entity, reg, in data, StorageType.Default);

            } else {

                // Inline all manually
                var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                DataBufferUtils.PushSet_INTERNAL(this, in entity, reg, in data, StorageType.Default);

            }

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetData<TComponent>(in Entity entity, ComponentLifetime lifetime) where TComponent : unmanaged, IStructComponent {

            TComponent data = default;
            this.SetData(in entity, in data, lifetime);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetData<TComponent>(in Entity entity, in TComponent data, ComponentLifetime lifetime) where TComponent : unmanaged, IStructComponent {

            this.SetData(in entity, in data, lifetime, 0f);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetData<TComponent>(in Entity entity, in TComponent data, ComponentLifetime lifetime, tfloat secondsLifetime) where TComponent : unmanaged, IStructComponent {

            this.SetData(ref this.currentState.structComponents, in entity, in data, lifetime, secondsLifetime);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal void SetData<TComponent>(ref StructComponentsContainer container, in Entity entity, in TComponent data, ComponentLifetime lifetime, tfloat secondsLifetime, bool addTaskOnly = false) where TComponent : unmanaged, IStructComponent {
            
            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);

            if (lifetime == ComponentLifetime.Infinite) {

                if (addTaskOnly == false) this.SetData(in entity, in data);
                return;
                
            }
            
            if (addTaskOnly == false && this.HasData<TComponent>(in entity) == true) return;

            var task = new StructComponentsContainer.NextTickTask {
                entity = entity,
                dataIndex = AllComponentTypes<TComponent>.typeId,
                secondsLifetime = secondsLifetime,
                lifetime = lifetime,
            };

            switch (lifetime) {
                case ComponentLifetime.NotifyAllSystems: {
                    break;
                }

                case ComponentLifetime.NotifyAllSystemsBelow: {
                    if (addTaskOnly == false) this.SetData(in entity, in data);
                    break;
                }

            }

            if (container.nextTickTasks.Add(task) == false) {

                task.Recycle();

            } else {

                ref var val = ref container.nextTickTasks.GetValue(task);
                val.data = new UnsafeData().Set(data);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveData<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);

            if (AllComponentTypes<TComponent>.isBlittable == true) {
             
                var reg = (StructComponentsBlittable<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                DataBlittableBufferUtils.PushRemove_INTERNAL(this, in entity, reg, StorageType.Default);
   
            } else {

                var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                DataBufferUtils.PushRemove_INTERNAL(this, in entity, reg, StorageType.Default);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetData(in Entity entity, in IComponentBase data, int dataIndex) {

            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);

            // Inline all manually
            ref var reg = ref this.currentState.structComponents.list.arr[dataIndex];
            reg.SetObject(entity, data, StorageType.Default);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal void SetData(in Entity entity, UnsafeData buffer, int dataIndex, StorageType storageType) {

            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);

            // Inline all manually
            ref var reg = ref this.currentState.structComponents.list.arr[dataIndex];
            reg.SetObject(entity, buffer, storageType);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveData(in Entity entity, int dataIndex, StorageType storageType = StorageType.Default) {

            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);

            if (storageType == StorageType.Default) {

                var reg = this.currentState.structComponents.list.arr[dataIndex];
                if (reg.RemoveObject(entity, storageType) == true) {

                    this.currentState.storage.versions.Increment(in entity);

                }

            } else if (storageType == StorageType.NoState) {
                
                var reg = this.structComponentsNoState.list.arr[dataIndex];
                if (reg.RemoveObject(entity, storageType) == true) {

                    this.currentState.storage.versions.Increment(in entity);

                }

            }

        }
        #endregion

    }

}
