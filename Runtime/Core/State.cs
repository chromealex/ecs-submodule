using RandomState = System.UInt32;

namespace ME.ECS {

    using Collections.LowLevel.Unsafe;
    
    public abstract class State : IPoolableRecycle {
        
        public static readonly Unity.Burst.SharedStatic<int> stateVersion = Unity.Burst.SharedStatic<int>.GetOrCreate<State>();

        [ME.ECS.Serializer.SerializeField]
        public Tick tick;
        [ME.ECS.Serializer.SerializeField]
        public RandomState randomState;
        [ME.ECS.Serializer.SerializeField]
        public Entity sharedEntity;

        [ME.ECS.Serializer.SerializeField]
        public ME.ECS.FiltersArchetype.FiltersArchetypeStorage storage;
        
        public MemoryAllocator allocator;
        [ME.ECS.Serializer.SerializeField]
        public StructComponentsContainer structComponents;

        public PluginsStorage pluginsStorage;

        public int localVersion = ++State.stateVersion.Data;
        
        /// <summary>
        /// Return most unique hash
        /// </summary>
        /// <returns></returns>
        public virtual int GetHash() {

            return this.tick ^ this.structComponents.GetHash() ^ this.randomState.GetHashCode() ^ this.storage.GetHash(ref this.allocator);

        }

        public virtual string GetHashString() {

            return $"{this.tick}.{this.structComponents.GetHash()}.{this.randomState.GetHashCode()}.{this.storage.GetHash(ref this.allocator)}";

        }

        public virtual void Initialize(World world, bool freeze, bool restore) {
            
            // Use 512 KB by default
            if (this.allocator.isValid == false) this.allocator.Initialize(512 * 1024);

            world.Register(ref this.allocator, ref this.storage, freeze, restore);
            world.Register(ref this.allocator, ref this.structComponents, freeze, restore);
            
            this.pluginsStorage.Initialize(ref this.allocator);

            ComponentTypesRegistry.burstStateVersionsDirectRef.Data = this.storage.versions.GetMemPtr();
            
        }

        public virtual void CopyFrom(State other) {
            
            this.allocator.CopyFrom(in other.allocator);
            
            this.tick = other.tick;
            this.randomState = other.randomState;
            this.sharedEntity = other.sharedEntity;

            this.pluginsStorage = other.pluginsStorage;

            this.storage = other.storage;
            this.structComponents.CopyFrom(other.structComponents);

            ComponentTypesRegistry.burstStateVersionsDirectRef.Data = this.storage.versions.GetMemPtr();

            this.localVersion = ++State.stateVersion.Data;

        }

        public virtual void OnRecycle() {
            
            this.tick = default;
            this.randomState = default;
            this.sharedEntity = default;

            this.pluginsStorage = default;
            this.storage = default;
            
            this.structComponents.OnRecycle(ref this.allocator, true);

            this.localVersion = default;
            
            this.allocator.Dispose();

        }

        public virtual byte[] Serialize<T>() where T : State {
         
            var serializers = ME.ECS.Serializer.ECSSerializers.GetSerializers();
            return ME.ECS.Serializer.Serializer.Pack((T)this, serializers);
            
        }

        public virtual void Deserialize<T>(byte[] bytes) where T : State {
            
            var serializers = ME.ECS.Serializer.ECSSerializers.GetSerializers();
            ME.ECS.Serializer.Serializer.Unpack(bytes, serializers, (T)this);
            
        }

        public virtual void Revalidate() { }

    }

}