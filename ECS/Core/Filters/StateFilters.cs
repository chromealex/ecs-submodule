#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using System.Collections.Generic;
using System.Linq;

namespace ME.ECS {

    using ME.ECS.Collections;

    public interface IFilterNode {

        bool Execute(Entity entity);

    }

    public interface IFilterAction {

        void Execute(in Entity entity);

    }

    public partial class World {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void UpdateFilterByStructComponentVersioned<T>(in Entity entity) where T : struct, IStructComponent {

            var containsFilters = this.currentState.filters.filtersTree.GetFiltersContainsForVersioned<T>();
            for (int i = 0; i < containsFilters.Length; ++i) {

                var filterId = containsFilters.arr[i];
                var filter = this.GetFilter(filterId);
                if (filter.IsForEntity(entity.id) == false) continue;
                filter.OnUpdate(in entity);

            }

            var notContainsFilters = this.currentState.filters.filtersTree.GetFiltersNotContainsForVersioned<T>();
            for (int i = 0; i < notContainsFilters.Length; ++i) {

                var filterId = notContainsFilters.arr[i];
                var filter = this.GetFilter(filterId);
                if (filter.IsForEntity(entity.id) == false) continue;
                filter.OnUpdate(in entity);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void UpdateFilterByStructComponent<T>(in Entity entity) where T : struct, IStructComponent {

            var containsFilters = this.currentState.filters.filtersTree.GetFiltersContainsFor<T>();
            for (int i = 0; i < containsFilters.Length; ++i) {

                var filterId = containsFilters.arr[i];
                var filter = this.GetFilter(filterId);
                if (filter.IsForEntity(entity.id) == false) continue;
                filter.OnUpdate(in entity);

            }

            var notContainsFilters = this.currentState.filters.filtersTree.GetFiltersNotContainsFor<T>();
            for (int i = 0; i < notContainsFilters.Length; ++i) {

                var filterId = notContainsFilters.arr[i];
                var filter = this.GetFilter(filterId);
                if (filter.IsForEntity(entity.id) == false) continue;
                filter.OnUpdate(in entity);

            }

        }

        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void UpdateFilterByComponent<T>(in Entity entity) where T : class, IComponent {

            var containsFilters = this.currentState.filters.filtersTree.GetFiltersContainsFor<T>();
            for (int i = 0; i < containsFilters.Length; ++i) {

                var filterId = containsFilters.arr[i];
                var filter = this.GetFilter(filterId);
                if (filter.IsForEntity(entity.id) == false) continue;
                filter.OnUpdate(in entity);

            }

            var notContainsFilters = this.currentState.filters.filtersTree.GetFiltersNotContainsFor<T>();
            for (int i = 0; i < notContainsFilters.Length; ++i) {

                var filterId = notContainsFilters.arr[i];
                var filter = this.GetFilter(filterId);
                if (filter.IsForEntity(entity.id) == false) continue;
                filter.OnUpdate(in entity);

            }

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

            //ArrayUtils.Resize(this.id, ref FiltersDirectCache.dic);
            ref var dic = ref FiltersDirectCache.dic.arr[this.id];
            if (dic.arr != null) {

                for (int i = 0; i < dic.Length; ++i) {

                    if (dic.arr[i] == false) continue;
                    var filterId = i + 1;
                    var filter = this.GetFilter(filterId);
                    filter.OnEntityDestroy(in entity);
                    if (filter.IsForEntity(entity.id) == false) continue;
                    filter.OnRemoveEntity(in entity);

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

        internal FiltersTree filtersTree;
        internal BufferArray<FilterData> filters;
        private int nextId;
        internal Archetype allFiltersArchetype;

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

            return this.allFiltersArchetype.Count;

        }

        public bool HasInAnyFilter<TComponent>() {

            return this.allFiltersArchetype.Has<TComponent>();

        }

        public void RegisterInAllArchetype(in Archetype archetype) {

            this.allFiltersArchetype.Add(in archetype);

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
            this.allFiltersArchetype = other.allFiltersArchetype;
            this.filtersTree.CopyFrom(other.filtersTree);
            ArrayUtils.Copy(other.filters, ref this.filters, new FilterCopy());

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
        //private readonly BufferArray<bool> dataContains;
        //private readonly BufferArray<bool> dataVersions;
        private readonly BufferArray<Entity> cache;

        internal FilterEnumerator(FilterData set) {

            this.set = set;
            this.set.GetBounds(out this.index, out this.max);
            --this.index;
            if (this.index > this.max) {

                this.index = 0;
                this.max = 0;

            }

            this.cache = this.set.world.currentState.storage.cache;
            this.onVersionChangedOnly = this.set.onVersionChangedOnly;
            //this.dataContains = this.set.dataContains;
            //this.dataVersions = this.set.dataVersions;
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

            //this.set.dataContains = this.dataContains;
            //this.set.dataVersions = this.dataVersions;
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

            do {

                ++this.index;
                if (this.index > this.max) return false;
                if (this.set.dataContains.arr[this.index] != true) continue;
                if (this.onVersionChangedOnly == true && this.set.dataVersions.arr[this.index] == false) continue;
                
                break;

            } while (true);

            if (this.onVersionChangedOnly == true) {

                this.set.dataVersions.arr[this.index] = false;

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
                return this.cache.arr[this.index];
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
                return ref this.cache.arr[this.index];
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

        public int Count => this.world.GetFilter(this.id).Count;

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
        public Filter Push(ref Filter filter) {

            FilterData filterData = null;
            this.temp.Push(ref filterData);
            filter.id = filterData.id;
            filter.world = Worlds.currentWorld;
            filter.temp = null;
            return filter;

        }

        public Filter OnVersionChangedOnly() {

            this.temp.OnVersionChangedOnly();
            return this;

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Filter WithStructComponent<TComponent>() where TComponent : struct, IStructComponent {

            this.temp.WithStructComponent<TComponent>();
            return this;

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Filter WithoutStructComponent<TComponent>() where TComponent : struct, IStructComponent {

            this.temp.WithoutStructComponent<TComponent>();
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Filter With<TComponent>() where TComponent : struct, IStructComponent {

            this.temp.WithStructComponent<TComponent>();
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Filter Without<TComponent>() where TComponent : struct, IStructComponent {

            this.temp.WithoutStructComponent<TComponent>();
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Filter Create(string customName = null) {

            var filter = FilterData.Create(customName);
            return new Filter() {
                id = filter.id,
                world = Worlds.currentWorld,
                temp = filter
            };

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
        public BufferArray<Entity> ToArray() {

            return this.world.GetFilter(this.id).ToArray();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void UseVersioned() {

            this.world.GetFilter(this.id).UseVersioned();

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
        private BufferArray<IFilterNode> nodes;
        internal Archetype archetypeContains;
        internal Archetype archetypeNotContains;
        private int nodesCount;
        internal BufferArray<bool> dataContains;
        internal BufferArray<bool> dataVersions;
        
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

        private List<IFilterNode> tempNodes;
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

        public void Clear() {

            for (int i = 0; i < this.dataContains.Length; ++i) {

                if (this.dataContains.arr[i] == true) {

                    this.Remove_INTERNAL(in this.world.currentState.storage.cache.arr[i]);

                }

            }

            ArrayUtils.Clear(this.dataVersions);
            ArrayUtils.Clear(this.dataContains);
            this.dataCount = 0;

        }

        public void OnDeserialize(int lastEntityId) {

            this.SetEntityCapacity(lastEntityId);

        }

        public void Recycle() {

            if (this.isPooled == false) PoolFilters.Recycle(this);

        }

        internal void SetEntityCapacity(int capacity) {

            ArrayUtils.Resize(capacity, ref this.dataContains);
            if (this.onVersionChangedOnly == true) ArrayUtils.Resize(capacity, ref this.dataVersions);

        }

        internal void OnEntityCreate(in Entity entity) {

            ArrayUtils.Resize(entity.id, ref this.dataContains);
            if (this.onVersionChangedOnly == true) ArrayUtils.Resize(entity.id, ref this.dataVersions);

        }

        internal void OnEntityDestroy(in Entity entity) {

            ArrayUtils.Resize(entity.id, ref this.dataContains);
            if (this.onVersionChangedOnly == true) ArrayUtils.Resize(entity.id, ref this.dataVersions);

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

        public void AddTypeToEditorWith<TComponent>() {

            var idx = (this.editorTypes != null ? this.editorTypes.Length : 0);
            System.Array.Resize(ref this.editorTypes, idx + 1);
            this.editorTypes[idx] = "W<" + typeof(TComponent).Name + ">";

        }

        public void AddTypeToEditorWithout<TComponent>() {

            var idx = (this.editorTypes != null ? this.editorTypes.Length : 0);
            System.Array.Resize(ref this.editorTypes, idx + 1);
            this.editorTypes[idx] = "WO<" + typeof(TComponent).Name + ">";

        }

        public void OnEditorFilterCreate() {

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

            if (this.onVersionChangedOnly == true) ArrayUtils.Clear(this.dataVersions);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public BufferArray<Entity> ToArray() {

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

            var data = PoolArray<Entity>.Spawn(customCount);
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
            this.nodes = default;
            this.dataContains = PoolArray<bool>.Spawn(FilterData.ENTITIES_CAPACITY);
            this.dataCount = 0;

            this.id = default;
            if (this.aliases.arr != null) PoolArray<string>.Recycle(ref this.aliases);
            this.nodesCount = default;
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

            PoolArray<bool>.Recycle(ref this.dataContains);
            if (this.onVersionChangedOnly == true) PoolArray<bool>.Recycle(ref this.dataVersions);
            PoolArray<IFilterNode>.Recycle(ref this.nodes);
            //PoolArray<Entity>.Recycle(ref this.requestsRemoveEntity);
            //PoolArray<Entity>.Recycle(ref this.requests);
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
            this.nodesCount = default;
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

            ref var previousArchetype = ref this.world.currentState.storage.archetypes.GetPrevious(entityId);
            if (previousArchetype.value.Has(in this.archetypeContains.value) == true && previousArchetype.value.HasNot(in this.archetypeNotContains.value) == true) return true;

            ref var currentArchetype = ref this.world.currentState.storage.archetypes.Get(entityId);
            if (currentArchetype.value.Has(in this.archetypeContains.value) == true && currentArchetype.value.HasNot(in this.archetypeNotContains.value) == true) return true;

            return false;

        }

        public int GetNodesCount() {

            return this.nodesCount;

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
            this.nodesCount = other.nodesCount;

            this.onVersionChangedOnly = other.onVersionChangedOnly;

            this.predicateOnAdd = other.predicateOnAdd;
            this.predicateOnRemove = other.predicateOnRemove;

            this.archetypeContains = other.archetypeContains;
            this.archetypeNotContains = other.archetypeNotContains;

            ArrayUtils.Copy(in other.nodes, ref this.nodes);
            ArrayUtils.Copy(in other.dataContains, ref this.dataContains);

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
            if (entArchetype.ContainsAll(in this.archetypeContains) == false) return false;
            if (entArchetype.NotContains(in this.archetypeNotContains) == false) return false;

            if (this.nodesCount > 0) {

                for (int i = 0; i < this.nodesCount; ++i) {

                    if (this.nodes.arr[i].Execute(entity) == false) {

                        return false;

                    }

                }

            }

            this.Add_INTERNAL(in entity);

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private bool CheckRemove(in Entity entity) {

            // If entity already exists in cache - try to remove if entity's archetype doesn't fit with contains & notContains
            ref var entArchetype = ref this.world.currentState.storage.archetypes.Get(entity.id);
            var allContains = entArchetype.ContainsAll(in this.archetypeContains);
            var allNotContains = entArchetype.NotContains(in this.archetypeNotContains);
            if (allContains == true && allNotContains == true) return false;

            if (this.nodesCount > 0) {

                var isFail = false;
                for (int i = 0; i < this.nodesCount; ++i) {

                    if (this.nodes.arr[i].Execute(entity) == false) {

                        isFail = true;
                        break;

                    }

                }

                if (isFail == false) return false;

            }

            return this.Remove_INTERNAL(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsEquals(FilterData filter) {

            if (this.GetArchetypeContains() == filter.GetArchetypeContains() &&
                this.GetArchetypeNotContains() == filter.GetArchetypeNotContains() &&
                this.onVersionChangedOnly == filter.onVersionChangedOnly &&
                this.GetType() == filter.GetType() &&
                this.GetNodesCount() == filter.GetNodesCount()) {

                return true;

            }

            return false;

        }

        public override int GetHashCode() {

            var hashCode = this.GetType().GetHashCode() ^ this.archetypeContains.GetHashCode() ^ this.archetypeNotContains.GetHashCode() ^
                           (this.onVersionChangedOnly == true ? 1 : 0);
            for (int i = 0; i < this.nodesCount; ++i) {

                hashCode ^= this.nodes.arr[i].GetType().GetHashCode();

            }

            return hashCode;

        }

        public FilterData Push() {

            FilterData filter = null;
            return this.Push(ref filter);

        }

        public FilterData Push(ref FilterData filter) {

            var world = Worlds.currentWorld;
            if (world.HasFilter(this.id) == false) {

                if (this.tempNodes.Count > 0) {

                    var arr = this.tempNodes.OrderBy(x => x.GetType().GetHashCode()).ToArray();
                    if (this.nodes.arr != null) PoolArray<IFilterNode>.Recycle(ref this.nodes);
                    this.nodes = BufferArray<IFilterNode>.From(arr);
                    this.nodesCount = this.nodes.Length;

                } else {

                    this.nodesCount = 0;

                }
                PoolList<IFilterNode>.Recycle(ref this.tempNodes);

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

        public FilterData Custom(IFilterNode filter) {

            this.tempNodes.Add(filter);
            return this;

        }

        public FilterData Custom<TFilter>() where TFilter : class, IFilterNode, new() {

            var filter = new TFilter();
            this.tempNodes.Add(filter);
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
        public FilterData WithStructComponent<TComponent>() where TComponent : struct, IStructComponent {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.setupVersioned += (f) => WorldUtilities.SetComponentVersioned<TComponent>(f.onVersionChangedOnly);
            this.archetypeContains.Add<TComponent>();
            #if UNITY_EDITOR
            this.AddTypeToEditorWith<TComponent>();
            #endif
            return this;

        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public FilterData WithoutStructComponent<TComponent>() where TComponent : struct, IStructComponent {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.setupVersioned += (f) => WorldUtilities.SetComponentVersioned<TComponent>(f.onVersionChangedOnly);
            this.archetypeNotContains.Add<TComponent>();
            #if UNITY_EDITOR
            this.AddTypeToEditorWithout<TComponent>();
            #endif
            return this;

        }

        public static FilterData Create(string customName = null) {

            var nextId = Worlds.currentWorld.currentState.filters.GetNextId();
            var f = PoolFilters.Spawn<FilterData>();
            f.setupVersioned = null;
            f.id = nextId;
            f.AddAlias(customName);
            f.tempNodes = PoolList<IFilterNode>.Spawn(0);
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