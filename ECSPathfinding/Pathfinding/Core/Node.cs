#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif
using UnityEngine;

namespace ME.ECS.Pathfinding {

    [System.Serializable]
    public abstract class Node : IPoolableRecycle {

        [System.Serializable]
        public struct Connection {

            public static Connection NoConnection => new Connection() { index = -1 };

            public int index;
            public float cost;

        }

        public Graph graph;
        public int index;
        public Vector3 worldPosition;
        public float penalty;
        public bool walkable;
        public int area;
        public int tag;
        public float height;

        internal readonly Node[] parent = new Node[Pathfinding.THREADS_COUNT];
        internal readonly float[] startToCurNodeLen = new float[Pathfinding.THREADS_COUNT];
        internal readonly bool[] isOpened = new bool[Pathfinding.THREADS_COUNT];
        internal readonly bool[] isClosed = new bool[Pathfinding.THREADS_COUNT];

        protected Node() { }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public virtual void CopyFrom(Node other) {

            this.graph = other.graph;
            this.index = other.index;
            this.worldPosition = other.worldPosition;
            this.penalty = other.penalty;
            this.walkable = other.walkable;
            this.area = other.area;
            this.tag = other.tag;
            this.height = other.height;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public virtual void OnRecycle() {

            this.graph = null;
            this.index = default;
            this.worldPosition = default;
            this.penalty = default;
            this.walkable = default;
            this.area = default;
            this.tag = default;
            this.height = default;

            System.Array.Clear(this.parent, 0, this.parent.Length);
            System.Array.Clear(this.startToCurNodeLen, 0, this.startToCurNodeLen.Length);
            System.Array.Clear(this.isOpened, 0, this.isOpened.Length);
            System.Array.Clear(this.isClosed, 0, this.isClosed.Length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public virtual Connection[] GetConnections() {

            return null;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal virtual void Reset(int threadIndex) {

            this.parent[threadIndex] = null;
            this.startToCurNodeLen[threadIndex] = 0f;
            this.isOpened[threadIndex] = false;
            this.isClosed[threadIndex] = false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public virtual bool IsSuitable(Constraint constraint) {

            if (constraint.checkWalkability == true && this.walkable != constraint.walkable) return false;
            if (constraint.checkArea == true && (constraint.areaMask & (1 << this.area)) == 0) return false;
            if (constraint.checkTags == true && (constraint.tagsMask & (1 << this.tag)) == 0) return false;
            if (constraint.graphMask >= 0 && (constraint.graphMask & (1 << this.graph.index)) == 0) return false;

            if (constraint.tagsMask > 0L &&
                (constraint.agentSize.x > 0f ||
                 constraint.agentSize.y > 0f ||
                 constraint.agentSize.z > 0f)) {

                var result = PoolListCopyable<Node>.Spawn(10);
                this.graph.GetNodesInBounds(result, new Bounds(this.worldPosition, constraint.agentSize));
                for (int e = 0, cnt = result.Count; e < cnt; ++e) {

                    var node = result[e];
                    var constraintErosion = constraint;
                    constraintErosion.agentSize = Vector3.zero;
                    if (node.IsSuitable(constraintErosion) == false) return false;

                }

                PoolListCopyable<Node>.Recycle(ref result);

            }

            return true;

        }

    }

}