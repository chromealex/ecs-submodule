
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

        public World world { get; set; }

        void ISystemBase.OnConstruct() {}
        
        void ISystemBase.OnDeconstruct() {}
        
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
        Filter ISystemFilter.filter { get; set; }
        Filter ISystemFilter.CreateFilter() {
            
            return Filter.Create("Filter-BuildGraphsSystem")
                         .WithStructComponent<IsPathfinding>()
                         .WithStructComponent<HasPathfindingInstance>()
                         .WithStructComponent<BuildAllGraphs>()
                         .Push();
            
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime) {
            
            entity.GetComponent<PathfindingInstance>().pathfinding.BuildAll();

            UnityEngine.Debug.Log("Graph built");
            entity.SetData(new IsAllGraphsBuilt(), ComponentLifetime.NotifyAllSystems);
            entity.RemoveData<BuildAllGraphs>();

        }
    
    }
    
}