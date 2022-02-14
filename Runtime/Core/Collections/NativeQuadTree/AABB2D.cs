using System;
using Unity.Mathematics;

namespace ME.ECS.Collections {

    [Serializable]
    public struct AABB2D {

        public float2 Center;
        public float2 Extents;

        public float2 Size => this.Extents * 2;
        public float2 Min => this.Center - this.Extents;
        public float2 Max => this.Center + this.Extents;

        public AABB2D(UnityEngine.Vector2 center, float2 extents) {
            this.Center = center;
            this.Extents = extents;
        }

        public bool Contains(float2 point) {
            if (point[0] < this.Center[0] - this.Extents[0]) {
                return false;
            }

            if (point[0] > this.Center[0] + this.Extents[0]) {
                return false;
            }

            if (point[1] < this.Center[1] - this.Extents[1]) {
                return false;
            }

            if (point[1] > this.Center[1] + this.Extents[1]) {
                return false;
            }

            return true;
        }

        public bool Contains(AABB2D b) {
            return this.Contains(b.Center + new float2(-b.Extents.x, -b.Extents.y)) && this.Contains(b.Center + new float2(-b.Extents.x, b.Extents.y)) &&
                   this.Contains(b.Center + new float2(b.Extents.x, -b.Extents.y)) && this.Contains(b.Center + new float2(b.Extents.x, b.Extents.y));
        }

        public bool Intersects(AABB2D b) {
            //bool noOverlap = Min[0] > b.Max[0] ||
            //                 b.Min[0] > Max[0]||
            //                 Min[1] > b.Max[1] ||
            //                 b.Min[1] > Max[1];
            //
            //return !noOverlap;

            return math.abs(this.Center[0] - b.Center[0]) < this.Extents[0] + b.Extents[0] &&
                   math.abs(this.Center[1] - b.Center[1]) < this.Extents[1] + b.Extents[1];
        }

    }

}