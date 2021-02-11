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
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public TComponent AddOrGetComponent<TComponent>(Entity entity) where TComponent : class, IComponent, new() {

            DeprecatedException.Throw();

            return default;

        }

        /// <summary>
        /// Add component for current entity only (create component data)
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public TComponent AddComponent<TComponent>(Entity entity) where TComponent : class, IComponent, new() {

            DeprecatedException.Throw();

            return default;

        }

        /// <summary>
        /// Add component for current entity only (create component data)
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TComponentType"></typeparam>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public TComponent AddComponent<TComponent, TComponentType>(Entity entity) where TComponentType : class, IComponent where TComponent : class, TComponentType, IComponent, new() {

            DeprecatedException.Throw();

            return default;

        }

        /// <summary>
        /// Add component for entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="data"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public TComponent AddComponent<TComponent>(in Entity entity, TComponent data) where TComponent : class, IComponent {

            DeprecatedException.Throw();

            return default;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        internal void AddComponent<TComponent>(in Entity entity, TComponent data, int componentIndex) where TComponent : class, IComponent {
            
            DeprecatedException.Throw();

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public TComponent GetComponent<TComponent>(in Entity entity) where TComponent : class, IComponent {
            
            DeprecatedException.Throw();

            return default;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public ListCopyable<IComponent> ForEachComponent<TComponent>(in Entity entity) where TComponent : class, IComponent {

            DeprecatedException.Throw();

            return default;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        internal ListCopyable<IComponent> ForEachComponent<TComponent>(int entityId) where TComponent : class, IComponent {

            DeprecatedException.Throw();

            return default;

        }

        /// <summary>
        /// Check is component exists on entity
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public bool HasComponent<TComponent>(in Entity entity) where TComponent : class, IComponent {

            DeprecatedException.Throw();

            return default;

        }

        /// <summary>
        /// Remove all components with type from certain entity by predicate
        /// </summary>
        /// <param name="entity"></param>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public void RemoveComponentsPredicate<TComponent, TComponentPredicate>(in Entity entity, TComponentPredicate predicate) where TComponent : class, IComponent where TComponentPredicate : IComponentPredicate<TComponent> {

            DeprecatedException.Throw();

        }

        /// <summary>
        /// Remove all components from certain entity
        /// </summary>
        /// <param name="entity"></param>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public void RemoveComponents(in Entity entity) {

            DeprecatedException.Throw();

        }

        /// <summary>
        /// Remove all components from certain entity by type
        /// </summary>
        /// <param name="entity"></param>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public void RemoveComponents<TComponent>(Entity entity) where TComponent : class, IComponent {

            DeprecatedException.Throw();

        }

        /// <summary>
        /// Remove all components with type TComponent from all entities
        /// This method doesn't update any filter, you should call UpdateFilter manually
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public void RemoveComponents<TComponent>() where TComponent : class, IComponent {

            DeprecatedException.Throw();

        }
        #endregion

        #region Shared Components
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public TComponent AddOrGetComponentShared<TComponent>() where TComponent : class, IComponent, new() {

            return this.AddOrGetComponent<TComponent>(this.sharedEntity);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public TComponent AddComponentShared<TComponent>() where TComponent : class, IComponent, new() {

            return this.AddComponent<TComponent>(this.sharedEntity);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public TComponent AddComponentShared<TComponent>(TComponent data) where TComponent : class, IComponent {
            
            return this.AddComponent<TComponent>(this.sharedEntity, data);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public TComponent GetComponentShared<TComponent>() where TComponent : class, IComponent {

            return this.GetComponent<TComponent>(this.sharedEntity);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public ListCopyable<IComponent> ForEachComponentShared<TComponent>(Entity entity) where TComponent : class, IComponent {
            
            return this.ForEachComponent<TComponent>(this.sharedEntity);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public bool HasComponentShared<TComponent>() where TComponent : class, IComponent {

            return this.HasComponent<TComponent>(this.sharedEntity);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public void RemoveComponentsShared<TComponent>() where TComponent : class, IComponent {
            
            this.RemoveComponents<TComponent>(this.sharedEntity);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
        public void RemoveComponentsSharedPredicate<TComponent, TComponentPredicate>(TComponentPredicate predicate) where TComponent : class, IComponent where TComponentPredicate : IComponentPredicate<TComponent> {
            
            this.RemoveComponentsPredicate<TComponent, TComponentPredicate>(this.sharedEntity, predicate);
            
        }
        #endregion
        
    }

}