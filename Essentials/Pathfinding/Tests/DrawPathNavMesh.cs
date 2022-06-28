using UnityEngine;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding.Tests {

    public class DrawPathNavMesh : MonoBehaviour {

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

            var cons = ME.ECS.Pathfinding.Constraint.Empty;
            cons.graphMask = this.constraint.graphMask;
            var graph = this.pathfinding.GetNearest((float3)this.transform.position, this.constraint).graph;
            var path = this.pathfinding.CalculatePath<ME.ECS.Pathfinding.PathModifierEmpty, ME.ECS.Pathfinding.PathfindingNavMeshProcessor>(
                (float3)this.transform.position, (float3)this.to.position, this.constraint, graph, new ME.ECS.Pathfinding.PathModifierEmpty(), burstEnabled: this.useBurst);
            if (path.result == ME.ECS.Pathfinding.PathCompleteState.Complete ||
                path.result == ME.ECS.Pathfinding.PathCompleteState.CompletePartial) {

                var fromNode = graph.GetNearest((float3)this.transform.position, this.constraint);
                var toNode = graph.GetNearest((float3)this.to.position, this.constraint);

                Gizmos.color = Color.green;
                Gizmos.DrawLine((Vector3)fromNode.worldPosition, (Vector3)fromNode.worldPosition + Vector3.up * 10f);

                Gizmos.color = Color.red;
                Gizmos.DrawLine((Vector3)toNode.worldPosition, (Vector3)toNode.worldPosition + Vector3.up * 10f);

                {

                    for (int i = 1; i < path.navMeshPoints.Count; ++i) {

                        Gizmos.color = Color.white;
                        var current = path.navMeshPoints[i - 1];
                        var next = path.navMeshPoints[i];
                        Gizmos.DrawLine((Vector3)current, (Vector3)next);

                    }

                }

            }

            path.Recycle();

        }
        #endif

    }

}