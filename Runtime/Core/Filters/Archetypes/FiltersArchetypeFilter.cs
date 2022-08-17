#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using System.Collections;
using System.Runtime.CompilerServices;
using ME.ECS.Collections;
using Unity.IL2CPP.CompilerServices;
using Il2Cpp = Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute;
using BURST = Unity.Burst.BurstCompileAttribute;

namespace ME.ECS {
    
    using Collections.V3;
    using Collections.MemoryAllocator;

    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(ME.ECS.DebugUtils.FilterProxyDebugger))]
    public struct Filter {

        public static Filter Empty = new Filter();

        public struct Enumerator : System.Collections.Generic.IEnumerator<Entity> {

            public World world;
            public State state;
            private ref MemoryAllocator allocator => ref this.state.allocator;
            private ref MemoryAllocator tempAllocator => ref this.world.tempAllocator;
            
            public bool isCreated;
            public int index;
            public int maxIndex;
            public int archIndex;
            public List<int> arr;
            public List<int> archetypes;
            public int archetypesCount;
            public bool enableGroupByEntityId;

            public List<int> onChanged;
            public List<ConnectInfo> connectedFilters;
            public ListCopyable<ConnectInfoLambda> connectedLambdas;
            public WithinType withinType;
            public Tick withinTicks;
            
            private Entity current;

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public bool MoveNext() {
                
                if (this.isCreated == false) return false;

                var onChanged = this.onChanged;
                var changedTracked = onChanged.Count;
                
                var connectedFilters = this.connectedFilters;
                var connectedTracked = connectedFilters.Count;

                ref var allocator = ref this.allocator;

                while (true) {
                    
                    if (this.archIndex >= this.archetypesCount) {
                        return false;
                    }

                    if (this.maxIndex >= 0 && this.index >= this.maxIndex) return false;

                    ++this.index;
                    ref var arch = ref this.state.storage.allArchetypes[in allocator, this.archetypes[in allocator, this.archIndex]];
                    if (arch.entitiesArr.isCreated == false || this.index >= arch.entitiesArr.Count) {

                        ++this.archIndex;
                        if (this.archIndex < this.archetypesCount) {
                            this.arr = this.state.storage.allArchetypes[in allocator, this.archetypes[in allocator, this.archIndex]].entitiesArr;
                        }

                        this.index = -1;
                        continue;

                    }

                    var entityId = this.arr[in allocator, this.index];
                    if (this.withinType == WithinType.GroupByEntityId && this.enableGroupByEntityId == true) {

                        if (entityId % this.withinTicks != this.state.tick % this.withinTicks) continue;

                    }

                    if (this.state.storage.IsDeadPrepared(in allocator, entityId) == true) continue;
                    this.current = this.state.storage.cache[in allocator, entityId];

                    if (connectedTracked > 0) {
                        // Check if all custom filters contains connected entity
                        var found = true;
                        for (int i = 0, cnt = connectedTracked; i < cnt; ++i) {
                            var connectedFilter = connectedFilters[in this.tempAllocator, i];
                            var connectedLambda = this.connectedLambdas[i];
                            if (connectedFilter.filter.Contains(in allocator, connectedLambda.get.Invoke(this.current)) == false) {
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
                            var typeId = onChanged[in this.tempAllocator, i];
                            var reg = this.state.structComponents.list.arr[typeId];
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

                ref var filters = ref Worlds.current.currentState.storage;
                --filters.forEachMode;

                if (filters.forEachMode == 0) {

                    filters.UpdateFilters(Worlds.current.currentState, ref Worlds.current.currentState.allocator);

                }

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public Enumerator GetEnumerator() {

            var world = Worlds.current;
            ref var filters = ref world.currentState.storage;
            filters.UpdateFilters(world.currentState, ref world.currentState.allocator);
            ++filters.forEachMode;

            ref var filterStaticData = ref world.GetFilterStaticData(this.id);
            if (FiltersArchetype.FiltersArchetypeStorage.CheckStaticShared(filterStaticData.data.containsShared, filterStaticData.data.notContainsShared) == false) {
                return new Enumerator();
            }

            ref var filterData = ref filters.GetFilter(in world.currentState.allocator, this.id);
            var range = this.GetRange(world, in filterStaticData, out bool enableGroupByEntityId);
            return new Enumerator() {
                world = world,
                state = world.currentState,
                isCreated = true,
                index = range.GetFrom(),
                maxIndex = range.GetTo(),
                archIndex = 0,
                arr = filterData.archetypes.Count > 0 ? filterData.storage.allArchetypes[in world.currentState.allocator, filterData.archetypesList[in world.currentState.allocator, 0]].entitiesArr : default,
                archetypes = filterData.archetypesList,
                archetypesCount = filterData.archetypesList.Count,
                enableGroupByEntityId = enableGroupByEntityId,
                onChanged = filterStaticData.data.onChanged,
                connectedFilters = filterStaticData.data.connectedFilters,
                connectedLambdas = world.GetFilterStaticDataLambdas(this.id),
                withinType = filterStaticData.data.withinType,
                withinTicks = filterStaticData.data.withinTicks,
            };

        }

        public int id;

        public World world => Worlds.current;

        // ReSharper disable once InconsistentNaming
        public int Count {
            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            get => Worlds.current.currentState.storage.Count(Worlds.current.currentState, ref Worlds.current.currentState.allocator, this);
        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static FilterBuilder CreateFromData(FilterDataTypes data) {

            var dataInternal = FilterInternalData.Create(ref Worlds.current.tempAllocator);
            
            foreach (var component in data.with) {

                var type = component.GetType();
                WorldUtilities.SetComponentTypeIdByType(type);
                if (ComponentTypesRegistry.typeId.TryGetValue(type, out var index) == true) {

                    dataInternal.contains.Add(ref Worlds.current.tempAllocator, index);

                }

            }

            foreach (var component in data.without) {

                var type = component.GetType();
                WorldUtilities.SetComponentTypeIdByType(type);
                if (ComponentTypesRegistry.typeId.TryGetValue(type, out var index) == true) {

                    dataInternal.notContains.Add(ref Worlds.current.tempAllocator, index);

                }

            }
            
            return new FilterBuilder() {
                data = dataInternal,
                dataLambda = null,
            };

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static FilterBuilder CreateFromData(FilterDataTypesOptional data) {

            var dataInternal = FilterInternalData.Create(ref Worlds.current.tempAllocator);

            foreach (var component in data.with) {

                var type = component.data.GetType();
                WorldUtilities.SetComponentTypeIdByType(type);
                if (ComponentTypesRegistry.typeId.TryGetValue(type, out var index) == true) {

                    dataInternal.contains.Add(ref Worlds.current.tempAllocator, index);
                    #if !FILTERS_LAMBDA_DISABLED
                    if (component.optional == true) Filter.CreateFromDataLambda(ref dataInternal, index, type, component.data, new UnsafeDataCheckLambdaInclude());
                    #endif

                }

            }

            foreach (var component in data.without) {

                var type = component.data.GetType();
                WorldUtilities.SetComponentTypeIdByType(type);
                if (ComponentTypesRegistry.typeId.TryGetValue(type, out var index) == true) {

                    if (component.optional == true) {
                        #if !FILTERS_LAMBDA_DISABLED
                        Filter.CreateFromDataLambda(ref dataInternal, index, type, component.data, new UnsafeDataCheckLambdaExclude());
                        #endif
                    } else {
                        dataInternal.notContains.Add(ref Worlds.current.tempAllocator, index);
                    }

                }

            }
            
            return new FilterBuilder() {
                data = dataInternal,
                dataLambda = null,
            };

        }

        private interface IEqualsChecker {

            bool Execute(UnsafeData component, UnsafeData data);

        }
        
        private struct UnsafeDataCheckLambdaInclude : IEqualsChecker {

            public bool Execute(UnsafeData component, UnsafeData data) {

                return data.Equals(in Worlds.current.currentState.allocator, component);

            }

        }

        private struct UnsafeDataCheckLambdaExclude : IEqualsChecker {

            public bool Execute(UnsafeData component, UnsafeData data) {

                return data.Equals(in Worlds.current.currentState.allocator, component) == false;

            }

        }

        #if !FILTERS_LAMBDA_DISABLED
        private static void CreateFromDataLambda<T>(ref FilterInternalData data, int typeId, System.Type type, IComponentBase component, T equalsChecker) where T : struct, IEqualsChecker {

            ComponentTypesRegistry.allTypeId.TryGetValue(type, out var globalTypeId);

            var lambdaTypeId = WorldUtilities.SetComponentTypeIdByType(typeof(T));
            
            var obj = new UnsafeData();
            var setMethod = UnsafeData.setMethodInfo.MakeGenericMethod(type);
            var unsafeData = (UnsafeData)setMethod.Invoke(obj, new object[] { component });
            
            System.Action<Entity> setAction = (e) => {
                if (new T().Execute(Worlds.current.ReadDataUnsafe(e, globalTypeId), unsafeData) == true) {
                    Worlds.current.currentState.storage.Set(ref Worlds.current.currentState.allocator, e, lambdaTypeId, true);
                } else {
                    Worlds.current.currentState.storage.Remove(ref Worlds.current.currentState.allocator, e, lambdaTypeId, true);
                }
            };
            System.Action<Entity> removeAction = (e) => {
                Worlds.current.currentState.storage.Remove(ref Worlds.current.currentState.allocator, e, lambdaTypeId, true);
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

            data.lambdas.Add(ref Worlds.current.tempAllocator, lambdaTypeId);

        }
        #endif

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

            ref var allocator = ref Worlds.current.currentState.allocator;
            var filterData = Worlds.current.currentState.storage.GetFilter(in allocator, this.id);
            min = int.MaxValue;
            max = int.MinValue;
            for (int j = 0; j < filterData.archetypesList.Count; ++j) {

                var archId = filterData.archetypesList[in allocator, j];
                var arch = filterData.storage.allArchetypes[in allocator, archId];
                for (int i = 0, count = arch.entitiesArr.Count; i < count; ++i) {
                    
                    var e = arch.entitiesArr[in allocator, i];
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

            var filterData = Worlds.current.currentState.storage.GetFilter(in Worlds.current.currentState.allocator, this.id);
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
            var filterData = Worlds.current.currentState.storage.GetFilter(in Worlds.current.currentState.allocator, this.id);
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

            var filterData = Worlds.current.currentState.storage.GetFilter(in Worlds.current.currentState.allocator, this.id);
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
            var filterData = Worlds.current.currentState.storage.GetFilter(in Worlds.current.currentState.allocator, this.id);
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

            var filterData = Worlds.current.currentState.storage.GetFilter(in Worlds.current.currentState.allocator, this.id);
            var result = new Unity.Collections.NativeList<int>(filterData.archetypes.Count * 10, allocator);
            foreach (var entity in this) {
                result.Add(entity.id);
            }

            return result;

        }

        public bool Contains(in MemoryAllocator allocator, in Entity entity) {

            var filterData = Worlds.current.currentState.storage.GetFilter(in allocator, this.id);
            return filterData.Contains(in allocator, in entity);

        }

        public bool Contains(in MemoryAllocator allocator, int entityId) {

            var filterData = Worlds.current.currentState.storage.GetFilter(in allocator, this.id);
            return filterData.Contains(in allocator, entityId);

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

            var data = FilterInternalData.Create(ref Worlds.current.tempAllocator);
            if (name == null) {
                data.name = "Unnamed";
            } else {
                data.name = name;
            }
            return new FilterBuilder() {
                data = data,
                dataLambda = PoolListCopyable<ConnectInfoLambda>.Spawn(1),
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

        public ref ME.ECS.FiltersArchetype.FiltersArchetypeStorage storage {
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref Worlds.current.currentState.storage;
        }

        [ME.ECS.Serializer.SerializeField]
        public int id;
        [ME.ECS.Serializer.SerializeField]
        internal EquatableHashSet<int> archetypes;
        [ME.ECS.Serializer.SerializeField]
        internal List<int> archetypesList;
        [ME.ECS.Serializer.SerializeField]
        public bool isAlive;

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose(ref MemoryAllocator allocator) {

            this.id = 0;
            this.isAlive = false;
            this.archetypes.Dispose(ref allocator);
            this.archetypesList.Dispose(ref allocator);
            
        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public int Count(State state, ref MemoryAllocator allocator) {
            return this.storage.Count(state, ref allocator, this.id);
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
                Worlds.current.GetFilterStaticData(this.id).data.name.Value,
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
        public bool Contains(in MemoryAllocator allocator, in Entity entity) {

            return this.Contains(in allocator, entity.id);

        }
        
        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Contains(in MemoryAllocator allocator, int entityId) {

            for (int i = 0; i < this.archetypesList.Count; ++i) {

                var idx = this.archetypesList[in allocator, i];
                var arch = this.storage.allArchetypes[in allocator, idx];
                if (arch.entitiesContains.Contains(in allocator, entityId) == true) {
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
        internal Unity.Collections.FixedString128Bytes name;

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
        internal List<int> onChanged;
        
        #if !FILTERS_LAMBDA_DISABLED
        [ME.ECS.Serializer.SerializeField]
        internal List<int> lambdas;
        #endif

        [ME.ECS.Serializer.SerializeField]
        internal List<ConnectInfo> connectedFilters;

        public Tick withinTicks;
        public WithinType withinType;
        public int withinMinChunkSize;
        
        public void Recycle(ref MemoryAllocator allocator) {

            this.name = default;

            this.anyPair2.Dispose(ref allocator);
            this.anyPair3.Dispose(ref allocator);
            this.anyPair4.Dispose(ref allocator);
            this.contains.Dispose(ref allocator);
            this.notContains.Dispose(ref allocator);
            this.containsShared.Dispose(ref allocator);
            this.notContainsShared.Dispose(ref allocator);
            this.onChanged.Dispose(ref allocator);
            #if !FILTERS_LAMBDA_DISABLED
            this.lambdas.Dispose(ref allocator);
            #endif
            this.connectedFilters.Dispose(ref allocator);
            this.withinTicks = default;
            this.withinType = default;
            this.withinMinChunkSize = default;

        }

        public static FilterInternalData Create(ref MemoryAllocator allocator) {

            return new FilterInternalData() {
                name = string.Empty,
                anyPair2 = new List<Pair2>(ref allocator, 1),
                anyPair3 = new List<Pair3>(ref allocator, 1),
                anyPair4 = new List<Pair4>(ref allocator, 1),
                contains = new List<int>(ref allocator, 2),
                notContains = new List<int>(ref allocator, 1),
                containsShared = new List<int>(ref allocator, 0),
                notContainsShared = new List<int>(ref allocator, 0),
                onChanged = new List<int>(ref allocator, 0),
                #if !FILTERS_LAMBDA_DISABLED
                lambdas = new List<int>(ref allocator, 0),
                #endif
                connectedFilters = new List<ConnectInfo>(ref allocator, 0),
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

    public struct ConnectInfo : System.IEquatable<ConnectInfo> {

        public Filter filter;

        public bool Equals(ConnectInfo other) {
            return this.filter.id.Equals(other.filter.id);
        }

        public override bool Equals(object obj) {
            return obj is ConnectInfo other && this.Equals(other);
        }

        public override int GetHashCode() {
            return this.filter.id.GetHashCode();
        }

    }

    public struct ConnectInfoLambda {

        public FilterBuilder.ConnectCustomGetEntityDelegate get;
        
    }

    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
    public struct FilterBuilder {

        public delegate void InnerFilterBuilderDelegate(FilterBuilder builder);
        public delegate Entity ConnectCustomGetEntityDelegate(in Entity entity);
        
        internal ref ME.ECS.FiltersArchetype.FiltersArchetypeStorage storage => ref Worlds.current.currentState.storage;
        internal ref MemoryAllocator allocator => ref Worlds.current.tempAllocator;

        internal FilterInternalData data;
        internal ListCopyable<ConnectInfoLambda> dataLambda;

        #if !FILTERS_LAMBDA_DISABLED
        public FilterBuilder WithLambda<T, TComponent>() where T : struct, ILambda<TComponent> where TComponent : struct, IStructComponent {

            System.Action<Entity> setAction = (e) => {
                if (new T().Execute(in e.Read<TComponent>())) {
                    Worlds.current.currentState.storage.Set(ref Worlds.current.currentState.allocator, e, ComponentTypes<T>.typeId, ComponentTypes<T>.isFilterLambda);
                } else {
                    Worlds.current.currentState.storage.Remove(ref Worlds.current.currentState.allocator, e, ComponentTypes<T>.typeId, ComponentTypes<T>.isFilterLambda);
                }
            };
            System.Action<Entity> removeAction = (e) => { Worlds.current.currentState.storage.Remove<T>(ref Worlds.current.currentState.allocator, e); };

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

            this.data.lambdas.Add(ref this.allocator, ComponentTypes<T>.typeId);
            return this.With<TComponent>();

        }
        #endif

        public FilterBuilder Connect(InnerFilterBuilderDelegate innerFilter, ConnectCustomGetEntityDelegate customGetEntity) {

            var filterBuilder = Filter.Create(this.data.name.Value);
            innerFilter.Invoke(filterBuilder);
            var filter = filterBuilder.Push();
            this.data.connectedFilters.Add(ref this.allocator, new ConnectInfo() {
                filter = filter,
            });
            this.dataLambda.Add(new ConnectInfoLambda() {
                get = customGetEntity,
            });
            return this;

        }

        public FilterBuilder Connect<TConnect>(InnerFilterBuilderDelegate innerFilter) where TConnect : struct, IStructComponent, IFilterConnect {

            this.With<TConnect>();
            var filterBuilder = Filter.Create(this.data.name.Value);
            innerFilter.Invoke(filterBuilder);
            var filter = filterBuilder.Push();
            this.data.connectedFilters.Add(ref this.allocator, new ConnectInfo() {
                filter = filter,
            });
            this.dataLambda.Add(new ConnectInfoLambda() {
                get = (in Entity e) => e.Read<TConnect>().entity,
            });
            return this;

        }

        public FilterBuilder OnChanged<T>(bool addWith = true) where T : struct, IVersioned {

            if (addWith == true) this.With<T>();
            WorldUtilities.SetComponentTypeId<T>();
            if (this.data.onChanged.Contains(in this.allocator, ComponentTypes<T>.typeId) == false) this.data.onChanged.Add(ref this.allocator, AllComponentTypes<T>.typeId);
            return this;

        }

        public FilterBuilder WithoutShared<T>() where T : struct {

            if (this.data.notContainsShared.Contains(in this.allocator, AllComponentTypes<T>.typeId) == false) this.data.notContainsShared.Add(ref this.allocator, AllComponentTypes<T>.typeId);
            return this;

        }

        public FilterBuilder WithShared<T>() where T : struct {

            if (this.data.containsShared.Contains(in this.allocator, AllComponentTypes<T>.typeId) == false) this.data.containsShared.Add(ref this.allocator, AllComponentTypes<T>.typeId);
            return this;

        }

        public FilterBuilder With<T>() where T : struct {

            WorldUtilities.SetComponentTypeId<T>();
            if (this.data.contains.Contains(in this.allocator, ComponentTypes<T>.typeId) == false) this.data.contains.Add(ref this.allocator, ComponentTypes<T>.typeId);
            return this;

        }

        public FilterBuilder Without<T>() where T : struct {

            WorldUtilities.SetComponentTypeId<T>();
            if (this.data.notContains.Contains(in this.allocator, ComponentTypes<T>.typeId) == false) this.data.notContains.Add(ref this.allocator, ComponentTypes<T>.typeId);
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
            this.data.anyPair2.Add(ref this.allocator, new FilterInternalData.Pair2() {
                t1 = ComponentTypes<T1>.typeId,
                t2 = ComponentTypes<T2>.typeId,
            });
            return this;

        }

        public FilterBuilder Any<T1, T2, T3>() where T1 : struct where T2 : struct where T3 : struct {

            WorldUtilities.SetComponentTypeId<T1>();
            WorldUtilities.SetComponentTypeId<T2>();
            WorldUtilities.SetComponentTypeId<T3>();
            this.data.anyPair3.Add(ref this.allocator, new FilterInternalData.Pair3() {
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
            this.data.anyPair4.Add(ref this.allocator, new FilterInternalData.Pair4() {
                t1 = ComponentTypes<T1>.typeId,
                t2 = ComponentTypes<T2>.typeId,
                t3 = ComponentTypes<T3>.typeId,
                t4 = ComponentTypes<T4>.typeId,
            });
            return this;

        }

        public Filter Push() {

            Filter _ = default;
            return this.Push(ref _);

        }

        // ReSharper disable once RedundantAssignment
        public Filter Push(ref Filter filter) {

            if (Filter.currentInject != null) {
                Filter.currentInject.Invoke(ref this);
            }

            ref var allocator = ref Worlds.current.currentState.allocator;
            if (this.storage.TryGetFilter(in allocator, this, out var filterData) == true) {

                // Already has filter with this restrictions
                filter = new Filter() {
                    id = filterData.id,
                };
                return filter;

            }

            var nextId = this.storage.filters.Count + 1;
            Worlds.current.SetFilterStaticData(nextId, this.data, this.dataLambda);
            filterData = new FilterData() {
                id = nextId,
                isAlive = true,
                archetypes = new EquatableHashSet<int>(ref allocator, 64),
                archetypesList = new List<int>(ref allocator, 64),
            };
            this.storage.MarkAllArchetypesAsDirty(ref allocator);
            this.storage.filters.Add(ref allocator, filterData);
            filter = new Filter() {
                id = nextId,
            };
            return filter;

        }

    }

}