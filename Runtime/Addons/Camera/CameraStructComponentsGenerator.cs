
namespace ME.ECS {

    public static class CameraComponentsInitializer {
    
        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<ME.ECS.Camera.Camera>(isVersioned: true, isSimple: true, isBlittable: true);
            
        }

        public static void Init(State state) {
    
            state.structComponents.ValidateUnmanaged<ME.ECS.Camera.Camera>(ref state.allocator);

        }
    
        public static void Init(in Entity entity) {

            entity.ValidateDataUnmanaged<ME.ECS.Camera.Camera>();
            
        }

    }

}
