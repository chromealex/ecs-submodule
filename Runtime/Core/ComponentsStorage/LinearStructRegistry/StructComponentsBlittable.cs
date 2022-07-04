namespace ME.ECS {

    using Collections;

    public interface IComponentsBlittable {

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public partial class StructComponentsBlittable<TComponent> : StructComponentsBase<TComponent>, IComponentsBlittable where TComponent : struct, IComponentBase {

        //[ME.ECS.Serializer.SerializeField]
        //internal NativeBufferArraySliced<Component<TComponent>> components;
        //[ME.ECS.Serializer.SerializeField]
        //private long maxVersion;

        private ref UnmanagedComponentsStorage unmanagedStorage => ref this.world.currentState.structComponents.unmanagedComponentsStorage;
        
        public override UnsafeData CreateObjectUnsafe(in Entity entity) {
            
            ref var storage = ref this.unmanagedStorage;
            ref var reg = ref storage.GetRegistry<TComponent>();
            return new UnsafeData().SetAsUnmanaged(reg.components[in storage.allocator, entity.id].data);

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
        public override long GetVersion(in Entity entity) {

            ref var storage = ref this.unmanagedStorage;
            ref var reg = ref storage.GetRegistry<TComponent>();
            return reg.components[in storage.allocator, entity.id].version;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override long GetVersion(int entityId) {

            ref var storage = ref this.unmanagedStorage;
            ref var reg = ref storage.GetRegistry<TComponent>();
            return reg.components[in storage.allocator, entityId].version;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool HasChanged(int entityId) {

            // Set as changed only if saved version is equals to current tick
            // So we have a change in this component at current tick
            ref var storage = ref this.unmanagedStorage;
            ref var reg = ref storage.GetRegistry<TComponent>();
            return reg.components[in storage.allocator, entityId].version == (long)Worlds.current.GetCurrentTick();

        }

        public override int GetCustomHash() {

            var hash = 0;
            if (typeof(TComponent) == typeof(ME.ECS.Transform.Position)) {

                ref var storage = ref this.unmanagedStorage;
                ref var reg = ref storage.GetRegistry<TComponent>();
                
                for (int i = 0; i < reg.components.Length; ++i) {

                    var p = (ME.ECS.Transform.Position)(object)reg.components[in storage.allocator, i].data;
                    hash ^= (int)(p.value.x * 100000f);
                    hash ^= (int)(p.value.y * 100000f);
                    hash ^= (int)(p.value.z * 100000f);

                }

            }

            return hash;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void UpdateVersion(in Entity entity) {

            /*if (AllComponentTypes<TComponent>.isVersioned == true) {
                var v = (long)this.world.GetCurrentTick();
                ref var data = ref this.components[entity.id];
                data.version = v;
                this.maxVersion = (v > this.maxVersion ? v : this.maxVersion);
            }*/

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void UpdateVersion(ref Component<TComponent> bucket) {

            /*if (AllComponentTypes<TComponent>.isVersioned == true) {
                bucket.version = this.world.GetCurrentTick();
                this.maxVersion = (bucket.version > this.maxVersion ? bucket.version : this.maxVersion);
            }*/

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Merge() {

            //this.components = this.components.Merge();
            this.unmanagedStorage.Merge<TComponent>();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        protected override StructRegistryBase SpawnInstance() {

            return PoolRegistries.SpawnBlittable<TComponent>();

        }

        public override void OnRecycle() {

            //this.components = this.components.Dispose();
            //this.maxVersion = default;
            base.OnRecycle();
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Validate(int capacity) {

            //this.components = this.components.Resize(capacity, true, out var resized);
            //return resized;
            this.unmanagedStorage.Validate<TComponent>(capacity);
            return base.Validate(capacity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Validate(in Entity entity) {

            //this.components = this.components.Resize(entity.id, true, out var resized);
            this.unmanagedStorage.Validate<TComponent>(entity.id);
            return base.Validate(entity);

        }

        public override IComponentBase GetObject(Entity entity) {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            var index = entity.id;
            
            ref var storage = ref this.unmanagedStorage;
            ref var reg = ref storage.GetRegistry<TComponent>();
            ref var bucket = ref reg.components[in storage.allocator, entity.id];
            //ref var bucket = ref this.components[index];
            if (bucket.state > 0) {

                return bucket.data;

            }

            return null;

        }

        public override bool SetObject(in Entity entity, UnsafeData buffer, StorageType storageType) {
            
            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            ref var storage = ref this.unmanagedStorage;
            ref var reg = ref storage.GetRegistry<TComponent>();
            return DataBlittableBufferAllocatorUtils.PushSet_INTERNAL(this.world, in entity, ref storage, ref reg, buffer.Read<TComponent>(), storageType);

        }

        public override bool SetObject(in Entity entity, IComponentBase data, StorageType storageType) {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            ref var storage = ref this.unmanagedStorage;
            ref var reg = ref storage.GetRegistry<TComponent>();
            return DataBlittableBufferAllocatorUtils.PushSet_INTERNAL(this.world, in entity, ref storage, ref reg, (TComponent)data, storageType);
            
        }

        public override bool RemoveObject(in Entity entity, StorageType storageType) {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            ref var storage = ref this.unmanagedStorage;
            ref var reg = ref storage.GetRegistry<TComponent>();
            return DataBlittableBufferAllocatorUtils.PushRemove_INTERNAL(this.world, in entity, ref storage, ref reg, storageType);

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

            ref var storage = ref this.unmanagedStorage;
            ref var reg = ref storage.GetRegistry<TComponent>();
            return reg.components[in storage.allocator, entity.id].state > 0;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Remove(in Entity entity, bool clearAll = false) {

            ref var storage = ref this.unmanagedStorage;
            ref var reg = ref storage.GetRegistry<TComponent>();
            
            var index = entity.id;
            if (index >= reg.components.Length) return false;
            ref var bucket = ref reg.components[in storage.allocator, index];
            if (bucket.state > 0) {

                this.RemoveData(in entity, ref bucket);
                bucket.state = 0;

                if (clearAll == false) {

                    this.world.currentState.storage.archetypes.Remove<TComponent>(in entity);

                }

                return true;

            }

            return false;

        }

        protected override byte CopyFromState(in Entity from, in Entity to) {

            //ref var bucket = ref this.components[from.id];
            //this.components[to.id] = bucket;
            //return bucket.state;
            return this.unmanagedStorage.CopyFromState<TComponent>(in from, in to);

        }
        
        public override void CopyFrom(StructRegistryBase other) {

            base.CopyFrom(other);
            //var _other = (StructComponentsBlittable<TComponent>)other;
            //NativeArrayUtils.Copy(in _other.components, ref this.components);
            //this.maxVersion = _other.maxVersion;

        }
        
    }

}