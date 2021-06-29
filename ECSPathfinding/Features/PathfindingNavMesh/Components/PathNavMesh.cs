using ME.ECS;

namespace ME.ECS.Pathfinding.Features.PathfindingNavMesh.Components {

    public struct PathNavMesh : IStructCopyable<PathNavMesh> {

        public ME.ECS.Pathfinding.PathCompleteState result;
        public ME.ECS.Collections.BufferArray<UnityEngine.Vector3> path;

        void IStructCopyable<PathNavMesh>.CopyFrom(in PathNavMesh other) {

            this.result = other.result;
            ArrayUtils.Copy(other.path, ref this.path);

        }

        void IStructCopyable<PathNavMesh>.OnRecycle() {

            this.result = default;
            this.path = this.path.Dispose();

        }

    }

}