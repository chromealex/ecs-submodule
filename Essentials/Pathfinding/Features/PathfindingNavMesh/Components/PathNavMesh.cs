using ME.ECS;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding.Features.PathfindingNavMesh.Components {

    public struct PathNavMesh : IComponent {

        public ME.ECS.Pathfinding.PathCompleteState result;
        public ME.ECS.Collections.NativeDataBufferArray<float3> path;

    }

}