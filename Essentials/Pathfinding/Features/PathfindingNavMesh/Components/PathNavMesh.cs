using ME.ECS;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding.Features.PathfindingNavMesh.Components {

    public struct PathNavMesh : ICopyable<PathNavMesh> {

        public ME.ECS.Pathfinding.PathCompleteState result;
        public ME.ECS.Collections.ListCopyable<float3> path;

        public void CopyFrom(in PathNavMesh other) {
            this.result = other.result;
            ArrayUtils.Copy(other.path, ref this.path);
        }

        public void OnRecycle() {
            this.result = default;
            PoolListCopyable<float3>.Recycle(ref this.path);
        }

    }

}