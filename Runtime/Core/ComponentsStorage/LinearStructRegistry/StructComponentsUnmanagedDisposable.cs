namespace ME.ECS {

    using Collections;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class StructComponentsUnmanagedDisposable<TComponent> : StructComponentsUnmanaged<TComponent> where TComponent : struct, IComponentDisposable<TComponent> {

        internal struct CopyItem : IArrayElementCopyUnmanaged<Component<TComponent>> {

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Copy(ref ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator, in Component<TComponent> @from, ref Component<TComponent> to) {

                var hasFrom = (from.state > 0);
                var hasTo = (to.state > 0);
                if (hasFrom == false && hasTo == false) return;

                to.state = from.state;
                to.version = from.version;

                if (hasFrom == false && hasTo == true) {
                    
                    to.data.OnDispose(ref allocator);
                    
                } else {

                    to.data.ReplaceWith(ref allocator, in from.data);

                }

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Recycle(ref ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator, ref Component<TComponent> item) {

                item.data.OnDispose(ref allocator);
                item = default;

            }

        }

        private ref UnmanagedComponentsStorage.ItemDisposable<TComponent> registry => ref this.storage.GetRegistryDisposable<TComponent>(in this.allocator);
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override ref byte GetState(in Entity entity) {

            return ref this.registry.components.Get(ref this.allocator, entity.id).state;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override bool TryRead(in Entity entity, out TComponent component) {
            
            var item = this.registry.components.Read(in this.allocator, entity.id);
            component = item.data;
            return item.state > 0;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override ref Component<TComponent> Get(in Entity entity) {

            return ref this.registry.components.Get(ref this.allocator, entity.id);

        }

        public override UnsafeData CreateObjectUnsafe(in Entity entity) {
            
            ref var data = ref this.registry.components.Get(ref this.allocator, entity.id).data;
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

            return this.registry.components.Get(ref this.allocator, entityId).version;

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
                this.registry.components.Get(ref this.allocator, entity.id).version = (ushort)v;
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void UpdateVersion(ref Component<TComponent> bucket) {

            if (AllComponentTypes<TComponent>.isVersioned == true) {
                bucket.version = (ushort)(long)this.world.GetCurrentTick();
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Merge() {

            this.registry.Merge(ref this.allocator);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        protected override StructRegistryBase SpawnInstance() {

            return PoolRegistries.SpawnUnmanagedDisposable<TComponent>();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Validate(int capacity) {

            //ref var storage = ref this.storage;
            //ref var reg = ref storage.GetRegistry<TComponent>();
            //var resized = reg.Validate(ref storage.allocator, capacity);
            this.storage.ValidateDisposable<TComponent>(ref this.allocator, capacity);
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

            return DataUnmanagedDisposableBufferUtils.PushSet_INTERNAL(this.world, in entity, this, buffer.Read<TComponent>(in this.allocator), storageType);

        }

        public override bool SetObject(in Entity entity, IComponentBase data, StorageType storageType) {

            E.IS_ALIVE(in entity);

            return DataUnmanagedDisposableBufferUtils.PushSet_INTERNAL(this.world, in entity, this, (TComponent)data, storageType);
            
        }

        public override bool RemoveObject(in Entity entity, StorageType storageType) {

            E.IS_ALIVE(in entity);

            return DataUnmanagedDisposableBufferUtils.PushRemove_INTERNAL(this.world, in entity, ref this.allocator, ref this.registry);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Replace(ref Component<TComponent> bucket, in TComponent data) {

            if (bucket.state > 0) {
                bucket.data.ReplaceWith(ref Worlds.current.currentState.allocator, in data);
            } else {
                bucket.data.CopyFrom(ref Worlds.current.currentState.allocator, in data);
            }
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void RemoveData(in Entity entity, ref Component<TComponent> bucket) {

            if (bucket.state > 0) bucket.data.OnDispose(ref Worlds.current.currentState.allocator);
            bucket.data = default;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Has(in Entity entity) {

            return this.TryRead(in entity, out var _);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Remove(in Entity entity, bool clearAll = false) {

            ref var bucket = ref this.Get(in entity);
            if (bucket.state > 0) {

                this.RemoveData(in entity, ref bucket);
                
                ref var reg = ref this.storage.GetRegistryDisposable<TComponent>(in this.allocator);
                reg.components.Remove(ref this.allocator, entity.id);
                
                bucket.state = 0;

                return true;

            }

            return false;

        }

        protected override byte CopyFromState(in Entity from, in Entity to) {

            ref var bucket = ref this.Get(in from);
            if (this.Get(in to).state > 0) this.Get(in to).data.OnDispose(ref Worlds.current.currentState.allocator);
            this.Get(in to) = bucket;
            return bucket.state;

        }
        
    }

}
