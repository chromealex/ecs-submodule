namespace ME.ECS.DataConfigs {

    public static class DataConfigComponentsInitializer {

        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<SourceConfig>();
            
        }

        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            structComponentsContainer.Validate<SourceConfig>();

        }
    
        public static void Init(in Entity entity) {

            entity.ValidateData<SourceConfig>();
            
        }

    }

}