using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS.Collections {

    public static class NativeQuadTreeUtils {

        public static void PrepareTick(in AABB2D mapSize, NativeArray<QuadElement<Entity>> items, int itemsCount) {
            
            NativeQuadTreeUtils<Entity>.PrepareTick(mapSize, items, itemsCount);

        }

        public static void EndTick() {

            NativeQuadTreeUtils<Entity>.EndTick();

        }

        public static void GetResults(in float2 position, tfloat radius, Unity.Collections.NativeList<QuadElement<Entity>> results, bool jobEnable = false) {

            NativeQuadTreeUtils<Entity>.GetResults(position, radius, results, jobEnable);

            for (int i = results.Length - 1; i >= 0; --i) {

                if (results[i].element.IsAlive() == false) {
                    
                    results.RemoveAtSwapBack(i);
                    
                }
                
            }

        }

        public static void GetResults(in float2 position, float2 size, Unity.Collections.NativeList<QuadElement<Entity>> results, bool jobEnable = false) {

            NativeQuadTreeUtils<Entity>.GetResults(position, size, results, jobEnable);

            for (int i = results.Length - 1; i >= 0; --i) {

                if (results[i].element.IsAlive() == false) {
                    
                    results.RemoveAtSwapBack(i);
                    
                }
                
            }

        }

    }
    
    public static class NativeQuadTreeUtils<T> where T : unmanaged {

        private static NativeQuadTree<T> tempTree;
        private static JobHandle jobHandle;
        
        public static void PrepareTick(in AABB2D mapSize, NativeArray<QuadElement<T>> items, int itemsCount) {
            
            if (itemsCount > items.Length) {
                itemsCount = items.Length;
                UnityEngine.Debug.LogWarningFormat("ClearAndBulkInsert: {0} > {1}", itemsCount, items.Length);
            }

            if (NativeQuadTreeUtils<T>.tempTree.isCreated == true) {
                UnityEngine.Debug.LogError("Temp tree collection must been disposed");
                NativeQuadTreeUtils<T>.EndTick();
            }
            //UnityEngine.Debug.Log("PrepareTick: " + Worlds.current.GetCurrentTick());
            NativeQuadTreeUtils<T>.tempTree = new NativeQuadTree<T>(mapSize, Unity.Collections.Allocator.TempJob, maxDepth: 4);
            NativeQuadTreeUtils<T>.jobHandle = new QuadTreeJobs.ClearJob<T>() {
                quadTree = NativeQuadTreeUtils<T>.tempTree,
                elements = items,
                elementsCount = itemsCount,
            }.Schedule();

        }

        public static void EndTick() {

            if (NativeQuadTreeUtils<T>.jobHandle.IsCompleted == false) NativeQuadTreeUtils<T>.jobHandle.Complete();
            NativeQuadTreeUtils<T>.jobHandle = default;
            if (NativeQuadTreeUtils<T>.tempTree.isCreated == true) NativeQuadTreeUtils<T>.tempTree.Dispose();
            //if (Worlds.current != null && Worlds.current.GetState() != null) UnityEngine.Debug.Log("EndTick: " + Worlds.current.GetCurrentTick());

        }

        public static void GetResults(in float2 position, tfloat radius, Unity.Collections.NativeList<QuadElement<T>> results, bool jobEnable = false) {

            if (NativeQuadTreeUtils<T>.tempTree.isCreated == false) {
                throw new System.Exception("Temp tree collection has been disposed");
            }

            var job = new QuadTreeJobs.QueryRadiusJob<T>() {
                quadTree = NativeQuadTreeUtils<T>.tempTree,
                bounds = new AABB2D(position, new float2(radius, radius)),
                radius = radius,
                results = results,
            };

            if (jobEnable == true) {
                job.Schedule(NativeQuadTreeUtils<T>.jobHandle).Complete();
            } else {
                NativeQuadTreeUtils<T>.jobHandle.Complete();
                job.Execute();
            }
            NativeQuadTreeUtils<T>.jobHandle = default;

        }

        public static void GetResults(in float2 position, in float2 size, Unity.Collections.NativeList<QuadElement<T>> results, bool jobEnable = false) {

            if (NativeQuadTreeUtils<T>.tempTree.isCreated == false) {
                throw new System.Exception("Temp tree collection has been disposed");
            }

            var job = new QuadTreeJobs.QueryJob<T>() {
                quadTree = NativeQuadTreeUtils<T>.tempTree,
                bounds = new AABB2D(position, size),
                results = results,
            };
            
            if (jobEnable == true) {
                job.Schedule(NativeQuadTreeUtils<T>.jobHandle).Complete();
            } else {
                NativeQuadTreeUtils<T>.jobHandle.Complete();
                job.Execute();
            }
            NativeQuadTreeUtils<T>.jobHandle = default;

        }

    }

    public static class QuadTreeJobs {

        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public struct ClearJob<T> : IJob where T : unmanaged {

            public NativeQuadTree<T> quadTree;
            public NativeArray<QuadElement<T>> elements;
            public int elementsCount;

            public void Execute() {
                this.quadTree.ClearAndBulkInsert(this.elements, this.elementsCount);
            }

        }
        
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public struct QueryJob<T> : IJob where T : unmanaged {

            public NativeQuadTree<T> quadTree;
            public AABB2D bounds;
            public NativeList<QuadElement<T>> results;

            public void Execute() {
                this.quadTree.RangeQuery(this.bounds, this.results);
            }

        }

        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public struct QueryRadiusJob<T> : IJob where T : unmanaged {

            public NativeQuadTree<T> quadTree;
            public AABB2D bounds;
            public tfloat radius;
            public NativeList<QuadElement<T>> results;

            public void Execute() {
                this.quadTree.RangeRadiusQuery(this.bounds, this.radius, this.results);
            }

        }

        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public struct QueryJobWithClear<T> : IJob where T : unmanaged {

            public NativeQuadTree<T> quadTree;
            public NativeArray<QuadElement<T>> elements;
            public int elementsCount;
            public AABB2D bounds;
            public NativeList<QuadElement<T>> results;

            public void Execute() {
                this.quadTree.ClearAndBulkInsert(this.elements, this.elementsCount);
                this.quadTree.RangeQuery(this.bounds, this.results);
            }

        }

    }

}
