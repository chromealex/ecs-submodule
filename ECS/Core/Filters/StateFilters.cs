#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using ME.ECS.Collections;
    using System.Collections.Generic;
    using Unity.Jobs;

    public interface IFilterNode {

        bool Execute(Entity entity);

    }

    public interface IFilterAction {

        void Execute(in Entity entity);

    }

    public partial class World {

        [Unity.Burst.BurstCompileAttribute]
        private struct UpdateFiltersJob : IJob {

            public Entity entity;
            public ArchetypeEntities archetypeEntities;
            public NativeBufferArray<FiltersTree.FilterBurst> filters;
            [Unity.Collections.NativeDisableParallelForRestriction] public Unity.Collections.NativeList<int> result;

            public void Execute() {

                for (int i = 0; i < this.filters.Length; ++i) {

                    ref readonly var item = ref this.filters.Read(i);
                    if (item.IsForEntity(in this.archetypeEntities, in this.entity) == true) {

                        this.result.Add(item.id);

                    }

                }

            }

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void CalculateJob(in Entity entity, in NativeBufferArray<FiltersTree.FilterBurst> contains, in NativeBufferArray<FiltersTree.FilterBurst> notContains, out Unity.Collections.NativeList<int> containsResult, out Unity.Collections.NativeList<int> notContainsResult) {

            containsResult = new Unity.Collections.NativeList<int>(Unity.Collections.Allocator.Temp);
            notContainsResult = new Unity.Collections.NativeList<int>(Unity.Collections.Allocator.Temp);

            ref var archetypeEntities = ref Worlds.currentWorld.currentState.storage.archetypes;
            var jobContains = new UpdateFiltersJob() {
                entity = entity,
                archetypeEntities = archetypeEntities,
                filters = contains,
                result = containsResult,
            };
            var jobNotContains = new UpdateFiltersJob() {
                entity = entity,
                archetypeEntities = archetypeEntities,
                filters = notContains,
                result = notContainsResult,
            };

            if (System.Threading.Thread.CurrentThread == this.worldThread) {
                
                JobHandle jobContainsHandle;
                JobHandle jobNotContainsHandle;

                {
                    jobContainsHandle = jobContains.Schedule();
                }
                {
                    jobNotContainsHandle = jobNotContains.Schedule();
                }
                JobHandle.CompleteAll(ref jobContainsHandle, ref jobNotContainsHandle);

            } else {
                
                jobContains.Execute();
                jobNotContains.Execute();
                
            }
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void UpdateFilterByStructComponentVersioned<T>(in Entity entity) where T : struct, IStructComponentBase {

            this.CalculateJob(in entity,
                              this.currentState.filters.filtersTree.GetFiltersContainsForVersioned<T>(),
                              this.currentState.filters.filtersTree.GetFiltersNotContainsForVersioned<T>(),
                              out var containsResult,
                              out var notContainsResult);

            for (int i = 0, cnt = containsResult.Length; i < cnt; ++i) {
                
                var filter = this.GetFilter(containsResult[i]);
                filter.OnUpdate(in entity);

            }

            for (int i = 0, cnt = notContainsResult.Length; i < cnt; ++i) {
                
                var filter = this.GetFilter(notContainsResult[i]);
                filter.OnUpdate(in entity);

            }

            containsResult.Dispose();
            notContainsResult.Dispose();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void UpdateFilterByStructComponent<T>(in Entity entity) where T : struct, IStructComponentBase {

            this.CalculateJob(in entity,
                              this.currentState.filters.filtersTree.GetFiltersContainsFor<T>(),
                              this.currentState.filters.filtersTree.GetFiltersNotContainsFor<T>(),
                                out var containsResult,
                                out var notContainsResult);

            for (int i = 0, cnt = containsResult.Length; i < cnt; ++i) {
                
                var filter = this.GetFilter(containsResult[i]);
                filter.OnUpdate(in entity);

            }

            for (int i = 0, cnt = notContainsResult.Length; i < cnt; ++i) {
                
                var filter = this.GetFilter(notContainsResult[i]);
                filter.OnUpdate(in entity);

            }

            containsResult.Dispose();
            notContainsResult.Dispose();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void UpdateFilterByStructComponent(in Entity entity, int componentIndex) {
            
            this.CalculateJob(in entity,
                              this.currentState.filters.filtersTree.GetFiltersContainsFor(componentIndex),
                              this.currentState.filters.filtersTree.GetFiltersNotContainsFor(componentIndex),
                                out var containsResult,
                                out var notContainsResult);

            for (int i = 0, cnt = containsResult.Length; i < cnt; ++i) {
                
                var filter = this.GetFilter(containsResult[i]);
                filter.OnUpdate(in entity);

            }

            for (int i = 0, cnt = notContainsResult.Length; i < cnt; ++i) {
                
                var filter = this.GetFilter(notContainsResult[i]);
                filter.OnUpdate(in entity);

            }

            containsResult.Dispose();
            notContainsResult.Dispose();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void AddComponentToFilter(in Entity entity) {

            ref var dic = ref FiltersDirectCache.dic.arr[this.id];
            if (dic.arr != null) {

                for (int i = 0; i < dic.Length; ++i) {

                    if (dic.arr[i] == false) continue;
                    var filterId = i + 1;
                    var filter = this.GetFilter(filterId);
                    if (filter.IsForEntity(entity.id) == false) continue;
                    filter.OnAddComponent(in entity);

                }

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveComponentFromFilter(in Entity entity) {

            ref var dic = ref FiltersDirectCache.dic.arr[this.id];
            if (dic.arr != null) {

                for (int i = 0; i < dic.Length; ++i) {

                    if (dic.arr[i] == false) continue;
                    var filterId = i + 1;
                    var filter = this.GetFilter(filterId);
                    if (filter.IsForEntity(entity.id) == false) continue;
                    filter.OnRemoveComponent(in entity);

                }

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal void RemoveFromAllFilters(in Entity entity) {

            var visited = PoolHashSet<int>.Spawn(10);
            ref readonly var bits = ref this.currentState.storage.archetypes.types.Read(entity.id);
            var bitsCount = bits.BitsCount;
            for (int i = 0; i < bitsCount; ++i) {

                if (bits.HasBit(i) == true) {
                    
                    var filters = this.currentState.filters.filtersTree.GetFiltersContainsFor(i);
                    for (int f = 0; f < filters.Length; ++f) {

                        ref readonly var filterId = ref filters.Read(f);
                        if (visited.Contains(filterId.id) == true) continue;
                        visited.Add(filterId.id);
                        var filter = this.GetFilter(filterId.id);
                        filter.OnEntityDestroy(in entity);
                        FilterDataStatic.OnRemoveEntity(ref filter.data, in entity);

                    }
                    
                }
                
            }
            PoolHashSet<int>.Recycle(ref visited);
            
        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class FiltersStorage : IPoolableRecycle {

        internal static Archetype allFiltersArchetype;
        
        internal FiltersTree filtersTree;
        internal BufferArray<FilterData> filters;
        private int nextId;

        public int Count {
            get {
                return this.filters.Length;
            }
        }

        public void Clear() {

            for (int i = 0; i < this.filters.Length; ++i) {

                if (this.filters[i] == null) continue;
                this.filters[i].Clear();

            }

        }

        public void OnDeserialize(int lastEntityId) {

            for (int i = 0; i < this.filters.Length; ++i) {

                if (this.filters[i] == null) continue;
                this.filters[i].OnDeserialize(lastEntityId);

            }

        }

        public int GetAllFiltersArchetypeCount() {

            return FiltersStorage.allFiltersArchetype.Count;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasInAnyFilter<TComponent>() {

            return ComponentTypes<TComponent>.typeId >= 0; //this.allFiltersArchetype.Has<TComponent>();

        }

        public void RegisterInAllArchetype(in Archetype archetype) {

            FiltersStorage.allFiltersArchetype.Add(in archetype);

        }

        void IPoolableRecycle.OnRecycle() {

            this.nextId = default;
            this.filtersTree.Dispose();

            for (int i = 0, count = this.filters.Length; i < count; ++i) {

                if (this.filters[i] == null) continue;
                this.filters[i].Recycle();
                this.filters[i] = null;

            }

            PoolArray<FilterData>.Recycle(ref this.filters);

        }

        public void Initialize(int capacity) {

            this.filters = PoolArray<FilterData>.Spawn(capacity);

        }

        public void SetFreeze(bool freeze) {

        }

        public BufferArray<FilterData> GetData() {

            return this.filters;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref FilterData Get(int id) {

            return ref this.filters.arr[id - 1];

        }

        public FilterData GetByHashCode(int hashCode) {

            for (int i = 0; i < this.filters.Length; ++i) {

                ref readonly var filter = ref this.filters.arr[i];
                if (filter != null) {

                    if (filter.GetHashCode() == hashCode) {

                        return filter;

                    }

                }

            }

            return null;

        }

        public FilterData GetFilterEquals(FilterBuilder builder) {

            for (int i = 0; i < this.filters.Length; ++i) {

                ref readonly var filter = ref this.filters.arr[i];
                if (filter != null) {

                    if (filter.GetHashCode() == builder.GetHashCode() &&
                        filter.IsEquals(builder) == true) {

                        return filter;

                    }

                }

            }

            return null;

        }

        public void Register(FilterData filter) {

            ArrayUtils.Resize(filter.id - 1, ref this.filters);
            this.filters[filter.id - 1] = filter;

        }

        public int GetNextId() {

            return this.nextId + 1;

        }

        public int AllocateNextId() {

            return ++this.nextId;

        }

        private struct FilterCopy : IArrayElementCopy<FilterData> {

            public void Copy(FilterData @from, ref FilterData to) {

                if (from == null && to == null) return;

                if (from == null && to != null) {

                    to.Recycle();
                    to = null;

                } else if (from != null && to == null) {

                    to = from.Clone();

                } else {

                    to.CopyFrom(from);

                }

            }

            public void Recycle(FilterData item) {

                if (item != null) item.Recycle();

            }

        }

        public void CopyFrom(FiltersStorage other) {

            this.nextId = other.nextId;
            this.filtersTree.CopyFrom(other.filtersTree);
            ArrayUtils.Copy(other.filters, ref this.filters, new FilterCopy());

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct MultipleFilterEnumerator : IEnumerator<Entity> {

        private FilterEnumerator primaryEnumerator;
        private FilterEnumerator secondaryEnumerator;
        private BufferArray<bool> primaryChecked;
        private readonly bool useSecondary;
        private bool iterateSecondary;
        private readonly int primaryOffset;

        internal MultipleFilterEnumerator(MultipleFilter set) {

            this.iterateSecondary = false;
            this.primaryEnumerator = set.primaryFilter.GetEnumerator();
            this.secondaryEnumerator = set.secondaryFilter.GetEnumerator();
            this.useSecondary = set.useSecondary;
            if (this.useSecondary == true) {

                var count = 0;
                set.primaryFilter.GetBounds(out var min, out var max);
                if (max >= min) {
                    
                    count = max - min + 1;
                    
                }
                this.primaryChecked = PoolArray<bool>.Spawn(count);
                this.primaryOffset = min;

            } else {

                this.primaryChecked = default;
                this.primaryOffset = 0;

            }

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {

            if (this.useSecondary == true) PoolArray<bool>.Recycle(ref this.primaryChecked);
            this.primaryEnumerator.Dispose();
            this.secondaryEnumerator.Dispose();

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool MoveNext() {

            if (this.primaryEnumerator.MoveNext() == true) {

                this.iterateSecondary = false;
                if (this.useSecondary == true) this.primaryChecked.arr[this.primaryEnumerator.Current.id - this.primaryOffset] = true;
                return true;
                
            }

            if (this.useSecondary == false) return false;
            
            this.iterateSecondary = true;
            var next = false;
            while (true) {
                
                next = this.secondaryEnumerator.MoveNext();
                if (next == true) {

                    var idx = this.secondaryEnumerator.Current.id - this.primaryOffset;
                    if (idx < this.primaryChecked.Length && this.primaryChecked.arr[idx] == true) {
                        
                        continue;
                        
                    }
                    
                }

                break;

            }
            
            return next;
            
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        Entity IEnumerator<Entity>.Current {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                if (this.iterateSecondary == true) return this.secondaryEnumerator.Current;
                return this.primaryEnumerator.Current;
            }
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public ref readonly Entity Current {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                if (this.iterateSecondary == true) return ref this.secondaryEnumerator.Current;
                return ref this.primaryEnumerator.Current;
            }
        }

        System.Object System.Collections.IEnumerator.Current {
            get {
                throw new AllocationException();
            }
        }

        void System.Collections.IEnumerator.Reset() { }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct MultipleFilter {

        internal Filter primaryFilter;
        internal Filter secondaryFilter;
        internal FilterBuilder primary;
        internal FilterBuilder secondary;
        internal bool useSecondary;

        public int Count {
            get {
                return this.primaryFilter.Count + (this.useSecondary == true ? this.secondaryFilter.Count : 0);
            }
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public MultipleFilterEnumerator GetEnumerator() {

            return new MultipleFilterEnumerator(this);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public MultipleFilter Any<TComponent0, TComponent1>() where TComponent0 : struct, IStructComponentBase where TComponent1 : struct, IStructComponentBase {

            this.primary.With<TComponent0>();
            this.secondary.With<TComponent1>();
            this.useSecondary = true;
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public MultipleFilter WithoutAny<TComponent0, TComponent1>() where TComponent0 : struct, IStructComponentBase where TComponent1 : struct, IStructComponentBase {

            this.primary.Without<TComponent0>();
            this.secondary.Without<TComponent1>();
            this.useSecondary = true;
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public MultipleFilter With<TComponent>() where TComponent : struct, IStructComponentBase {

            this.primary.With<TComponent>();
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public MultipleFilter Without<TComponent>() where TComponent : struct, IStructComponentBase {

            this.primary.Without<TComponent>();
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public MultipleFilter Push() {

            return this.Push(ref this);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public MultipleFilter Push(ref MultipleFilter filter) {

            filter.useSecondary = this.useSecondary;
            Filter.Push(ref filter.primaryFilter, this.primary, checkExist: true);
            if (this.useSecondary == true) Filter.Push(ref filter.secondaryFilter, this.secondary, checkExist: false);
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public MultipleFilter OnVersionChangedOnly() {

            this.primary.OnVersionChangedOnly();
            this.secondary.OnVersionChangedOnly();
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static MultipleFilter Create(string customName = null) {

            var filter = new MultipleFilter {
                primary = Filter.Create(customName),
                secondary = Filter.Create(customName),
            };
            return filter;

        }

        public NativeBufferArray<Entity> ToArray() {

            if (this.useSecondary == false) return this.primaryFilter.ToArray();
            
            var arr = PoolArrayNative<Entity>.Spawn(this.primaryFilter.Count + this.secondaryFilter.Count);
            var primaryArray = this.primaryFilter.ToArray();
            NativeArrayUtils.Copy(primaryArray, 0, ref arr, 0, this.primaryFilter.Count);
            primaryArray.Dispose();
            var secondaryArray = this.secondaryFilter.ToArray();
            if (secondaryArray.Length > 0) NativeArrayUtils.Copy(secondaryArray, 0, ref arr, this.primaryFilter.Count, this.secondaryFilter.Count);
            secondaryArray.Dispose();
            return arr;

        }
        
    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct FilterEnumerator : IEnumerator<Entity> {

        private readonly FilterData set;
        private readonly int max;
        private int index;
        private readonly byte onVersionChangedOnly;
        
        private readonly State state;

        internal FilterEnumerator(FilterData set) {

            this.set = set;
            this.set.GetBounds(out this.index, out this.max);
            --this.index;
            if (this.index > this.max) {

                this.index = 0;
                this.max = 0;

            }

            this.state = this.set.world.currentState;
            this.onVersionChangedOnly = this.set.data.onVersionChangedOnly;
            this.set.SetForEachMode(true);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {

            this.set.SetForEachMode(false);
            this.set.UseVersioned();

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool MoveNext() {

            if (this.set.data.hasShared == 1) {

                ref var entArchetype = ref this.set.world.currentState.storage.archetypes.Get(0);
                if (entArchetype.Has(in this.set.data.sharedArchetypeContains) == false) return false;
                if (entArchetype.HasNot(in this.set.data.sharedArchetypeNotContains) == false) return false;
                
            }

            ref readonly var arr = ref this.set.data.dataContains;
            do {

                ++this.index;
                if (this.index > this.max) return false;
                if (arr.GetRefRead(this.index) != 1) continue;
                if (this.onVersionChangedOnly == 1 && this.set.data.dataVersions.GetRefRead(this.index) == 0) continue;
                
                break;

            } while (true);

            if (this.onVersionChangedOnly == 1) {

                this.set.data.dataVersions[this.index] = 0;

            }

            return true;

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        Entity IEnumerator<Entity>.Current {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                return this.state.storage.cache.Read(this.index);
            }
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public ref readonly Entity Current {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                return ref this.state.storage.cache.Read(this.index);
            }
        }

        System.Object System.Collections.IEnumerator.Current {
            get {
                throw new AllocationException();
            }
        }

        void System.Collections.IEnumerator.Reset() { }

    }

    public struct FilterBuilder {

        internal string name;
        internal World world;
        internal bool onVersionChangedOnly;
        internal Archetype with;
        internal Archetype without;
        internal Archetype withShared;
        internal Archetype withoutShared;
        internal System.Action<FilterData> actions;

        public override int GetHashCode() {

            var hashCode = this.with.GetHashCode() ^ this.without.GetHashCode()
                           ^ this.withShared.GetHashCode() ^ this.withoutShared.GetHashCode()
                           ^ (this.onVersionChangedOnly == true ? 1 : 0);
            
            return hashCode;

        }
        
        public Filter Push() {

            var filter = new Filter();
            return this.Push(ref filter);

        }

        public Filter Push(ref Filter filter, bool checkExist = true) {

            return Filter.Push(ref filter, this, checkExist);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public FilterBuilder OnVersionChangedOnly() {

            this.onVersionChangedOnly = true;
            return this;

        }

        #if !ENTITY_API_VERSION1_TURN_OFF
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public FilterBuilder WithStructComponent<TComponent>() where TComponent : struct, IStructComponentBase {

            return this.With<TComponent>();

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public FilterBuilder WithoutStructComponent<TComponent>() where TComponent : struct, IStructComponentBase {

            return this.Without<TComponent>();

        }
        #endif

        #if !ENTITY_API_VERSION2_TURN_OFF
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public FilterBuilder With<TComponent>() where TComponent : struct, IStructComponentBase {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.with.Add<TComponent>();
            this.actions += (filterData) => {

                filterData.WithStructComponent<TComponent>();

            };
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public FilterBuilder Without<TComponent>() where TComponent : struct, IStructComponentBase {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.without.Add<TComponent>();
            this.actions += (filterData) => {

                filterData.WithoutStructComponent<TComponent>();

            };
            return this;

        }
        #endif
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public FilterBuilder WithShared<TComponent>() where TComponent : struct, IStructComponentBase {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.withShared.Add<TComponent>();
            this.actions += (filterData) => {

                filterData.WithSharedComponent<TComponent>();

            };
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public FilterBuilder WithoutShared<TComponent>() where TComponent : struct, IStructComponentBase {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.withoutShared.Add<TComponent>();
            this.actions += (filterData) => {

                filterData.WithoutSharedComponent<TComponent>();

            };
            return this;

        }

        [System.ObsoleteAttribute("Entity actions in filters are no longer available")]
        public FilterBuilder SetOnEntityAdd<T>(T predicate) where T : class, IFilterAction => this;
        [System.ObsoleteAttribute("Entity actions in filters are no longer available")]
        public FilterBuilder SetOnEntityRemove<T>(T predicate) where T : class, IFilterAction => this;

    }

    public delegate void FilterInjectionDelegate(ref FilterBuilder builder);

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct Filter {

        internal int id;
        internal World world => Worlds.current;

        public static Filter Empty => new Filter();
        internal static FilterInjectionDelegate injections;

        public int Count => this.world.GetFilter(this.id).Count;

        public static void RegisterInject(FilterInjectionDelegate onFilter) {

            Filter.injections += onFilter;

        }

        public static void UnregisterInject(FilterInjectionDelegate onFilter) {

            Filter.injections -= onFilter;
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int GetMaxEntityId() {

            return this.world.GetFilter(this.id).GetMaxEntityId();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsForEntity(int id) {

            return this.world.GetFilter(this.id).IsForEntity(id);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void ApplyAllRequests() {

            this.world.GetFilter(this.id).ApplyAllRequests();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Contains(in Entity entity) {

            return this.world.GetFilter(this.id).Contains(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public FilterEnumerator GetEnumerator() {

            return new FilterEnumerator(this.world.GetFilter(this.id));

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsAlive() {

            return Worlds.currentWorld.HasFilter(this.id);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void GetBounds(out int min, out int max) {

            this.world.GetFilter(this.id).GetBounds(out min, out max);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArray<Entity> ToArray() {

            return this.world.GetFilter(this.id).ToArray();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void UseVersioned() {

            this.world.GetFilter(this.id).UseVersioned();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FilterBuilder Create(string customName = null) {

            return new FilterBuilder() {
                name = customName,
                world = Worlds.currentWorld,
            };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FilterBuilder CreateFromData(FilterDataTypes filterDataTypes, string customName = null) {

            /*var filter = FilterData.Create(customName).With(filterDataTypes.with).Without(filterDataTypes.without);
            var f = new Filter() {
                id = filter.id,
                world = Worlds.currentWorld,
            };

            return f;*/
            return default;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal static Filter Push(ref Filter filter, FilterBuilder builder, bool checkExist = true) {

            if (Filter.injections != null) Filter.injections.Invoke(ref builder);

            var filterData = (checkExist == true ? builder.world.GetFilterEquals(builder) : null);
            if (filterData == null) {
                filterData = FilterData.Create(builder.name);
            } else {
                filterData.AddAlias(builder.name);
                #if UNITY_EDITOR
                filterData.OnEditorFilterAddStackTrace(filterData.GetEditorStackTraceFilename(0), filterData.GetEditorStackTraceLineNumber(0));
                #endif
                                                                      
            }
            filterData.Push(ref builder);
            filter.id = filterData.id;
            return filter;

        }

    }

    public static class FilterDataStatic {

        public static class InterlockedExtension {

            public static bool AssignIfNewValueSmaller(ref int target, int newValue) {
                int snapshot;
                bool stillLess;
                do {
                    snapshot = target;
                    stillLess = newValue < snapshot;
                } while (stillLess && System.Threading.Interlocked.CompareExchange(ref target, newValue, snapshot) != snapshot);

                return stillLess;
            }

            public static bool AssignIfNewValueBigger(ref int target, int newValue) {
                int snapshot;
                bool stillMore;
                do {
                    snapshot = target;
                    stillMore = newValue > snapshot;
                } while (stillMore && System.Threading.Interlocked.CompareExchange(ref target, newValue, snapshot) != snapshot);

                return stillMore;
            }

        }

        public static byte Contains_INTERNAL(ref Unity.Collections.NativeArray<byte> arr, int entityId) {

            if (entityId >= arr.Length) return 0;
            return arr[entityId];

        }

        public static bool OnUpdateForced_INTERNAL(ref FilterBurstData data, in Entity entity) {

            if (entity.generation == Entity.GENERATION_ZERO) return false;

            var isExists = FilterDataStatic.Contains_INTERNAL(ref data.dataContains, entity.id);
            FilterDataStatic.Update_INTERNAL(ref data, in entity);
            if (isExists == 1) {

                return FilterDataStatic.CheckRemove(ref data, in entity);

            } else {

                return FilterDataStatic.CheckAdd(ref data, in entity);

            }

        }

        public static bool OnUpdate_INTERNAL(ref FilterBurstData data, in Entity entity) {

            if (entity.generation == Entity.GENERATION_ZERO) return false;

            if (data.forEachMode > 0) {

                for (int i = 0; i < data.requestsRemoveCount; ++i) {

                    if (data.requestsRemoveEntity[i] == entity) {
                        
                        data.requestsRemoveEntity[i] = default;
                        data.requestsRemoveEntity[i] = data.requestsRemoveEntity[data.requestsRemoveCount - 1];
                        data.requestsRemoveEntity[data.requestsRemoveCount - 1] = default;
                        --data.requestsRemoveCount;
                        break;

                    }
                    
                }

                var cnt = data.requestsCount;
                NativeArrayUtils.Resize(cnt, ref data.requests, resizeWithOffset: true);
                data.requests[cnt] = entity;
                System.Threading.Interlocked.Increment(ref data.requestsCount);
                return false;

            }

            var isExists = FilterDataStatic.Contains_INTERNAL(ref data.dataContains, entity.id);
            FilterDataStatic.Update_INTERNAL(ref data, in entity);
            if (isExists == 1) {

                return FilterDataStatic.CheckRemove(ref data, in entity);

            } else {

                return FilterDataStatic.CheckAdd(ref data, in entity);

            }

        }

        public static bool OnRemoveEntity(ref FilterBurstData data, in Entity entity) {

            if (entity.generation == Entity.GENERATION_ZERO) return false;

            if (data.forEachMode > 0) {

                for (int i = 0; i < data.requestsCount; ++i) {

                    if (data.requests[i] == entity) {
                        
                        data.requests[i] = default;
                        data.requests[i] = data.requests[data.requestsCount - 1];
                        data.requests[data.requestsCount - 1] = default;
                        --data.requestsCount;
                        break;

                    }
                    
                }

                //this.requestsRemoveEntity.TryAdd(entity.version, entity);
                var cnt = data.requestsRemoveCount;
                NativeArrayUtils.Resize(cnt, ref data.requestsRemoveEntity, resizeWithOffset: true);
                data.requestsRemoveEntity[cnt] = entity;
                System.Threading.Interlocked.Increment(ref data.requestsRemoveCount);
                return false;

            }

            return FilterDataStatic.Remove_INTERNAL(ref data, entity);

        }

        public static void Update_INTERNAL(ref FilterBurstData data, in Entity entity) {

            if (data.onVersionChangedOnly == 1) {

                var idx = entity.id;
                data.dataVersions[idx] = 1;
                FilterDataStatic.UpdateMinMaxAdd(ref data, idx);

            }

        }

        public static bool Add_INTERNAL(ref FilterBurstData data, in Entity entity) {

            var idx = entity.id;
            ref var res = ref data.dataContains.GetRef(idx);
            if (res == 0) {

                res = 1;
                System.Threading.Interlocked.Increment(ref data.count);
                FilterDataStatic.UpdateMinMaxAdd(ref data, idx);
                return true;

            }

            return false;

        }

        public static bool Remove_INTERNAL(ref FilterBurstData data, in Entity entity) {

            var idx = entity.id;
            ref var res = ref data.dataContains.GetRef(idx);
            if (res == 1) {

                res = 0;
                if (data.onVersionChangedOnly == 1) data.dataVersions[idx] = 0;
                System.Threading.Interlocked.Decrement(ref data.count);
                FilterDataStatic.UpdateMinMaxRemove(ref data, idx);
                return true;

            }

            return false;

        }

        public static void UpdateMinMaxAdd(ref FilterBurstData data, int idx) {

            InterlockedExtension.AssignIfNewValueSmaller(ref data.min, idx);
            InterlockedExtension.AssignIfNewValueBigger(ref data.max, idx);
            
        }

        public static void UpdateMinMaxRemove(ref FilterBurstData data, int idx) {

            if (idx == data.min && idx == data.max) {

                data.min = int.MaxValue;
                data.max = int.MinValue;
                return;

            }

            if (idx == data.min) {

                // Update new min (find next index)
                var changed = false;
                for (int i = idx; i < data.dataContains.Length; ++i) {

                    if (data.dataContains[i] == 1) {

                        data.min = i;
                        changed = true;
                        break;

                    }

                }

                if (changed == false) {

                    data.min = int.MaxValue;

                }

            }

            if (idx == data.max) {

                // Update new max (find prev index)
                var changed = false;
                for (int i = idx; i >= 0; --i) {

                    if (data.dataContains[i] == 1) {

                        data.max = i;
                        changed = true;
                        break;

                    }

                }

                if (changed == false) {

                    data.max = int.MinValue;

                }

            }

        }

        public static bool CheckAdd(ref FilterBurstData data, in Entity entity) {

            // If entity doesn't exist in cache - try to add if entity's archetype fit with contains & notContains
            ref var entArchetype = ref data.archetypes.Get(entity.id);
            if (entArchetype.Has(in data.archetypeContains) == false) return false;
            if (entArchetype.HasNot(in data.archetypeNotContains) == false) return false;

            FilterDataStatic.Add_INTERNAL(ref data, in entity);

            return true;

        }

        public static bool CheckRemove(ref FilterBurstData data, in Entity entity) {

            // If entity already exists in cache - try to remove if entity's archetype doesn't fit with contains & notContains
            ref var entArchetype = ref data.archetypes.Get(entity.id);
            var allContains = entArchetype.Has(in data.archetypeContains);
            var allNotContains = entArchetype.HasNot(in data.archetypeNotContains);
            if (allContains == true && allNotContains == true) return false;

            return FilterDataStatic.Remove_INTERNAL(ref data, in entity);

        }

    }
    
    public struct FilterBurstData {

        // Readonly
        public byte forEachMode;
        public byte hasShared;
        [Extensions.TestIgnoreAttribute]
        public ArchetypeEntities archetypes;
        public Archetype archetypeContains;
        public Archetype archetypeNotContains;
        public Archetype sharedArchetypeContains;
        public Archetype sharedArchetypeNotContains;
        public byte onVersionChangedOnly;
        
        // Read&Write
        [Unity.Collections.NativeDisableParallelForRestrictionAttribute]
        public Unity.Collections.NativeArray<Entity> requests;
        [Unity.Collections.NativeDisableParallelForRestrictionAttribute]
        public Unity.Collections.NativeArray<Entity> requestsRemoveEntity;
        [Unity.Collections.NativeDisableParallelForRestrictionAttribute]
        public Unity.Collections.NativeArray<byte> dataContains;
        [Extensions.TestIgnoreAttribute]
        [Unity.Collections.NativeDisableParallelForRestrictionAttribute]
        public Unity.Collections.NativeArray<byte> dataVersions;
        [Unity.Collections.NativeDisableParallelForRestrictionAttribute]
        public Unity.Collections.NativeArray<int> counters;

        public ref int min => ref this.counters.GetRef(0);
        public ref int max => ref this.counters.GetRef(1);
        public ref int count => ref this.counters.GetRef(2);
        public ref int requestsCount => ref this.counters.GetRef(3);
        public ref int requestsRemoveCount => ref this.counters.GetRef(4);

        public void CopyFrom(in FilterBurstData other) {
            
            this.forEachMode = other.forEachMode;

            this.counters.CopyFrom(other.counters);

            this.onVersionChangedOnly = other.onVersionChangedOnly;

            this.archetypeContains = other.archetypeContains;
            this.archetypeNotContains = other.archetypeNotContains;
            this.sharedArchetypeContains = other.sharedArchetypeContains;
            this.sharedArchetypeNotContains = other.sharedArchetypeNotContains;
            this.hasShared = other.hasShared;

            NativeArrayUtils.Copy(in other.requests, ref this.requests);
            NativeArrayUtils.Copy(in other.requestsRemoveEntity, ref this.requestsRemoveEntity);
            NativeArrayUtils.Copy(in other.dataContains, ref this.dataContains);
            if (this.onVersionChangedOnly == 1) NativeArrayUtils.Copy(in other.dataVersions, ref this.dataVersions);

        }

        public void Dispose() {
            
            this.dataContains.Dispose();
            this.requests.Dispose();
            this.requestsRemoveEntity.Dispose();
            if (this.onVersionChangedOnly == 1) this.dataVersions.Dispose();
            this.counters.Dispose();

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class FilterData : IPoolableSpawn, IPoolableRecycle, IEnumerable<Entity> {

        private const int REQUESTS_CAPACITY = 4;
        private const int NODES_CAPACITY = 4;
        private const int ENTITIES_CAPACITY = 100;

        public int id;

        public string name {
            get {
                return this.aliases.arr[0];
            }
        }

        internal World world {
            get {
                return Worlds.currentWorld;
            }
        }

        private System.Action<FilterData> setupVersioned;
        
        public FilterBurstData data;
        internal BufferArray<string> aliases;
        
        public bool isPooled;

        #if UNITY_EDITOR
        private string[] editorTypes;
        private string[] editorStackTraceFile;
        private int[] editorStackTraceLineNumber;
        #endif

        public FilterData() { }
        
        internal FilterData(string name) {

            this.AddAlias(name);

        }

        public FiltersTree.FilterBurst GetBurstData() {
            
            return new FiltersTree.FilterBurst() {
                id = this.id,
                contains = this.data.archetypeContains,
                notContains = this.data.archetypeNotContains,
            };
            
        }

        public void Clear() {

            this.data.archetypes = this.world.currentState.storage.archetypes;
            for (int i = 0; i < this.data.dataContains.Length; ++i) {

                if (this.data.dataContains[i] == 1) {

                    FilterDataStatic.Remove_INTERNAL(ref this.data, in this.world.currentState.storage.cache[i]);

                }

            }

            if (this.data.onVersionChangedOnly == 1) NativeArrayUtils.Clear(this.data.dataVersions);
            NativeArrayUtils.Clear(this.data.dataContains);
            this.data.count = 0;

        }

        public void OnDeserialize(int lastEntityId) {

            this.SetEntityCapacity(lastEntityId);

        }

        public void Recycle() {

            if (this.isPooled == false) PoolFilters.Recycle(this);

        }

        internal void SetEntityCapacity(int capacity) {

            NativeArrayUtils.Resize(capacity, ref this.data.dataContains, resizeWithOffset: true);
            if (this.data.onVersionChangedOnly == 1) NativeArrayUtils.Resize(capacity, ref this.data.dataVersions, resizeWithOffset: true);

        }

        internal void OnEntityCreate(in Entity entity) {

            NativeArrayUtils.Resize(entity.id, ref this.data.dataContains, resizeWithOffset: true);
            if (this.data.onVersionChangedOnly == 1) NativeArrayUtils.Resize(entity.id, ref this.data.dataVersions, resizeWithOffset: true);

        }

        internal void OnEntityDestroy(in Entity entity) {

            NativeArrayUtils.Resize(entity.id, ref this.data.dataContains, resizeWithOffset: true);
            if (this.data.onVersionChangedOnly == 1) NativeArrayUtils.Resize(entity.id, ref this.data.dataVersions, resizeWithOffset: true);

        }

        public void Update() {

            var list = PoolListCopyable<Entity>.Spawn(FilterData.ENTITIES_CAPACITY);
            if (this.world.ForEachEntity(list) == true) {

                for (int i = 0; i < list.Count; ++i) {

                    this.OnUpdate(in list[i]);

                }

            }

            PoolListCopyable<Entity>.Recycle(ref list);

        }

        public FilterData Clone() {

            var instance = PoolFilters.Spawn<FilterData>();
            instance.CopyFrom(this);
            return instance;

        }

        public BufferArray<string> GetAllNames() {

            return this.aliases;

        }

        internal void AddAlias(string name) {

            if (string.IsNullOrEmpty(name) == true) return;

            var idx = (this.aliases.arr != null ? this.aliases.Length : 0);
            ArrayUtils.Resize(idx, ref this.aliases);
            this.aliases.arr[idx] = name;

        }

        #if UNITY_EDITOR
        public string GetEditorStackTraceFilename(int index) {

            return this.editorStackTraceFile[index];

        }

        public int GetEditorStackTraceLineNumber(int index) {

            return this.editorStackTraceLineNumber[index];

        }

        public string ToEditorTypesString() {

            return string.Join(", ", this.editorTypes, 0, this.editorTypes.Length);

        }

        private void AddTypeToEditorWith<TComponent>() {

            this.AddTypeToEditorWith(typeof(TComponent));

        }

        private void AddTypeToEditorWithout<TComponent>() {

            this.AddTypeToEditorWithout(typeof(TComponent));

        }

        private void AddTypeToEditorWithShared<TComponent>() {

            this.AddTypeToEditorWithShared(typeof(TComponent));

        }

        private void AddTypeToEditorWithoutShared<TComponent>() {

            this.AddTypeToEditorWithoutShared(typeof(TComponent));

        }

        private void AddTypeToEditorWith(System.Type type) {

            var idx = (this.editorTypes != null ? this.editorTypes.Length : 0);
            System.Array.Resize(ref this.editorTypes, idx + 1);
            this.editorTypes[idx] = "W<" + type.Name + ">";

        }

        private void AddTypeToEditorWithout(System.Type type) {

            var idx = (this.editorTypes != null ? this.editorTypes.Length : 0);
            System.Array.Resize(ref this.editorTypes, idx + 1);
            this.editorTypes[idx] = "WO<" + type.Name + ">";

        }

        private void AddTypeToEditorWithShared(System.Type type) {

            var idx = (this.editorTypes != null ? this.editorTypes.Length : 0);
            System.Array.Resize(ref this.editorTypes, idx + 1);
            this.editorTypes[idx] = "WS<" + type.Name + ">";

        }

        private void AddTypeToEditorWithoutShared(System.Type type) {

            var idx = (this.editorTypes != null ? this.editorTypes.Length : 0);
            System.Array.Resize(ref this.editorTypes, idx + 1);
            this.editorTypes[idx] = "WOS<" + type.Name + ">";

        }

        private void OnEditorFilterCreate() {

            const int frameIndex = 3;
            var st = new System.Diagnostics.StackTrace(true);
            string currentFile = st.GetFrame(frameIndex).GetFileName();
            int currentLine = st.GetFrame(frameIndex).GetFileLineNumber();

            var path = UnityEngine.Application.dataPath;
            currentFile = "Assets" + currentFile.Substring(path.Length);

            this.OnEditorFilterAddStackTrace(currentFile, currentLine);

        }

        public void OnEditorFilterAddStackTrace(string file, int lineNumber) {

            var idx = (this.editorStackTraceFile != null ? this.editorStackTraceFile.Length : 0);
            System.Array.Resize(ref this.editorStackTraceFile, idx + 1);
            this.editorStackTraceFile[idx] = file;

            idx = (this.editorStackTraceLineNumber != null ? this.editorStackTraceLineNumber.Length : 0);
            System.Array.Resize(ref this.editorStackTraceLineNumber, idx + 1);
            this.editorStackTraceLineNumber[idx] = lineNumber;

        }
        #endif

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void GetBounds(out int min, out int max) {

            min = this.data.min;
            max = this.data.max;

        }

        public void UseVersioned() {

            if (this.data.onVersionChangedOnly == 1) NativeArrayUtils.Clear(this.data.dataVersions);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArray<Entity> ToArray() {

            int customCount = 0;
            if (this.data.onVersionChangedOnly == 1) {

                for (int i = this.data.min; i <= this.data.max; ++i) {

                    if (this.data.dataContains[i] == 1 && this.data.dataVersions[i] == 1) {

                        ++customCount;

                    }

                }

            } else {

                customCount = (this.data.count >= this.data.requestsRemoveCount ? this.data.count - this.data.requestsRemoveCount : 0);

            }

            var data = PoolArrayNative<Entity>.Spawn(customCount);
            for (int i = this.data.min, k = 0; i <= this.data.max; ++i) {

                if (this.data.dataContains[i] == 1) {

                    if (this.data.onVersionChangedOnly == 1) {

                        if (this.data.dataVersions[i] == 0) continue;
                        this.data.dataVersions[i] = 0;

                    }

                    data[k++] = this.world.currentState.storage.cache.arr[i];

                }

            }

            return data;

        }

        void IPoolableSpawn.OnSpawn() {

            this.isPooled = false;

            this.id = default;
            if (this.aliases.arr != null) PoolArray<string>.Recycle(ref this.aliases);
            this.data = new FilterBurstData() {
                requests = new Unity.Collections.NativeArray<Entity>(FilterData.REQUESTS_CAPACITY, Unity.Collections.Allocator.Persistent),
                requestsRemoveEntity = new Unity.Collections.NativeArray<Entity>(FilterData.REQUESTS_CAPACITY, Unity.Collections.Allocator.Persistent),
                dataContains = new Unity.Collections.NativeArray<byte>(FilterData.ENTITIES_CAPACITY, Unity.Collections.Allocator.Persistent),
                dataVersions = default,
                archetypeContains = default,
                archetypeNotContains = default,
                counters = new Unity.Collections.NativeArray<int>(5, Unity.Collections.Allocator.Persistent),
                hasShared = 0,
                forEachMode = 0,
                onVersionChangedOnly = 0,
            };
            this.data.counters[0] = int.MaxValue;
            this.data.counters[1] = int.MinValue;

            #if UNITY_EDITOR
            this.editorTypes = null;
            this.editorStackTraceFile = null;
            this.editorStackTraceLineNumber = null;
            #endif

        }

        void IPoolableRecycle.OnRecycle() {

            this.data.Dispose();
            this.data = default;
            
            if (this.aliases.arr != null) PoolArray<string>.Recycle(ref this.aliases);
            this.id = default;

            this.isPooled = true;

            #if UNITY_EDITOR
            this.editorTypes = null;
            this.editorStackTraceFile = null;
            this.editorStackTraceLineNumber = null;
            #endif

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsForEntity(int entityId) {

            ref var arch = ref this.world.currentState.storage.archetypes;
            ref var cont = ref this.data.archetypeContains;
            ref var notCont = ref this.data.archetypeNotContains;
            
            ref var previousArchetype = ref arch.prevTypes[entityId];
            if (previousArchetype.Has(in cont) == true &&
                previousArchetype.HasNot(in notCont) == true) {
                return true;
            }

            ref var currentArchetype = ref arch.types[entityId];
            if (currentArchetype.Has(in cont) == true &&
                currentArchetype.HasNot(in notCont) == true) {
                return true;
            }
            
            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Archetype GetArchetypeContains() {

            return this.data.archetypeContains;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Archetype GetArchetypeNotContains() {

            return this.data.archetypeNotContains;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetForEachMode(bool state) {

            if (state == true) {

                #if WORLD_EXCEPTIONS
                if (this.data.forEachMode == 255) {
                    throw new System.Exception("Filter: Max forEachMode has reached. Please, decrease foreach instructions.");
                }
                #endif
                ++this.data.forEachMode;
                
            } else {
                
                #if WORLD_EXCEPTIONS
                if (this.data.forEachMode == 0) {
                    throw new System.Exception("Filter: Call SetForEachMode(false), but filter has not in foreach mode on.");
                }
                #endif
                --this.data.forEachMode;
                
            }
            
            if (state == false) {

                this.ApplyAllRequests();

            }

        }

        [Unity.Burst.BurstCompile(Unity.Burst.FloatPrecision.Low, Unity.Burst.FloatMode.Fast, CompileSynchronously = true)]
        private struct ApplyRequestsJob : IJobParallelFor {

            public FilterBurstData data;

            public void Execute(int index) {

                ref var dataRef = ref this.data;
                FilterDataStatic.OnUpdateForced_INTERNAL(ref dataRef, in dataRef.requests.GetRefRead(index));

            }

        }

        [Unity.Burst.BurstCompile(Unity.Burst.FloatPrecision.Low, Unity.Burst.FloatMode.Fast, CompileSynchronously = true)]
        private struct ApplyRequestsJobToRemove : IJobParallelFor {

            public FilterBurstData data;

            public void Execute(int index) {
                
                ref var dataRef = ref this.data;
                FilterDataStatic.Remove_INTERNAL(ref dataRef, in dataRef.requestsRemoveEntity.GetRefRead(index));
                
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void ApplyAllRequests() {

            JobHandle jobHandle = default;
            JobHandle jobRemoveHandle = default;
            if (this.data.requestsCount > 0) {

                var job = new ApplyRequestsJob() {
                    data = this.data,
                };
                jobHandle = job.Schedule(this.data.requestsCount, 64);

            }

            if (this.data.requestsRemoveCount > 0) {

                var jobRemove = new ApplyRequestsJobToRemove() {
                    data = this.data,
                };
                jobRemoveHandle = jobRemove.Schedule(this.data.requestsRemoveCount, 64);

            }

            if (this.data.requestsCount > 0 && this.data.requestsRemoveCount > 0) {

                JobHandle.CompleteAll(ref jobHandle, ref jobRemoveHandle);

            } else if (this.data.requestsCount > 0) {

                jobHandle.Complete();

            } else if (this.data.requestsRemoveCount > 0) {
                
                jobRemoveHandle.Complete();
                
            }

            this.data.requestsCount = 0;
            this.data.requestsRemoveCount = 0;

        }

        public void CopyFrom(FilterData other) {

            this.isPooled = other.isPooled;
            
            this.data.CopyFrom(other.data);
            
            this.id = other.id;
            ArrayUtils.Copy(in other.aliases, ref this.aliases);
            
            #if UNITY_EDITOR
            this.editorTypes = other.editorTypes;
            this.editorStackTraceFile = other.editorStackTraceFile;
            this.editorStackTraceLineNumber = other.editorStackTraceLineNumber;
            #endif

        }

        public int Count {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                return this.data.count;
            }
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Contains(in Entity entity) {

            return FilterDataStatic.Contains_INTERNAL(ref this.world.GetFilter(this.id).data.dataContains, entity.id) == 1 ? true : false;

        }

        IEnumerator<Entity> IEnumerable<Entity>.GetEnumerator() {

            //return ((IEnumerable<Entity>)this.data).GetEnumerator();
            throw new AllocationException();

        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {

            //return ((System.Collections.IEnumerable)this.data).GetEnumerator();
            throw new AllocationException();

        }

        public FilterEnumerator GetEnumerator() {

            return new FilterEnumerator(this);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool OnUpdate(in Entity entity) {

            this.data.archetypes = this.world.currentState.storage.archetypes;
            return FilterDataStatic.OnUpdate_INTERNAL(ref this.data, in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool OnAddComponent(in Entity entity) {

            this.data.archetypes = this.world.currentState.storage.archetypes;
            return FilterDataStatic.OnUpdate_INTERNAL(ref this.data, in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool OnRemoveComponent(in Entity entity) {

            this.data.archetypes = this.world.currentState.storage.archetypes;
            return FilterDataStatic.OnUpdate_INTERNAL(ref this.data, in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int GetMaxEntityId() {
            return this.data.max;
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsEquals(FilterBuilder builder) {

            if (this.GetArchetypeContains() == builder.with &&
                this.GetArchetypeNotContains() == builder.without &&
                this.data.sharedArchetypeContains == builder.withShared &&
                this.data.sharedArchetypeNotContains == builder.withoutShared &&
                this.data.onVersionChangedOnly == (builder.onVersionChangedOnly == true ? 1 : 0)) {

                return true;

            }

            return false;

        }

        public override int GetHashCode() {

            var hashCode = this.data.archetypeContains.GetHashCode() ^ this.data.archetypeNotContains.GetHashCode()
                            ^ this.data.sharedArchetypeContains.GetHashCode() ^ this.data.sharedArchetypeNotContains.GetHashCode()
                            ^ this.data.onVersionChangedOnly;
            
            return hashCode;

        }

        public FilterData Push(ref FilterBuilder builder) {

            // Apply filter actions
            builder.actions.Invoke(this);
            // Postprocess all actions
            this.setupVersioned?.Invoke(this);
            this.setupVersioned = null;

            var world = builder.world;
            world.currentState.filters.RegisterInAllArchetype(in this.data.archetypeContains);
            world.currentState.filters.RegisterInAllArchetype(in this.data.archetypeNotContains);
            world.currentState.filters.RegisterInAllArchetype(in this.data.sharedArchetypeContains);
            world.currentState.filters.RegisterInAllArchetype(in this.data.sharedArchetypeNotContains);
            world.Register(this);

            return this;

        }

        public FilterData OnVersionChangedOnly() {

            this.data.onVersionChangedOnly = 1;

            return this;

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public FilterData With(IStructComponentBase[] components) {

            for (int i = 0; i < components.Length; ++i) {

                var type = components[i].GetType();
                if (ComponentTypesRegistry.allTypeId.TryGetValue(type, out var bit) == true) {
                
                    this.data.archetypeContains.AddBit(bit);
                    #if UNITY_EDITOR
                    this.AddTypeToEditorWith(type);
                    #endif
                    
                }

            }

            return this;

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public FilterData Without(IStructComponentBase[] components) {

            for (int i = 0; i < components.Length; ++i) {

                var type = components[i].GetType();
                if (ComponentTypesRegistry.allTypeId.TryGetValue(type, out var bit) == true) {
                
                    this.data.archetypeNotContains.AddBit(bit);
                    #if UNITY_EDITOR
                    this.AddTypeToEditorWithout(type);
                    #endif
                    
                }

            }

            return this;

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public FilterData WithStructComponent<TComponent>() where TComponent : struct, IStructComponentBase {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.setupVersioned += (f) => WorldUtilities.SetComponentAsFilterVersioned<TComponent>(f.data.onVersionChangedOnly == 1 ? true : false);
            this.data.archetypeContains.Add<TComponent>();
            #if UNITY_EDITOR
            this.AddTypeToEditorWith<TComponent>();
            #endif
            return this;

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public FilterData WithoutStructComponent<TComponent>() where TComponent : struct, IStructComponentBase {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.setupVersioned += (f) => WorldUtilities.SetComponentAsFilterVersioned<TComponent>(f.data.onVersionChangedOnly == 1 ? true : false);
            this.data.archetypeNotContains.Add<TComponent>();
            #if UNITY_EDITOR
            this.AddTypeToEditorWithout<TComponent>();
            #endif
            return this;

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public FilterData WithSharedComponent<TComponent>() where TComponent : struct, IStructComponentBase {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.data.sharedArchetypeContains.Add<TComponent>();
            this.data.hasShared = 1;
            #if UNITY_EDITOR
            this.AddTypeToEditorWithShared<TComponent>();
            #endif
            return this;

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public FilterData WithoutSharedComponent<TComponent>() where TComponent : struct, IStructComponentBase {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.data.sharedArchetypeNotContains.Add<TComponent>();
            this.data.hasShared = 1;
            #if UNITY_EDITOR
            this.AddTypeToEditorWithoutShared<TComponent>();
            #endif
            return this;

        }

        public static FilterData Create(string customName = null) {

            var nextId = Worlds.currentWorld.currentState.filters.AllocateNextId();
            var f = PoolFilters.Spawn<FilterData>();
            f.setupVersioned = null;
            f.id = nextId;
            f.AddAlias(customName);
            #if UNITY_EDITOR
            f.OnEditorFilterCreate();
            #endif
            return f;

        }

        public override string ToString() {

            if (this.aliases.isCreated == false) return "Filter (#" + this.id.ToString() + ")";
            return "Name: " + string.Join("/", this.aliases.arr, 0, this.aliases.Length) + " (#" + this.id.ToString() + ")";

        }

    }

}