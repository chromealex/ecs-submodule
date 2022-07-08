using UnityEngine;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding.Tests {

    public class DrawPathAstar : MonoBehaviour {

        public ME.ECS.Pathfinding.Pathfinding pathfinding;
        public ME.ECS.Pathfinding.Constraint constraint;
        public UnityEngine.Transform to;
        public bool useBurst;

        #if UNITY_EDITOR
        public void OnDrawGizmos() {

            if (this.pathfinding == null || this.to == null) {

                return;

            }

            //ME.ECS.Pathfinding.PathfindingFlowFieldProcessor.cacheEnabled = true;

            var graph = this.pathfinding.GetNearest((float3)this.transform.position, this.constraint).graph as ME.ECS.Pathfinding.GridGraph;
            var path = this.pathfinding.CalculatePath<ME.ECS.Pathfinding.PathModifierEmpty, ME.ECS.Pathfinding.PathfindingAstarProcessor>(
                (float3)this.transform.position, (float3)this.to.position, this.constraint, graph, new ME.ECS.Pathfinding.PathModifierEmpty(), 0, this.useBurst);
            if (path.result == ME.ECS.Pathfinding.PathCompleteState.Complete ||
                path.result == ME.ECS.Pathfinding.PathCompleteState.CompletePartial) {

                for (int i = 1; i < path.nodes.Count; ++i) {

                    Gizmos.color = Color.white;
                    var current = path.nodes[i - 1].worldPosition;
                    var next = path.nodes[i].worldPosition;
                    Gizmos.DrawLine((Vector3)current, (Vector3)next);

                }

            }

            path.Recycle();

        }
        #endif

    }

}
