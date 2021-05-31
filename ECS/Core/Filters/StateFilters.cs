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

                    if (this.filters[i].IsForEntity(in this.archetypeEntities, in this.entity) == true) {

                        this.result.Add(this.filters[i].id);

                    }

                }

            }

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void CalculateJob(in Entity entity, in NativeBufferArray<FiltersTree.FilterBurst> contains, in NativeBufferArray<FiltersTree.FilterBurst> notContains, out Unity.Collections.NativeList<int> containsResult, out Unity.Collections.NativeList<int> notContainsResult) {
            
            JobHandle jobContains;
            JobHandle jobNotContains;

            containsResult = new Unity.Collections.NativeList<int>(Unity.Collections.Allocator.Persistent);
            notContainsResult = new Unity.Collections.NativeList<int>(Unity.Collections.Allocator.Persistent);

            ref var archetypeEntities = ref Worlds.currentWorld.currentState.storage.archetypes;
            
            {
                var job = new UpdateFiltersJob() {
                    entity = entity,
                    archetypeEntities = archetypeEntities,
                    filters = contains,
                    result = containsResult,
                };
                jobContains = job.Schedule();
            }
            {
                var job = new UpdateFiltersJob() {
                    entity = entity,
                    archetypeEntities = archetypeEntities,
                    filters = notContains,
                    result = notContainsResult,
                };
                jobNotContains = job.Schedule();
            }
            JobHandle.CompleteAll(ref jobContains, ref jobNotContains);

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
        internal void RemoveFromFilters_INTERNAL(in Entity entity) {

            var bits = this.currentState.storage.archetypes.types.arr[entity.id];
            var bitsCount =  bits.BitsCount;
            for (int i = 0; i < bitsCount; ++i) {

                if (bits.HasBit(i) == true) {
                    
                    var filters = this.currentState.filters.filtersTree.GetFiltersContainsFor(i);
                    for (int f = 0; f < filters.Length; ++f) {

                        var filterId = filters.arr[f];
                        var filter = this.GetFilter(filterId.id);
                        filter.OnEntityDestroy(in entity);
                        filter.OnRemoveEntity(in entity);

                    }
                    
                }
                
            }
            
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

                if (this.filters.arr[i] == null) continue;
                this.filters.arr[i].Clear();

            }

        }

        public void OnDeserialize(int lastEntityId) {

            for (int i = 0; i < this.filters.Length; ++i) {

                if (this.filters.arr[i] == null) continue;
                this.filters.arr[i].OnDeserialize(lastEntityId);

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

                if (this.filters.arr[i] == null) continue;
                this.filters.arr[i].Recycle();
                this.filters.arr[i] = null;

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

                ref var filter = ref this.filters.arr[i];
                if (filter != null) {

                    if (filter.GetHashCode() == hashCode) {

                        return filter;

                    }

                }

            }

            return null;

        }

        public FilterData GetFilterEquals(FilterData other) {

            for (int i = 0; i < this.filters.Length; ++i) {

                ref var filter = ref this.filters.arr[i];
                if (filter != null) {

                    if (filter.GetHashCode() == other.GetHashCode() && filter.IsEquals(other) == true) {

                        return filter;

                    }

                }

            }

            return null;

        }

        public void Register(FilterData filter) {

            ArrayUtils.Resize(filter.id - 1, ref this.filters);
            this.filters.arr[filter.id - 1] = filter;

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
            this.primaryEnumerator = set.primary.GetEnumerator();
            this.secondaryEnumerator = set.secondary.GetEnumerator();
            this.useSecondary = set.useSecondary;
            if (this.useSecondary == true) {

                var count = 0;
                set.primary.GetBounds(out var min, out var max);
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
        public ref Entity Current {
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

        internal Filter primary;
        internal Filter secondary;
        internal bool useSecondary;

        public int Count {
            get {
                return this.primary.Count + (this.useSecondary == true ? this.secondary.Count : 0);
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
            this.primary.Push(ref filter.primary, checkExist: true);
            if (this.useSecondary == true) this.secondary.Push(ref filter.secondary, checkExist: false);
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

            if (this.useSecondary == false) return this.primary.ToArray();
            
            var arr = PoolArrayNative<Entity>.Spawn(this.primary.Count + this.secondary.Count);
            var primaryArray = this.primary.ToArray();
            NativeArrayUtils.Copy(primaryArray, 0, ref arr, 0, this.primary.Count);
            primaryArray.Dispose();
            var secondaryArray = this.secondary.ToArray();
            if (secondaryArray.Length > 0) NativeArrayUtils.Copy(secondaryArray, 0, ref arr, this.primary.Count, this.secondary.Count);
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
        private readonly bool onVersionChangedOnly;
        
        private readonly Storage storage;

        internal FilterEnumerator(FilterData set) {

            this.set = set;
            this.set.GetBounds(out this.index, out this.max);
            --this.index;
            if (this.index > this.max) {

                this.index = 0;
                this.max = 0;

            }

            this.storage = this.set.world.currentState.storage;
            this.onVersionChangedOnly = this.set.onVersionChangedOnly;
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

            if (this.set.hasShared == true) {

                ref var entArchetype = ref this.set.world.currentState.storage.archetypes.Get(0);
                if (entArchetype.Has(in this.set.sharedArchetypeContains) == false) return false;
                if (entArchetype.HasNot(in this.set.sharedArchetypeNotContains) == false) return false;
                
            }

            ref readonly var arr = ref this.set.dataContains.arr;
            ref readonly var ver = ref this.set.dataVersions.arr;
            do {

                ++this.index;
                if (this.index > this.max) return false;
                if (arr[this.index] != true) continue;
                if (this.onVersionChangedOnly == true && ver[this.index] == false) continue;
                
                break;

            } while (true);

            if (this.onVersionChangedOnly == true) {

                ver[this.index] = false;

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
                return this.storage.cache.arr[this.index];
            }
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public ref Entity Current {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                return ref this.storage.cache.arr[this.index];
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
    public struct Filter {

        internal int id;
        internal World world;
        private FilterData temp;

        public static Filter Empty => new Filter();
        internal static System.Action<Filter> injections;

        public int Count => this.world.GetFilter(this.id).Count;

        public static void RegisterInject(System.Action<Filter> onFilter) {

            Filter.injections += onFilter;

        }

        public static void UnregisterInject(System.Action<Filter> onFilter) {

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
        public Filter SetOnEntityAdd<T>(T predicate) where T : class, IFilterAction {

            this.temp.SetOnEntityAdd(predicate);
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Filter SetOnEntityRemove<T>(T predicate) where T : class, IFilterAction {

            this.temp.SetOnEntityRemove(predicate);
            return this;

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
        public Filter Push() {

            var filter = new Filter();
            return this.Push(ref filter);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Filter Push(ref Filter filter, bool checkExist = true) {

            if (Filter.injections != null) Filter.injections.Invoke(this);

            FilterData filterData = null;
            this.temp.Push(ref filterData, checkExist);
            filter.id = filterData.id;
            filter.world = Worlds.currentWorld;
            filter.temp = null;
            return filter;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Filter OnVersionChangedOnly() {

            this.temp.OnVersionChangedOnly();
            return this;

        }

        #if !ENTITY_API_VERSION1_TURN_OFF
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Filter WithStructComponent<TComponent>() where TComponent : struct, IStructComponentBase {

            this.temp.WithStructComponent<TComponent>();
            return this;

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Filter WithoutStructComponent<TComponent>() where TComponent : struct, IStructComponentBase {

            this.temp.WithoutStructComponent<TComponent>();
            return this;

        }
        #endif

        #if !ENTITY_API_VERSION2_TURN_OFF
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Filter With<TComponent>() where TComponent : struct, IStructComponentBase {

            this.temp.WithStructComponent<TComponent>();
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Filter Without<TComponent>() where TComponent : struct, IStructComponentBase {

            this.temp.WithoutStructComponent<TComponent>();
            return this;

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Filter WithShared<TComponent>() where TComponent : struct, IStructComponentBase {

            this.temp.WithSharedComponent<TComponent>();
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Filter WithoutShared<TComponent>() where TComponent : struct, IStructComponentBase {

            this.temp.WithoutSharedComponent<TComponent>();
            return this;

        }
        #endif

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
        public static Filter Create(string customName = null) {

            var filter = FilterData.Create(customName);
            return new Filter() {
                id = filter.id,
                world = Worlds.currentWorld,
                temp = filter,
            };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Filter CreateFromData(FilterDataTypes filterDataTypes, string customName = null) {

            var filter = FilterData.Create(customName).With(filterDataTypes.with).Without(filterDataTypes.without);
            var f = new Filter() {
                id = filter.id,
                world = Worlds.currentWorld,
                temp = filter,
            };

            return f;

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
        internal Archetype archetypeContains;
        internal Archetype archetypeNotContains;
        internal Archetype sharedArchetypeContains;
        internal Archetype sharedArchetypeNotContains;
        internal bool hasShared;
        
        internal NativeBufferArray<bool> dataContains;
        internal NativeBufferArray<bool> dataVersions;
        
        private bool forEachMode;
        #if MULTITHREAD_SUPPORT
        private CCList<Entity> requests;
        private CCList<Entity> requestsRemoveEntity;
        #else
        private ListCopyable<Entity> requests;
        private ListCopyable<Entity> requestsRemoveEntity;
        #endif
        private int min;
        private int max;

        internal BufferArray<string> aliases;
        private int dataCount;

        private IFilterAction predicateOnAdd;
        private IFilterAction predicateOnRemove;

        internal bool onVersionChangedOnly;

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
                contains = this.archetypeContains,
                notContains = this.archetypeNotContains,
            };
            
        }

        public void Clear() {

            for (int i = 0; i < this.dataContains.Length; ++i) {

                if (this.dataContains.arr[i] == true) {

                    this.Remove_INTERNAL(in this.world.currentState.storage.cache.arr[i]);

                }

            }

            NativeArrayUtils.Clear(this.dataVersions);
            NativeArrayUtils.Clear(this.dataContains);
            this.dataCount = 0;

        }

        public void OnDeserialize(int lastEntityId) {

            this.SetEntityCapacity(lastEntityId);

        }

        public void Recycle() {

            if (this.isPooled == false) PoolFilters.Recycle(this);

        }

        internal void SetEntityCapacity(int capacity) {

            NativeArrayUtils.Resize(capacity, ref this.dataContains);
            if (this.onVersionChangedOnly == true) NativeArrayUtils.Resize(capacity, ref this.dataVersions);

        }

        internal void OnEntityCreate(in Entity entity) {

            NativeArrayUtils.Resize(entity.id, ref this.dataContains);
            if (this.onVersionChangedOnly == true) NativeArrayUtils.Resize(entity.id, ref this.dataVersions);

        }

        internal void OnEntityDestroy(in Entity entity) {

            NativeArrayUtils.Resize(entity.id, ref this.dataContains);
            if (this.onVersionChangedOnly == true) NativeArrayUtils.Resize(entity.id, ref this.dataVersions);

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

        private void AddAlias(string name) {

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

            min = this.min;
            max = this.max;

        }

        public void UseVersioned() {

            if (this.onVersionChangedOnly == true) NativeArrayUtils.Clear(this.dataVersions);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArray<Entity> ToArray() {

            int customCount = 0;
            if (this.onVersionChangedOnly == true) {

                for (int i = this.min; i <= this.max; ++i) {

                    if (this.dataContains.arr[i] == true && this.dataVersions.arr[i] == true) {

                        ++customCount;

                    }

                }

            } else {

                customCount = (this.dataCount >= this.requestsRemoveEntity.Count ? this.dataCount - this.requestsRemoveEntity.Count : 0);

            }

            var data = PoolArrayNative<Entity>.Spawn(customCount);
            for (int i = this.min, k = 0; i <= this.max; ++i) {

                if (this.dataContains.arr[i] == true) {

                    if (this.onVersionChangedOnly == true) {

                        if (this.dataVersions.arr[i] == false) continue;
                        this.dataVersions.arr[i] = false;

                    }

                    data.arr[k++] = this.world.currentState.storage.cache.arr[i];

                }

            }

            return data;

        }

        void IPoolableSpawn.OnSpawn() {

            this.isPooled = false;

            //this.requests = PoolArray<Entity>.Spawn(Filter.REQUESTS_CAPACITY);
            //this.requestsRemoveEntity = PoolArray<Entity>.Spawn(Filter.REQUESTS_CAPACITY);
            #if MULTITHREAD_SUPPORT
            this.requests = PoolCCList<Entity>.Spawn();
            this.requestsRemoveEntity = PoolCCList<Entity>.Spawn();
            #else
            this.requests = PoolListCopyable<Entity>.Spawn(FilterData.REQUESTS_CAPACITY);
            this.requestsRemoveEntity = PoolListCopyable<Entity>.Spawn(FilterData.REQUESTS_CAPACITY);
            #endif
            this.dataContains = PoolArrayNative<bool>.Spawn(FilterData.ENTITIES_CAPACITY);
            this.dataCount = 0;

            this.id = default;
            if (this.aliases.arr != null) PoolArray<string>.Recycle(ref this.aliases);
            this.archetypeContains = default;
            this.archetypeNotContains = default;

            this.min = int.MaxValue;
            this.max = int.MinValue;

            this.predicateOnAdd = null;
            this.predicateOnRemove = null;

            this.onVersionChangedOnly = default;

            #if UNITY_EDITOR
            this.editorTypes = null;
            this.editorStackTraceFile = null;
            this.editorStackTraceLineNumber = null;
            #endif

        }

        void IPoolableRecycle.OnRecycle() {

            this.isPooled = true;

            PoolArrayNative<bool>.Recycle(ref this.dataContains);
            if (this.onVersionChangedOnly == true) PoolArrayNative<bool>.Recycle(ref this.dataVersions);
            #if MULTITHREAD_SUPPORT
            PoolCCList<Entity>.Recycle(ref this.requestsRemoveEntity);
            PoolCCList<Entity>.Recycle(ref this.requests);
            #else
            PoolListCopyable<Entity>.Recycle(ref this.requests);
            PoolListCopyable<Entity>.Recycle(ref this.requestsRemoveEntity);
            #endif

            this.min = int.MaxValue;
            this.max = int.MinValue;

            this.predicateOnAdd = null;
            this.predicateOnRemove = null;

            this.onVersionChangedOnly = default;

            this.dataCount = 0;
            this.archetypeContains = default;
            this.archetypeNotContains = default;
            this.id = default;
            if (this.aliases.arr != null) PoolArray<string>.Recycle(ref this.aliases);

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
            ref var cont = ref this.archetypeContains;
            ref var notCont = ref this.archetypeNotContains;
            
            ref var previousArchetype = ref arch.prevTypes.arr[entityId];
            if (previousArchetype.Has(in cont) == true &&
                previousArchetype.HasNot(in notCont) == true) {
                return true;
            }

            ref var currentArchetype = ref arch.types.arr[entityId];
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

            return this.archetypeContains;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Archetype GetArchetypeNotContains() {

            return this.archetypeNotContains;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetForEachMode(bool state) {

            this.forEachMode = state;
            if (state == false) {

                if (this.world.currentSystemContext == null) {

                    this.ApplyAllRequests();

                }

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void ApplyAllRequests() {

            {

                var requests = this.requests;
                for (int i = 0, cnt = requests.Count; i < cnt; ++i) {

                    this.OnUpdateForced_INTERNAL(requests[i]);

                }

                //System.Array.Clear(requests.arr, 0, requests.Length);
                #if MULTITHREAD_SUPPORT
                requests.ClearNoCC();
                #else
                requests.Clear();
                #endif

            }

            {

                var requests = this.requestsRemoveEntity;
                for (int i = 0, cnt = requests.Count; i < cnt; ++i) {

                    this.Remove_INTERNAL(requests[i]);

                }

                //System.Array.Clear(requests.arr, 0, requests.Length);
                #if MULTITHREAD_SUPPORT
                requests.ClearNoCC();
                #else
                requests.Clear();
                #endif

            }

        }

        public void CopyFrom(FilterData other) {

            this.isPooled = other.isPooled;

            this.id = other.id;
            this.min = other.min;
            this.max = other.max;
            this.dataCount = other.dataCount;
            ArrayUtils.Copy(in other.aliases, ref this.aliases);

            this.onVersionChangedOnly = other.onVersionChangedOnly;

            this.predicateOnAdd = other.predicateOnAdd;
            this.predicateOnRemove = other.predicateOnRemove;

            this.archetypeContains = other.archetypeContains;
            this.archetypeNotContains = other.archetypeNotContains;
            this.sharedArchetypeContains = other.sharedArchetypeContains;
            this.sharedArchetypeNotContains = other.sharedArchetypeNotContains;
            this.hasShared = other.hasShared;

            NativeArrayUtils.Copy(in other.dataContains, ref this.dataContains);
            if (this.onVersionChangedOnly == true) NativeArrayUtils.Copy(in other.dataVersions, ref this.dataVersions);

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
                return this.dataCount;
            }
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Contains(in Entity entity) {

            return this.world.GetFilter(this.id).Contains_INTERNAL(entity.id);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private bool Contains_INTERNAL(int entityId) {

            if (entityId >= this.dataContains.Length) return false;
            return this.dataContains.arr[entityId];

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

            return this.OnUpdate_INTERNAL(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool OnAddComponent(in Entity entity) {

            return this.OnUpdate_INTERNAL(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool OnRemoveComponent(in Entity entity) {

            return this.OnUpdate_INTERNAL(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private bool OnUpdateForced_INTERNAL(in Entity entity) {

            if (entity.generation == Entity.GENERATION_ZERO) return false;

            var isExists = this.Contains_INTERNAL(entity.id);
            this.Update_INTERNAL(in entity);
            if (isExists == true) {

                return this.CheckRemove(in entity);

            } else {

                return this.CheckAdd(in entity);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private bool OnUpdate_INTERNAL(in Entity entity) {

            if (entity.generation == Entity.GENERATION_ZERO) return false;

            if (this.world.currentSystemContext != null) {

                this.world.currentSystemContextFiltersUsed.arr[this.id] = true;
                this.world.currentSystemContextFiltersUsedAnyChanged = true;
                this.requests.Add(entity);
                return false;

            }

            if (this.forEachMode == true) {

                this.requests.Add(entity);
                return false;

            }

            var isExists = this.Contains_INTERNAL(entity.id);
            this.Update_INTERNAL(in entity);
            if (isExists == true) {

                return this.CheckRemove(in entity);

            } else {

                return this.CheckAdd(in entity);

            }

        }

        internal bool OnRemoveEntity(in Entity entity) {

            if (entity.generation == Entity.GENERATION_ZERO) return false;

            if (this.world.currentSystemContext != null) {

                this.world.currentSystemContextFiltersUsed.arr[this.id] = true;
                this.world.currentSystemContextFiltersUsedAnyChanged = true;
                //this.requestsRemoveEntity.TryAdd(entity.version, entity);
                this.requestsRemoveEntity.Add(entity);
                return false;

            }

            if (this.forEachMode == true) {

                //this.requestsRemoveEntity.TryAdd(entity.version, entity);
                this.requestsRemoveEntity.Add(entity);
                return false;

            }

            return this.Remove_INTERNAL(entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal void Update_INTERNAL(in Entity entity) {

            if (this.onVersionChangedOnly == true) {

                var idx = entity.id;
                this.dataVersions.arr[idx] = true;
                this.UpdateMinMaxAdd(idx);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal bool Add_INTERNAL(in Entity entity) {

            var idx = entity.id;
            ref var res = ref this.dataContains.arr[idx];
            if (res == false) {

                res = true;
                ++this.dataCount;
                this.UpdateMinMaxAdd(idx);

                if (this.predicateOnAdd != null) this.predicateOnAdd.Execute(in entity);
                return true;

            }

            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal bool Remove_INTERNAL(in Entity entity) {

            var idx = entity.id;
            ref var res = ref this.dataContains.arr[idx];
            if (res == true) {

                res = false;
                if (this.onVersionChangedOnly == true) this.dataVersions.arr[idx] = false;
                --this.dataCount;
                this.UpdateMinMaxRemove(idx);

                if (this.predicateOnRemove != null) this.predicateOnRemove.Execute(in entity);
                return true;

            }

            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int GetMaxEntityId() {
            return this.max;
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void UpdateMinMaxAdd(int idx) {

            if (idx < this.min) this.min = idx;
            if (idx > this.max) this.max = idx;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void UpdateMinMaxRemove(int idx) {

            if (idx == this.min && idx == this.max) {

                this.min = int.MaxValue;
                this.max = int.MinValue;
                return;

            }

            if (idx == this.min) {

                // Update new min (find next index)
                var changed = false;
                for (int i = idx; i < this.dataContains.Length; ++i) {

                    if (this.dataContains.arr[i] == true) {

                        this.min = i;
                        changed = true;
                        break;

                    }

                }

                if (changed == false) {

                    this.min = int.MaxValue;

                }

            }

            if (idx == this.max) {

                // Update new max (find prev index)
                var changed = false;
                for (int i = idx; i >= 0; --i) {

                    if (this.dataContains.arr[i] == true) {

                        this.max = i;
                        changed = true;
                        break;

                    }

                }

                if (changed == false) {

                    this.max = int.MinValue;

                }

            }

        }

        /*#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        private bool ContainsAllNodes(Entity entity) {
            
            for (int i = 0; i < this.nodesCount; ++i) {

                if (this.nodes[i].Execute(entity) == false) {

                    return false;

                }

            }

            return true;

        }*/

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private bool CheckAdd(in Entity entity) {

            // If entity doesn't exist in cache - try to add if entity's archetype fit with contains & notContains
            ref var entArchetype = ref this.world.currentState.storage.archetypes.Get(entity.id);
            if (entArchetype.Has(in this.archetypeContains) == false) return false;
            if (entArchetype.HasNot(in this.archetypeNotContains) == false) return false;

            this.Add_INTERNAL(in entity);

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private bool CheckRemove(in Entity entity) {

            // If entity already exists in cache - try to remove if entity's archetype doesn't fit with contains & notContains
            ref var entArchetype = ref this.world.currentState.storage.archetypes.Get(entity.id);
            var allContains = entArchetype.Has(in this.archetypeContains);
            var allNotContains = entArchetype.HasNot(in this.archetypeNotContains);
            if (allContains == true && allNotContains == true) return false;

            return this.Remove_INTERNAL(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsEquals(FilterData filter) {

            if (this.GetArchetypeContains() == filter.GetArchetypeContains() &&
                this.GetArchetypeNotContains() == filter.GetArchetypeNotContains() &&
                this.onVersionChangedOnly == filter.onVersionChangedOnly &&
                this.GetType() == filter.GetType()) {

                return true;

            }

            return false;

        }

        public override int GetHashCode() {

            var hashCode = this.GetType().GetHashCode()
                            ^ this.archetypeContains.GetHashCode() ^ this.archetypeNotContains.GetHashCode()
                            ^ this.sharedArchetypeContains.GetHashCode() ^ this.sharedArchetypeNotContains.GetHashCode()
                            ^ (this.onVersionChangedOnly == true ? 1 : 0);
            
            return hashCode;

        }

        public FilterData Push() {

            FilterData filter = null;
            return this.Push(ref filter);

        }

        public FilterData Push(ref FilterData filter, bool checkExist = true) {

            var world = Worlds.currentWorld;
            if (checkExist == false || world.HasFilter(this.id) == false) {

                var existsFilter = (this.onVersionChangedOnly == true ? null : world.GetFilterEquals(this));
                if (existsFilter != null) {

                    filter = existsFilter;
                    filter.AddAlias(this.name);
                    #if UNITY_EDITOR
                    filter.OnEditorFilterAddStackTrace(this.editorStackTraceFile[0], this.editorStackTraceLineNumber[0]);
                    #endif
                    this.Recycle();
                    return existsFilter;

                } else {

                    this.id = world.currentState.filters.AllocateNextId();

                    filter = this;
                    filter.setupVersioned?.Invoke(filter);
                    filter.setupVersioned = null;
                    world.currentState.filters.RegisterInAllArchetype(in this.archetypeContains);
                    world.currentState.filters.RegisterInAllArchetype(in this.archetypeNotContains);
                    world.Register(this);

                }

            } else {

                UnityEngine.Debug.LogWarning(string.Format("World #{0} already has filter {1}!", world.id, this));

            }

            return this;

        }

        public FilterData SetOnEntityAdd<T>(T predicate) where T : class, IFilterAction {

            this.predicateOnAdd = predicate;

            return this;

        }

        public FilterData SetOnEntityRemove<T>(T predicate) where T : class, IFilterAction {

            this.predicateOnRemove = predicate;

            return this;

        }

        public FilterData OnVersionChangedOnly() {

            this.onVersionChangedOnly = true;

            return this;

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public FilterData With(IStructComponentBase[] components) {

            for (int i = 0; i < components.Length; ++i) {

                var type = components[i].GetType();
                if (ComponentTypesRegistry.allTypeId.TryGetValue(type, out var bit) == true) {
                
                    this.archetypeContains.AddBit(bit);
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
                
                    this.archetypeNotContains.AddBit(bit);
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
            this.setupVersioned += (f) => WorldUtilities.SetComponentAsFilterVersioned<TComponent>(f.onVersionChangedOnly);
            this.archetypeContains.Add<TComponent>();
            #if UNITY_EDITOR
            this.AddTypeToEditorWith<TComponent>();
            #endif
            return this;

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public FilterData WithoutStructComponent<TComponent>() where TComponent : struct, IStructComponentBase {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.setupVersioned += (f) => WorldUtilities.SetComponentAsFilterVersioned<TComponent>(f.onVersionChangedOnly);
            this.archetypeNotContains.Add<TComponent>();
            #if UNITY_EDITOR
            this.AddTypeToEditorWithout<TComponent>();
            #endif
            return this;

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public FilterData WithSharedComponent<TComponent>() where TComponent : struct, IStructComponentBase {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.sharedArchetypeContains.Add<TComponent>();
            this.hasShared = true;
            #if UNITY_EDITOR
            this.AddTypeToEditorWithShared<TComponent>();
            #endif
            return this;

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public FilterData WithoutSharedComponent<TComponent>() where TComponent : struct, IStructComponentBase {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.sharedArchetypeNotContains.Add<TComponent>();
            this.hasShared = true;
            #if UNITY_EDITOR
            this.AddTypeToEditorWithoutShared<TComponent>();
            #endif
            return this;

        }

        public static FilterData Create(string customName = null) {

            var nextId = Worlds.currentWorld.currentState.filters.GetNextId();
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