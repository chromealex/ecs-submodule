using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace ME.ECS.Collections {

    public unsafe partial struct NativeQuadTree<T> where T : unmanaged {

        private struct QuadTreeRangeQuery {

            private NativeQuadTree<T> tree;

            private UnsafeList* fastResults;
            private int count;

            private AABB2D bounds;

            public void Query(NativeQuadTree<T> tree, AABB2D bounds, NativeList<QuadElement<T>> results) {
                this.tree = tree;
                this.bounds = bounds;
                this.count = 0;

                // Get pointer to inner list data for faster writing
                this.fastResults = (UnsafeList*)NativeListUnsafeUtility.GetInternalListDataPtrUnchecked(ref results);

                this.RecursiveRangeQuery(tree.bounds, false, 1, 1);

                this.fastResults->Length = this.count;
            }

            public void RecursiveRangeQuery(AABB2D parentBounds, bool parentContained, int prevOffset, int depth) {
                if (this.count + 4 * this.tree.maxLeafElements > this.fastResults->Capacity) {
                    this.fastResults->Resize<QuadElement<T>>(math.max(this.fastResults->Capacity * 2, this.count + 4 * this.tree.maxLeafElements));
                }

                var depthSize = LookupTables.DepthSizeLookup[this.tree.maxDepth - depth + 1];
                for (var l = 0; l < 4; l++) {
                    var childBounds = NativeQuadTree<T>.QuadTreeRangeQuery.GetChildBounds(parentBounds, l);

                    var contained = parentContained;
                    if (!contained) {
                        if (this.bounds.Contains(childBounds)) {
                            contained = true;
                        } else if (!this.bounds.Intersects(childBounds)) {
                            continue;
                        }
                    }


                    var at = prevOffset + l * depthSize;

                    var elementCount = this.tree.lookup[at]; //UnsafeUtility.ReadArrayElement<int>(tree.lookup->Ptr, at);

                    if (elementCount > this.tree.maxLeafElements && depth < this.tree.maxDepth) {
                        this.RecursiveRangeQuery(childBounds, contained, at + 1, depth + 1);
                    } else if (elementCount != 0) {

                        var node = this.tree.nodes[at]; //UnsafeUtility.ReadArrayElement<QuadNode>(tree.nodes->Ptr, at);

                        if (contained) {

                            var index = (void*)((IntPtr)this.tree.elements.GetUnsafePtr() + node.firstChildIndex * UnsafeUtility.SizeOf<QuadElement<T>>());

                            //NativeArrayUtils.Copy(tree.elements, node.firstChildIndex, this.fastResults, count, node.count);
                            UnsafeUtility.MemCpy((void*)((IntPtr)this.fastResults->Ptr + this.count * UnsafeUtility.SizeOf<QuadElement<T>>()), index,
                                                 node.count * UnsafeUtility.SizeOf<QuadElement<T>>());
                            this.count += node.count;
                        } else {
                            for (var k = 0; k < node.count; k++) {

                                var element = this.tree.elements[
                                    node.firstChildIndex + k]; //UnsafeUtility.ReadArrayElement<QuadElement<T>>(tree.elements->Ptr, node.firstChildIndex + k);
                                if (this.bounds.Contains(element.pos)) {
                                    UnsafeUtility.WriteArrayElement(this.fastResults->Ptr, this.count++, element);
                                }
                            }
                        }
                    }
                }
            }

            private static AABB2D GetChildBounds(AABB2D parentBounds, int childZIndex) {
                var half = parentBounds.Extents.x * .5f;

                switch (childZIndex) {
                    case 0:
                        return new AABB2D(new float2(parentBounds.Center.x - half, parentBounds.Center.y + half), half);

                    case 1:
                        return new AABB2D(new float2(parentBounds.Center.x + half, parentBounds.Center.y + half), half);

                    case 2:
                        return new AABB2D(new float2(parentBounds.Center.x - half, parentBounds.Center.y - half), half);

                    case 3:
                        return new AABB2D(new float2(parentBounds.Center.x + half, parentBounds.Center.y - half), half);

                    default:
                        throw new Exception();
                }
            }

        }

    }

}