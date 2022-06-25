#if FIXED_POINT_MATH
using math = ME.ECS.Mathematics.math;
using float3 = ME.ECS.Mathematics.float3;
using tfloat = sfloat;
#else
using math = Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;
using tfloat = System.Single;
#endif
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
                  .Push(ref this.filter);

        }

        public void OnDeconstruct() {
        
        }

        private struct GoalTemp {

            public bool isCreated;
            public GOAPEntityPlan plan;
            public tfloat h;

        }
        
        public void AdvanceTick(in float deltaTime) {

            var module = this.world.GetModule<GOAPModule>();
            var arr = this.filter.ToArray(Unity.Collections.Allocator.Temp);
            for (int k = 0; k < arr.Length; ++k) {

                var entity = arr[k];
                var group = module.GetGroupById(entity.Read<GOAPEntityGroup>().groupId);
                if (group.goals == null ||
                    group.goals.Length == 0) {

                    continue;

                }

                GoalTemp resultGoal = new GoalTemp() {
                    isCreated = false,
                    h = float.MaxValue,
                };
                foreach (var goal in group.goals) {

                    var w = goal.GetWeight(in entity);
                    if (w <= 0f) continue;

                    var goalData = Goal.Create(goal.data);
                    if (goalData.Has(in entity) == true) {

                        // All events have done
                        // Should we remove GOAP group after that?
                        //entity.Remove<GOAPEntityGroup>();
                        continue;

                    }

                    var actions = module.GetGroupActions(in entity, entity.Read<GOAPEntityGroup>().groupId, Unity.Collections.Allocator.TempJob);
                
                    var plan = Planner.GetPlan(this.world, actions, goalData, entity);
                    if (plan.planStatus == PathStatus.Success) {

                        var idx = 0;
                        var prevPlan = entity.Read<GOAPEntityPrevPlan>();
                        if (plan.actions[0].data.groupId == prevPlan.groupId &&
                            plan.actions[0].data.id == prevPlan.nextActionIdx) {

                            ++idx;

                        }

                        var h = plan.cost * (1f / w);
                        if (h < resultGoal.h && idx < plan.actions.Length) {
                            resultGoal = new GoalTemp() {
                                isCreated = true,
                                plan = new GOAPEntityPlan() {
                                    groupId = plan.actions[idx].data.groupId,
                                    nextActionIdx = plan.actions[idx].data.id,
                                },
                                h = h,
                            };
                        }

                    }
                    plan.Dispose();

                    for (int i = 0; i < actions.Length; ++i) {
                        actions[i].action.Dispose();
                    }
                    actions.Dispose();

                }

                // Choose the right plan to go if we have one
                if (resultGoal.isCreated == true) {

                    ref var prevPlan = ref entity.Get<ME.ECS.Essentials.GOAP.GOAPEntityPrevPlan>();
                    var plan = resultGoal.plan;
                    if (prevPlan.groupId != plan.groupId ||
                        prevPlan.nextActionIdx != plan.nextActionIdx) {
                    
                        prevPlan.groupId = plan.groupId;
                        prevPlan.nextActionIdx = plan.nextActionIdx;

                        plan.nextAction.PerformBegin(in entity);

                    }

                    if(this.feature.showDebug)
                        UnityEngine.Debug.Log("Set plan: " + entity);
                    entity.Set(plan);

                }
                
            }

            arr.Dispose();

        }

    }
    
}