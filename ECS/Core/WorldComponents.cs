using System.Collections.Generic;

namespace ME.ECS {

    using ME.ECS.Collections;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed partial class World : IPoolableSpawn, IPoolableRecycle {

        private Entity sharedEntity;
        private bool sharedEntityInitialized;
        
        partial void OnSpawnComponents() {

            this.sharedEntity = default;
            this.sharedEntityInitialized = false;

        }

        partial void OnRecycleComponents() {
            
        }
        
        #region Regular Components
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TComponent AddOrGetComponent<TComponent>(Entity entity) where TComponent : class, IComponent, new() {

            var element = this.GetComponent<TComponent>(entity);
            if (element != null) return element;
            
            return this.AddComponent<TComponent>(entity);

        }

        /// <summary>
        /// Add component for current entity only (create component data)
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TComponent AddComponent<TComponent>(Entity entity) where TComponent : class, IComponent, new() {

            TComponent data;
            data = PoolComponents.Spawn<TComponent>();

            return this.AddComponent<TComponent>(entity, data);

        }

        /// <summary>
        /// Add component for current entity only (create component data)
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TComponentType"></typeparam>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TComponent AddComponent<TComponent, TComponentType>(Entity entity) where TComponentType : class, IComponent where TComponent : class, TComponentType, IComponent, new() {

            TComponent data;
            data = PoolComponents.Spawn<TComponent>();

            return (TComponent)this.AddComponent<TComponentType>(entity, data);

        }

        /// <summary>
        /// Add component for entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="data"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TComponent AddComponent<TComponent>(Entity entity, TComponent data) where TComponent : class, IComponent {

            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();

            }
            #endif

            #if WORLD_THREAD_CHECK
            if (this.worldThread != System.Threading.Thread.CurrentThread) {
                
                WrongThreadException.Throw("AddComponent");

            }
            #endif

            this.currentState.storage.versions.Increment(in entity);
            this.currentState.components.Add(entity.id, data);
            if (this.currentState.filters.HasInFilters<TComponent>() == true) this.currentState.storage.archetypes.Set<TComponent>(in entity);
            this.AddComponentToFilter(entity);
            
            return data;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AddComponent<TComponent>(Entity entity, TComponent data, in int componentIndex) where TComponent : class, IComponent {
            
            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();

            }
            #endif

            #if WORLD_THREAD_CHECK
            if (this.worldThread != System.Threading.Thread.CurrentThread) {
                
                WrongThreadException.Throw("AddComponent");

            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            this.currentState.components.Add(entity.id, data);
            if (this.currentState.filters.allFiltersArchetype.HasBit(in componentIndex) == true) this.currentState.storage.archetypes.Set(in entity, in componentIndex);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TComponent GetComponent<TComponent>(Entity entity) where TComponent : class, IComponent {
            
            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif
            
            return this.currentState.components.GetFirst<TComponent>(entity.id);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ListCopyable<IComponent> ForEachComponent<TComponent>(Entity entity) where TComponent : class, IComponent {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            return this.currentState.components.ForEach<TComponent>(entity.id);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ListCopyable<IComponent> ForEachComponent<TComponent>(int entityId) where TComponent : class, IComponent {

            return this.currentState.components.ForEach<TComponent>(entityId);

        }

        /// <summary>
        /// Check is component exists on entity
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool HasComponent<TComponent>(Entity entity) where TComponent : class, IComponent {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            return this.currentState.components.Contains<TComponent>(entity.id);
            
        }

        /// <summary>
        /// Remove all components with type from certain entity by predicate
        /// </summary>
        /// <param name="entity"></param>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveComponentsPredicate<TComponent, TComponentPredicate>(Entity entity, TComponentPredicate predicate) where TComponent : class, IComponent where TComponentPredicate : IComponentPredicate<TComponent> {

            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();

            }
            #endif

            #if WORLD_THREAD_CHECK
            if (this.worldThread != System.Threading.Thread.CurrentThread) {
                
                WrongThreadException.Throw("RemoveComponentsPredicate");
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            if (this.currentState.components.RemoveAllPredicate<TComponent, TComponentPredicate>(entity.id, predicate) > 0) {
                
                this.currentState.storage.versions.Increment(in entity);
                this.currentState.storage.archetypes.Remove<TComponent>(in entity);
                this.RemoveComponentFromFilter(in entity);

            }

        }

        /// <summary>
        /// Remove all components from certain entity
        /// </summary>
        /// <param name="entity"></param>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveComponents(Entity entity) {

            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();

            }
            #endif

            #if WORLD_THREAD_CHECK
            if (this.worldThread != System.Threading.Thread.CurrentThread) {
                
                WrongThreadException.Throw("RemoveComponents");

            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            if (this.currentState.components.RemoveAll(entity.id) > 0) {
                
                this.currentState.storage.versions.Increment(in entity);
                this.currentState.storage.archetypes.RemoveAll(in entity);
                ComponentsInitializerWorld.Init(in entity);
                this.RemoveComponentFromFilter(in entity);

            }

        }

        /// <summary>
        /// Remove all components from certain entity by type
        /// </summary>
        /// <param name="entity"></param>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveComponents<TComponent>(Entity entity) where TComponent : class, IComponent {

            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();

            }
            #endif

            #if WORLD_THREAD_CHECK
            if (this.worldThread != System.Threading.Thread.CurrentThread) {
                
                WrongThreadException.Throw("RemoveComponents");

            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            if (this.currentState.components.RemoveAll<TComponent>(entity.id) > 0) {
                
                this.currentState.storage.versions.Increment(in entity);
                this.currentState.storage.archetypes.Remove<TComponent>(in entity);
                this.RemoveComponentFromFilter(in entity);

            }

        }

        /// <summary>
        /// Remove all components with type TComponent from all entities
        /// This method doesn't update any filter, you should call UpdateFilter manually
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveComponents<TComponent>() where TComponent : class, IComponent {

            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();

            }
            #endif

            #if WORLD_THREAD_CHECK
            if (this.worldThread != System.Threading.Thread.CurrentThread) {
                
                WrongThreadException.Throw("RemoveComponents");

            }
            #endif

            if (this.currentState.components.RemoveAll<TComponent>() > 0) {
                
                this.currentState.storage.archetypes.RemoveAll<TComponent>();
                this.UpdateAllFilters();

            }

        }
        #endregion

        #region Shared Components
        public TComponent AddOrGetComponentShared<TComponent>() where TComponent : class, IComponent, new() {

            return this.AddOrGetComponent<TComponent>(this.sharedEntity);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TComponent AddComponentShared<TComponent>() where TComponent : class, IComponent, new() {

            return this.AddComponent<TComponent>(this.sharedEntity);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TComponent AddComponentShared<TComponent>(TComponent data) where TComponent : class, IComponent {
            
            return this.AddComponent<TComponent>(this.sharedEntity, data);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public TComponent GetComponentShared<TComponent>() where TComponent : class, IComponent {

            return this.GetComponent<TComponent>(this.sharedEntity);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ListCopyable<IComponent> ForEachComponentShared<TComponent>(Entity entity) where TComponent : class, IComponent {
            
            return this.ForEachComponent<TComponent>(this.sharedEntity);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool HasComponentShared<TComponent>() where TComponent : class, IComponent {

            return this.HasComponent<TComponent>(this.sharedEntity);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveComponentsShared<TComponent>() where TComponent : class, IComponent {
            
            this.RemoveComponents<TComponent>(this.sharedEntity);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveComponentsSharedPredicate<TComponent, TComponentPredicate>(TComponentPredicate predicate) where TComponent : class, IComponent where TComponentPredicate : IComponentPredicate<TComponent> {
            
            this.RemoveComponentsPredicate<TComponent, TComponentPredicate>(this.sharedEntity, predicate);
            
        }
        #endregion
        
    }

}