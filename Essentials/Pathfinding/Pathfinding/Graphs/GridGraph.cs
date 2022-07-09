#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using ME.ECS.Mathematics;

using Unity.Jobs;
using UnityEngine;

namespace ME.ECS.Pathfinding {

    using ME.ECS.Collections;

    #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class GridGraph : Graph {

        public enum Direction : int {

            Up = 0,
            Down = 1,
            Forward = 2,
            Right = 3,
            Backward = 4,
            Left = 5,

            RightForward = 6,
            RightBackward = 7,
            LeftBackward = 8,
            LeftForward = 9,

            RightUpForward = 10,
            RightUpBackward = 11,
            LeftUpBackward = 12,
            LeftUpForward = 13,

            RightDownForward = 14,
            RightDownBackward = 15,
            LeftDownBackward = 16,
            LeftDownForward = 17,

        }

        public enum DrawMode : byte {

            None = 0,
            Solid,
            Areas,
            Penalty,
            Tags,
            Height,

        }

        public enum ConnectionsType : byte {

            All = 0,
            NoDirectional,
            DirectionalOnly,
            DirectionalIfHasDirect,

        }

        public GridNodeData[] nodesData;

        public Vector3Int size = new Vector3Int(100, 1, 100);
        public float nodeSize = 1f;
        public float maxSlope = 45f;
        public bool useSlopePhysics;

        public float initialPenalty = 100f;
        public float initialHeight = 0f;
        public float diagonalCostFactor = 1.41421f;
        public ConnectionsType connectionsType = ConnectionsType.All;

        public float agentHeight;
        public int erosion;

        public LayerMask checkMask;
        public LayerMask collisionMask;
        public float collisionCheckRadius;

        public DrawMode drawMode;
        public bool drawNonwalkableNodes;
        public bool drawConnections;
        public bool drawConnectionsToUnwalkable;

        private struct CopyNode : IArrayElementCopy<Node> {

            public void Copy(in Node from, ref Node to) {

                if (to == null) to = PoolClass<GridNode>.Spawn();
                to.CopyFrom(from);

            }

            public void Recycle(ref Node item) {

                var g = (GridNode)item;
                PoolClass<GridNode>.Recycle(ref g);

            }

        }

        public override void BuildAreas() {
            
            base.BuildAreas();

            this.BuildAllErosion();

            this.nodesData = new GridNodeData[this.nodes.Count];
            for (int i = 0; i < this.nodesData.Length; ++i) {

                this.nodesData[i] = this.GetNodeByIndex<GridNode>(i).GetData();

            }

        }

        public void BuildAllErosion() {

            this.BuildErosion(this.nodes);

        }
        
        public void BuildErosion(Bounds bounds) {

            var nodes = PoolListCopyable<Node>.Spawn(this.nodes.Count);
            this.GetNodesInBounds(nodes, bounds, Constraint.Empty);

            this.BuildErosion(nodes);
            
            PoolListCopyable<Node>.Recycle(ref nodes);

        }

        public void BuildErosion(ListCopyable<Node> nodes) {

            var list = PoolList<GridNode>.Spawn(nodes.Count);
            for (int j = 0; j <= this.erosion; ++j) {

                list.Clear();
                for (int i = 0; i < nodes.Count; ++i) {

                    var node = (GridNode)nodes[i];
                    if (node.walkable == true && node.erosion == 0) this.TestErosion(list, node, j);

                }

                foreach (var node in list) {

                    node.erosion = j + 1;

                }

            }
            PoolList<GridNode>.Recycle(ref list);

        }

        public void BuildErosion(Unity.Collections.NativeArray<int> customWalkableField, ref Unity.Collections.NativeArray<int> resultErosionField) {
            
            var list = PoolList<int>.Spawn(resultErosionField.Length);
            for (int j = 0; j <= this.erosion; ++j) {

                list.Clear();
                for (int i = 0; i < resultErosionField.Length; ++i) {

                    var node = this.GetNodeByIndex<GridNode>(i);
                    if (customWalkableField[i] == 0 && resultErosionField[i] == 0) this.TestErosion(customWalkableField, resultErosionField, list, node, j);

                }

                foreach (var nodeIndex in list) {

                    resultErosionField[nodeIndex] = j + 1;

                }

            }
            PoolList<int>.Recycle(ref list);
            
        }

        [Unity.Burst.BurstCompileAttribute(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true)]
        private struct BuildErosionJobData : Unity.Jobs.IJob {

            public int erosion;
            public Vector3Int graphSize;
            public Unity.Collections.NativeArray<GridNodeData> nodes;
            public Unity.Collections.NativeArray<int> customWalkableField;
            public Unity.Collections.NativeArray<int> resultErosionField;
            public Unity.Collections.NativeList<int> list;

            public void Execute() {

                for (int j = 0; j <= this.erosion; ++j) {

                    this.list.Clear();
                    for (int i = 0; i < this.resultErosionField.Length; ++i) {

                        var node = this.nodes[i];
                        if (this.customWalkableField[i] == 0 && this.resultErosionField[i] == 0) {
                            
                            this.TestErosion(ref this.customWalkableField, ref this.resultErosionField, ref this.list, ref node, j);
                            
                        }

                    }

                    for (var i = 0; i < this.list.Length; ++i) {
                    
                        var nodeIndex = this.list[i];
                        this.resultErosionField[nodeIndex] = j + 1;
                        
                    }

                }

            }

            private void TestErosion(ref Unity.Collections.NativeArray<int> customWalkableField, ref Unity.Collections.NativeArray<int> resultErosionField, ref Unity.Collections.NativeList<int> list, ref GridNodeData node, int erodeIteration) {

                if (GridGraphUtilities.HasErodeConnectionFail(this.graphSize, node, this.nodes, customWalkableField, resultErosionField, erodeIteration) == false) return;

                list.Add(node.index);

            }

        }

        public void BuildErosionJob(Unity.Collections.NativeArray<GridNodeData> nodes, Unity.Collections.NativeArray<int> customWalkableField, ref Unity.Collections.NativeArray<int> resultErosionField) {

            var list = new Unity.Collections.NativeList<int>(Unity.Collections.Allocator.TempJob);
            var job = new BuildErosionJobData() {
                erosion = this.erosion,
                graphSize = this.size,
                nodes = nodes,
                customWalkableField = customWalkableField,
                resultErosionField = resultErosionField,
                list = list,
            };
            job.Schedule().Complete();
            resultErosionField = job.resultErosionField;
            list.Dispose();
            
        }

        private void TestErosion(Unity.Collections.NativeArray<int> customWalkableField, Unity.Collections.NativeArray<int> resultErosionField, System.Collections.Generic.List<int> list, GridNode node, int erodeIteration) {

            if (GridGraphUtilities.HasErodeConnectionFail(this, node, customWalkableField, resultErosionField, erodeIteration) == false) return;

            list.Add(node.index);

        }

        private void TestErosion(System.Collections.Generic.List<GridNode> list, GridNode node, int erodeIteration) {

            if (GridGraphUtilities.HasErodeConnectionFail(this, node, erodeIteration) == false) return;

            list.Add(node);

        }

        protected override void OnRecycle() {

            base.OnRecycle();
            
            this.size = default;
            this.nodeSize = default;
            this.initialPenalty = default;
            this.diagonalCostFactor = default;
            this.connectionsType = default;
            this.agentHeight = default;
            this.erosion = default;
            this.checkMask = default;
            this.collisionMask = default;
            this.collisionCheckRadius = default;

            ArrayUtils.Recycle(ref this.nodes, new CopyNode());

        }

        public override void OnCopyFrom(Graph other) {

            var gg = (GridGraph)other;
            this.size = gg.size;
            this.nodeSize = gg.nodeSize;
            this.initialPenalty = gg.initialPenalty;
            this.initialHeight = gg.initialHeight;
            this.diagonalCostFactor = gg.diagonalCostFactor;
            this.connectionsType = gg.connectionsType;
            this.agentHeight = gg.agentHeight;
            this.erosion = gg.erosion;
            this.checkMask = gg.checkMask;
            this.collisionMask = gg.collisionMask;
            this.collisionCheckRadius = gg.collisionCheckRadius;

            ArrayUtils.Copy(other.nodes, ref this.nodes, new CopyNode());

        }

        public override float GetNodeMinDistance() {

            return this.nodeSize / 10f;

        }

        public override void RemoveNode(ref Node node, bool bruteForceConnections = false) {

            base.RemoveNode(ref node, bruteForceConnections);

            var g = (GridNode)node;
            PoolClass<GridNode>.Recycle(ref g);
            node = null;

        }

        public bool HasConnectionByDirection(int sourceIndex, Direction direction, bool walkableOnly = true) {

            var node = this.GetNodeByIndex<GridNode>(sourceIndex);
            var conn = node.connections[(int)direction];
            var idx = conn.index;
            if (idx >= 0) {

                if (walkableOnly == true) {

                    return this.nodes[idx].walkable == true;

                }

                return true;

            }

            return false;

        }

        public void ResetConnections(int sourceIndex) {

            var connection = Node.Connection.NoConnection;
            var node = this.GetNodeByIndex<GridNode>(sourceIndex);
            for (int i = 0; i < node.connections.Length; ++i) {

                node.connections[i] = connection;

            }

        }

        public void SetupConnectionByDirection(int sourceIndex, Direction direction) {

            var connection = Node.Connection.NoConnection;
            var node = this.GetNodeByIndex<GridNode>(sourceIndex);
            var target = GridGraphUtilities.GetIndexByDirection(this, sourceIndex, direction);
            if (target >= 0) {

                var targetNode = this.GetNodeByIndex<GridNode>(target);
                if (direction != Direction.Up &&
                    direction != Direction.Down) {
                    
                    var force = false;
                    var maxSlope = this.maxSlope;
                    var angle = VecMath.Angle(targetNode.worldPosition - node.worldPosition, new float3(targetNode.worldPosition.x, node.worldPosition.y, targetNode.worldPosition.z) - node.worldPosition);
                    if (this.useSlopePhysics == true) {

                        var f1 = false;
                        var f2 = false;
                        float3 p1 = float3.zero;
                        float3 p2 = float3.zero;
                        
                        var orig = math.lerp(node.worldPosition, targetNode.worldPosition, 0.25f) + (float3)Vector3.up * (this.agentHeight * 0.5f);
                        if (Physics.Raycast(new Ray((Vector3)orig, Vector3.down), out var hit, this.agentHeight, this.checkMask) == true) {

                            p1 = (float3)hit.point;
                            f1 = true;

                        }

                        orig = math.lerp(node.worldPosition, targetNode.worldPosition, 0.75f) + (float3)Vector3.up * (this.agentHeight * 0.5f);
                        if (Physics.Raycast(new Ray((Vector3)orig, Vector3.down), out hit, this.agentHeight, this.checkMask) == true) {

                            p2 = (float3)hit.point;
                            f2 = true;

                        }

                        if (f1 == true && f2 == true) {
                            
                            //UnityEngine.Debug.DrawLine(p1, p1 + Vector3.up, Color.cyan, 5f);
                            //UnityEngine.Debug.DrawLine(p2, p2 + Vector3.up, Color.red, 5f);
                            angle = VecMath.Angle(p2 - p1, new float3(p2.x, p1.y, p2.z) - p1);
                            if (angle <= maxSlope) force = true;
                            
                        }

                    }

                    if (force == true || angle <= maxSlope) {

                        var cost = math.distancesq(node.worldPosition, targetNode.worldPosition);
                        connection.cost = (cost + targetNode.penalty) * (GridGraphUtilities.IsDiagonalDirection(direction) == true ? this.diagonalCostFactor : 1f);
                        connection.index = target;

                    }

                } else {

                    if (math.distancesq(node.worldPosition, targetNode.worldPosition) <= this.agentHeight * 0.1f) {

                        connection.cost = 0f;
                        connection.index = target;

                    }

                }

            }

            node.connections[(int)direction] = connection;

        }

        public override bool ClampPosition(float3 worldPosition, Constraint constraint, out float3 position) {

            var node = this.GetNearest(worldPosition, constraint);
            if (node.node != null) {
                
                position = node.node.worldPosition;
                return true;
                
            }

            position = default;
            return false;

        }

        public override NodeInfo GetNearest(float3 worldPosition, Constraint constraint) {

            if (this.nodes == null) return default;

            var clamped = new float3(
                math.clamp(worldPosition.x - this.graphCenter.x, -this.nodeSize * this.size.x * 0.5f + this.nodeSize * 0.5f,
                            this.nodeSize * this.size.x * 0.5f - this.nodeSize * 0.5f),
                math.clamp(worldPosition.y - this.graphCenter.y, -this.agentHeight * this.size.y * 0.5f, this.agentHeight * this.size.y * 0.5f),
                math.clamp(worldPosition.z - this.graphCenter.z, -this.nodeSize * this.size.z * 0.5f + this.nodeSize * 0.5f,
                            this.nodeSize * this.size.z * 0.5f - this.nodeSize * 0.5f));

            var x = (int)((clamped.x + this.nodeSize * this.size.x * 0.5f) / this.nodeSize);
            var y = (int)((clamped.y + this.agentHeight * this.size.y * 0.5f) / this.agentHeight);
            var z = (int)((clamped.z + this.nodeSize * this.size.z * 0.5f) / this.nodeSize);

            y = Mathf.Clamp(y, -this.size.y + 1, this.size.y - 1);

            var maxIterations = this.nodes.Count;
            for (int idx = 0, cnt = maxIterations; idx < cnt; ++idx) {

                var p = ME.ECS.MathUtils.GetSpiralPointByIndex(new Vector2Int(x, z), idx);
                var i = GridGraphUtilities.GetIndexByPosition(this, new Vector3Int(p.x, y, p.y));
                var node = this.GetNodeByIndex<Node>(i);
                if (node == null) continue;
                if (node.IsSuitable(constraint) == true) {
                    
                    return new NodeInfo() {
                        graph = this,
                        node = node,
                        worldPosition = node.worldPosition,
                    };
                    
                }

            }

            return new NodeInfo() {
                graph = this,
                node = null,
                worldPosition = worldPosition,
            };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void GetNodesInBounds(ListCopyable<Node> result, Bounds bounds, Constraint constraint) {

            var min = (float3)bounds.min;
            var max = (float3)bounds.max;

            var minNode = this.GetNearest(min + (float3)this.graphCenter, Constraint.Empty);
            if (minNode.node == null) return;
            var maxNode = this.GetNearest(max + (float3)this.graphCenter, Constraint.Empty);
            if (maxNode.node == null) return;

            var ggNodeMin = (GridNode)minNode.node;
            var ggNodeMax = (GridNode)maxNode.node;
            for (int y = ggNodeMin.position.y; y <= ggNodeMax.position.y; ++y) {

                for (int x = ggNodeMin.position.x; x <= ggNodeMax.position.x; ++x) {

                    for (int z = ggNodeMin.position.z; z <= ggNodeMax.position.z; ++z) {

                        var index = GridGraphUtilities.GetIndexByPosition(this, new Vector3Int(z, y, x));
                        var n = this.nodes[index];
                        //Debug.DrawLine(n.worldPosition + Vector3.up * 5f, n.worldPosition + Vector3.up * 10f, Color.red, 5f);
                        if (bounds.Contains((Vector3)n.worldPosition) == true && n.IsSuitable(constraint) == true) {

                            //Debug.DrawLine(n.worldPosition + Vector3.up * 10f, n.worldPosition + Vector3.up * 15f, Color.magenta, 5f);
                            result.Add(n);

                        }

                    }

                }

            }

        }

        protected override void DrawGizmos() {

            if (this.nodes == null) return;

            var center = this.graphCenter;
            const float drawOffset = 0.01f;

            var borderColor = new Color(1f, 1f, 1f, 1f);
            Gizmos.color = borderColor;
            Gizmos.DrawWireCube((Vector3)center, new Vector3(this.size.x * this.nodeSize, this.size.y * this.agentHeight, this.size.z * this.nodeSize));

            if (this.drawMode != DrawMode.None) {

                var nodeBorderColor = new Color(0.2f, 0.5f, 1f, 0.05f);
                var nodeColor = new Color(0.2f, 0.5f, 1f, 0.05f);
                var nodeBorderColorWalkableWorld = new Color(0.2f, 0.5f, 1f, 0.6f);
                var nodeColorWalkableWorld = new Color(0.2f, 0.5f, 1f, 0.6f);
                var nodeBorderColorUnwalkable = new Color(1f, 0.2f, 0.2f, 0.4f);
                var nodeColorUnwalkable = new Color(1f, 0.2f, 0.2f, 0.4f);
                for (int j = 0; j < this.nodes.innerArray.Length; ++j) {

                    var node = (GridNode)this.nodes.innerArray[j];
                    if (node == null) continue;
                    //var x = node.position.z;
                    //var y = node.position.y;
                    //var z = node.position.x;
                    //var nodePosition = new Vector3(x * this.nodeSize + this.nodeSize * 0.5f, y * this.agentHeight + this.agentHeight * 0.5f, z * this.nodeSize + this.nodeSize * 0.5f);
                    var worldPos = node.worldPosition; //center + nodePosition;
                    worldPos.y += drawOffset;
                    
                    if (this.drawMode == DrawMode.Solid) { } else if (this.drawMode == DrawMode.Areas) {

                        nodeColor = this.GetAreaColor(node.area);
                        nodeBorderColor = nodeColor;
                        nodeBorderColor.a = 0.06f;

                        nodeColorWalkableWorld = nodeColor;
                        nodeColorWalkableWorld.a = 0.6f;
                        nodeBorderColorWalkableWorld = nodeBorderColor;

                    } else if (this.drawMode == DrawMode.Penalty) {

                        nodeColor = this.GetPenaltyColor(node.penalty);
                        nodeBorderColor = nodeColor;
                        nodeBorderColor.a = 0.06f;

                        nodeColorWalkableWorld = nodeColor;
                        nodeColorWalkableWorld.a = 0.6f;
                        nodeBorderColorWalkableWorld = nodeBorderColor;

                    } else if (this.drawMode == DrawMode.Tags) {

                        nodeColor = this.GetAreaColor(node.tag);
                        nodeBorderColor = nodeColor;
                        nodeBorderColor.a = 0.06f;

                        nodeColorWalkableWorld = nodeColor;
                        nodeColorWalkableWorld.a = 0.6f;
                        nodeBorderColorWalkableWorld = nodeBorderColor;

                    } else if (this.drawMode == DrawMode.Height) {

                        nodeColor = this.GetHeightColor(node.height);
                        nodeBorderColor = nodeColor;
                        nodeBorderColor.a = 0.06f;

                        nodeColorWalkableWorld = nodeColor;
                        nodeColorWalkableWorld.a = 0.6f;
                        nodeBorderColorWalkableWorld = nodeBorderColor;

                    }

                    if (node.walkable == true || this.drawNonwalkableNodes == true) {

                        Gizmos.color = (node.walkable == true ? nodeColor : nodeColorUnwalkable);
                        Gizmos.DrawCube((Vector3)worldPos, new Vector3(this.nodeSize, this.agentHeight, this.nodeSize));

                        Gizmos.color = (node.walkable == true ? nodeBorderColor : nodeBorderColorUnwalkable);
                        Gizmos.DrawWireCube((Vector3)worldPos, new Vector3(this.nodeSize, this.agentHeight, this.nodeSize));

                    }

                    if (node.walkable == true) {

                        Gizmos.color = nodeColorWalkableWorld;
                        Gizmos.DrawCube((Vector3)worldPos, new Vector3(0.9f, 0f, 0.9f) * this.nodeSize);

                        Gizmos.color = nodeBorderColorWalkableWorld;
                        Gizmos.DrawWireCube((Vector3)worldPos, new Vector3(0.9f, 0f, 0.9f) * this.nodeSize);

                    }

                    if (this.drawConnections == true) { // Draw connections

                        if (node.walkable == true) {

                            Gizmos.color = new Color(1f, 0.9215686f, 0.01568628f, 0.9f);
                            for (int k = 0; k < node.connections.Length; ++k) {

                                var conn = node.connections[k];
                                var n = this.GetNodeByIndex<GridNode>(conn.index);
                                if (n != null && (this.drawConnectionsToUnwalkable == true || n.walkable == true)) {

                                    Gizmos.DrawRay((Vector3)node.worldPosition + Vector3.up * 0.1f, (Vector3)(n.worldPosition - node.worldPosition) * 0.5f + Vector3.up * 0.1f);
                                    Gizmos.DrawRay((Vector3)node.worldPosition, (Vector3)(n.worldPosition - node.worldPosition) * 0.5f);

                                }

                            }

                        }

                        var connections = node.GetCustomConnections();
                        if (connections != null) {

                            Gizmos.color = new Color(0.9215686f, 0.01568628f, 1f, 0.9f);
                            for (int k = 0; k < connections.Count; ++k) {
                                
                                var conn = connections[k];
                                var n = this.GetNodeByIndex<GridNode>(conn.index);
                                if (n != null) {
                                
                                    Gizmos.DrawLine((Vector3)n.worldPosition, (Vector3)node.worldPosition);
                                    
                                }

                            }
                            
                        }

                    }
                    
                    //UnityEditor.Handles.Label(node.worldPosition + Vector3.up, node.erosion.ToString());

                }

            }

        }

        protected override void Validate() {

            if (this.size.x <= 0) this.size.x = 1;
            if (this.size.y <= 0) this.size.y = 1;
            if (this.size.z <= 0) this.size.z = 1;

        }

        protected override void BuildConnections() {

            var noConnection = Node.Connection.NoConnection;
            for (var i = 0; i < this.nodes.Count; ++i) {

                //if (i != this.size.x && i != 0 && i != this.nodes.Length - 1 && i != this.size.x - 1 && i != (50 + this.size.x * this.size.z) && i != (150 + this.size.x * this.size.z * 2) &&
                //    i != this.size.z * this.size.x) continue;

                var node = this.nodes[i];
                var connections = node.GetConnections();
                this.ResetConnections(node.index);

                if (this.connectionsType != ConnectionsType.DirectionalOnly) {

                    this.SetupConnectionByDirection(node.index, Direction.Up);
                    this.SetupConnectionByDirection(node.index, Direction.Down);
                    this.SetupConnectionByDirection(node.index, Direction.Forward);
                    this.SetupConnectionByDirection(node.index, Direction.Right);
                    this.SetupConnectionByDirection(node.index, Direction.Backward);
                    this.SetupConnectionByDirection(node.index, Direction.Left);

                } else {

                    connections[0] = connections[1] = connections[2] = connections[3] = connections[4] = connections[5] = noConnection;

                }

                if (this.connectionsType == ConnectionsType.All || this.connectionsType == ConnectionsType.DirectionalOnly) {

                    this.SetupConnectionByDirection(node.index, Direction.RightForward);
                    this.SetupConnectionByDirection(node.index, Direction.RightBackward);
                    this.SetupConnectionByDirection(node.index, Direction.LeftBackward);
                    this.SetupConnectionByDirection(node.index, Direction.LeftForward);

                    this.SetupConnectionByDirection(node.index, Direction.RightUpForward);
                    this.SetupConnectionByDirection(node.index, Direction.RightUpBackward);
                    this.SetupConnectionByDirection(node.index, Direction.LeftUpBackward);
                    this.SetupConnectionByDirection(node.index, Direction.LeftUpForward);

                    this.SetupConnectionByDirection(node.index, Direction.RightDownForward);
                    this.SetupConnectionByDirection(node.index, Direction.RightDownBackward);
                    this.SetupConnectionByDirection(node.index, Direction.LeftDownBackward);
                    this.SetupConnectionByDirection(node.index, Direction.LeftDownForward);

                } else if (this.connectionsType == ConnectionsType.DirectionalIfHasDirect) {

                    if (this.HasConnectionByDirection(node.index, Direction.Forward) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Right) == true) {

                        this.SetupConnectionByDirection(node.index, Direction.RightForward);

                    }

                    if (this.HasConnectionByDirection(node.index, Direction.Backward) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Right) == true) {

                        this.SetupConnectionByDirection(node.index, Direction.RightBackward);

                    }

                    if (this.HasConnectionByDirection(node.index, Direction.Forward) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Left) == true) {

                        this.SetupConnectionByDirection(node.index, Direction.LeftForward);

                    }

                    if (this.HasConnectionByDirection(node.index, Direction.Backward) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Left) == true) {

                        this.SetupConnectionByDirection(node.index, Direction.LeftBackward);

                    }

                    if (this.HasConnectionByDirection(node.index, Direction.Backward) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Right) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Down) == true) {

                        this.SetupConnectionByDirection(node.index, Direction.RightDownBackward);

                    }

                    if (this.HasConnectionByDirection(node.index, Direction.Backward) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Left) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Down) == true) {

                        this.SetupConnectionByDirection(node.index, Direction.LeftDownBackward);

                    }

                    if (this.HasConnectionByDirection(node.index, Direction.Forward) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Right) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Down) == true) {

                        this.SetupConnectionByDirection(node.index, Direction.RightDownForward);

                    }

                    if (this.HasConnectionByDirection(node.index, Direction.Forward) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Left) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Down) == true) {

                        this.SetupConnectionByDirection(node.index, Direction.LeftDownForward);

                    }

                    if (this.HasConnectionByDirection(node.index, Direction.Backward) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Right) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Up) == true) {

                        this.SetupConnectionByDirection(node.index, Direction.RightUpBackward);

                    }

                    if (this.HasConnectionByDirection(node.index, Direction.Backward) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Left) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Up) == true) {

                        this.SetupConnectionByDirection(node.index, Direction.LeftUpBackward);

                    }

                    if (this.HasConnectionByDirection(node.index, Direction.Forward) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Right) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Up) == true) {

                        this.SetupConnectionByDirection(node.index, Direction.RightUpForward);

                    }

                    if (this.HasConnectionByDirection(node.index, Direction.Forward) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Left) == true ||
                        this.HasConnectionByDirection(node.index, Direction.Up) == true) {

                        this.SetupConnectionByDirection(node.index, Direction.LeftUpForward);

                    }

                }
                
            }

        }

        private bool BuildNodePhysics(Node node) {

            var worldPos = node.worldPosition;

            if (this.checkMask == 0) {

                node.worldPosition = worldPos;
                node.walkable = true;
                return true;

            }

            #if WORLD_TICK_THREADED
            // Quit if threaded logic is active
            node.worldPosition = worldPos;
            node.walkable = true;
            return false;
            #else

            const float offset = 0.01f;
            var minSize = Mathf.Min(0.01f, this.collisionCheckRadius);
            if (Physics.CheckBox((Vector3)worldPos,
                                 new Vector3(minSize, this.agentHeight - offset * 2f, minSize),
                                 Quaternion.identity,
                                 this.collisionMask) == true) {

                node.walkable = false;
                return false;

            }

            var raycastResult = false;
            RaycastHit hit;
            if (this.collisionCheckRadius <= 0f) {

                raycastResult = Physics.Raycast((Vector3)worldPos + Vector3.up * (this.agentHeight * 0.5f - offset), Vector3.down, out hit, this.agentHeight, this.checkMask);

            } else {

                if (Physics.CheckBox((Vector3)worldPos + Vector3.up * (this.agentHeight - this.collisionCheckRadius),
                                     new Vector3(this.collisionCheckRadius, this.agentHeight - this.collisionCheckRadius * 2f, this.collisionCheckRadius),
                                     Quaternion.identity,
                                     this.collisionMask) == true) {

                    node.walkable = false;
                    return false;

                }
                
                raycastResult = Physics.BoxCast((Vector3)worldPos + Vector3.up * (this.agentHeight * 0.5f - offset),
                                                new Vector3(this.collisionCheckRadius, this.agentHeight * 0.5f, this.collisionCheckRadius),
                                                Vector3.down,
                                                out hit,
                                                Quaternion.identity,
                                                this.agentHeight,
                                                this.checkMask);
                
            }

            if (raycastResult == true) {

                node.worldPosition.y = hit.point.y;

                if ((this.collisionMask & (1 << hit.collider.gameObject.layer)) != 0) {

                    node.walkable = false;

                } else {

                    return true;

                }

            } else {

                node.walkable = false;

            }
            #endif

            return false;

        }

        protected override void BuildNodes() {

            this.nodes = PoolListCopyable<Node>.Spawn(this.size.x * this.size.y * this.size.z);

            var center = (float3)this.graphCenter - new float3(this.size.x * this.nodeSize * 0.5f, this.size.y * this.agentHeight * 0.5f, this.size.z * this.nodeSize * 0.5f);

            var i = 0;
            for (int y = 0; y < this.size.y; ++y) {

                for (int x = 0; x < this.size.x; ++x) {

                    for (int z = 0; z < this.size.z; ++z) {

                        var nodePosition = new float3(x * this.nodeSize + this.nodeSize * 0.5f, y * this.agentHeight + this.agentHeight * 0.5f,
                                                      z * this.nodeSize + this.nodeSize * 0.5f);
                        var worldPos = center + nodePosition;

                        var node = PoolClass<GridNode>.Spawn();
                        node.graph = this;
                        node.index = i;
                        node.position = new Vector3Int(z, y, x);
                        node.walkable = true;
                        node.worldPosition = worldPos;
                        node.penalty = this.initialPenalty;
                        node.height = this.initialHeight;
                        this.nodes.Add(node);

                        this.BuildNodePhysics(node);

                        ++i;

                    }

                }

            }

        }

    }

    public static class GridGraphUtilities {

        public static bool HasErodeConnectionFail(Vector3Int graphSize, GridNodeData node, Unity.Collections.NativeArray<GridNodeData> nodes, Unity.Collections.NativeArray<int> customWalkableField, Unity.Collections.NativeArray<int> resultErosionField, int erodeIteration) {

            if (GridGraphUtilities.IsBorder(graphSize, node) == true) return true;

            for (int i = 0; i < node.connections.Length; ++i) {

                var connection = node.connections.Get(i);
                if (connection.index == -1) continue;
                
                var neighbour = nodes[connection.index];
                if (customWalkableField[neighbour.index] != 0 || neighbour.walkable == 0) return true;
                if (resultErosionField[neighbour.index] > 0) return true;
                
            }
            
            return false;
            
        }

        public static bool HasErodeConnectionFail(GridGraph graph, GridNode node, Unity.Collections.NativeArray<int> customWalkableField, Unity.Collections.NativeArray<int> resultErosionField, int erodeIteration) {

            if (GridGraphUtilities.IsBorder(graph.size, node) == true) return true;

            foreach (var connection in node.connections) {

                if (connection.index == -1) continue;
                
                var neighbour = graph.GetNodeByIndex<GridNode>(connection.index);
                if (customWalkableField[neighbour.index] != 0 || neighbour.walkable == false) return true;
                if (resultErosionField[neighbour.index] > 0) return true;
                
            }
            
            return false;
            
        }

        public static bool HasErodeConnectionFail(GridGraph graph, GridNode node, int erodeIteration) {

            if (GridGraphUtilities.IsBorder(graph.size, node) == true) return true;

            foreach (var connection in node.connections) {

                if (connection.index == -1) continue;
                
                var neighbour = graph.GetNodeByIndex<GridNode>(connection.index);
                if (neighbour.walkable == false) return true;
                if (neighbour.erosion > 0) return true;
                
            }
            
            return false;
            
        }

        public static bool IsBorder(Vector3Int size, GridNodeData node) {
            
            if (node.position.x == 0 ||
                node.position.x == size.z - 1) return true;

            if (size.y > 2) {

                if (node.position.y == 0 ||
                    node.position.y == size.y - 1)
                    return true;

            }

            if (node.position.z == 0 ||
                node.position.z == size.x - 1) return true;

            return false;
            
        }

        public static bool IsBorder(Vector3Int size, GridNode node) {
            
            if (node.position.x == 0 ||
                node.position.x == size.z - 1) return true;

            if (size.y > 2) {

                if (node.position.y == 0 ||
                    node.position.y == size.y - 1)
                    return true;

            }

            if (node.position.z == 0 ||
                node.position.z == size.x - 1) return true;

            return false;
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void DirUp(Vector3Int size, GridNode node, ref int index) {

            if (index == -1) return;

            index += size.x * size.z;
            if (node.position.y >= size.y - 1) index = -1;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void DirDown(Vector3Int size, GridNode node, ref int index) {

            if (index == -1) return;

            index -= size.x * size.z;
            if (node.position.y <= 0) index = -1;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void DirRight(Vector3Int size, GridNode node, ref int index) {

            if (index == -1) return;

            index -= 1;
            if (node.position.x <= 0) index = -1;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void DirLeft(Vector3Int size, GridNode node, ref int index) {

            if (index == -1) return;

            index += 1;
            if (node.position.x >= size.z - 1) index = -1;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void DirForward(Vector3Int size, GridNode node, ref int index) {

            if (index == -1) return;

            index += size.z;
            if (node.position.z >= size.x - 1) index = -1;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void DirBackward(Vector3Int size, GridNode node, ref int index) {

            if (index == -1) return;

            index -= size.z;
            if (node.position.z <= 0) index = -1;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool IsDiagonalDirection(GridGraph.Direction direction) {

            return (int)direction >= 6;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static GridGraph.Direction GetDirection(GridNode from, GridNode to) {

            var toPos = to.position;
            var fromPos = from.position;
            if (toPos.x < fromPos.x && toPos.y == fromPos.y) return GridGraph.Direction.Left;
            if (toPos.x > fromPos.x && toPos.y == fromPos.y) return GridGraph.Direction.Right;
            if (toPos.x == fromPos.x && toPos.y < fromPos.y) return GridGraph.Direction.Backward;
            if (toPos.x == fromPos.x && toPos.y > fromPos.y) return GridGraph.Direction.Forward;
            if (toPos.x < fromPos.x && toPos.y > fromPos.y) return GridGraph.Direction.LeftForward;
            if (toPos.x > fromPos.x && toPos.y > fromPos.y) return GridGraph.Direction.RightForward;
            if (toPos.x < fromPos.x && toPos.y < fromPos.y) return GridGraph.Direction.LeftBackward;
            if (toPos.x > fromPos.x && toPos.y < fromPos.y) return GridGraph.Direction.RightBackward;
            
            return (GridGraph.Direction)(-1);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static GridGraph.Direction GetDirection(float3 dir) {

            var direction = (Vector3)dir;
            direction = direction.normalized.XZ().XZ();
            var x = direction.z;
            direction.z = direction.x;
            direction.x = x;
            if (direction == Vector3.left) return GridGraph.Direction.Right;
            if (direction == Vector3.right) return GridGraph.Direction.Left;
            if (direction == Vector3.forward) return GridGraph.Direction.Forward;
            if (direction == Vector3.back) return GridGraph.Direction.Backward;

            return (GridGraph.Direction)(-1);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Vector3 GetDirection(GridGraph.Direction direction) {

            var dir = Vector3.zero;
            switch (direction) {
                
                case GridGraph.Direction.Backward:          dir = new Vector3(0f, 0f, -1f);
                    break;
                case GridGraph.Direction.Forward:           dir = new Vector3(0f, 0f, 1f);
                    break;
                case GridGraph.Direction.Left:              dir = new Vector3(-1f, 0f, 0f);
                    break;
                case GridGraph.Direction.Right:             dir = new Vector3(1f, 0f, 0f);
                    break;
                case GridGraph.Direction.LeftForward:       dir = new Vector3(-1f, 0f, 1f);
                    break;
                case GridGraph.Direction.RightForward:      dir = new Vector3(1f, 0f, 1f);
                    break;
                case GridGraph.Direction.LeftBackward:      dir = new Vector3(-1f, 0f, -1f);
                    break;
                case GridGraph.Direction.RightBackward:     dir = new Vector3(1f, 0f, -1f);
                    break;
                case GridGraph.Direction.Up:                dir = new Vector3(0f, 1f, 0f);
                    break;
                case GridGraph.Direction.Down:              dir = new Vector3(0f, -1f, 0f);
                    break;
                case GridGraph.Direction.LeftUpBackward:    dir = new Vector3(-1f, 1f, -1f);
                    break;
                case GridGraph.Direction.LeftDownBackward:  dir = new Vector3(-1f, -1f, -1f);
                    break;
                case GridGraph.Direction.RightUpBackward:   dir = new Vector3(1f, 1f, -1f);
                    break;
                case GridGraph.Direction.RightDownBackward: dir = new Vector3(1f, -1f, -1f);
                    break;
                case GridGraph.Direction.LeftUpForward:     dir = new Vector3(-1f, 1f, 1f);
                    break;
                case GridGraph.Direction.LeftDownForward:   dir = new Vector3(-1f, -1f, 1f);
                    break;
                case GridGraph.Direction.RightUpForward:    dir = new Vector3(1f, 1f, 1f);
                    break;
                case GridGraph.Direction.RightDownForward:  dir = new Vector3(1f, -1f, 1f);
                    break;
                
            }

            return new Vector3(dir.z, dir.y, -dir.x);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int GetIndexByDirection(GridGraph graph, int sourceIndex, GridGraph.Direction direction) {

            var node = graph.GetNodeByIndex<GridNode>(sourceIndex);

            switch (direction) {

                case GridGraph.Direction.LeftDownForward:
                    GridGraphUtilities.DirLeft(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirDown(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirForward(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.RightDownForward:
                    GridGraphUtilities.DirRight(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirDown(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirForward(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.LeftDownBackward:
                    GridGraphUtilities.DirLeft(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirDown(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirBackward(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.RightDownBackward:
                    GridGraphUtilities.DirRight(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirDown(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirBackward(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.LeftUpForward:
                    GridGraphUtilities.DirLeft(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirUp(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirForward(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.RightUpForward:
                    GridGraphUtilities.DirRight(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirUp(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirForward(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.LeftUpBackward:
                    GridGraphUtilities.DirLeft(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirUp(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirBackward(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.RightUpBackward:
                    GridGraphUtilities.DirRight(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirUp(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirBackward(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.LeftForward:
                    GridGraphUtilities.DirLeft(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirForward(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.RightForward:
                    GridGraphUtilities.DirRight(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirForward(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.LeftBackward:
                    GridGraphUtilities.DirLeft(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirBackward(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.RightBackward:
                    GridGraphUtilities.DirRight(graph.size, node, ref sourceIndex);
                    GridGraphUtilities.DirBackward(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.Left:
                    GridGraphUtilities.DirLeft(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.Right:
                    GridGraphUtilities.DirRight(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.Forward:
                    GridGraphUtilities.DirForward(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.Backward:
                    GridGraphUtilities.DirBackward(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.Up:
                    GridGraphUtilities.DirUp(graph.size, node, ref sourceIndex);
                    break;

                case GridGraph.Direction.Down:
                    GridGraphUtilities.DirDown(graph.size, node, ref sourceIndex);
                    break;

            }

            return sourceIndex;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int GetIndexByPosition(GridGraph graph, Vector3Int position) {

            var x = position.x;
            var y = position.y;
            var z = position.z;
            x = Mathf.Clamp(x, 0, graph.size.x);
            y = Mathf.Clamp(y, 0, graph.size.y);
            z = Mathf.Clamp(z, 0, graph.size.z);
            var idx = y * graph.size.x * graph.size.z + (x * graph.size.z) + z;
            return idx;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int GetIndexByPosition(Vector3Int graphSize, Vector3Int position) {

            var x = position.x;
            var y = position.y;
            var z = position.z;
            if (x < 0 || x > graphSize.x) return -1;
            if (y < 0 || y > graphSize.y) return -1;
            if (z < 0 || z > graphSize.z) return -1;
            var idx = y * graphSize.x * graphSize.z + (x * graphSize.z) + z;
            return idx;

        }

    }

    public struct GridNodeData {

        public int graphIndex;
        public int index;
        public Vector3Int position;
        public float3 worldPosition;
        public int penalty;
        public byte walkable;
        public int area;
        public int tag;
        public int erosion;
        public sfloat height;
        public ConnectionsArray connections;
        
        public bool IsSuitable(BurstConstraint constraint, Unity.Collections.NativeArray<GridNodeData> nodes, Vector3Int graphSize, float3 graphCenter) {

            if (constraint.checkWalkability == 1 && this.walkable != constraint.walkable) return false;
            if (constraint.checkArea == 1 && (constraint.areaMask & (1 << this.area)) == 0) return false;
            if (constraint.checkTags == 1 && (constraint.tagsMask & (1 << this.tag)) == 0) return false;
            if (constraint.graphMask >= 0 && (constraint.graphMask & (1 << this.graphIndex)) == 0) return false;

            if (constraint.agentSize > 0) {

                if (this.erosion == 0 || this.erosion > constraint.agentSize) return true;
                return false;

            }
            
            return true;

        }
        
    }

    public readonly struct ConnectionsArray {

        public readonly int Length;
        private readonly Node.Connection c0, c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12, c13, c14, c15, c16, c17;

        public ConnectionsArray(Node.Connection[] arr) {

            this.Length = arr.Length;
            this.c0 = arr[0];
            this.c1 = arr[1];
            this.c2 = arr[2];
            this.c3 = arr[3];
            this.c4 = arr[4];
            this.c5 = arr[5];
            this.c6 = arr[6];
            this.c7 = arr[7];
            this.c8 = arr[8];
            this.c9 = arr[9];
            this.c10 = arr[10];
            this.c11 = arr[11];
            this.c12 = arr[12];
            this.c13 = arr[13];
            this.c14 = arr[14];
            this.c15 = arr[15];
            this.c16 = arr[16];
            this.c17 = arr[17];

        }
        
        public Node.Connection Get(int index) {

            switch (index) {
                
                case 0: return this.c0;
                case 1: return this.c1;
                case 2: return this.c2;
                case 3: return this.c3;
                case 4: return this.c4;
                case 5: return this.c5;
                case 6: return this.c6;
                case 7: return this.c7;
                case 8: return this.c8;
                case 9: return this.c9;
                case 10: return this.c10;
                case 11: return this.c11;
                case 12: return this.c12;
                case 13: return this.c13;
                case 14: return this.c14;
                case 15: return this.c15;
                case 16: return this.c16;
                case 17: return this.c17;
                
            }

            return default;

        }

    }

    [System.Serializable]
    public class GridNode : Node {

        public Vector3Int position;
        public int erosion;

        public readonly Connection[] connections = new Connection[6 + 4 + 4 + 4];

        public GridNodeData GetData() {

            return new GridNodeData() {
                graphIndex = this.graph.index,
                index = this.index,
                position = this.position,
                worldPosition = this.worldPosition,
                penalty = (int)this.penalty,
                walkable = (this.walkable == true ? (byte)1 : (byte)0),
                area = this.area,
                erosion = this.erosion,
                tag = this.tag,
                height = this.height,
                connections = new ConnectionsArray(this.connections),
            };
            
        }

        public override bool IsSuitable(Constraint constraint) {

            if (base.IsSuitable(constraint) == false) return false;
            
            if (constraint.agentSize > 0) {

                if (this.erosion == 0 || this.erosion > constraint.agentSize) return true;
                return false;

            }

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override Connection[] GetConnections() {

            return this.connections;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override BufferArray<Connection> GetAllConnections() {

            var connections = PoolArray<Connection>.Spawn(this.connections.Length + (this.customConnections != null ? this.customConnections.Count : 0));
            ArrayUtils.Copy(this.connections, 0, ref connections, 0, this.connections.Length);
            if (this.customConnections != null) {
                
                ArrayUtils.Copy(this.customConnections, 0, ref connections, this.connections.Length, this.customConnections.Count);
                
            }
            return connections;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void CopyFrom(Node other) {

            base.CopyFrom(other);

            var g = (GridNode)other;
            this.position = g.position;
            for (int i = 0; i < this.connections.Length; ++i) {

                this.connections[i] = g.connections[i];

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void OnRecycle() {

            base.OnRecycle();

            this.position = default;
            System.Array.Clear(this.connections, 0, this.connections.Length);

        }

    }

}