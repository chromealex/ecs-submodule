using UnityEngine;

namespace ME.ECS.Pathfinding {
    
    using ME.ECS.Collections;

    public struct PathfindingAstarProcessor : IPathfindingProcessor {
        
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
            
            var visited = PoolListCopyable<Node>.Spawn(10);
            System.Diagnostics.Stopwatch swPath = null;
            if ((pathfindingLogLevel & LogLevel.Path) != 0) swPath = System.Diagnostics.Stopwatch.StartNew();
            var nodesPath = this.AstarSearch(graph, visited, startNode, endNode, constraint, threadIndex);

            var statVisited = visited.Count;
            var statLength = 0;
            
            var path = new Path();
            path.graph = graph;
            path.result = PathCompleteState.NotCalculated;

            if (nodesPath == null) {

                path.result = PathCompleteState.NotExist;

            } else {

                statLength = nodesPath.Count;
                
                path.result = PathCompleteState.Complete;
                path.nodes = nodesPath;

            }
            
            for (int i = 0; i < visited.Count; ++i) {

                visited[i].Reset(threadIndex);

            }

            PoolListCopyable<Node>.Recycle(ref visited);

            System.Diagnostics.Stopwatch swModifier = null;
            if ((pathfindingLogLevel & LogLevel.PathMods) != 0) swModifier = System.Diagnostics.Stopwatch.StartNew();
            if (path.result == PathCompleteState.Complete) {

                path = pathModifier.Run(path, constraint);

            }
            
            if ((pathfindingLogLevel & LogLevel.Path) != 0) {
                
                Logger.Log(string.Format("Path result {0}, built in {1}ms. Path length: {2} (visited: {3})\nThread Index: {4}", path.result, swPath.ElapsedMilliseconds, statLength, statVisited, threadIndex));
                
            }

            if ((pathfindingLogLevel & LogLevel.PathMods) != 0) {

                Logger.Log(string.Format("Path Mods: {0}ms", swModifier.ElapsedMilliseconds));

            }

            return path;

        }

        private ListCopyable<Node> AstarSearch(Graph graph, ListCopyable<Node> visited, Node startNode, Node endNode, Constraint constraint, int threadIndex) {
            
            var openList = PoolPriorityQueue<Node>.Spawn(500);
            openList.isMinPriorityQueue = true;
            
            startNode.startToCurNodeLen[threadIndex] = 0f;
            
            openList.Enqueue(0, startNode);
            startNode.isOpened[threadIndex] = true;

            while (openList.Count > 0) {
                
                var node = openList.Dequeue();
                node.isClosed[threadIndex] = true;
                
                visited.Add(node);

                if (node.index == endNode.index) {
                    
                    PoolPriorityQueue<Node>.Recycle(ref openList);
                    return this.RetracePath(threadIndex, endNode);
                    
                }

                var neighbors = node.GetAllConnections();
                var currentNodeCost = node.startToCurNodeLen[threadIndex];
                foreach(var conn in neighbors) {
                    
                    if (conn.index < 0) continue;
                    
                    var neighbor = graph.nodes[conn.index];
                    if (neighbor.isClosed[threadIndex] == true) continue;
                    if (neighbor.IsSuitable(constraint) == false) continue;

                    var cost = currentNodeCost + conn.cost;
                    if (neighbor.isOpened[threadIndex] == false || cost < neighbor.startToCurNodeLen[threadIndex]) {
                        
                        neighbor.startToCurNodeLen[threadIndex] = cost;
                        neighbor.parent[threadIndex] = node;
                        if (neighbor.isOpened[threadIndex] == false) {
                            
                            openList.Enqueue(cost, neighbor);
                            visited.Add(neighbor);
                            neighbor.isOpened[threadIndex] = true;
                            
                        }
                        
                    }
                    
                }

                neighbors.Dispose();

            }

            PoolPriorityQueue<Node>.Recycle(ref openList);
            return null;

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
