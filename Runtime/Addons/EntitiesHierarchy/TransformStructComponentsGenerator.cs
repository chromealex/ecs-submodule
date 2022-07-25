
namespace ME.ECS {

    public static class TransformComponentConstants {

        public const GroupColor GROUP_COLOR = ME.ECS.GroupColor.Cyan;

        [ComponentGroup("Transform", TransformComponentConstants.GROUP_COLOR, -1000)]
        public static class GroupInfo { }

    }
    
    public static class TransformComponentsInitializer {

        public static void InitTypeId() {

            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Nodes>(isVersioned: true, isSimple: true, isBlittable: true);
            WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Container>(isVersioned: true, isSimple: true, isBlittable: true);
            
            Transform2DComponentsInitializer.InitTypeId();
            Transform3DComponentsInitializer.InitTypeId();

        }

        public static void Init(State state) {
    
            state.structComponents.ValidateUnmanaged<ME.ECS.Transform.Nodes>(ref state.allocator);
            state.structComponents.ValidateUnmanaged<ME.ECS.Transform.Container>(ref state.allocator);

            Transform2DComponentsInitializer.Init(state);
            Transform3DComponentsInitializer.Init(state);

        }
    
        public static void Init(in Entity entity) {

            entity.ValidateDataUnmanaged<ME.ECS.Transform.Nodes>();
            entity.ValidateDataUnmanaged<ME.ECS.Transform.Container>();
            
            Transform2DComponentsInitializer.Init(in entity);
            Transform3DComponentsInitializer.Init(in entity);

        }

    }

}
