namespace ME.ECS.Mathematics {

    using System.Runtime.CompilerServices;

    public static partial class VecMath {

        internal const float VECTOR3_EPSILON = 0.00001f;
        internal const float K1_OVER_SQRT2 = 0.7071067811865475244008443621048490f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sfloat Angle(float2 lhs, float2 rhs) {
            var num = math.sqrt(math.lengthsq(lhs) * math.lengthsq(rhs));
            return num < sfloat.Epsilon ? sfloat.Zero : math.degrees(math.acos(math.clamp(math.dot(lhs, rhs) / num, -sfloat.One, sfloat.One)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sfloat Angle(float3 lhs, float3 rhs) {
            var num = math.sqrt(math.lengthsq(lhs) * math.lengthsq(rhs));
            return num < sfloat.Epsilon ? sfloat.Zero : math.degrees(math.acos(math.clamp(math.dot(lhs, rhs) / num, -sfloat.One, sfloat.One)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sfloat SignedAngle(float2 lhs, float2 rhs) {
            return VecMath.Angle(lhs, rhs) * math.sign(lhs.x * rhs.y - lhs.y * rhs.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sfloat SignedAngle(float3 lhs, float3 rhs, float3 axis) {
            var num1 = VecMath.Angle(lhs, rhs);
            var num2 = (lhs.y * rhs.z - lhs.z * rhs.y);
            var num3 = (lhs.z * rhs.x - lhs.x * rhs.z);
            var num4 = (lhs.x * rhs.y - lhs.y * rhs.x);
            var num5 = math.sign(axis.x * num2 + axis.y * num3 + axis.z * num4);
            return num1 * num5;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 Lerp(float3 lhs, float3 rhs, sfloat t) {
            return math.lerp(lhs, rhs, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 Lerp(float2 lhs, float2 rhs, sfloat t) {
            return math.lerp(lhs, rhs, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 Slerp(float3 lhs, float3 rhs, sfloat t) {

            var lhsMag = math.length(lhs);
            var rhsMag = math.length(rhs);

            if (lhsMag < VecMath.VECTOR3_EPSILON || rhsMag < VecMath.VECTOR3_EPSILON) {
                return VecMath.Lerp(lhs, rhs, t);
            }

            var lerpedMagnitude = math.lerp(lhsMag, rhsMag, t);

            var dot = math.dot(lhs, rhs) / (lhsMag * rhsMag);
            // direction is almost the same
            if (dot > 1.0F - VecMath.VECTOR3_EPSILON) {
                return VecMath.Lerp(lhs, rhs, t);
            }
            // directions are almost opposite
            else if (dot < -1.0F + VecMath.VECTOR3_EPSILON) {
                var lhsNorm = lhs / lhsMag;
                var axis = VecMath.OrthoNormalVectorFast(lhsNorm);
                var m = fpmatrix3x3.zero;
                m.SetAxisAngle(axis, math.PI * t);
                var slerped = m.MultiplyVector3(lhsNorm);
                slerped *= lerpedMagnitude;
                return slerped;
            }
            // normal case
            else {
                var axis = math.cross(lhs, rhs);
                var lhsNorm = lhs / lhsMag;
                axis = math.normalizesafe(axis);
                var angle = math.acos(dot) * t;
                var m = fpmatrix3x3.zero;
                m.SetAxisAngle(axis, angle);
                var slerped = m.MultiplyVector3(lhsNorm);
                slerped *= lerpedMagnitude;
                return slerped;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 RotateTowards(float3 lhs, float3 rhs, sfloat angleMove, sfloat magnitudeMove) {

            var lhsMag = math.length(lhs);
            var rhsMag = math.length(rhs);

            // both vectors are non-zero
            if (lhsMag > VecMath.VECTOR3_EPSILON && rhsMag > VecMath.VECTOR3_EPSILON) {
                float3 lhsNorm = lhs / lhsMag;
                float3 rhsNorm = rhs / rhsMag;

                var dot = math.dot(lhsNorm, rhsNorm);
                // direction is almost the same
                if (dot > sfloat.One - VecMath.VECTOR3_EPSILON) {
                    return VecMath.MoveTowards(lhs, rhs, magnitudeMove);
                }
                // directions are almost opposite
                else if (dot < -sfloat.One + VecMath.VECTOR3_EPSILON) {
                    var axis = VecMath.OrthoNormalVectorFast(lhsNorm);
                    var m = fpmatrix3x3.zero;
                    m.SetAxisAngle(axis, angleMove);
                    var rotated = m.MultiplyVector3(lhsNorm);
                    rotated *= VecMath.ClampedMove(lhsMag, rhsMag, magnitudeMove);
                    return rotated;
                }
                // normal case
                else {
                    var angle = math.acos(dot);
                    var axis = math.normalize(math.cross(lhsNorm, rhsNorm));
                    var m = fpmatrix3x3.zero;
                    m.SetAxisAngle(axis, math.min(angleMove, angle));
                    var rotated = m.MultiplyVector3(lhsNorm);
                    rotated *= VecMath.ClampedMove(lhsMag, rhsMag, magnitudeMove);
                    return rotated;
                }
            }
            // at least one of the vectors is almost zero
            else {
                return VecMath.MoveTowards(lhs, rhs, magnitudeMove);
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 MoveTowards(float3 lhs, float3 rhs, sfloat clampedDistance) {

            var delta = rhs - lhs;
            var sqrDelta = math.lengthsq(delta);
            var sqrClampedDistance = clampedDistance * clampedDistance;
            if (sqrDelta > sqrClampedDistance) {
                var deltaMag = math.sqrt(sqrDelta);
                if (deltaMag > VecMath.VECTOR3_EPSILON) {
                    return lhs + delta / deltaMag * clampedDistance;
                } else {
                    return lhs;
                }
            } else {
                return rhs;
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sfloat ClampedMove(sfloat lhs, sfloat rhs, sfloat clampedDelta) {
            var delta = rhs - lhs;
            if (delta > sfloat.Zero) {
                return lhs + math.min(delta, clampedDelta);
            } else {
                return lhs - math.min(-delta, clampedDelta);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float3 OrthoNormalVectorFast(float3 n) {
            float3 res;
            if (math.abs(n.z) > VecMath.K1_OVER_SQRT2) {
                // choose p in y-z plane
                var a = n.y * n.y + n.z * n.z;
                var k = sfloat.One / math.sqrt(a);
                res.x = sfloat.Zero;
                res.y = -n.z * k;
                res.z = n.y * k;
            } else {
                // choose p in x-y plane
                var a = n.x * n.x + n.y * n.y;
                var k = sfloat.One / math.sqrt(a);
                res.x = -n.y * k;
                res.y = n.x * k;
                res.z = sfloat.Zero;
            }

            return res;
        }

    }

}