
namespace ME.ECS {

    public static class PathfindingComponentsInitializer {
    
        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.PathfindingInstance>(false, true, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.CalculatePath>(false, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.PathfindingAstar.Components.Path>(false, true, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.PathfindingFlowField.Components.PathFlowField>(false, true, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathfinding>(true, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.BuildAllGraphs>(true, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsAllGraphsBuilt>(true, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.HasPathfindingInstance>(true, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathBuilt>(true, false, false, false);
            
            structComponentsContainer.ValidateCopyable<ME.ECS.Pathfinding.Features.Pathfinding.Components.PathfindingInstance>(false);
            structComponentsContainer.Validate<ME.ECS.Pathfinding.Features.Pathfinding.Components.CalculatePath>();
            structComponentsContainer.ValidateCopyable<ME.ECS.Pathfinding.Features.PathfindingAstar.Components.Path>();
            structComponentsContainer.ValidateCopyable<ME.ECS.Pathfinding.Features.PathfindingFlowField.Components.PathFlowField>();
            structComponentsContainer.Validate<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathfinding>(true);
            structComponentsContainer.Validate<ME.ECS.Pathfinding.Features.Pathfinding.Components.BuildAllGraphs>(true);
            structComponentsContainer.Validate<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsAllGraphsBuilt>(true);
            structComponentsContainer.Validate<ME.ECS.Pathfinding.Features.Pathfinding.Components.HasPathfindingInstance>(true);
            structComponentsContainer.Validate<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathBuilt>(true);

        }

        public static void InitEntity(Entity entity) {

            entity.ValidateDataCopyable<ME.ECS.Pathfinding.Features.Pathfinding.Components.PathfindingInstance>();
            entity.ValidateData<ME.ECS.Pathfinding.Features.Pathfinding.Components.CalculatePath>();
            entity.ValidateDataCopyable<ME.ECS.Pathfinding.Features.PathfindingAstar.Components.Path>();
            entity.ValidateDataCopyable<ME.ECS.Pathfinding.Features.PathfindingFlowField.Components.PathFlowField>();
            entity.ValidateData<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathfinding>(true);
            entity.ValidateData<ME.ECS.Pathfinding.Features.Pathfinding.Components.BuildAllGraphs>(true);
            entity.ValidateData<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsAllGraphsBuilt>(true);
            entity.ValidateData<ME.ECS.Pathfinding.Features.Pathfinding.Components.HasPathfindingInstance>(true);
            entity.ValidateData<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathBuilt>();
            
        }

    }

}
