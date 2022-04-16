using Unity.Jobs;

namespace ME.ECS {

    using Collections;
    
    #if FILTERS_STORAGE_LEGACY
    public struct FiltersCache {

        private World world;
        private DictionaryCopyable<int, HashSetCopyable<Entity>> hash;

        public FiltersCache(World world) {

            this.world = world;
            this.hash = PoolDictionaryCopyable<int, HashSetCopyable<Entity>>.Spawn(100);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void UpdateRequest<T>(in Entity entity) {
            
            var componentId = WorldUtilities.GetComponentTypeId<T>();
            this.UpdateRequest(in entity, componentId);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void UpdateRequest(in Entity entity, int componentId) {
            
            if (this.hash.TryGetValue(componentId, out var list) == true) {
                
                if (list.Contains(entity) == false) list.Add(entity);
                
            } else {

                list = PoolHashSetCopyable<Entity>.Spawn();
                list.Add(entity);
                this.hash.Add(componentId, list);
                
            }
                
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Apply() {

            if (this.hash.Count == 0) return;
            
            #if UNITY_EDITOR
            var marker = new Unity.Profiling.ProfilerMarker(new Unity.Profiling.ProfilerCategory("ME.ECS"), "FiltersCache::Apply()");
            marker.Begin();
            #endif

            var filtersContains = PoolHashSetCopyable<FiltersTree.FilterBurst>.Spawn();
            var filtersNotContains = PoolHashSetCopyable<FiltersTree.FilterBurst>.Spawn();
            foreach (var kv in this.hash) {
                
                var componentId = kv.Key;
                var list = kv.Value;
                var contains = this.world.currentState.filters.filtersTree.GetFiltersContainsFor(componentId);
                var notContains = this.world.currentState.filters.filtersTree.GetFiltersNotContainsFor(componentId);
                for (int i = 0; i < contains.Length; ++i) {

                    var filterData = contains[i];
                    if (filtersContains.Contains(filterData) == false) {
                        
                        filtersContains.Add(filterData);
                        
                        var filter = this.world.GetFilter(filterData.id);
                        foreach (var entity in list) {
                            
                            filter.OnUpdate(in entity);
                            
                        }

                    }

                }
                
                for (int i = 0; i < notContains.Length; ++i) {

                    var filterData = notContains[i];
                    if (filtersNotContains.Contains(filterData) == false) {
                        
                        filtersNotContains.Add(filterData);
                        
                        var filter = this.world.GetFilter(filterData.id);
                        foreach (var entity in list) {
                            
                            filter.OnUpdate(in entity);
                            
                        }

                    }

                }
                
            }
            
            PoolHashSetCopyable<FiltersTree.FilterBurst>.Recycle(ref filtersContains);
            PoolHashSetCopyable<FiltersTree.FilterBurst>.Recycle(ref filtersNotContains);
            
            this.hash.Clear();

            #if UNITY_EDITOR
            marker.End();
            #endif
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Dispose() {
            
            this.world = null;
            PoolDictionaryCopyable<int, HashSetCopyable<Entity>>.Recycle(ref this.hash);
            
        }
        
    }
    #endif

}