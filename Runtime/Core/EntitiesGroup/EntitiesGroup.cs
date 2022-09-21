#if !ENTITIES_GROUP_DISABLED
using ME.ECS.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct EntitiesGroup : System.IDisposable {

        public readonly bool copyMode;
        public readonly int Length;
        public readonly int fromId;
        public readonly int toId;
        public Unity.Collections.NativeArray<Entity> slice;

        public EntitiesGroup(int fromId, int toId, Unity.Collections.NativeArray<Entity> slice, bool copyMode) {

            this.fromId = fromId;
            this.toId = toId;
            this.Length = this.toId - this.fromId + 1;
            this.slice = slice;
            this.copyMode = copyMode;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {

            if (this.slice.IsCreated == true) this.slice.Dispose();
            this = default;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly void InstantiateView(ViewId viewId) {
            
            var world = Worlds.current;
            var viewsModule = world.GetModule<ME.ECS.Views.ViewsModule>();
            viewsModule.InstantiateView(viewId, in this);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly void UpdateFilters() {

            var maxEntity = this.slice[this.slice.Length - 1];
            var world = Worlds.current;
            ComponentsInitializerWorld.Init(in maxEntity);
            world.UpdateFilters(in this);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly void Set(IComponentBase component, int dataIndex) {

            var typeId = dataIndex;
            var world = Worlds.current;
            ref var container = ref world.GetStructComponents();
            var reg = container.list[typeId];
            reg.Merge();
            reg.SetObject(in this, component);

        }

        public readonly void Remove(int dataIndex) {

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
        public readonly BufferArray<Component<T>> Read<T>() where T : struct, IComponentBase {

            return this.Get<T>();

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly BufferArray<Component<T>> Get<T>() where T : struct, IComponentBase {
            
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
        public readonly void Set<T>(T component, bool updateFilters = true) where T : struct, IComponentBase {

            var typeId = WorldUtilities.GetAllComponentTypeId<T>();
            var world = Worlds.current;
            ref var container = ref world.GetStructComponents();
            var reg = (StructComponentsBase<T>)container.list[typeId];
            reg.Merge();
            reg.Set(in this, component);
            if (updateFilters == true) this.UpdateFilters();

        }

        /// <summary>
        /// Remove component data from the storage.
        /// Using Array.Clear to set up.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly void Remove<T>(bool updateFilters = true) where T : struct, IComponentBase {

            var typeId = WorldUtilities.GetAllComponentTypeId<T>();
            var world = Worlds.current;
            ref var container = ref world.GetStructComponents();
            var reg = (StructComponentsBase<T>)container.list[typeId];
            reg.Merge();
            reg.Remove(in this);
            if (updateFilters == true) this.UpdateFilters();

        }

    }

    namespace Views {

        public partial class ViewsModule {

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void InstantiateView(ViewId sourceId, in EntitiesGroup group) {

                if (this.world.settings.turnOffViews == true) return;

                // Called by tick system
                if (this.world.HasStep(WorldStep.LogicTick) == false && this.world.HasResetState() == true) {

                    throw new OutOfStateException();

                }

                if (this.registryIdToPrefab.ContainsKey(sourceId) == false) {

                    throw new ViewRegistryNotFoundException(sourceId);

                }

                var components = group.Read<ViewComponent>();
                for (int i = group.fromId, k = 0; i <= group.toId; ++i, ++k) {
                    
                    var entity = group.slice[k];
                    var viewInfo = new ViewInfo(entity, sourceId, this.world.GetStateTick(), DestroyViewBehaviour.DestroyWithEntity);
                    var view = new ViewComponent() {
                        viewInfo = viewInfo,
                        seed = (uint)this.world.GetSeed(),
                    };
                    ref var comp = ref components.arr[k];
                    comp.state = 1;
                    comp.data = view;
                    
                    if (this.world.HasResetState() == false) {

                        this.CreateVisualInstance(view.seed, in view.viewInfo);

                    }

                }
                
                this.isRequestsDirty = true;

            }

        }

    }

    public abstract partial class StructRegistryBase {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void SetObject(in EntitiesGroup group, IComponentBase data, bool setBits = true);
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
        public override void SetObject(in EntitiesGroup group, IComponentBase component, bool setBits = true) {
            
            var data = new Component<TComponent>() {
                data = (TComponent)component,
                state = 1,
                version = 1,
            };
            var componentIndex = ComponentTypes<TComponent>.typeId;
            if (group.copyMode == true) {
                for (int i = group.fromId; i <= group.toId; ++i) {
                    this.components.data[i] = data;
                }
            } else {
                for (int i = group.fromId; i <= group.toId; ++i) {
                    this.components.data[i] = data;
                }
            }
            this.world.currentState.storage.Set(ref this.world.currentState.allocator, in group, componentIndex, ComponentTypes<TComponent>.isFilterLambda);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Set(in EntitiesGroup group, TComponent component, bool setBits = true) {
            
            var data = new Component<TComponent>() {
                data = component,
                state = 1,
                version = 1,
            };
            var componentIndex = ComponentTypes<TComponent>.typeId;
            if (group.copyMode == true) {
                for (int i = group.fromId; i <= group.toId; ++i) {
                    this.components.data[i] = data;
                }
            } else {
                for (int i = group.fromId; i <= group.toId; ++i) {
                    this.components.data[i] = data;
                }
            }
            this.world.currentState.storage.Set(ref this.world.currentState.allocator, in group, componentIndex, ComponentTypes<TComponent>.isFilterLambda);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Remove(in EntitiesGroup group, bool setBits = true) {

            System.Array.Clear(this.components.data.arr, group.fromId, group.Length);

            if (setBits == true) {
                
                var componentIndex = ComponentTypes<TComponent>.typeId;
                this.world.currentState.storage.Remove(ref this.world.currentState.allocator, in group, componentIndex, ComponentTypes<TComponent>.isFilterLambda);

            }
            
        }

    }

    public partial class StructComponentsBlittable<TComponent> {
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void SetObject(in EntitiesGroup group, IComponentBase component, bool setBits = true) {
            
            var data = new Component<TComponent>() {
                data = (TComponent)component,
                state = 1,
                version = 1,
            };
            var componentIndex = ComponentTypes<TComponent>.typeId;
            if (group.copyMode == true) {
                for (int i = group.fromId; i <= group.toId; ++i) {
                    this.components.data[i] = data;
                }
            } else {
                for (int i = group.fromId; i <= group.toId; ++i) {
                    this.components.data[i] = data;
                }
            }
            this.world.currentState.storage.Set(ref this.world.currentState.allocator, in group, componentIndex, ComponentTypes<TComponent>.isFilterLambda);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Set(in EntitiesGroup group, TComponent component, bool setBits = true) {
            
            var data = new Component<TComponent>() {
                data = component,
                state = 1,
                version = 1,
            };
            var componentIndex = ComponentTypes<TComponent>.typeId;
            if (group.copyMode == true) {
                for (int i = group.fromId; i <= group.toId; ++i) {
                    this.components.data[i] = data;
                }
            } else {
                for (int i = group.fromId; i <= group.toId; ++i) {
                    this.components.data[i] = data;
                }
            }
            this.world.currentState.storage.Set(ref this.world.currentState.allocator, in group, componentIndex, ComponentTypes<TComponent>.isFilterLambda);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Remove(in EntitiesGroup group, bool setBits = true) {

            NativeArrayUtils.Clear(this.components.data.arr, group.fromId, group.Length);

            if (setBits == true) {
                
                var componentIndex = ComponentTypes<TComponent>.typeId;
                this.world.currentState.storage.Remove(ref this.world.currentState.allocator, in group, componentIndex, ComponentTypes<TComponent>.isFilterLambda);

            }
            
        }

    }

    public partial class StructComponentsUnmanaged<TComponent> {
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void SetObject(in EntitiesGroup group, IComponentBase component, bool setBits = true) {
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Set(in EntitiesGroup group, TComponent component, bool setBits = true) {
            
            var data = new Component<TComponent>() {
                data = component,
                state = 1,
                version = 1,
            };
            var componentIndex = ComponentTypes<TComponent>.typeId;
            ref var reg = ref this.registry;
            if (group.copyMode == true) {
                for (int i = group.fromId; i <= group.toId; ++i) {
                    reg.components.Get(ref this.allocator, i) = data;
                }
            } else {
                for (int i = group.fromId; i <= group.toId; ++i) {
                    reg.components.Get(ref this.allocator, i) = data;
                }
            }
            this.world.currentState.storage.Set(ref this.world.currentState.allocator, in group, componentIndex, ComponentTypes<TComponent>.isFilterLambda);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Remove(in EntitiesGroup group, bool setBits = true) {

            //NativeArrayUtils.Clear(this.components.data.arr, group.fromId, group.Length);
            this.registry.components.Remove(ref this.allocator, group.fromId, group.Length);

            if (setBits == true) {
                
                var componentIndex = ComponentTypes<TComponent>.typeId;
                this.world.currentState.storage.Remove(ref this.world.currentState.allocator, in group, componentIndex, ComponentTypes<TComponent>.isFilterLambda);

            }
            
        }

    }

    public partial class World {

        /// <summary>
        /// Create EntitiesGroup.
        /// New entities will be created to be sure entities store in line.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="allocator"></param>
        /// <param name="copyMode">
        /// If true all entities would be copied with the same behaviour and you should never use entities from this group outside of the group.
        /// If false some optimizations will be skipped.
        /// </param>
        /// <returns></returns>
        public EntitiesGroup AddEntities(int count, Unity.Collections.Allocator allocator, bool copyMode) {
            
            E.IS_LOGIC_STEP(this);
            
            var group = new EntitiesGroup();
            if (count <= 0) return group;
            
            this.currentState.storage.Alloc(ref this.currentState.allocator, count, ref group, allocator, copyMode);
            this.UpdateEntityOnCreate(in group);

            return group;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void UpdateEntityOnCreate(in EntitiesGroup group) {

            var maxEntity = group.slice[group.slice.Length - 1];
            ComponentsInitializerWorld.Init(in maxEntity);
            this.currentState.storage.versions.Validate(ref this.currentState.allocator, in maxEntity);
            this.CreateEntityPlugins(maxEntity, true);
            this.CreateEntityInFilters(ref this.currentState.allocator, maxEntity);

        }

    }

}
#endif