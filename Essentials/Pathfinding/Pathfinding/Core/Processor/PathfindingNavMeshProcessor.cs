using Unity.Jobs;
using UnityEngine;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding {

    public struct PathfindingNavMeshProcessor : IPathfindingProcessor {
        
        private struct Cache {

            private struct Entry {

                public Tick tick;
                public int hash;

                public float3 from;
                public float3 to;
                public int constraintKey;
                public Path path;

            }
            
            private const int maxCacheSize = 20;
            private static Entry[] pool = new Entry[Cache.maxCacheSize];
            private static int currentIndex = 0;

            public static bool Get(in float3 from, in float3 to, int constraintKey, in NavMeshGraph graph, out Path path) {

                path = default;

                if (Cache.GetTick(out var tick) == true && graph != null) {
                    var hash = graph.lastGraphUpdateHash;
                    
                    for (int i = 0; i < Cache.pool.Length; i++) {
                        ref readonly var entry = ref Cache.pool[i];
    
                        if (entry.hash == hash && entry.tick == tick) {
                            if (math.all(entry.from == from) && math.all(entry.to == to) && entry.constraintKey == constraintKey) {
                                path = Path.Clone(entry.path);
    
                                return true;
                            }
                        }
                    }
                }

                return false;
            }

            public static void Set(in float3 from, in float3 to, int constraintKey, in NavMeshGraph graph, in Path path) {
                
                if (path.result == PathCompleteState.Complete && Cache.GetTick(out var tick) == true && graph != null) {

                    ref var entry = ref Cache.pool[Cache.currentIndex];
                    
                    entry.hash = graph.lastGraphUpdateHash;
                    entry.tick = tick;
                    entry.from = from;
                    entry.to = to;
                    entry.constraintKey = constraintKey;
                    
                    entry.path.Recycle();
                    entry.path = Path.Clone(path);

                    Cache.currentIndex = (Cache.currentIndex + 1) % Cache.maxCacheSize;

                }
                
            }

            private static bool GetTick(out Tick tick) {
                
                tick = Tick.Invalid;
                
                var world = Worlds.currentWorld;

                if (world != null) {
                    var state = world.GetState();

                    if (state != null) {
                        tick = state.tick;
                        return true;
                    } else {
                        UnityEngine.Debug.LogError("[Path Cache] world.GetState() == null");
                    }
                } else {
                    UnityEngine.Debug.LogError("[Path Cache] Worlds.currentWorld == null");
                }

                return false;
            }

        }

        private struct PathInternal {

            public int corners;
            public Unity.Collections.NativeArray<UnityEngine.Experimental.AI.NavMeshLocation> results;
            public UnityEngine.Experimental.AI.PathQueryStatus pathStatus;

            public void Dispose() {

                this.corners = 0;
                if (this.results.IsCreated == true) {
                    this.results.Dispose();
                }

                this.pathStatus = default;

            }

        }

        [Unity.Burst.BurstCompile(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true)]
        private struct FindStraightPathJob : IJob {

            public UnityEngine.Experimental.AI.NavMeshQuery query;
            public Unity.Collections.NativeArray<UnityEngine.Experimental.AI.PolygonId> pathInternal;
            public UnityEngine.Experimental.AI.NavMeshLocation from;
            public UnityEngine.Experimental.AI.NavMeshLocation to;
            public int pathSize;
            public Unity.Collections.NativeArray<UnityEngine.Experimental.AI.NavMeshLocation> results;
            public Unity.Collections.NativeArray<StraightPathFlags> straightPathFlags;
            public Unity.Collections.NativeArray<float> vertexSide;
            public Unity.Collections.NativeArray<int> resultStatus;

            public void Execute() {

                var cornerCount = 0;
                this.resultStatus[0] = (int)PathUtils.FindStraightPath(
                    this.query,
                    this.from.position,
                    this.to.position,
                    this.pathInternal,
                    this.pathSize,
                    ref this.results,
                    ref this.straightPathFlags,
                    ref this.vertexSide,
                    ref cornerCount,
                    PathfindingNavMeshProcessor.MAX_PATH_SIZE
                );
                this.resultStatus[1] = cornerCount;

            }

        }

        [Unity.Burst.BurstCompile(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true)]
        private struct BuildPathJob : IJob {

            public UnityEngine.Experimental.AI.NavMeshQuery query;
            public float3 fromPoint;
            public float3 toPoint;
            public int agentTypeId;
            public int areas;
            
            public Unity.Collections.NativeArray<UnityEngine.Experimental.AI.NavMeshLocation> results;
            public Unity.Collections.NativeArray<int> pathResults;

            public void Execute() {

                this.pathResults[0] = default;
                this.pathResults[1] = default;

                //UnityEngine.Debug.Log("Exec1");
                var from = this.query.MapLocation((Vector3)this.fromPoint, new Vector3(100f, 100f, 100f), this.agentTypeId, this.areas);
                if (from.polygon.IsNull() == true) {
                    return;
                }
                
                //UnityEngine.Debug.Log("Exec2");
                var to = this.query.MapLocation((Vector3)this.toPoint, new Vector3(100f, 100f, 100f), this.agentTypeId, this.areas);
                if (to.polygon.IsNull() == true) {
                    return;
                }

                //UnityEngine.Debug.Log("Exec3");
                this.query.BeginFindPath(from, to, this.areas);
                this.query.UpdateFindPath(PathfindingNavMeshProcessor.MAX_ITERATIONS, out var performed);
                //statVisited = performed;

                var result = this.query.EndFindPath(out var pathSize);
                if ((result & UnityEngine.Experimental.AI.PathQueryStatus.Success) != 0) {

                    var pathInternal = new Unity.Collections.NativeArray<UnityEngine.Experimental.AI.PolygonId>(pathSize, Unity.Collections.Allocator.Temp);
                    var straightPathFlags = new Unity.Collections.NativeArray<StraightPathFlags>(PathfindingNavMeshProcessor.MAX_PATH_SIZE, Unity.Collections.Allocator.Temp);
                    var vertexSide = new Unity.Collections.NativeArray<float>(PathfindingNavMeshProcessor.MAX_PATH_SIZE, Unity.Collections.Allocator.Temp);

                    //UnityEngine.Debug.Log("Exec4");
                    this.query.GetPathResult(pathInternal);
                    var job = new FindStraightPathJob() {
                        query = this.query,
                        from = from,
                        to = to,
                        pathInternal = pathInternal,
                        pathSize = pathSize,
                        results = this.results,
                        straightPathFlags = straightPathFlags,
                        vertexSide = vertexSide,
                        resultStatus = this.pathResults,
                    };
                    job.Execute();

                    if ((result & UnityEngine.Experimental.AI.PathQueryStatus.PartialResult) != 0) {
                        
                        this.pathResults[0] |= (int)UnityEngine.Experimental.AI.PathQueryStatus.PartialResult;
                        
                    }

                    vertexSide.Dispose();
                    straightPathFlags.Dispose();
                    pathInternal.Dispose();

                }

            }

        }

        private const int POOL_SIZE = 1024;
        private const int MAX_ITERATIONS = 1024;
        private const int MAX_PATH_SIZE = 1024;

        public Path Run<TMod>(LogLevel pathfindingLogLevel, float3 fromPoint, float3 toPoint, Constraint constraint, Graph graph, TMod pathModifier, int threadIndex = 0,
                              bool burstEnabled = true, bool cacheEnabled = false) where TMod : struct, IPathModifier {
            
            var navMeshGraph = (NavMeshGraph)graph;

            if (Cache.Get(fromPoint, toPoint, constraint.GetKey(), navMeshGraph, out var path) == true) {
                return path;
            }

            var pathResult = new PathInternal();
            

            var areas = -1;
            if (constraint.checkArea == true) {

                areas = (int)constraint.areaMask;

            }

            System.Diagnostics.Stopwatch swPath = null;
            if ((pathfindingLogLevel & LogLevel.Path) != 0) {
                swPath = System.Diagnostics.Stopwatch.StartNew();
            }

            var statLength = 0;
            var statVisited = 0;

            var query = new UnityEngine.Experimental.AI.NavMeshQuery(UnityEngine.Experimental.AI.NavMeshWorld.GetDefaultWorld(), Unity.Collections.Allocator.TempJob, PathfindingNavMeshProcessor.POOL_SIZE);

            if (burstEnabled == true) {

                var results = new Unity.Collections.NativeArray<UnityEngine.Experimental.AI.NavMeshLocation>(PathfindingNavMeshProcessor.MAX_PATH_SIZE, Unity.Collections.Allocator.TempJob);
                var pathResults = new Unity.Collections.NativeArray<int>(2, Unity.Collections.Allocator.TempJob);
                var job = new BuildPathJob() {
                    query = query,
                    fromPoint = fromPoint,
                    toPoint = toPoint,
                    agentTypeId = navMeshGraph.agentTypeId,
                    areas = areas,
                    pathResults = pathResults,
                    results = results,
                };
                job.Run();
                
                var pathStatus = (UnityEngine.Experimental.AI.PathQueryStatus)pathResults[0];
                var cornerCount = pathResults[1];
                pathResults.Dispose();
                
                if ((pathStatus & UnityEngine.Experimental.AI.PathQueryStatus.Success) != 0) {

                    if (cornerCount >= 2) {

                        path.navMeshPoints = PoolListCopyable<float3>.Spawn(cornerCount);
                        for (var i = 0; i < cornerCount; ++i) {

                            path.navMeshPoints.Add((float3)results[i].position);

                        }

                        if ((pathfindingLogLevel & LogLevel.Path) != 0) {
                        
                            var hash = 0;
                            for (var i = 0; i < cornerCount; ++i) {
                                hash ^= (int)(results[i].position.x * 1000000f);
                            }

                            UnityEngine.Debug.Log("Path hash X: " + hash);

                            hash = 0;
                            for (var i = 0; i < cornerCount; ++i) {
                                hash ^= (int)(results[i].position.y * 1000000f);
                            }

                            UnityEngine.Debug.Log("Path hash Y: " + hash);

                            hash = 0;
                            for (var i = 0; i < cornerCount; ++i) {
                                hash ^= (int)(results[i].position.z * 1000000f);
                            }

                            UnityEngine.Debug.Log("Path hash Z: " + hash);
                            
                        }

                        if ((pathStatus & UnityEngine.Experimental.AI.PathQueryStatus.PartialResult) != 0) {
                            
                            path.result = PathCompleteState.CompletePartial;
                            
                        } else {

                            path.result = PathCompleteState.Complete;

                        }

                    } else {

                        path.result = PathCompleteState.NotExist;

                    }

                }
                
                Cache.Set(fromPoint, toPoint, constraint.GetKey(), navMeshGraph, path);
                
                results.Dispose();
                query.Dispose();
                return path;
                
            }
            
            UnityEngine.AI.NavMesh.SamplePosition((Vector3)fromPoint, out var hitFrom, 1000f, new UnityEngine.AI.NavMeshQueryFilter() {
                agentTypeID = navMeshGraph.agentTypeId,
                areaMask = areas,
            });
            fromPoint = (float3)hitFrom.position;
            var from = query.MapLocation((Vector3)fromPoint, Vector3.one * 10f, navMeshGraph.agentTypeId, areas);
            if (from.polygon.IsNull() == true) {
                return path;
            }

            UnityEngine.AI.NavMesh.SamplePosition((Vector3)toPoint, out var hitTo, 1000f, new UnityEngine.AI.NavMeshQueryFilter() {
                agentTypeID = navMeshGraph.agentTypeId,
                areaMask = areas,
            });
            toPoint = (float3)hitTo.position;
            var to = query.MapLocation((Vector3)toPoint, Vector3.one * 10f, navMeshGraph.agentTypeId, areas);
            if (to.polygon.IsNull() == true) {
                return path;
            }

            var marker = new Unity.Profiling.ProfilerMarker("PathfindingNavMeshProcessor::Query::BuildPath");
            marker.Begin();
            query.BeginFindPath(from, to, areas);
            query.UpdateFindPath(PathfindingNavMeshProcessor.MAX_ITERATIONS, out var performed);
            marker.End();
            statVisited = performed;

            var result = query.EndFindPath(out var pathSize);
            if ((result & UnityEngine.Experimental.AI.PathQueryStatus.Success) != 0) {

                var pathInternal = new Unity.Collections.NativeArray<UnityEngine.Experimental.AI.PolygonId>(pathSize, Unity.Collections.Allocator.Persistent);
                query.GetPathResult(pathInternal);

                var markerFindStraight = new Unity.Profiling.ProfilerMarker("PathfindingNavMeshProcessor::Query::FindStraightPath");
                markerFindStraight.Begin();
                
                var straightPathFlags = new Unity.Collections.NativeArray<StraightPathFlags>(PathfindingNavMeshProcessor.MAX_PATH_SIZE, Unity.Collections.Allocator.Persistent);
                var vertexSide = new Unity.Collections.NativeArray<float>(PathfindingNavMeshProcessor.MAX_PATH_SIZE, Unity.Collections.Allocator.Persistent);
                var results = new Unity.Collections.NativeArray<UnityEngine.Experimental.AI.NavMeshLocation>(PathfindingNavMeshProcessor.MAX_PATH_SIZE, Unity.Collections.Allocator.Persistent);
                var resultStatus = new Unity.Collections.NativeArray<int>(2, Unity.Collections.Allocator.TempJob);
                var job = new FindStraightPathJob() {
                    query = query,
                    from = from,
                    to = to,
                    pathInternal = pathInternal,
                    pathSize = pathSize,
                    results = results,
                    straightPathFlags = straightPathFlags,
                    vertexSide = vertexSide,
                    resultStatus = resultStatus,
                };
                job.Schedule().Complete();

                var pathStatus = (UnityEngine.Experimental.AI.PathQueryStatus)job.resultStatus[0];
                var cornerCount = job.resultStatus[1];
                resultStatus.Dispose();
                
                statLength = cornerCount;
                
                markerFindStraight.End();

                if (pathStatus == UnityEngine.Experimental.AI.PathQueryStatus.Success) {

                    /*for (int i = 1; i < cornerCount; ++i) {

                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(results[i].position, results[i - 1].position);

                    }*/
                    pathResult.pathStatus = pathStatus;
                    pathResult.results = results;
                    pathResult.corners = cornerCount;

                    if (cornerCount >= 2) {

                        path.navMeshPoints = PoolListCopyable<float3>.Spawn(cornerCount);
                        for (var i = 0; i < cornerCount; ++i) {

                            path.navMeshPoints.Add((float3)results[i].position);

                        }

                        if ((pathfindingLogLevel & LogLevel.Path) != 0) {
                        
                            var hash = 0;
                            for (var i = 0; i < cornerCount; ++i) {
                                hash ^= (int)(results[i].position.x * 1000000f);
                            }

                            UnityEngine.Debug.Log("Path hash X: " + hash);

                            hash = 0;
                            for (var i = 0; i < cornerCount; ++i) {
                                hash ^= (int)(results[i].position.y * 1000000f);
                            }

                            UnityEngine.Debug.Log("Path hash Y: " + hash);

                            hash = 0;
                            for (var i = 0; i < cornerCount; ++i) {
                                hash ^= (int)(results[i].position.z * 1000000f);
                            }

                            UnityEngine.Debug.Log("Path hash Z: " + hash);
                            
                        }

                        path.result = PathCompleteState.Complete;

                    } else {

                        path.result = PathCompleteState.NotExist;

                    }

                    pathResult.Dispose();

                } else {

                    path.result = PathCompleteState.NotExist;
                    results.Dispose();

                }

                vertexSide.Dispose();
                straightPathFlags.Dispose();
                pathInternal.Dispose();

            } else {

                path.result = PathCompleteState.NotExist;
                //Debug.LogWarning("Path result: " + result + ", performed: " + performed);

            }

            System.Diagnostics.Stopwatch swModifier = null;
            if ((pathfindingLogLevel & LogLevel.PathMods) != 0) {
                swModifier = System.Diagnostics.Stopwatch.StartNew();
            }

            if ((path.result & PathCompleteState.Complete) != 0) {

                path = pathModifier.Run(path, constraint);

            }

            if ((pathfindingLogLevel & LogLevel.Path) != 0) {

                Logger.Log(
                    $"Path result {path.result}, built in {(swPath.ElapsedTicks / (double)System.TimeSpan.TicksPerMillisecond).ToString("0.##")}ms. Path length: {statLength} (Visited: {statVisited})\nThread Index: {threadIndex}");

            }

            if ((pathfindingLogLevel & LogLevel.PathMods) != 0) {

                Logger.Log($"Path Mods: {swModifier.ElapsedMilliseconds}ms");

            }

            query.Dispose();
            
            Cache.Set(fromPoint, toPoint, constraint.GetKey(), navMeshGraph, path);

            return path;

        }

        //
        // Copyright (c) 2009-2010 Mikko Mononen memon@inside.org
        //
        // This software is provided 'as-is', without any express or implied
        // warranty.  In no event will the authors be held liable for any damages
        // arising from the use of this software.
        // Permission is granted to anyone to use this software for any purpose,
        // including commercial applications, and to alter it and redistribute it
        // freely, subject to the following restrictions:
        // 1. The origin of this software must not be misrepresented; you must not
        //    claim that you wrote the original software. If you use this software
        //    in a product, an acknowledgment in the product documentation would be
        //    appreciated but is not required.
        // 2. Altered source versions must be plainly marked as such, and must not be
        //    misrepresented as being the original software.
        // 3. This notice may not be removed or altered from any source distribution.
        //

        // The original source code has been modified by Unity Technologies and Zulfa Juniadi.

        [System.FlagsAttribute]
        public enum StraightPathFlags {

            Start = 0x01, // The vertex is the start position.
            End = 0x02, // The vertex is the end position.
            OffMeshConnection = 0x04, // The vertex is start of an off-mesh link.

        }

        public class PathUtils {

            public static float Perp2D(Vector3 u, Vector3 v) {
                return u.z * v.x - u.x * v.z;
            }

            public static void Swap(ref Vector3 a, ref Vector3 b) {
                var temp = a;
                a = b;
                b = temp;
            }

            // Retrace portals between corners and register if type of polygon changes
            public static int RetracePortals(UnityEngine.Experimental.AI.NavMeshQuery query, int startIndex, int endIndex,
                                             Unity.Collections.NativeSlice<UnityEngine.Experimental.AI.PolygonId> path, int n, Vector3 termPos,
                                             ref Unity.Collections.NativeArray<UnityEngine.Experimental.AI.NavMeshLocation> straightPath,
                                             ref Unity.Collections.NativeArray<StraightPathFlags> straightPathFlags, int maxStraightPath) {
                for (var k = startIndex; k < endIndex - 1; ++k) {
                    var type1 = query.GetPolygonType(path[k]);
                    var type2 = query.GetPolygonType(path[k + 1]);
                    if (type1 != type2) {
                        Vector3 l, r;
                        var status = query.GetPortalPoints(path[k], path[k + 1], out l, out r);
                        Unity.Mathematics.float3 cpa1, cpa2;
                        GeometryUtils.SegmentSegmentCPA(out cpa1, out cpa2, l, r, straightPath[n - 1].position, termPos);
                        straightPath[n] = query.CreateLocation(cpa1, path[k + 1]);

                        straightPathFlags[n] = type2 == UnityEngine.Experimental.AI.NavMeshPolyTypes.OffMeshConnection ? StraightPathFlags.OffMeshConnection : 0;
                        if (++n == maxStraightPath) {
                            return maxStraightPath;
                        }
                    }
                }

                straightPath[n] = query.CreateLocation(termPos, path[endIndex]);
                straightPathFlags[n] = query.GetPolygonType(path[endIndex]) == UnityEngine.Experimental.AI.NavMeshPolyTypes.OffMeshConnection
                                           ? StraightPathFlags.OffMeshConnection
                                           : 0;
                return ++n;
            }

            public static UnityEngine.Experimental.AI.PathQueryStatus FindStraightPath(UnityEngine.Experimental.AI.NavMeshQuery query, Vector3 startPos, Vector3 endPos,
                                                                                       Unity.Collections.NativeSlice<UnityEngine.Experimental.AI.PolygonId> path, int pathSize,
                                                                                       ref Unity.Collections.NativeArray<UnityEngine.Experimental.AI.NavMeshLocation> straightPath,
                                                                                       ref Unity.Collections.NativeArray<StraightPathFlags> straightPathFlags,
                                                                                       ref Unity.Collections.NativeArray<float> vertexSide, ref int straightPathCount,
                                                                                       int maxStraightPath) {
                if (!query.IsValid(path[0])) {
                    straightPath[0] = new UnityEngine.Experimental.AI.NavMeshLocation(); // empty terminator
                    return UnityEngine.Experimental.AI.PathQueryStatus.Failure; // | PathQueryStatus.InvalidParam;
                }

                straightPath[0] = query.CreateLocation(startPos, path[0]);

                straightPathFlags[0] = StraightPathFlags.Start;

                var apexIndex = 0;
                var n = 1;

                if (pathSize > 1) {
                    
                    var startPolyWorldToLocal = query.PolygonWorldToLocalMatrix(path[0]);
                    var apex = startPolyWorldToLocal.MultiplyPoint(startPos);
                    var left = new Vector3(0, 0, 0); // Vector3.zero accesses a static readonly which does not work in burst yet
                    var right = new Vector3(0, 0, 0);
                    var leftIndex = -1;
                    var rightIndex = -1;

                    for (var i = 1; i <= pathSize; ++i) {
                        var polyWorldToLocal = query.PolygonWorldToLocalMatrix(path[apexIndex]);

                        Vector3 vl, vr;
                        if (i == pathSize) {
                            vl = vr = polyWorldToLocal.MultiplyPoint(endPos);
                        } else {
                            var success = query.GetPortalPoints(path[i - 1], path[i], out vl, out vr);
                            if (!success) {
                                return UnityEngine.Experimental.AI.PathQueryStatus.Failure; // | PathQueryStatus.InvalidParam;
                            }

                            vl = polyWorldToLocal.MultiplyPoint(vl);
                            vr = polyWorldToLocal.MultiplyPoint(vr);
                        }

                        vl = vl - apex;
                        vr = vr - apex;

                        // Ensure left/right ordering
                        if (PathUtils.Perp2D(vl, vr) < 0) {
                            PathUtils.Swap(ref vl, ref vr);
                        }

                        // Terminate funnel by turning
                        if (PathUtils.Perp2D(left, vr) < 0) {
                            var polyLocalToWorld = query.PolygonLocalToWorldMatrix(path[apexIndex]);
                            var termPos = polyLocalToWorld.MultiplyPoint(apex + left);

                            n = PathUtils.RetracePortals(query, apexIndex, leftIndex, path, n, termPos, ref straightPath, ref straightPathFlags, maxStraightPath);
                            if (vertexSide.Length > 0) {
                                vertexSide[n - 1] = -1;
                            }

                            //Debug.Log("LEFT");

                            if (n == maxStraightPath) {
                                straightPathCount = n;
                                return UnityEngine.Experimental.AI.PathQueryStatus.Success; // | PathQueryStatus.BufferTooSmall;
                            }

                            apex = polyWorldToLocal.MultiplyPoint(termPos);
                            left.Set(0, 0, 0);
                            right.Set(0, 0, 0);
                            i = apexIndex = leftIndex;
                            continue;
                        }

                        if (PathUtils.Perp2D(right, vl) > 0) {
                            var polyLocalToWorld = query.PolygonLocalToWorldMatrix(path[apexIndex]);
                            var termPos = polyLocalToWorld.MultiplyPoint(apex + right);

                            n = PathUtils.RetracePortals(query, apexIndex, rightIndex, path, n, termPos, ref straightPath, ref straightPathFlags, maxStraightPath);
                            if (vertexSide.Length > 0) {
                                vertexSide[n - 1] = 1;
                            }

                            //Debug.Log("RIGHT");

                            if (n == maxStraightPath) {
                                straightPathCount = n;
                                return UnityEngine.Experimental.AI.PathQueryStatus.Success; // | PathQueryStatus.BufferTooSmall;
                            }

                            apex = polyWorldToLocal.MultiplyPoint(termPos);
                            left.Set(0, 0, 0);
                            right.Set(0, 0, 0);
                            i = apexIndex = rightIndex;
                            continue;
                        }

                        // Narrow funnel
                        if (PathUtils.Perp2D(left, vl) >= 0) {
                            left = vl;
                            leftIndex = i;
                        }

                        if (PathUtils.Perp2D(right, vr) <= 0) {
                            right = vr;
                            rightIndex = i;
                        }
                    }
                }

                // Remove the the next to last if duplicate point - e.g. start and end positions are the same
                // (in which case we have get a single point)
                if (n > 0 && straightPath[n - 1].position == endPos) {
                    n--;
                }

                n = PathUtils.RetracePortals(query, apexIndex, pathSize - 1, path, n, endPos, ref straightPath, ref straightPathFlags, maxStraightPath);
                if (vertexSide.Length > 0) {
                    vertexSide[n - 1] = 0;
                }

                if (n == maxStraightPath) {
                    straightPathCount = n;
                    return UnityEngine.Experimental.AI.PathQueryStatus.Success; // | PathQueryStatus.BufferTooSmall;
                }

                // Fix flag for final path point
                straightPathFlags[n - 1] = StraightPathFlags.End;

                straightPathCount = n;
                return UnityEngine.Experimental.AI.PathQueryStatus.Success;
            }

        }

        public class GeometryUtils {

            // Calculate the closest point of approach for line-segment vs line-segment.
            public static bool SegmentSegmentCPA(out Unity.Mathematics.float3 c0, out Unity.Mathematics.float3 c1, Unity.Mathematics.float3 p0, Unity.Mathematics.float3 p1,
                                                 Unity.Mathematics.float3 q0, Unity.Mathematics.float3 q1) {
                var u = p1 - p0;
                var v = q1 - q0;
                var w0 = p0 - q0;

                float a = Unity.Mathematics.math.dot(u, u);
                float b = Unity.Mathematics.math.dot(u, v);
                float c = Unity.Mathematics.math.dot(v, v);
                float d = Unity.Mathematics.math.dot(u, w0);
                float e = Unity.Mathematics.math.dot(v, w0);

                var den = a * c - b * b;
                float sc, tc;

                if (den == 0) {
                    sc = 0;
                    tc = d / b;

                    // todo: handle b = 0 (=> a and/or c is 0)
                } else {
                    sc = (b * e - c * d) / (a * c - b * b);
                    tc = (a * e - b * d) / (a * c - b * b);
                }

                c0 = Unity.Mathematics.math.lerp(p0, p1, sc);
                c1 = Unity.Mathematics.math.lerp(q0, q1, tc);

                return den != 0;
            }

        }

    }

}
