#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ME.ECS.Collections;

#if FILTERS_STORAGE_ARCHETYPES
namespace ME.ECS {
    
    using FiltersArchetype;

    public partial class World {

        internal void OnSpawnFilters() {
            
        }

        internal void OnRecycleFilters() {
            
        }
        
        public void Register(ref FiltersArchetypeStorage storageRef, bool freeze, bool restore) {
            
            this.RegisterPluginsModuleForEntity();

            if (storageRef.isCreated == false) {

                storageRef = new FiltersArchetypeStorage();
                storageRef.Initialize(World.ENTITIES_CACHE_CAPACITY);
                storageRef.SetFreeze(freeze);

            }

            if (freeze == false) {

                if (this.sharedEntity.id == 0 && this.sharedEntityInitialized == false) {

                    // Create shared entity which should store shared components
                    this.sharedEntityInitialized = true;
                    this.sharedEntity = this.AddEntity();

                }

            }

            if (restore == true) {

                this.BeginRestoreEntities();

                // Update entities cache
                var list = PoolListCopyable<Entity>.Spawn(World.ENTITIES_CACHE_CAPACITY);
                if (this.ForEachEntity(list) == true) {

                    for (int i = 0; i < list.Count; ++i) {

                        ref var item = ref list[i];
                        // This call resets FilterData.dataVersions[item.id] to true which might create state desynchronization
                        // in case entity hadn't been updated on the previous tick. FilterData seems to have its state already
                        // stored within the main state, so it's possible that this call is not needed at all.
                        //this.UpdateFiltersOnFilterCreate(item);
                        this.CreateEntityPlugins(item);

                    }

                }

                PoolListCopyable<Entity>.Recycle(ref list);

                this.EndRestoreEntities();

            }
            
        }

        public void SetEntityCapacityInFilters(int capacity) {
            
            // On change capacity
            
        }

        public void CreateEntityInFilters(in Entity entity) {
            
            // On create new entity
            
        }

        public void RemoveFromAllFilters(in Entity entity) {
            
            // On destroy entity
            this.currentState.filters.Remove(in entity);
            
        }

        public void UpdateFilters(in EntitiesGroup group) {

            // Force to update entity group in filters
            this.currentState.filters.UpdateFilters();
            
        }

        public void UpdateFilters(in Entity entity) {

            // Force to update entity in filters
            this.currentState.filters.UpdateFilters();
            
        }

        public void AddFilterByStructComponent(in Entity entity, int componentId) {
            
            this.currentState.filters.Set(in entity, componentId);
            
        }

        public void RemoveFilterByStructComponent(in Entity entity, int componentId) {
            
            this.currentState.filters.Remove(in entity, componentId);
            
        }

        public void UpdateFilterByStructComponent(in Entity entity, int componentId) {
            
        }

        public void AddFilterByStructComponent<T>(in Entity entity) where T : struct, IStructComponentBase {
            
            this.currentState.filters.Set<T>(in entity);
            
        }

        public void RemoveFilterByStructComponent<T>(in Entity entity) where T : struct, IStructComponentBase {
            
            this.currentState.filters.Remove<T>(in entity);
            
        }

        public void UpdateFilterByStructComponent<T>(in Entity entity) where T : struct, IStructComponentBase {
            
        }

        public void UpdateFilterByStructComponentVersioned<T>(in Entity entity) where T : struct, IStructComponentBase {
            
        }

        public void RemoveComponentFromFilter(in Entity entity) {
            
            // Remove all components from entity
            this.RemoveFromAllFilters(in entity);
            
        }

        public void AddComponentToFilter(in Entity entity) {
            
            // Update filters for this entity
            
        }

        public FilterData GetFilter(int id) {

            return this.currentState.filters.GetFilter(id);

        }
        
    }

    public struct Filter {
        
        public static Filter Empty = new Filter();
        
        public struct Enumerator : IEnumerator<Entity> {

            //public Unity.Collections.NativeArray<Entity> arr;
            public FilterData filterData;
            public int index;
            public int archIndex;
            public HashSetCopyable<int>.Enumerator localEnumerator;
            private Entity current;

            public bool MoveNext() {

                if (this.archIndex >= this.filterData.archetypes.Count) return false;
                
                ++this.index;
                var arch = this.filterData.storage.allArchetypes[this.filterData.archetypes[this.archIndex]];
                if (this.index >= arch.entitiesList.Count) {

                    ++this.archIndex;
                    if (this.archIndex < this.filterData.archetypes.Count) {
                        this.localEnumerator.Dispose();
                        this.localEnumerator = this.filterData.storage.allArchetypes[this.filterData.archetypes[this.archIndex]].entitiesList.GetEnumerator();
                    }
                    this.index = -1;
                    return this.MoveNext();

                }
                
                this.localEnumerator.MoveNext();
                this.current = this.filterData.storage.GetEntityById(this.localEnumerator.Current);
                
                return true;

            }

            public void Reset() {

                this.index = -1;

            }

            object IEnumerator.Current => this.Current;

            public Entity Current => this.current;

            public void Dispose() {

                //this.arr.Dispose();
                this.index = -1;
                this.archIndex = 0;
                this.localEnumerator.Dispose();
                
                ref var filters = ref Worlds.current.currentState.filters;
                --filters.forEachMode;

                if (filters.forEachMode == 0) {

                    filters.UpdateFilters();

                }

            }

        }

        public int id;

        public World world => Worlds.current;
        public int Count => Worlds.current.currentState.filters.Count(this);

        public static FilterBuilder CreateFromData(FilterDataTypes data) {

            var dataInternal = FilterInternalData.Create();
            
            foreach (var component in data.with) {

                if (ComponentTypesRegistry.allTypeId.TryGetValue(component.GetType(), out var index) == true) {
                    
                    dataInternal.contains.Add(index);
                    
                }
                
            }

            foreach (var component in data.without) {

                if (ComponentTypesRegistry.allTypeId.TryGetValue(component.GetType(), out var index) == true) {
                    
                    dataInternal.notContains.Add(index);
                    
                }
                
            }

            return new FilterBuilder() {
                data = dataInternal,
            };
            
        }
        
        public Enumerator GetEnumerator() {

            ref var filters = ref Worlds.current.currentState.filters;
            ++filters.forEachMode;
            var filterData = filters.GetFilter(this.id);
            return new Enumerator() {
                index = -1,
                archIndex = 0,
                localEnumerator = filterData.archetypes.Count > 0 ? filterData.storage.allArchetypes[filterData.archetypes[0]].entitiesList.GetEnumerator() : default,
                filterData = filterData,
                //arr = this.ToArray(Unity.Collections.Allocator.Temp),
            };

        }

        public void GetBounds(out int min, out int max) {

            var filterData = Worlds.current.currentState.filters.GetFilter(this.id);
            min = int.MaxValue;
            max = int.MinValue;
            foreach (var archId in filterData.archetypes) {

                var arch = filterData.storage.allArchetypes[archId];
                foreach (var e in arch.entitiesList) {

                    if (e < min) min = e;
                    if (e > max) max = e;

                }
                
            }
            
        }

        public Unity.Collections.NativeArray<Entity> ToArray(Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) {
            
            var filterData = Worlds.current.currentState.filters.GetFilter(this.id);
            var result = new Unity.Collections.NativeArray<Entity>(filterData.storage.Count(filterData), allocator);
            var k = 0;
            foreach (var archId in filterData.archetypes) {

                var arch = filterData.storage.allArchetypes[archId];
                foreach (var e in arch.entitiesList) {

                    result[k++] = filterData.storage.GetEntityById(e);

                }
                
            }

            return result;

        }

        public bool Contains(in Entity entity) {

            var filterData = Worlds.current.currentState.filters.GetFilter(this.id);
            return filterData.Contains(in entity);
            
        }

        public bool IsAlive() {

            return this.id > 0;

        }

        public delegate void InjectDelegate(ref FilterBuilder builder);

        internal static InjectDelegate currentInject;
        public static void RegisterInject(InjectDelegate injectFilter) {

            Filter.currentInject += injectFilter;

        }
        
        public static void UnregisterInject(InjectDelegate injectFilter) {
            
            Filter.currentInject -= injectFilter;
            
        }

        public static FilterBuilder Create(string name = null) {

            var data = FilterInternalData.Create();
            data.name = name;
            return new FilterBuilder() {
                data = data,
            };
            
        }

    }

    public struct FilterData {

        public struct CopyData : IArrayElementCopy<FilterData> {
            
            public void Copy(FilterData @from, ref FilterData to) {

                to.CopyFrom(from);

            }

            public void Recycle(FilterData item) {

                item.Recycle();

            }

        }

        public ME.ECS.FiltersArchetype.FiltersArchetypeStorage storage => Worlds.current.currentState.filters;

        public int id;
        internal FilterInternalData data;
        internal List<int> archetypes;
        public bool isAlive;

        public void CopyFrom(FilterData other) {

            this.id = other.id;
            this.data.CopyFrom(other.data);
            ArrayUtils.Copy(other.archetypes, ref this.archetypes);
            this.isAlive = other.isAlive;

        }

        public void Recycle() {

            this.id = 0;
            this.isAlive = false;
            this.data.Recycle();
            PoolList<int>.Recycle(ref this.archetypes);

        }
        
        public int Count => this.storage.Count(this);

        internal string ToEditorTypesString() {

            var str = string.Empty;
            foreach (var c in this.data.contains) {
                var type = string.Empty;
                foreach (var t in ComponentTypesRegistry.typeId) {
                    if (t.Value == c) {
                        type = t.Key.Name;
                        break;
                    }
                }
                str += $"W<{type}>";
            }
            foreach (var c in this.data.notContains) {
                var type = string.Empty;
                foreach (var t in ComponentTypesRegistry.typeId) {
                    if (t.Value == c) {
                        type = t.Key.Name;
                        break;
                    }
                }
                str += $"WO<{type}>";
            }
            return str;

        }

        public string[] GetAllNames() {
            
            return new string[] {
                this.data.name,
            };
            
        }

        public void ApplyAllRequests() {
            
            // Apply all requests after enumeration has ended
            
        }

        public bool Contains(in Entity entity) {

            foreach (var archId in this.archetypes) {

                var arch = this.storage.allArchetypes[archId];
                if (arch.entitiesList.Contains(entity.id) == true) return true;

            }

            return false;

        }

    }

    internal struct FilterInternalData {

        public struct Pair2 {

            public int t1;
            public int t2;

        }

        public struct Pair3 {

            public int t1;
            public int t2;
            public int t3;

        }

        public struct Pair4 {

            public int t1;
            public int t2;
            public int t3;
            public int t4;

        }

        internal string name;
        
        internal List<Pair2> anyPair2;
        internal List<Pair3> anyPair3;
        internal List<Pair4> anyPair4;

        internal List<int> contains;
        internal List<int> notContains;
        
        internal List<int> containsShared;
        internal List<int> notContainsShared;

        public void CopyFrom(FilterInternalData other) {

            this.name = other.name;
            
            ArrayUtils.Copy(other.anyPair2, ref this.anyPair2);
            ArrayUtils.Copy(other.anyPair3, ref this.anyPair3);
            ArrayUtils.Copy(other.anyPair4, ref this.anyPair4);
            ArrayUtils.Copy(other.contains, ref this.contains);
            ArrayUtils.Copy(other.notContains, ref this.notContains);
            ArrayUtils.Copy(other.containsShared, ref this.containsShared);
            ArrayUtils.Copy(other.notContainsShared, ref this.notContainsShared);

        }

        public void Recycle() {

            this.name = default;
            
            PoolList<Pair2>.Recycle(ref this.anyPair2);
            PoolList<Pair3>.Recycle(ref this.anyPair3);
            PoolList<Pair4>.Recycle(ref this.anyPair4);
            PoolList<int>.Recycle(ref this.contains);
            PoolList<int>.Recycle(ref this.notContains);
            PoolList<int>.Recycle(ref this.containsShared);
            PoolList<int>.Recycle(ref this.notContainsShared);
            
        }

        public static FilterInternalData Create() {
            
            return new FilterInternalData() {
                name = string.Empty,
                anyPair2 = new List<Pair2>(),
                anyPair3 = new List<Pair3>(),
                anyPair4 = new List<Pair4>(),
                contains = new List<int>(),
                notContains = new List<int>(),
                containsShared = new List<int>(),
                notContainsShared = new List<int>(),
            };
            
        }

    }
    
    public struct FilterBuilder {

        internal ME.ECS.FiltersArchetype.FiltersArchetypeStorage storage => Worlds.current.currentState.filters;

        internal FilterInternalData data;
        
        public FilterBuilder WithoutShared<T>() where T : struct {
            
            WorldUtilities.SetComponentTypeId<T>();
            this.data.notContainsShared.Add(ComponentTypes<T>.typeId);
            return this;

        }

        public FilterBuilder WithShared<T>() where T : struct {
            
            WorldUtilities.SetComponentTypeId<T>();
            this.data.containsShared.Add(ComponentTypes<T>.typeId);
            return this;

        }

        public FilterBuilder With<T>() where T : struct {

            WorldUtilities.SetComponentTypeId<T>();
            this.data.contains.Add(ComponentTypes<T>.typeId);
            return this;

        }

        public FilterBuilder Without<T>() where T : struct {

            WorldUtilities.SetComponentTypeId<T>();
            this.data.notContains.Add(ComponentTypes<T>.typeId);
            return this;

        }

        public FilterBuilder Any<T1, T2>() where T1 : struct where T2 : struct {

            WorldUtilities.SetComponentTypeId<T1>();
            WorldUtilities.SetComponentTypeId<T2>();
            this.data.anyPair2.Add(new FilterInternalData.Pair2() {
                t1 = ComponentTypes<T1>.typeId,
                t2 = ComponentTypes<T2>.typeId,
            });
            return this;

        }

        public FilterBuilder Any<T1, T2, T3>() where T1 : struct where T2 : struct where T3 : struct {

            WorldUtilities.SetComponentTypeId<T1>();
            WorldUtilities.SetComponentTypeId<T2>();
            WorldUtilities.SetComponentTypeId<T3>();
            this.data.anyPair3.Add(new FilterInternalData.Pair3() {
                t1 = ComponentTypes<T1>.typeId,
                t2 = ComponentTypes<T2>.typeId,
                t3 = ComponentTypes<T3>.typeId,
            });
            return this;

        }

        public FilterBuilder Any<T1, T2, T3, T4>() where T1 : struct where T2 : struct where T3 : struct where T4 : struct {

            WorldUtilities.SetComponentTypeId<T1>();
            WorldUtilities.SetComponentTypeId<T2>();
            WorldUtilities.SetComponentTypeId<T3>();
            WorldUtilities.SetComponentTypeId<T4>();
            this.data.anyPair4.Add(new FilterInternalData.Pair4() {
                t1 = ComponentTypes<T1>.typeId,
                t2 = ComponentTypes<T2>.typeId,
                t3 = ComponentTypes<T3>.typeId,
                t4 = ComponentTypes<T4>.typeId,
            });
            return this;

        }

        public FilterBuilder OnVersionChangedOnly() {
            throw new System.NotImplementedException("OnVersionChangedOnly can't be used with FILTERS_STORAGE_ARCHETYPES.");
        }

        public Filter Push() {

            Filter _ = default;
            return this.Push(ref _);

        }

        public Filter Push(ref Filter filter) {

            if (Filter.currentInject != null) Filter.currentInject.Invoke(ref this);
            
            if (this.storage.TryGetFilter(this, out var filterData) == true) {
                
                // Already has filter with this restrictions
                filter = new Filter() {
                    id = filterData.id,
                };
                return filter;

            }
            
            var nextId = this.storage.filters.Count + 1;
            filterData = new FilterData() {
                id = nextId,
                isAlive = true,
                data = this.data,
                archetypes = new List<int>(),
            };
            this.storage.filters.Add(filterData);
            filter = new Filter() {
                id = nextId,
            };
            return filter;

        }

    }

}

namespace ME.ECS.FiltersArchetype {

    public struct FiltersArchetypeStorage : IStorage {

        public struct Archetype {

            public struct CopyData : IArrayElementCopy<Archetype> {
                
                public void Copy(Archetype @from, ref Archetype to) {

                    to.CopyFrom(in from);

                }

                public void Recycle(Archetype item) {

                    item.Recycle();

                }

            }

            private void CopyFrom(in Archetype other) {

                if (this.components == null && other.components != null) {
                    this.components = new DictionaryUInt<Info>();
                } else if (this.components != null && other.components == null) {
                    this.components = null;
                }
                if (this.components != null) this.components.CopyFrom(other.components);
                this.index = other.index;
                ArrayUtils.Copy(other.componentIds, ref this.componentIds);
                ArrayUtils.Copy(other.entitiesList, ref this.entitiesList);
                if (this.edges == null && other.edges != null) {
                    this.edges = new DictionaryUInt<Edge>();
                } else if (this.edges != null && other.edges == null) {
                    this.edges = null;
                }
                if (this.edges != null) this.edges.CopyFrom(other.edges);

            }

            private void Recycle() {

                this.index = default;
                this.components = null;
                this.componentIds = null;
                this.entitiesList = null;
                this.edges = null;

            }

            public struct Edge {

                public bool isCreated;
                public int add;
                public int remove;

            }

            public struct Info {

                public int index; // Index in list

            }

            public int index;
            public DictionaryUInt<Info> components; // Contains componentId => Info index
            public List<int> componentIds; // Contains raw list of component ids
            public HashSetCopyable<int> entitiesList; // Contains raw list of entities
            public DictionaryUInt<Edge> edges; // Contains edges to move from this archetype to another
            
            //private bool isCreated;

            internal bool HasAnyPair(List<FilterInternalData.Pair2> list) {

                foreach (var pair in list) {

                    if (this.Has(pair.t1) == false &&
                        this.Has(pair.t2) == false) {
                        return false;
                    }

                }
                
                return true;

            }

            internal bool HasAnyPair(List<FilterInternalData.Pair3> list) {

                foreach (var pair in list) {

                    if (this.Has(pair.t1) == false &&
                        this.Has(pair.t2) == false &&
                        this.Has(pair.t3) == false) {
                        return false;
                    }

                }
                
                return true;

            }

            internal bool HasAnyPair(List<FilterInternalData.Pair4> list) {

                foreach (var pair in list) {

                    if (this.Has(pair.t1) == false &&
                        this.Has(pair.t2) == false &&
                        this.Has(pair.t3) == false &&
                        this.Has(pair.t4) == false) {
                        return false;
                    }

                }
                
                return true;

            }

            public bool Has(int componentId) {

                return this.components.ContainsKey(componentId);

            }

            public bool HasAll(List<int> componentIds) {

                foreach (var item in componentIds) {

                    if (this.components.ContainsKey(item) == false) return false;

                }
                
                return true;

            }

            public bool HasNotAll(List<int> componentIds) {

                foreach (var item in componentIds) {

                    if (this.components.ContainsKey(item) == true) return false;

                }
                
                return true;

            }

            public bool HasAllExcept(List<int> componentIds, int componentId) {

                foreach (var item in componentIds) {

                    if (item == componentId) continue;
                    if (this.components.ContainsKey(item) == false) return false;

                }
                
                return true;

            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public void Remove(ref FiltersArchetypeStorage storage, Entity entity) {

                this.entitiesList.Remove(entity.id);

            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public Archetype Set(ref FiltersArchetypeStorage storage, Entity entity, int componentId) {

                if (this.Has(componentId) == true) return this;
                
                // Remove entity from current archetype
                this.entitiesList.Remove(entity.id);

                // Find the edge to move
                ref var edge = ref this.edges.GetValue(componentId);
                if (edge.isCreated == false) edge = new Edge() {
                    isCreated = true,
                    add = -1,
                    remove = -1,
                };
                
                {
                    
                    if (edge.add == -1) {
                        edge.add = CreateAdd(ref storage, this.index, this.componentIds, this.components, componentId);
                    }

                    var arch = storage.allArchetypes[edge.add];
                    arch.entitiesList.Add(entity.id);
                    
                }

                // Return the new archetype we are moved to
                return storage.allArchetypes[edge.add];

            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public Archetype Remove(ref FiltersArchetypeStorage storage, Entity entity, int componentId) {

                if (this.Has(componentId) == false) return this;

                // Remove entity from current archetype
                this.entitiesList.Remove(entity.id);
                
                // Find the edge to move
                ref var edge = ref this.edges.GetValue(componentId);
                if (edge.isCreated == false) edge = new Edge() {
                    isCreated = true,
                    add = -1,
                    remove = -1,
                };
                
                {
                    
                    if (edge.remove == -1) {
                        edge.remove = CreateRemove(ref storage, this.index, this.componentIds, this.components, componentId);
                    }
                    
                    var arch = storage.allArchetypes[edge.remove];
                    arch.entitiesList.Add(entity.id);

                }
                
                // Return the new archetype we are moved to
                return storage.allArchetypes[edge.remove];

            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            internal static int CreateAdd(ref FiltersArchetypeStorage storage, int node, List<int> componentIds, DictionaryUInt<Info> components, int componentId) {

                if (storage.TryGetArchetypeAdd(componentIds, componentId, out var ar) == true) return ar;

                var arch = new Archetype() {
                    //isCreated = true,
                    edges = new DictionaryUInt<Edge>(),
                    entitiesList = new HashSetCopyable<int>(),
                    componentIds = new List<int>(componentIds),
                    components = new DictionaryUInt<Info>(components),
                    //componentStorage = new ComponentStorage[componentIds.Count + 1],
                };
                storage.isArchetypesDirty = true;
                var idx = storage.allArchetypes.Count;
                arch.index = idx;
                storage.allArchetypes.Add(arch);
                arch.components.Add(componentId, new Info() {
                    index = arch.componentIds.Count,
                });
                arch.componentIds.Add(componentId);
                if (node >= 0) {
                    arch.edges.Add(componentId, new Edge() {
                        isCreated = true,
                        remove = node,
                        add = -1,
                    });
                }

                return idx;

            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            internal static int CreateRemove(ref FiltersArchetypeStorage storage, int node, List<int> componentIds, DictionaryUInt<Info> components, int componentId) {

                if (storage.TryGetArchetypeRemove(componentIds, componentId, out var ar) == true) return ar;
                
                var arch = new Archetype() {
                    //isCreated = true,
                    edges = new DictionaryUInt<Edge>(),
                    entitiesList = new HashSetCopyable<int>(),
                    componentIds = new List<int>(componentIds),
                    components = new DictionaryUInt<Info>(),
                    //componentStorage = new ComponentStorage[componentIds.Count - 1],
                };
                storage.isArchetypesDirty = true;
                var idx = storage.allArchetypes.Count;
                arch.index = idx;
                storage.allArchetypes.Add(arch);
                var info = components[componentId];
                arch.componentIds.RemoveAt(info.index);
                for (int i = 0; i < arch.componentIds.Count; ++i) {
                    var cId = arch.componentIds[i];
                    arch.components.Add(cId, new Info() {
                        index = i,
                    });
                }
                
                if (node >= 0) {
                    arch.edges.Add(componentId, new Edge() {
                        isCreated = true,
                        add = node,
                        remove = -1,
                    });
                }

                return idx;

            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public void Dispose() {

                //if (this.isCreated == false) return;
                //this.isCreated = false;
                
                /*foreach (var storage in this.componentStorage) {
                    
                    storage.Dispose();
                    
                }*/
                
            }

        }

        public struct NullArchetypes {

            public void Set<T>(in Entity entity) {}
            public void Remove<T>(in Entity entity) {}
            
            public void Set(in Entity entity, int componentId) {}
            public void Remove(in Entity entity, int componentId) {}

            public void Set(in EntitiesGroup group, int componentId) {}
            public void Remove(in EntitiesGroup group, int componentId) {}

            public void Set(int entityId, int componentId) {}
            public void Remove(int entityId, int componentId) {}

            public void Clear(in Entity entity) {}

            public void Validate(int capacity) {}

            public void Validate(in Entity entity) {}

            public void CopyFrom(in Entity @from, in Entity to) {
                
            }

        }

        private struct Request {

            public Entity entity;
            public byte op;
            public int componentId;

        }

        [ME.ECS.Serializer.SerializeField]
        internal int forEachMode;
        [ME.ECS.Serializer.SerializeField]
        internal int root;
        [ME.ECS.Serializer.SerializeField]
        internal DictionaryULong<int> index;
        [ME.ECS.Serializer.SerializeField]
        internal List<Archetype> allArchetypes;
        [ME.ECS.Serializer.SerializeField]
        internal List<FilterData> filters;
        [ME.ECS.Serializer.SerializeField]
        internal EntityVersions versions;
        
        internal NullArchetypes archetypes; // Used for backward compability
        
        #region Entities Storage
        public int AliveCount => this.aliveCount;
        public int DeadCount => this.dead.Count;

        internal NativeBufferArray<Entity> cache;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> dead;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> deadPrepared;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> alive;
        [ME.ECS.Serializer.SerializeField]
        private int aliveCount;
        [ME.ECS.Serializer.SerializeField]
        private int nextEntityId;
        [ME.ECS.Serializer.SerializeField]
        internal bool isCreated;

        private List<Request> requests;
        private bool isArchetypesDirty;
        
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Initialize(int capacity) {
            
            this.cache = PoolArrayNative<Entity>.Spawn(capacity);
            this.dead = PoolListCopyable<int>.Spawn(capacity);
            this.alive = PoolListCopyable<int>.Spawn(capacity);
            this.deadPrepared = PoolListCopyable<int>.Spawn(capacity);
            this.versions = new EntityVersions();
            this.aliveCount = 0;
            this.nextEntityId = -1;
            this.isCreated = true;
            this.forEachMode = 0;
            this.isArchetypesDirty = false;

            this.requests = PoolList<Request>.Spawn(10);

            var arch = new Archetype() {
                edges = new DictionaryUInt<Archetype.Edge>(),
                entitiesList = new HashSetCopyable<int>(),
                componentIds = new List<int>(),
                components = new DictionaryUInt<Archetype.Info>(),
                index = 0,
            };
            this.root = arch.index;
            this.index = new DictionaryULong<int>();
            this.allArchetypes = new List<Archetype>();
            this.filters = new List<FilterData>();
            this.allArchetypes.Add(arch);

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void CopyFrom(FiltersArchetypeStorage other) {
            
            if (this.isCreated == false || this.requests == null) this.Initialize(0);

            ArrayUtils.Copy(other.requests, ref this.requests);

            this.isCreated = other.isCreated;
            this.forEachMode = other.forEachMode;
            NativeArrayUtils.Copy(other.cache, ref this.cache);
            ArrayUtils.Copy(other.dead, ref this.dead);
            ArrayUtils.Copy(other.alive, ref this.alive);
            ArrayUtils.Copy(other.deadPrepared, ref this.deadPrepared);
            this.aliveCount = other.aliveCount;
            this.nextEntityId = other.nextEntityId;
            this.versions.CopyFrom(other.versions);
            this.isArchetypesDirty = other.isArchetypesDirty;

            this.root = other.root;
            ArrayUtils.Copy(other.filters, ref this.filters, new FilterData.CopyData());
            if (this.index == null && other.index != null) {
                this.index = new DictionaryULong<int>();
            } else if (this.index != null && other.index == null) {
                this.index = null;
            }
            if (this.index != null) this.index.CopyFrom(other.index);
            ArrayUtils.Copy(other.allArchetypes, ref this.allArchetypes, new Archetype.CopyData());

        }
        
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Recycle() {

            this.root = default;
            this.index = null;
            this.allArchetypes = null;
            this.filters = null;
            this.isArchetypesDirty = default;
            
            PoolList<Request>.Recycle(ref this.requests);
            
            PoolArrayNative<Entity>.Recycle(ref this.cache);
            PoolListCopyable<int>.Recycle(ref this.dead);
            PoolListCopyable<int>.Recycle(ref this.alive);
            PoolListCopyable<int>.Recycle(ref this.deadPrepared);
            this.forEachMode = default;
            this.isCreated = false;
            this.aliveCount = 0;
            this.nextEntityId = -1;
            this.versions.Recycle();
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ref Entity GetEntityById(int id) {
            
            return ref this.cache[id];
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool IsAlive(int id, ushort generation) {

            return this.cache[id].generation == generation;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool ForEach(ListCopyable<Entity> results) {

            results.Clear();
            for (int i = 0; i < this.alive.Count; ++i) {
                results.Add(this.GetEntityById(this.alive[i]));
            }
            
            return true;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Alloc(int count, ref EntitiesGroup group, Unity.Collections.Allocator allocator, bool copyMode) {
            
            var lastId = ++this.nextEntityId + count;
            NativeArrayUtils.Resize(lastId, ref this.cache);

            this.aliveCount += count;

            var from = this.nextEntityId;
            var id = this.nextEntityId;
            for (int i = 0; i < count; ++i) {
                this.cache.arr[id] = new Entity(id, 1);
                this.OnAlloc(id);
                this.alive.Add(id++);
            }
            this.versions.Reset(id);

            this.nextEntityId += count;

            var slice = new Unity.Collections.NativeSlice<Entity>(this.cache.arr, from, count);
            var array = new Unity.Collections.NativeArray<Entity>(count, allocator, Unity.Collections.NativeArrayOptions.UninitializedMemory);
            slice.CopyTo(array);
            group = new EntitiesGroup(from, from + count - 1, array, copyMode);
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ref Entity Alloc() {

            int id = -1;
            if (this.dead.Count > 0) {

                id = this.dead[0];
                this.dead.RemoveAtFast(0);

            } else {

                id = ++this.nextEntityId;
                NativeArrayUtils.Resize(id, ref this.cache);

            }

            ++this.aliveCount;
            ref var e = ref this.cache[id];
            this.alive.Add(id);
            if (e.generation == 0) e = new Entity(id, 1);
            this.versions.Reset(id);
            
            this.OnAlloc(e.id);

            return ref e;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Dealloc(in Entity entity) {
            
            if (this.IsAlive(entity.id, entity.generation) == false) return false;

            //using (NoStackTrace.All) UnityEngine.Debug.Log("Dealloc: " + entity + ", tick: " + Worlds.current.GetCurrentTick());
            this.deadPrepared.Add(entity.id);

            return true;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void ApplyDead() {
            
            var cnt = this.deadPrepared.Count;
            if (cnt > 0) {

                for (int i = 0; i < cnt; ++i) {

                    var id = this.deadPrepared[i];
                    --this.aliveCount;
                    this.dead.Add(id);
                    this.alive.Remove(id);
                    this.OnDealloc(id);
                    
                }

                this.deadPrepared.Clear();

            }

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private void OnAlloc(int entityId) {

            var arch = this.allArchetypes[this.root];
            arch.entitiesList.Add(entityId);
            this.index.Add((ulong)entityId << 32, this.root);
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private void OnDealloc(int entityId) {

            // Remove from archetype
            var key = ((ulong)entityId) << 32;
            var archIdx = this.index.GetValueAndRemove(key);
            var arch = this.allArchetypes[archIdx];
            arch.entitiesList.Remove(entityId);

        }
        #endregion

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public FilterData GetFilter(int id) {

            return this.filters[id - 1];

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private bool TryGetArchetypeAdd(List<int> componentIds, int componentId, out int arch) {

            // Try to search archetype with componentIds + componentId contained in
            arch = default;
            for (var i = 0; i < this.allArchetypes.Count; ++i) {
                
                var ar = this.allArchetypes[i];
                if (ar.componentIds.Count == componentIds.Count &&
                    ar.Has(componentId) == true &&
                    ar.HasAll(componentIds) == true) {

                    arch = i;
                    return true;

                }
                
            }

            return false;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private bool TryGetArchetypeRemove(List<int> componentIds, int componentId, out int arch) {

            // Try to search archetype with componentIds except componentId contained in
            arch = default;
            for (var i = 0; i < this.allArchetypes.Count; ++i) {
                
                var ar = this.allArchetypes[i];
                if (ar.componentIds.Count == componentIds.Count - 1 &&
                    ar.Has(componentId) == false &&
                    ar.HasAllExcept(componentIds, componentId) == true) {

                    arch = i;
                    return true;

                }
                
            }

            return false;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>(in Entity entity) where T : struct {

            var key = ((ulong)entity.id) << 32;
            var archIdx = this.index[key];
            var arch = this.allArchetypes[archIdx];
            return arch.Has(ComponentTypes<T>.typeId);

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Set(in EntitiesGroup group, int componentId) {

            for (int i = group.fromId; i <= group.toId; ++i) {
                
                this.Set(this.GetEntityById(i), componentId);
                
            }
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Remove(in EntitiesGroup group, int componentId) {

            for (int i = group.fromId; i <= group.toId; ++i) {
                
                this.Remove(this.GetEntityById(i), componentId);
                
            }
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Set(in Entity entity, int componentId) {

            if (this.forEachMode > 0) {
                
                // Add request
                this.AddSetRequest(in entity, componentId);
                return;

            }
            
            var key = ((ulong)entity.id) << 32;
            ref var archIdx = ref this.index.GetValue(key);
            var arch = this.allArchetypes[archIdx];
            archIdx = arch.Set(ref this, entity, componentId).index;
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Set<T>(in Entity entity) where T : struct {

            if (this.forEachMode > 0) {
                
                // Add request
                this.AddSetRequest(in entity, ComponentTypes<T>.typeId);
                return;

            }

            var key = ((ulong)entity.id) << 32;
            ref var archIdx = ref this.index.GetValue(key);
            var arch = this.allArchetypes[archIdx];
            archIdx = arch.Set(ref this, entity, ComponentTypes<T>.typeId).index;
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Remove(in Entity entity) {
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Remove(in Entity entity, int componentId) {
            
            if (this.forEachMode > 0) {
                
                // Add request
                this.AddRemoveRequest(in entity, componentId);
                return;

            }

            var key = ((ulong)entity.id) << 32;
            ref var archIdx = ref this.index.GetValue(key);
            var arch = this.allArchetypes[archIdx];
            archIdx = arch.Remove(ref this, entity, componentId).index;
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Remove<T>(in Entity entity) where T : struct {
            
            if (this.forEachMode > 0) {
                
                // Add request
                this.AddRemoveRequest(in entity, ComponentTypes<T>.typeId);
                return;

            }

            var key = ((ulong)entity.id) << 32;
            ref var archIdx = ref this.index.GetValue(key);
            var arch = this.allArchetypes[archIdx];
            archIdx = arch.Remove(ref this, entity, ComponentTypes<T>.typeId).index;
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private void ApplyAllRequests() {

            foreach (var req in this.requests) {

                if (req.entity.IsAlive() == false) continue;
                
                if (req.op == 1) {
                    
                    this.Set(in req.entity, req.componentId);
                    
                } else if (req.op == 2) {
                    
                    this.Remove(in req.entity, req.componentId);
                    
                }
                
            }
            this.requests.Clear();

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private void AddSetRequest(in Entity entity, int componentId) {

            this.requests.Add(new Request() {
                entity = entity,
                componentId = componentId,
                op = 1,
            });

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private void AddRemoveRequest(in Entity entity, int componentId) {
            
            this.requests.Add(new Request() {
                entity = entity,
                componentId = componentId,
                op = 2,
            });
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Each(FilterData filter, System.Action<FiltersArchetypeStorage, Entity> callback) {

            foreach (var archId in filter.archetypes) {

                var arch = this.allArchetypes[archId];
                foreach (var e in arch.entitiesList) {
                    
                    callback.Invoke(this, this.GetEntityById(e));
                    
                }
                
            }
            
            this.UpdateFilters();
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public int Count(Filter filter) {

            return this.Count(this.GetFilter(filter.id));

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public int Count(FilterData filter) {

            if (this.forEachMode == 0) this.UpdateFilters();
            
            var count = 0;
            foreach (var archId in filter.archetypes) {

                var arch = this.allArchetypes[archId];
                count += arch.entitiesList.Count;

            }

            return count;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Dispose() {

            foreach (var arch in this.allArchetypes) {

                arch.Dispose();

            }
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void UpdateFilters() {

            if (this.forEachMode > 0) return;

            this.ApplyDead();
            this.ApplyAllRequests();

            if (this.isArchetypesDirty == true) {
                
                this.isArchetypesDirty = false;
                foreach (var item in this.filters) {

                    item.archetypes.Clear();

                    for (var i = 0; i < this.allArchetypes.Count; ++i) {
                        
                        var arch = this.allArchetypes[i];
                        if (arch.HasAll(item.data.contains) == true &&
                            arch.HasNotAll(item.data.notContains) == true &&
                            arch.HasAnyPair(item.data.anyPair2) == true &&
                            arch.HasAnyPair(item.data.anyPair3) == true &&
                            arch.HasAnyPair(item.data.anyPair4) == true &&
                            this.CheckStaticShared(item.data.containsShared, item.data.notContainsShared) == true) {

                            item.archetypes.Add(i);

                        }

                    }

                }

            }
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private bool CheckStaticShared(List<int> containsShared, List<int> notContainsShared) {

            if (containsShared.Count == 0 && notContainsShared.Count == 0) return true;
            
            var w = Worlds.current;
            for (int i = 0, count = containsShared.Count; i < count; ++i) {

                if (w.HasSharedDataBit(containsShared[i]) == false) return false;

            }
            
            for (int i = 0, count = notContainsShared.Count; i < count; ++i) {

                if (w.HasSharedDataBit(notContainsShared[i]) == true) return false;

            }

            return true;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool TryGetFilter(FilterBuilder filterBuilder, out FilterData filterData) {

            filterData = default;
            
            for (int i = 0, cnt = this.filters.Count; i < cnt; ++i) {

                var filter = this.filters[i];
                if (this.IsEquals(filter.data.contains, filterBuilder.data.contains) == true &&
                    this.IsEquals(filter.data.notContains, filterBuilder.data.notContains) == true &&
                    this.IsEquals(filter.data.notContainsShared, filterBuilder.data.notContainsShared) == true &&
                    this.IsEquals(filter.data.containsShared, filterBuilder.data.containsShared) == true) {

                    filterData = filter;
                    return true;

                }

            }

            return false;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private bool IsEquals(List<int> list1, List<int> list2) {

            if (list1.Count != list2.Count) return false;

            for (int i = 0; i < list1.Count; ++i) {

                if (list2.Contains(list1[i]) == false) return false;
                
            }
            
            return true;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool WillNew() {
            
            return this.dead.Count == 0;
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ListCopyable<int> GetAlive() {

            return this.alive;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ref Entity IncrementGeneration(in Entity entity) {

            // Make this entity not alive, but not completely destroyed at this time
            this.cache[entity.id] = new Entity(entity.id, unchecked((ushort)(entity.generation + 1)));
            return ref this.cache[entity.id];

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void SetFreeze(bool freeze) {
            
        }

    }

}
#endif