namespace ME.ECS.Essentials.GOAP {

    using Modules;
    
    public static class WorldExtensions {

        public static GOAPGroupId RegisterGOAPGroup(this World world, GOAPGroup group) {

            var module = world.GetModule<GOAPModule>();
            return module.RegisterGroup(group);

        }

    }
    
    public static class EntityExtensions {

        public static void SetGOAPGroup(this in Entity entity, GOAPGroup group) {

            var module = Worlds.current.GetModule<GOAPModule>();
            entity.SetGOAPGroup(module.RegisterGroup(group));

        }

        public static void SetGOAPGroup(this in Entity entity, GOAPGroupId groupId) {

            //var module = Worlds.current.GetModule<GOAPModule>();
            entity.Set(new GOAPEntityGroup() {
                groupId = groupId,
            });
            /*var group = module.GetGroupById(groupId);
            if (group.goals != null) {
                entity.Set(new GOAPEntityGoal() {
                    goal = Goal.Create(group.goal.data),
                });
            }*/

        }

    }

}