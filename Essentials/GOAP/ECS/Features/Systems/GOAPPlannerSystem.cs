using ME.ECS;

namespace ME.ECS.Essentials.GOAP.Systems {

    #pragma warning disable
    using Components; using Modules; using Systems; using Markers;
    #pragma warning restore
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class GOAPPlannerSystem : ISystem, IAdvanceTick {
        
        private GOAPFeature feature;

        public Filter filter;
    
        public World world { get; set; }
        public void OnConstruct() {

            this.GetFeature(out this.feature);
            
            Filter.Create()
                  .With<GOAPEntityGroup>()
                  .Without<GOAPEntityPlan>()
                  .With<GOAPEntityGoal>()
                  .Push(ref this.filter);

        }

        public void OnDeconstruct() {
        
        }

        public void AdvanceTick(in float deltaTime) {

            var module = this.world.GetModule<GOAPModule>();
            foreach (var entity in this.filter) {

                var goal = entity.Read<GOAPEntityGoal>().goal;
                if (goal.Has(in entity) == true) {

                    // All events have done
                    entity.Remove<GOAPEntityGoal>();
                    continue;

                }
                
                var actions = module.GetGroupActions(in entity, entity.Read<GOAPEntityGroup>().groupId, Unity.Collections.Allocator.TempJob);
                
                var plan = Planner.GetPlan(this.world, actions, goal, entity);
                if (plan.planStatus == PathStatus.Success) {

                    var idx = 0;
                    var prevPlan = entity.Read<GOAPEntityPrevPlan>();
                    if (plan.actions[0].data.groupId == prevPlan.groupId &&
                        plan.actions[0].data.id == prevPlan.nextActionIdx) {

                        ++idx;

                    }
                    
                    entity.Set(new GOAPEntityPlan() {
                        groupId = plan.actions[idx].data.groupId,
                        nextActionIdx = plan.actions[idx].data.id,
                    });

                }
                plan.Dispose();

                for (int i = 0; i < actions.Length; ++i) {
                    actions[i].Dispose();
                }
                actions.Dispose();

            }
        
        }

    }
    
}