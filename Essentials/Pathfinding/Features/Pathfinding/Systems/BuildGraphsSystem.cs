
namespace ME.ECS.Pathfinding.Features.Pathfinding.Systems {

    #pragma warning disable
    using Components; using Modules; using Systems; using Markers;
    #pragma warning restore
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class BuildGraphsSystem : ISystemFilter {

        private PathfindingFeature pathfindingFeature;
        private bool isBuilt;

        public World world { get; set; }

        void ISystemBase.OnConstruct() {

            this.isBuilt = false;
            this.pathfindingFeature = this.world.GetFeature<PathfindingFeature>();

        }
        
        void ISystemBase.OnDeconstruct() {}
        
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
        Filter ISystemFilter.filter { get; set; }
        Filter ISystemFilter.CreateFilter() {
            
            return Filter.Create("Filter-BuildGraphsSystem")
                         .With<IsPathfinding>()
                         .With<HasPathfindingInstance>()
                         .With<BuildAllGraphs>()
                         .Push();
            
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime) {

            var pathfinding = entity.Read<PathfindingInstance>().pathfinding;
            if (this.isBuilt == false) {

                pathfinding.BuildAll();
                
                var instance = this.pathfindingFeature.GetInstance();
                this.isBuilt = (instance.clonePathfinding == false);

            }

            UnityEngine.Debug.Log($"Graph built tick: {this.world.GetCurrentTick()}");
            entity.Set(new IsAllGraphsBuilt(), ComponentLifetime.NotifyAllSystems);
            entity.Remove<BuildAllGraphs>();

        }
    
    }
    
}