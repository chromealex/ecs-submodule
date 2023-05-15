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
    using ME.ECS.Collections.LowLevel.Unsafe;
    using Collections.LowLevel;

    public struct Component<TComponent> where TComponent : struct, IComponentBase {

        public TComponent data;
        public byte state;
        public ushort version;

    }

    #if !SHARED_COMPONENTS_DISABLED
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
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
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
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

                if (this.lifetime == ComponentLifetime.NotifyAllSystemsBelow) return;

                if (this.entity.IsAlive() == true) {
                    
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
            
            public void Dispose(ref MemoryAllocator allocator) {

                this.dataIndex = default;
                this.storageType = default;
                this.entity = default;
                this.lifetime = default;
                this.secondsLifetime = default;
                this.destroyEntity = default;
                this.data.Dispose(ref allocator);

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
        internal ME.ECS.Collections.LowLevel.HashSet<NextTickTask> nextTickTasks;

        [ME.ECS.Serializer.SerializeField]
        internal BufferArray<StructRegistryBase> list;
        [ME.ECS.Serializer.SerializeField]
        public EntitiesIndexer entitiesIndexer;
        [ME.ECS.Serializer.SerializeField]
        private bool isCreated;

        [ME.ECS.Serializer.SerializeField]
        private ME.ECS.Collections.LowLevel.List<int> dirtyMap;

        public UnmanagedComponentsStorage unmanagedComponentsStorage;

        public bool IsCreated() {

            return this.isCreated;

        }

        public void Initialize(ref MemoryAllocator allocator, bool freeze) {

            this.nextTickTasks = new ME.ECS.Collections.LowLevel.HashSet<NextTickTask>(ref allocator, 10);
            this.dirtyMap = new ME.ECS.Collections.LowLevel.List<int>(ref allocator, 10);
            
            this.entitiesIndexer = new EntitiesIndexer();
            this.entitiesIndexer.Initialize(ref allocator, 100);

            ArrayUtils.Resize(100, ref this.list, false);
            
            this.unmanagedComponentsStorage.Initialize();
            this.isCreated = true;
            
        }

        public void Merge(in MemoryAllocator allocator) {

            if (this.dirtyMap.isCreated == false) return;

            for (int i = 0, count = this.dirtyMap.Count; i < count; ++i) {

                var idx = this.dirtyMap[in allocator, i];
                if (idx < 0 || idx >= this.list.Count) {
                    UnityEngine.Debug.LogError($"DirtyMap: Idx {idx} is out of range [0..{this.list.Count})");
                    continue;
                }
                this.list.arr[idx].Merge();

            }
            
            this.dirtyMap.Clear(in allocator);

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
        public void SetEntityCapacity(ref MemoryAllocator allocator, int capacity) {

            this.entitiesIndexer.Validate(ref allocator, capacity);
            
            // Update all known structs
            for (int i = 0, length = this.list.Length; i < length; ++i) {

                var item = this.list.arr[i];
                if (item != null) {

                    if (item.Validate(capacity) == true) {

                        this.dirtyMap.Add(ref allocator, i);

                    }

                }

            }

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void OnEntityCreate(ref MemoryAllocator allocator, in Entity entity) {

            this.entitiesIndexer.Validate(ref allocator, entity.id);
            
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
        public void RemoveAll(State state, ref MemoryAllocator allocator, in Entity entity) {

            E.IS_ALIVE(in entity);

            ref var list = ref this.entitiesIndexer.Get(in allocator, entity.id);
            if (list.isCreated == true) {

                if (state == null) {

                    var e = list.GetEnumerator(in allocator);
                    while (e.MoveNext() == true) {

                        var index = e.Current;
                        var item = this.list.arr[index];
                        if (item != null) {

                            item.Remove(in entity, clearAll: true);

                        }

                    }
                    e.Dispose();

                } else {

                    var e = list.GetEnumerator(state);
                    while (e.MoveNext() == true) {

                        var index = e.Current;
                        var item = this.list.arr[index];
                        if (item != null) {

                            item.Remove(in entity, clearAll: true);

                        }

                    }
                    e.Dispose();

                }

                list.Clear(in allocator);

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
        
        #region Unmanaged
        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateUnmanaged<TComponent>(ref MemoryAllocator allocator, bool isTag = false) where TComponent : struct, IComponentBase {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            if (isTag == true) WorldUtilities.SetComponentAsTag<TComponent>();
            this.ValidateUnmanaged<TComponent>(ref allocator, code, isTag);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateUnmanaged<TComponent>(ref MemoryAllocator allocator, in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            this.ValidateUnmanaged<TComponent>(ref allocator, code, isTag);
            var reg = (StructComponentsUnmanaged<TComponent>)this.list.arr[code];
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
        private void ValidateUnmanaged<TComponent>(ref MemoryAllocator allocator, int code, bool isTag) where TComponent : struct, IComponentBase {

            this.unmanagedComponentsStorage.ValidateTypeId<TComponent>(ref allocator);

            if (ArrayUtils.WillResize(code, ref this.list) == true) {

                ArrayUtils.Resize(code, ref this.list, true);

            }

            if (this.list.arr[code] == null) {

                var instance = PoolRegistries.SpawnUnmanaged<TComponent>();
                this.list.arr[code] = instance;

            }

        }
        #endregion
        
        #region UnmanagedDisposable
        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateUnmanagedDisposable<TComponent>(ref MemoryAllocator allocator, bool isTag = false) where TComponent : struct, IComponentDisposable<TComponent> {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            if (isTag == true) WorldUtilities.SetComponentAsTag<TComponent>();
            this.ValidateUnmanagedDisposable<TComponent>(ref allocator, code, isTag);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateUnmanagedDisposable<TComponent>(ref MemoryAllocator allocator, in Entity entity, bool isTag = false) where TComponent : struct, IComponentDisposable<TComponent> {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            this.ValidateUnmanagedDisposable<TComponent>(ref allocator, code, isTag);
            var reg = (StructComponentsUnmanagedDisposable<TComponent>)this.list.arr[code];
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
        private void ValidateUnmanagedDisposable<TComponent>(ref MemoryAllocator allocator, int code, bool isTag) where TComponent : struct, IComponentDisposable<TComponent> {

            this.unmanagedComponentsStorage.ValidateTypeIdDisposable<TComponent>(ref allocator, code);

            if (ArrayUtils.WillResize(code, ref this.list) == true) {

                ArrayUtils.Resize(code, ref this.list, true);

            }

            if (this.list.arr[code] == null) {

                var instance = PoolRegistries.SpawnUnmanagedDisposable<TComponent>();
                this.list.arr[code] = instance;

            }

        }
        #endregion
        
        #region Tags
        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateTag<TComponent>(bool isTag = true) where TComponent : struct, IComponentBase {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            WorldUtilities.SetComponentAsTag<TComponent>();
            this.ValidateTag<TComponent>(code, isTag);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public void ValidateTag<TComponent>(in Entity entity, bool isTag = true) where TComponent : struct, IComponentBase {

            var code = WorldUtilities.GetAllComponentTypeId<TComponent>();
            this.ValidateTag<TComponent>(code);
            var reg = (StructComponentsTag<TComponent>)this.list.arr[code];
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
        private void ValidateTag<TComponent>(int code, bool isTag = true) where TComponent : struct, IComponentBase {

            if (ArrayUtils.WillResize(code, ref this.list) == true) {

                ArrayUtils.Resize(code, ref this.list, true);

            }

            if (this.list.arr[code] == null) {

                var instance = PoolRegistries.SpawnTag<TComponent>();
                this.list.arr[code] = instance;

            }

        }
        #endregion

        #if COMPONENTS_COPYABLE
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
        #endif

        #if COMPONENTS_COPYABLE
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
        #endif

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasBit(in MemoryAllocator allocator, in Entity entity, int bit) {

            return this.entitiesIndexer.Has(in allocator, entity.id, bit);
            
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
        public void OnRecycle(ref MemoryAllocator allocator, bool destroyAllocator) {

            if (destroyAllocator == false) {
                
                this.entitiesIndexer.Dispose(ref allocator);
                
                if (this.nextTickTasks.isCreated == true) {

                    var e = this.nextTickTasks.GetEnumerator(in allocator);
                    while (e.MoveNext() == true) {

                        var task = e.Current;
                        task.Dispose(ref allocator);

                    }
                    e.Dispose();
                    this.nextTickTasks.Dispose(ref allocator);

                }

                if (this.dirtyMap.isCreated == true) this.dirtyMap.Dispose(ref allocator);

                this.unmanagedComponentsStorage = default;

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

            this.isCreated = other.isCreated;
            this.unmanagedComponentsStorage = other.unmanagedComponentsStorage;
            this.entitiesIndexer = other.entitiesIndexer;
            this.dirtyMap = other.dirtyMap;
            this.nextTickTasks = other.nextTickTasks;
            
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

        public struct NoState {

            public StructComponentsContainer storage;
            public MemoryAllocator allocator;
            public PluginsStorage pluginsStorage;

            public void Initialize() {

                this.allocator.Initialize(1024 * 1024, -1);
                this.storage.Initialize(ref this.allocator, true);
                this.pluginsStorage.Initialize(ref this.allocator);

            }

            public void Dispose() {
                
                this.pluginsStorage = default;
                this.storage.OnRecycle(ref this.allocator, true);
                this.allocator.Dispose();
                
            }

        }

        public NoState noStateData;
        
        public ref StructComponentsContainer GetStructComponents() {

            return ref this.currentState.structComponents;

        }

        public ref NoState GetNoStateData() {

            return ref this.noStateData;

        }

        partial void OnSpawnStructComponents() { }

        partial void OnRecycleStructComponents() { }

        public void IncrementEntityVersion(in Entity entity) {

            this.currentState.storage.versions.Increment(in this.currentState.allocator, in entity);
            
        }

        partial void SetEntityCapacityPlugin1(int capacity) {

            this.noStateData.storage.SetEntityCapacity(ref this.noStateData.allocator, capacity);
            this.currentState.structComponents.SetEntityCapacity(ref this.currentState.allocator, capacity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        partial void CreateEntityPlugin1(Entity entity, bool isNew) {

            if (isNew == true) {
                
                this.noStateData.storage.SetEntityCapacity(ref this.noStateData.allocator, entity.id);
                this.currentState.structComponents.OnEntityCreate(ref this.currentState.allocator, in entity);
                
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        partial void DestroyEntityPlugin1(Entity entity) {

            this.noStateData.storage.RemoveAll(null, ref this.noStateData.allocator, in entity);
            this.currentState.structComponents.RemoveAll(this.currentState, ref this.currentState.allocator, in entity);

        }

        public void Register(ref MemoryAllocator allocator, ref StructComponentsContainer componentsContainer, bool freeze, bool restore) {

            if (componentsContainer.IsCreated() == false) {

                //componentsContainer = new StructComponentsContainer();
                componentsContainer.Initialize(ref allocator, freeze);

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

            return this.currentState.structComponents.HasBit(in this.currentState.allocator, in entity, bit);

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

        public void ValidateDataTag<TComponent>(in Entity entity) where TComponent : struct, IComponentBase {

            this.currentState.structComponents.ValidateTag<TComponent>(in entity);

        }

        public void ValidateDataOneShot<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase, IComponentOneShot {

            this.noStateData.storage.ValidateOneShot<TComponent>(in entity, isTag);

        }

        public void ValidateDataUnmanaged<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase {

            this.currentState.structComponents.ValidateUnmanaged<TComponent>(ref this.currentState.allocator, in entity, isTag);

        }

        public void ValidateDataUnmanagedDisposable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentDisposable<TComponent> {

            this.currentState.structComponents.ValidateUnmanagedDisposable<TComponent>(ref this.currentState.allocator, in entity, isTag);

        }

        public void ValidateDataBlittable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase {

            this.currentState.structComponents.ValidateBlittable<TComponent>(in entity, isTag);

        }

        #if COMPONENTS_COPYABLE
        public void ValidateDataCopyable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase, IStructCopyable<TComponent> {

            this.currentState.structComponents.ValidateCopyable<TComponent>(in entity, isTag);

        }

        public void ValidateDataBlittableCopyable<TComponent>(in Entity entity, bool isTag = false) where TComponent : struct, IComponentBase, IStructCopyable<TComponent> {

            this.currentState.structComponents.ValidateBlittableCopyable<TComponent>(in entity, isTag);

        }
        #endif

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void UseLifetimeStep(ComponentLifetime step, tfloat deltaTime) {

            WorldStaticCallbacks.RaiseCallbackLifetimeStep(this, step, deltaTime);

            this.UseLifetimeStep(ref this.currentState.allocator, step, deltaTime, ref this.currentState.structComponents);
            this.UseLifetimeStep(ref this.noStateData.allocator, step, deltaTime, ref this.noStateData.storage);
            
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
        private unsafe void UseLifetimeStep(ref MemoryAllocator allocator, ComponentLifetime step, tfloat deltaTime, ref StructComponentsContainer structComponentsContainer) {

            ref var list = ref structComponentsContainer.nextTickTasks;
            if (list.Count > 0) {

                // We need to allocate temporary list to store entities
                // because on entity.Destroy we clean up all data including tasks list
                var tempList = stackalloc Entity[list.Count];
                var k = 0;
                var cnt = 0;
                var e = list.GetEnumerator(in allocator);
                while (e.MoveNext() == true) {

                    var idx = e.Index;
                    ref var task = ref list.GetByIndex(in allocator, idx);
                    var taskStep = task.GetStep();
                    if (taskStep != step) continue;
                    
                    if (step == ComponentLifetime.NotifyAllSystemsBelow) {

                        if (task.Update(deltaTime) == true) {
                            
                            // Remove task on complete
                            if (task.destroyEntity == true) tempList[k++] = task.entity;
                            task.Dispose(ref allocator);
                            ++cnt;

                        }

                    } else if (step == ComponentLifetime.NotifyAllSystems) {
                        
                        task.NextStep();
                        
                    }
                    
                }
                e.Dispose();

                for (int i = 0; i < k; ++i) {
                    tempList[i].Destroy();
                }
                
                if (cnt == list.Count) {
                    
                    // All is null
                    list.Clear(in allocator);
                    
                }

            }
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public long GetDataVersion<TComponent>(in Entity entity) where TComponent : struct, IVersioned {
            
            E.IS_ALIVE(in entity);
            E.IS_TAG<TComponent>(in entity);

            if (AllComponentTypes<TComponent>.isVersioned == false) return 0L;
            var reg = (StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            return reg.GetVersion(entity.id);
            
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

            if (groupId == 0 && entity.Has<SharedData>() == true) {

                ref readonly var sharedData = ref entity.Read<SharedData>();
                ref var allocator = ref Worlds.current.currentState.allocator;
                if (sharedData.archetypeToId.TryGetValue(in allocator, AllComponentTypes<TComponent>.typeId, out var innerGroupId) == true && innerGroupId != 0) {

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

            if (groupId == 0 && entity.Has<SharedData>() == true) {

                ref readonly var sharedData = ref entity.Read<SharedData>();
                ref var allocator = ref Worlds.current.currentState.allocator;
                if (sharedData.archetypeToId.TryGetValue(in allocator, AllComponentTypes<TComponent>.typeId, out var innerGroupId) == true && innerGroupId != 0) {

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

            if (groupId == 0 && entity.Has<SharedData>() == true) {

                ref readonly var sharedData = ref entity.Read<SharedData>();
                ref var allocator = ref Worlds.current.currentState.allocator;
                if (sharedData.archetypeToId.TryGetValue(in allocator, AllComponentTypes<TComponent>.typeId, out var innerGroupId) == true && innerGroupId != 0) {

                    return ref this.GetSharedData<TComponent>(in entity, innerGroupId);

                }

            }

            // Inline all manually
            var reg = (StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            ref var state = ref SharedGroupsAPI<TComponent>.Has(ref reg.sharedStorage, entity.id, groupId);
            var incrementVersion = (this.HasStep(WorldStep.LogicTick) == true || this.HasResetState() == false);
            if (state == false) {

                E.IS_LOGIC_STEP(this);

                if (this.currentState.structComponents.entitiesIndexer.GetCount(in this.currentState.allocator, entity.id) == 0 &&
                    (this.currentState.storage.flags.Get(in this.currentState.allocator, entity.id) & (byte)EntityFlag.DestroyWithoutComponents) != 0) {
                    entity.RemoveOneShot<IsEntityEmptyOneShot>();
                }
                
                incrementVersion = true;
                SharedGroupsAPI<TComponent>.Set(ref reg.sharedStorage, entity.id, groupId);
                this.currentState.structComponents.entitiesIndexer.Set(ref this.currentState.allocator, entity.id, AllComponentTypes<TComponent>.typeId);
                if (ComponentTypes<TComponent>.typeId >= 0) {

                    this.AddFilterByStructComponent<TComponent>(ref this.currentState.allocator, in entity);

                }

            }

            if (ComponentTypes<TComponent>.isFilterLambda == true && ComponentTypes<TComponent>.typeId >= 0) {

                this.ValidateFilterByStructComponent<TComponent>(ref this.currentState.allocator, in entity, true);
                
            }
            
            if (incrementVersion == true) {
                
                // Increment versions for all entities stored this group
                ref var states = ref SharedGroupsAPI<TComponent>.GetGroup(ref reg.sharedStorage, groupId).states;
                for (int i = 0; i < states.Length; ++i) {
                    if (states.arr[i] == true) {
                        this.currentState.storage.versions.Increment(in this.currentState.allocator, i);
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
            
            if (groupId == 0 && entity.Has<SharedData>() == true) {

                ref readonly var sharedData = ref entity.Read<SharedData>();
                ref var allocator = ref Worlds.current.currentState.allocator;
                if (sharedData.archetypeToId.TryGetValue(in allocator, AllComponentTypes<TComponent>.typeId, out var innerGroupId) == true && innerGroupId != 0) {

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
                        this.currentState.storage.versions.Increment(in this.currentState.allocator, i);
                    }
                }

                state = false;
                this.currentState.structComponents.entitiesIndexer.Remove(ref this.currentState.allocator, entity.id, AllComponentTypes<TComponent>.typeId);
                
                if (ComponentTypes<TComponent>.typeId >= 0) {

                    this.RemoveFilterByStructComponent<TComponent>(ref this.currentState.allocator, in entity);

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
                        this.currentState.storage.versions.Increment(in this.currentState.allocator, i);
                    }
                }

            }
            
            ref var state = ref SharedGroupsAPI<TComponent>.Has(ref reg.sharedStorage, entity.id, groupId);
            if (state == false) {

                state = true;
                this.currentState.structComponents.entitiesIndexer.Set(ref this.currentState.allocator, entity.id, AllComponentTypes<TComponent>.typeId);
                if (ComponentTypes<TComponent>.typeId >= 0) {

                    this.AddFilterByStructComponent<TComponent>(ref this.currentState.allocator, in entity);

                }

            }
            
            if (ComponentTypes<TComponent>.isFilterLambda == true && ComponentTypes<TComponent>.typeId >= 0) {

                this.ValidateFilterByStructComponent<TComponent>(ref this.currentState.allocator, in entity);
                
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

                this.currentState.storage.versions.Increment(in this.currentState.allocator, in entity);
                reg.UpdateVersion(in entity);

            }
            
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

            return ((StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId]).TryRead(in entity, out component);
            
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasData<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            E.IS_ALIVE(in entity);

            if (AllComponentTypes<TComponent>.isBlittable == true &&
                AllComponentTypes<TComponent>.isDisposable == false) {
                ref var allocator = ref this.currentState.allocator;
                ref var item = ref this.currentState.structComponents.unmanagedComponentsStorage.GetRegistry<TComponent>(in this.currentState.allocator);
                return item.components.Read(in allocator, entity.id).state != 0;
            }
            
            return this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId].Has(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public MemPtr ReadDataPtr<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            E.IS_ALIVE(in entity);
            E.IS_TAG<TComponent>(in entity);

            return ((StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId]).ReadPtr(in entity);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref readonly TComponent ReadData<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            E.IS_ALIVE(in entity);
            E.IS_TAG<TComponent>(in entity);

            if (AllComponentTypes<TComponent>.isBlittable == true &&
                AllComponentTypes<TComponent>.isDisposable == false) {
                ref var allocator = ref this.currentState.allocator;
                ref var item = ref this.currentState.structComponents.unmanagedComponentsStorage.GetRegistry<TComponent>(in this.currentState.allocator);
                return ref item.components.Read(in allocator, entity.id).data;
            }

            return ref ((StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId]).Read(in entity);

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
            
            if (AllComponentTypes<TComponent>.isBlittable == true &&
                AllComponentTypes<TComponent>.isDisposable == false) {
                ref var allocator = ref this.currentState.allocator;
                ref var item = ref this.currentState.structComponents.unmanagedComponentsStorage.GetRegistry<TComponent>(in this.currentState.allocator);
                ref var bucket = ref item.components.Get(ref allocator, entity.id);
                DataBufferUtilsBase.PushSetCreate_INTERNAL(ref bucket, this, in entity, StorageType.Default, makeRequest: true);
                return ref bucket.data;
            } else {

                var reg = (StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                ref var bucket = ref reg.Get(in entity);
                DataBufferUtilsBase.PushSetCreate_INTERNAL(ref bucket, this, in entity, StorageType.Default, makeRequest: true);
                return ref bucket.data;
            }

            /*
            if (AllComponentTypes<TComponent>.isBlittable == true) {
                
                // Inline all manually
                var reg = (StructComponentsBlittable<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                return ref DataBlittableBufferUtils.PushGet_INTERNAL(this, in entity, reg, StorageType.Default);

            } else {

                // Inline all manually
                var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                return ref DataBufferUtils.PushGet_INTERNAL(this, in entity, reg, StorageType.Default);

            }*/

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
            
            var reg = (StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
            if (AllComponentTypes<TComponent>.isTag == true) {
                
                ref var state = ref reg.GetState(in entity);
                DataBufferUtilsBase.PushSetCreate_INTERNAL<TComponent>(ref state, this, in entity, StorageType.Default, makeRequest: false);
                
            } else {

                ref var bucket = ref reg.Get(in entity);
                reg.Replace(ref bucket, in data);
                DataBufferUtilsBase.PushSetCreate_INTERNAL(ref bucket, this, in entity, StorageType.Default, makeRequest: false);
                
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

            this.SetData(ref this.currentState.allocator, ref this.currentState.structComponents, in entity, in data, lifetime, secondsLifetime);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal void SetData<TComponent>(ref MemoryAllocator allocator, ref StructComponentsContainer container, in Entity entity, in TComponent data, ComponentLifetime lifetime, tfloat secondsLifetime, bool addTaskOnly = false) where TComponent : unmanaged, IStructComponent {
            
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

            if (container.nextTickTasks.Add(ref allocator, task) == false) {

                task.Dispose(ref allocator);

            } else {

                ref var val = ref container.nextTickTasks.GetValue(in allocator, task);
                val.data = new UnsafeData().Set(ref allocator, data);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveData<TComponent>(in Entity entity, tfloat secondsLifetime) where TComponent : unmanaged, IStructComponent {

            if (this.HasData<TComponent>(in entity) == false) return;
            this.SetData<TComponent>(ref this.currentState.allocator, ref this.currentState.structComponents, in entity, default, ComponentLifetime.NotifyAllSystemsBelow, secondsLifetime, addTaskOnly: true);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveData<TComponent>(in Entity entity) where TComponent : struct, IStructComponent {

            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);

            ((StructComponentsBase<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId]).RemoveObject(in entity, StorageType.Default);

            /*
            if (AllComponentTypes<TComponent>.isBlittable == true) {
             
                if (AllComponentTypes<TComponent>.isCopyable == true) {
                    
                    
                    
                } else {
                    
                    var reg = (StructComponentsUnmanaged<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(this, in entity, reg, StorageType.Default);

                }

            } else {

                if (AllComponentTypes<TComponent>.isTag == true) {
                 
                    var reg = (StructComponentsTag<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                    DataTagBufferUtils.PushRemove_INTERNAL(this, in entity, reg, StorageType.Default);
   
                } else {

                    var reg = (StructComponents<TComponent>)this.currentState.structComponents.list.arr[AllComponentTypes<TComponent>.typeId];
                    DataBufferUtils.PushRemove_INTERNAL(this, in entity, reg, StorageType.Default);

                }

            }*/

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

                    this.currentState.storage.versions.Increment(in this.currentState.allocator, in entity);

                }

            } else if (storageType == StorageType.NoState) {
                
                var reg = this.noStateData.storage.list.arr[dataIndex];
                if (reg.RemoveObject(entity, storageType) == true) {

                    this.currentState.storage.versions.Increment(in this.currentState.allocator, in entity);

                }

            }

        }
        #endregion

    }

}
