using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding {

    public class PathCornerModifierSeeker : PathModifierSeeker {

        public PathCornersModifier modifier;

        public override Path Run(Path path, Constraint constraint) {

            return this.modifier.Run(path, constraint);

        }

        public override bool IsWalkable(int index, Constraint constraint) {
            
            return this.modifier.IsWalkable(index, constraint);
            
        }

    }

    public struct PathCustomWalkableField : IPathModifier {

        public Unity.Collections.NativeArray<int> walkableField;
        public Unity.Collections.NativeArray<int> erosionField;
        public int customCost;

        public bool IsTraversable(int index, BurstConstraint constraint) {

            if (this.erosionField.Length == 0) return true;
            if (constraint.agentSize > 0) {

                if (this.erosionField[index] == 0 || this.erosionField[index] > constraint.agentSize) return true;
                
                return false;
                
            }
            
            return true;
            
        }
        
        public bool IsWalkable(int index, BurstConstraint constraint) {

            if (this.walkableField.Length == 0) return true;
            return this.walkableField[index] == 0;

        }

        public int GetCustomCost(int index, BurstConstraint constraint) {

            if (this.walkableField.Length == 0) return this.customCost;
            return this.customCost - this.walkableField[index];

        }
        
        public bool IsWalkable(int index, Constraint constraint) {

            if (this.walkableField.Length == 0) return true;
            return this.walkableField[index] == 0;

        }

        public int GetCustomCost(int index, Constraint constraint) {

            if (this.walkableField.Length == 0) return this.customCost;
            return this.customCost - this.walkableField[index];

        }

        public Path Run(Path path, Constraint constraint) {

            return path;

        }

    }
    
    public struct PathCornersModifier : IPathModifier {
        
        public bool IsWalkable(int index, Constraint constraint) {

            return true;

        }

        public Path Run(Path path, Constraint constraint) {

            ref var nodes = ref path.nodes;
            if (nodes == null) return path;
            
            var corners = PoolListCopyable<Node>.Spawn(10);
            
            var prevDir = -1;
            for (int i = 0; i < nodes.Count - 1; ++i) {

                var node = nodes[i];
                var next = nodes[i + 1];
                var dir = 0;
                var connections = node.GetConnections();
                for (int j = 0; j < connections.Length; ++j) {

                    if (connections[j].index == next.index) {

                        dir = j;
                        break;
                        
                    }
                    
                }

                if (prevDir != dir) {
                    
                    corners.Add(node);
                    prevDir = dir;

                }

            }
            corners.Add(path.graph.GetNearest(nodes[nodes.Count - 1].worldPosition, constraint).node);

            this.UpdateCorners(path, constraint, nodes, corners);
            path.nodesModified = corners;

            return path;

        }

        private void UpdateCorners(Path path, Constraint constraint, ME.ECS.Collections.ListCopyable<Node> nodes, ME.ECS.Collections.ListCopyable<Node> corners) {
            
            var cons = Constraint.Empty;
            cons.graphMask = constraint.graphMask;
            for (int iter = 0; iter < corners.Count; ++iter) {

                for (int i = 0; i < corners.Count - 2; ++i) {

                    var currentIndex = i;
                    var nextIndex = i + 2;

                    var current = corners[currentIndex];
                    var next = corners[nextIndex];
                    var allWalkable = true;
                    var pos = current.worldPosition;
                    do {

                        pos = VecMath.MoveTowards(pos, next.worldPosition, path.graph.GetNodeMinDistance());
                        
                        var node = path.graph.GetNearest(pos, cons);
                        if ( //node.walkable == false ||
                            math.abs(node.node.penalty - current.penalty) > sfloat.Epsilon ||
                            node.node.IsSuitable(constraint) == false) {

                            allWalkable = false;
                            break;

                        }
                        
                    } while (math.distancesq(pos, next.worldPosition) > 0.01f);
                    
                    if (allWalkable == true) {

                        if (i + 1 < corners.Count) {
                        
                            corners.RemoveAt(i + 1);
        
                        }
                        
                    }

                    //var distance = (next.worldPosition - c.worldPosition).magnitude;
                    /*if (Physics.CapsuleCast(
                            c.worldPosition,
                            c.worldPosition + Vector3.up * this.agentHeight,
                            this.agentRadius,
                            next.worldPosition - c.worldPosition,
                            distance,
                            this.collisionMask
                        ) == false) {
                        
                        nodes.RemoveAt(i + 1);
                        --i;

                    }*/

                }

            }

        }

    }

}
