namespace ME.ECS {

    using Collections;

    public interface IComponentsUnmanaged {

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public partial class StructComponentsUnmanaged<TComponent> : StructComponentsBase<TComponent>, IComponentsUnmanaged where TComponent : struct, IComponentBase {

        protected ref UnmanagedComponentsStorage storage => ref this.world.currentState.structComponents.unmanagedComponentsStorage;
        private ref UnmanagedComponentsStorage.Item<TComponent> registry => ref this.storage.GetRegistry<TComponent>(in this.allocator);
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override ref byte GetState(in Entity entity) {

            ref var storage = ref this.storage;
            ref var reg = ref storage.GetRegistry<TComponent>(in this.allocator);
            return ref reg.components.Get(ref this.allocator, entity.id).state;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override bool TryRead(in Entity entity, out TComponent component) {
            
            ref var storage = ref this.storage;
            ref var reg = ref storage.GetRegistry<TComponent>(in this.allocator);
            ref var item = ref reg.components.Get(ref this.allocator, entity.id);
            component = item.data;
            return item.state > 0;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override ref Component<TComponent> Get(in Entity entity) {

            ref var storage = ref this.storage;
            ref var reg = ref storage.GetRegistry<TComponent>(in this.allocator);
            return ref reg.components.Get(ref this.allocator, entity.id);

        }

        public override UnsafeData CreateObjectUnsafe(in Entity entity) {
            
            ref var storage = ref this.storage;
            ref var reg = ref storage.GetRegistry<TComponent>(in this.allocator);
            ref var data = ref reg.components.Get(ref this.allocator, entity.id).data;
            return new UnsafeData().SetAsUnmanaged(ref this.allocator, data);

        }

        public override void Recycle() {
            
            PoolRegistries.Recycle(this);

        }

        public override bool IsNeedToDispose() {

            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override long GetVersion(int entityId) {

            ref var storage = ref this.storage;
            ref var reg = ref storage.GetRegistry<TComponent>(in this.allocator);
            return reg.components.Get(ref this.allocator, entityId).version;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool HasChanged(int entityId) {

            // Set as changed only if saved version is equals to current tick
            // So we have a change in this component at current tick
            return this.GetVersion(entityId) == (long)Worlds.current.GetCurrentTick();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void UpdateVersion(in Entity entity) {

            if (AllComponentTypes<TComponent>.isVersioned == true) {
                var v = (long)this.world.GetCurrentTick();
                ref var storage = ref this.storage;
                ref var reg = ref storage.GetRegistry<TComponent>(in this.allocator);
                reg.components.Get(ref this.allocator, entity.id).version = v;
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void UpdateVersion(ref Component<TComponent> bucket) {

            if (AllComponentTypes<TComponent>.isVersioned == true) {
                bucket.version = this.world.GetCurrentTick();
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Merge() {

            ref var storage = ref this.storage;
            ref var reg = ref storage.GetRegistry<TComponent>(in this.allocator);
            reg.Merge(ref this.allocator);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        protected override StructRegistryBase SpawnInstance() {

            return PoolRegistries.SpawnUnmanaged<TComponent>();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Validate(int capacity) {

            //ref var storage = ref this.storage;
            //ref var reg = ref storage.GetRegistry<TComponent>();
            //var resized = reg.Validate(ref storage.allocator, capacity);
            this.storage.Validate<TComponent>(ref this.allocator, capacity);
            return base.Validate(capacity);
            //return resized;

        }

        public override IComponentBase GetObject(Entity entity) {

            E.IS_ALIVE(in entity);

            if (this.TryRead(entity, out var comp) == true) {

                return comp;

            }
            
            return null;

        }

        public override bool SetObject(in Entity entity, UnsafeData buffer, StorageType storageType) {
            
            E.IS_ALIVE(in entity);

            return DataUnmanagedBufferUtils.PushSet_INTERNAL(this.world, in entity, this, buffer.Read<TComponent>(in this.allocator), storageType);

        }

        public override bool SetObject(in Entity entity, IComponentBase data, StorageType storageType) {

            E.IS_ALIVE(in entity);

            return DataUnmanagedBufferUtils.PushSet_INTERNAL(this.world, in entity, this, (TComponent)data, storageType);
            
        }

        public override bool RemoveObject(in Entity entity, StorageType storageType) {

            E.IS_ALIVE(in entity);

            ref var storage = ref this.storage;
            ref var reg = ref storage.GetRegistry<TComponent>(in this.allocator);
            return DataUnmanagedBufferUtils.PushRemove_INTERNAL(this.world, in entity, ref this.allocator, ref reg);

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
        public virtual void RemoveData(in Entity entity, ref Component<TComponent> bucket) {

            bucket.data = default;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Has(in Entity entity) {

            return this.Get(in entity).state > 0;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Remove(in Entity entity, bool clearAll = false) {

            ref var bucket = ref this.Get(in entity);
            if (bucket.state > 0) {

                this.RemoveData(in entity, ref bucket);
                
                ref var storage = ref this.storage;
                ref var reg = ref storage.GetRegistry<TComponent>(in this.allocator);
                reg.components.Remove(ref this.allocator, entity.id);
                
                bucket.state = 0;

                return true;

            }

            return false;

        }

        protected override byte CopyFromState(in Entity from, in Entity to) {

            ref var bucket = ref this.Get(in from);
            this.Get(in to) = bucket;
            return bucket.state;

        }
        
    }

}
