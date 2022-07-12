
namespace ME.ECS {

    public static class Transform3DComponentsInitializer {
    
        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Position>(isVersioned: true, isSimple: true, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Rotation>(isVersioned: true, isSimple: true, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Scale>(isVersioned: true, isSimple: true, isBlittable: true);
            
        }

        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            structComponentsContainer.ValidateUnmanaged<ME.ECS.Transform.Position>();
            structComponentsContainer.ValidateUnmanaged<ME.ECS.Transform.Rotation>();
            structComponentsContainer.ValidateUnmanaged<ME.ECS.Transform.Scale>();

        }
        
        public static void Init(in Entity entity) {

            entity.ValidateDataUnmanaged<ME.ECS.Transform.Position>();
            entity.ValidateDataUnmanaged<ME.ECS.Transform.Rotation>();
            entity.ValidateDataUnmanaged<ME.ECS.Transform.Scale>();

        }
    
    }

}
