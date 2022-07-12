
namespace ME.ECS {

    public static class CameraComponentsInitializer {
    
        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<ME.ECS.Camera.Camera>(isVersioned: true, isSimple: true, isBlittable: true);
            
        }

        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            structComponentsContainer.ValidateUnmanaged<ME.ECS.Camera.Camera>();

        }
    
        public static void Init(in Entity entity) {

            entity.ValidateDataUnmanaged<ME.ECS.Camera.Camera>();
            
        }

    }

}
