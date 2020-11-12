
namespace ME.ECS.Pathfinding.Features.Pathfinding.Systems {

    #pragma warning disable
    using Components; using Modules; using Systems; using Markers;
    #pragma warning restore
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class BuildPathSystem : ISystemFilter {

        private PathfindingFeature pathfindingFeature;
        
        public World world { get; set; }

        void ISystemBase.OnConstruct() {

            this.pathfindingFeature = this.world.GetFeature<PathfindingFeature>();

        }
        
        void ISystemBase.OnDeconstruct() {}
        
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
        Filter ISystemFilter.filter { get; set; }
        Filter ISystemFilter.CreateFilter() {
            
            return Filter.Create("Filter-BuildPathSystem")
                         .WithStructComponent<CalculatePath>()
                         .Push();
            
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime) {
            
            var active = this.pathfindingFeature.GetEntity().GetComponent<PathfindingInstance>().pathfinding;
            if (active == null) {

                return;

            }
            
            entity.RemoveComponents<Path>();

            var request = entity.GetData<CalculatePath>();
            //UnityEngine.Debug.LogWarning("REQUEST PATH: " + request.@from.ToStringDec() + " to " + request.to.ToStringDec());
            var constraint = request.constraint;
            var path = active.CalculatePath(request.from, request.to, constraint, new ME.ECS.Pathfinding.PathCornersModifier());
            if (path.result == ME.ECS.Pathfinding.PathCompleteState.Complete) {

                this.pathfindingFeature.SetPath(in entity, path, constraint, request.to);
                
            }

            path.Recycle();

            entity.RemoveData<CalculatePath>();

        }
    
    }
    
}