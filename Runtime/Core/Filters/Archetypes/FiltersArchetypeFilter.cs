//#define FILTERS_STORAGE_ARCHETYPES
#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ME.ECS.Collections;
using Unity.IL2CPP.CompilerServices;
using Il2Cpp = Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute;

#if FILTERS_STORAGE_ARCHETYPES
namespace ME.ECS {

    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(ME.ECS.Debug.FilterProxyDebugger))]
    public struct Filter {

        public static Filter Empty = new Filter();

        public struct Enumerator : IEnumerator<Entity> {

            public FilterData filterData;
            public int index;
            public int archIndex;
            public ListCopyable<int> arr;
            private Entity current;
            public ListCopyable<FiltersArchetype.FiltersArchetypeStorage.Archetype> allArchetypes;
            public List<int> archetypes;

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public bool MoveNext() {
                
                while (true) {

                    if (this.archIndex >= this.archetypes.Count) {
                        return false;
                    }

                    ++this.index;
                    ref var arch = ref this.allArchetypes[this.archetypes[this.archIndex]];
                    if (this.index >= arch.entitiesArr.Count) {

                        ++this.archIndex;
                        if (this.archIndex < this.archetypes.Count) {
                            this.arr = this.allArchetypes[this.archetypes[this.archIndex]].entitiesArr;
                        }

                        this.index = -1;
                        continue;

                    }

                    this.current = this.filterData.storage.GetEntityById(this.arr[this.index]);

                    return true;

                }
                
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

        public int Count {
            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            get => Worlds.current.currentState.filters.Count(this);
        }

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
            filters.UpdateFilters();
            ++filters.forEachMode;
            var filterData = filters.GetFilter(this.id);
            return new Enumerator() {
                index = -1,
                archIndex = 0,
                filterData = filterData,
                arr = filterData.archetypes.Count > 0 ? filterData.storage.allArchetypes[filterData.archetypesList[0]].entitiesArr : default,
                archetypes = filterData.archetypesList,
                allArchetypes = filterData.storage.allArchetypes,
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

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
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
        
        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public Unity.Collections.NativeArray<Entity> ToArray(Unity.Collections.Allocator allocator, out int min, out int max) {

            min = int.MaxValue;
            max = int.MinValue;
            var filterData = Worlds.current.currentState.filters.GetFilter(this.id);
            var result = new Unity.Collections.NativeArray<Entity>(filterData.storage.Count(filterData), allocator);
            var k = 0;
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

    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
    public struct FilterData {

        [Il2Cpp(Option.NullChecks, false)]
        [Il2Cpp(Option.ArrayBoundsChecks, false)]
        [Il2Cpp(Option.DivideByZeroChecks, false)]
        public struct CopyData : IArrayElementCopy<FilterData> {

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Copy(FilterData @from, ref FilterData to) {

                to.CopyFrom(from);

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Recycle(FilterData item) {

                item.Recycle();

            }

        }

        public ref ME.ECS.FiltersArchetype.FiltersArchetypeStorage storage {
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref Worlds.current.currentState.filters;
        }

        [ME.ECS.Serializer.SerializeField]
        public int id;
        [ME.ECS.Serializer.SerializeField]
        internal FilterInternalData data;
        [ME.ECS.Serializer.SerializeField]
        internal HashSetCopyable<int> archetypes;
        [ME.ECS.Serializer.SerializeField]
        internal List<int> archetypesList;
        [ME.ECS.Serializer.SerializeField]
        public bool isAlive;

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void CopyFrom(FilterData other) {

            this.id = other.id;
            this.data.CopyFrom(other.data);
            ArrayUtils.Copy(other.archetypes, ref this.archetypes);
            ArrayUtils.Copy(other.archetypesList, ref this.archetypesList);
            this.isAlive = other.isAlive;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Recycle() {

            this.id = 0;
            this.isAlive = false;
            this.data.Recycle();
            PoolHashSetCopyable<int>.Recycle(ref this.archetypes);
            PoolList<int>.Recycle(ref this.archetypesList);

        }

        public int Count {
            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            get => this.storage.Count(this);
        }

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

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void ApplyAllRequests() {

            // Apply all requests after enumeration has ended

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
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

        [ME.ECS.Serializer.SerializeField]
        internal string name;

        [ME.ECS.Serializer.SerializeField]
        internal List<Pair2> anyPair2;
        [ME.ECS.Serializer.SerializeField]
        internal List<Pair3> anyPair3;
        [ME.ECS.Serializer.SerializeField]
        internal List<Pair4> anyPair4;

        [ME.ECS.Serializer.SerializeField]
        internal List<int> contains;
        [ME.ECS.Serializer.SerializeField]
        internal List<int> notContains;

        [ME.ECS.Serializer.SerializeField]
        internal List<int> containsShared;
        [ME.ECS.Serializer.SerializeField]
        internal List<int> notContainsShared;

        [ME.ECS.Serializer.SerializeField]
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

        [ME.ECS.Serializer.SerializeField]
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
                archetypes = PoolHashSetCopyable<int>.Spawn(),
                archetypesList = PoolList<int>.Spawn(64),
            };
            this.storage.MarkAllArchetypesAsDirty();
            this.storage.filters.Add(filterData);
            filter = new Filter() {
                id = nextId,
            };
            return filter;

        }

    }

}
#endif