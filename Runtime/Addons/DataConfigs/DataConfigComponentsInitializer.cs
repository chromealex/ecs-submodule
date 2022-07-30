namespace ME.ECS.DataConfigs {

    public static class DataConfigConstants {

        public const string FILE_NAME = "ME.ECS.DataConfigIndexer";

    }

    public static class DataConfigComponentsInitializer {

        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<SourceConfig>();
            #if !STATIC_API_DISABLED
            WorldUtilities.InitComponentTypeId<SourceConfigs>();
            #endif
            
        }

        public static DataConfigIndexerFeature GetFeature() {

            try {

                return UnityEngine.Resources.Load<DataConfigIndexerFeature>(DataConfigConstants.FILE_NAME);

            } catch (System.Exception) {

                return null;

            }

        }

        public static void Init(State state) {
    
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

            entity.ValidateData<SourceConfig>();
            #if !STATIC_API_DISABLED
            entity.ValidateData<SourceConfigs>();
            #endif
            
        }

    }

}