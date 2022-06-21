namespace ME.ECS {

    using Collections;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public partial class StructComponents<TComponent> : StructComponentsBase<TComponent> where TComponent : struct, IComponentBase {

        [ME.ECS.Serializer.SerializeField]
        internal BufferArraySliced<Component<TComponent>> components;
        [ME.ECS.Serializer.SerializeField]
        private long maxVersion;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        protected override StructRegistryBase SpawnInstance() {

            return PoolRegistries.Spawn<TComponent>();

        }

        public override bool IsNeedToDispose() {

            return false;

        }

        public override void Recycle() {
            
            PoolRegistries.Recycle(this);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override long GetVersion(in Entity entity) {

            return this.components[entity.id].version;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override long GetVersion(int entityId) {

            return this.components[entityId].version;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool HasChanged(int entityId) {

            // Set as changed only if saved version is equals to current tick
            // So we have a change in this component at current tick
            return this.components[entityId].version == (long)Worlds.current.GetCurrentTick();

        }

        public override int GetCustomHash() {

            var hash = 0;
            if (typeof(TComponent) == typeof(ME.ECS.Transform.Position)) {

                for (int i = 0; i < this.components.Length; ++i) {

                    var p = (ME.ECS.Transform.Position)(object)this.components[i].data;
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

            if (AllComponentTypes<TComponent>.isVersioned == true) {
                var v = (long)this.world.GetCurrentTick();
                ref var data = ref this.components[entity.id];
                data.version = v;
                this.maxVersion = (v > this.maxVersion ? v : this.maxVersion);
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void UpdateVersion(ref Component<TComponent> bucket) {

            if (AllComponentTypes<TComponent>.isVersioned == true) {
                bucket.version = this.world.GetCurrentTick();
                this.maxVersion = (bucket.version > this.maxVersion ? bucket.version : this.maxVersion);
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Merge() {

            this.components = this.components.Merge();

        }

        public override void OnRecycle() {

            this.components = this.components.Dispose();
            this.maxVersion = default;
            base.OnRecycle();
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Validate(int capacity) {

            var resized = ArrayUtils.Resize(capacity, ref this.components, resizeWithOffset: true);
            base.Validate(capacity);
            return resized;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Validate(in Entity entity) {

            var resized = ArrayUtils.Resize(entity.id, ref this.components, true);
            base.Validate(entity);
            return resized;

        }

        public override IComponentBase GetObject(Entity entity) {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            var index = entity.id;
            ref var bucket = ref this.components[index];
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

            return DataBufferUtils.PushSet_INTERNAL(this.world, in entity, this, buffer.Read<TComponent>(), storageType);

        }

        public override bool SetObject(in Entity entity, IComponentBase data, StorageType storageType) {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            return DataBufferUtils.PushSet_INTERNAL(this.world, in entity, this, (TComponent)data, storageType);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Replace(ref Component<TComponent> bucket, in TComponent data) {
            
            bucket.data = data;
            
        }

        public override bool RemoveObject(in Entity entity, StorageType storageType) {

            return DataBufferUtils.PushRemove_INTERNAL(this.world, in entity, this, storageType);

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

            return this.components[entity.id].state > 0;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Remove(in Entity entity, bool clearAll = false) {

            var index = entity.id;
            if (index >= this.components.Length) return false;
            ref var bucket = ref this.components[index];
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

            ref var bucket = ref this.components[from.id];
            this.components[to.id] = bucket;
            return bucket.state;

        }
        
        public override void CopyFrom(StructRegistryBase other) {

            base.CopyFrom(other);
            var _other = (StructComponents<TComponent>)other;
            ArrayUtils.Copy(in _other.components, ref this.components);
            this.maxVersion = _other.maxVersion;

        }
        
    }

}