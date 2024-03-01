using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
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

        public static void Construct(in AABB2D mapSize) {
            
            NativeQuadTreeUtils<Entity>.Construct(in mapSize);

        }

        public static void PrepareTick(in AABB2D mapSize, NativeArray<QuadElement<Entity>> items, int itemsCount) {
            
            NativeQuadTreeUtils<Entity>.PrepareTick(mapSize, items, itemsCount);

        }

        public static void EndTick() {

            NativeQuadTreeUtils<Entity>.EndTick();

        }

        public static void Dispose() {

            NativeQuadTreeUtils<Entity>.Dispose();

        }

        public static void GetResults(in float2 position, tfloat radius, Unity.Collections.NativeList<QuadElement<Entity>> results) {

            NativeQuadTreeUtils<Entity>.GetResults(position, radius, results);

            var sqRadius = radius * radius;
            for (int i = results.Length - 1; i >= 0; --i) {

                var elem = results[i];
                if (elem.element.IsAlive() == false ||
                    math.distancesq(elem.pos, position) > sqRadius) {
                    
                    results.RemoveAtSwapBack(i);
                    
                }
                
            }

        }

        public static void GetResults(in ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator, in ME.ECS.FiltersArchetype.FiltersArchetypeStorage storage, in float2 position, tfloat radius, Unity.Collections.NativeList<QuadElement<Entity>> results) {

            NativeQuadTreeUtils<Entity>.GetResults(position, radius, results);

            var sqRadius = radius * radius;
            for (int i = results.Length - 1; i >= 0; --i) {

                var elem = results[i];
                if (elem.element.IsAlive(in allocator, in storage) == false ||
                    math.distancesq(elem.pos, position) > sqRadius) {
                    
                    results.RemoveAtSwapBack(i);
                    
                }
                
            }

        }

    }
    
    public class NativeQuadTreeUtils<T> where T : unmanaged {

        public static readonly SharedStatic<NativeQuadTree<T>> tempTree = SharedStatic<NativeQuadTree<T>>.GetOrCreate<NativeQuadTreeUtils<T>>();
        //private static readonly NativeQuadTree<T> tempTree;
        private static NativeArray<QuadElement<T>> items;
        public static int itemsCount;

        public static void Construct(in AABB2D mapSize) {
            
            NativeQuadTreeUtils<T>.tempTree.Data = new NativeQuadTree<T>(mapSize, Unity.Collections.Allocator.Persistent, maxDepth: 4);

        }
        
        public static void PrepareTick(in AABB2D mapSize, NativeArray<QuadElement<T>> items, int itemsCount) {
            
            if (NativeQuadTreeUtils<T>.tempTree.Data.isCreated == false) {
                throw new System.Exception("Temp tree collection has been disposed");
            }

            if (itemsCount > items.Length) {
                itemsCount = items.Length;
                UnityEngine.Debug.LogWarningFormat("ClearAndBulkInsert: {0} > {1}", itemsCount, items.Length);
            }

            NativeQuadTreeUtils<T>.items = items;
            NativeQuadTreeUtils<T>.itemsCount = itemsCount;

            new QuadTreeJobs.ClearJob<T>() {
                quadTree = NativeQuadTreeUtils<T>.tempTree.Data,
                elements = items,
                elementsCount = itemsCount,
            }.Execute();

        }

        public static void EndTick() {
            
        }

        public static void Dispose() {
            
            if (NativeQuadTreeUtils<T>.tempTree.Data.isCreated == true) NativeQuadTreeUtils<T>.tempTree.Data.Dispose();
            
        }

        public static unsafe void GetResults(in float2 position, tfloat radius, Unity.Collections.NativeList<QuadElement<T>> results) {

            if (NativeQuadTreeUtils<T>.tempTree.Data.isCreated == false) {
                throw new System.Exception("Temp tree collection has been disposed");
            }

            /*var marker = new Unity.Profiling.ProfilerMarker("GetNearestUnitTarget");
            marker.Begin();
            results.AddRange(NativeQuadTreeUtils<T>.items.GetUnsafeReadOnlyPtr(), NativeQuadTreeUtils<T>.itemsCount);
            marker.End();*/
            
            new QuadTreeJobs.QueryRadiusJob<T>() {
                quadTree = NativeQuadTreeUtils<T>.tempTree.Data,
                bounds = new AABB2D(position, new float2(radius, radius)),
                radius = radius,
                results = results,
            }.Execute();
            
        }

        public static void GetResults(in float2 position, in float2 size, Unity.Collections.NativeList<QuadElement<T>> results) {

            if (NativeQuadTreeUtils<T>.tempTree.Data.isCreated == false) {
                throw new System.Exception("Temp tree collection has been disposed");
            }

            new QuadTreeJobs.QueryJob<T>() {
                quadTree = NativeQuadTreeUtils<T>.tempTree.Data,
                bounds = new AABB2D(position, size),
                results = results,
            }.Execute();

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

    }

}
