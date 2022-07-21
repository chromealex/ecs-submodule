using RandomState = System.UInt32;

namespace ME.ECS {

    public abstract class State : IPoolableRecycle {

        [ME.ECS.Serializer.SerializeField]
        public Tick tick;
        [ME.ECS.Serializer.SerializeField]
        public RandomState randomState;
        [ME.ECS.Serializer.SerializeField]
        public Entity sharedEntity;

        [ME.ECS.Serializer.SerializeField]
        public ME.ECS.FiltersArchetype.FiltersArchetypeStorage storage;
        
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

            return this.tick ^ this.structComponents.GetHash() ^ this.randomState.GetHashCode() ^ this.storage.GetHashCode();

        }

        public virtual void Initialize(World world, bool freeze, bool restore) {
            
            world.Register(ref this.storage, freeze, restore);
            world.Register(ref this.structComponents, freeze, restore);
            this.globalEvents.Initialize();
            #if !ENTITY_TIMERS_DISABLED
            this.timers.Initialize();
            #endif
            
        }

        public virtual void CopyFrom(State other) {
            
            this.tick = other.tick;
            this.randomState = other.randomState;
            this.sharedEntity = other.sharedEntity;

            this.storage.CopyFrom(other.storage);
            this.structComponents.CopyFrom(other.structComponents);
            this.globalEvents.CopyFrom(in other.globalEvents);
            #if !ENTITY_TIMERS_DISABLED
            this.timers.CopyFrom(in other.timers);
            #endif

        }

        public virtual void OnRecycle() {

            this.tick = default;
            this.randomState = default;
            this.sharedEntity = default;
            
            #if !ENTITY_TIMERS_DISABLED
            this.timers.Dispose();
            #endif
            this.globalEvents.DeInitialize();
            this.globalEvents = default;
            WorldUtilities.Release(ref this.storage);
            WorldUtilities.Release(ref this.structComponents);
            
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