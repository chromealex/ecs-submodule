using System.Collections;
using System.Collections.Generic;
using ME.ECS;
using UnityEngine;

public class DrawPathAstar : MonoBehaviour {

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
        var graph = this.pathfinding.GetNearest(this.transform.position, this.constraint).graph as ME.ECS.Pathfinding.GridGraph;
        var path = this.pathfinding.CalculatePath<ME.ECS.Pathfinding.PathCornersModifier, ME.ECS.Pathfinding.PathfindingAstarProcessor>(this.transform.position, this.to.position, this.constraint, graph, new ME.ECS.Pathfinding.PathCornersModifier(), 0, this.useBurst);
        if (path.result == ME.ECS.Pathfinding.PathCompleteState.Complete) {

            for (int i = 1; i < path.nodesModified.Count; ++i) {

                Gizmos.color = Color.white;
                var nodeNext = path.nodesModified[i];
                var current = path.nodesModified[i - 1].worldPosition;
                var next = path.nodesModified[i].worldPosition;
                Gizmos.DrawLine(current, next);

                do {

                    current = Vector3.MoveTowards(current, next, path.graph.GetNodeMinDistance());
                    var node = path.graph.GetNearest(current, cons);
                    if ( //node.walkable == false ||
                        node.penalty != nodeNext.penalty ||
                        node.IsSuitable(this.constraint) == false) {

                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(node.worldPosition, Vector3.one);
                        break;

                    }

                } while ((current - next).sqrMagnitude > 0.01f);
                
            }
            
        }
        path.Recycle();

    }
    #endif

}
