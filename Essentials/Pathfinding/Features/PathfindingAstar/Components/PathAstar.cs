using ME.ECS;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding.Features.PathfindingAstar.Components {

    public struct Path : IStructCopyable<Path> {

        public byte resultValue;
        public ME.ECS.Pathfinding.PathCompleteState result {
            get => (ME.ECS.Pathfinding.PathCompleteState)this.resultValue;
            set => this.resultValue = (byte)value;
        }
        public ME.ECS.Collections.BufferArray<float3> path;
        public ME.ECS.Collections.BufferArray<ME.ECS.Pathfinding.Node> nodes;

        void IStructCopyable<Path>.CopyFrom(in Path other) {

            //var val = other.path.arr[0];
            this.resultValue = other.resultValue;
            ArrayUtils.Copy(in other.path, ref this.path);
            ArrayUtils.Copy(in other.nodes, ref this.nodes);
            //UnityEngine.Debug.Log($"COPY PATH: " + val + " >> " + this.path.arr[0] + "\n" + this.ToString());

        }

        void IStructCopyable<Path>.OnRecycle() {

            //UnityEngine.Debug.LogWarning("PATH RECYCLE");
            this.resultValue = default;
            PoolArray<float3>.Recycle(ref this.path);
            PoolArray<ME.ECS.Pathfinding.Node>.Recycle(ref this.nodes);
        
        }

        /*
        public override string ToString() {
            
            return (this.path.arr != null ? string.Join(", ", this.path.arr.data) : "NULL");
            
        }*/

    }

}