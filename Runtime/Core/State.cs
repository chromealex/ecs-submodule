using RandomState = System.UInt32;

namespace ME.ECS {

    public abstract class State : IPoolableRecycle {

        [ME.ECS.Serializer.SerializeField]
        public Tick tick;
        [ME.ECS.Serializer.SerializeField]
        public RandomState randomState;
        [ME.ECS.Serializer.SerializeField]
        public Entity sharedEntity;

        #if FILTERS_STORAGE_LEGACY
        public FiltersStorage filters;
        [ME.ECS.Serializer.SerializeField]
        public Storage storage;
        #endif
        
        #if !FILTERS_STORAGE_LEGACY
        [ME.ECS.Serializer.SerializeField]
        public ME.ECS.FiltersArchetype.FiltersArchetypeStorage filters;
        public ref ME.ECS.FiltersArchetype.FiltersArchetypeStorage storage => ref this.filters;
        #endif
        
        [ME.ECS.Serializer.SerializeField]
        public Timers timers;
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
            
            world.Register(ref this.filters, freeze, restore);
            world.Register(ref this.structComponents, freeze, restore);
            #if FILTERS_STORAGE_LEGACY
            world.Register(ref this.storage, freeze, restore);
            #endif
            this.globalEvents.Initialize();
            this.timers.Initialize();
            
        }

        public virtual void CopyFrom(State other) {
            
            this.tick = other.tick;
            this.randomState = other.randomState;
            this.sharedEntity = other.sharedEntity;

            this.filters.CopyFrom(other.filters);
            this.structComponents.CopyFrom(other.structComponents);
            #if FILTERS_STORAGE_LEGACY
            this.storage.CopyFrom(other.storage);
            #endif
            this.globalEvents.CopyFrom(in other.globalEvents);
            this.timers.CopyFrom(in other.timers);

        }

        public virtual void OnRecycle() {

            this.tick = default;
            this.randomState = default;
            this.sharedEntity = default;
            
            this.timers.Dispose();
            this.globalEvents.DeInitialize();
            this.globalEvents = default;
            WorldUtilities.Release(ref this.filters);
            WorldUtilities.Release(ref this.structComponents);
            #if FILTERS_STORAGE_LEGACY
            WorldUtilities.Release(ref this.storage);
            #endif

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