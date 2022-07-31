#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using System.Runtime.CompilerServices;
using ME.ECS.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.IL2CPP.CompilerServices;
using Il2Cpp = Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute;

namespace ME.ECS.FiltersArchetype {
    
    using Collections.V3;
    using Collections.MemoryAllocator;
    
    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
    public struct FiltersArchetypeStorage : IStorage {

        [Il2Cpp(Option.NullChecks, false)]
        [Il2Cpp(Option.ArrayBoundsChecks, false)]
        [Il2Cpp(Option.DivideByZeroChecks, false)]
        public struct Archetype {

            public struct Info {

                public int index; // Index in list

            }

            public int index;
            public Dictionary<int, Info> components; // Contains componentId => Info index
            public List<int> componentIds; // Contains raw list of component ids
            public List<int> entitiesArr; // Contains raw unsorted list of entities
            public HashSet<int> entitiesContains;
            public Dictionary<int, int> edgesToAdd; // Contains edges to move from this archetype to another
            public Dictionary<int, int> edgesToRemove; // Contains edges to move from this archetype to another
            
            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            internal readonly bool HasAnyPair(in MemoryAllocator allocator, ListCopyable<FilterInternalData.Pair2> list) {

                for (int i = 0, cnt = list.Count; i < cnt; ++i) {
                    var pair = list[i];
                    if (this.Has(in allocator, pair.t1) == false &&
                        this.Has(in allocator, pair.t2) == false) {
                        return false;
                    }
                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            internal readonly bool HasAnyPair(in MemoryAllocator allocator, ListCopyable<FilterInternalData.Pair3> list) {

                for (int i = 0, cnt = list.Count; i < cnt; ++i) {
                    var pair = list[i];
                    if (this.Has(in allocator, pair.t1) == false &&
                        this.Has(in allocator, pair.t2) == false &&
                        this.Has(in allocator, pair.t3) == false) {
                        return false;
                    }
                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            internal readonly bool HasAnyPair(in MemoryAllocator allocator, ListCopyable<FilterInternalData.Pair4> list) {

                for (int i = 0, cnt = list.Count; i < cnt; ++i) {
                    var pair = list[i];
                    if (this.Has(in allocator, pair.t1) == false &&
                        this.Has(in allocator, pair.t2) == false &&
                        this.Has(in allocator, pair.t3) == false &&
                        this.Has(in allocator, pair.t4) == false) {
                        return false;
                    }
                }

                return true;

            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public readonly bool Has(in MemoryAllocator allocator, int componentId) {

                return this.components.ContainsKey(in allocator, componentId);

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public readonly bool HasAll(in MemoryAllocator allocator, ListCopyable<int> componentIds) {

                for (int i = 0, cnt = componentIds.Count; i < cnt; ++i) {
                    var item = componentIds[i];
                    if (this.components.ContainsKey(in allocator, item) == false) {
                        return false;
                    }
                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public readonly bool HasAll(in MemoryAllocator allocator, List<int> componentIds) {

                for (int i = 0, cnt = componentIds.Count(in allocator); i < cnt; ++i) {
                    var item = componentIds[in allocator, i];
                    if (this.components.ContainsKey(in allocator, item) == false) {
                        return false;
                    }
                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public readonly bool HasNotAll(in MemoryAllocator allocator, ListCopyable<int> componentIds) {

                for (int i = 0, cnt = componentIds.Count; i < cnt; ++i) {
                    
                    var item = componentIds[i];
                    if (this.components.ContainsKey(in allocator, item) == true) {
                        return false;
                    }

                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public readonly bool HasAllExcept(in MemoryAllocator allocator, List<int> componentIds, int componentId) {

                for (int i = 0, cnt = componentIds.Count(in allocator); i < cnt; ++i) {
                    
                    var item = componentIds[in allocator, i];
                    if (item == componentId) {
                        continue;
                    }

                    if (this.components.ContainsKey(in allocator, item) == false) {
                        return false;
                    }

                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public Archetype Set(ref MemoryAllocator allocator, ref FiltersArchetypeStorage storage, Entity entity, int componentId) {

                if (this.Has(in allocator, componentId) == true) {
                    return this;
                }

                // Remove entity from current archetype
                storage.RemoveEntityFromArch(ref allocator, ref this, entity.id);

                // Find the edge to move
                ref var edge = ref this.edgesToAdd.GetValue(ref allocator, componentId, out var exist);
                if (exist == false) {
                    edge = Archetype.CreateAdd(ref allocator, ref storage, this.index, this.componentIds, this.components, componentId);
                }
                
                {
                    ref var arch = ref storage.allArchetypes[in allocator, edge];
                    storage.AddEntityToArch(ref allocator, ref arch, entity.id);
                }

                // Return the new archetype we are moved to
                return storage.allArchetypes[in allocator, edge];

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public Archetype Remove(ref MemoryAllocator allocator, ref FiltersArchetypeStorage storage, Entity entity, int componentId) {

                if (this.Has(in allocator, componentId) == false) {
                    return this;
                }

                // Remove entity from current archetype
                storage.RemoveEntityFromArch(ref allocator, ref this, entity.id);

                // Find the edge to move
                ref var edge = ref this.edgesToRemove.GetValue(ref allocator, componentId, out var exist);
                if (exist == false) {
                    edge = Archetype.CreateRemove(ref allocator, ref storage, this.index, this.componentIds, this.components, componentId);
                }

                {
                    ref var arch = ref storage.allArchetypes[in allocator, edge];
                    storage.AddEntityToArch(ref allocator, ref arch, entity.id);
                }

                // Return the new archetype we are moved to
                return storage.allArchetypes[in allocator, edge];

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            private static int CreateAdd(ref MemoryAllocator allocator, ref FiltersArchetypeStorage storage, int node, in List<int> componentIds, in Dictionary<int, Info> components, int componentId) {

                if (storage.TryGetArchetypeAdd(ref allocator, componentIds, componentId, out var ar) == true) {
                    return ar;
                }

                var arch = new Archetype() {
                    edgesToAdd = new Dictionary<int, int>(ref allocator, 16),
                    edgesToRemove = new Dictionary<int, int>(ref allocator, 16),
                    entitiesArr = new List<int>(ref allocator, 16),
                    entitiesContains = new HashSet<int>(ref allocator, 16),
                    componentIds = new List<int>(ref allocator, componentIds.Count(in allocator) + 1),
                    components = new Dictionary<int, Info>(ref allocator, components.Count(in allocator) + 1),
                };
                arch.components.CopyFrom(ref allocator, components);
                arch.componentIds.AddRange(ref allocator, componentIds);
                arch.components.Add(ref allocator, componentId, new Info() {
                    index = arch.componentIds.Count(in allocator),
                });
                arch.componentIds.Add(ref allocator, componentId);
                if (node >= 0) {
                    arch.edgesToRemove.Add(ref allocator, componentId, node);
                }

                storage.isArchetypesDirty = true;
                var idx = storage.allArchetypes.Count(in allocator);
                arch.index = idx;

                storage.dirtyArchetypes.Add(ref allocator, idx);
                storage.allArchetypes.Add(ref allocator, arch);
                
                return idx;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            private static int CreateRemove(ref MemoryAllocator allocator, ref FiltersArchetypeStorage storage, int node, in List<int> componentIds, in Dictionary<int, Info> components, int componentId) {

                if (storage.TryGetArchetypeRemove(ref allocator, componentIds, componentId, out var ar) == true) {
                    return ar;
                }

                var arch = new Archetype() {
                    edgesToAdd = new Dictionary<int, int>(ref allocator, 16),
                    edgesToRemove = new Dictionary<int, int>(ref allocator, 16),
                    entitiesArr = new List<int>(ref allocator, 16),
                    entitiesContains = new HashSet<int>(ref allocator, 16),
                    componentIds = new List<int>(ref allocator, componentIds.Count(in allocator) - 1),
                    components = new Dictionary<int, Info>(ref allocator, components.Count(in allocator) - 1),
                };
                arch.componentIds.AddRange(ref allocator, componentIds);
                storage.isArchetypesDirty = true;
                var idx = storage.allArchetypes.Count(in allocator);
                arch.index = idx;
                
                var info = components[in allocator, componentId];
                arch.componentIds.RemoveAt(ref allocator, info.index);
                for (var i = 0; i < arch.componentIds.Count(in allocator); ++i) {
                    var cId = arch.componentIds[in allocator, i];
                    arch.components.Add(ref allocator, cId, new Info() {
                        index = i,
                    });
                    
                }

                if (node >= 0) {
                    arch.edgesToAdd.Add(ref allocator, componentId, node);
                }

                storage.dirtyArchetypes.Add(ref allocator, idx);
                storage.allArchetypes.Add(ref allocator, arch);
                
                return idx;

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public int GetHash(ref MemoryAllocator allocator) {
            
            if (this.dead.isCreated == false) return 0;
            
            return this.versions.GetHash(in allocator) ^ this.flags.GetHash(in allocator) ^ this.aliveCount ^ this.nextEntityId ^ this.dead.Count(in allocator) ^ this.allArchetypes.Count(in allocator);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void RemoveEntityFromArch(ref MemoryAllocator allocator, ref Archetype arch, int entityId) {

            var idx = this.GetEntityArrIndex(ref allocator, entityId);
            var movedEntityId = arch.entitiesArr[in allocator, arch.entitiesArr.Count(in allocator) - 1];
            arch.entitiesArr.RemoveAtFast(ref allocator, idx);
            arch.entitiesContains.Remove(ref allocator, entityId);
            if (movedEntityId != entityId) this.SetEntityArrIndex(ref allocator, movedEntityId, idx);
            this.SetEntityArrIndex(ref allocator, entityId, -1);
            
        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void AddEntityToArch(ref MemoryAllocator allocator, ref Archetype arch, int entityId) {

            var idx = arch.entitiesArr.Count(in allocator);
            arch.entitiesArr.Add(ref allocator, entityId);
            arch.entitiesContains.Add(ref allocator, entityId);
            this.SetEntityArrIndex(ref allocator, entityId, idx);
            
        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private int GetEntityArrIndex(ref MemoryAllocator allocator, int entityId) {

            this.entitiesArrIndex.Resize(ref allocator, entityId + 1);
            return this.entitiesArrIndex[in allocator, entityId];

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void SetEntityArrIndex(ref MemoryAllocator allocator, int entityId, int index) {

            this.entitiesArrIndex.Resize(ref allocator, entityId + 1);
            this.entitiesArrIndex[in allocator, entityId] = index;

        }

        private struct Request {

            public Entity entity;
            public byte op;
            public int componentId;
            public bool checkLambda;

        }

        [ME.ECS.Serializer.SerializeField]
        internal int forEachMode;
        [ME.ECS.Serializer.SerializeField]
        internal int root;
        [ME.ECS.Serializer.SerializeField]
        internal Dictionary<ulong, int> index;
        [ME.ECS.Serializer.SerializeField]
        internal List<Archetype> allArchetypes;
        [ME.ECS.Serializer.SerializeField]
        internal List<FilterData> filters;
        [ME.ECS.Serializer.SerializeField]
        internal ME.ECS.EntityVersions versions;
        [ME.ECS.Serializer.SerializeField]
        internal ME.ECS.EntityFlags flags;
        [ME.ECS.Serializer.SerializeField]
        internal List<int> entitiesArrIndex;
        [ME.ECS.Serializer.SerializeField]
        internal HashSet<int> dirtyArchetypes;

        #region Entities Storage
        public int AliveCount => this.aliveCount;
        public int DeadCount(in MemoryAllocator allocator) => this.dead.Count(in allocator);

        [ME.ECS.Serializer.SerializeField]
        internal MemArrayAllocator<Entity> cache;
        [ME.ECS.Serializer.SerializeField]
        internal List<int> dead;
        [ME.ECS.Serializer.SerializeField]
        internal List<int> deadPrepared;
        [ME.ECS.Serializer.SerializeField]
        internal List<int> alive;
        [ME.ECS.Serializer.SerializeField]
        private int aliveCount;
        [ME.ECS.Serializer.SerializeField]
        internal int nextEntityId;
        [ME.ECS.Serializer.SerializeField]
        internal bool isCreated;

        [ME.ECS.Serializer.SerializeField]
        private List<Request> requests;
        [ME.ECS.Serializer.SerializeField]
        private bool isArchetypesDirty;

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetCapacity(ref MemoryAllocator allocator, int capacity) {

            this.entitiesArrIndex.Resize(ref allocator, capacity);
            this.cache.Resize(ref allocator, capacity);
            this.dead.EnsureCapacity(ref allocator, capacity);
            this.alive.EnsureCapacity(ref allocator, capacity);
            this.deadPrepared.EnsureCapacity(ref allocator, capacity);
            
        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Initialize(ref MemoryAllocator allocator, int capacity) {

            this.entitiesArrIndex = new List<int>(ref allocator, capacity);

            this.cache = new MemArrayAllocator<Entity>(ref allocator, capacity);
            this.dead = new List<int>(ref allocator, capacity);
            this.alive = new List<int>(ref allocator, capacity);
            this.deadPrepared = new List<int>(ref allocator, capacity);
            this.versions = new ME.ECS.EntityVersions(ref allocator, capacity);
            this.flags = new ME.ECS.EntityFlags(ref allocator, capacity);
            this.aliveCount = 0;
            this.nextEntityId = -1;
            this.isCreated = true;
            this.forEachMode = 0;
            this.isArchetypesDirty = false;

            this.requests = new List<Request>(ref allocator, 10);

            var arch = new Archetype() {
                edgesToAdd = new Dictionary<int, int>(ref allocator, 16),
                edgesToRemove = new Dictionary<int, int>(ref allocator, 16),
                entitiesArr = new List<int>(ref allocator, 16),
                entitiesContains = new HashSet<int>(ref allocator, 16),
                componentIds = new List<int>(ref allocator, 10),
                components = new Dictionary<int, Archetype.Info>(ref allocator, 16),
                index = 0,
            };
            this.root = arch.index;
            this.index = new Dictionary<ulong, int>(ref allocator, 16);
            this.allArchetypes = new List<Archetype>(ref allocator, capacity);
            this.filters = new List<FilterData>(ref allocator, capacity);
            this.dirtyArchetypes = new HashSet<int>(ref allocator, 16);
            this.allArchetypes.Add(ref allocator, arch);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose(ref MemoryAllocator allocator) {
            
            this.versions.Dispose(ref allocator);
            this.versions = default;
            this.flags.Dispose(ref allocator);
            this.flags = default;
            
            this.root = default;
            this.isArchetypesDirty = default;
            this.requests.Dispose(ref allocator);
            this.forEachMode = default;
            this.isCreated = false;
            this.aliveCount = 0;
            this.nextEntityId = -1;
            
            this.index.Dispose(ref allocator);
            this.allArchetypes.Dispose(ref allocator);
            this.filters.Dispose(ref allocator);
            
            this.dirtyArchetypes.Dispose(ref allocator);
            this.entitiesArrIndex.Dispose(ref allocator);
            
            this.cache.Dispose(ref allocator);
            this.dead.Dispose(ref allocator);
            this.alive.Dispose(ref allocator);
            this.deadPrepared.Dispose(ref allocator);

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ref Entity GetEntityById(in MemoryAllocator allocator, int id) {

            return ref this.cache[in allocator, id];

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool IsAlive(in MemoryAllocator allocator, int id, ushort generation) {

            return this.cache[in allocator, id].generation == generation;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool ForEach(in MemoryAllocator allocator, ListCopyable<Entity> results) {

            results.Clear();
            for (var i = 0; i < this.alive.Count(in allocator); ++i) {
                results.Add(this.GetEntityById(in allocator, this.alive[in allocator, i]));
            }

            return true;

        }

        #if !ENTITIES_GROUP_DISABLED
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public unsafe void Alloc(ref MemoryAllocator allocator, int count, ref EntitiesGroup group, Unity.Collections.Allocator unityAllocator, bool copyMode) {

            var lastId = ++this.nextEntityId + count;
            this.cache.Resize(ref allocator, lastId + 1);

            this.aliveCount += count;

            var from = this.nextEntityId;
            var id = this.nextEntityId;
            for (var i = 0; i < count; ++i) {
                this.cache[in allocator, id] = new Entity(id, 1);
                this.OnAlloc(ref allocator, id);
                this.alive.Add(ref allocator, id++);
            }

            this.versions.Reset(ref allocator, id);
            this.flags.Reset(ref allocator, id);

            this.nextEntityId += count;

            var array = new Unity.Collections.NativeArray<Entity>(count, unityAllocator, Unity.Collections.NativeArrayOptions.UninitializedMemory);
            var size = sizeof(Entity);
            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemCpy(array.GetUnsafePtr(), this.cache.GetUnsafePtr(in allocator), size * count);
            group = new EntitiesGroup(from, from + count - 1, array, copyMode);

        }
        #endif

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ref Entity Alloc(ref MemoryAllocator allocator) {

            var id = -1;
            if (this.dead.Count(in allocator) > 0) {

                id = this.dead[in allocator, 0];
                this.dead.RemoveAtFast(ref allocator, 0);

            } else {

                id = ++this.nextEntityId;
                this.cache.Resize(ref allocator, id + 1);
                
            }

            ++this.aliveCount;
            ref var e = ref this.cache[in allocator, id];
            if (e.generation == 0) {
                e = new Entity(id, 1);
            }
            
            this.alive.Add(ref allocator, id);
            this.versions.Reset(ref allocator, id);
            this.flags.Reset(ref allocator, id);
            this.OnAlloc(ref allocator, id);
            
            return ref this.cache[in allocator, id];

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Dealloc(ref MemoryAllocator allocator, in Entity entity) {

            if (this.IsAlive(in allocator, entity.id, entity.generation) == false) {
                return false;
            }

            //UnityEngine.Debug.Log("Dealloc: " + entity + ", tick: " + Worlds.current.GetCurrentTick());
            this.deadPrepared.Add(ref allocator, entity.id);

            return true;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void ApplyDead(ref MemoryAllocator allocator) {

            var cnt = this.deadPrepared.Count(in allocator);
            if (cnt > 0) {

                for (var i = 0; i < cnt; ++i) {

                    var id = this.deadPrepared[in allocator, i];
                    --this.aliveCount;
                    this.dead.Add(ref allocator, id);
                    this.alive.Remove(ref allocator, id);
                    this.OnDealloc(ref allocator, id);

                }

                this.deadPrepared.Clear(in allocator);

            }

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private void OnAlloc(ref MemoryAllocator allocator, int entityId) {

            ref var arch = ref this.allArchetypes[in allocator, this.root];
            this.AddEntityToArch(ref allocator, ref arch, entityId);
            this.index.Add(ref allocator, (ulong)entityId << 32, this.root);

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private void OnDealloc(ref MemoryAllocator allocator, int entityId) {

            // Remove from archetype
            var key = (ulong)entityId << 32;
            var archIdx = this.index.GetValueAndRemove(ref allocator, key);
            ref var arch = ref this.allArchetypes[in allocator, archIdx];
            this.RemoveEntityFromArch(ref allocator, ref arch, entityId);

        }
        #endregion

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public ref FilterData GetFilter(in MemoryAllocator allocator, int id) {

            return ref this.filters[in allocator, id - 1];

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private bool TryGetArchetypeAdd(ref MemoryAllocator allocator, List<int> componentIds, int componentId, out int arch) {

            // Try to search archetype with componentIds + componentId contained in
            arch = default;
            for (var i = 0; i < this.allArchetypes.Count(in allocator); ++i) {

                ref var ar = ref this.allArchetypes[in allocator, i];
                if (ar.componentIds.Count(in allocator) == componentIds.Count(in allocator) &&
                    ar.Has(in allocator, componentId) == true &&
                    ar.HasAll(in allocator, componentIds) == true) {

                    arch = i;
                    return true;

                }

            }

            return false;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private bool TryGetArchetypeRemove(ref MemoryAllocator allocator, List<int> componentIds, int componentId, out int arch) {

            // Try to search archetype with componentIds except componentId contained in
            arch = default;
            for (var i = 0; i < this.allArchetypes.Count(in allocator); ++i) {

                ref var ar = ref this.allArchetypes[in allocator, i];
                if (ar.componentIds.Count(in allocator) == componentIds.Count(in allocator) - 1 &&
                    ar.Has(in allocator, componentId) == false &&
                    ar.HasAllExcept(in allocator, componentIds, componentId) == true) {

                    arch = i;
                    return true;

                }

            }

            return false;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Has<T>(in MemoryAllocator allocator, in Entity entity) where T : struct {

            var key = (ulong)entity.id << 32;
            var archIdx = this.index[in allocator, key];
            var arch = this.allArchetypes[in allocator, archIdx];
            return arch.Has(in allocator, ComponentTypes<T>.typeId);

        }

        #if !ENTITIES_GROUP_DISABLED
        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(ref MemoryAllocator allocator, in EntitiesGroup group, int componentId, bool checkLambda) {

            for (var i = group.fromId; i <= group.toId; ++i) {

                this.Set(ref allocator, this.GetEntityById(in allocator, i), componentId, checkLambda);

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove(ref MemoryAllocator allocator, in EntitiesGroup group, int componentId, bool checkLambda) {

            for (var i = group.fromId; i <= group.toId; ++i) {

                this.Remove(ref allocator, this.GetEntityById(in allocator, i), componentId, checkLambda);

            }

        }
        #endif

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate<T>(ref MemoryAllocator allocator, in Entity entity, bool makeRequest) where T : struct {

            this.Validate(ref allocator, in entity, ComponentTypes<T>.typeId, makeRequest);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(ref MemoryAllocator allocator, in Entity entity, int componentId, bool makeRequest) {

            if (makeRequest == true) {

                // Add request and apply set on next UpdateFilters call
                this.AddValidateRequest(ref allocator, in entity, componentId);

            } else {

                #if !FILTERS_LAMBDA_DISABLED
                if (ComponentTypesLambda.itemsSet.TryGetValue(componentId, out var lambda) == true) {
                    lambda.Invoke(entity);
                }
                #endif

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(ref MemoryAllocator allocator, in Entity entity, int componentId, bool checkLambda) {

            #if !FILTERS_LAMBDA_DISABLED
            if (checkLambda == true && ComponentTypesLambda.itemsSet.TryGetValue(componentId, out var lambda) == true) {
                lambda.Invoke(entity);
            }
            #endif

            if (this.forEachMode > 0) {

                // Add request
                this.AddSetRequest(ref allocator, in entity, componentId, checkLambda);
                return;

            }

            var key = (ulong)entity.id << 32;
            ref var archIdx = ref this.index.GetValue(ref allocator, key);
            ref var arch = ref this.allArchetypes[in allocator, archIdx];
            archIdx = arch.Set(ref allocator, ref this, entity, componentId).index;
            this.index[in allocator, key] = archIdx;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set<T>(ref MemoryAllocator allocator, in Entity entity) where T : struct {

            #if !FILTERS_LAMBDA_DISABLED
            var checkLambda = ComponentTypes<T>.isFilterLambda;
            #else
            var checkLambda = false;
            #endif
            this.Set(ref allocator, in entity, ComponentTypes<T>.typeId, checkLambda);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove(ref MemoryAllocator allocator, in Entity entity) { }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove(ref MemoryAllocator allocator, in Entity entity, int componentId, bool checkLambda) {

            #if !FILTERS_LAMBDA_DISABLED
            if (checkLambda == true && ComponentTypesLambda.itemsRemove.TryGetValue(componentId, out var lambda) == true) {
                lambda.Invoke(entity);
            }
            #endif

            if (this.forEachMode > 0) {

                // Add request
                this.AddRemoveRequest(ref allocator, in entity, componentId, checkLambda);
                return;

            }

            var key = (ulong)entity.id << 32;
            ref var archIdx = ref this.index.GetValue(ref allocator, key);
            ref var arch = ref this.allArchetypes[in allocator, archIdx];
            archIdx = arch.Remove(ref allocator, ref this, entity, componentId).index;
            this.index[in allocator, key] = archIdx;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove<T>(ref MemoryAllocator allocator, in Entity entity) where T : struct {

            #if !FILTERS_LAMBDA_DISABLED
            var checkLambda = ComponentTypes<T>.isFilterLambda;
            #else
            var checkLambda = false;
            #endif
            this.Remove(ref allocator, in entity, ComponentTypes<T>.typeId, checkLambda);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void ApplyAllRequests(ref MemoryAllocator allocator) {

            var e = this.requests.GetEnumerator(in allocator);
            while (e.MoveNext() == true) {

                var req = e.Current;
                if (req.entity.IsAlive() == false) {
                    continue;
                }

                if (req.op == 1) {

                    this.Set(ref allocator, in req.entity, req.componentId, req.checkLambda);

                } else if (req.op == 2) {

                    this.Remove(ref allocator, in req.entity, req.componentId, req.checkLambda);

                } else if (req.op == 3) {

                    this.Validate(ref allocator, in req.entity, req.componentId, false);

                }

            }
            e.Dispose();

            this.requests.Clear(in allocator);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void AddValidateRequest(ref MemoryAllocator allocator, in Entity entity, int componentId) {

            this.requests.Add(ref allocator, new Request() {
                entity = entity,
                componentId = componentId,
                op = 3,
            });

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void AddSetRequest(ref MemoryAllocator allocator, in Entity entity, int componentId, bool checkLambda) {

            this.requests.Add(ref allocator, new Request() {
                entity = entity,
                componentId = componentId,
                op = 1,
                checkLambda = checkLambda,
            });

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void AddRemoveRequest(ref MemoryAllocator allocator, in Entity entity, int componentId, bool checkLambda) {

            this.requests.Add(ref allocator, new Request() {
                entity = entity,
                componentId = componentId,
                op = 2,
                checkLambda = checkLambda,
            });

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public int Count(State state, ref MemoryAllocator allocator, Filter filter) {

            return this.Count(state, ref allocator, filter.id);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public int Count(State state, ref MemoryAllocator allocator, int filterId) {

            var filterStaticData = Worlds.current.GetFilterStaticData(filterId);
            if (FiltersArchetype.FiltersArchetypeStorage.CheckStaticShared(filterStaticData.data.containsShared, filterStaticData.data.notContainsShared) == false) {
                return 0;
            }

            if (this.forEachMode == 0) {
                this.UpdateFilters(state, ref allocator);
            }
            
            var onChanged = filterStaticData.data.onChanged;
            var changedTracked = onChanged.Count;
            
            var connectedFilters = filterStaticData.data.connectedFilters;
            var connectedTracked = connectedFilters.Count;

            ref var filter = ref this.GetFilter(in allocator, filterId);
            var count = 0;
            for (int i = 0, cnt = filter.archetypes.Count(in allocator); i < cnt; ++i) {

                var archId = filter.archetypesList[in allocator, i];
                var arch = this.allArchetypes[in allocator, archId];
                if (changedTracked > 0 || connectedTracked > 0) {

                    for (int index = 0; index < arch.entitiesArr.Count(in allocator); ++index) {

                        var entityId = arch.entitiesArr[in allocator, index];
                        
                        if (connectedTracked > 0) {
                            var entity = this.GetEntityById(in allocator, entityId);
                            // Check if all custom filters contains connected entity
                            var found = true;
                            for (int j = 0, cntj = connectedTracked; j < cntj; ++j) {
                                var connectedFilter = connectedFilters[j];
                                if (connectedFilter.filter.Contains(in allocator, connectedFilter.get.Invoke(entity)) == false) {
                                    found = false;
                                    break;
                                }
                            }
                            
                            if (found == false) {
                                continue;
                            }
                        }

                        if (changedTracked > 0) {
                            var hasChanged = false;
                            // Check if any component has changed on this entity
                            for (int j = 0, cntj = changedTracked; j < cntj; ++j) {
                                var typeId = onChanged[j];
                                var reg = Worlds.current.currentState.structComponents.list.arr[typeId];
                                if (reg.HasChanged(entityId) == true) {
                                    hasChanged = true;
                                    break;
                                }
                            }

                            if (hasChanged == false) {
                                continue;
                            }
                        }
                        
                        ++count;
                        
                    }
                    
                }
                
                if (changedTracked == 0 && connectedTracked == 0) {

                    count += arch.entitiesArr.Count(in allocator);

                }

            }

            return count;

        }

        public void MarkAllArchetypesAsDirty(ref MemoryAllocator allocator) {

            for (int archId = 0, cnt2 = this.allArchetypes.Count(in allocator); archId < cnt2; ++archId) {
                
                this.dirtyArchetypes.Add(ref allocator, archId);
                
            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void UpdateFilters(State state, ref MemoryAllocator allocator) {

            if (this.forEachMode > 0) {
                return;
            }

            this.ApplyDead(ref allocator);
            this.ApplyAllRequests(ref allocator);

            if (this.isArchetypesDirty == true) {

                this.isArchetypesDirty = false;
                var world = Worlds.current;
                for (int idx = 0, cnt = this.filters.Count(in allocator); idx < cnt; ++idx) {
                    
                    ref var item = ref this.filters[in allocator, idx];
                    var e = this.dirtyArchetypes.GetEnumerator(state);
                    while (e.MoveNext() == true) {

                        var archId = e.Current;
                        if (item.archetypes.Contains(in allocator, archId) == true) continue;

                        var filterStaticData = world.GetFilterStaticData(item.id);
                        ref var arch = ref this.allArchetypes[in allocator, archId];
                        
                        if (arch.HasAll(in allocator, filterStaticData.data.contains) == true &&
                            arch.HasNotAll(in allocator, filterStaticData.data.notContains) == true &&
                            arch.HasAnyPair(in allocator, filterStaticData.data.anyPair2) == true &&
                            arch.HasAnyPair(in allocator, filterStaticData.data.anyPair3) == true &&
                            arch.HasAnyPair(in allocator, filterStaticData.data.anyPair4) == true
                            #if !FILTERS_LAMBDA_DISABLED
                            && FiltersArchetypeStorage.CheckLambdas(in allocator, in arch, filterStaticData.data.lambdas) == true
                            #endif
                            ) {

                            item.archetypes.Add(ref allocator, archId);
                            item.archetypesList.Add(ref allocator, archId);

                        }

                    }
                    e.Dispose();
                    
                }

                this.dirtyArchetypes.Clear(in allocator);

            }

        }
        
        #if !FILTERS_LAMBDA_DISABLED
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static bool CheckLambdas(in MemoryAllocator allocator, in Archetype arch, ListCopyable<int> lambdas) {

            return arch.HasAll(in allocator, lambdas);

        }
        #endif

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        internal static bool CheckStaticShared(ListCopyable<int> containsShared, ListCopyable<int> notContainsShared) {

            if (containsShared.Count == 0 && notContainsShared.Count == 0) {
                return true;
            }

            var w = Worlds.current;
            for (int i = 0, count = containsShared.Count; i < count; ++i) {

                if (w.HasSharedDataBit(containsShared[i]) == false) {
                    return false;
                }

            }

            for (int i = 0, count = notContainsShared.Count; i < count; ++i) {

                if (w.HasSharedDataBit(notContainsShared[i]) == true) {
                    return false;
                }

            }

            return true;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool TryGetFilter(in MemoryAllocator allocator, FilterBuilder filterBuilder, out FilterData filterData) {

            filterData = default;

            var world = Worlds.current;
            for (int i = 0, cnt = this.filters.Count(in allocator); i < cnt; ++i) {

                var filter = this.filters[in allocator, i];
                var filterStaticData = world.GetFilterStaticData(filter.id);
                if (filterStaticData.isCreated == false) continue;
                
                if (filterStaticData.data.withinTicks == filterBuilder.data.withinTicks &&
                    filterStaticData.data.withinType == filterBuilder.data.withinType &&
                    filterStaticData.data.withinMinChunkSize == filterBuilder.data.withinMinChunkSize &&
                    FiltersArchetypeStorage.IsEquals(filterStaticData.data.contains, filterBuilder.data.contains) == true &&
                    FiltersArchetypeStorage.IsEquals(filterStaticData.data.notContains, filterBuilder.data.notContains) == true &&
                    FiltersArchetypeStorage.IsEquals(filterStaticData.data.notContainsShared, filterBuilder.data.notContainsShared) == true &&
                    FiltersArchetypeStorage.IsEquals(filterStaticData.data.containsShared, filterBuilder.data.containsShared) == true &&
                    FiltersArchetypeStorage.IsEquals(filterStaticData.data.onChanged, filterBuilder.data.onChanged) == true &&
                    FiltersArchetypeStorage.IsEquals(filterStaticData.data.anyPair2, filterBuilder.data.anyPair2) == true &&
                    FiltersArchetypeStorage.IsEquals(filterStaticData.data.anyPair3, filterBuilder.data.anyPair3) == true &&
                    FiltersArchetypeStorage.IsEquals(filterStaticData.data.anyPair4, filterBuilder.data.anyPair4) == true &&
                    FiltersArchetypeStorage.IsEquals(filterStaticData.data.connectedFilters, filterBuilder.data.connectedFilters) == true
                    #if !FILTERS_LAMBDA_DISABLED
                    && FiltersArchetypeStorage.IsEquals(filterStaticData.data.lambdas, filterBuilder.data.lambdas) == true
                    #endif
                    ) {

                    filterData = filter;
                    return true;

                }

            }

            return false;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private static bool IsEquals(ListCopyable<int> list1, ListCopyable<int> list2) {

            if (list1.Count != list2.Count) {
                return false;
            }

            for (var i = 0; i < list1.Count; ++i) {

                if (list2.Contains(list1[i]) == false) {
                    return false;
                }

            }

            return true;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private static bool IsEquals<T>(ListCopyable<T> list1, ListCopyable<T> list2) where T : struct, System.IEquatable<T> {

            if (list1.Count != list2.Count) {
                return false;
            }

            for (var i = 0; i < list1.Count; ++i) {

                if (list2.Contains(list1[i]) == false) {
                    return false;
                }

            }

            return true;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private static bool IsEquals(ListCopyable<ConnectInfo> list1, ListCopyable<ConnectInfo> list2) {

            return list1.Count == 0 && list2.Count == 0;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool WillNew(in MemoryAllocator allocator) {

            return this.dead.Count(in allocator) == 0;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public List<int> GetAlive() {

            return this.alive;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Entity IncrementGeneration(in MemoryAllocator allocator, in Entity entity) {

            // Make this entity not alive, but not completely destroyed at this time
            this.cache[in allocator, entity.id] = new Entity(entity.id, unchecked((ushort)(entity.generation + 1)));
            return ref this.cache[in allocator, entity.id];

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetFreeze(bool freeze) { }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsDeadPrepared(in MemoryAllocator allocator, int entityId) {

            if (this.deadPrepared.Count(in allocator) == 0) return false;

            return this.deadPrepared.Contains(in allocator, entityId);

        }

    }

}