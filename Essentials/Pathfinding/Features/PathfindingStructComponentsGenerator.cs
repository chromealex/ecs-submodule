
namespace ME.ECS {

    public static class PathfindingComponentsInitializer {
    
        public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.PathfindingInstance>(false, false, false, true, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.CalculatePath>(false, false, true, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.PathfindingAstar.Components.Path>(false, false, false, true, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.PathfindingFlowField.Components.PathFlowField>(false, false, false, true, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathfinding>(true, false, true, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.BuildAllGraphs>(true, false, true, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsAllGraphsBuilt>(true, false, true, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.HasPathfindingInstance>(true, false, true, false, false, false);
            WorldUtilities.InitComponentTypeId<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathBuilt>(true, false, true, false, false, false);
            
            structComponentsContainer.ValidateCopyable<ME.ECS.Pathfinding.Features.Pathfinding.Components.PathfindingInstance>(false);
            structComponentsContainer.ValidateBlittable<ME.ECS.Pathfinding.Features.Pathfinding.Components.CalculatePath>();
            structComponentsContainer.ValidateCopyable<ME.ECS.Pathfinding.Features.PathfindingAstar.Components.Path>();
            structComponentsContainer.ValidateCopyable<ME.ECS.Pathfinding.Features.PathfindingFlowField.Components.PathFlowField>();
            structComponentsContainer.ValidateBlittable<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathfinding>(true);
            structComponentsContainer.ValidateBlittable<ME.ECS.Pathfinding.Features.Pathfinding.Components.BuildAllGraphs>(true);
            structComponentsContainer.ValidateBlittable<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsAllGraphsBuilt>(true);
            structComponentsContainer.ValidateBlittable<ME.ECS.Pathfinding.Features.Pathfinding.Components.HasPathfindingInstance>(true);
            structComponentsContainer.ValidateBlittable<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathBuilt>(true);

        }

        public static void InitEntity(Entity entity) {

            entity.ValidateDataCopyable<ME.ECS.Pathfinding.Features.Pathfinding.Components.PathfindingInstance>();
            entity.ValidateDataBlittable<ME.ECS.Pathfinding.Features.Pathfinding.Components.CalculatePath>();
            entity.ValidateDataCopyable<ME.ECS.Pathfinding.Features.PathfindingAstar.Components.Path>();
            entity.ValidateDataCopyable<ME.ECS.Pathfinding.Features.PathfindingFlowField.Components.PathFlowField>();
            entity.ValidateDataBlittable<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathfinding>(true);
            entity.ValidateDataBlittable<ME.ECS.Pathfinding.Features.Pathfinding.Components.BuildAllGraphs>(true);
            entity.ValidateDataBlittable<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsAllGraphsBuilt>(true);
            entity.ValidateDataBlittable<ME.ECS.Pathfinding.Features.Pathfinding.Components.HasPathfindingInstance>(true);
            entity.ValidateDataBlittable<ME.ECS.Pathfinding.Features.Pathfinding.Components.IsPathBuilt>();
            
        }

    }

}
