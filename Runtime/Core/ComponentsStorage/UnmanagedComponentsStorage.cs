using ME.ECS.Collections;
using ME.ECS.Extensions;

namespace ME.ECS {
    
    using Collections.V3;
    using MemPtr = System.Int64;

    public struct UnmanagedComponentsStorage {

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct Item<T> where T : struct, IComponentBase {

            public Collections.MemoryAllocator.SparseSet<Component<T>> components;
            
            public void Merge(ref MemoryAllocator allocator) {

                this.components.Merge(ref allocator);

            }

            public bool Validate(ref MemoryAllocator allocator, int entityId) {

                var resized = false;
                if (this.components.isCreated == false) {
                    this.components = new ME.ECS.Collections.MemoryAllocator.SparseSet<Component<T>>(ref allocator, entityId + 1);
                    resized = true;
                } else {
                    this.components.Validate(ref allocator, entityId + 1);
                }
                return resized;

            }

            public void Replace(ref Component<T> bucket, in T data) {

                this.DisposeData(ref bucket);
                bucket.data = data;

            }

            public void RemoveData(ref Component<T> bucket) {

                this.DisposeData(ref bucket);
                bucket.data = default;

            }

            private void DisposeData(ref Component<T> bucket) {
                
                
                
            }

        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct ItemDisposable<T> where T : struct, IComponentDisposable<T> {

            public Collections.MemoryAllocator.SparseSet<Component<T>> components;
            
            public void Merge(ref MemoryAllocator allocator) {

                this.components.Merge(ref allocator);

            }

            public bool Validate(ref MemoryAllocator allocator, int entityId) {

                var resized = false;
                if (this.components.isCreated == false) {
                    this.components = new ME.ECS.Collections.MemoryAllocator.SparseSet<Component<T>>(ref allocator, entityId + 1);
                    resized = true;
                } else {
                    this.components.Validate(ref allocator, entityId + 1);
                }
                return resized;

            }

            public void Replace(ref MemoryAllocator allocator, ref Component<T> bucket, in T data) {

                bucket.data.ReplaceWith(ref allocator, in data);

            }

            public void RemoveData(ref MemoryAllocator allocator, ref Component<T> bucket) {

                if (bucket.state > 0) bucket.data.OnDispose(ref allocator);
                bucket.data = default;

            }

        }

        public MemArrayAllocator<MemPtr> items;

        public void Initialize() {

        }

        public ref Item<T> GetRegistry<T>(in MemoryAllocator allocator) where T : struct, IComponentBase {

            var typeId = AllComponentTypes<T>.typeId;
            var ptr = this.items[in allocator, typeId];
            return ref allocator.Ref<Item<T>>(ptr);

        }

        public void ValidateTypeId<T>(ref MemoryAllocator allocator) where T : struct, IComponentBase {

            this.items.Resize(ref allocator, AllComponentTypesCounter.counter + 1);
            
            var typeId = AllComponentTypes<T>.typeId;
            var ptr = this.items[in allocator, typeId];
            AllComponentTypes<T>.burstTypeStorageDirectRef.Data = ptr;

        }

        public void Validate<T>(ref MemoryAllocator allocator, int entityId) where T : struct, IComponentBase {

            var typeId = AllComponentTypes<T>.typeId;
            this.items.Resize(ref allocator, typeId + 1);
            var ptr = this.items[in allocator, typeId];
            if (ptr == 0L) {
                ptr = this.items[in allocator, typeId] = allocator.Alloc<Item<T>>();
            }
            AllComponentTypes<T>.burstTypeStorageDirectRef.Data = ptr;
            var item = allocator.Ref<Item<T>>(ptr);
            item.Validate(ref allocator, entityId);
            allocator.Ref<Item<T>>(ptr) = item;

        }

        public ref ItemDisposable<T> GetRegistryDisposable<T>(in MemoryAllocator allocator) where T : struct, IComponentDisposable<T> {

            var typeId = AllComponentTypes<T>.typeId;
            var ptr = this.items[in allocator, typeId];
            return ref allocator.Ref<ItemDisposable<T>>(ptr);

        }

        public void ValidateTypeIdDisposable<T>(ref MemoryAllocator allocator, int typeId) where T : struct, IComponentDisposable<T> {

            this.items.Resize(ref allocator, AllComponentTypesCounter.counter + 1);

        }

        public void ValidateDisposable<T>(ref MemoryAllocator allocator, int entityId) where T : struct, IComponentDisposable<T> {

            var typeId = AllComponentTypes<T>.typeId;
            this.items.Resize(ref allocator, typeId + 1);
            var ptr = this.items[in allocator, typeId];
            if (ptr == 0L) {
                ptr = this.items[in allocator, typeId] = allocator.Alloc<ItemDisposable<T>>();
            }
            AllComponentTypes<T>.burstTypeStorageDirectRef.Data = ptr;
            var item = allocator.Ref<ItemDisposable<T>>(ptr);
            item.Validate(ref allocator, entityId);
            allocator.Ref<ItemDisposable<T>>(ptr) = item;

        }

    }

}
