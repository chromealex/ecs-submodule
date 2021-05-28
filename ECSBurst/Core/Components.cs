
namespace ME.ECSBurst {
    
    using Collections;
    using System.Runtime.InteropServices;
    using Unity.Collections.LowLevel.Unsafe;
    using static MemUtilsCuts;

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

            if (this.data.isCreated == true) this.data.Dispose();
            if (this.dataExists.isCreated == true) this.dataExists.Dispose();

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
            if (this.list.isCreated == true) this.list.Dispose();

        }

        public void RemoveAll(int entityId) {

            for (int i = 0; i < this.list.Length; ++i) {
                
                ref var item = ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<StructComponentsItemUnknown>((void*)this.list[i]);
                item.dataExists[entityId] = false;

            }
            
        }

        public void Validate(int entityId) {
            
            ArrayUtils.Resize(AllComponentTypesCounter.counter.Data, ref this.list);
            
            for (int i = 0; i < this.list.Length; ++i) {
                
                if (this.list[i] == System.IntPtr.Zero) {
    
                    /*var size = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<StructComponentsItemUnknown>();
                    var allocator = Unity.Collections.Allocator.Persistent;
                    var align = WorldUtilities.GetComponentAlign(i);
                    var ptr = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.Malloc(size, align, allocator);
                    Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemClear(ptr, size);
                    this.list[i] = (System.IntPtr)ptr;*/
                    UnityEngine.Debug.LogError(string.Format("Validation failed on index {0}. Do you forget to call Validate<T>?", i));
                    
                }
                
                ref var item = ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<StructComponentsItemUnknown>((void*)this.list[i]);
                item.Validate(entityId);

            }

        }

        public void Validate<T>() where T : struct {

            var id = WorldUtilities.GetAllComponentTypeId<T>();
            ArrayUtils.Resize(id, ref this.list);

            if (this.list[id] == System.IntPtr.Zero) {

                var size = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<StructComponentsItem<T>>();
                var allocator = Unity.Collections.Allocator.Persistent;
                var ptr = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.Malloc(size, Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AlignOf<T>(), allocator);
                Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemClear(ptr, size);
                this.list[id] = (System.IntPtr)ptr;
                
            }
            
        }

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

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct State {

        [NativeDisableUnsafePtrRestriction]
        internal void* components;
        [NativeDisableUnsafePtrRestriction]
        internal void* storage;
        [NativeDisableUnsafePtrRestriction]
        internal void* filters;

        public static State* Create(int entitiesCapacity) {

            var state = new State();
            state.Initialize(entitiesCapacity);
            return tnew(state);

        }

        public void Initialize(int capacity) {

            this.components = pnew(new StructComponents());
            this.storage = pnew(new Storage());
            this.filters = pnew(new Filters());
            
            mref<StructComponents>(this.components).Initialize();
            mref<Storage>(this.storage).Initialize(capacity);
            mref<Filters>(this.filters).Initialize();

        }

        public void Dispose() {

            mref<StructComponents>(this.components).Dispose();
            mref<Storage>(this.storage).Dispose();
            mref<Filters>(this.filters).Dispose();
            
            free(ref this.components);
            free(ref this.storage);
            free(ref this.filters);

        }
        
        public void Validate<T>() where T : struct, IComponentBase {

            ref var c = ref mref<StructComponents>(this.components);
            c.Validate<T>();

        }
        
        public Filter AddFilter(ref Filter filter) {
            
            ref var f = ref mref<Filters>(this.filters);
            return f.Add(ref filter);

        }

        [Unity.Collections.NotBurstCompatibleAttribute]
        public ref readonly Entity AddEntity() {

            ref var st = ref mref<Storage>(this.storage);
            ref var c = ref mref<StructComponents>(this.components);
            
            var willNew = st.WillNew();
            ref var entity = ref st.Alloc();
            if (willNew == true) {

                st.archetypes.Validate(in entity);
                c.Validate(entity.id);

            }
            
            return ref entity;

        }

        [Unity.Collections.NotBurstCompatibleAttribute]
        public bool RemoveEntity(in Entity entity) {

            ref var st = ref mref<Storage>(this.storage);
            ref var c = ref mref<StructComponents>(this.components);
            ref var f = ref mref<Filters>(this.filters);

            if (st.Dealloc(in entity) == true) {

                c.RemoveAll(entity.id);
                f.OnBeforeEntityDestroy(in entity);
                st.IncrementGeneration(in entity);
                return true;

            }
            
            return false;

        }

        public bool IsAlive(in Entity entity) {

            ref var st = ref mref<Storage>(this.storage);
            return st.IsAlive(entity.id, entity.generation);

        }

        public void Set<T>(in Entity entity, T data) where T : struct, IComponentBase {
            
            ref var st = ref mref<Storage>(this.storage);
            ref var c = ref mref<StructComponents>(this.components);
            ref var f = ref mref<Filters>(this.filters);

            c.Set(entity.id, data);
            f.OnBeforeAddComponent<T>(in entity);
            st.archetypes.Set<T>(in entity);
            st.versions.Increment(entity.id);
            
        }

        public bool Has<T>(in Entity entity) where T : struct, IComponentBase {
            
            ref var c = ref mref<StructComponents>(this.components);
            return c.Has<T>(entity.id);
            
        }

        public bool Remove<T>(in Entity entity) where T : struct, IComponentBase {

            ref var st = ref mref<Storage>(this.storage);
            ref var c = ref mref<StructComponents>(this.components);
            ref var f = ref mref<Filters>(this.filters);

            if (c.Remove<T>(entity.id) == true) {
                
                f.OnBeforeRemoveComponent<T>(in entity);
                st.archetypes.Remove<T>(in entity);
                st.versions.Increment(entity.id);
                
                return true;

            }
            
            return false;

        }

        public ref T Get<T>(in Entity entity) where T : struct, IComponentBase {

            ref var st = ref mref<Storage>(this.storage);
            ref var c = ref mref<StructComponents>(this.components);
            ref var f = ref mref<Filters>(this.filters);

            if (c.Has<T>(entity.id) == false) {

                f.OnBeforeAddComponent<T>(in entity);
                c.Set<T>(entity.id, default);
                st.versions.Increment(entity.id);

            }
            
            return ref c.Get<T>(entity.id);

        }

        public ref readonly T Read<T>(in Entity entity) where T : struct, IComponentBase {
            
            ref var c = ref mref<StructComponents>(this.components);
            return ref c.Get<T>(entity.id);

        }

    }

}
