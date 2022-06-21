using System;

#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
#else
using Unity.Mathematics;
#endif

namespace ME.ECS.Collections {

    [Serializable]
    public struct AABB2D {

        public float2 center;
        public float2 extents;

        public float2 size => this.extents * 2;
        public float2 min => this.center - this.extents;
        public float2 max => this.center + this.extents;

        public AABB2D(float2 center, float2 extents) {
            this.center = center;
            this.extents = extents;
        }

        public bool Contains(float2 point) {
            if (point.x < this.center.x - this.extents.x) {
                return false;
            }

            if (point.x > this.center.x + this.extents.x) {
                return false;
            }

            if (point.y < this.center.y - this.extents.y) {
                return false;
            }

            if (point.y > this.center.y + this.extents.y) {
                return false;
            }

            return true;
        }

        public bool Contains(AABB2D b) {
            return this.Contains(b.center + new float2(-b.extents.x, -b.extents.y)) && this.Contains(b.center + new float2(-b.extents.x, b.extents.y)) &&
                   this.Contains(b.center + new float2(b.extents.x, -b.extents.y)) && this.Contains(b.center + new float2(b.extents.x, b.extents.y));
        }

        public bool Intersects(AABB2D b) {
            return math.abs(this.center.x - b.center.x) < this.extents.x + b.extents.x &&
                   math.abs(this.center.y - b.center.y) < this.extents.y + b.extents.y;
        }

    }

}