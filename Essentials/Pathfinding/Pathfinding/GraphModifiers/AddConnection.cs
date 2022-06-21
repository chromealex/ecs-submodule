using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding {
    
    [ExecuteInEditMode]
    public class AddConnection : GraphModifierBase {

        public UnityEngine.Transform currentNode;
        public UnityEngine.Transform connectTo;
        public bool doubleSided;

        public override void ApplyAfterConnections(Graph graph) {
            
            this.Connect(graph, (float3)this.currentNode.position, (float3)this.connectTo.position);
            if (this.doubleSided == true) {
            
                this.Connect(graph, (float3)this.connectTo.position, (float3)this.currentNode.position);

            }

        }

        public override void ApplyBeforeConnections(Graph graph) {
            
        }
        
        private void Connect(Graph graph, float3 nearestPos, float3 connectToPos) {
            
            var nearest = graph.GetNearest(nearestPos, Constraint.Empty);
            var connectTo = graph.GetNearest(connectToPos, Constraint.Empty);

            nearest.node.AddConnection(connectTo.node);
            
        }

    }

}
