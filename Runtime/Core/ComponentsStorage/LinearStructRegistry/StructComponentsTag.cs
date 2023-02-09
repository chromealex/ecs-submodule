namespace ME.ECS {

    using Collections;

    public interface IComponentsTag {

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class StructComponentsTag<TComponent> : StructComponentsBase<TComponent>, IComponentsTag where TComponent : struct, IComponentBase {

        [ME.ECS.Serializer.SerializeField]
        internal NativeBufferArraySliced<byte> componentStates;

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override ref byte GetState(in Entity entity) {

            return ref this.componentStates[entity.id];

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override long ReadPtr(in Entity entity) {
            throw new System.NotImplementedException();
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override bool TryRead(in Entity entity, out TComponent component) {
            throw new System.NotImplementedException();
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override ref readonly TComponent Read(in Entity entity) {
            throw new System.NotImplementedException();
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override ref Component<TComponent> Get(in Entity entity) {
            throw new System.NotImplementedException();
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
        public override void Merge() {

            this.componentStates = this.componentStates.Merge();

        }

        protected override StructRegistryBase SpawnInstance() {

            return PoolRegistries.SpawnTag<TComponent>();

        }

        public override void OnRecycle() {

            this.componentStates = this.componentStates.Dispose();
            base.OnRecycle();
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Validate(int capacity) {

            this.componentStates = this.componentStates.Resize(capacity, true, out var resized);
            base.Validate(capacity);
            return resized;

        }

        public override bool SetObject(in Entity entity, UnsafeData buffer, StorageType storageType) {
            
            E.IS_ALIVE(in entity);

            return DataTagBufferUtils.PushSet_INTERNAL(this.world, in entity, this, buffer.Read<TComponent>(in this.allocator), storageType);

        }

        public override bool SetObject(in Entity entity, IComponentBase data, StorageType storageType) {

            E.IS_ALIVE(in entity);

            return DataTagBufferUtils.PushSet_INTERNAL(this.world, in entity, this, (TComponent)data, storageType);
            
        }

        public override bool RemoveObject(in Entity entity, StorageType storageType) {

            E.IS_ALIVE(in entity);

            return DataTagBufferUtils.PushRemove_INTERNAL(this.world, in entity, this, storageType);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Has(in Entity entity) {

            return this.componentStates[entity.id] > 0;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool Remove(in Entity entity, bool clearAll = false) {

            var index = entity.id;
            if (index >= this.componentStates.Length) return false;
            ref var bucket = ref this.componentStates[index];
            if (bucket > 0) {

                bucket = 0;

                if (clearAll == false) { }

                return true;

            }

            return false;

        }

        protected override byte CopyFromState(in Entity from, in Entity to) {

            ref var bucket = ref this.componentStates[from.id];
            this.componentStates[to.id] = bucket;
            return bucket;

        }
        
        public override void CopyFrom(StructRegistryBase other) {

            base.CopyFrom(other);
            var _other = (StructComponentsTag<TComponent>)other;
            NativeArrayUtils.Copy(in _other.componentStates, ref this.componentStates);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public virtual void RemoveData(in Entity entity, ref byte state) {

            state = 0;

        }

        public override IComponentBase GetObject(Entity entity) {
            throw new System.NotImplementedException();
        }

        public override void Replace(ref Component<TComponent> bucket, in TComponent data) {
            throw new System.NotImplementedException();
        }

        public override long GetVersion(int entityId) {
            throw new System.NotImplementedException();
        }

        public override bool HasChanged(int entityId) {
            throw new System.NotImplementedException();
        }

        public override void UpdateVersion(in Entity entity) {
            throw new System.NotImplementedException();
        }
        
        #if !ENTITIES_GROUP_DISABLED
        public override void Set(in EntitiesGroup @group, TComponent component, bool setBits = true) {
            for (int i = group.fromId; i <= group.toId; ++i) {
                var entityId = i;
                var entity = this.world.currentState.storage.cache[in this.world.currentState.allocator, entityId];
                DataTagBufferUtils.PushSet_INTERNAL(this.world, in entity, this, component, StorageType.Default);
            }
        }

        public override void SetObject(in EntitiesGroup @group, IComponentBase data, bool setBits = true) {
            for (int i = group.fromId; i <= group.toId; ++i) {
                var entityId = i;
                var entity = this.world.currentState.storage.cache[in this.world.currentState.allocator, entityId];
                DataTagBufferUtils.PushSet_INTERNAL(this.world, in entity, this, (TComponent)data, StorageType.Default);
            }
        }

        public override void Remove(in EntitiesGroup @group, bool setBits = true) {
            for (int i = group.fromId; i <= group.toId; ++i) {
                var entityId = i;
                var entity = this.world.currentState.storage.cache[in this.world.currentState.allocator, entityId];
                DataTagBufferUtils.PushRemove_INTERNAL(this.world, in entity, this, StorageType.Default);
            }
        }
        #endif

    }

}