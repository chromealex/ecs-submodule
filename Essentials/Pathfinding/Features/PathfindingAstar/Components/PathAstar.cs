using ME.ECS;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding.Features.PathfindingAstar.Components {

    public struct Path : IComponent, IComponentRuntime {

        public byte resultValue;
        public ME.ECS.Pathfinding.PathCompleteState result {
            get => (ME.ECS.Pathfinding.PathCompleteState)this.resultValue;
            set => this.resultValue = (byte)value;
        }
        public ME.ECS.Collections.V3.MemArrayAllocator<float3> path;
        public ME.ECS.Collections.V3.MemArrayAllocator<int> nodes;

    }

}