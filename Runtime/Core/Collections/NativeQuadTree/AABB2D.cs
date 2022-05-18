using System;

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
using FLOAT2 = Unity.Mathematics.float2;
using FLOAT3 = Unity.Mathematics.float3;
using FLOAT4 = Unity.Mathematics.float4;
using QUATERNION = UnityEngine.Quaternion;
#endif

namespace ME.ECS.Collections {

    [Serializable]
    public struct AABB2D {

        public FLOAT2 center;
        public FLOAT2 extents;

        public FLOAT2 size => this.extents * 2;
        public FLOAT2 min => this.center - this.extents;
        public FLOAT2 max => this.center + this.extents;

        public AABB2D(UnityEngine.Vector2 center, Unity.Mathematics.float2 extents) {
            this.center = center;
            this.extents = new FLOAT2(extents.x, extents.y);
        }

        public AABB2D(FLOAT2 center, FLOAT2 extents) {
            this.center = center;
            this.extents = extents;
        }

        public bool Contains(FLOAT2 point) {
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
            return this.Contains(b.center + new FLOAT2(-b.extents.x, -b.extents.y)) && this.Contains(b.center + new FLOAT2(-b.extents.x, b.extents.y)) &&
                   this.Contains(b.center + new FLOAT2(b.extents.x, -b.extents.y)) && this.Contains(b.center + new FLOAT2(b.extents.x, b.extents.y));
        }

        public bool Intersects(AABB2D b) {
            return MATH.abs(this.center.x - b.center.x) < this.extents.x + b.extents.x &&
                   MATH.abs(this.center.y - b.center.y) < this.extents.y + b.extents.y;
        }

    }

}