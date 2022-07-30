#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS {

    using ME.ECS.Collections;

    public interface IStructRegistryBase {

        IComponentBase GetObject(Entity entity);
        bool SetObject(in Entity entity, IComponentBase data, StorageType storageType);
        #if !SHARED_COMPONENTS_DISABLED
        IComponentBase GetSharedObject(Entity entity, uint groupId);
        bool SetSharedObject(in Entity entity, IComponentBase data, uint groupId);
        bool RemoveObject(in Entity entity, StorageType storageType);
        #endif
        bool HasType(System.Type type);

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract partial class StructRegistryBase : IStructRegistryBase, IPoolableRecycle {

        public World world {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                return Worlds.current;
            }
        }

        public abstract bool IsNeedToDispose();

        public abstract int GetTypeBit();
        public abstract int GetAllTypeBit();
        public abstract bool IsTag();

        public abstract long GetVersion(int entityId);
        public abstract bool HasChanged(int entityId);

        public abstract void UpdateVersion(in Entity entity);
        
        #if !COMPONENTS_VERSION_NO_STATE_DISABLED
        public abstract void UpdateVersionNoState(in Entity entity);
        #endif
        
        public abstract bool HasType(System.Type type);
        public abstract IComponentBase GetObject(Entity entity);
        public abstract bool SetObject(in Entity entity, IComponentBase data, StorageType storageType);
        public abstract bool SetObject(in Entity entity, UnsafeData buffer, StorageType storageType);
        public abstract bool RemoveObject(in Entity entity, StorageType storageType);
        #if !SHARED_COMPONENTS_DISABLED
        public abstract System.Collections.Generic.ICollection<uint> GetSharedGroups(Entity entity);
        public abstract IComponentBase GetSharedObject(Entity entity, uint groupId);
        public abstract bool SetSharedObject(in Entity entity, IComponentBase data, uint groupId);
        #endif

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void Merge();

        public abstract void CopyFrom(StructRegistryBase other);

        public abstract void CopyFrom(in Entity from, in Entity to);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract bool Validate(int capacity);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract bool Has(in Entity entity);

        #if !SHARED_COMPONENTS_DISABLED
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract bool HasShared(in Entity entity, uint groupId);
        #endif

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract bool Remove(in Entity entity, bool clearAll = false);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void OnRecycle();

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void Recycle();

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract StructRegistryBase Clone();

        public abstract int GetCustomHash();

        public virtual UnsafeData CreateObjectUnsafe(in Entity entity) {
            throw new System.Exception("Object must be unmanaged to use CreateObjectUnsafe");
        }

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract partial class StructComponentsBase<TComponent> : StructRegistryBase where TComponent : struct, IComponentBase {

        protected ref ME.ECS.Collections.V3.MemoryAllocator allocator => ref this.world.currentState.allocator;
        
        #if !COMPONENTS_VERSION_NO_STATE_DISABLED
        // We don't need to serialize this field
        internal BufferArray<uint> versionsNoState;
        #endif
        #if !SHARED_COMPONENTS_DISABLED
        // Shared data
        [ME.ECS.Serializer.SerializeField]
        internal SharedDataStorageGroup<TComponent> sharedStorage;
        #endif
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public abstract bool TryRead(in Entity entity, out TComponent component);

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public abstract ref Component<TComponent> Get(in Entity entity);

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public abstract ref byte GetState(in Entity entity);

        public override int GetCustomHash() => 0;

        public virtual void UpdateVersion(ref Component<TComponent> bucket) {
            
            if (AllComponentTypes<TComponent>.isVersioned == true) {
                bucket.version = this.world.GetCurrentTick();
            }

        }
        
        #if !COMPONENTS_VERSION_NO_STATE_DISABLED
        public uint GetVersionNotStated(in Entity entity) {

            return this.versionsNoState.arr[entity.id];

        }
        #endif

        public override bool IsTag() {

            return WorldUtilities.IsComponentAsTag<TComponent>();

        }

        public override int GetTypeBit() {

            return WorldUtilities.GetComponentTypeId<TComponent>();

        }

        public override int GetAllTypeBit() {

            return WorldUtilities.GetAllComponentTypeId<TComponent>();

        }

        #if !COMPONENTS_VERSION_NO_STATE_DISABLED
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void UpdateVersionNoState(in Entity entity) {

            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ++this.versionsNoState.arr[entity.id];

        }
        #endif

        public override StructRegistryBase Clone() {

            var reg = this.SpawnInstance();
            ME.WeakRef.Reg(reg);
            this.Merge();
            reg.CopyFrom(this);
            return reg;

        }

        protected virtual StructRegistryBase SpawnInstance() {

            return PoolRegistries.Spawn<TComponent>();

        }

        public override void OnRecycle() {

            #if !COMPONENTS_VERSION_NO_STATE_DISABLED
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) PoolArray<uint>.Recycle(ref this.versionsNoState);
            #endif
            #if !SHARED_COMPONENTS_DISABLED
            if (AllComponentTypes<TComponent>.isShared == true) SharedGroupsAPI<TComponent>.OnRecycle(ref this.sharedStorage);
            #endif
            
        }

        public override bool Validate(int capacity) {
            
            #if !SHARED_COMPONENTS_DISABLED
            if (AllComponentTypes<TComponent>.isShared == true) SharedGroupsAPI<TComponent>.Validate(ref this.sharedStorage, capacity);
            #endif
            #if !COMPONENTS_VERSION_NO_STATE_DISABLED
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) ArrayUtils.Resize(capacity, ref this.versionsNoState, true);
            #endif
            
            return false;

        }

        public override bool HasType(System.Type type) {

            return typeof(TComponent) == type;

        }

        #if !SHARED_COMPONENTS_DISABLED
        public override System.Collections.Generic.ICollection<uint> GetSharedGroups(Entity entity) {

            return SharedGroupsAPI<TComponent>.GetGroups(ref this.sharedStorage);

        }

        public override IComponentBase GetSharedObject(Entity entity, uint groupId) {

            if (SharedGroupsAPI<TComponent>.Has(ref this.sharedStorage, entity.id, groupId) == false) return null;
            return SharedGroupsAPI<TComponent>.Get(ref this.sharedStorage, entity.id, groupId);

        }

        public override bool SetSharedObject(in Entity entity, IComponentBase data, uint groupId) {
            
            E.IS_ALIVE(in entity);

            SharedGroupsAPI<TComponent>.Set(ref this.sharedStorage, entity.id, groupId, (TComponent)data);
            
            var componentIndex = ComponentTypes<TComponent>.typeId;
            if (componentIndex >= 0) { }

            return true;
            
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override bool HasShared(in Entity entity, uint groupId) {

            E.IS_ALIVE(in entity);

            return SharedGroupsAPI<TComponent>.Has(ref this.sharedStorage, entity.id, groupId);

        }
        #endif

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void Replace(ref Component<TComponent> bucket, in TComponent data);
        
        #if !SHARED_COMPONENTS_DISABLED
        private struct ElementCopy : IArrayElementCopy<SharedDataStorage<TComponent>> {

            public void Copy(in SharedDataStorage<TComponent> @from, ref SharedDataStorage<TComponent> to) {
                
                to.data = from.data;
                ArrayUtils.Copy(from.states, ref to.states);
                
            }

            public void Recycle(ref SharedDataStorage<TComponent> item) {
                
                PoolArray<bool>.Recycle(ref item.states);
                item.data = default;
                item = default;
                
            }

        }
        #endif

        public override unsafe void CopyFrom(in Entity from, in Entity to) {

            if (typeof(TComponent) == typeof(ME.ECS.Views.ViewComponent)) {

                var view = from.Read<ME.ECS.Views.ViewComponent>();
                if (view.viewInfo.entity == from) {

                    to.ReplaceView(view.viewInfo.prefabSourceId);

                }

                return;

            }

            {
                var tempList = stackalloc StructComponentsContainer.NextTickTask[this.world.currentState.structComponents.nextTickTasks.Count(in this.world.currentState.allocator)];
                var e = this.world.currentState.structComponents.nextTickTasks.GetEnumerator(in this.world.currentState.allocator);
                var k = 0;
                while (e.MoveNext() == true) {
                    var task = e.Current;
                    if (task.entity == from) {
                        var item = task;
                        item.entity = to;
                        tempList[k++] = item;
                    }
                }

                e.Dispose();

                for (int i = 0; i < k; ++i) {
                    this.world.currentState.structComponents.nextTickTasks.Add(ref this.world.currentState.allocator, tempList[i]);
                }
            }

            #if !SHARED_COMPONENTS_DISABLED
            if (AllComponentTypes<TComponent>.isShared == true) SharedGroupsAPI<TComponent>.CopyFrom(ref this.sharedStorage, in from, in to);
            #endif
            if (this.CopyFromState(in from, in to) > 0) {

                if (ComponentTypes<TComponent>.typeId >= 0) { }

            } else {

                if (ComponentTypes<TComponent>.typeId >= 0) { }

            }

        }

        protected abstract byte CopyFromState(in Entity from, in Entity to);

        public override void CopyFrom(StructRegistryBase other) {

            var _other = (StructComponentsBase<TComponent>)other;
            #if !COMPONENTS_VERSION_NO_STATE_DISABLED
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) _other.versionsNoState = this.versionsNoState;
            #endif
            #if !SHARED_COMPONENTS_DISABLED
            if (AllComponentTypes<TComponent>.isShared == true) SharedGroupsAPI<TComponent>.CopyFrom(ref this.sharedStorage, _other.sharedStorage, new ElementCopy());
            #endif
            
        }

    }
    
}
