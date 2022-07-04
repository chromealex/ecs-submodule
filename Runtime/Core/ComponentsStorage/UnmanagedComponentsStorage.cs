using ME.ECS.Collections;
using ME.ECS.Extensions;

namespace ME.ECS {
    
    using Collections;
    using MemPtr = System.Int64;

    public struct UnmanagedComponentsStorage {

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct Item<T> where T : struct, IComponentBase {

            public Collections.V2.MemArraySlicedAllocator<Component<T>> components;
            public long maxVersion;

            public void Merge(ref Collections.V2.MemoryAllocator allocator) {

                this.components.Merge(ref allocator);

            }

            public void Validate(ref Collections.V2.MemoryAllocator allocator, int entityId) {

                this.components.Resize(ref allocator, entityId + 1, out _);

            }

            public void Replace(in UnmanagedComponentsStorage storage, ref Component<T> bucket, in T data) {

                bucket.data = data;

            }

            public byte CopyFromState(ref Collections.V2.MemoryAllocator allocator, int from, int to) {

                ref var bucket = ref this.components[in allocator, from];
                this.components[in allocator, to] = bucket;
                return bucket.state;

            }

            public void UpdateVersion(in UnmanagedComponentsStorage storage, in Entity entity) {
                
                if (AllComponentTypes<T>.isVersioned == true) {
                    var v = (long)Worlds.current.GetCurrentTick();
                    ref var data = ref this.components[in storage.allocator, entity.id];
                    data.version = v;
                    this.maxVersion = (v > this.maxVersion ? v : this.maxVersion);
                }

            }
            
            public void UpdateVersion(ref Component<T> bucket) {

                if (AllComponentTypes<T>.isVersioned == true) {
                    bucket.version = Worlds.current.GetCurrentTick();
                    this.maxVersion = (bucket.version > this.maxVersion ? bucket.version : this.maxVersion);
                }

            }

            public void RemoveData(in Entity entity, ref Component<T> bucket) {

                bucket.data = default;

            }

        }

        public Collections.V2.MemoryAllocator allocator;
        public Collections.V2.MemArrayAllocator<MemPtr> items;

        public void Initialize() {

            // Use 512 KB by default
            this.allocator.Initialize(512 * 1024 * 2 * 20, -1);

        }

        public void Dispose() {
            
            this.allocator.Dispose();
            
        }

        public void CopyFrom(in UnmanagedComponentsStorage other) {
            
            this.allocator.CopyFrom(in other.allocator);
            this.items = other.items;

        }
        
        public byte CopyFromState<T>(in Entity from, in Entity to) where T : struct, IComponentBase {

            ref var reg = ref this.GetRegistry<T>();
            /*var ptr = this.items[in this.allocator, typeId];
            var size = Component.HEADER_SIZE + Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<T>();
            var addrFrom = ptr.value + from.id * size;
            var addrTo = ptr.value + to.id * size;
            this.allocator.MemCopy(new MemPtr(addrTo), 0, new MemPtr(addrFrom), 0, size);*/
            return reg.CopyFromState(ref this.allocator, from.id, to.id);

        }
        
        public ref Item<T> GetRegistry<T>() where T : struct, IComponentBase {

            var typeId = AllComponentTypes<T>.typeId;
            var ptr = this.items[in this.allocator, typeId];
            return ref this.allocator.Ref<Item<T>>(ptr);

        }

        public void ValidateTypeId<T>(int typeId) where T : struct, IComponentBase {

            //UnityEngine.Debug.Log("ValidateTypeId: " + typeId + " :: length: " + this.items.Length);
            this.items.Resize(ref this.allocator, AllComponentTypesCounter.counter + 1);

        }

        public void Validate<T>(int entityId) where T : struct, IComponentBase {

            var typeId = AllComponentTypes<T>.typeId;
            //UnityEngine.Debug.Log("Validate: " + typeId + " :: length: " + this.items.Length);
            this.items.Resize(ref this.allocator, typeId + 1);
            var ptr = this.items[in this.allocator, typeId];
            if (ptr == 0L) {
                ptr = this.items[in this.allocator, typeId] = this.allocator.Alloc<Item<T>>();
                //var data = new MemArrayAllocatorProxy<MemPtr>(ref this.allocator, this.items);
            }
            //UnityEngine.Debug.Log("Allocator.Ref<>: " + ptr);
            var item = this.allocator.Ref<Item<T>>(ptr);
            item.Validate(ref this.allocator, entityId);
            this.allocator.Ref<Item<T>>(ptr) = item;

        }

        public void Merge<T>() where T : struct, IComponentBase {

            var typeId = AllComponentTypes<T>.typeId;
            var ptr = this.items[in this.allocator, typeId];
            if (ptr == 0L) return;
            this.allocator.Ref<Item<T>>(ptr).Merge(ref this.allocator);
            
        }

    }

}