namespace ME.ECS.DataConfigs {

    public static class DataConfigComponentsInitializer {

        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<SourceConfig>();
            #if !STATIC_API_DISABLED
            WorldUtilities.InitComponentTypeId<SourceConfigs>(isCopyable: true);
            #endif
            
        }

        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            structComponentsContainer.Validate<SourceConfig>();
            #if !STATIC_API_DISABLED
            structComponentsContainer.ValidateCopyable<SourceConfigs>();
            #endif

        }
    
        public static void Init(in Entity entity) {

            entity.ValidateData<SourceConfig>();
            #if !STATIC_API_DISABLED
            entity.ValidateDataCopyable<SourceConfigs>();
            #endif
            
        }

    }

}