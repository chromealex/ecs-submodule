
namespace ME.ECS {

    public static class NameComponentConstants {

        public const GroupColor GROUP_COLOR = ME.ECS.GroupColor.Default;

        [ComponentGroup("Entity", NameComponentConstants.GROUP_COLOR, -2000)]
        public static class GroupInfo { }

    }
    
    public static class NameComponentsInitializer {

        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<ME.ECS.Name.Name>(isVersioned: false, isSimple: true);
            
        }
        
        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            structComponentsContainer.Validate<ME.ECS.Name.Name>();

        }
    
        public static void Init(in Entity entity) {

            entity.ValidateData<ME.ECS.Name.Name>();
            
        }

    }

}
