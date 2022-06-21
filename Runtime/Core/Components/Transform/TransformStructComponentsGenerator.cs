
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

        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            structComponentsContainer.ValidateBlittable<ME.ECS.Transform.Nodes>();
            structComponentsContainer.ValidateBlittable<ME.ECS.Transform.Container>();

            Transform2DComponentsInitializer.Init(ref structComponentsContainer);
            Transform3DComponentsInitializer.Init(ref structComponentsContainer);

        }
    
        public static void Init(in Entity entity) {

            entity.ValidateDataBlittable<ME.ECS.Transform.Nodes>();
            entity.ValidateDataBlittable<ME.ECS.Transform.Container>();
            
            Transform2DComponentsInitializer.Init(in entity);
            Transform3DComponentsInitializer.Init(in entity);

        }

    }

}
