using ME.ECS.Collections;

namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public readonly struct EntitiesGroup {

        public readonly bool copyMode;
        public readonly int Length;
        public readonly int fromId;
        public readonly int toId;
        public readonly Unity.Collections.NativeSlice<Entity> slice;

        public EntitiesGroup(int fromId, int toId, Unity.Collections.NativeSlice<Entity> slice, bool copyMode) {

            this.fromId = fromId;
            this.toId = toId;
            this.Length = this.toId - this.fromId + 1;
            this.slice = slice;
            this.copyMode = copyMode;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void UpdateFilters() {

            var maxEntity = this.slice[this.slice.Length - 1];
            var world = Worlds.current;
            ComponentsInitializerWorld.Init(in maxEntity);
            world.UpdateFilters(in this);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(IStructComponentBase component, int dataIndex) {

            var typeId = dataIndex;
            var world = Worlds.current;
            ref var container = ref world.GetStructComponents();
            var reg = container.list[typeId];
            reg.SetObject(in this, component);

        }

        public void Remove(int dataIndex) {

            var typeId = dataIndex;
            var world = Worlds.current;
            ref var container = ref world.GetStructComponents();
            var reg = container.list[typeId];
            reg.Merge();
            reg.Remove(in this);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public BufferArray<Component<T>> Get<T>() where T : struct, IStructComponentBase {
            
            var typeId = WorldUtilities.GetAllComponentTypeId<T>();
            var world = Worlds.current;
            ref var container = ref world.GetStructComponents();
            var reg = (StructComponents<T>)container.list[typeId];
            reg.Merge();
            return reg.components.data;

        }

        /// <summary>
        /// Write component data to the storage.
        /// Using foreach to set up.
        /// </summary>
        /// <param name="component"></param>
        /// <typeparam name="T"></typeparam>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set<T>(T component) where T : struct, IStructComponentBase {

            var typeId = WorldUtilities.GetAllComponentTypeId<T>();
            var world = Worlds.current;
            ref var container = ref world.GetStructComponents();
            var reg = (StructComponentsBase<T>)container.list[typeId];
            reg.Merge();
            reg.Set(in this, component);
            this.UpdateFilters();

        }

        /// <summary>
        /// Remove component data from the storage.
        /// Using Array.Clear to set up.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove<T>() where T : struct, IStructComponentBase {

            var typeId = WorldUtilities.GetAllComponentTypeId<T>();
            var world = Worlds.current;
            ref var container = ref world.GetStructComponents();
            var reg = (StructComponentsBase<T>)container.list[typeId];
            reg.Merge();
            reg.Remove(in this);
            this.UpdateFilters();

        }

    }

    namespace DataConfigs {

        public partial class DataConfig {

            /// <summary>
            /// Apply config onto the group.
            /// IComponentInitializable/IComponentDeinitializable will not be called.
            /// </summary>
            /// <param name="group"></param>
            public virtual void Apply(in EntitiesGroup group) {

                this.Prewarm();

                var world = Worlds.currentWorld;
                for (int i = 0; i < this.removeStructComponents.Length; ++i) {

                    var dataIndex = this.GetComponentDataIndexByTypeRemoveWithCache(this.removeStructComponents[i], i);
                    group.Remove(dataIndex);

                }

                for (int i = 0; i < this.structComponents.Length; ++i) {

                    var dataIndex = this.GetComponentDataIndexByTypeWithCache(this.structComponents[i], i);
                    if (this.structComponents[i] is IComponentStatic) continue;

                    var isShared = (this.structComponents[i] is IComponentShared);
                    if (isShared == true) { // is shared?

                        throw new System.NotImplementedException("Set Shared for EntityGroups is not implemented");

                    } else {

                        group.Set(this.structComponents[i], dataIndex);

                    }

                }

                // Update filters
                {
                    group.UpdateFilters();
                }

            }

        }

    }

    public abstract partial class StructRegistryBase {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void SetObject(in EntitiesGroup group, IStructComponentBase data, bool setBits = true);
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void Remove(in EntitiesGroup group, bool setBits = true);

    }
    
    public abstract partial class StructComponentsBase<TComponent> {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void Set(in EntitiesGroup group, TComponent component, bool setBits = true);

    }

    public partial class StructComponents<TComponent> {
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void SetObject(in EntitiesGroup group, IStructComponentBase component, bool setBits = true) {
            
            var data = new Component<TComponent>() {
                data = (TComponent)component,
                state = 1,
                version = ++this.maxVersion + 1,
            };
            var componentIndex = ComponentTypes<TComponent>.typeId;
            ref var archetypes = ref this.world.currentState.storage.archetypes;
            if (group.copyMode == true) {
                for (int i = group.fromId; i <= group.toId; ++i) {
                    this.components.data[i] = data;
                }
                if (componentIndex >= 0 && setBits == true) archetypes.Set(in group, componentIndex);
            } else {
                for (int i = group.fromId; i <= group.toId; ++i) {
                    this.components.data[i] = data;
                    if (componentIndex >= 0 && setBits == true) archetypes.Set(i, componentIndex);
                }
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Set(in EntitiesGroup group, TComponent component, bool setBits = true) {
            
            var data = new Component<TComponent>() {
                data = component,
                state = 1,
                version = ++this.maxVersion + 1,
            };
            var componentIndex = ComponentTypes<TComponent>.typeId;
            ref var archetypes = ref this.world.currentState.storage.archetypes;
            if (group.copyMode == true) {
                for (int i = group.fromId; i <= group.toId; ++i) {
                    this.components.data[i] = data;
                }
                if (componentIndex >= 0 && setBits == true) archetypes.Set(in group, componentIndex);
            } else {
                for (int i = group.fromId; i <= group.toId; ++i) {
                    this.components.data[i] = data;
                    if (componentIndex >= 0 && setBits == true) archetypes.Set(i, componentIndex);
                }
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Remove(in EntitiesGroup group, bool setBits = true) {

            System.Array.Clear(this.components.data.arr, group.fromId, group.Length);

            if (setBits == true) {
                
                var componentIndex = ComponentTypes<TComponent>.typeId;
                if (componentIndex >= 0) {
                    ref var archetypes = ref this.world.currentState.storage.archetypes;
                    if (group.copyMode == true) {
                        archetypes.Remove(in group, componentIndex);
                    } else {
                        for (int i = group.fromId; i <= group.toId; ++i) {
                            archetypes.Remove(i, componentIndex);
                        }
                    }
                }

            }
            
        }

    }

    public partial class World {

        public void UpdateFilters(in EntitiesGroup group) {

            //ArrayUtils.Resize(this.id, ref FiltersDirectCache.dic);
            ref var dic = ref FiltersDirectCache.dic.arr[this.id];
            if (dic.arr != null) {

                for (int i = 0; i < dic.Length; ++i) {

                    if (dic.arr[i] == false) continue;
                    var filterId = i + 1;
                    var filter = this.GetFilter(filterId);
                    for (int j = group.fromId, k = 0; j <= group.toId; ++j, ++k) {
                    
                        if (filter.IsForEntity(j) == false) continue;
                        filter.OnUpdate(in group.slice.GetRefRead(k));
                        
                    }

                }

            }

        }

        /// <summary>
        /// Create EntitiesGroup.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="copyMode">
        /// If true all entities would be copied with the same behaviour and you should never use entities from this group outside of the group.
        /// if false some optimizations will be skipped.
        /// </param>
        /// <returns></returns>
        public EntitiesGroup AddEntities(int count, bool copyMode) {
            
            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            var group = new EntitiesGroup();
            if (count <= 0) return group;
            
            this.currentState.storage.Alloc(count, ref group, copyMode);
            this.UpdateEntityOnCreate(in group);

            return group;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void UpdateEntityOnCreate(in EntitiesGroup group) {

            var maxEntity = group.slice[group.slice.Length - 1];
            ComponentsInitializerWorld.Init(in maxEntity);
            this.currentState.storage.versions.Validate(in maxEntity);
            this.CreateEntityPlugins(maxEntity);
            this.CreateEntityInFilters(maxEntity);

        }

    }

}