#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ME.ECS.Collections;
using Unity.IL2CPP.CompilerServices;
using Il2Cpp = Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute;

#if !FILTERS_STORAGE_LEGACY
namespace ME.ECS {

    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(ME.ECS.Debug.FilterProxyDebugger))]
    public struct Filter {

        public static Filter Empty = new Filter();

        public struct Enumerator : IEnumerator<Entity> {

            public bool isCreated;
            public FilterStaticData filterStaticData;
            public FilterData filterData;
            public int index;
            public int maxIndex;
            public int archIndex;
            public ListCopyable<int> arr;
            private Entity current;
            public ListCopyable<FiltersArchetype.FiltersArchetypeStorage.Archetype> allArchetypes;
            public List<int> archetypes;
            public bool enableGroupByEntityId;

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public bool MoveNext() {
                
                if (this.isCreated == false) return false;

                var onChanged = this.filterStaticData.data.onChanged;
                var changedTracked = onChanged.Count;
                
                var connectedFilters = this.filterStaticData.data.connectedFilters;
                var connectedTracked = connectedFilters.Count;

                var currentState = Worlds.current.currentState;
                
                while (true) {
                    
                    if (this.archIndex >= this.archetypes.Count) {
                        return false;
                    }

                    if (this.maxIndex >= 0 && this.index >= this.maxIndex) return false;
                    
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

                    var entityId = this.arr[this.index];
                    if (this.filterStaticData.data.withinType == WithinType.GroupByEntityId && this.enableGroupByEntityId == true) {

                        if (entityId % this.filterStaticData.data.withinTicks != currentState.tick % this.filterStaticData.data.withinTicks) continue;

                    }

                    if (currentState.storage.IsDeadPrepared(entityId) == true) continue;
                    this.current = this.filterData.storage.GetEntityById(entityId);

                    if (connectedTracked > 0) {
                        // Check if all custom filters contains connected entity
                        var found = true;
                        for (int i = 0, cnt = connectedTracked; i < cnt; ++i) {
                            var connectedFilter = connectedFilters[i];
                            if (connectedFilter.filter.Contains(connectedFilter.get.Invoke(this.current)) == false) {
                                found = false;
                                break;
                            }
                        }

                        if (found == false) {
                            continue;
                        }
                    }

                    if (changedTracked > 0) {
                        // Check if any component has changed on this entity
                        var hasChanged = false;
                        for (int i = 0, cnt = changedTracked; i < cnt; ++i) {
                            var typeId = onChanged[i];
                            var reg = currentState.structComponents.list.arr[typeId];
                            if (reg.HasChanged(entityId) == true) {
                                hasChanged = true;
                                break;
                            }
                        }

                        if (hasChanged == false) {
                            continue;
                        }
                    }
                    
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

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public Enumerator GetEnumerator() {

            var world = Worlds.current;
            ref var filters = ref world.currentState.filters;
            filters.UpdateFilters();
            ++filters.forEachMode;

            var filterStaticData = world.GetFilterStaticData(this.id);
            if (FiltersArchetype.FiltersArchetypeStorage.CheckStaticShared(filterStaticData.data.containsShared, filterStaticData.data.notContainsShared) == false) {
                return new Enumerator();
            }

            var filterData = filters.GetFilter(this.id);
            var range = this.GetRange(world, in filterStaticData, out bool enableGroupByEntityId);
            return new Enumerator() {
                isCreated = true,
                index = range.GetFrom(),
                maxIndex = range.GetTo(),
                archIndex = 0,
                filterData = filterData,
                filterStaticData = filterStaticData,
                arr = filterData.archetypes.Count > 0 ? filterData.storage.allArchetypes[filterData.archetypesList[0]].entitiesArr : default,
                archetypes = filterData.archetypesList,
                allArchetypes = filterData.storage.allArchetypes,
                enableGroupByEntityId = enableGroupByEntityId,
            };

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

                var type = component.GetType();
                WorldUtilities.SetComponentTypeIdByType(type);
                if (ComponentTypesRegistry.typeId.TryGetValue(type, out var index) == true) {

                    dataInternal.contains.Add(index);

                }

            }

            foreach (var component in data.without) {

                var type = component.GetType();
                WorldUtilities.SetComponentTypeIdByType(type);
                if (ComponentTypesRegistry.typeId.TryGetValue(type, out var index) == true) {

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
        public static FilterBuilder CreateFromData(FilterDataTypesOptional data) {

            var dataInternal = FilterInternalData.Create();

            foreach (var component in data.with) {

                var type = component.data.GetType();
                WorldUtilities.SetComponentTypeIdByType(type);
                if (ComponentTypesRegistry.typeId.TryGetValue(type, out var index) == true) {

                    dataInternal.contains.Add(index);
                    if (component.optional == true) Filter.CreateFromDataLambda(ref dataInternal, index, type, component.data, new UnsafeDataCheckLambdaInclude());

                }

            }

            foreach (var component in data.without) {

                var type = component.data.GetType();
                WorldUtilities.SetComponentTypeIdByType(type);
                if (ComponentTypesRegistry.typeId.TryGetValue(type, out var index) == true) {

                    if (component.optional == true) {
                        Filter.CreateFromDataLambda(ref dataInternal, index, type, component.data, new UnsafeDataCheckLambdaExclude());
                    } else {
                        dataInternal.notContains.Add(index);
                    }

                }

            }
            
            return new FilterBuilder() {
                data = dataInternal,
            };

        }

        private interface IEqualsChecker {

            bool Execute(UnsafeData component, UnsafeData data);

        }
        
        private struct UnsafeDataCheckLambdaInclude : IEqualsChecker {

            public bool Execute(UnsafeData component, UnsafeData data) {

                return data.Equals(component);

            }

        }

        private struct UnsafeDataCheckLambdaExclude : IEqualsChecker {

            public bool Execute(UnsafeData component, UnsafeData data) {

                return data.Equals(component) == false;

            }

        }

        private static void CreateFromDataLambda<T>(ref FilterInternalData data, int typeId, System.Type type, IComponentBase component, T equalsChecker) where T : struct, IEqualsChecker {

            ComponentTypesRegistry.allTypeId.TryGetValue(type, out var globalTypeId);

            var lambdaTypeId = WorldUtilities.SetComponentTypeIdByType(typeof(T));
            
            var obj = new UnsafeData();
            var setMethod = UnsafeData.setMethodInfo.MakeGenericMethod(type);
            var unsafeData = (UnsafeData)setMethod.Invoke(obj, new object[] { component });
            
            System.Action<Entity> setAction = (e) => {
                if (new T().Execute(Worlds.current.ReadDataUnsafe(e, globalTypeId), unsafeData) == true) {
                    Worlds.current.currentState.filters.Set(e, lambdaTypeId, true);
                } else {
                    Worlds.current.currentState.filters.Remove(e, lambdaTypeId, true);
                }
            };
            System.Action<Entity> removeAction = (e) => {
                Worlds.current.currentState.filters.Remove(e, lambdaTypeId, true);
            };
            
            WorldUtilities.SetComponentFilterLambdaByType(type);

            var key = typeId;
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

            data.lambdas.Add(lambdaTypeId);

        }

        public struct FilterRange {

            public int from;
            public int to;

            public FilterRange(int from, int to) {

                this.from = from;
                this.to = to;

            }

            public int GetFrom() {

                if (this.from < 0) return -1;
                return this.from - 1;

            }

            public int GetTo() {
                
                if (this.to < 0) return -1;
                return this.to;

            }

        }

        private FilterRange GetRange(World world, in FilterStaticData data, out bool enableGroupByEntityId) {

            enableGroupByEntityId = false;
            if (data.data.withinTicks > 0 && data.data.withinType == WithinType.GroupByChunk) {

                var count = this.Count;
                if (data.data.withinMinChunkSize <= count) {

                    return new FilterRange(-1, -1);

                }

                var currentTick = world.GetCurrentTick();
                var withinTicks = data.data.withinTicks;
                var target = currentTick % withinTicks;
                #if FIXED_POINT_MATH
                var range = (Tick)ME.ECS.Mathematics.math.ceil((sfloat)count / (sfloat)withinTicks);
                #else
                var range = (Tick)Unity.Mathematics.math.ceil(count / (float)withinTicks);
                #endif
                var from = target * range;
                var to = (target + 1) * range;
                
                return new FilterRange(from, to);

            } else if (data.data.withinType == WithinType.GroupByEntityId) {

                if (data.data.withinMinChunkSize > 1) {

                    var count = this.Count;
                    if (data.data.withinMinChunkSize > count) {
                        enableGroupByEntityId = true;
                    }
                    
                } else {
                    
                    enableGroupByEntityId = true;

                }

            }

            return new FilterRange(-1, -1);

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

        /// <summary>
        /// Fast copy raw data from filter 
        /// </summary>
        /// <param name="allocator"></param>
        /// <returns></returns>
        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public Unity.Collections.NativeArray<Entity> ToArray(Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) {

            var filterData = Worlds.current.currentState.filters.GetFilter(this.id);
            var result = new Unity.Collections.NativeList<Entity>(filterData.archetypes.Count * 10, allocator);
            foreach (var entity in this) {
                result.Add(entity);
            }

            if (result.Length == 0) {
                result.Dispose();
                return new Unity.Collections.NativeArray<Entity>(0, allocator);
            }
            var res = new Unity.Collections.NativeArray<Entity>(result.AsArray(), allocator);
            result.Dispose();
            return res;

        }
        
        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public Unity.Collections.NativeArray<Entity> ToArray(Unity.Collections.Allocator allocator, out int min, out int max) {

            min = int.MaxValue;
            max = int.MinValue;
            var filterData = Worlds.current.currentState.filters.GetFilter(this.id);
            var result = new Unity.Collections.NativeList<Entity>(filterData.archetypes.Count * 10, allocator);
            foreach (var entity in this) {
                if (entity.id < min) {
                    min = entity.id;
                }

                if (entity.id > max) {
                    max = entity.id;
                }
                result.Add(entity);
            }
            
            if (result.Length == 0) {
                result.Dispose();
                return new Unity.Collections.NativeArray<Entity>(0, allocator);
            }
            var res = new Unity.Collections.NativeArray<Entity>(result.AsArray(), allocator);
            result.Dispose();
            return res;

        }
        
        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public Unity.Collections.NativeList<int> ToList(Unity.Collections.Allocator allocator, out Unity.Collections.NativeArray<int> idToIndex) {

            var filterData = Worlds.current.currentState.filters.GetFilter(this.id);
            var result = new Unity.Collections.NativeList<int>(filterData.archetypes.Count * 10, allocator);
            var max = -1;
            foreach (var entity in this) {
                result.Add(entity.id);
                if (entity.id > max) max = entity.id;
            }

            idToIndex = new Unity.Collections.NativeArray<int>(max + 1, allocator);
            for (int i = 0; i < result.Length; ++i) {
                
                idToIndex[result[i]] = i;
                
            }

            return result;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public Unity.Collections.NativeList<int> ToList(Unity.Collections.Allocator allocator, out int min, out int max) {

            min = int.MaxValue;
            max = int.MinValue;
            var filterData = Worlds.current.currentState.filters.GetFilter(this.id);
            var result = new Unity.Collections.NativeList<int>(filterData.archetypes.Count * 10, allocator);
            foreach (var entity in this) {
                if (entity.id < min) {
                    min = entity.id;
                }

                if (entity.id > max) {
                    max = entity.id;
                }
                result.Add(entity.id);
            }

            return result;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public Unity.Collections.NativeList<int> ToList(Unity.Collections.Allocator allocator) {

            var filterData = Worlds.current.currentState.filters.GetFilter(this.id);
            var result = new Unity.Collections.NativeList<int>(filterData.archetypes.Count * 10, allocator);
            foreach (var entity in this) {
                result.Add(entity.id);
            }

            return result;

        }

        public bool Contains(in Entity entity) {

            var filterData = Worlds.current.currentState.filters.GetFilter(this.id);
            return filterData.Contains(in entity);

        }

        public bool Contains(int entityId) {

            var filterData = Worlds.current.currentState.filters.GetFilter(this.id);
            return filterData.Contains(entityId);

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

    public struct FilterStaticData {

        public bool isCreated;
        internal FilterInternalData data;

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
            public void Copy(in FilterData @from, ref FilterData to) {

                to.CopyFrom(from);

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Recycle(ref FilterData item) {

                item.Recycle();
                item = default;

            }

        }

        public ref ME.ECS.FiltersArchetype.FiltersArchetypeStorage storage {
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref Worlds.current.currentState.filters;
        }

        [ME.ECS.Serializer.SerializeField]
        public int id;
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
            foreach (var c in Worlds.current.GetFilterStaticData(this.id).data.contains) {
                var type = string.Empty;
                foreach (var t in ComponentTypesRegistry.typeId) {
                    if (t.Value == c) {
                        type = t.Key.Name;
                        break;
                    }
                }

                str += $"W<{type}>";
            }

            foreach (var c in Worlds.current.GetFilterStaticData(this.id).data.notContains) {
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
                Worlds.current.GetFilterStaticData(this.id).data.name,
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

            return this.Contains(entity.id);

        }
        
        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Contains(int entityId) {

            foreach (var archId in this.archetypes) {

                var arch = this.storage.allArchetypes[archId];
                if (arch.entitiesContains.Contains(entityId) == true) {
                    return true;
                }

            }

            return false;

        }

    }

    public enum WithinType {

        GroupByChunk,
        GroupByEntityId,

    }

    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
    internal struct FilterInternalData {

        public struct Pair2 : System.IEquatable<Pair2> {

            public int t1;
            public int t2;

            public bool Equals(Pair2 other) {
                return this.t1 == other.t1 && this.t2 == other.t2;
            }

            public override bool Equals(object obj) {
                return obj is Pair2 other && Equals(other);
            }

            public override int GetHashCode() {
                unchecked {
                    return (this.t1 * 397) ^ this.t2;
                }
            }

        }

        public struct Pair3 : System.IEquatable<Pair3> {

            public int t1;
            public int t2;
            public int t3;

            public bool Equals(Pair3 other) {
                return this.t1 == other.t1 && this.t2 == other.t2 && this.t3 == other.t3;
            }

            public override bool Equals(object obj) {
                return obj is Pair3 other && Equals(other);
            }

            public override int GetHashCode() {
                unchecked {
                    var hashCode = this.t1;
                    hashCode = (hashCode * 397) ^ this.t2;
                    hashCode = (hashCode * 397) ^ this.t3;
                    return hashCode;
                }
            }

        }

        public struct Pair4 : System.IEquatable<Pair4> {

            public int t1;
            public int t2;
            public int t3;
            public int t4;

            public bool Equals(Pair4 other) {
                return this.t1 == other.t1 && this.t2 == other.t2 && this.t3 == other.t3 && this.t4 == other.t4;
            }

            public override bool Equals(object obj) {
                return obj is Pair4 other && Equals(other);
            }

            public override int GetHashCode() {
                unchecked {
                    var hashCode = this.t1;
                    hashCode = (hashCode * 397) ^ this.t2;
                    hashCode = (hashCode * 397) ^ this.t3;
                    hashCode = (hashCode * 397) ^ this.t4;
                    return hashCode;
                }
            }

        }

        [ME.ECS.Serializer.SerializeField]
        internal string name;

        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<Pair2> anyPair2;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<Pair3> anyPair3;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<Pair4> anyPair4;

        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> contains;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> notContains;

        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> containsShared;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> notContainsShared;

        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> onChanged;
        
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> lambdas;

        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<ConnectInfo> connectedFilters;

        public Tick withinTicks;
        public WithinType withinType;
        public int withinMinChunkSize;
        
        public void CopyFrom(FilterInternalData other) {

            this.name = other.name;

            ArrayUtils.Copy(other.anyPair2, ref this.anyPair2);
            ArrayUtils.Copy(other.anyPair3, ref this.anyPair3);
            ArrayUtils.Copy(other.anyPair4, ref this.anyPair4);
            ArrayUtils.Copy(other.contains, ref this.contains);
            ArrayUtils.Copy(other.notContains, ref this.notContains);
            ArrayUtils.Copy(other.containsShared, ref this.containsShared);
            ArrayUtils.Copy(other.notContainsShared, ref this.notContainsShared);
            ArrayUtils.Copy(other.onChanged, ref this.onChanged);
            ArrayUtils.Copy(other.lambdas, ref this.lambdas);
            ArrayUtils.Copy(other.connectedFilters, ref this.connectedFilters);
            this.withinTicks = other.withinTicks;
            this.withinType = other.withinType;
            this.withinMinChunkSize = other.withinMinChunkSize;

        }

        public void Recycle() {

            this.name = default;

            PoolListCopyable<Pair2>.Recycle(ref this.anyPair2);
            PoolListCopyable<Pair3>.Recycle(ref this.anyPair3);
            PoolListCopyable<Pair4>.Recycle(ref this.anyPair4);
            PoolListCopyable<int>.Recycle(ref this.contains);
            PoolListCopyable<int>.Recycle(ref this.notContains);
            PoolListCopyable<int>.Recycle(ref this.containsShared);
            PoolListCopyable<int>.Recycle(ref this.notContainsShared);
            PoolListCopyable<int>.Recycle(ref this.onChanged);
            PoolListCopyable<int>.Recycle(ref this.lambdas);
            PoolListCopyable<ConnectInfo>.Recycle(ref this.connectedFilters);
            this.withinTicks = default;
            this.withinType = default;
            this.withinMinChunkSize = default;

        }

        public static FilterInternalData Create() {

            return new FilterInternalData() {
                name = string.Empty,
                anyPair2 = PoolListCopyable<Pair2>.Spawn(4),
                anyPair3 = PoolListCopyable<Pair3>.Spawn(4),
                anyPair4 = PoolListCopyable<Pair4>.Spawn(4),
                contains = PoolListCopyable<int>.Spawn(4),
                notContains = PoolListCopyable<int>.Spawn(4),
                containsShared = PoolListCopyable<int>.Spawn(4),
                notContainsShared = PoolListCopyable<int>.Spawn(4),
                onChanged = PoolListCopyable<int>.Spawn(4),
                lambdas = PoolListCopyable<int>.Spawn(4),
                connectedFilters = PoolListCopyable<ConnectInfo>.Spawn(0),
                withinTicks = Tick.Zero,
                withinType = WithinType.GroupByChunk,
                withinMinChunkSize = 1,
            };

        }

    }

    public interface ILambda<T> where T : struct {

        bool Execute(in T data);

    }

    public interface IFilterConnect {

        Entity entity { get; }

    }

    public struct ConnectInfo {

        public FilterBuilder.ConnectCustomGetEntityDelegate get;
        public Filter filter;

    }

    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
    public struct FilterBuilder {

        public delegate void InnerFilterBuilderDelegate(FilterBuilder builder);
        public delegate Entity ConnectCustomGetEntityDelegate(in Entity entity);
        
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

            ComponentTypes<TComponent>.isFilterLambda = true;
            ComponentTypes<TComponent>.burstIsFilterLambda.Data = 1;

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

        public FilterBuilder Parent(InnerFilterBuilderDelegate parentFilter) {

            this.Connect<ME.ECS.Transform.Container>(parentFilter);
            return this;

        }

        public FilterBuilder Connect(InnerFilterBuilderDelegate innerFilter, ConnectCustomGetEntityDelegate customGetEntity) {

            var filterBuilder = Filter.Create(this.data.name);
            innerFilter.Invoke(filterBuilder);
            var filter = filterBuilder.Push();
            this.data.connectedFilters.Add(new ConnectInfo() {
                get = customGetEntity,
                filter = filter,
            });
            return this;

        }

        public FilterBuilder Connect<TConnect>(InnerFilterBuilderDelegate innerFilter) where TConnect : struct, IStructComponent, IFilterConnect {

            this.With<TConnect>();
            var filterBuilder = Filter.Create(this.data.name);
            innerFilter.Invoke(filterBuilder);
            var filter = filterBuilder.Push();
            this.data.connectedFilters.Add(new ConnectInfo() {
                get = (in Entity e) => e.Read<TConnect>().entity,
                filter = filter,
            });
            return this;

        }

        public FilterBuilder OnChanged<T>(bool addWith = true) where T : struct, IVersioned {

            if (addWith == true) this.With<T>();
            WorldUtilities.SetComponentTypeId<T>();
            if (this.data.onChanged.Contains(ComponentTypes<T>.typeId) == false) this.data.onChanged.Add(AllComponentTypes<T>.typeId);
            return this;

        }

        public FilterBuilder WithoutShared<T>() where T : struct {

            if (this.data.notContainsShared.Contains(AllComponentTypes<T>.typeId) == false) this.data.notContainsShared.Add(AllComponentTypes<T>.typeId);
            return this;

        }

        public FilterBuilder WithShared<T>() where T : struct {

            if (this.data.containsShared.Contains(AllComponentTypes<T>.typeId) == false) this.data.containsShared.Add(AllComponentTypes<T>.typeId);
            return this;

        }

        public FilterBuilder With<T>() where T : struct {

            WorldUtilities.SetComponentTypeId<T>();
            if (this.data.contains.Contains(ComponentTypes<T>.typeId) == false) this.data.contains.Add(ComponentTypes<T>.typeId);
            return this;

        }

        public FilterBuilder Without<T>() where T : struct {

            WorldUtilities.SetComponentTypeId<T>();
            if (this.data.notContains.Contains(ComponentTypes<T>.typeId) == false) this.data.notContains.Add(ComponentTypes<T>.typeId);
            return this;

        }

        /// <summary>
        /// Filter will automatically range results and run partial enumeration depends on current tick
        /// </summary>
        /// <param name="ticks">How many ticks should passed to enumerate whole filter</param>
        /// <param name="groupBy">Algorithm to filter entities</param>
        /// <param name="minChunkSize">Minimum length of the filter to begin chunk filtering</param>
        /// <returns></returns>
        public FilterBuilder WithinTicks(Tick ticks, WithinType groupBy = WithinType.GroupByChunk, int minChunkSize = 1) {

            this.data.withinTicks = ticks;
            this.data.withinType = groupBy;
            this.data.withinMinChunkSize = minChunkSize;
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
            throw new System.NotImplementedException("OnVersionChangedOnly can't be used with !FILTERS_STORAGE_LEGACY.");
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
            Worlds.current.SetFilterStaticData(nextId, this.data);
            filterData = new FilterData() {
                id = nextId,
                isAlive = true,
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