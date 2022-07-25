
namespace ME.ECS {

    public static class Transform2DComponentsInitializer {

        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Position2D>(isVersioned: true, isSimple: true, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Rotation2D>(isVersioned: true, isSimple: true, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Scale2D>(isVersioned: true, isSimple: true, isBlittable: true);
            
        }
        
        public static void Init(State state) {
    
            state.structComponents.ValidateUnmanaged<ME.ECS.Transform.Position2D>(ref state.allocator);
            state.structComponents.ValidateUnmanaged<ME.ECS.Transform.Rotation2D>(ref state.allocator);
            state.structComponents.ValidateUnmanaged<ME.ECS.Transform.Scale2D>(ref state.allocator);

        }
    
        public static void Init(in Entity entity) {

            entity.ValidateDataUnmanaged<ME.ECS.Transform.Position2D>();
            entity.ValidateDataUnmanaged<ME.ECS.Transform.Rotation2D>();
            entity.ValidateDataUnmanaged<ME.ECS.Transform.Scale2D>();

        }

    }

}
