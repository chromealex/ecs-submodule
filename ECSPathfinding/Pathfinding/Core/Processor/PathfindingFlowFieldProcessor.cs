using Unity.Jobs;
using UnityEngine;

namespace ME.ECS.Pathfinding {
    
    using ME.ECS.Collections;

    public struct PathfindingFlowFieldProcessor : IPathfindingProcessor {

        public static bool cacheEnabled;
        private static System.Collections.Generic.Dictionary<int, BufferArray<byte>> pathCache = new System.Collections.Generic.Dictionary<int, BufferArray<byte>>();
        private static System.Collections.Generic.Queue<int> pathCacheQueue = new System.Collections.Generic.Queue<int>();
        private const int CACHE_SIZE = 100;
        
        public Path Run<TMod>(LogLevel pathfindingLogLevel, Vector3 from, Vector3 to, Constraint constraint, Graph graph, TMod pathModifier, int threadIndex = 0, bool burstEnabled = false) where TMod : struct, IPathModifier {

            if (threadIndex < 0) threadIndex = 0;
            threadIndex = threadIndex % Pathfinding.THREADS_COUNT;

            var constraintStart = constraint;
            constraintStart.checkWalkability = true;
            constraintStart.walkable = true;
            var startNode = graph.GetNearest(from, constraintStart);
            if (startNode == null) return new Path();

            var constraintEnd = constraintStart;
            constraintEnd.checkArea = true;
            constraintEnd.areaMask = (1 << startNode.area);
            
            var endNode = graph.GetNearest(to, constraintEnd);
            if (endNode == null) return new Path();
            
            System.Diagnostics.Stopwatch swPath = null;
            if ((pathfindingLogLevel & LogLevel.Path) != 0) swPath = System.Diagnostics.Stopwatch.StartNew();

            if (PathfindingFlowFieldProcessor.cacheEnabled == true) {

                if (PathfindingFlowFieldProcessor.pathCache.TryGetValue(endNode.index, out var buffer) == true) {

                    var pathCache = new Path() {
                        graph = graph,
                        result = PathCompleteState.Complete,
                        flowField = buffer,
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
                
                this.FlowFieldBurst(ref statVisited, (GridGraph)graph, ref flowField, (GridNode)endNode, constraint, pathModifier);
                if (PathfindingFlowFieldProcessor.cacheEnabled == true) {

                    if (PathfindingFlowFieldProcessor.pathCache.Count > PathfindingFlowFieldProcessor.CACHE_SIZE) {

                        const int size = PathfindingFlowFieldProcessor.CACHE_SIZE / 10;
                        for (int i = 0; i < size; ++i) {
                            
                            var idx = PathfindingFlowFieldProcessor.pathCacheQueue.Dequeue();
                            PathfindingFlowFieldProcessor.pathCache.Remove(idx);
                            
                        }
                        
                        PathfindingFlowFieldProcessor.pathCacheQueue.Clear();

                    }

                    PathfindingFlowFieldProcessor.pathCache.Add(endNode.index, flowField);
                    PathfindingFlowFieldProcessor.pathCacheQueue.Enqueue(endNode.index);

                }

            } else { // no burst
            
                var visited = PoolListCopyable<Node>.Spawn(10);
                for (int i = 0; i < graph.nodes.Count; ++i) {

                    graph.nodes[i].Reset(threadIndex);

                }

                this.CreateIntegrationField(graph, visited, endNode, constraint, threadIndex);
                this.CreateFlowField(graph, ref flowField, endNode, constraint, threadIndex);
                
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

            if ((pathfindingLogLevel & LogLevel.Path) != 0) {
                
                Logger.Log(string.Format("Path result {0}, built in {1}ms. Path length: (visited: {2})\nThread Index: {3}", path.result, swPath.ElapsedMilliseconds, statVisited, threadIndex));
                
            }

            return path;

        }

        public struct NativeQueue<T> where T : struct {

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

        [Unity.Burst.BurstCompile(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, DisableSafetyChecks = true)]
        private struct Job : Unity.Jobs.IJob {

            public BurstConstraint constraint;
            public Unity.Collections.NativeArray<GridNodeData> arr;
            public Unity.Collections.NativeArray<int> bestCost;
            public Unity.Collections.NativeArray<byte> flowField;
            public NativeQueue<int> queue;
            public Unity.Collections.NativeArray<int> results;
            public PathCustomWalkableField pathCustomWalkableField;
            
            public int endNodeIndex;

            public void Execute() {

                // Creating cost field
                var visited = new Unity.Collections.NativeList<int>(this.arr.Length, Unity.Collections.Allocator.Temp);
                this.queue.Enqueue(this.endNodeIndex);
                while (this.queue.Count > 0) {

                    ++this.results[0];
                    var curNodeIndex = this.queue.Dequeue();
                    visited.Add(curNodeIndex);
                    var nodeData = this.arr[curNodeIndex];
                    // TODO: add custom connections support
                    var connections = nodeData.connections;
                    for (int i = 0; i < connections.Length; ++i) {

                        var conn = connections.Get(i);
                        if (conn.index < 0) continue;

                        var neighbor = this.arr[conn.index];
                        if (neighbor.IsSuitable(this.constraint) == false) {

                            continue;

                        }

                        int customCost = 0;
                        if (this.pathCustomWalkableField.IsWalkable(conn.index, default) == false) {
                            
                            customCost = this.pathCustomWalkableField.GetCustomCost(conn.index, default);
                            
                        }
                        
                        var cost = neighbor.penalty + customCost;
                        var endNodeCost = cost + this.bestCost[nodeData.index];
                        if (endNodeCost < this.bestCost[neighbor.index]) {

                            this.bestCost[neighbor.index] = endNodeCost;
                            this.queue.Enqueue(neighbor.index);
                            
                        }

                    }

                }
                
                // Creating direction field
                for (int i = 0, cnt = visited.Length; i < cnt; ++i) {

                    var nodeIdx = visited[i];
                    var ffCost = this.bestCost[nodeIdx];
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
                    
                        var cost = this.bestCost[conn.index];
                        if (cost < minCost) {

                            minCost = cost;
                            dir = iterDir - 1;

                        }

                    }

                    this.flowField[nodeIdx] = (byte)dir;

                }

                visited.Dispose();

            }

        }

        private void FlowFieldBurst<TMod>(ref int statVisited, GridGraph graph, ref BufferArray<byte> flowFieldResult, GridNode endNode, Constraint constraint, TMod mod) where TMod : struct, IPathModifier {
            
            var arr = new Unity.Collections.NativeArray<GridNodeData>(graph.nodes.Count, Unity.Collections.Allocator.TempJob);
            var bestCost = new Unity.Collections.NativeArray<int>(graph.nodes.Count, Unity.Collections.Allocator.TempJob);
            for (int i = 0; i < arr.Length; ++i) {

                arr[i] = ((GridNode)graph.nodes[i]).GetData();
                bestCost[i] = int.MaxValue;

            }
            
            bestCost[endNode.index] = 0;
            
            var queue = new NativeQueue<int>(graph.nodes.Count, Unity.Collections.Allocator.TempJob);
            var flowField = new Unity.Collections.NativeArray<byte>(graph.nodes.Count, Unity.Collections.Allocator.TempJob);
            var results = new Unity.Collections.NativeArray<int>(1, Unity.Collections.Allocator.TempJob);

            PathCustomWalkableField pathCustomWalkableField = new PathCustomWalkableField() {
                field = new Unity.Collections.NativeArray<int>(0, Unity.Collections.Allocator.TempJob),
            };
            if (mod is PathCustomWalkableField custom) {

                pathCustomWalkableField.field.Dispose();
                pathCustomWalkableField = custom;

            }
            
            var job = new Job() {
                arr = arr,
                queue = queue,
                bestCost = bestCost,
                flowField = flowField,
                endNodeIndex = endNode.index,
                constraint = constraint.GetBurstConstraint(),
                results = results,
                pathCustomWalkableField = pathCustomWalkableField,
            };
            job.Schedule().Complete();

            Unity.Collections.NativeArray<byte>.Copy(flowField, flowFieldResult.arr, flowField.Length);

            statVisited = results[0];

            pathCustomWalkableField.field.Dispose();
            results.Dispose();
            queue.Dispose();
            flowField.Dispose();
            bestCost.Dispose();
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
