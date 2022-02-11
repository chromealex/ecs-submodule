namespace ME.ECS {

    using Collections;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public partial class StructComponents<TComponent> : StructComponentsBase<TComponent> where TComponent : struct, IStructComponentBase {

        [ME.ECS.Serializer.SerializeField]
        internal BufferArraySliced<Component<TComponent>> components;
        [ME.ECS.Serializer.SerializeField]
        private long maxVersion;

        public override bool IsNeedToDispose() {

            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override long GetVersion(in Entity entity) {

            return this.components[entity.id].version;

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
                this.components[entity.id].version = v;
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

        public override IStructComponentBase GetObject(Entity entity) {

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

            var index = entity.id;
            ref var bucket = ref this.components[index];
            bucket.data = buffer.Read<TComponent>();

            var componentIndex = ComponentTypes<TComponent>.typeId;
            if (bucket.state == 0) {

                bucket.state = 1;

                if (storageType == StorageType.Default) {

                    this.world.currentState.structComponents.entitiesIndexer.Set(entity.id, AllComponentTypes<TComponent>.typeId);

                } else if (storageType == StorageType.NoState) {
                    
                    this.world.structComponentsNoState.entitiesIndexer.Set(entity.id, AllComponentTypes<TComponent>.typeId);

                }

                if (componentIndex >= 0) {
                    
                    this.world.currentState.storage.archetypes.Set<TComponent>(in entity);
                    this.world.AddFilterByStructComponent(in entity, componentIndex);
                    this.world.UpdateFilterByStructComponent(in entity, componentIndex);
                    
                }

                return true;

            }
            
            if (componentIndex >= 0) {
                
                this.world.ValidateFilterByStructComponent(in entity, componentIndex);
                    
            }

            return false;

        }

        public override bool SetObject(in Entity entity, IStructComponentBase data, StorageType storageType) {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            var index = entity.id;
            ref var bucket = ref this.components[index];
            bucket.data = (TComponent)data;

            var componentIndex = ComponentTypes<TComponent>.typeId;
            if (bucket.state == 0) {

                bucket.state = 1;
                
                if (storageType == StorageType.Default) {

                    this.world.currentState.structComponents.entitiesIndexer.Set(entity.id, AllComponentTypes<TComponent>.typeId);

                } else if (storageType == StorageType.NoState) {
                    
                    this.world.structComponentsNoState.entitiesIndexer.Set(entity.id, AllComponentTypes<TComponent>.typeId);

                }

                if (componentIndex >= 0) {
                    
                    this.world.currentState.storage.archetypes.Set<TComponent>(in entity);
                    this.world.AddFilterByStructComponent(in entity, componentIndex);
                    this.world.UpdateFilterByStructComponent(in entity, componentIndex);
                    
                }

                return true;

            }
            
            if (componentIndex >= 0) {
                
                this.world.ValidateFilterByStructComponent(in entity, componentIndex);
                    
            }

            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Replace(ref Component<TComponent> bucket, in TComponent data) {
            
            bucket.data = data;
            
        }

        public override bool RemoveObject(in Entity entity, StorageType storageType) {

            var index = entity.id;
            ref var bucket = ref this.components[index];
            if (bucket.state > 0) {

                this.RemoveData(in entity, ref bucket);
                bucket.state = 0;

                if (storageType == StorageType.Default) {

                    this.world.currentState.structComponents.entitiesIndexer.Remove(entity.id, AllComponentTypes<TComponent>.typeId);

                } else if (storageType == StorageType.NoState) {
                    
                    this.world.structComponentsNoState.entitiesIndexer.Remove(entity.id, AllComponentTypes<TComponent>.typeId);

                }

                if (ComponentTypes<TComponent>.isFilterVersioned == true) this.world.UpdateFilterByStructComponentVersioned<TComponent>(in entity);
                
                var componentIndex = ComponentTypes<TComponent>.typeId;
                if (componentIndex >= 0) {
                    
                    this.world.currentState.storage.archetypes.Remove<TComponent>(in entity);
                    this.world.RemoveFilterByStructComponent<TComponent>(in entity);
                    this.world.UpdateFilterByStructComponent<TComponent>(in entity);
                    
                }

                return true;

            }

            return false;

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