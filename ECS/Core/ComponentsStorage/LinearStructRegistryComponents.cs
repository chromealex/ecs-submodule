#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using ME.ECS.Collections;

    public interface IStructRegistryBase {

        IStructComponentBase GetObject(Entity entity);
        bool SetObject(in Entity entity, IStructComponentBase data);
        IStructComponentBase GetSharedObject(Entity entity, uint groupId);
        bool SetSharedObject(in Entity entity, IStructComponentBase data, uint groupId);
        bool RemoveObject(in Entity entity);
        bool HasType(System.Type type);

    }

    public struct Component<TComponent> where TComponent : struct, IStructComponentBase {

        public TComponent data;
        public byte state;
        public long version;

    }

    public struct LifetimeData {

        public int entityId;
        public float lifetime;

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract partial class StructRegistryBase : IStructRegistryBase, IPoolableRecycle {

        public World world {
            get {
                return Worlds.currentWorld;
            }
        }

        public abstract int GetTypeBit();
        public abstract int GetAllTypeBit();
        public abstract bool IsTag();

        public abstract void UpdateVersion(in Entity entity);
        public abstract void UpdateVersionNoState(in Entity entity);
        
        public abstract bool HasType(System.Type type);
        public abstract IStructComponentBase GetObject(Entity entity);
        public abstract bool SetObject(in Entity entity, IStructComponentBase data);
        public abstract System.Collections.Generic.ICollection<uint> GetSharedGroups(Entity entity);
        public abstract IStructComponentBase GetSharedObject(Entity entity, uint groupId);
        public abstract bool SetSharedObject(in Entity entity, IStructComponentBase data, uint groupId);
        public abstract bool RemoveObject(in Entity entity);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void Merge();

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract bool UseLifetimeStep(World world, byte step, float deltaTime);

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

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract partial class StructComponentsBase<TComponent> : StructRegistryBase where TComponent : struct, IStructComponentBase {

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

        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<LifetimeData> lifetimeData;
        // We don't need to serialize this field
        internal BufferArray<uint> versionsNoState;
        // Shared data
        [ME.ECS.Serializer.SerializeField]
        internal SharedGroups sharedGroups;
        
        public override int GetCustomHash() => 0;

        public abstract long GetVersion(in Entity entity);

        public abstract void UpdateVersion(ref Component<TComponent> bucket);
        
        public uint GetVersionNotStated(in Entity entity) {

            return this.versionsNoState.arr[entity.id];

        }

        public override bool IsTag() {

            return WorldUtilities.IsComponentAsTag<TComponent>();

        }

        public override int GetTypeBit() {

            return WorldUtilities.GetComponentTypeId<TComponent>();

        }

        public override int GetAllTypeBit() {

            return WorldUtilities.GetAllComponentTypeId<TComponent>();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void UpdateVersionNoState(in Entity entity) {

            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++this.versionsNoState.arr[entity.id];

        }

        public override void Recycle() {

            PoolRegistries.Recycle(this);

        }

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

            if (AllComponentTypes<TComponent>.isVersionedNoState == true) PoolArray<uint>.Recycle(ref this.versionsNoState);
            if (this.lifetimeData != null) PoolListCopyable<LifetimeData>.Recycle(ref this.lifetimeData);
            
            if (AllComponentTypes<TComponent>.isShared == true) this.sharedGroups.OnRecycle();
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool UseLifetimeStep(World world, byte step, float deltaTime) {

            if (this.lifetimeData != null) {

                for (int i = 0, cnt = this.lifetimeData.Count; i < cnt; ++i) {

                    ref var data = ref this.lifetimeData[i];
                    data.lifetime -= deltaTime;
                    if (data.lifetime <= 0f) {

                        if (this.UseLifetimeStep(data.entityId, world, step) == true) {

                            this.lifetimeData.RemoveAt(i);
                            --i;
                            --cnt;

                        }

                    }

                }

                return this.lifetimeData.Count == 0;

            }

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        protected abstract bool UseLifetimeStep(int id, World world, byte step);

        public override bool Validate(int capacity) {
            
            if (AllComponentTypes<TComponent>.isShared == true) this.sharedGroups.Validate(capacity);
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ArrayUtils.Resize(capacity, ref this.versionsNoState, true);
            
            this.world.currentState.storage.archetypes.Validate(capacity);
            
            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Validate(in Entity entity) {

            if (AllComponentTypes<TComponent>.isShared == true) this.sharedGroups.Validate(in entity);
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ArrayUtils.Resize(entity.id, ref this.versionsNoState, true);

            this.world.currentState.storage.archetypes.Validate(in entity);

            if (ComponentTypes<TComponent>.typeId >= 0 && this.Has(in entity) == true) {

                this.world.currentState.storage.archetypes.Set<TComponent>(in entity);

            }

            return false;

        }

        public override bool HasType(System.Type type) {

            return typeof(TComponent) == type;

        }

        public override System.Collections.Generic.ICollection<uint> GetSharedGroups(Entity entity) {

            return this.sharedGroups.GetGroups();

        }

        public override IStructComponentBase GetSharedObject(Entity entity, uint groupId) {

            if (this.sharedGroups.Has(entity.id, groupId) == false) return null;
            return this.sharedGroups.Get(entity.id, groupId);

        }

        public override bool SetSharedObject(in Entity entity, IStructComponentBase data, uint groupId) {
            
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

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void Replace(ref Component<TComponent> bucket, in TComponent data);
        
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

        private struct ElementCopy : IArrayElementCopy<SharedGroupData> {

            public void Copy(SharedGroupData @from, ref SharedGroupData to) {
                
                to.data = from.data;
                ArrayUtils.Copy(from.states, ref to.states);
                
            }

            public void Recycle(SharedGroupData item) {
                
                PoolArray<bool>.Recycle(ref item.states);
                
            }

        }

        public override void CopyFrom(in Entity from, in Entity to) {

            if (typeof(TComponent) == typeof(ME.ECS.Views.ViewComponent)) {

                var view = from.Read<ME.ECS.Views.ViewComponent>();
                if (view.viewInfo.entity == from) {

                    to.InstantiateView(view.viewInfo.prefabSourceId);

                }

                return;

            }

            if (this.lifetimeData != null) {

                for (int i = 0; i < this.lifetimeData.Count; ++i) {

                    var data = this.lifetimeData[i];
                    if (data.entityId == from.id) {

                        data.entityId = to.id;
                        this.lifetimeData.Add(data);
                        break;

                    }

                }

            }

            if (AllComponentTypes<TComponent>.isShared == true) this.sharedGroups.CopyFrom(in from, in to);
            if (this.CopyFromState(in from, in to) > 0) {

                if (ComponentTypes<TComponent>.typeId >= 0) this.world.currentState.storage.archetypes.Set<TComponent>(in to);

            } else {

                if (ComponentTypes<TComponent>.typeId >= 0) this.world.currentState.storage.archetypes.Remove<TComponent>(in to);

            }

        }

        protected abstract byte CopyFromState(in Entity from, in Entity to);

        public override void CopyFrom(StructRegistryBase other) {

            var _other = (StructComponents<TComponent>)other;
            ArrayUtils.Copy(_other.lifetimeData, ref this.lifetimeData);
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) _other.versionsNoState = this.versionsNoState;
            if (AllComponentTypes<TComponent>.isShared == true) this.sharedGroups.CopyFrom(_other.sharedGroups, new ElementCopy());
            
        }

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
            this.listLifetimeTick = PoolHashSetCopyable<int>.Spawn();
            this.listLifetimeFrame = PoolHashSetCopyable<int>.Spawn();

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
        public void RemoveAll(in Entity entity) {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

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
        public void ValidateOneShot<TComponent>(bool isTag = false) where TComponent : struct, IStructComponentBase, IComponentOneShot {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            if (isTag == true) WorldUtilities.SetComponentAsTag<TComponent>();
            this.ValidateOneShot<TComponent>(code, isTag);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateOneShot<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IStructComponentBase, IComponentOneShot {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            this.ValidateOneShot<TComponent>(code, isTag);
            var reg = (StructComponentsOneShot<TComponent>)this.list.arr[code];
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
        private void ValidateOneShot<TComponent>(int code, bool isTag) where TComponent : struct, IStructComponentBase, IComponentOneShot {

            if (ArrayUtils.WillResize(code, ref this.list) == true) {

                ArrayUtils.Resize(code, ref this.list, true);

            }

            if (this.list.arr[code] == null) {

                var instance = PoolRegistries.SpawnOneShot<TComponent>();
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

            if (this.nextFrameTasks != null) {

                for (int i = 0; i < this.nextFrameTasks.array.Length; ++i) {

                    if (this.nextFrameTasks.array[i] == null) continue;

                    for (int j = 0; j < this.nextFrameTasks.array[i].Length; ++j) {

                        if (this.nextFrameTasks.array[i][j] == null) continue;

                        this.nextFrameTasks.array[i][j].Recycle();

                    }

                }

                PoolCCList<ITask>.Recycle(ref this.nextFrameTasks);

            }

            if (this.nextTickTasks != null) {

                for (int i = 0; i < this.nextTickTasks.array.Length; ++i) {

                    if (this.nextTickTasks.array[i] == null) continue;

                    for (int j = 0; j < this.nextTickTasks.array[i].Length; ++j) {

                        if (this.nextTickTasks.array[i][j] == null) continue;

                        this.nextTickTasks.array[i][j].Recycle();

                    }

                }

                PoolCCList<ITask>.Recycle(ref this.nextTickTasks);

            }

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
            if (this.listLifetimeTick != null) PoolHashSetCopyable<int>.Recycle(ref this.listLifetimeTick);
            if (this.listLifetimeFrame != null) PoolHashSetCopyable<int>.Recycle(ref this.listLifetimeFrame);

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

            this.isCreated = other.isCreated;

            {

                if (this.nextFrameTasks != null) {

                    for (int i = 0; i < this.nextFrameTasks.array.Length; ++i) {

                        if (this.nextFrameTasks.array[i] == null) continue;

                        for (int j = 0; j < this.nextFrameTasks.array[i].Length; ++j) {

                            if (this.nextFrameTasks.array[i][j] == null) continue;

                            this.nextFrameTasks.array[i][j].Recycle();

                        }

                    }

                    PoolCCList<ITask>.Recycle(ref this.nextFrameTasks);

                }

                if (this.nextTickTasks != null) {

                    for (int i = 0; i < this.nextTickTasks.array.Length; ++i) {

                        if (this.nextTickTasks.array[i] == null) continue;

                        for (int j = 0; j < this.nextTickTasks.array[i].Length; ++j) {

                            if (this.nextTickTasks.array[i][j] == null) continue;

                            this.nextTickTasks.array[i][j].Recycle();

                        }

                    }

                    PoolCCList<ITask>.Recycle(ref this.nextTickTasks);

                }

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

        public void ValidateDataOneShot<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IStructComponentBase, IComponentOneShot {

            this.structComponentsNoState.ValidateOneShot<TComponent>(in entity, isTag);

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

            if (step == ComponentLifetime.NotifyAllSystemsBelow) {
                
                this.currentState.timers.Update(deltaTime);
                
            }
        
            this.UseLifetimeStep(step, deltaTime, ref this.currentState.structComponents);
            this.UseLifetimeStep(step, deltaTime, ref this.structComponentsNoState);
            
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void UseLifetimeStep(ComponentLifetime step, float deltaTime, ref StructComponentsContainer structComponentsContainer) {

            var list = structComponentsContainer.listLifetimeTick;
            if (step == ComponentLifetime.NotifyAllModules || step == ComponentLifetime.NotifyAllModulesBelow) {

                list = structComponentsContainer.listLifetimeFrame;

            }
            
            if (list.Count > 0) {

                var bStep = (byte)step;
                var cnt = list.Count;
                var c = 0;
                foreach (var idx in list) {

                    ref var reg = ref structComponentsContainer.list.arr[idx];
                    if (reg == null) continue;

                    if (reg.UseLifetimeStep(this, bStep, deltaTime) == true) {
                        ++c;
                    }

                }

                if (c == cnt) list.Clear();

            }
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private static void AddToLifetimeIndex<TComponent>(in Entity entity, ComponentLifetime lifetime, float secondsLifetime, ref StructComponentsContainer structComponents) where TComponent : struct, IStructComponentBase {
            
            var list = structComponents.listLifetimeTick;
            if (lifetime == ComponentLifetime.NotifyAllModules || lifetime == ComponentLifetime.NotifyAllModulesBelow) {

                list = structComponents.listLifetimeFrame;

            }
            
            var idx = WorldUtilities.GetAllComponentTypeId<TComponent>();
            if (list.Contains(idx) == false) list.Add(idx);
            
            ref var r = ref structComponents.list.arr[idx];
            var reg = (StructComponents<TComponent>)r;
            if (reg.lifetimeData == null) reg.lifetimeData = PoolListCopyable<LifetimeData>.Spawn(10);
            reg.lifetimeData.Add(new LifetimeData() {
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
            var reg = (StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            return reg.GetVersion(in entity);
            
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
                if (ComponentTypes<TComponent>.typeId >= 0) {

                    this.currentState.storage.archetypes.Set<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

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

                ref var bucket = ref reg.components[entity.id];
                reg.UpdateVersion(ref bucket);
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

            var reg = (StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
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
                if (ComponentTypes<TComponent>.typeId >= 0) {

                    this.currentState.storage.archetypes.Remove<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

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
                if (ComponentTypes<TComponent>.typeId >= 0) {

                    this.currentState.storage.archetypes.Set<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

            }
            #if ENTITY_ACTIONS
            this.RaiseEntityActionOnAdd<TComponent>(in entity);
            #endif
            ref var bucket = ref reg.components[entity.id];
            reg.UpdateVersion(ref bucket);
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
                if (ComponentTypes<TComponent>.typeId >= 0) {

                    this.currentState.storage.archetypes.Set<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

            }
            #if ENTITY_ACTIONS
            this.RaiseEntityActionOnAdd<TComponent>(in entity);
            #endif
            this.currentState.storage.versions.Increment(in entity);
            ref var bucket = ref reg.components[entity.id];
            reg.UpdateVersion(ref bucket);
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

            }
            
        }
        #endregion

        #region TIMERS
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetTimer(in Entity entity, int index, float time) {

            this.currentState.timers.Set(in entity, index, time);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref float GetTimer(in Entity entity, int index) {

            return ref this.currentState.timers.Get(in entity, index);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public float ReadTimer(in Entity entity, int index) {

            return this.currentState.timers.Read(in entity, index);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool RemoveTimer(in Entity entity, int index) {

            return this.currentState.timers.Remove(in entity, index);

        }
        #endregion

        #region HAS/GET/SET/READ/REMOVE
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasData<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            return this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId].Has(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref readonly TComponent ReadData<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            // Inline all manually
            if (AllComponentTypes<TComponent>.isTag == true) return ref AllComponentTypes<TComponent>.empty;
            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            return ref reg.components[entity.id].data;

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

            return this.currentState.structComponents.list.arr[registryIndex].GetObject(entity);

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
            var incrementVersion = (this.HasResetState() == false || this.HasStep(WorldStep.LogicTick) == true);
            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            ref var storage = ref this.currentState.storage;
            ref var bucket = ref reg.components[entity.id];
            if (createIfNotExists == true && bucket.state == 0) {

                #if WORLD_EXCEPTIONS
                if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                    OutOfStateException.ThrowWorldStateCheck();

                }
                #endif

                incrementVersion = true;
                bucket.state = 1;
                if (ComponentTypes<TComponent>.typeId >= 0) {

                    storage.archetypes.Set<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

                #if ENTITY_ACTIONS
                this.RaiseEntityActionOnAdd<TComponent>(in entity);
                #endif

            }

            if (AllComponentTypes<TComponent>.isTag == true) return ref AllComponentTypes<TComponent>.empty;
            if (incrementVersion == true) {

                reg.UpdateVersion(ref bucket);
                storage.versions.Increment(in entity);
                if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
                if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);

            }

            return ref bucket.data;

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
            ref var storage = ref this.currentState.storage;
            ref var bucket = ref reg.components[entity.id];
            ref var state = ref bucket.state;
            reg.UpdateVersion(ref bucket);
            if (state == 0) {

                reg.Replace(ref bucket, default);
                state = 1;
                if (ComponentTypes<TComponent>.typeId >= 0) {

                    storage.archetypes.Set<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

            }
            #if ENTITY_ACTIONS
            this.RaiseEntityActionOnAdd<TComponent>(in entity);
            #endif
            storage.versions.Increment(in entity);
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
            ref var storage = ref this.currentState.storage;
            ref var bucket = ref reg.components[entity.id];
            ref var state = ref bucket.state;
            reg.Replace(ref bucket, in data);
            reg.UpdateVersion(ref bucket);
            if (state == 0) {

                state = 1;
                if (ComponentTypes<TComponent>.typeId >= 0) {

                    storage.archetypes.Set<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

            }
            #if ENTITY_ACTIONS
            this.RaiseEntityActionOnAdd<TComponent>(in entity);
            #endif
            storage.versions.Increment(in entity);
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

                switch (lifetime) {
                    case ComponentLifetime.NotifyAllModules:
                        task.lifetime = ComponentLifetime.NotifyAllModulesBelow;

                        this.currentState.structComponents.nextFrameTasks.Add(task);
                        break;

                    case ComponentLifetime.NotifyAllSystems: {
                        task.lifetime = ComponentLifetime.NotifyAllSystemsBelow;

                        if (this.currentState.structComponents.nextTickTasks.Contains(task) == false) {
                        
                            this.currentState.structComponents.nextTickTasks.Add(task);
                        
                        } else {
                        
                            task.Recycle();
                        
                        }

                        break;
                    }

                }

            } else {

                ref var state = ref this.SetData(in entity, in data);

                if (lifetime == ComponentLifetime.Infinite) return;
                state = (byte)(lifetime + 1);

                World.AddToLifetimeIndex<TComponent>(in entity, lifetime, secondsLifetime, ref this.currentState.structComponents);

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
                if (reg == null || reg.Remove(in entity, false) == false) continue;

                var bit = reg.GetTypeBit();
                if (bit >= 0) this.currentState.storage.archetypes.Remove(in entity, bit);
                changed = true;

            }

            if (changed == false) return;

            this.currentState.storage.versions.Increment(in entity);
            this.RemoveComponentFromFilter(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveData<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            #if WORLD_STATE_CHECK
            if (this.isActive == true && this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {
                
                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            ref var storage = ref this.currentState.storage;
            ref var bucket = ref reg.components[entity.id];
            if (bucket.state == 0) return;
            bucket.state = 0;
            
            storage.versions.Increment(in entity);
            if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);
            reg.RemoveData(ref bucket);
            
            if (ComponentTypes<TComponent>.typeId >= 0) {

                storage.archetypes.Remove<TComponent>(in entity);
                this.UpdateFilterByStructComponent<TComponent>(in entity);

            }

            #if ENTITY_ACTIONS
            this.RaiseEntityActionOnRemove<TComponent>(in entity);
            #endif

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
                if (componentIndex < 0) return;

                this.currentState.storage.archetypes.Set(in entity, componentIndex);
                this.UpdateFilterByStructComponent(in entity, componentIndex);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveData(in Entity entity, int dataIndex) {

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

            }

        }
        #endregion

    }

}
