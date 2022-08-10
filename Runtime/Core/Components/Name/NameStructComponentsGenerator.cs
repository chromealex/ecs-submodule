
namespace ME.ECS {

    public static class NameComponentConstants {

        public const GroupColor GROUP_COLOR = ME.ECS.GroupColor.Default;

        [ComponentGroup("Entity", NameComponentConstants.GROUP_COLOR, -2000)]
        public static class GroupInfo { }

    }
    
    public static class NameComponentsInitializer {

        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<ME.ECS.Name.Name>(isVersioned: false, isSimple: true, isBlittable: true);
            
        }
        
        public static void Init(State state) {
    
            state.structComponents.ValidateUnmanaged<ME.ECS.Name.Name>(ref state.allocator, false);

        }
    
        public static void Init(in Entity entity) {

            entity.ValidateDataUnmanaged<ME.ECS.Name.Name>(false);
            
        }

    }

}
