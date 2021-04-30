
namespace ME.ECS {

    public static class CoreComponentsInitializer {

        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<ME.ECS.Views.ViewComponent>(false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveListNode>(false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveHashSetBucket>(false);
            
            ME.ECS.DataConfigs.DataConfig.InitTypeId();
            TransformComponentsInitializer.InitTypeId();
            NameComponentsInitializer.InitTypeId();
            CameraComponentsInitializer.InitTypeId();

        }
        
        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
            
            structComponentsContainer.Validate<ME.ECS.Views.ViewComponent>(false);
            structComponentsContainer.Validate<ME.ECS.Collections.IntrusiveListNode>(false);
            structComponentsContainer.Validate<ME.ECS.Collections.IntrusiveHashSetBucket>(false);

            ME.ECS.DataConfigs.DataConfig.Init(ref structComponentsContainer);
            TransformComponentsInitializer.Init(ref structComponentsContainer);
            NameComponentsInitializer.Init(ref structComponentsContainer);
            CameraComponentsInitializer.Init(ref structComponentsContainer);
            
        }

        public static void Init(in Entity entity) {
            
            entity.ValidateData<ME.ECS.Views.ViewComponent>(false);
            entity.ValidateData<ME.ECS.Collections.IntrusiveListNode>(false);
            entity.ValidateData<ME.ECS.Collections.IntrusiveHashSetBucket>(false);

            ME.ECS.DataConfigs.DataConfig.Init(in entity);
            TransformComponentsInitializer.Init(in entity);
            NameComponentsInitializer.Init(in entity);
            CameraComponentsInitializer.Init(in entity);

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
            
            CoreComponentsInitializer.Init(in entity);
            
            if (ComponentsInitializerWorld.onEntity != null) ComponentsInitializerWorld.onEntity.Invoke(entity);
            
        }

    }

}
