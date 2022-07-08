namespace ME.ECS.DataConfigs {

    public static class DataConfigComponentsInitializer {

        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<SourceConfig>();
            WorldUtilities.InitComponentTypeId<SourceConfigs>(isCopyable: true);
            
        }

        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            structComponentsContainer.Validate<SourceConfig>();
            structComponentsContainer.ValidateCopyable<SourceConfigs>();

        }
    
        public static void Init(in Entity entity) {

            entity.ValidateData<SourceConfig>();
            entity.ValidateDataCopyable<SourceConfigs>();
            
        }

    }

}