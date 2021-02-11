
namespace ME.ECS.Pathfinding.Features.Pathfinding.Components {

    public struct IsPathfinding : IStructComponent {}
    public struct BuildAllGraphs : IStructComponent {}
    public struct HasPathfindingInstance : IStructComponent {}
    public struct IsAllGraphsBuilt : IStructComponent {}
    
    public struct CalculatePath : IStructComponent {

        public UnityEngine.Vector3 from;
        public UnityEngine.Vector3 to;
        public ME.ECS.Pathfinding.Constraint constraint;

    }

    public struct IsPathBuilt : IStructComponent {}

    public struct Path : IStructCopyable<Path> {

        public ME.ECS.Pathfinding.PathCompleteState result;
        public ME.ECS.Collections.BufferArray<UnityEngine.Vector3> path;
        public ME.ECS.Collections.BufferArray<ME.ECS.Pathfinding.Node> nodes;

        void IStructCopyable<Path>.CopyFrom(in Path other) {

            this.result = other.result;
            ArrayUtils.Copy(in other.path, ref this.path); 
            ArrayUtils.Copy(in other.nodes, ref this.nodes); 

        }

        void IStructCopyable<Path>.OnRecycle() {

            this.result = default;
            PoolArray<UnityEngine.Vector3>.Recycle(ref this.path);
            PoolArray<ME.ECS.Pathfinding.Node>.Recycle(ref this.nodes);
        
        }

    }

    public struct PathfindingInstance : IStructCopyable<PathfindingInstance> {

        public ME.ECS.Pathfinding.Pathfinding pathfinding;
        
        void IStructCopyable<PathfindingInstance>.CopyFrom(in PathfindingInstance other) {

            if (this.pathfinding == null && other.pathfinding == null) {

                return;

            }

            if (this.pathfinding == null && other.pathfinding != null) {
                
                this.pathfinding = (other.pathfinding.clonePathfinding == true ? other.pathfinding.Clone() : other.pathfinding);
                
            } else {

                if (other.pathfinding.clonePathfinding == true) {
                    
                    this.pathfinding.CopyFrom(other.pathfinding);
                    
                } else {

                    this.pathfinding = other.pathfinding;

                }

            }
            
        }

        void IStructCopyable<PathfindingInstance>.OnRecycle() {

            if (this.pathfinding != null && this.pathfinding.clonePathfinding == true) this.pathfinding.Recycle();
            this.pathfinding = null;

        }

    }

}