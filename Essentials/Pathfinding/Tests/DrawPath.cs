using UnityEngine;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding.Tests {

    public class DrawPath : MonoBehaviour {

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
            var graph = this.pathfinding.GetNearest((float3)this.transform.position, this.constraint).graph as ME.ECS.Pathfinding.GridGraph;
            var path = this.pathfinding.CalculatePath<ME.ECS.Pathfinding.PathCornersModifier, ME.ECS.Pathfinding.PathfindingFlowFieldProcessor>(
                (float3)this.transform.position, (float3)this.to.position, this.constraint, graph, new ME.ECS.Pathfinding.PathCornersModifier(), 0, this.useBurst);
            if (path.result == ME.ECS.Pathfinding.PathCompleteState.Complete ||
                path.result == ME.ECS.Pathfinding.PathCompleteState.CompletePartial) {

                var fromNode = graph.GetNearest((float3)this.transform.position, this.constraint);
                var toNode = graph.GetNearest((float3)this.to.position, this.constraint);

                Gizmos.color = Color.green;
                Gizmos.DrawLine((Vector3)fromNode.worldPosition, (Vector3)fromNode.worldPosition + Vector3.up * 10f);

                Gizmos.color = Color.red;
                Gizmos.DrawLine((Vector3)toNode.worldPosition, (Vector3)toNode.worldPosition + Vector3.up * 10f);

                if (path.flowField.arr != null) {

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

                        Gizmos.color = Color.white;
                        var offset = Vector3.up * 0.1f;
                        UnityEditor.Handles.color = Color.white;
                        #if UNITY_2020_2_OR_NEWER
                        var thickness = 4f;
                        UnityEditor.Handles.DrawLine(offset + (Vector3)node.worldPosition - dir3d * nodeSize * 0.3f, offset + (Vector3)node.worldPosition + dir3d * nodeSize * 0.3f,
                                                     thickness);
                        UnityEditor.Handles.DrawLine(offset + (Vector3)node.worldPosition + dir3d * nodeSize * 0.3f,
                                                     offset + ((Vector3)node.worldPosition + Quaternion.Euler(0f, 120f, 0f) * dir3d * nodeSize * 0.1f), thickness);
                        UnityEditor.Handles.DrawLine(offset + (Vector3)node.worldPosition + dir3d * nodeSize * 0.3f,
                                                     offset + ((Vector3)node.worldPosition + Quaternion.Euler(0f, -120f, 0f) * dir3d * nodeSize * 0.1f), thickness);
                        #else
                    UnityEditor.Handles.DrawLine(offset + node.worldPosition - dir3d * nodeSize * 0.3f, offset + node.worldPosition + dir3d * nodeSize * 0.3f);
                    UnityEditor.Handles.DrawLine(offset + node.worldPosition + dir3d * nodeSize * 0.3f, offset + (node.worldPosition + Quaternion.Euler(0f, 120f, 0f) * dir3d * nodeSize * 0.1f));
                    UnityEditor.Handles.DrawLine(offset + node.worldPosition + dir3d * nodeSize * 0.3f, offset + (node.worldPosition + Quaternion.Euler(0f, -120f, 0f) * dir3d * nodeSize * 0.1f));
                        #endif

                    }

                    var from = this.transform.position;
                    var currentNode = graph.GetNearest((float3)from, this.constraint).node;
                    var targetNode = graph.GetNearest((float3)this.to.position, this.constraint).node;
                    Gizmos.color = Color.blue;
                    var max = 10000;
                    while (currentNode.index != targetNode.index) {

                        if (--max <= 0) break;

                        var currentDir = (ME.ECS.Pathfinding.GridGraph.Direction)path.flowField.arr[currentNode.index];
                        var nextNodeIndex = ME.ECS.Pathfinding.GridGraphUtilities.GetIndexByDirection(graph, currentNode.index, currentDir);
                        var nextNode = graph.GetNodeByIndex<ME.ECS.Pathfinding.GridNode>(nextNodeIndex);

                        if (nextNode != null) {

                            Gizmos.DrawLine((Vector3)currentNode.worldPosition, (Vector3)nextNode.worldPosition);
                            currentNode = nextNode;

                        } else {

                            break;

                        }

                    }

                } else {

                    for (int i = 1; i < path.nodesModified.Count; ++i) {

                        Gizmos.color = Color.white;
                        var nodeNext = path.nodesModified[i];
                        var current = path.nodesModified[i - 1].worldPosition;
                        var next = path.nodesModified[i].worldPosition;
                        Gizmos.DrawLine((Vector3)current, (Vector3)next);

                        do {

                            current = VecMath.MoveTowards(current, next, path.graph.GetNodeMinDistance());
                            var node = path.graph.GetNearest(current, cons);
                            if ( //node.walkable == false ||
                                node.node.penalty != nodeNext.penalty ||
                                node.node.IsSuitable(this.constraint) == false) {

                                Gizmos.color = Color.red;
                                Gizmos.DrawCube((Vector3)node.worldPosition, Vector3.one);
                                break;

                            }

                        } while (math.distancesq(current, next) > 0.01f);

                    }

                }

            }

            path.Recycle();

        }
        #endif

    }

}