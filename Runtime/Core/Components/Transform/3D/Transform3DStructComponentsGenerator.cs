
namespace ME.ECS {

    public static class Transform3DComponentsInitializer {
    
        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Position>(isVersioned: true, isSimple: true, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Rotation>(isVersioned: true, isSimple: true, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Scale>(isVersioned: true, isSimple: true, isBlittable: true);
            
        }

        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            structComponentsContainer.ValidateBlittable<ME.ECS.Transform.Position>();
            structComponentsContainer.ValidateBlittable<ME.ECS.Transform.Rotation>();
            structComponentsContainer.ValidateBlittable<ME.ECS.Transform.Scale>();

        }
        
        public static void Init(in Entity entity) {

            entity.ValidateDataBlittable<ME.ECS.Transform.Position>();
            entity.ValidateDataBlittable<ME.ECS.Transform.Rotation>();
            entity.ValidateDataBlittable<ME.ECS.Transform.Scale>();

        }
    
    }

}
