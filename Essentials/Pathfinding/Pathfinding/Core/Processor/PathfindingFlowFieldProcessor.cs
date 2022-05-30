using Unity.Jobs;
using UnityEngine;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding {
    
    using ME.ECS.Collections;

    public struct PathfindingFlowFieldProcessor : IPathfindingProcessor {

        private static System.Collections.Generic.Dictionary<long, BufferArray<byte>> pathCache = new System.Collections.Generic.Dictionary<long, BufferArray<byte>>();
        private static System.Collections.Generic.Queue<long> pathCacheQueue = new System.Collections.Generic.Queue<long>();
        private const int CACHE_SIZE = 100;

        public static void ClearCache() {

            foreach (var item in PathfindingFlowFieldProcessor.pathCache) {
                
                PoolArray<byte>.Recycle(item.Value);
                
            }
            PathfindingFlowFieldProcessor.pathCache.Clear();
            PathfindingFlowFieldProcessor.pathCacheQueue.Clear();
            
        }
        
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

            //UnityEngine.Debug.Log(endNode.worldPosition + " :: " + ((GridNode)endNode).erosion);
            //UnityEngine.Debug.DrawLine(endNode.worldPosition, endNode.worldPosition + Vector3.up * 10f, Color.red, 3f);

            var key = MathUtils.GetKey(constraint.GetKey(), endNode.node.index);
            //UnityEngine.Debug.Log("Build path cache: " + cacheEnabled + ", burst: " + burstEnabled);
            if (cacheEnabled == true) {

                if (PathfindingFlowFieldProcessor.pathCache.TryGetValue(key, out var buffer) == true) {

                    var pathCache = new Path() {
                        graph = graph,
                        result = PathCompleteState.Complete,
                        flowField = buffer,
                        cacheEnabled = cacheEnabled,
                    };
                    
                    if ((pathfindingLogLevel & LogLevel.Path) != 0) {
                
                        Logger.Log(string.Format("Path result {0}, cache in {1}ms. \nThread Index: {2}", pathCache.result, swPath.ElapsedMilliseconds, threadIndex));
                
                    }

                    return pathCache;

                }

            }

            var flowField = PoolArray<byte>.Spawn(graph.nodes.Count);
            int statVisited = 0;
            if (burstEnabled == true) { // burst
                
                this.FlowFieldBurst(ref statVisited, (GridGraph)graph, ref flowField, (GridNode)endNode.node, constraint, pathModifier);
                if (cacheEnabled == true) {

                    if (PathfindingFlowFieldProcessor.pathCache.Count > PathfindingFlowFieldProcessor.CACHE_SIZE) {

                        const int size = PathfindingFlowFieldProcessor.CACHE_SIZE / 10;
                        for (int i = 0; i < size; ++i) {
                            
                            var idx = PathfindingFlowFieldProcessor.pathCacheQueue.Dequeue();
                            PathfindingFlowFieldProcessor.pathCache.Remove(idx);
                            
                        }
                        
                        PathfindingFlowFieldProcessor.pathCacheQueue.Clear();

                    }

                    PathfindingFlowFieldProcessor.pathCache.Add(key, flowField);
                    PathfindingFlowFieldProcessor.pathCacheQueue.Enqueue(key);

                }

            } else { // no burst
            
                var visited = PoolListCopyable<Node>.Spawn(10);
                for (int i = 0; i < graph.nodes.Count; ++i) {

                    graph.nodes[i].Reset(threadIndex);

                }

                this.CreateIntegrationField(graph, visited, endNode.node, constraint, threadIndex);
                this.CreateFlowField(graph, ref flowField, endNode.node, constraint, threadIndex);
                
                statVisited = visited.Count;
                for (int i = 0; i < visited.Count; ++i) {

                    visited[i].Reset(threadIndex);

                }
                PoolListCopyable<Node>.Recycle(ref visited);

            }

            var path = new Path();
            path.graph = graph;
            path.result = PathCompleteState.Complete;
            path.flowField = flowField;
            path.cacheEnabled = cacheEnabled;

            if ((pathfindingLogLevel & LogLevel.Path) != 0) {
                
                Logger.Log(string.Format("Path result {0}, built in {1}ms. Path length: (visited: {2})\nThread Index: {3}", path.result, (swPath.ElapsedTicks / (double)System.TimeSpan.TicksPerMillisecond).ToString("0.##"), statVisited, threadIndex));
                
            }

            return path;

        }

        public struct NativeQueue<T> where T : unmanaged {

            public int Count;
            public Unity.Collections.NativeList<T> arr;
            public int head;
            public int last;
            
            public NativeQueue(int size, Unity.Collections.Allocator allocator) {
                
                this.arr = new Unity.Collections.NativeList<T>(size, allocator);
                this.Count = 0;
                this.head = -1;
                this.last = -1;

            }

            public T Dequeue() {
                
                var data = this.arr[this.head];
                --this.Count;
                ++this.head;
                if (this.head > this.last) {
                    
                    this.head = -1;
                    this.last = -1;

                }
                return data;
                
            }

            public void Enqueue(T data) {

                ++this.last;
                if (this.last >= this.arr.Length) this.arr.Add(default);
                this.arr[this.last] = data;
                if (this.head == -1) this.head = this.last;
                ++this.Count;

            }

            public void Dispose() {

                this.arr.Dispose();
                this.Count = default;
                this.last = default;
                this.head = default;

            }

        }

        [Unity.Burst.BurstCompile(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true)]
        private struct Job : Unity.Jobs.IJob {

            public BurstConstraint constraint;
            public Unity.Collections.NativeArray<GridNodeData> arr;
            public Unity.Collections.NativeArray<byte> flowField;
            public NativeQueue<int> queue;
            public Unity.Collections.NativeArray<int> results;
            public PathCustomWalkableField pathCustomWalkableField;
            public Vector3Int graphSize;
            public float3 graphCenter;
            
            public int endNodeIndex;

            public void Execute() {
                
                var bestCost = new Unity.Collections.NativeArray<int>(this.arr.Length, Unity.Collections.Allocator.Temp);
                for (int i = 0; i < this.arr.Length; ++i) {

                    bestCost[i] = int.MaxValue;

                }
            
                bestCost[this.endNodeIndex] = 0;

                // Creating cost field
                //var visited = new Unity.Collections.NativeList<int>(this.arr.Length, Unity.Collections.Allocator.Temp);
                this.queue.Enqueue(this.endNodeIndex);
                while (this.queue.Count > 0) {

                    ++this.results[0];
                    var curNodeIndex = this.queue.Dequeue();
                    //visited.Add(curNodeIndex);
                    var nodeData = this.arr[curNodeIndex];
                    // TODO: add custom connections support
                    var connections = nodeData.connections;
                    for (int i = 0; i < connections.Length; ++i) {

                        var conn = connections.Get(i);
                        if (conn.index < 0) continue;

                        var neighbor = this.arr[conn.index];
                        if (neighbor.IsSuitable(this.constraint, this.arr, this.graphSize, this.graphCenter) == false) {

                            continue;

                        }

                        if (this.pathCustomWalkableField.IsTraversable(conn.index, this.constraint) == false) {

                            continue;

                        }

                        int customCost = 0;
                        if (this.pathCustomWalkableField.IsWalkable(conn.index, this.constraint) == false) {
                            
                            customCost = this.pathCustomWalkableField.GetCustomCost(conn.index, this.constraint);
                            
                        }
                        
                        var cost = neighbor.penalty + customCost;
                        var endNodeCost = cost + bestCost[nodeData.index];
                        if (endNodeCost < bestCost[neighbor.index]) {

                            bestCost[neighbor.index] = endNodeCost;
                            this.queue.Enqueue(neighbor.index);
                            
                        }

                    }

                }
                
                // Creating direction field
                for (int i = 0, cnt = this.arr.Length; i < cnt; ++i) {

                    var nodeIdx = this.arr[i].index;
                    var ffCost = bestCost[nodeIdx];
                    var node = this.arr[nodeIdx];
                    var minCost = ffCost;
                    if (this.endNodeIndex == node.index) minCost = 0;

                    // TODO: add custom connections support
                    var connections = node.connections;
                    var dir = 0;
                    var iterDir = 2;
                    for (int j = 2; j < 10; ++j) {
                        
                        ++iterDir;

                        var conn = connections.Get(j);
                        if (conn.index < 0) continue;
                    
                        var cost = bestCost[conn.index];
                        if (cost < minCost) {

                            minCost = cost;
                            dir = iterDir - 1;

                        }

                    }

                    this.flowField[nodeIdx] = (byte)dir;

                }

                bestCost.Dispose();

            }

        }

        [Unity.Burst.BurstCompileAttribute(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true)]
        private struct UpdateWalkableField : IJobParallelFor {

            public Unity.Collections.NativeArray<GridNodeData> nodes;
            public PathCustomWalkableField pathCustomWalkableField;
            public BurstConstraint constraint;
            
            public void Execute(int index) {
                
                var node = this.nodes[index];
                if ((this.constraint.tagsMask & node.tag) != 0) {

                    this.pathCustomWalkableField.walkableField[index] = 1;

                }
                
            }

        }

        private void FlowFieldBurst<TMod>(ref int statVisited, GridGraph graph, ref BufferArray<byte> flowFieldResult, GridNode endNode, Constraint constraint, TMod mod) where TMod : struct, IPathModifier {
            
            var arr = new Unity.Collections.NativeArray<GridNodeData>(graph.nodesData, Unity.Collections.Allocator.TempJob);
            var queue = new NativeQueue<int>(graph.nodes.Count, Unity.Collections.Allocator.TempJob);
            var flowField = new Unity.Collections.NativeArray<byte>(graph.nodes.Count, Unity.Collections.Allocator.TempJob);
            var results = new Unity.Collections.NativeArray<int>(1, Unity.Collections.Allocator.TempJob);
            var burstConstraint = constraint.GetBurstConstraint();

            PathCustomWalkableField pathCustomWalkableField = new PathCustomWalkableField() {
                walkableField = new Unity.Collections.NativeArray<int>(graph.nodes.Count, Unity.Collections.Allocator.TempJob),
                erosionField = new Unity.Collections.NativeArray<int>(graph.nodes.Count, Unity.Collections.Allocator.TempJob),
            };
            if (mod is PathCustomWalkableField custom) {

                pathCustomWalkableField.walkableField.Dispose();
                pathCustomWalkableField.erosionField.Dispose();
                pathCustomWalkableField = custom;

            }
            
            // Update walkable field by tags
            if (constraint.checkTags == true) {

                var updateWalkableFieldJob = new UpdateWalkableField() {
                    nodes = arr,
                    pathCustomWalkableField = pathCustomWalkableField,
                    constraint = burstConstraint,
                };
                updateWalkableFieldJob.Schedule(pathCustomWalkableField.walkableField.Length, 64).Complete();
                pathCustomWalkableField = updateWalkableFieldJob.pathCustomWalkableField;

            }

            graph.BuildErosionJob(arr, pathCustomWalkableField.walkableField, ref pathCustomWalkableField.erosionField);
            /*foreach (var node in graph.nodes) {

                var erosion = pathCustomWalkableField.erosionField[node.index];
                UnityEngine.Debug.DrawLine(node.worldPosition, node.worldPosition + UnityEngine.Vector3.up * (1f * erosion), UnityEngine.Color.red, 1f);

            }*/
            var job = new Job() {
                arr = arr,
                queue = queue,
                flowField = flowField,
                endNodeIndex = endNode.index,
                constraint = burstConstraint,
                results = results,
                pathCustomWalkableField = pathCustomWalkableField,
                graphSize = graph.size,
                graphCenter = (float3)graph.graphCenter,
            };
            job.Schedule().Complete();

            Unity.Collections.NativeArray<byte>.Copy(flowField, flowFieldResult.arr, flowField.Length);

            statVisited = results[0];

            pathCustomWalkableField.walkableField.Dispose();
            pathCustomWalkableField.erosionField.Dispose();
            results.Dispose();
            queue.Dispose();
            flowField.Dispose();
            arr.Dispose();

        }

        private void CreateFlowField(Graph graph, ref BufferArray<byte> flowField, Node endNode, Constraint constraint, int threadIndex) {

            // Create flow field
            for (int i = 0, cnt = graph.nodes.Count; i < cnt; ++i) {

                var ffCost = graph.nodes[i].bestCost[threadIndex];
                var node = graph.nodes[i];
                var minCost = ffCost;
                if (endNode == node) minCost = 0;

                var connections = node.GetConnections();
                var dir = 0;
                var iterDir = 0;
                var dirFound = false;
                foreach (var conn in connections) {
                    
                    ++iterDir;

                    if (conn.index < 0) continue;
                    
                    var idx = conn.index;
                    var cost = graph.nodes[idx].bestCost[threadIndex];

                    if (cost < minCost) {

                        minCost = cost;
                        dir = iterDir - 1;
                        dirFound = true;

                    }

                }

                if (dirFound == false) {

                    var customConnections = node.GetCustomConnections();
                    if (customConnections != null) {

                        foreach (var conn in customConnections) {

                            if (conn.index < 0) continue;

                            var target = graph.nodes[conn.index];
                            var cost = target.bestCost[threadIndex];
                            if (cost < minCost) {

                                minCost = cost;
                                dir = (byte)GridGraphUtilities.GetDirection(target.worldPosition - node.worldPosition);

                            }

                        }

                    }

                }

                flowField.arr[i] = (byte)dir;

            }
            
        }
        
        private void CreateIntegrationField(Graph graph, ListCopyable<Node> visited, Node endNode, Constraint constraint, int threadIndex) {

            // Create integration field
            var queue = PoolQueue<Node>.Spawn(500);
            queue.Enqueue(endNode);

            endNode.bestCost[threadIndex] = 0;
            visited.Add(endNode);

            while (queue.Count > 0) {

                var curNode = queue.Dequeue();
                var connections = curNode.GetAllConnections();
                for (int i = 0; i < connections.Length; ++i) {

                    var conn = connections.arr[i];
                    if (conn.index < 0) continue;

                    var neighbor = graph.nodes[conn.index];
                    if (neighbor.IsSuitable(constraint) == false) {

                        continue;

                    }

                    var cost = neighbor.penalty;
                    var endNodeCost = (float)(cost + curNode.bestCost[threadIndex]);
                    if (endNodeCost < neighbor.bestCost[threadIndex]) {

                        neighbor.bestCost[threadIndex] = endNodeCost;
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);

                    }

                }

                connections.Dispose();

            }
            
            PoolQueue<Node>.Recycle(ref queue);

        }
        
    }

}
