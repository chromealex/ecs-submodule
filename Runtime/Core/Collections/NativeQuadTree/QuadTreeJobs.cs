using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace ME.ECS.Collections {

    public static class NativeQuadTreeUtils {

        public static void GetResults(in AABB2D mapSize, in UnityEngine.Vector2 position, float radius, Unity.Collections.NativeList<QuadElement<Entity>> results,
                                      NativeArray<QuadElement<Entity>> items, int itemsCount) {

            var tree = new NativeQuadTree<Entity>(mapSize, Unity.Collections.Allocator.Temp);
            {
                new QuadTreeJobs.QueryJob<Entity>() {
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
        public struct QueryJob<T> : IJob where T : unmanaged {

            public NativeQuadTree<T> quadTree;
            [ReadOnly]
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

        /// <summary>
        /// Bulk insert many items into the tree
        /// </summary>
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public struct AddBulkJob<T> : IJob where T : unmanaged {

            [ReadOnly]
            public NativeArray<QuadElement<T>> Elements;

            public NativeQuadTree<T> QuadTree;

            public void Execute() {
                this.QuadTree.ClearAndBulkInsert(this.Elements, this.Elements.Length);
            }

        }

        /// <summary>
        /// Example on how to do a range query, it's better to write your own and do many queries in a batch
        /// </summary>
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public struct RangeQueryJob<T> : IJob where T : unmanaged {

            [ReadOnly]
            public AABB2D Bounds;

            public NativeQuadTree<T> QuadTree;

            public NativeList<QuadElement<T>> Results;

            public void Execute() {
                this.QuadTree.RangeQuery(this.Bounds, this.Results);
            }

        }

    }

}