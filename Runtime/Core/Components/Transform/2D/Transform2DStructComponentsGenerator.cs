
namespace ME.ECS {

    public static class Transform2DComponentsInitializer {

        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Position2D>(isVersioned: true, isSimple: true, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Rotation2D>(isVersioned: true, isSimple: true, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Scale2D>(isVersioned: true, isSimple: true, isBlittable: true);
            
        }
        
        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            structComponentsContainer.ValidateBlittable<ME.ECS.Transform.Position2D>();
            structComponentsContainer.ValidateBlittable<ME.ECS.Transform.Rotation2D>();
            structComponentsContainer.ValidateBlittable<ME.ECS.Transform.Scale2D>();

        }
    
        public static void Init(in Entity entity) {

            entity.ValidateDataBlittable<ME.ECS.Transform.Position2D>();
            entity.ValidateDataBlittable<ME.ECS.Transform.Rotation2D>();
            entity.ValidateDataBlittable<ME.ECS.Transform.Scale2D>();

        }

    }

}
