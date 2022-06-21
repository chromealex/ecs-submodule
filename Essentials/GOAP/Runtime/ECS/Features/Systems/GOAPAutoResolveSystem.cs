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
    public sealed class GOAPAutoResolveSystem : ISystemFilter {
        
        private GOAPFeature feature;
        
        public World world { get; set; }
        
        void ISystemBase.OnConstruct() {
            
            this.GetFeature(out this.feature);
            
        }
        
        void ISystemBase.OnDeconstruct() {}
        
        #if !CSHARP_8_OR_NEWER
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
        #endif
        Filter ISystemFilter.filter { get; set; }
        Filter ISystemFilter.CreateFilter() {
            
            return Filter.Create("Filter-GOAPAutoResolveSystem")
                         .With<ME.ECS.Essentials.GOAP.GOAPEntityPlan>()
                         .Push();
            
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime) {
            
            ref var prevPlan = ref entity.Get<ME.ECS.Essentials.GOAP.GOAPEntityPrevPlan>();
            var plan = entity.Read<ME.ECS.Essentials.GOAP.GOAPEntityPlan>();
            var action = plan.nextAction;
            if (action != null) {

                // if (prevPlan.groupId != plan.groupId ||
                //     prevPlan.nextActionIdx != plan.nextActionIdx) {
                //     
                //     prevPlan.groupId = plan.groupId;
                //     prevPlan.nextActionIdx = plan.nextActionIdx;
                //
                //     action.PerformBegin(in entity);
                //
                // }
                
                action.Perform(in entity);
                if (action.IsDone(in entity) == true) {
                    
                    action.OnComplete(in entity);
                    entity.Remove<GOAPEntityPlan>();

                }
                
            }
            
        }
    
    }
    
}