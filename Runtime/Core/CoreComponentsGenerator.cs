
namespace ME.ECS {

    public struct IsEntityOneShot : IComponentOneShot {}
    public struct IsEntityEmptyOneShot : IComponentOneShot {}
    
    public static class CoreComponentsInitializer {

        public delegate void InitTypeIdStaticCallback();
        private static InitTypeIdStaticCallback initTypeIdStaticCallback;

        public delegate void InitStaticCallback(State state, ref World.NoState noState);
        private static InitStaticCallback initStaticCallback;
        
        public delegate void InitEntityStaticCallback(in Entity entity);
        private static InitEntityStaticCallback initEntityStaticCallback;

        public static void RegisterInitCallback(InitTypeIdStaticCallback initTypeId, InitStaticCallback init, InitEntityStaticCallback initEntity) {
            
            CoreComponentsInitializer.initTypeIdStaticCallback += initTypeId;
            CoreComponentsInitializer.initStaticCallback += init;
            CoreComponentsInitializer.initEntityStaticCallback += initEntity;
            
        }

        public static void UnRegisterInitCallback(InitTypeIdStaticCallback initTypeId, InitStaticCallback init, InitEntityStaticCallback initEntity) {
            
            CoreComponentsInitializer.initTypeIdStaticCallback -= initTypeId;
            CoreComponentsInitializer.initStaticCallback -= init;
            CoreComponentsInitializer.initEntityStaticCallback -= initEntity;
            
        }

        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<IsEntityOneShot>(true, isOneShot: true);
            WorldUtilities.InitComponentTypeId<IsEntityEmptyOneShot>(true, isOneShot: true);
            
            ViewComponentsInitializer.InitTypeId();
            NameComponentsInitializer.InitTypeId();
            
            initTypeIdStaticCallback?.Invoke();

        }
        
        public static void Init(State state, ref World.NoState noState) {
            
            noState.storage.ValidateOneShot<IsEntityOneShot>(true);
            noState.storage.ValidateOneShot<IsEntityEmptyOneShot>(true);

            ViewComponentsInitializer.Init(state);
            NameComponentsInitializer.Init(state);

            initStaticCallback?.Invoke(state, ref noState);
            
        }

        public static void Init(in Entity entity) {
            
            entity.ValidateDataOneShot<IsEntityOneShot>(true);
            entity.ValidateDataOneShot<IsEntityEmptyOneShot>(true);

            ViewComponentsInitializer.Init(in entity);
            NameComponentsInitializer.Init(in entity);
            
            initEntityStaticCallback?.Invoke(in entity);

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
