using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Pathfinding {
    
    [ExecuteInEditMode]
    public class AddConnection : GraphModifierBase {

        public UnityEngine.Transform currentNode;
        public UnityEngine.Transform connectTo;
        public bool doubleSided;

        public override void ApplyAfterConnections(Graph graph) {
            
            this.Connect(graph, this.currentNode.position, this.connectTo.position);
            if (this.doubleSided == true) {
            
                this.Connect(graph, this.connectTo.position, this.currentNode.position);

            }

        }

        public override void ApplyBeforeConnections(Graph graph) {
            
        }
        
        private void Connect(Graph graph, Vector3 nearestPos, Vector3 connectToPos) {
            
            var nearest = graph.GetNearest(nearestPos, Constraint.Empty);
            var connectTo = graph.GetNearest(connectToPos, Constraint.Empty);

            nearest.node.AddConnection(connectTo.node);
            
        }

    }

}
