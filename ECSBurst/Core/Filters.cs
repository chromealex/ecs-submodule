
namespace ME.ECSBurst {
    
    using Collections;

    public struct Filters {

        public Unity.Collections.NativeList<Filter> filters;

        public void Initialize() {
            
            this.filters = new Unity.Collections.NativeList<Filter>(Unity.Collections.Allocator.Persistent);
            
        }
        
        public void Dispose() {

            this.filters.Dispose();

        }
        
        public Filter Add(ref Filter filter) {

            for (int i = 0; i < this.filters.Length; ++i) {

                if (this.filters[i].IsEquals(in filter) == true) {
                    
                    return this.filters[i];
                    
                }
                
            }

            filter.id = this.filters.Length;
            filter.entities = PoolArrayNative<byte>.Spawn(10);
            this.filters.Add(filter);
            return filter;

        }

        public void OnBeforeAddComponent<T>(in Entity entity) {
            
            for (int i = 0; i < this.filters.Length; ++i) {
                
                this.filters[i].AddEntityCheckComponent<T>(in entity);
                
            }

        }

        public void OnBeforeRemoveComponent<T>(in Entity entity) {
            
            for (int i = 0; i < this.filters.Length; ++i) {
                
                this.filters[i].RemoveEntityCheckComponent<T>(in entity);
                
            }

        }

        public void OnBeforeEntityDestroy(in Entity entity) {

            for (int i = 0; i < this.filters.Length; ++i) {
                
                this.filters[i].RemoveEntity(in entity);
                
            }

        }

    }

    public unsafe struct Filter {

        public struct Enumerator : System.Collections.Generic.IEnumerator<Entity> {

            private Filter filter;
            private int index;
            private NativeBufferArray<Entity> cache;

            public Enumerator(Filter filter) {
                
                this.filter = filter;
                this.index = -1;
                this.cache = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<Storage>((void*)filter.storage).cache;

            }

            public bool MoveNext() {

                ++this.index;
                while (this.index < this.filter.entities.Length) {

                    var state = this.filter.entities[this.index];
                    if (state == 0) {
                        ++this.index;
                        continue;
                    }
                    return true;
                    
                }
                
                return false;
                
            }

            object System.Collections.IEnumerator.Current => throw new AllocationException();

            Entity System.Collections.Generic.IEnumerator<Entity>.Current => throw new AllocationException();

            public ref Entity Current => ref this.cache[this.index];

            public void Reset() {
                
                this.index = -1;
                
            }

            public void Dispose() {

                this.index = default;
                this.filter = default;
                this.cache = default;

            }

        }

        public int id;
        [Unity.Collections.LowLevel.Unsafe.NativeDisableUnsafePtrRestrictionAttribute]
        public System.IntPtr storage;
        public NativeBufferArray<byte> entities; // 0 - not contains, 1 - contains
        public Archetype contains;
        public Archetype notContains;

        #region Internal API
        public void AddEntityCheckComponent<T>(in Entity entity) {

            ref var storage = ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<Storage>((void*)this.storage);
            var arch = storage.archetypes.Get(in entity);
            if (arch.Has(this.contains) == false ||
                arch.HasNot(this.notContains) == false) {

                arch.Add<T>();
                
                if (arch.Has(this.contains) == true &&
                    arch.HasNot(this.notContains) == true) {

                    this.entities[entity.id] = 1;

                }

            }

        }

        public void RemoveEntityCheckComponent<T>(in Entity entity) {
            
            ref var storage = ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<Storage>((void*)this.storage);
            var arch = storage.archetypes.Get(in entity);
            if (arch.Has(this.contains) == true &&
                arch.HasNot(this.notContains) == true) {

                arch.Subtract<T>();
                
                if (arch.Has(this.contains) == false ||
                    arch.HasNot(this.notContains) == false) {

                    this.entities[entity.id] = 0;

                }

            }

        }

        public void RemoveEntity(in Entity entity) {
            
            this.entities[entity.id] = 0;
            
        }
        #endregion

        #region Public API
        public Filter With<T>() {
            
            this.contains.Add<T>();
            return this;
            
        }

        public Filter Without<T>() {
            
            this.notContains.Add<T>();
            return this;
            
        }

        public Enumerator GetEnumerator() {
            
            return new Enumerator(this);
            
        }

        public Filter Push(ref StateStruct state) {

            Filter _ = default;
            return this.Push(ref state, ref _);
            
        }

        public Filter Push(ref StateStruct state, ref Filter variable) {

            this.storage = (System.IntPtr)Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AddressOf(ref state.storage);
            variable = state.filters.Add(ref this);
            return variable;

        }
        
        public static Filter Create() {
            
            var filter = new Filter();
            return filter;

        }
        #endregion

        public bool IsEquals(in Filter filter) {

            return this.contains == filter.contains &&
                   this.notContains == filter.notContains;

        }

    }

}
