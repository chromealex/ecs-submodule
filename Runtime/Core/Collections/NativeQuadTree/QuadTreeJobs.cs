using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace ME.ECS.Collections {

    public static class NativeQuadTreeUtils {

        private static NativeQuadTree<Entity> tempTree;
        
        public static void PrepareTick(in AABB2D mapSize, NativeArray<QuadElement<Entity>> items, int itemsCount) {
            
            NativeQuadTreeUtils.tempTree = new NativeQuadTree<Entity>(mapSize, Unity.Collections.Allocator.Temp);
            new QuadTreeJobs.ClearJob<Entity>() {
                quadTree = NativeQuadTreeUtils.tempTree,
                elements = items,
                elementsCount = itemsCount,
            }.Schedule();

        }

        public static void EndTick() {

            NativeQuadTreeUtils.tempTree.Dispose();

        }

        public static void GetResults(in UnityEngine.Vector2 position, float radius, Unity.Collections.NativeList<QuadElement<Entity>> results) {

            new QuadTreeJobs.QueryJob<Entity>() {
                quadTree = NativeQuadTreeUtils.tempTree,
                bounds = new AABB2D(position, new Unity.Mathematics.float2(radius, radius)),
                results = results,
            }.Schedule().Complete();

        }

        public static void GetResults(in AABB2D mapSize, in UnityEngine.Vector2 position, float radius, Unity.Collections.NativeList<QuadElement<Entity>> results,
                                      NativeArray<QuadElement<Entity>> items, int itemsCount) {

            var tree = new NativeQuadTree<Entity>(mapSize, Unity.Collections.Allocator.Temp);
            {
                new QuadTreeJobs.QueryJobWithClear<Entity>() {
                    quadTree = tree,
                    elements = items,
                    elementsCount = itemsCount,
                    bounds = new AABB2D(position, new Unity.Mathematics.float2(radius, radius)),
                    results = results,
                }.Schedule().Complete();
            }
            tree.Dispose();

        }

    }

    /// <summary>
    /// Examples on jobs for the NativeQuadTree
    /// </summary>
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

            [ReadOnly]
            public AABB2D bounds;
            public NativeList<QuadElement<T>> results;

            public void Execute() {
                this.quadTree.RangeQuery(this.bounds, this.results);
            }

        }

        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public struct QueryJobWithClear<T> : IJob where T : unmanaged {

            public NativeQuadTree<T> quadTree;
            public NativeArray<QuadElement<T>> elements;
            public int elementsCount;

            [ReadOnly]
            public AABB2D bounds;
            public NativeList<QuadElement<T>> results;

            public void Execute() {
                this.quadTree.ClearAndBulkInsert(this.elements, this.elementsCount);
                this.quadTree.RangeQuery(this.bounds, this.results);
            }

        }

    }

}