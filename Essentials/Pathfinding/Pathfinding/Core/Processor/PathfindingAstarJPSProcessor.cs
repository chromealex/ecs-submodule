using Unity.Jobs;
using UnityEngine;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding {
    
    using ME.ECS.Collections;

    public struct PathfindingAstarJPSProcessor : IPathfindingProcessor {
        
        public Path Run<TMod>(LogLevel pathfindingLogLevel, float3 from, float3 to, Constraint constraint, Graph graph, TMod pathModifier, int threadIndex = 0, bool burstEnabled = true, bool cacheEnabled = false) where TMod : struct, IPathModifier {

            if (threadIndex < 0) threadIndex = 0;
            threadIndex = threadIndex % Pathfinding.THREADS_COUNT;

            var constraintStart = constraint;
            constraintStart.checkWalkability = true;
            constraintStart.walkable = true;
            var startNode = graph.GetNearest(from, constraintStart);
            if (startNode.node == null) return new Path();

            var constraintEnd = constraintStart;
            constraintEnd.checkArea = true;
            constraintEnd.areaMask = (1 << startNode.node.area);
            
            var endNode = graph.GetNearest(to, constraintEnd);
            if (endNode.node == null) return new Path();
            
            System.Diagnostics.Stopwatch swPath = null;
            if ((pathfindingLogLevel & LogLevel.Path) != 0) swPath = System.Diagnostics.Stopwatch.StartNew();
            var statVisited = 0;
            var nodesPath = this.AstarSearch(ref statVisited, graph as GridGraph, startNode.node, endNode.node, constraint, threadIndex);

            var statLength = 0;
            
            var path = new Path();
            path.graph = graph;
            path.result = PathCompleteState.NotCalculated;

            if (nodesPath.Length == 0) {

                path.result = PathCompleteState.NotExist;

            } else {

                statLength = nodesPath.Length;
                
                path.result = PathCompleteState.Complete;
                var list = PoolListCopyable<Node>.Spawn(nodesPath.Length);
                for (int i = 0; i < nodesPath.Length; ++i) {
                    
                    list.Add(graph.nodes[nodesPath[i].index]);
                    
                }
                path.nodes = list;
                
            }
            
            nodesPath.Dispose();

            System.Diagnostics.Stopwatch swModifier = null;
            if ((pathfindingLogLevel & LogLevel.PathMods) != 0) swModifier = System.Diagnostics.Stopwatch.StartNew();
            if (path.result == PathCompleteState.Complete) {

                path = pathModifier.Run(path, constraint);

            }
            
            if ((pathfindingLogLevel & LogLevel.Path) != 0) {
                
                Logger.Log(string.Format("Path result {0}, built in {1}ms. Path length: {2} (visited: {3})\nThread Index: {4}", path.result, (swPath.ElapsedTicks / (double)System.TimeSpan.TicksPerMillisecond).ToString("0.##"), statLength, statVisited, threadIndex));
                
            }

            if ((pathfindingLogLevel & LogLevel.PathMods) != 0) {

                Logger.Log(string.Format("Path Mods: {0}ms", swModifier.ElapsedMilliseconds));

            }

            return path;

        }

        private struct TempNodeData {

            public bool isClosed;
            public bool isOpened;
            public sfloat startToCurNodeLen;
            public int parent;

        }

        //[Unity.Burst.BurstCompile(Unity.Burst.FloatPrecision.Standard, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true)]
        private struct Job : Unity.Jobs.IJob {

            public GridGraph graph;
            public Vector3Int graphSize;
            public float3 graphCenter;
            public BurstConstraint burstConstraint;
            public Unity.Collections.NativeList<GridNodeData> resultPath;
            public Unity.Collections.NativeArray<GridNodeData> arr;
            public Unity.Collections.NativeArray<int> results;
            public PriorityQueueNative<GridNodeData> openList;
            public int startNodeIndex;
            public int endNodeIndex;

            public void Execute() {
                
                var temp = new Unity.Collections.NativeArray<TempNodeData>(this.arr.Length, Unity.Collections.Allocator.Temp);
                var tmp = temp[this.startNodeIndex];
                tmp.startToCurNodeLen = 0f;
                tmp.isOpened = true;
                temp[this.startNodeIndex] = tmp;

                this.openList.Enqueue(0, this.arr[this.startNodeIndex]);
                
                var maxIter = 10000;
                while (this.openList.Count > 0) {

                    if (--maxIter <= 0) {

                        UnityEngine.Debug.LogError("Break");
                        break;
                        
                    }
                    
                    var node = this.openList.Dequeue();
                    var nodeTemp = temp[node.index];
                    nodeTemp.isClosed = true;
                    temp[node.index] = nodeTemp;
                    
                    if (node.index == this.endNodeIndex) {
                        
                        maxIter = 10000;
                        this.resultPath.Add(this.arr[this.endNodeIndex]);
                        while (temp[this.endNodeIndex].parent != 0) {
                            if (--maxIter <= 0) {
                                UnityEngine.Debug.LogError("Break");
                                break;
                            }

                            var n = this.endNodeIndex;
                            this.endNodeIndex = temp[this.endNodeIndex].parent - 1;
                            this.resultPath.Add(this.arr[n]);
                    
                        }

                        for (int i = 0; i < this.resultPath.Length / 2; ++i) {

                            var ePtr = this.resultPath[this.resultPath.Length - 1 - i];
                            this.resultPath[this.resultPath.Length - 1 - i] = this.resultPath[i];
                            this.resultPath[i] = ePtr;

                        }
                        
                        break;

                    }

                    var neighbors = node.connections;
                    var currentNodeCost = nodeTemp.startToCurNodeLen;
                    for (int i = 0; i < neighbors.Length; ++i) {

                        var conn = neighbors.Get(i);
                        if (conn.index < 0) continue;

                        if (this.Jump(out var neighbor, in temp, in node, this.arr[conn.index]) == false) continue;
                        var neighborTemp = temp[neighbor.index];
                        if (neighborTemp.isClosed == true) continue;
                        if (neighbor.IsSuitable(this.burstConstraint, this.arr, this.graphSize, this.graphCenter) == false) continue;

                        var cost = currentNodeCost + conn.cost;
                        if (neighborTemp.isOpened == false || cost < neighborTemp.startToCurNodeLen) {
                            
                            neighborTemp.startToCurNodeLen = cost;
                            neighborTemp.parent = node.index + 1;
                            if (neighborTemp.isOpened == false) {

                                this.openList.Enqueue(cost, neighbor);
                                ++this.results[0];
                                neighborTemp.isOpened = true;
                                
                            }
                            
                        }

                        temp[neighbor.index] = neighborTemp;

                    }

                }
                
                temp.Dispose();

            }

            private bool IsSuitable(int x, int y) {
                
                var idx = GridGraphUtilities.GetIndexByPosition(this.graphSize, new Vector3Int(y, 0, x));
                if (idx < 0 || idx >= this.arr.Length) return false;
                
                return this.arr[idx].IsSuitable(this.burstConstraint, this.arr, this.graphSize, this.graphCenter);
                
            }

            private int GetIndex(int x, int y) {
                
                var idx = GridGraphUtilities.GetIndexByPosition(this.graphSize, new Vector3Int(y, 0, x));
                return idx;
                
            }

            private GridNodeData GetNodeData(int x, int y) {
                
                var idx = GridGraphUtilities.GetIndexByPosition(this.graphSize, new Vector3Int(y, 0, x));
                if (idx < 0 || idx >= this.arr.Length) return default;
                return this.arr[idx];
                
            }

            private bool Jump(out GridNodeData neighbor, in Unity.Collections.NativeArray<TempNodeData> temp, in GridNodeData current, in GridNodeData neighbourNode) {

                neighbor = default;
                if (neighbourNode.IsSuitable(this.burstConstraint, this.arr, this.graphSize, this.graphCenter) == false) {

                    return false;

                }
                
                var iX = neighbourNode.position.x;
                var iY = neighbourNode.position.z;

                if (this.IsSuitable(iX, iY) == false) {
                    return false;
                }

                if (this.GetIndex(iX, iY) == this.endNodeIndex) {
                    neighbor = this.GetNodeData(iX, iY);
                    return true;
                }

                int tDx = iX - current.position.x;
                int tDy = iY - current.position.z;

                {
                    // check for forced neighbors
                    // along the diagonal
                    if (tDx != 0 && tDy != 0) {
                        if ((this.IsSuitable(iX - tDx, iY + tDy) && !this.IsSuitable(iX - tDx, iY)) ||
                            (this.IsSuitable(iX + tDx, iY - tDy) && !this.IsSuitable(iX, iY - tDy))) {
                            neighbor = neighbourNode;
                            return true;
                        }
                    }
                    // horizontally/vertically
                    else {
                        if (tDx != 0) {
                            // moving along x
                            if ((this.IsSuitable(iX + tDx, iY + 1) && !this.IsSuitable(iX, iY + 1)) ||
                                (this.IsSuitable(iX + tDx, iY - 1) && !this.IsSuitable(iX, iY - 1))) {
                                neighbor = neighbourNode;
                                return true;
                            }
                        } else {
                            if ((this.IsSuitable(iX + 1, iY + tDy) && !this.IsSuitable(iX + 1, iY)) ||
                                (this.IsSuitable(iX - 1, iY + tDy) && !this.IsSuitable(iX - 1, iY))) {
                                neighbor = neighbourNode;
                                return true;
                            }
                        }
                    }

                    // when moving diagonally, must check for vertical/horizontal jump points
                    if (tDx != 0 && tDy != 0) {
                        if (this.Jump(out _, temp, neighbourNode, this.GetNodeData(iX + tDx, iY)) == true) {
                            neighbor = neighbourNode;
                            return true;
                        }

                        if (this.Jump(out _, temp, neighbourNode, this.GetNodeData(iX, iY + tDy)) == true) {
                            neighbor = neighbourNode;
                            return true;
                        }
                    }

                    // moving diagonally, must make sure one of the vertical/horizontal
                    // neighbors is open to allow the path
                    if (this.IsSuitable(iX + tDx, iY) || this.IsSuitable(iX, iY + tDy)) {
                        return this.Jump(out neighbor, temp, neighbourNode, this.GetNodeData(iX + tDx, iY + tDy));
                    } else {
                        return this.Jump(out neighbor, temp, neighbourNode, this.GetNodeData(iX + tDx, iY + tDy));
                    }
                }

                //return false;

            }

        }

        private Unity.Collections.NativeList<GridNodeData> AstarSearch(ref int statVisited, GridGraph graph, Node startNode, Node endNode, Constraint constraint, int threadIndex) {

            var graphSize = graph.size;
            var graphCenter = graph.graphCenter;
            var burstConstraint = constraint.GetBurstConstraint();
            var resultPath = new Unity.Collections.NativeList<GridNodeData>(10, Unity.Collections.Allocator.Persistent);
            var arr = new Unity.Collections.NativeArray<GridNodeData>(graph.nodesData, Unity.Collections.Allocator.TempJob);
            var openList = new PriorityQueueNative<GridNodeData>(Unity.Collections.Allocator.TempJob, 500, true);
            var results = new Unity.Collections.NativeArray<int>(2, Unity.Collections.Allocator.TempJob);
            var endNodeIndex = endNode.index;
            var startNodeIndex = startNode.index;

            var job = new Job() {
                graph = graph,
                graphCenter = (float3)graphCenter,
                graphSize = graphSize,
                burstConstraint = burstConstraint,
                resultPath = resultPath,
                arr = arr,
                openList = openList,
                results = results,
                endNodeIndex = endNodeIndex,
                startNodeIndex = startNodeIndex,
            };
            //job.Schedule().Complete();
            job.Execute();

            statVisited = results[0];
            
            results.Dispose();
            openList.Dispose();
            arr.Dispose();
            return resultPath;

        }
        
        private ListCopyable<Node> RetracePath(int threadIndex, Node endNode) {
            
            var path = PoolListCopyable<Node>.Spawn(10);
            path.Add(endNode);
            while (endNode.parent[threadIndex] != null) {
                
                endNode = endNode.parent[threadIndex];
                path.Add(endNode);
                
            }
            path.Reverse();
            return path;
            
        }

    }

}
