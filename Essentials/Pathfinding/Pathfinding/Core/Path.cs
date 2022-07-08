using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ME.ECS.Mathematics;

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
        public ListCopyable<float3> navMeshPoints;

        public void Recycle() {
            
            this.result = PathCompleteState.NotCalculated;
            this.graph = null;
            this.cacheEnabled = default;

            if (this.nodes != null) PoolListCopyable<Node>.Recycle(ref this.nodes);
            if (this.nodesModified != null) PoolListCopyable<Node>.Recycle(ref this.nodesModified);
            
            if (this.cacheEnabled == false && this.flowField.arr != null) PoolArray<byte>.Recycle(ref this.flowField);
            
            if (this.navMeshPoints != null) PoolListCopyable<float3>.Recycle(ref this.navMeshPoints);

        }

        public static Path Clone(in Path other) {
            
            var path = new Path {
                result = other.result,
                graph = other.graph,
                cacheEnabled = other.cacheEnabled,
            };

            if (other.nodes != null) {
                path.nodes = PoolListCopyable<Node>.Spawn(other.nodes.Count);
                path.nodes.AddRange(other.nodes);
            }

            if (other.nodesModified != null) {
                path.nodesModified = PoolListCopyable<Node>.Spawn(other.nodesModified.Count);
                path.nodesModified.AddRange(other.nodesModified);
            }
            
            if (other.flowField.arr != null) {
                path.flowField = BufferArray<byte>.From(other.flowField);
            }
            
            if (other.navMeshPoints != null) {
                path.navMeshPoints = PoolListCopyable<float3>.Spawn(other.navMeshPoints.Count);
                path.navMeshPoints.AddRange(other.navMeshPoints);
            }

            return path;
        }

    }

}