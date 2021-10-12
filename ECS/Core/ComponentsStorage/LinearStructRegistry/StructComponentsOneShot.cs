namespace ME.ECS {

    public static partial class EntityExtensionsV2 {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool HasOneShot<TComponent>(this in Entity entity) where TComponent : struct, IComponentOneShot {

            return Worlds.currentWorld.HasDataOneShot<TComponent>(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity SetOneShot<TComponent>(this in Entity entity) where TComponent : struct, IComponentOneShot {

            Worlds.currentWorld.SetDataOneShot(in entity, new TComponent());
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity SetOneShot<TComponent>(this in Entity entity, in TComponent data) where TComponent : struct, IComponentOneShot {

            Worlds.currentWorld.SetDataOneShot(in entity, data);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity RemoveOneShot<TComponent>(this in Entity entity) where TComponent : struct, IComponentOneShot {

            Worlds.currentWorld.RemoveDataOneShot<TComponent>(in entity);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref TComponent GetOneShot<TComponent>(this in Entity entity) where TComponent : struct, IComponentOneShot {

            return ref Worlds.currentWorld.GetDataOneShot<TComponent>(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref readonly TComponent ReadOneShot<TComponent>(this in Entity entity) where TComponent : struct, IComponentOneShot {

            return ref Worlds.currentWorld.ReadDataOneShot<TComponent>(in entity);

        }

    }

    public partial class World {
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveDataOneShot<TComponent>(in Entity entity) where TComponent : struct, IComponentOneShot {

            #if WORLD_STATE_CHECK
            if (this.isActive == true && this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {
                
                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            var reg = (StructComponents<TComponent>)this.structComponentsNoState.list.arr[AllComponentTypes<TComponent>.typeId];
            ref var storage = ref this.currentState.storage;
            ref var bucket = ref reg.components[entity.id];
            if (bucket.state == 0) return;
            bucket.state = 0;
            
            storage.versions.Increment(in entity);
            if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);
            reg.RemoveData(ref bucket);
            
            if (ComponentTypes<TComponent>.typeId >= 0) {

                storage.archetypes.Remove<TComponent>(in entity);
                this.UpdateFilterByStructComponent<TComponent>(in entity);

            }

            #if ENTITY_ACTIONS
            this.RaiseEntityActionOnRemove<TComponent>(in entity);
            #endif

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasDataOneShot<TComponent>(in Entity entity) where TComponent : struct, IComponentOneShot {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            return this.structComponentsNoState.list.arr[AllComponentTypes<TComponent>.typeId].Has(in entity);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref readonly TComponent ReadDataOneShot<TComponent>(in Entity entity) where TComponent : struct, IComponentOneShot {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            // Inline all manually
            if (AllComponentTypes<TComponent>.isTag == true) return ref AllComponentTypes<TComponent>.empty;
            var reg = (StructComponents<TComponent>)this.structComponentsNoState.list.arr[AllComponentTypes<TComponent>.typeId];
            return ref reg.components[entity.id].data;

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref TComponent GetDataOneShot<TComponent>(in Entity entity, bool createIfNotExists = true) where TComponent : struct, IComponentOneShot {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            // Inline all manually
            var incrementVersion = (this.HasResetState() == false || this.HasStep(WorldStep.LogicTick) == true);
            var reg = (StructComponents<TComponent>)this.structComponentsNoState.list.arr[AllComponentTypes<TComponent>.typeId];
            ref var storage = ref this.currentState.storage;
            ref var bucket = ref reg.components[entity.id];
            if (createIfNotExists == true && bucket.state == 0) {

                #if WORLD_EXCEPTIONS
                if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                    OutOfStateException.ThrowWorldStateCheck();

                }
                #endif

                incrementVersion = true;
                bucket.state = 1;
                if (ComponentTypes<TComponent>.typeId >= 0) {

                    storage.archetypes.Set<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

                #if ENTITY_ACTIONS
                this.RaiseEntityActionOnAdd<TComponent>(in entity);
                #endif

            }

            if (AllComponentTypes<TComponent>.isTag == true) return ref AllComponentTypes<TComponent>.empty;
            if (incrementVersion == true) {

                reg.UpdateVersion(ref bucket);
                storage.versions.Increment(in entity);
                if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
                if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);

            }

            return ref bucket.data;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref byte SetDataOneShot<TComponent>(in Entity entity, in TComponent data) where TComponent : struct, IComponentOneShot {
            
            #if WORLD_STATE_CHECK
            if (this.HasStep(WorldStep.LogicTick) == false && this.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();
                
            }
            #endif

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            var reg = (StructComponents<TComponent>)this.structComponentsNoState.list.arr[AllComponentTypes<TComponent>.typeId];
            ref var storage = ref this.currentState.storage;
            ref var bucket = ref reg.components[entity.id];
            ref var state = ref bucket.state;
            reg.Replace(ref bucket, in data);
            reg.UpdateVersion(ref bucket);
            if (state == 0) {

                if (ComponentTypes<TComponent>.typeId >= 0) {

                    storage.archetypes.Set<TComponent>(in entity);
                    this.UpdateFilterByStructComponent<TComponent>(in entity);

                }

            }
            #if ENTITY_ACTIONS
            this.RaiseEntityActionOnAdd<TComponent>(in entity);
            #endif
            storage.versions.Increment(in entity);
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
            if (ComponentTypes<TComponent>.isFilterVersioned == true) this.UpdateFilterByStructComponentVersioned<TComponent>(in entity);

            state = (byte)(ComponentLifetime.NotifyAllSystemsBelow + 1);
            World.AddToLifetimeIndex<TComponent>(in entity, ComponentLifetime.NotifyAllSystemsBelow, 0f, ref this.structComponentsNoState);
            
            return ref state;
            
        }

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class StructComponentsOneShot<TComponent> : StructComponents<TComponent> where TComponent : struct, IStructComponentBase, IComponentOneShot {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void RemoveData(ref Component<TComponent> bucket) {

            bucket.data = default;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        protected override StructRegistryBase SpawnInstance() {

            return PoolRegistries.SpawnOneShot<TComponent>();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Replace(ref Component<TComponent> bucket, in TComponent data) {
            
            bucket.data = data;
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void CopyFrom(StructRegistryBase other) {

            var _other = (StructComponents<TComponent>)other;
            ArrayUtils.Copy(_other.lifetimeData, ref this.lifetimeData);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        protected override byte CopyFromState(in Entity @from, in Entity to) {
            
            return 0;
            
        }

    }

}