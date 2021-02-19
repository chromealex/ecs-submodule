using ME.ECS;

namespace ME.ECS.Pathfinding.Features.Pathfinding.Components {

    public struct PathFlowField : IStructCopyable<PathFlowField> {

        public ME.ECS.Collections.BufferArray<byte> flowField;
        public UnityEngine.Vector3 from;
        public UnityEngine.Vector3 to;

        public void CopyFrom(in PathFlowField other) {
            
            this.from = other.from;
            this.to = other.to;
            ArrayUtils.Copy(other.flowField, ref this.flowField);
            
        }

        public void OnRecycle() {

            this.from = default;
            this.to = default;
            PoolArray<byte>.Recycle(ref this.flowField);
            
        }

    }
    
}