using ME.ECS;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding.Features.PathfindingNavMesh.Components {

    public struct PathNavMesh : ICopyable<PathNavMesh> {

        public byte resultValue;
        public ME.ECS.Pathfinding.PathCompleteState result {
            get => (ME.ECS.Pathfinding.PathCompleteState)this.resultValue;
            set => this.resultValue = (byte)value;
        }
        public ME.ECS.Collections.ListCopyable<float3> path;

        public void CopyFrom(in PathNavMesh other) {
            this.resultValue = other.resultValue;
            ArrayUtils.Copy(other.path, ref this.path);
        }

        public void OnRecycle() {
            this.resultValue = default;
            PoolListCopyable<float3>.Recycle(ref this.path);
        }

    }

}