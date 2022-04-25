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

                if (this.sharedEntity.generation == 0 && this.sharedEntityInitialized == false) {

                    // Create shared entity which should store shared components
                    this.sharedEntity = this.AddEntity();

                }
                this.sharedEntityInitialized = true;

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
                        this.CreateEntityPlugins(item, false);

                    }

                }

                PoolListCopyable<Entity>.Recycle(ref list);

                this.EndRestoreEntities();

            }

        }

        public void SetEntityCapacityInFilters(int capacity) {

            // On change capacity
            this.currentState.filters.SetCapacity(capacity);

        }

        public void CreateEntityInFilters(in Entity entity) {

            // On create new entity
            this.currentState.filters.SetCapacity(entity.id + 1);

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

        public void AddFilterByStructComponent(in Entity entity, int componentId, bool checkLambda) {

            this.currentState.filters.Set(in entity, componentId, checkLambda);

        }

        public void RemoveFilterByStructComponent(in Entity entity, int componentId, bool checkLambda) {

            this.currentState.filters.Remove(in entity, componentId, checkLambda);

        }

        public void UpdateFilterByStructComponent(in Entity entity, int componentId) { }

        public void ValidateFilterByStructComponent(in Entity entity, int componentId, bool makeRequest = false) {

            this.currentState.filters.Validate(in entity, componentId, makeRequest);

        }

        public void ValidateFilterByStructComponent<T>(in Entity entity, bool makeRequest = false) where T : struct, IComponentBase {

            this.currentState.filters.Validate<T>(in entity, makeRequest);

        }

        public void AddFilterByStructComponent<T>(in Entity entity) where T : struct, IComponentBase {

            this.currentState.filters.Set<T>(in entity);

        }

        public void RemoveFilterByStructComponent<T>(in Entity entity) where T : struct, IComponentBase {

            this.currentState.filters.Remove<T>(in entity);

        }

        public void UpdateFilterByStructComponent<T>(in Entity entity) where T : struct, IComponentBase { }

        public void UpdateFilterByStructComponentVersioned<T>(in Entity entity) where T : struct, IComponentBase { }

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

}
#endif
