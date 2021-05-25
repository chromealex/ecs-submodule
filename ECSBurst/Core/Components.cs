
namespace ME.ECSBurst {
    
    using Collections;
    using System.Runtime.InteropServices;

    public interface IComponentBase {}
    
    [StructLayout(LayoutKind.Sequential)]
    public struct StructComponentsItem<T> where T : struct {

        private NativeBufferArray<bool> dataExists;
        private NativeBufferArray<T> data;

        public bool Has(int entityId) {

            return this.dataExists[entityId];

        }

        public ref T Get(int entityId) {

            this.dataExists[entityId] = true;
            return ref this.data[entityId];

        }

        public void Set(int entityId, T data) {
            
            this.data[entityId] = data;
            this.dataExists[entityId] = true;

        }

        public bool Remove(int entityId) {
            
            ref var state = ref this.dataExists[entityId];
            var prevState = state;
            this.data[entityId] = default;
            state = false;
            return prevState;

        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct StructComponentsItemUnknown {

        public NativeBufferArray<bool> dataExists;
        public NativeBufferArray<int> data;
        
        public void Dispose() {

            this.data.Dispose();
            this.dataExists.Dispose();

        }

        public void Validate(int entityId) {

            ArrayUtils.Resize(entityId, ref this.data);
            ArrayUtils.Resize(entityId, ref this.dataExists);
            
        }

    }

    public unsafe struct StructComponents {

        public NativeBufferArray<System.IntPtr> list;

        public void Initialize() {
            
        }

        public void Dispose() {

            for (int i = 0; i < this.list.Length; ++i) {
                
                ref var item = ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<StructComponentsItemUnknown>((void*)this.list[i]);
                item.Dispose();

            }
            this.list.Dispose();

        }

        public void RemoveAll(int entityId) {

            for (int i = 0; i < this.list.Length; ++i) {
                
                ref var item = ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<StructComponentsItemUnknown>((void*)this.list[i]);
                item.dataExists[entityId] = false;

            }
            
        }

        public void Validate(int entityId) {
            
            ArrayUtils.Resize(AllComponentTypesCounter.counter, ref this.list);
            
            for (int i = 0; i < this.list.Length; ++i) {
                
                if (this.list[i] == System.IntPtr.Zero) {
    
                    var size = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<StructComponentsItemUnknown>();
                    var allocator = Unity.Collections.Allocator.Persistent;
                    var align = WorldUtilities.GetComponentAlign(i);
                    var ptr = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.Malloc(size, align, allocator);
                    Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemClear(ptr, size);
                    this.list[i] = (System.IntPtr)ptr;
                    
                }
                
                ref var item = ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<StructComponentsItemUnknown>((void*)this.list[i]);
                item.Validate(entityId);

            }

        }

        /*
        public void Validate<T>(int entityId) where T : struct {

            var id = WorldUtilities.GetAllComponentTypeId<T>();
            ArrayUtils.Resize(id, ref this.list);

            if (this.list[id] == System.IntPtr.Zero) {

                var size = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<StructComponentsItem<T>>();
                var allocator = Unity.Collections.Allocator.Persistent;
                var ptr = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.Malloc(size, Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AlignOf<T>(), allocator);
                Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemClear(ptr, size);
                this.list[id] = (System.IntPtr)ptr;
                
            }
            
            {
                
                ref var item = ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<StructComponentsItem<T>>((void*)this.list[id]);
                item.Validate(entityId);

            }

        }*/

        public bool Remove<T>(int entityId) where T : struct {

            var id = WorldUtilities.GetAllComponentTypeId<T>();
            var ptr = this.list[id];
            ref var item = ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<StructComponentsItem<T>>((void*)ptr);
            return item.Remove(entityId);

        }

        public bool Has<T>(int entityId) where T : struct {

            var id = WorldUtilities.GetAllComponentTypeId<T>();
            var ptr = this.list[id];
            ref var item = ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<StructComponentsItem<T>>((void*)ptr);
            return item.Has(entityId);

        }

        public void Set<T>(int entityId, T data) where T : struct {

            var id = WorldUtilities.GetAllComponentTypeId<T>();
            var ptr = this.list[id];
            ref var item = ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<StructComponentsItem<T>>((void*)ptr);
            item.Set(entityId, data);

        }

        public ref T Get<T>(int entityId) where T : struct {
            
            var id = WorldUtilities.GetAllComponentTypeId<T>();
            var ptr = this.list[id];
            ref var item = ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<StructComponentsItem<T>>((void*)ptr);
            return ref item.Get(entityId);

        }

    }

    public struct State {

        public StructComponents components;
        public Storage storage;
        public Filters filters;

        public void Initialize(int capacity) {
            
            this.components.Initialize();
            this.storage.Initialize(capacity);
            this.filters.Initialize();
            
        }

        public void Dispose() {

            this.components.Dispose();
            this.storage.Dispose();
            this.filters.Dispose();

        }

        [Unity.Collections.NotBurstCompatibleAttribute]
        public Entity AddEntity() {

            var willNew = this.storage.WillNew();
            var entity = this.storage.Alloc();
            if (willNew == true) {

                this.storage.archetypes.Validate(in entity);
                this.components.Validate(entity.id);
                // TODO: On new entity - validate all components from generator
                //ComponentsInitializer.Init();

            }
            
            return entity;

        }

        [Unity.Collections.NotBurstCompatibleAttribute]
        public void RemoveEntity(in Entity entity) {

            if (this.storage.Dealloc(in entity) == true) {

                this.components.RemoveAll(entity.id);
                this.filters.OnBeforeEntityDestroy(in entity);
                this.storage.IncrementGeneration(in entity);

            }

        }

        public bool IsAlive(in Entity entity) {

            return this.storage.IsAlive(entity.id, entity.generation);

        }

        public void Set<T>(in Entity entity, T data) where T : struct, IComponentBase {
            
            this.components.Set(entity.id, data);
            this.filters.OnBeforeAddComponent<T>(in entity);
            this.storage.archetypes.Set<T>(in entity);
            this.storage.versions.Increment(entity.id);
            
        }

        public bool Has<T>(in Entity entity) where T : struct, IComponentBase {
            
            return this.components.Has<T>(entity.id);
            
        }

        public void Remove<T>(in Entity entity) where T : struct, IComponentBase {

            if (this.components.Remove<T>(entity.id) == true) {
                
                this.filters.OnBeforeRemoveComponent<T>(in entity);
                this.storage.archetypes.Remove<T>(in entity);
                this.storage.versions.Increment(entity.id);

            }

        }

        public ref T Get<T>(in Entity entity) where T : struct, IComponentBase {

            if (this.components.Has<T>(entity.id) == false) {

                this.filters.OnBeforeAddComponent<T>(in entity);
                this.components.Set<T>(entity.id, default);
                this.storage.versions.Increment(entity.id);

            }
            
            return ref this.components.Get<T>(entity.id);

        }

        public ref readonly T Read<T>(in Entity entity) where T : struct, IComponentBase {
            
            return ref this.components.Get<T>(entity.id);

        }

    }

}
