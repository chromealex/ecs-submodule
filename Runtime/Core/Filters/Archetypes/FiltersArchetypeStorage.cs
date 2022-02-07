//#if ENABLE_IL2CPP
#define INLINE_METHODS
//#endif

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ME.ECS.Collections;
using Unity.IL2CPP.CompilerServices;
using Il2Cpp = Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute;

#if FILTERS_STORAGE_ARCHETYPES
namespace ME.ECS {

    using FiltersArchetype;

    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
    public partial class World {

        internal void OnSpawnFilters() { }

        internal void OnRecycleFilters() { }

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

                    for (var i = 0; i < list.Count; ++i) {

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

        public void UpdateFilterByStructComponent(in Entity entity, int componentId) { }

        public void ValidateFilterByStructComponent(in Entity entity, int componentId, bool makeRequest = false) {

            this.currentState.filters.Validate(in entity, componentId, makeRequest);

        }

        public void ValidateFilterByStructComponent<T>(in Entity entity, bool makeRequest = false) where T : struct, IStructComponentBase {

            this.currentState.filters.Validate<T>(in entity, makeRequest);

        }

        public void AddFilterByStructComponent<T>(in Entity entity) where T : struct, IStructComponentBase {

            this.currentState.filters.Set<T>(in entity);

        }

        public void RemoveFilterByStructComponent<T>(in Entity entity) where T : struct, IStructComponentBase {

            this.currentState.filters.Remove<T>(in entity);

        }

        public void UpdateFilterByStructComponent<T>(in Entity entity) where T : struct, IStructComponentBase { }

        public void UpdateFilterByStructComponentVersioned<T>(in Entity entity) where T : struct, IStructComponentBase { }

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

    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
    public struct Filter {

        public static Filter Empty = new Filter();

        public struct Enumerator : IEnumerator<Entity> {

            public FilterData filterData;
            public int index;
            public int archIndex;
            //public HashSetCopyable<int>.Enumerator localEnumerator;
            public ListCopyable<int> arr;
            private Entity current;

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public bool MoveNext() {

                if (this.archIndex >= this.filterData.archetypes.Count) {
                    return false;
                }

                ++this.index;
                var arch = this.filterData.storage.allArchetypes[this.filterData.archetypes[this.archIndex]];
                if (this.index >= arch.entitiesArr.Count) {

                    ++this.archIndex;
                    if (this.archIndex < this.filterData.archetypes.Count) {
                        this.arr = this.filterData.storage.allArchetypes[this.filterData.archetypes[this.archIndex]].entitiesArr;
                    }

                    this.index = -1;
                    return this.MoveNext();

                }

                this.current = this.filterData.storage.GetEntityById(this.arr[this.index]);

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Reset() {

                this.index = -1;

            }

            object IEnumerator.Current => this.Current;

            public Entity Current {
                #if INLINE_METHODS
                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                #endif
                get => this.current;
            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Dispose() {

                //this.arr.Dispose();
                this.index = -1;
                this.archIndex = 0;
                //this.localEnumerator.Dispose();

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

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
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

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public Enumerator GetEnumerator() {

            ref var filters = ref Worlds.current.currentState.filters;
            ++filters.forEachMode;
            var filterData = filters.GetFilter(this.id);
            return new Enumerator() {
                index = -1,
                archIndex = 0,
                //localEnumerator = filterData.archetypes.Count > 0 ? filterData.storage.allArchetypes[filterData.archetypes[0]].entitiesList.GetEnumerator() : default,
                filterData = filterData,
                arr = filterData.archetypes.Count > 0 ? filterData.storage.allArchetypes[filterData.archetypes[0]].entitiesArr : default,
            };

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void GetBounds(out int min, out int max) {

            var filterData = Worlds.current.currentState.filters.GetFilter(this.id);
            min = int.MaxValue;
            max = int.MinValue;
            foreach (var archId in filterData.archetypes) {

                var arch = filterData.storage.allArchetypes[archId];
                for (int i = 0, count = arch.entitiesArr.Count; i < count; ++i) {
                    
                    var e = arch.entitiesArr[i];
                    if (e < min) {
                        min = e;
                    }

                    if (e > max) {
                        max = e;
                    }
                    
                }

            }

        }

        public Unity.Collections.NativeArray<Entity> ToArray(Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) {

            var filterData = Worlds.current.currentState.filters.GetFilter(this.id);
            var result = new Unity.Collections.NativeArray<Entity>(filterData.storage.Count(filterData), allocator);
            var k = 0;
            foreach (var archId in filterData.archetypes) {

                var arch = filterData.storage.allArchetypes[archId];
                for (int i = 0, count = arch.entitiesArr.Count; i < count; ++i) {
                    
                    result[k++] = filterData.storage.GetEntityById(arch.entitiesArr[i]);
                    
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

    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
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
                if (arch.entitiesArr.IndexOf(entity.id) >= 0) {
                    return true;
                }

            }

            return false;

        }

    }

    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
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

        internal List<int> lambdas;

        public void CopyFrom(FilterInternalData other) {

            this.name = other.name;

            ArrayUtils.Copy(other.anyPair2, ref this.anyPair2);
            ArrayUtils.Copy(other.anyPair3, ref this.anyPair3);
            ArrayUtils.Copy(other.anyPair4, ref this.anyPair4);
            ArrayUtils.Copy(other.contains, ref this.contains);
            ArrayUtils.Copy(other.notContains, ref this.notContains);
            ArrayUtils.Copy(other.containsShared, ref this.containsShared);
            ArrayUtils.Copy(other.notContainsShared, ref this.notContainsShared);
            ArrayUtils.Copy(other.lambdas, ref this.lambdas);

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
            PoolList<int>.Recycle(ref this.lambdas);

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
                lambdas = new List<int>(),
            };

        }

    }

    public interface ILambda<T> where T : struct {

        bool Execute(in T data);

    }

    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
    public struct FilterBuilder {

        internal ME.ECS.FiltersArchetype.FiltersArchetypeStorage storage => Worlds.current.currentState.filters;

        internal FilterInternalData data;

        public FilterBuilder WithLambda<T, TComponent>() where T : struct, ILambda<TComponent> where TComponent : struct, IStructComponent {

            System.Action<Entity> setAction = (e) => {
                if (new T().Execute(in e.Read<TComponent>())) {
                    Worlds.current.currentState.filters.Set<T>(e);
                } else {
                    Worlds.current.currentState.filters.Remove<T>(e);
                }
            };
            System.Action<Entity> removeAction = (e) => { Worlds.current.currentState.filters.Remove<T>(e); };

            WorldUtilities.SetComponentTypeId<T>();
            WorldUtilities.SetComponentTypeId<TComponent>();

            var key = ComponentTypes<TComponent>.typeId;
            {
                if (ComponentTypesLambda.itemsSet.TryGetValue(key, out var actions) == true) {

                    actions += setAction;
                    ComponentTypesLambda.itemsSet[key] = actions;

                } else {

                    ComponentTypesLambda.itemsSet.Add(key, setAction);

                }
            }

            {
                if (ComponentTypesLambda.itemsRemove.TryGetValue(key, out var actions) == true) {

                    actions += removeAction;
                    ComponentTypesLambda.itemsRemove[key] = actions;

                } else {

                    ComponentTypesLambda.itemsRemove.Add(key, removeAction);

                }
            }

            this.data.lambdas.Add(ComponentTypes<T>.typeId);
            return this.With<TComponent>();

        }

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

            if (Filter.currentInject != null) {
                Filter.currentInject.Invoke(ref this);
            }

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

    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
    public struct FiltersArchetypeStorage : IStorage {

        [Il2Cpp(Option.NullChecks, false)]
        [Il2Cpp(Option.ArrayBoundsChecks, false)]
        [Il2Cpp(Option.DivideByZeroChecks, false)]
        public struct Archetype {

            [Il2Cpp(Option.NullChecks, false)]
            [Il2Cpp(Option.ArrayBoundsChecks, false)]
            [Il2Cpp(Option.DivideByZeroChecks, false)]
            public struct CopyData : IArrayElementCopy<Archetype> {

                public void Copy(Archetype @from, ref Archetype to) {

                    to.CopyFrom(in from);

                }

                public void Recycle(Archetype item) {

                    item.Recycle();

                }

            }

            private void CopyFrom(in Archetype other) {

                this.index = other.index;
                ArrayUtils.Copy(other.componentIds, ref this.componentIds);
                ArrayUtils.Copy(other.entitiesArr, ref this.entitiesArr);
                
                ArrayUtils.Copy(other.edges, ref this.edges);
                ArrayUtils.Copy(other.components, ref this.components);

            }

            public void Recycle() {

                this.index = default;
                PoolDictionaryInt<Info>.Recycle(ref this.components);
                PoolList<int>.Recycle(ref this.componentIds);
                PoolListCopyable<int>.Recycle(ref this.entitiesArr);
                PoolDictionaryInt<Edge>.Recycle(ref this.edges);

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
            public DictionaryInt<Info> components; // Contains componentId => Info index
            public List<int> componentIds; // Contains raw list of component ids
            public ListCopyable<int> entitiesArr; // Contains raw list of entities
            public DictionaryInt<Edge> edges; // Contains edges to move from this archetype to another

            //private bool isCreated;

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            internal readonly bool HasAnyPair(List<FilterInternalData.Pair2> list) {

                foreach (var pair in list) {

                    if (this.Has(pair.t1) == false &&
                        this.Has(pair.t2) == false) {
                        return false;
                    }

                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            internal readonly bool HasAnyPair(List<FilterInternalData.Pair3> list) {

                foreach (var pair in list) {

                    if (this.Has(pair.t1) == false &&
                        this.Has(pair.t2) == false &&
                        this.Has(pair.t3) == false) {
                        return false;
                    }

                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            internal readonly bool HasAnyPair(List<FilterInternalData.Pair4> list) {

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

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public readonly bool Has(int componentId) {

                return this.components.ContainsKey(componentId);

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public readonly bool HasAll(List<int> componentIds) {

                foreach (var item in componentIds) {

                    if (this.components.ContainsKey(item) == false) {
                        return false;
                    }

                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public readonly bool HasNotAll(List<int> componentIds) {

                foreach (var item in componentIds) {

                    if (this.components.ContainsKey(item) == true) {
                        return false;
                    }

                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public readonly bool HasAllExcept(List<int> componentIds, int componentId) {

                foreach (var item in componentIds) {

                    if (item == componentId) {
                        continue;
                    }

                    if (this.components.ContainsKey(item) == false) {
                        return false;
                    }

                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public Archetype Set(ref FiltersArchetypeStorage storage, Entity entity, int componentId) {

                if (this.Has(componentId) == true) {
                    return this;
                }

                // Remove entity from current archetype
                storage.RemoveEntityFromArch(ref this, entity.id);

                // Find the edge to move
                ref var edge = ref this.edges.GetValue(componentId);
                if (edge.isCreated == false) {
                    edge = new Edge() {
                        isCreated = true,
                        add = -1,
                        remove = -1,
                    };
                }

                {

                    if (edge.add == -1) {
                        edge.add = Archetype.CreateAdd(ref storage, this.index, this.componentIds, this.components, componentId);
                    }

                    ref var arch = ref storage.allArchetypes[edge.add];
                    storage.AddEntityToArch(ref arch, entity.id);

                }

                // Return the new archetype we are moved to
                return storage.allArchetypes[edge.add];

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public Archetype Remove(ref FiltersArchetypeStorage storage, Entity entity, int componentId) {

                if (this.Has(componentId) == false) {
                    return this;
                }

                // Remove entity from current archetype
                storage.RemoveEntityFromArch(ref this, entity.id);

                // Find the edge to move
                ref var edge = ref this.edges.GetValue(componentId);
                if (edge.isCreated == false) {
                    edge = new Edge() {
                        isCreated = true,
                        add = -1,
                        remove = -1,
                    };
                }

                {

                    if (edge.remove == -1) {
                        edge.remove = Archetype.CreateRemove(ref storage, this.index, this.componentIds, this.components, componentId);
                    }

                    ref var arch = ref storage.allArchetypes[edge.remove];
                    storage.AddEntityToArch(ref arch, entity.id);

                }

                // Return the new archetype we are moved to
                return storage.allArchetypes[edge.remove];

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            internal static int CreateAdd(ref FiltersArchetypeStorage storage, int node, List<int> componentIds, DictionaryInt<Info> components, int componentId) {

                if (storage.TryGetArchetypeAdd(componentIds, componentId, out var ar) == true) {
                    return ar;
                }

                var arch = new Archetype() {
                    //isCreated = true,
                    edges = PoolDictionaryInt<Edge>.Spawn(16),
                    entitiesArr = PoolListCopyable<int>.Spawn(16),
                    componentIds = PoolList<int>.Spawn(componentIds.Count),
                    components = PoolDictionaryInt<Info>.Spawn(components.Count),
                    //componentStorage = new ComponentStorage[componentIds.Count + 1],
                };
                foreach (var c in components) {
                    arch.components.Add(c.Key, c.Value);
                }

                arch.componentIds.AddRange(componentIds);
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

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            internal static int CreateRemove(ref FiltersArchetypeStorage storage, int node, List<int> componentIds, DictionaryInt<Info> components, int componentId) {

                if (storage.TryGetArchetypeRemove(componentIds, componentId, out var ar) == true) {
                    return ar;
                }

                var arch = new Archetype() {
                    //isCreated = true,
                    edges = PoolDictionaryInt<Edge>.Spawn(16),
                    entitiesArr = PoolListCopyable<int>.Spawn(16),
                    componentIds = PoolList<int>.Spawn(componentIds.Count),
                    components = PoolDictionaryInt<Info>.Spawn(16),
                    //componentStorage = new ComponentStorage[componentIds.Count - 1],
                };
                arch.componentIds.AddRange(componentIds);
                storage.isArchetypesDirty = true;
                var idx = storage.allArchetypes.Count;
                arch.index = idx;
                storage.allArchetypes.Add(arch);
                var info = components[componentId];
                arch.componentIds.RemoveAt(info.index);
                for (var i = 0; i < arch.componentIds.Count; ++i) {
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

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void RemoveEntityFromArch(ref Archetype arch, int entityId) {

            var idx = this.GetEntityArrIndex(entityId);
            var movedEntityId = arch.entitiesArr[arch.entitiesArr.Count - 1];
            arch.entitiesArr.RemoveAtFast(idx);
            if (movedEntityId != entityId) this.SetEntityArrIndex(movedEntityId, idx);
            this.SetEntityArrIndex(entityId, -1);
            
        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void AddEntityToArch(ref Archetype arch, int entityId) {

            var idx = arch.entitiesArr.Count;
            arch.entitiesArr.Add(entityId);
            this.SetEntityArrIndex(entityId, idx);
            
        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private int GetEntityArrIndex(int entityId) {

            ArrayUtils.Resize(entityId, ref this.entitiesArrIndex);
            return this.entitiesArrIndex[entityId];

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void SetEntityArrIndex(int entityId, int index) {

            ArrayUtils.Resize(entityId, ref this.entitiesArrIndex);
            this.entitiesArrIndex[entityId] = index;

        }

        public struct NullArchetypes {

            public void Set<T>(in Entity entity) { }
            public void Remove<T>(in Entity entity) { }

            public void Set(in Entity entity, int componentId) { }
            public void Remove(in Entity entity, int componentId) { }

            public void Set(in EntitiesGroup group, int componentId) { }
            public void Remove(in EntitiesGroup group, int componentId) { }

            public void Set(int entityId, int componentId) { }
            public void Remove(int entityId, int componentId) { }

            public void Clear(in Entity entity) { }

            public void Validate(int capacity) { }

            public void Validate(in Entity entity) { }

            public void CopyFrom(in Entity @from, in Entity to) { }

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
        internal ListCopyable<Archetype> allArchetypes;
        [ME.ECS.Serializer.SerializeField]
        internal List<FilterData> filters;
        [ME.ECS.Serializer.SerializeField]
        internal EntityVersions versions;
        [ME.ECS.Serializer.SerializeField]
        internal BufferArray<int> entitiesArrIndex;

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

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Initialize(int capacity) {

            this.entitiesArrIndex = PoolArray<int>.Spawn(capacity);

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
                edges = PoolDictionaryInt<Archetype.Edge>.Spawn(16),
                entitiesArr = PoolListCopyable<int>.Spawn(16),
                componentIds = PoolList<int>.Spawn(10),
                components = PoolDictionaryInt<Archetype.Info>.Spawn(16),
                index = 0,
            };
            this.root = arch.index;
            this.index = PoolDictionaryULong<int>.Spawn(16);
            this.allArchetypes = PoolListCopyable<Archetype>.Spawn(capacity);
            this.filters = PoolList<FilterData>.Spawn(capacity);
            this.allArchetypes.Add(arch);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void CopyFrom(FiltersArchetypeStorage other) {

            if (this.isCreated == false || this.requests == null) {
                this.Initialize(0);
            }

            ArrayUtils.Copy(other.requests, ref this.requests);

            ArrayUtils.Copy(other.entitiesArrIndex, ref this.entitiesArrIndex);

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
            ArrayUtils.Copy(other.index, ref this.index);
            
            ArrayUtils.Copy(other.allArchetypes, ref this.allArchetypes, new Archetype.CopyData());

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Recycle() {

            for (int i = 0; i < this.allArchetypes.Count; ++i) {
                
                this.allArchetypes[i].Recycle();
                
            }
            
            this.versions.Recycle();
            this.versions = default;
            
            this.root = default;
            PoolDictionaryULong<int>.Recycle(ref this.index);
            PoolListCopyable<Archetype>.Recycle(ref this.allArchetypes);
            PoolList<FilterData>.Recycle(ref this.filters);
            this.isArchetypesDirty = default;

            PoolArray<int>.Recycle(ref this.entitiesArrIndex);

            PoolList<Request>.Recycle(ref this.requests);

            PoolArrayNative<Entity>.Recycle(ref this.cache);
            PoolListCopyable<int>.Recycle(ref this.dead);
            PoolListCopyable<int>.Recycle(ref this.alive);
            PoolListCopyable<int>.Recycle(ref this.deadPrepared);
            this.forEachMode = default;
            this.isCreated = false;
            this.aliveCount = 0;
            this.nextEntityId = -1;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Entity GetEntityById(int id) {

            return ref this.cache[id];

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsAlive(int id, ushort generation) {

            return this.cache[id].generation == generation;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool ForEach(ListCopyable<Entity> results) {

            results.Clear();
            for (var i = 0; i < this.alive.Count; ++i) {
                results.Add(this.GetEntityById(this.alive[i]));
            }

            return true;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Alloc(int count, ref EntitiesGroup group, Unity.Collections.Allocator allocator, bool copyMode) {

            var lastId = ++this.nextEntityId + count;
            NativeArrayUtils.Resize(lastId, ref this.cache);

            this.aliveCount += count;

            var from = this.nextEntityId;
            var id = this.nextEntityId;
            for (var i = 0; i < count; ++i) {
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

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Entity Alloc() {

            var id = -1;
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
            if (e.generation == 0) {
                e = new Entity(id, 1);
            }

            this.versions.Reset(id);

            this.OnAlloc(e.id);

            return ref e;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Dealloc(in Entity entity) {

            if (this.IsAlive(entity.id, entity.generation) == false) {
                return false;
            }

            //using (NoStackTrace.All) UnityEngine.Debug.Log("Dealloc: " + entity + ", tick: " + Worlds.current.GetCurrentTick());
            this.deadPrepared.Add(entity.id);

            return true;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void ApplyDead() {

            var cnt = this.deadPrepared.Count;
            if (cnt > 0) {

                for (var i = 0; i < cnt; ++i) {

                    var id = this.deadPrepared[i];
                    --this.aliveCount;
                    this.dead.Add(id);
                    this.alive.Remove(id);
                    this.OnDealloc(id);

                }

                this.deadPrepared.Clear();

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void OnAlloc(int entityId) {

            ref var arch = ref this.allArchetypes[this.root];
            this.AddEntityToArch(ref arch, entityId);
            this.index.Add((ulong)entityId << 32, this.root);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void OnDealloc(int entityId) {

            // Remove from archetype
            var key = (ulong)entityId << 32;
            var archIdx = this.index.GetValueAndRemove(key);
            ref var arch = ref this.allArchetypes[archIdx];
            this.RemoveEntityFromArch(ref arch, entityId);

        }
        #endregion

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public FilterData GetFilter(int id) {

            return this.filters[id - 1];

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
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

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
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

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Has<T>(in Entity entity) where T : struct {

            var key = (ulong)entity.id << 32;
            var archIdx = this.index[key];
            var arch = this.allArchetypes[archIdx];
            return arch.Has(ComponentTypes<T>.typeId);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(in EntitiesGroup group, int componentId) {

            for (var i = group.fromId; i <= group.toId; ++i) {

                this.Set(this.GetEntityById(i), componentId);

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove(in EntitiesGroup group, int componentId) {

            for (var i = group.fromId; i <= group.toId; ++i) {

                this.Remove(this.GetEntityById(i), componentId);

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate<T>(in Entity entity, bool makeRequest) where T : struct {

            this.Validate(in entity, ComponentTypes<T>.typeId, makeRequest);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(in Entity entity, int componentId, bool makeRequest) {

            if (makeRequest == true) {

                // Add request and apply set on next UpdateFilters call
                this.AddValidateRequest(in entity, componentId);

            } else {

                if (ComponentTypesLambda.itemsSet.TryGetValue(componentId, out var lambda) == true) {
                    lambda.Invoke(entity);
                }

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(in Entity entity, int componentId) {

            if (ComponentTypesLambda.itemsSet.TryGetValue(componentId, out var lambda) == true) {
                lambda.Invoke(entity);
            }

            if (this.forEachMode > 0) {

                // Add request
                this.AddSetRequest(in entity, componentId);
                return;

            }

            var key = (ulong)entity.id << 32;
            ref var archIdx = ref this.index.GetValue(key);
            ref var arch = ref this.allArchetypes[archIdx];
            archIdx = arch.Set(ref this, entity, componentId).index;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set<T>(in Entity entity) where T : struct {

            this.Set(in entity, ComponentTypes<T>.typeId);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove(in Entity entity) { }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove(in Entity entity, int componentId) {

            if (ComponentTypesLambda.itemsRemove.TryGetValue(componentId, out var lambda) == true) {
                lambda.Invoke(entity);
            }

            if (this.forEachMode > 0) {

                // Add request
                this.AddRemoveRequest(in entity, componentId);
                return;

            }

            var key = (ulong)entity.id << 32;
            ref var archIdx = ref this.index.GetValue(key);
            ref var arch = ref this.allArchetypes[archIdx];
            archIdx = arch.Remove(ref this, entity, componentId).index;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove<T>(in Entity entity) where T : struct {

            this.Remove(in entity, ComponentTypes<T>.typeId);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void ApplyAllRequests() {

            foreach (var req in this.requests) {

                if (req.entity.IsAlive() == false) {
                    continue;
                }

                if (req.op == 1) {

                    this.Set(in req.entity, req.componentId);

                } else if (req.op == 2) {

                    this.Remove(in req.entity, req.componentId);

                } else if (req.op == 3) {

                    this.Validate(in req.entity, req.componentId, false);

                }

            }

            this.requests.Clear();

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void AddValidateRequest(in Entity entity, int componentId) {

            this.requests.Add(new Request() {
                entity = entity,
                componentId = componentId,
                op = 3,
            });

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void AddSetRequest(in Entity entity, int componentId) {

            this.requests.Add(new Request() {
                entity = entity,
                componentId = componentId,
                op = 1,
            });

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void AddRemoveRequest(in Entity entity, int componentId) {

            this.requests.Add(new Request() {
                entity = entity,
                componentId = componentId,
                op = 2,
            });

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public int Count(Filter filter) {

            return this.Count(this.GetFilter(filter.id));

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public int Count(FilterData filter) {

            if (this.forEachMode == 0) {
                this.UpdateFilters();
            }

            var count = 0;
            foreach (var archId in filter.archetypes) {

                var arch = this.allArchetypes[archId];
                count += arch.entitiesArr.Count;

            }

            return count;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void UpdateFilters() {

            if (this.forEachMode > 0) {
                return;
            }

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
                            this.CheckStaticShared(item.data.containsShared, item.data.notContainsShared) == true &&
                            this.CheckLambdas(in arch, item.data.lambdas) == true) {

                            item.archetypes.Add(i);

                        }

                    }

                }

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private bool CheckLambdas(in Archetype arch, List<int> lambdas) {

            return arch.HasAll(lambdas);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private bool CheckStaticShared(List<int> containsShared, List<int> notContainsShared) {

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
        public bool TryGetFilter(FilterBuilder filterBuilder, out FilterData filterData) {

            filterData = default;

            for (int i = 0, cnt = this.filters.Count; i < cnt; ++i) {

                var filter = this.filters[i];
                if (this.IsEquals(filter.data.contains, filterBuilder.data.contains) == true &&
                    this.IsEquals(filter.data.notContains, filterBuilder.data.notContains) == true &&
                    this.IsEquals(filter.data.notContainsShared, filterBuilder.data.notContainsShared) == true &&
                    this.IsEquals(filter.data.containsShared, filterBuilder.data.containsShared) == true &&
                    this.IsEquals(filter.data.lambdas, filterBuilder.data.lambdas) == true) {

                    filterData = filter;
                    return true;

                }

            }

            return false;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private bool IsEquals(List<int> list1, List<int> list2) {

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
        public bool WillNew() {

            return this.dead.Count == 0;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public ListCopyable<int> GetAlive() {

            return this.alive;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Entity IncrementGeneration(in Entity entity) {

            // Make this entity not alive, but not completely destroyed at this time
            this.cache[entity.id] = new Entity(entity.id, unchecked((ushort)(entity.generation + 1)));
            return ref this.cache[entity.id];

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetFreeze(bool freeze) { }

    }

}
#endif