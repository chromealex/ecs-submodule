
namespace ME.ECS {

    public struct IsEntityOneShot : IComponentOneShot {}
    public struct IsEntityEmptyOneShot : IComponentOneShot {}
    
    public static class CoreComponentsInitializer {

        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<IsEntityOneShot>(true, isOneShot: true);
            WorldUtilities.InitComponentTypeId<IsEntityEmptyOneShot>(true, isOneShot: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveData>(false, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveSortedListData>(false, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveListNode>(false, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveHashSetBucket>(false, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveHashSetData>(false, isBlittable: true);
            
            ViewComponentsInitializer.InitTypeId();
            ME.ECS.DataConfigs.DataConfig.InitTypeId();
            TransformComponentsInitializer.InitTypeId();
            NameComponentsInitializer.InitTypeId();
            CameraComponentsInitializer.InitTypeId();
            ME.ECS.DataConfigs.DataConfigComponentsInitializer.InitTypeId();

        }
        
        public static void Init(State state, ref World.NoState noState) {
            
            noState.storage.ValidateOneShot<IsEntityOneShot>(true);
            noState.storage.ValidateOneShot<IsEntityEmptyOneShot>(true);
            state.structComponents.ValidateUnmanaged<ME.ECS.Collections.IntrusiveData>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<ME.ECS.Collections.IntrusiveSortedListData>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<ME.ECS.Collections.IntrusiveListNode>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<ME.ECS.Collections.IntrusiveHashSetBucket>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<ME.ECS.Collections.IntrusiveHashSetData>(ref state.allocator, false);

            ViewComponentsInitializer.Init(state);
            ME.ECS.DataConfigs.DataConfig.Init(state);
            TransformComponentsInitializer.Init(state);
            NameComponentsInitializer.Init(state);
            CameraComponentsInitializer.Init(state);
            ME.ECS.DataConfigs.DataConfigComponentsInitializer.Init(state);
            
        }

        public static void Init(in Entity entity) {
            
            entity.ValidateDataOneShot<IsEntityOneShot>(true);
            entity.ValidateDataOneShot<IsEntityEmptyOneShot>(true);
            entity.ValidateDataUnmanaged<ME.ECS.Collections.IntrusiveData>(false);
            entity.ValidateDataUnmanaged<ME.ECS.Collections.IntrusiveSortedListData>(false);
            entity.ValidateDataUnmanaged<ME.ECS.Collections.IntrusiveListNode>(false);
            entity.ValidateDataUnmanaged<ME.ECS.Collections.IntrusiveHashSetBucket>(false);
            entity.ValidateDataUnmanaged<ME.ECS.Collections.IntrusiveHashSetData>(false);

            ViewComponentsInitializer.Init(in entity);
            ME.ECS.DataConfigs.DataConfig.Init(in entity);
            TransformComponentsInitializer.Init(in entity);
            NameComponentsInitializer.Init(in entity);
            CameraComponentsInitializer.Init(in entity);
            ME.ECS.DataConfigs.DataConfigComponentsInitializer.Init(in entity);

        }

    }

    public static class ComponentsInitializerWorld {

        private static System.Action<Entity> onEntity;

        public static void Setup(System.Action<Entity> onEntity) {

            ComponentsInitializerWorld.onEntity = onEntity;

        }

        public static void Register(System.Action<Entity> onEntity) {

            ComponentsInitializerWorld.onEntity += onEntity;

        }

        public static void UnRegister(System.Action<Entity> onEntity) {

            ComponentsInitializerWorld.onEntity -= onEntity;

        }

        public static void Init(in Entity entity) {
            
            CoreComponentsInitializer.InitTypeId();
            CoreComponentsInitializer.Init(in entity);
            
            if (ComponentsInitializerWorld.onEntity != null) ComponentsInitializerWorld.onEntity.Invoke(entity);
            
        }

    }

}
