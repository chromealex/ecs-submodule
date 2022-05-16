using System;
using Unity.Mathematics;

namespace ME.ECS.Collections {

    [Serializable]
    public struct AABB2D {

        public float2 center;
        public float2 extents;

        public float2 size => this.extents * 2;
        public float2 min => this.center - this.extents;
        public float2 max => this.center + this.extents;

        public AABB2D(UnityEngine.Vector2 center, float2 extents) {
            this.center = center;
            this.extents = extents;
        }

        public bool Contains(float2 point) {
            if (point[0] < this.center[0] - this.extents[0]) {
                return false;
            }

            if (point[0] > this.center[0] + this.extents[0]) {
                return false;
            }

            if (point[1] < this.center[1] - this.extents[1]) {
                return false;
            }

            if (point[1] > this.center[1] + this.extents[1]) {
                return false;
            }

            return true;
        }

        public bool Contains(AABB2D b) {
            return this.Contains(b.center + new float2(-b.extents.x, -b.extents.y)) && this.Contains(b.center + new float2(-b.extents.x, b.extents.y)) &&
                   this.Contains(b.center + new float2(b.extents.x, -b.extents.y)) && this.Contains(b.center + new float2(b.extents.x, b.extents.y));
        }

        public bool Intersects(AABB2D b) {
            //bool noOverlap = Min[0] > b.Max[0] ||
            //                 b.Min[0] > Max[0]||
            //                 Min[1] > b.Max[1] ||
            //                 b.Min[1] > Max[1];
            //
            //return !noOverlap;

            return math.abs(this.center[0] - b.center[0]) < this.extents[0] + b.extents[0] &&
                   math.abs(this.center[1] - b.center[1]) < this.extents[1] + b.extents[1];
        }

    }

}