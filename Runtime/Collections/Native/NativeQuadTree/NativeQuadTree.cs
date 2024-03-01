using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif
using ME.ECS.Collections.LowLevel;
using ME.ECS.Collections.LowLevel.Unsafe;

namespace ME.ECS.Collections {

    // Represents an element node in the quadtree.
    public struct QuadElement<T> where T : unmanaged {

        public float2 pos;
        public T element;

    }

    internal struct QuadNode {

        // Points to this node's first child index in elements
        public int firstChildIndex;

        // Number of elements in the leaf
        public short count;
        public bool isLeaf;

    }

    /// <summary>
    /// A QuadTree aimed to be used with Burst, supports fast bulk insertion and querying.
    ///
    /// TODO:
    /// - Better test coverage
    /// - Automated depth / bounds / max leaf elements calculation
    /// </summary>
    public unsafe partial struct NativeQuadTree<T> : IDisposable where T : unmanaged {

        // Data
        [NativeDisableUnsafePtrRestriction]
        private UnsafeList<QuadElement<T>>* elements;

        [NativeDisableUnsafePtrRestriction]
        private MemArrayAllocator<int> lookup;

        [NativeDisableUnsafePtrRestriction]
        private MemArrayAllocator<QuadNode> nodes;

        private int elementsCount;

        public bool isCreated;
        private int maxDepth;
        private short maxLeafElements;

        private AABB2D bounds; // NOTE: Currently assuming uniform

        private Allocator allocator;

        /// <summary>
        /// Create a new QuadTree.
        /// - Ensure the bounds are not way bigger than needed, otherwise the buckets are very off. Probably best to calculate bounds
        /// - The higher the depth, the larger the overhead, it especially goes up at a depth of 7/8
        /// </summary>
        public NativeQuadTree(AABB2D bounds, Allocator allocator = Allocator.Temp, int maxDepth = 4, short maxLeafElements = 16, int initialElementsCapacity = 256) : this() {
            this.allocator = allocator;
            this.bounds = bounds;
            this.maxDepth = maxDepth;
            this.maxLeafElements = maxLeafElements;
            this.elementsCount = 0;
            this.isCreated = true;

            if (maxDepth > 8) {
                // Currently no support for higher depths, the morton code lookup tables would have to support it
                throw new InvalidOperationException();
            }

            // Allocate memory for every depth, the nodes on all depths are stored in a single continuous array
            var totalSize = LookupTables.DepthSizeLookup[maxDepth + 1];

            ref var tempAllocator = ref StaticAllocators.GetAllocator(AllocatorType.Temp);
            this.lookup = new MemArrayAllocator<int>(ref tempAllocator, totalSize);
                //new NativeArray<int>(
                //totalSize, allocator); //UnsafeList.Create(UnsafeUtility.SizeOf<int>(), UnsafeUtility.AlignOf<int>(), totalSize, allocator, NativeArrayOptions.ClearMemory);

            this.nodes = new MemArrayAllocator<QuadNode>(ref tempAllocator, totalSize);
                //totalSize, allocator); //UnsafeList.Create(UnsafeUtility.SizeOf<QuadNode>(), UnsafeUtility.AlignOf<QuadNode>(), totalSize, allocator, NativeArrayOptions.ClearMemory);

            this.elements = UnsafeList<QuadElement<T>>.Create(initialElementsCapacity, allocator);
        }

        public void ClearAndBulkInsert(NativeArray<QuadElement<T>> incomingElements, int incomingElementsLength) {

            // Always have to clear before bulk insert as otherwise the lookup and node allocations need to account
            // for existing data.
            this.Clear();

            // Resize if needed
            if (this.elements->Length < this.elementsCount + incomingElementsLength) {
                //NativeArrayUtils.Resize(math.max(incomingElementsLength, this.elements.Length * 2), ref this.elements, this.allocator);
                this.elements->Resize(math.max(incomingElementsLength, this.elements->Capacity * 2), NativeArrayOptions.UninitializedMemory);
            }

            // Prepare morton codes
            var mortonCodes = new NativeArray<int>(incomingElementsLength, Allocator.Temp);
            var depthExtentsScaling = LookupTables.DepthLookup[this.maxDepth] / this.bounds.extents;
            for (var i = 0; i < incomingElementsLength; i++) {
                var incPos = incomingElements[i].pos;

                if (this.bounds.Contains(incPos) == false) {
                    UnityEngine.Debug.LogError("NativeQuadTree element out of bounds");
                    incPos = math.clamp(incPos, this.bounds.min, this.bounds.max);
                }
                
                incPos -= this.bounds.center; // Offset by center
                incPos.y = -incPos.y; // World -> array
                var pos = (incPos + this.bounds.extents) * .5f; // Make positive
                // Now scale into available space that belongs to the depth
                pos *= depthExtentsScaling;
                // And interleave the bits for the morton code
                mortonCodes[i] = LookupTables.MortonLookup[(int)pos.x] | (LookupTables.MortonLookup[(int)pos.y] << 1);
            }

            ref var tempAllocator = ref StaticAllocators.GetAllocator(AllocatorType.Temp);
            // Index total child element count per node (total, so parent's counts include those of child nodes)
            for (var i = 0; i < mortonCodes.Length; i++) {
                var atIndex = 0;
                var val = mortonCodes[i];
                for (var depth = 0; depth <= this.maxDepth; depth++) {
                    // Increment the node on this depth that this element is contained in
                    //(*(int*)((IntPtr)this.lookup->Ptr + atIndex * sizeof(int)))++;
                    ++this.lookup[in tempAllocator, atIndex];
                    atIndex = this.IncrementIndex(depth, val, atIndex);
                }
            }

            // Prepare the tree leaf nodes
            this.RecursivePrepareLeaves(ref tempAllocator, 1, 1);

            // Add elements to leaf nodes
            for (var i = 0; i < incomingElementsLength; i++) {
                var atIndex = 0;
                var val = mortonCodes[i];
                for (var depth = 0; depth <= this.maxDepth; depth++) {
                    var node = this.nodes[in tempAllocator, atIndex]; //UnsafeUtility.ReadArrayElement<QuadNode>(this.nodes->Ptr, atIndex);
                    if (node.isLeaf) {
                        // We found a leaf, add this element to it and move to the next element
                        UnsafeUtility.WriteArrayElement(this.elements->Ptr, node.firstChildIndex + node.count, incomingElements[i]);
                        //this.elements[node.firstChildIndex + node.count] = incomingElements[i];
                        node.count++;
                        this.nodes[in tempAllocator, atIndex] = node;
                        //UnsafeUtility.WriteArrayElement(this.nodes->Ptr, atIndex, node);
                        break;
                    }

                    // No leaf found, we keep going deeper until we find one
                    atIndex = this.IncrementIndex(depth, val, atIndex);
                }
            }

            mortonCodes.Dispose();
        }

        private int IncrementIndex(int depth, int val, int atIndex) {
            var atDepth = math.max(0, this.maxDepth - depth);
            // Shift to the right and only get the first two bits
            var shiftedMortonCode = (val >> ((atDepth - 1) * 2)) & 0b11;
            // so the index becomes that... (0,1,2,3)
            atIndex += LookupTables.DepthSizeLookup[atDepth] * shiftedMortonCode;
            atIndex++; // offset for self
            return atIndex;
        }

        private void RecursivePrepareLeaves(ref ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator tempAllocator, int prevOffset, int depth) {
            for (var l = 0; l < 4; l++) {
                var at = prevOffset + l * LookupTables.DepthSizeLookup[this.maxDepth - depth + 1];

                var elementCount = this.lookup[in tempAllocator, at]; //UnsafeUtility.ReadArrayElement<int>(this.lookup->Ptr, at);

                if (elementCount > this.maxLeafElements && depth < this.maxDepth) {
                    // There's more elements than allowed on this node so keep going deeper
                    this.RecursivePrepareLeaves(ref tempAllocator, at + 1, depth + 1);
                } else if (elementCount != 0) {
                    // We either hit max depth or there's less than the max elements on this node, make it a leaf
                    var node = new QuadNode { firstChildIndex = this.elementsCount, count = 0, isLeaf = true };
                    //UnsafeUtility.WriteArrayElement(this.nodes->Ptr, at, node);
                    this.nodes[in tempAllocator, at] = node;
                    this.elementsCount += elementCount;
                }
            }
        }

        public void RangeQuery(AABB2D bounds, NativeList<QuadElement<T>> results) {
            new QuadTreeRangeQuery().Query(this, bounds, results);
        }

        public void RangeRadiusQuery(AABB2D bounds, tfloat radius, NativeList<QuadElement<T>> results) {
            new QuadTreeRangeQuery().RadiusQuery(this, bounds, radius, results);
        }

        public void Clear() {
            ref var tempAllocator = ref StaticAllocators.GetAllocator(AllocatorType.Temp);
            //this.lookup.Clear();
            //this.nodes.Clear();
            //this.elements.Clear();
            this.lookup.Clear(in tempAllocator);
            this.nodes.Clear(in tempAllocator);
            //NativeArrayUtils.Clear(this.elements);
            //UnsafeUtility.MemClear(this.lookup->Ptr, this.lookup->Capacity * UnsafeUtility.SizeOf<int>());
            //UnsafeUtility.MemClear(this.nodes->Ptr, this.nodes->Capacity * UnsafeUtility.SizeOf<QuadNode>());
            UnsafeUtility.MemClear(this.elements->Ptr, this.elements->Capacity * UnsafeUtility.SizeOf<QuadElement<T>>());
            this.elementsCount = 0;
        }

        public void Dispose() {
            ref var tempAllocator = ref StaticAllocators.GetAllocator(AllocatorType.Temp);
            //this.elements.Dispose();
            //this.elements = default;
            this.lookup.Dispose(ref tempAllocator);
            this.lookup = default;
            this.nodes.Dispose(ref tempAllocator);
            this.nodes = default;
            this.isCreated = false;
            UnsafeList<QuadElement<T>>.Destroy(this.elements);
            this.elements = null;
            //UnsafeList.Destroy(this.lookup);
            //this.lookup = null;
            //UnsafeList.Destroy(this.nodes);
            //this.nodes = null;
        }

    }

}
