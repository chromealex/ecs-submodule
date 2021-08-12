using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ME.ECS.Pathfinding {

    using ME.ECS.Collections;
    
    public enum PathCompleteState {

        NotCalculated = 0,
        Complete,
        CompletePartial,
        NotExist,

    }
 
    public struct Path {

        public PathCompleteState result;
        public Graph graph;
        public bool cacheEnabled;
        
        // For Astar processor
        public ListCopyable<Node> nodes;
        public ListCopyable<Node> nodesModified;
        
        // For FlowField processor
        public BufferArray<byte> flowField;
        
        // For NavMesh processor
        public ListCopyable<Vector3> navMeshPoints;

        public void Recycle() {
            
            this.result = PathCompleteState.NotCalculated;
            this.graph = null;
            this.cacheEnabled = default;

            if (this.nodes != null) PoolListCopyable<Node>.Recycle(ref this.nodes);
            if (this.nodesModified != null) PoolListCopyable<Node>.Recycle(ref this.nodesModified);
            
            if (this.cacheEnabled == false && this.flowField.arr != null) PoolArray<byte>.Recycle(ref this.flowField);
            
            if (this.navMeshPoints != null) PoolListCopyable<Vector3>.Recycle(ref this.navMeshPoints);

        }

    }

}