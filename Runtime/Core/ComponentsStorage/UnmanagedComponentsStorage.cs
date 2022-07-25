using ME.ECS.Collections;
using ME.ECS.Extensions;

namespace ME.ECS {
    
    using Collections.V3;
    using MemPtr = System.Int64;

    public struct UnmanagedComponentsStorage {

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct Item<T> where T : struct, IComponentBase {

            public MemArraySlicedAllocator<Component<T>> components;
            public long maxVersion;

            public void Merge(ref MemoryAllocator allocator) {

                this.components.Merge(ref allocator);

            }

            public bool Validate(ref MemoryAllocator allocator, int entityId) {

                var resized = false;
                this.components.Resize(ref allocator, entityId + 1, out resized);
                return resized;

            }

            public void Replace(ref Component<T> bucket, in T data) {

                bucket.data = data;

            }

            public void RemoveData(in Entity entity, ref Component<T> bucket) {

                bucket.data = default;

            }

        }

        public MemArrayAllocator<MemPtr> items;

        public void Initialize() {

        }

        public void Dispose() {
            
            this.items = default;

        }

        /*
        public byte CopyFromState<T>(in Entity from, in Entity to) where T : struct, IComponentBase {

            ref var reg = ref this.GetRegistry<T>();
            return reg.CopyFromState(ref this.allocator, from.id, to.id);

        }*/
        
        public ref Item<T> GetRegistry<T>(in MemoryAllocator allocator) where T : struct, IComponentBase {

            var typeId = AllComponentTypes<T>.typeId;
            var ptr = this.items[in allocator, typeId];
            return ref allocator.Ref<Item<T>>(ptr);

        }

        public void ValidateTypeId<T>(ref MemoryAllocator allocator, int typeId) where T : struct, IComponentBase {

            //UnityEngine.Debug.Log("ValidateTypeId: " + typeId + " :: length: " + this.items.Length);
            this.items.Resize(ref allocator, AllComponentTypesCounter.counter + 1);

        }

        public void Validate<T>(ref MemoryAllocator allocator, int entityId) where T : struct, IComponentBase {

            var typeId = AllComponentTypes<T>.typeId;
            //UnityEngine.Debug.Log("Validate: " + typeId + " :: length: " + this.items.Length);
            this.items.Resize(ref allocator, typeId + 1);
            var ptr = this.items[in allocator, typeId];
            if (ptr == 0L) {
                ptr = this.items[in allocator, typeId] = allocator.Alloc<Item<T>>();
                //var data = new MemArrayAllocatorProxy<MemPtr>(ref this.allocator, this.items);
            }
            //UnityEngine.Debug.Log("Allocator.Ref<>: " + ptr);
            var item = allocator.Ref<Item<T>>(ptr);
            item.Validate(ref allocator, entityId);
            allocator.Ref<Item<T>>(ptr) = item;

        }

    }

}
