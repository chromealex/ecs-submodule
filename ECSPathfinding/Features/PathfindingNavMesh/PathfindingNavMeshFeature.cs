using ME.ECS;

namespace ME.ECS.Pathfinding.Features {

    using PathfindingNavMesh.Components; using PathfindingNavMesh.Modules; using PathfindingNavMesh.Systems; using PathfindingNavMesh.Markers;
    
    namespace PathfindingNavMesh.Components {}
    namespace PathfindingNavMesh.Modules {}
    namespace PathfindingNavMesh.Systems {}
    namespace PathfindingNavMesh.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class PathfindingNavMeshFeature : Feature {

        protected override void OnConstruct() {
            
            this.AddSystem<ME.ECS.Pathfinding.Features.PathfindingNavMesh.Systems.BuildSystem>();
            
        }

        protected override void OnDeconstruct() {
            
        }

    }

}