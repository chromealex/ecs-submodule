
namespace ME.ECS {

    public static class PathfindingComponentsInitializer {
    
        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.PathfindingInstance>(false, false, false, true, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.CalculatePath>(false, false, true, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.PathfindingAstar.Components.Path>(false, false, false, true, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.PathfindingFlowField.Components.PathFlowField>(false, false, false, true, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathfinding>(true, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.BuildAllGraphs>(true, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsAllGraphsBuilt>(true, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.HasPathfindingInstance>(true, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathBuilt>(true, false, false, false, false, false);
            
            structComponentsContainer.ValidateCopyable<ME.ECS.Pathfinding.Features.Pathfinding.Components.PathfindingInstance>(false);
            structComponentsContainer.ValidateBlittable<ME.ECS.Pathfinding.Features.Pathfinding.Components.CalculatePath>(false);
            structComponentsContainer.ValidateCopyable<ME.ECS.Pathfinding.Features.PathfindingAstar.Components.Path>(false);
            structComponentsContainer.ValidateCopyable<ME.ECS.Pathfinding.Features.PathfindingFlowField.Components.PathFlowField>(false);
            structComponentsContainer.ValidateTag<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathfinding>(true);
            structComponentsContainer.ValidateTag<ME.ECS.Pathfinding.Features.Pathfinding.Components.BuildAllGraphs>(true);
            structComponentsContainer.ValidateTag<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsAllGraphsBuilt>(true);
            structComponentsContainer.ValidateTag<ME.ECS.Pathfinding.Features.Pathfinding.Components.HasPathfindingInstance>(true);
            structComponentsContainer.ValidateTag<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathBuilt>(true);

        }

        public static void InitEntity(Entity entity) {

            entity.ValidateDataCopyable<ME.ECS.Pathfinding.Features.Pathfinding.Components.PathfindingInstance>();
            entity.ValidateDataBlittable<ME.ECS.Pathfinding.Features.Pathfinding.Components.CalculatePath>();
            entity.ValidateDataCopyable<ME.ECS.Pathfinding.Features.PathfindingAstar.Components.Path>();
            entity.ValidateDataCopyable<ME.ECS.Pathfinding.Features.PathfindingFlowField.Components.PathFlowField>();
            entity.ValidateDataTag<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathfinding>(true);
            entity.ValidateDataTag<ME.ECS.Pathfinding.Features.Pathfinding.Components.BuildAllGraphs>(true);
            entity.ValidateDataTag<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsAllGraphsBuilt>(true);
            entity.ValidateDataTag<ME.ECS.Pathfinding.Features.Pathfinding.Components.HasPathfindingInstance>(true);
            entity.ValidateDataTag<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathBuilt>(true);
            
        }

    }

}
