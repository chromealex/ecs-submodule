using ME.ECS;

namespace ME.ECS.Pathfinding.Features {

    using PathfindingFlowField.Components; using PathfindingFlowField.Modules; using PathfindingFlowField.Systems; using PathfindingFlowField.Markers;
    
    namespace PathfindingFlowField.Components {}
    namespace PathfindingFlowField.Modules {}
    namespace PathfindingFlowField.Systems {}
    namespace PathfindingFlowField.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class PathfindingFlowFieldFeature : Feature {

        protected override void OnConstruct() {
            
            this.AddSystem<ME.ECS.Pathfinding.Features.PathfindingFlowField.Systems.BuildSystem>();
            
        }

        protected override void OnDeconstruct() {
            
        }

    }

}