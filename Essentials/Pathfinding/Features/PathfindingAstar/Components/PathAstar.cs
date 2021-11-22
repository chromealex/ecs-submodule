using ME.ECS;

namespace ME.ECS.Pathfinding.Features.PathfindingAstar.Components {

    public struct Path : IStructCopyable<Path> {

        public ME.ECS.Pathfinding.PathCompleteState result;
        public ME.ECS.Collections.BufferArray<UnityEngine.Vector3> path;
        public ME.ECS.Collections.BufferArray<ME.ECS.Pathfinding.Node> nodes;

        void IStructCopyable<Path>.CopyFrom(in Path other) {

            //var val = other.path.arr[0];
            this.result = other.result;
            ArrayUtils.Copy(in other.path, ref this.path);
            ArrayUtils.Copy(in other.nodes, ref this.nodes);
            //UnityEngine.Debug.Log($"COPY PATH: " + val + " >> " + this.path.arr[0] + "\n" + this.ToString());

        }

        void IStructCopyable<Path>.OnRecycle() {

            //UnityEngine.Debug.LogWarning("PATH RECYCLE");
            this.result = default;
            PoolArray<UnityEngine.Vector3>.Recycle(ref this.path);
            PoolArray<ME.ECS.Pathfinding.Node>.Recycle(ref this.nodes);
        
        }

        /*
        public override string ToString() {
            
            return (this.path.arr != null ? string.Join(", ", this.path.arr.data) : "NULL");
            
        }*/

    }

}