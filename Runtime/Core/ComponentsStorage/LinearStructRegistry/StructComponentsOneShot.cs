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

            TComponent data = default;
            Worlds.currentWorld.SetDataOneShot(in entity, data);
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

            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);
            
            var reg = (StructComponents<TComponent>)this.structComponentsNoState.list.arr[OneShotComponentTypes<TComponent>.typeId];
            DataBufferUtils.PushRemove_INTERNAL(this, in entity, reg, StorageType.NoState);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool SetSharedDataOneShot<TComponent>(in TComponent data) where TComponent : struct, IComponentOneShot {

            return this.SetDataOneShot(in this.sharedEntity, in data);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveSharedDataOneShot<TComponent>() where TComponent : struct, IComponentOneShot {

            this.RemoveDataOneShot<TComponent>(in this.sharedEntity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasSharedDataOneShot<TComponent>() where TComponent : struct, IComponentOneShot {

            return this.HasDataOneShot<TComponent>(in this.sharedEntity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref readonly TComponent ReadSharedDataOneShot<TComponent>() where TComponent : struct, IComponentOneShot {

            return ref this.ReadDataOneShot<TComponent>(in this.sharedEntity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref TComponent GetSharedDataOneShot<TComponent>() where TComponent : struct, IComponentOneShot {

            return ref this.GetDataOneShot<TComponent>(in this.sharedEntity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasDataOneShot<TComponent>(in Entity entity) where TComponent : struct, IComponentOneShot {

            E.IS_ALIVE(in entity);

            return this.structComponentsNoState.list.arr[OneShotComponentTypes<TComponent>.typeId].Has(in entity);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref readonly TComponent ReadDataOneShot<TComponent>(in Entity entity) where TComponent : struct, IComponentOneShot {

            E.IS_TAG<TComponent>(in entity);
            E.IS_ALIVE(in entity);

            // Inline all manually
            var reg = (StructComponents<TComponent>)this.structComponentsNoState.list.arr[OneShotComponentTypes<TComponent>.typeId];
            return ref reg.components[entity.id].data;

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref TComponent GetDataOneShot<TComponent>(in Entity entity) where TComponent : struct, IComponentOneShot {

            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);
            E.IS_TAG<TComponent>(in entity);

            var reg = (StructComponents<TComponent>)this.structComponentsNoState.list.arr[OneShotComponentTypes<TComponent>.typeId];
            return ref DataBufferUtils.PushGet_INTERNAL(this, in entity, reg, StorageType.NoState);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool SetDataOneShot<TComponent>(in Entity entity, in TComponent data) where TComponent : struct, IComponentOneShot {
            
            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);

            var reg = (StructComponents<TComponent>)this.structComponentsNoState.list.arr[OneShotComponentTypes<TComponent>.typeId];
            return DataBufferUtils.PushSet_INTERNAL(this, in entity, reg, in data, StorageType.NoState);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetEntityOneShot(in Entity entity) {

            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in entity);
            
            var task = new StructComponentsContainer.NextTickTask {
                lifetime = ComponentLifetime.NotifyAllSystemsBelow,
                storageType = StorageType.NoState,
                secondsLifetime = 0f,
                entity = entity,
                dataIndex = -1,
                destroyEntity = true,
            };

            if (this.structComponentsNoState.nextTickTasks.Add(task) == false) {

                task.Recycle();

            }

        }

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class StructComponentsOneShot<TComponent> : StructComponents<TComponent> where TComponent : struct, IComponentOneShot {

        public override void Recycle() {
            
            PoolRegistries.Recycle(this);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void RemoveData(in Entity entity, ref Component<TComponent> bucket) {

            base.RemoveData(in entity, ref bucket);

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
        protected override byte CopyFromState(in Entity @from, in Entity to) {
            
            return 0;
            
        }

    }

}
