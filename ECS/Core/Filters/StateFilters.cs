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

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class FiltersStorage : IPoolableRecycle {

        internal BufferArray<FilterData> filters;
        private bool freeze;
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

        public bool HasInFilters<TComponent>() {

            return this.allFiltersArchetype.Has<TComponent>();

        }

        public void RegisterInAllArchetype(in Archetype archetype) {

            this.allFiltersArchetype.Add(in archetype);

        }
        
        void IPoolableRecycle.OnRecycle() {

            this.nextId = default;
            this.freeze = default;
            
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

            this.freeze = freeze;

        }

        public BufferArray<FilterData> GetData() {

            return this.filters;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ref FilterData Get(in int id) {

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
                
                item.Recycle();
                
            }

        }

        public void CopyFrom(FiltersStorage other) {

            this.nextId = other.nextId;
            this.allFiltersArchetype = other.allFiltersArchetype;
            
            /*if (this.filters != null) {

                for (int i = 0, count = this.filters.Count; i < count; ++i) {
                    
                    this.filters[i].Recycle();

                }

                PoolList<IFilterBase>.Recycle(ref this.filters);
                
            }
            this.filters = PoolList<IFilterBase>.Spawn(other.filters.Count);

            for (int i = 0, count = other.filters.Count; i < count; ++i) {
                
                var copy = other.filters[i].Clone();
                this.filters.Add(copy);
                UnityEngine.Debug.Log("Copy filter: " + i + " :: " + other.filters[i].id + " >> " + this.filters.Count + " (" + copy.id + ")");
                
            }*/

            ArrayUtils.Copy(other.filters, ref this.filters, new FilterCopy());
            
            /*if (this.freeze == true) {

                // Just copy if filters storage is in freeze mode
                if (this.filters.arr == null) {

                    this.filters = PoolArray<FilterData>.Spawn(other.filters.Length);

                }

                for (int i = 0, count = other.filters.Length; i < count; ++i) {

                    if (other.filters.arr[i] == null && this.filters.arr[i] == null) {
                        
                        continue;
                        
                    }

                    if (other.filters.arr[i] == null && this.filters.arr[i] != null) {

                        this.filters.arr[i].Recycle();
                        this.filters.arr[i] = null;
                        continue;
                        
                    }

                    if (i >= this.filters.Length || this.filters.arr[i] == null) {

                        this.Register(other.filters.arr[i].Clone());

                    } else {

                        this.filters.arr[i].CopyFrom(other.filters.arr[i]);

                    }

                }

            } else {
                
                // Filters storage is not in a freeze mode, so it is an active state filters
                if (this.filters.arr == null && other.filters.arr != null) this.filters = PoolArray<FilterData>.Spawn(other.filters.Length);
                for (int i = 0, count = other.filters.Length; i < count; ++i) {

                    if (other.filters.arr[i] == null && this.filters.arr[i] == null) {
                        
                        continue;
                        
                    }

                    if (other.filters.arr[i] == null && this.filters.arr[i] != null) {

                        this.filters.arr[i].Recycle();
                        this.filters.arr[i] = null;
                        continue;
                        
                    }

                    if (i >= this.filters.Length || this.filters.arr[i] == null && other.filters.arr[i] != null) {

                        this.Register(other.filters.arr[i].Clone());

                    } else {

                        this.filters.arr[i].CopyFrom(other.filters.arr[i]);

                    }

                }
                
            }

            this.freeze = other.freeze;*/

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct FilterEnumerator : IEnumerator<Entity> {
        
        /*public struct EntityEnumerator : IEnumerator<Entity> {

            private BufferArray<Entity> bufferArray;
            private int index;
            
            public EntityEnumerator(BufferArray<Entity> bufferArray) {

                this.bufferArray = bufferArray;
                this.index = -1;

            }

            object System.Collections.IEnumerator.Current {
                get {
                    throw new AllocationException();
                }
            }

            #if ECS_COMPILE_IL2CPP_OPTIONS
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
            #endif
            Entity IEnumerator<Entity>.Current {
                [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get {
                    return this.bufferArray.arr[this.index];
                }
            }
            
            #if ECS_COMPILE_IL2CPP_OPTIONS
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
            #endif
            public Entity Current {
                [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get {
                    return this.bufferArray.arr[this.index];
                }
            }

            #if ECS_COMPILE_IL2CPP_OPTIONS
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
            #endif
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() {

                ++this.index;
                return this.index < this.bufferArray.Length;

            }

            public void Reset() {}

            public void Dispose() {}
            
        }*/
        
        private readonly FilterData set;
        //private EntityEnumerator setEnumerator;
        private int max;
        private int index;
            
        internal FilterEnumerator(FilterData set) {
                
            this.set = set;
            this.set.GetBounds(out this.index, out this.max);
            --this.index;
            if (this.index > this.max) {

                this.index = 0;
                this.max = 0;
                
            }
            //this.setEnumerator = new EntityEnumerator(this.arr);
            this.set.SetForEachMode(true);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Dispose() {

            //PoolArray<Entity>.Recycle(this.arr);
            this.set.SetForEachMode(false);

        }
 
        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() {
            
            do {

                ++this.index;
                if (this.index > this.max) return false;

            } while (this.set.dataContains.arr[this.index] == false);
            
            return true;
            
            //return this.setEnumerator.MoveNext();
            
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        Entity IEnumerator<Entity>.Current {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                return this.set.world.currentState.storage.cache.arr[this.index]; //this.arr.arr[this.index]; //this.setEnumerator.Current;
            }
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public ref Entity Current {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                return ref this.set.world.currentState.storage.cache.arr[this.index]; //this.arr.arr[this.index]; //this.setEnumerator.Current;
            }
        }
 
        System.Object System.Collections.IEnumerator.Current {
            get {
                throw new AllocationException();
            }
        }
 
        void System.Collections.IEnumerator.Reset() {
                
        }
    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct Filter {

        private int id;
        internal World world;
        private FilterData temp;

        public static Filter Empty => new Filter();

        public int Count => this.world.GetFilter(this.id).Count;

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int GetMaxEntityId() {

            return this.world.GetFilter(this.id).GetMaxEntityId();

        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool IsForEntity(int id) {

            return this.world.GetFilter(this.id).IsForEntity(id);

        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ApplyAllRequests() {
            
            this.world.GetFilter(this.id).ApplyAllRequests();
            
        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public FilterEnumerator GetEnumerator() {

            return new FilterEnumerator(this.world.GetFilter(this.id));

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Filter SetOnEntityAdd<T>(T predicate) where T : class, IFilterAction {

            this.temp.SetOnEntityAdd(predicate);
            return this;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Filter SetOnEntityRemove<T>(T predicate) where T : class, IFilterAction {
            
            this.temp.SetOnEntityRemove(predicate);
            return this;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool IsAlive() {

            return Worlds.currentWorld.HasFilter(this.id);

        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Filter Push() {

            var filter = new Filter();
            return this.Push(ref filter);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Filter Push(ref Filter filter) {

            FilterData filterData = null;
            this.temp.Push(ref filterData);
            filter.id = filterData.id;
            filter.world = Worlds.currentWorld;
            filter.temp = null;
            return filter;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Filter WithComponent<TComponent>() where TComponent : class, IComponent {

            this.temp.WithComponent<TComponent>();
            return this;

        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Filter WithoutComponent<TComponent>() where TComponent : class, IComponent {

            this.temp.WithoutComponent<TComponent>();
            return this;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Filter WithStructComponent<TComponent>() where TComponent : struct, IStructComponent {

            this.temp.WithStructComponent<TComponent>();
            return this;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Filter WithoutStructComponent<TComponent>() where TComponent : struct, IStructComponent {
            
            this.temp.WithoutStructComponent<TComponent>();
            return this;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Filter Create(string customName = null) {

            var filter = FilterData.Create(customName);
            return new Filter() {
                id = filter.id,
                world = Worlds.currentWorld,
                temp = filter
            };
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BufferArray<Entity> ToArray() {
            
            return this.world.GetFilter(this.id).ToArray();
            
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
        private BufferArray<IFilterNode> nodes;
        private Archetype archetypeContains;
        private Archetype archetypeNotContains;
        private int nodesCount;
        internal BufferArray<bool> dataContains;
        //private BufferArray<Entity> data;
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
        private List<IFilterNode> tempNodesCustom;
        internal BufferArray<string> aliases;
        private int dataCount;

        private IFilterAction predicateOnAdd;
        private IFilterAction predicateOnRemove;

        public bool isPooled;

        #if UNITY_EDITOR
        private string[] editorTypes;
        private string[] editorStackTraceFile;
        private int[] editorStackTraceLineNumber;
        #endif

        public FilterData() {}

        internal FilterData(string name) {

            this.AddAlias(name);

        }

        public void Clear() {
            
            for (int i = 0; i < this.dataContains.Length; ++i) {

                if (this.dataContains.arr[i] == true) {

                    this.Remove_INTERNAL(in this.world.currentState.storage.cache.arr[i]);

                }
                
            }
            
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

        }
        
        internal void OnEntityCreate(in Entity entity) {

            ArrayUtils.Resize(entity.id, ref this.dataContains);

        }

        internal void OnEntityDestroy(in Entity entity) {

            ArrayUtils.Resize(entity.id, ref this.dataContains);
            
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
            ArrayUtils.Resize(idx, ref this.aliases, resizeWithOffset: false);
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

            const int frameIndex = 2;
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
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void GetBounds(out int min, out int max) {

            min = this.min;
            max = this.max;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BufferArray<Entity> ToArray() {

            var data = PoolArray<Entity>.Spawn(this.dataCount >= this.requestsRemoveEntity.Count ? this.dataCount - this.requestsRemoveEntity.Count : 0);
            for (int i = this.min, k = 0; i <= this.max; ++i) {
                if (this.dataContains.arr[i] == true) {
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
            this.nodes = PoolArray<IFilterNode>.Spawn(FilterData.NODES_CAPACITY);
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
            
            #if UNITY_EDITOR
            this.editorTypes = null;
            this.editorStackTraceFile = null;
            this.editorStackTraceLineNumber = null;
            #endif
            
        }

        void IPoolableRecycle.OnRecycle() {
            
            this.isPooled = true;

            PoolArray<bool>.Recycle(ref this.dataContains);
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

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool IsForEntity(in int entityId) {

            ref var previousArchetype = ref this.world.currentState.storage.archetypes.GetPrevious(in entityId);
            if (previousArchetype.ContainsAll(in this.archetypeContains) == true && previousArchetype.NotContains(in this.archetypeNotContains) == true) return true;
            
            ref var currentArchetype = ref this.world.currentState.storage.archetypes.Get(in entityId);
            if (currentArchetype.ContainsAll(in this.archetypeContains) == true && currentArchetype.NotContains(in this.archetypeNotContains) == true) return true;

            return false;

        }

        public int GetNodesCount() {

            return this.nodesCount;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Archetype GetArchetypeContains() {

            return this.archetypeContains;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Archetype GetArchetypeNotContains() {

            return this.archetypeNotContains;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetForEachMode(bool state) {

            this.forEachMode = state;
            if (state == false) {

                if (this.world.currentSystemContext == null) {

                    this.ApplyAllRequests();

                }
                
            }

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                return this.dataCount;
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Contains(in Entity entity) {

            return this.world.GetFilter(this.id).Contains_INTERNAL(in entity.id);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool Contains_INTERNAL(in int entityId) {

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

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool OnUpdate(in Entity entity) {

            return this.OnUpdate_INTERNAL(in entity);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool OnAddComponent(in Entity entity) {

            return this.OnUpdate_INTERNAL(in entity);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool OnRemoveComponent(in Entity entity) {

            return this.OnUpdate_INTERNAL(in entity);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool OnUpdateForced_INTERNAL(in Entity entity) {
            
            if (entity.generation == Entity.GENERATION_ZERO) return false;

            var isExists = this.Contains_INTERNAL(in entity.id);
            if (isExists == true) {

                return this.CheckRemove(in entity);

            } else {

                return this.CheckAdd(in entity);

            }

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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

            var isExists = this.Contains_INTERNAL(in entity.id);
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

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Add_INTERNAL(in Entity entity) {

            var idx = entity.id;
            ref var res = ref this.dataContains.arr[idx];
            if (res == false) {

                res = true;
                ++this.dataCount;
                this.UpdateMinMaxAdd(idx);

                if (this.predicateOnAdd != null) this.predicateOnAdd.Execute(in entity);

            }

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Remove_INTERNAL(in Entity entity) {

            var idx = entity.id;
            ref var res = ref this.dataContains.arr[idx];
            if (res == true) {

                res = false;
                --this.dataCount;
                this.UpdateMinMaxRemove(idx);
                
                if (this.predicateOnRemove != null) this.predicateOnRemove.Execute(in entity);
                return true;

            }

            return false;

        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int GetMaxEntityId() {
            return this.max;
        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void UpdateMinMaxAdd(int idx) {

            if (idx < this.min) this.min = idx;
            if (idx > this.max) this.max = idx;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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

        /*[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool ContainsAllNodes(Entity entity) {
            
            for (int i = 0; i < this.nodesCount; ++i) {

                if (this.nodes[i].Execute(entity) == false) {

                    return false;

                }

            }

            return true;

        }*/

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool CheckAdd(in Entity entity) {

            // If entity doesn't exist in cache - try to add if entity's archetype fit with contains & notContains
            ref var entArchetype = ref this.world.currentState.storage.archetypes.Get(in entity.id);
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
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool CheckRemove(in Entity entity) {

            // If entity already exists in cache - try to remove if entity's archetype doesn't fit with contains & notContains
            ref var entArchetype = ref this.world.currentState.storage.archetypes.Get(in entity.id);
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

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool IsEquals(FilterData filter) {

            if (this.GetArchetypeContains() == filter.GetArchetypeContains() &&
                this.GetArchetypeNotContains() == filter.GetArchetypeNotContains() &&
                this.GetType() == filter.GetType() &&
                this.GetNodesCount() == filter.GetNodesCount()) {

                return true;

            }

            return false;

        }

        public override int GetHashCode() {

            var hashCode = this.GetType().GetHashCode() ^ this.archetypeContains.GetHashCode() ^ this.archetypeNotContains.GetHashCode();
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

                this.tempNodes.AddRange(this.tempNodesCustom);
                var arr = this.tempNodes.OrderBy(x => x.GetType().GetHashCode()).ToArray();

                if (this.nodes.arr != null) PoolArray<IFilterNode>.Recycle(ref this.nodes);
                this.nodes = BufferArray<IFilterNode>.From(arr);
                this.nodesCount = this.nodes.Length;
                this.tempNodes.Clear();
                this.tempNodesCustom.Clear();
                
                var existsFilter = world.GetFilterEquals(this);
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

            this.tempNodesCustom.Add(filter);
            return this;

        }

        public FilterData Custom<TFilter>() where TFilter : class, IFilterNode, new() {

            var filter = new TFilter();
            this.tempNodesCustom.Add(filter);
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

        public FilterData WithComponent<TComponent>() where TComponent : class, IComponent {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.archetypeContains.Add<TComponent>();
            #if UNITY_EDITOR
            this.AddTypeToEditorWith<TComponent>();
            #endif
            return this;

        }
        
        public FilterData WithoutComponent<TComponent>() where TComponent : class, IComponent {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.archetypeNotContains.Add<TComponent>();
            #if UNITY_EDITOR
            this.AddTypeToEditorWithout<TComponent>();
            #endif
            return this;

        }

        public FilterData WithStructComponent<TComponent>() where TComponent : struct, IStructComponent {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.archetypeContains.Add<TComponent>();
            #if UNITY_EDITOR
            this.AddTypeToEditorWith<TComponent>();
            #endif
            return this;

        }

        public FilterData WithoutStructComponent<TComponent>() where TComponent : struct, IStructComponent {

            WorldUtilities.SetComponentTypeId<TComponent>();
            this.archetypeNotContains.Add<TComponent>();
            #if UNITY_EDITOR
            this.AddTypeToEditorWithout<TComponent>();
            #endif
            return this;

        }

        public static FilterData Create(string customName = null) {

            var nextId = Worlds.currentWorld.currentState.filters.GetNextId();
            var f = PoolFilters.Spawn<FilterData>();
            f.id = nextId;
            f.AddAlias(customName);
            f.tempNodes = new List<IFilterNode>();
            f.tempNodesCustom = new List<IFilterNode>();
            #if UNITY_EDITOR
            f.OnEditorFilterCreate();
            #endif
            return f;

        }

        public override string ToString() {

            return "Name: " + string.Join("/", this.aliases.arr, 0, this.aliases.Length) + " (#" + this.id.ToString() + ")";

        }

    }

}