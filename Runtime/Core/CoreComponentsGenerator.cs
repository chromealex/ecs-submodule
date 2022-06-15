
namespace ME.ECS {

    public struct IsEntityOneShot : IComponentOneShot {}
    public struct IsEntityEmptyOneShot : IComponentOneShot {}
    
    public static class CoreComponentsInitializer {

        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<IsEntityOneShot>(true, isOneShot: true);
            WorldUtilities.InitComponentTypeId<IsEntityEmptyOneShot>(true, isOneShot: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Views.ViewComponent>(false, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveData>(false, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveSortedListData>(false, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveListNode>(false, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveHashSetBucket>(false, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveHashSetData>(false, isBlittable: true);
            
            ME.ECS.DataConfigs.DataConfig.InitTypeId();
            TransformComponentsInitializer.InitTypeId();
            NameComponentsInitializer.InitTypeId();
            CameraComponentsInitializer.InitTypeId();
            ME.ECS.DataConfigs.DataConfigComponentsInitializer.InitTypeId();

        }
        
        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer, ref ME.ECS.StructComponentsContainer noStateStructComponentsContainer) {
            
            noStateStructComponentsContainer.Validate<IsEntityOneShot>(true);
            noStateStructComponentsContainer.Validate<IsEntityEmptyOneShot>(true);
            structComponentsContainer.ValidateBlittable<ME.ECS.Views.ViewComponent>(false);
            structComponentsContainer.ValidateBlittable<ME.ECS.Collections.IntrusiveData>(false);
            structComponentsContainer.ValidateBlittable<ME.ECS.Collections.IntrusiveSortedListData>(false);
            structComponentsContainer.ValidateBlittable<ME.ECS.Collections.IntrusiveListNode>(false);
            structComponentsContainer.ValidateBlittable<ME.ECS.Collections.IntrusiveHashSetBucket>(false);
            structComponentsContainer.ValidateBlittable<ME.ECS.Collections.IntrusiveHashSetData>(false);

            ME.ECS.DataConfigs.DataConfig.Init(ref structComponentsContainer);
            TransformComponentsInitializer.Init(ref structComponentsContainer);
            NameComponentsInitializer.Init(ref structComponentsContainer);
            CameraComponentsInitializer.Init(ref structComponentsContainer);
            ME.ECS.DataConfigs.DataConfigComponentsInitializer.Init(ref structComponentsContainer);
            
        }

        public static void Init(in Entity entity) {
            
            entity.ValidateDataOneShot<IsEntityOneShot>(true);
            entity.ValidateDataOneShot<IsEntityEmptyOneShot>(true);
            entity.ValidateDataBlittable<ME.ECS.Views.ViewComponent>(false);
            entity.ValidateDataBlittable<ME.ECS.Collections.IntrusiveListNode>(false);
            entity.ValidateDataBlittable<ME.ECS.Collections.IntrusiveHashSetBucket>(false);

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
