using System.Collections;
using System.Collections.Generic;
using ME.ECS;
using UnityEngine;

public class DrawPath : MonoBehaviour {

    public ME.ECS.Pathfinding.Pathfinding pathfinding;
    public ME.ECS.Pathfinding.Constraint constraint;
    public Transform to;
    public float agentRadius;

    public void OnDrawGizmos() {

        if (this.pathfinding == null || this.to == null) {

            return;

        }

        var cons = ME.ECS.Pathfinding.Constraint.Empty;
        cons.graphMask = this.constraint.graphMask;
        var path = this.pathfinding.CalculatePath(this.transform.position, this.to.position, this.constraint, new ME.ECS.Pathfinding.PathCornersModifier());
        if (path.result == ME.ECS.Pathfinding.PathCompleteState.Complete) {

            if (path.flowField.arr != null) {

                var graph = this.pathfinding.graphs[0] as ME.ECS.Pathfinding.GridGraph;
                var nodeSize = graph.nodeSize;

                /*var maxIntegrationCost = 0f;
                for (int i = 0; i < path.integrationField.Length; ++i) {

                    if (maxIntegrationCost < path.integrationField.arr[i]) {

                        maxIntegrationCost = path.integrationField.arr[i];

                    }
                    
                }*/

                for (int i = 0; i < path.flowField.Length; ++i) {

                    var dir = path.flowField.arr[i];
                    var node = graph.nodes[i];

                    /*var ffCost = path.integrationField.arr[i];
                    Gizmos.color = Color.Lerp(new Color(0f, 1f, 0f, 0.5f), new Color(1f, 0f, 0f, 0.5f), ffCost >= 0f ? (maxIntegrationCost > 0 ? ffCost / (float)maxIntegrationCost : 0f) : 1f);
                    Gizmos.DrawCube(node.worldPosition, new Vector3(5f, 0.1f, 5f));*/
                    //UnityEditor.Handles.Label(node.worldPosition + Vector3.up * 0.5f, ffCost.ToString());
                    
                    //UnityEditor.Handles.Label(node.worldPosition, ((ME.ECS.Pathfinding.GridGraph.Direction)dir).ToString());
                    var dir3d = ME.ECS.Pathfinding.GridGraphUtilities.GetDirection((ME.ECS.Pathfinding.GridGraph.Direction)dir);
                    
                    Gizmos.DrawLine(node.worldPosition - dir3d * nodeSize * 0.3f, node.worldPosition + dir3d * nodeSize * 0.3f);
                    Gizmos.DrawLine(node.worldPosition + dir3d * nodeSize * 0.3f, (node.worldPosition + Quaternion.Euler(0f, 120f, 0f) * dir3d * nodeSize * 0.1f));
                    Gizmos.DrawLine(node.worldPosition + dir3d * nodeSize * 0.3f, (node.worldPosition + Quaternion.Euler(0f, -120f, 0f) * dir3d * nodeSize * 0.1f));
                    
                }

                var stepSpeed = 0.033f;
                var to = this.to.position;
                var from = this.transform.position;
                var radius = this.agentRadius;
                Gizmos.color = Color.blue;
                var max = 10000;
                ME.ECS.Pathfinding.GridGraph.Direction prevDir = ME.ECS.Pathfinding.GridGraph.Direction.Up;
                Vector3 prevPos = from;
                var dist = 0f;
                while ((from.XZ() - to.XZ()).sqrMagnitude >= 1f) {

                    if (--max <= 0) break;
                    
                    var currentNode = graph.GetNearest(from, this.constraint);
                    var currentDir = (ME.ECS.Pathfinding.GridGraph.Direction)path.flowField.arr[currentNode.index];
                    if (currentDir != prevDir) {

                        //prevPos = from;
                        dist = radius + (prevPos - from).magnitude;
                        prevDir = currentDir;
                        Gizmos.DrawWireCube(prevPos, Vector3.one * 0.2f);
                        Gizmos.DrawCube(from, Vector3.one * 0.2f);
                        
                    }
                    if ((prevPos - from).sqrMagnitude >= dist * dist) {

                        prevPos = from;
                        Gizmos.DrawSphere(from, 0.1f);

                    }

                    var node = graph.GetNearest(prevPos, this.constraint);
                    var dir = (ME.ECS.Pathfinding.GridGraph.Direction)path.flowField.arr[node.index];
                    if (dir == ME.ECS.Pathfinding.GridGraph.Direction.Up) break;

                    var dir3d = ME.ECS.Pathfinding.GridGraphUtilities.GetDirection(dir);
                    var nextPosition = from + dir3d * stepSpeed;
                    Gizmos.DrawLine(from, nextPosition);
                    from = nextPosition;

                }

            } else {

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

        }
        path.Recycle();

    }

}
