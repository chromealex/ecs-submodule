using ME.ECS;

namespace ME.ECS.Pathfinding.Features {

    using PathfindingAstar.Components; using PathfindingAstar.Modules; using PathfindingAstar.Systems; using PathfindingAstar.Markers;
    
    namespace PathfindingAstar.Components {}
    namespace PathfindingAstar.Modules {}
    namespace PathfindingAstar.Systems {}
    namespace PathfindingAstar.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class PathfindingAstarFeature : Feature {

        protected override void OnConstruct() {
            
            this.AddSystem<ME.ECS.Pathfinding.Features.PathfindingAstar.Systems.BuildSystem>();
            
        }

        protected override void OnDeconstruct() {
            
        }

    }

}