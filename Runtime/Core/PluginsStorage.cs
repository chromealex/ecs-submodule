namespace ME.ECS {

    using ME.ECS.Collections.V3;
    using ME.ECS.Collections.MemoryAllocator;

    public interface IPlugin {

        void Initialize(int key, ref MemoryAllocator allocator);
        int GetKey();

    }
    
    public struct PluginsStorage {

        public Dictionary<int, UnsafeData> storages;
        public int nextKey;

        public void Initialize(ref MemoryAllocator allocator) {

            if (this.storages.isCreated == false) {
                this.storages = new Dictionary<int, UnsafeData>(ref allocator, 1);
                this.nextKey = 0;
            }
            
        }

        public ref T GetOrCreate<T>(ref MemoryAllocator allocator) where T : unmanaged, IPlugin {
            
            var key = default(T).GetKey();
            if (this.storages.ContainsKey(in allocator, key) == false) {
                var storage = default(T);
                key = this.Add(ref allocator, key, storage);
            }
            return ref this.storages.GetValue(ref allocator, key).Get<T>(ref allocator);
            
        }

        private int Add<T>(ref MemoryAllocator allocator, int key, T storage) where T : unmanaged, IPlugin {

            if (key == 0) key = ++this.nextKey;
            storage.Initialize(key, ref allocator);
            this.storages.Add(ref allocator, key, new UnsafeData().Set(ref allocator, storage));
            return key;
            
        }

        public ref T Get<T>(ref MemoryAllocator allocator, int key) where T : unmanaged, IPlugin {

            if (key == 0 || this.storages.ContainsKey(in allocator, key) == false) {
                this.GetOrCreate<T>(ref allocator);
                //throw new System.Collections.Generic.KeyNotFoundException();
            }
            return ref this.storages.GetValue(ref allocator, key).Get<T>(ref allocator);

        }

    }

}