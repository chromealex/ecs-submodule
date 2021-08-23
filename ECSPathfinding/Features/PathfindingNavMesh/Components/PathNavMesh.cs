using ME.ECS;

namespace ME.ECS.Pathfinding.Features.PathfindingNavMesh.Components {

    public struct PathNavMesh : IComponent {

        public ME.ECS.Pathfinding.PathCompleteState result;
        public ME.ECS.Collections.NativeDataBufferArray<UnityEngine.Vector3> path;

    }

}