
namespace ME.ECS {

    public static class Transform2DComponentsInitializer {

        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Position2D>(isVersioned: true, isSimple: true, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Rotation2D>(isVersioned: true, isSimple: true, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Scale2D>(isVersioned: true, isSimple: true, isBlittable: true);
            
        }
        
        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            structComponentsContainer.ValidateUnmanaged<ME.ECS.Transform.Position2D>();
            structComponentsContainer.ValidateUnmanaged<ME.ECS.Transform.Rotation2D>();
            structComponentsContainer.ValidateUnmanaged<ME.ECS.Transform.Scale2D>();

        }
    
        public static void Init(in Entity entity) {

            entity.ValidateDataUnmanaged<ME.ECS.Transform.Position2D>();
            entity.ValidateDataUnmanaged<ME.ECS.Transform.Rotation2D>();
            entity.ValidateDataUnmanaged<ME.ECS.Transform.Scale2D>();

        }

    }

}
