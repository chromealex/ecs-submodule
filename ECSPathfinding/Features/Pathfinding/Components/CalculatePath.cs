
namespace ME.ECS.Pathfinding.Features.Pathfinding.Components {

    public enum PathType {

        AStar,
        FlowField,
        NavMesh,

    }
    
    public struct IsPathfinding : IStructComponent {}
    public struct BuildAllGraphs : IStructComponent {}
    public struct HasPathfindingInstance : IStructComponent {}
    public struct IsAllGraphsBuilt : IStructComponent {}
    
    public struct CalculatePath : IStructComponent {

        public UnityEngine.Vector3 from;
        public UnityEngine.Vector3 to;
        public bool alignToGraphNodes;
        public ME.ECS.Pathfinding.Constraint constraint;
        public PathType pathType;
        public bool burstEnabled;
        public bool cacheEnabled;

    }

    public struct IsPathBuilt : IStructComponent {}

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