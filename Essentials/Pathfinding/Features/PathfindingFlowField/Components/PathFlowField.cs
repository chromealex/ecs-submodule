using ME.ECS;

namespace ME.ECS.Pathfinding.Features.PathfindingFlowField.Components {

    public struct PathFlowField : IStructCopyable<PathFlowField> {

        public ME.ECS.Collections.BufferArray<byte> flowField;
        public UnityEngine.Vector3 from;
        public UnityEngine.Vector3 to;
        public bool cacheEnabled;

        public void CopyFrom(in PathFlowField other) {
            
            this.from = other.from;
            this.to = other.to;
            this.cacheEnabled = other.cacheEnabled;
            ArrayUtils.Copy(other.flowField, ref this.flowField);
            
        }

        public void OnRecycle() {

            this.from = default;
            this.to = default;
            this.cacheEnabled = default;
            /*if (this.cacheEnabled == false) */PoolArray<byte>.Recycle(ref this.flowField);
            
        }

    }
    
}