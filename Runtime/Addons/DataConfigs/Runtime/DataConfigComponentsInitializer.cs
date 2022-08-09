namespace ME.ECS.DataConfigs {

    public static class DataConfigConstants {

        public const string FILE_NAME = "ME.ECS.DataConfigIndexer";

    }

    public static class DataConfigComponentsInitializer {

        static DataConfigComponentsInitializer() {
            
            CoreComponentsInitializer.RegisterInitCallback(DataConfigComponentsInitializer.InitTypeId, DataConfigComponentsInitializer.Init, DataConfigComponentsInitializer.Init);
            
        }
        
        public static DataConfigIndexerFeature GetFeature() {

            try {

                return UnityEngine.Resources.Load<DataConfigIndexerFeature>(DataConfigConstants.FILE_NAME);

            } catch (System.Exception) {

                return null;

            }

        }

        public static void InitTypeId() {
            
            ME.ECS.DataConfigs.DataConfig.InitTypeId();
            
            WorldUtilities.InitComponentTypeId<SourceConfig>();
            #if !STATIC_API_DISABLED
            WorldUtilities.InitComponentTypeId<SourceConfigs>();
            #endif
            
        }

        public static void Init(State state, ref World.NoState noState) {
    
            ME.ECS.DataConfigs.DataConfig.Init(state);
            
            state.structComponents.Validate<SourceConfig>();
            #if !STATIC_API_DISABLED
            state.structComponents.Validate<SourceConfigs>();
            var feature = DataConfigComponentsInitializer.GetFeature();
            if (feature == null) {

                E.FILE_NOT_FOUND($"Feature `DataConfigIndexerFeature` not found. Create it at path `Resources/{DataConfigConstants.FILE_NAME}`. You can turn off this feature by enabling STATIC_API_DISABLED define.");

            }
            Worlds.current.AddFeature(feature, true);
            #endif

        }
    
        public static void Init(in Entity entity) {

            ME.ECS.DataConfigs.DataConfig.Init(in entity);
            
            entity.ValidateData<SourceConfig>();
            #if !STATIC_API_DISABLED
            entity.ValidateData<SourceConfigs>();
            #endif
            
        }

    }

}