using System.Collections;
using System.Collections.Generic;
using ME.ECS;
using UnityEngine;

public class DrawPathNavMesh : MonoBehaviour {

    public ME.ECS.Pathfinding.Pathfinding pathfinding;
    public ME.ECS.Pathfinding.Constraint constraint;
    public Transform to;
    public bool useBurst;

    #if UNITY_EDITOR
    public void OnDrawGizmos() {

        if (this.pathfinding == null || this.to == null) {

            return;

        }

        //ME.ECS.Pathfinding.PathfindingFlowFieldProcessor.cacheEnabled = true;

        var cons = ME.ECS.Pathfinding.Constraint.Empty;
        cons.graphMask = this.constraint.graphMask;
        var graph = this.pathfinding.GetNearest(this.transform.position, this.constraint).graph;
        var path = this.pathfinding.CalculatePath<ME.ECS.Pathfinding.PathfindingNavMeshProcessor>(this.transform.position, this.to.position, this.constraint);
        if (path.result == ME.ECS.Pathfinding.PathCompleteState.Complete ||
            path.result == ME.ECS.Pathfinding.PathCompleteState.CompletePartial) {

            var fromNode = graph.GetNearest(this.transform.position, this.constraint);
            var toNode = graph.GetNearest(this.to.position, this.constraint);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(fromNode.worldPosition, fromNode.worldPosition + Vector3.up * 10f);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(toNode.worldPosition, toNode.worldPosition + Vector3.up * 10f);

            {

                for (int i = 1; i < path.navMeshPoints.Count; ++i) {

                    Gizmos.color = Color.white;
                    var current = path.navMeshPoints[i - 1];
                    var next = path.navMeshPoints[i];
                    Gizmos.DrawLine(current, next);

                }

            }

        }
        path.Recycle();

    }
    #endif

}
