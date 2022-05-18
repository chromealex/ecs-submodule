using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

#if FIXED_POINT_MATH
using MATH = ME.ECS.fpmath;
using FLOAT = ME.ECS.fp;
using FLOAT2 = ME.ECS.fp2;
using FLOAT3 = ME.ECS.fp3;
using FLOAT4 = ME.ECS.fp4;
using QUATERNION = ME.ECS.fpquaternion;
#else
using MATH = Unity.Mathematics.math;
using FLOAT = System.Single;
using FLOAT2 = UnityEngine.Vector2;
using FLOAT3 = UnityEngine.Vector3;
using FLOAT4 = UnityEngine.Vector4;
using QUATERNION = UnityEngine.Quaternion;
#endif

namespace ME.ECS.Collections {

    public static class NativeQuadTreeUtils {

        public static void PrepareTick(in AABB2D mapSize, NativeArray<QuadElement<Entity>> items, int itemsCount) {
            
            NativeQuadTreeUtils<Entity>.PrepareTick(mapSize, items, itemsCount);

        }

        public static void EndTick() {

            NativeQuadTreeUtils<Entity>.EndTick();

        }

        #if FIXED_POINT_MATH
        public static void GetResults(in UnityEngine.Vector2 position, float radius, Unity.Collections.NativeList<QuadElement<Entity>> results) {

            NativeQuadTreeUtils<Entity>.GetResults(position, radius, results);

        }
        #endif

        public static void GetResults(in FLOAT2 position, FLOAT radius, Unity.Collections.NativeList<QuadElement<Entity>> results) {

            NativeQuadTreeUtils<Entity>.GetResults(position, radius, results);

        }

        public static void GetResults(in FLOAT2 position, FLOAT2 size, Unity.Collections.NativeList<QuadElement<Entity>> results) {

            NativeQuadTreeUtils<Entity>.GetResults(position, size, results);

        }

    }
    
    public static class NativeQuadTreeUtils<T> where T : unmanaged {

        private static NativeQuadTree<T> tempTree;
        private static JobHandle jobHandle;
        
        public static void PrepareTick(in AABB2D mapSize, NativeArray<QuadElement<T>> items, int itemsCount) {
            
            if (NativeQuadTreeUtils<T>.tempTree.isCreated == true) {
                throw new System.Exception("Temp tree collection must been disposed");
            }
            NativeQuadTreeUtils<T>.tempTree = new NativeQuadTree<T>(mapSize, Unity.Collections.Allocator.TempJob);
            NativeQuadTreeUtils<T>.jobHandle = new QuadTreeJobs.ClearJob<T>() {
                quadTree = NativeQuadTreeUtils<T>.tempTree,
                elements = items,
                elementsCount = itemsCount,
            }.Schedule();

        }

        public static void EndTick() {

            if (NativeQuadTreeUtils<T>.jobHandle.IsCompleted == false) NativeQuadTreeUtils<T>.jobHandle.Complete();
            NativeQuadTreeUtils<T>.jobHandle = default;
            NativeQuadTreeUtils<T>.tempTree.Dispose();

        }

        public static void GetResults(in FLOAT2 position, FLOAT radius, Unity.Collections.NativeList<QuadElement<T>> results) {

            if (NativeQuadTreeUtils<T>.tempTree.isCreated == false) {
                throw new System.Exception("Temp tree collection has been disposed");
            }
            new QuadTreeJobs.QueryRadiusJob<T>() {
                quadTree = NativeQuadTreeUtils<T>.tempTree,
                bounds = new AABB2D(position, new FLOAT2(radius, radius)),
                radius = radius,
                results = results,
            }.Schedule(NativeQuadTreeUtils<T>.jobHandle).Complete();
            NativeQuadTreeUtils<T>.jobHandle = default;

        }

        public static void GetResults(in FLOAT2 position, in FLOAT2 size, Unity.Collections.NativeList<QuadElement<T>> results) {

            if (NativeQuadTreeUtils<T>.tempTree.isCreated == false) {
                throw new System.Exception("Temp tree collection has been disposed");
            }
            new QuadTreeJobs.QueryJob<T>() {
                quadTree = NativeQuadTreeUtils<T>.tempTree,
                bounds = new AABB2D(position, size),
                results = results,
            }.Schedule(NativeQuadTreeUtils<T>.jobHandle).Complete();
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
            public FLOAT radius;
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