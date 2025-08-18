using ME.ECS.Collections;
using ME.ECS.Extensions;

namespace ME.ECS {
    
    using Collections.LowLevel;
    using Collections.LowLevel.Unsafe;

    public struct UnmanagedComponentsStorage {

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct Item<T> where T : struct, IComponentBase {

            public Collections.LowLevel.SparseSet<Component<T>> components;
            
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Merge(ref MemoryAllocator allocator) {

                this.components.Merge(ref allocator);

            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public bool HasMerge(in MemoryAllocator allocator) {

                return this.components.HasMerge(in allocator);
                
            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public bool Validate(ref MemoryAllocator allocator, int entityId) {

                var resized = false;
                if (this.components.isCreated == false) {
                    this.components = new ME.ECS.Collections.LowLevel.SparseSet<Component<T>>(ref allocator, entityId + 1);
                    resized = true;
                } else {
                    this.components.Validate(ref allocator, entityId + 1);
                }
                return resized;

            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Replace(ref Component<T> bucket, in T data) {

                this.DisposeData(ref bucket);
                bucket.data = data;

            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void RemoveData(ref Component<T> bucket) {

                this.DisposeData(ref bucket);
                bucket.data = default;

            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            private void DisposeData(ref Component<T> bucket) {
                
                
                
            }

        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct ItemDisposable<T> where T : struct, IComponentDisposable<T> {

            public Collections.LowLevel.SparseSet<Component<T>> components;
            
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Merge(ref MemoryAllocator allocator) {

                this.components.Merge(ref allocator);

            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public bool Validate(ref MemoryAllocator allocator, int entityId) {

                var resized = false;
                if (this.components.isCreated == false) {
                    this.components = new ME.ECS.Collections.LowLevel.SparseSet<Component<T>>(ref allocator, entityId + 1);
                    resized = true;
                } else {
                    this.components.Validate(ref allocator, entityId + 1);
                }
                return resized;

            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Replace(ref MemoryAllocator allocator, ref Component<T> bucket, in T data) {

                bucket.data.ReplaceWith(ref allocator, in data);

            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void RemoveData(ref MemoryAllocator allocator, ref Component<T> bucket) {

                if (bucket.state > 0) bucket.data.OnDispose(ref allocator);
                bucket.data = default;

            }

        }

        public MemArrayAllocator<MemPtr> items;

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Initialize() {

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public readonly ref Item<T> GetRegistry<T>(in MemoryAllocator allocator) where T : struct, IComponentBase {

            var typeId = AllComponentTypes<T>.typeId;
            var ptr = this.items[in allocator, typeId];
            return ref allocator.Ref<Item<T>>(ptr);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public MemPtr GetRegistryPtr<T>(in MemoryAllocator allocator) where T : struct, IComponentBase {

            var typeId = AllComponentTypes<T>.typeId;
            return this.items[in allocator, typeId];

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ValidateTypeId<T>(ref MemoryAllocator allocator) where T : struct, IComponentBase {

            this.items.Resize(ref allocator, AllComponentTypesCounter.counter + 1);
            
            var typeId = AllComponentTypes<T>.typeId;
            var ptr = this.items[in allocator, typeId];
            AllComponentTypes<T>.burstTypeStorageDirectRef.Data = ptr;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Validate<T>(ref MemoryAllocator allocator, int entityId) where T : struct, IComponentBase {

            var typeId = AllComponentTypes<T>.typeId;
            this.items.Resize(ref allocator, typeId + 1);
            var ptr = this.items[in allocator, typeId];
            if (ptr == MemPtr.Invalid) {
                ptr = this.items[in allocator, typeId] = allocator.Alloc<Item<T>>();
                allocator.MemClear<Item<T>>(ptr);
            }
            AllComponentTypes<T>.burstTypeStorageDirectRef.Data = ptr;
            var item = allocator.Ref<Item<T>>(ptr);
            item.Validate(ref allocator, entityId);
            allocator.Ref<Item<T>>(ptr) = item;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ref ItemDisposable<T> GetRegistryDisposable<T>(in MemoryAllocator allocator) where T : struct, IComponentDisposable<T> {

            var typeId = AllComponentTypes<T>.typeId;
            var ptr = this.items[in allocator, typeId];
            return ref allocator.Ref<ItemDisposable<T>>(ptr);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ValidateTypeIdDisposable<T>(ref MemoryAllocator allocator, int typeId) where T : struct, IComponentDisposable<T> {

            this.items.Resize(ref allocator, AllComponentTypesCounter.counter + 1);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ValidateDisposable<T>(ref MemoryAllocator allocator, int entityId) where T : struct, IComponentDisposable<T> {

            var typeId = AllComponentTypes<T>.typeId;
            this.items.Resize(ref allocator, typeId + 1);
            var ptr = this.items[in allocator, typeId];
            if (ptr == MemPtr.Invalid) {
                ptr = this.items[in allocator, typeId] = allocator.Alloc<ItemDisposable<T>>();
                allocator.MemClear<ItemDisposable<T>>(ptr);
            }
            AllComponentTypes<T>.burstTypeStorageDirectRef.Data = ptr;
            var item = allocator.Ref<ItemDisposable<T>>(ptr);
            item.Validate(ref allocator, entityId);
            allocator.Ref<ItemDisposable<T>>(ptr) = item;

        }

    }

}
