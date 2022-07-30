using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding.Features.Pathfinding.Components {

    public enum PathType : byte {

        AStar,
        FlowField,
        NavMesh,

    }
    
    public struct IsPathfinding : IComponent {}
    public struct BuildAllGraphs : IComponent {}
    public struct HasPathfindingInstance : IComponent {}
    public struct IsAllGraphsBuilt : IComponent {}
    
    public struct CalculatePath : IComponent {

        public float3 from;
        public float3 to;
        public bool alignToGraphNodes;
        public ME.ECS.Pathfinding.Constraint constraint;
        public byte pathTypeValue;
        public PathType pathType {
            get => (PathType)this.pathTypeValue;
            set => this.pathTypeValue = (byte)value;
        }
        public bool burstEnabled;
        public bool cacheEnabled;

    }

    public struct IsPathBuilt : IComponent {}
    
    #if COMPONENTS_COPYABLE
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
    #else
    public struct PathfindingInstance : IComponent, IComponentRuntime {

        public ME.ECS.Pathfinding.Pathfinding pathfinding;

    }
    #endif

}