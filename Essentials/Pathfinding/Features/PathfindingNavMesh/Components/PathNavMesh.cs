using ME.ECS;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding.Features.PathfindingNavMesh.Components {

    public struct PathNavMesh : IComponent, IComponentRuntime, IComponentDisposable {

        public byte resultValue;
        public ME.ECS.Pathfinding.PathCompleteState result {
            get => (ME.ECS.Pathfinding.PathCompleteState)this.resultValue;
            set => this.resultValue = (byte)value;
        }
        public ME.ECS.Collections.MemoryAllocator.List<float3> path;

        public void OnDispose(ref ME.ECS.Collections.V3.MemoryAllocator allocator) {
            if (this.path.isCreated == true) this.path.Dispose(ref allocator);
        }

    }

}