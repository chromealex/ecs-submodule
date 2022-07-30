using RandomState = System.UInt32;

namespace ME.ECS {

    using Collections.V3;
    
    public abstract class State : IPoolableRecycle {

        [ME.ECS.Serializer.SerializeField]
        public Tick tick;
        [ME.ECS.Serializer.SerializeField]
        public RandomState randomState;
        [ME.ECS.Serializer.SerializeField]
        public Entity sharedEntity;

        [ME.ECS.Serializer.SerializeField]
        public ME.ECS.FiltersArchetype.FiltersArchetypeStorage storage;
        
        public MemoryAllocator allocator;
        #if !ENTITY_TIMERS_DISABLED
        [ME.ECS.Serializer.SerializeField]
        public Timers timers;
        #endif
        [ME.ECS.Serializer.SerializeField]
        public StructComponentsContainer structComponents;
        [ME.ECS.Serializer.SerializeField]
        public GlobalEventStorage globalEvents;
        
        /// <summary>
        /// Return most unique hash
        /// </summary>
        /// <returns></returns>
        public virtual int GetHash() {

            return this.tick ^ this.structComponents.GetHash() ^ this.randomState.GetHashCode() ^ this.storage.GetHash(ref this.allocator);

        }

        public virtual void Initialize(World world, bool freeze, bool restore) {
            
            // Use 512 KB by default
            this.allocator.Initialize(512 * 1024, -1);

            world.Register(ref this.allocator, ref this.storage, freeze, restore);
            world.Register(ref this.allocator, ref this.structComponents, freeze, restore);
            this.globalEvents.Initialize(ref this.allocator);
            #if !ENTITY_TIMERS_DISABLED
            this.timers.Initialize(ref this.allocator);
            #endif
            
        }

        public virtual void CopyFrom(State other) {
            
            this.allocator.CopyFrom(in other.allocator);
            
            this.tick = other.tick;
            this.randomState = other.randomState;
            this.sharedEntity = other.sharedEntity;

            this.storage = other.storage;
            this.structComponents.CopyFrom(other.structComponents);
            this.globalEvents = other.globalEvents;
            #if !ENTITY_TIMERS_DISABLED
            this.timers = other.timers;
            #endif

        }

        public virtual void OnRecycle() {

            this.tick = default;
            this.randomState = default;
            this.sharedEntity = default;
            
            #if !ENTITY_TIMERS_DISABLED
            this.timers.Dispose(ref this.allocator);
            #endif
            this.globalEvents.Dispose(ref this.allocator);
            this.storage.Dispose(ref this.allocator);
            this.structComponents.OnRecycle(ref this.allocator);
            
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

    }

}