using ME.ECS;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding.Features.PathfindingFlowField.Components {

    public struct PathFlowField : IComponent, IComponentRuntime {

        public ME.ECS.Collections.V3.MemArrayAllocator<byte> flowField;
        public float3 from;
        public float3 to;
        public bool cacheEnabled;

    }
    
}