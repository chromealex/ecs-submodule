namespace ME.ECS {

    using UnityEngine;
    using System.Runtime.CompilerServices;

    public static partial class VecMath {

        internal const double VECTOR3_EPSILON = 0.00001d;
        internal const double K1_OVER_SQRT2 = 0.7071067811865475244008443621048490d;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Lerp(Vector2 lhs, Vector2 rhs, float t) {
            return VecMath.Lerp((fp2)lhs, (fp2)rhs, (fp)t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Lerp(Vector3 lhs, Vector3 rhs, float t) {
            return VecMath.Lerp((fp3)lhs, (fp3)rhs, (fp)t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Slerp(Vector3 lhs, Vector3 rhs, float t) {
            return VecMath.Slerp((fp3)lhs, (fp3)rhs, (fp)t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 RotateTowards(Vector3 lhs, Vector3 rhs, float angleMove, float magnitudeMove) {

            return VecMath.RotateTowards((fp3)lhs, (fp3)rhs, (fp)angleMove, (fp)magnitudeMove);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 MoveTowards(Vector3 lhs, Vector3 rhs, float clampedDistance) {

            return VecMath.MoveTowards((fp3)lhs, (fp3)rhs, (fp)clampedDistance);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ClampedMove(float lhs, float rhs, float clampedDelta) {

            return VecMath.ClampedMove((fp)lhs, (fp)rhs, (fp)clampedDelta);

        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Angle(Vector2 lhs, Vector2 rhs) {
            return VecMath.Angle((fp2)lhs, (fp2)rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Angle(Vector3 lhs, Vector3 rhs) {
            return VecMath.Angle((fp3)lhs, (fp3)rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SignedAngle(Vector2 lhs, Vector2 rhs) {
            return VecMath.SignedAngle((fp2)lhs, (fp2)rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SignedAngle(Vector3 lhs, Vector3 rhs, Vector3 axis) {
            return VecMath.SignedAngle((fp3)lhs, (fp3)rhs, (fp3)axis);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Angle(fp2 lhs, fp2 rhs) {
            var num = fpmath.sqrt(lhs.sqrMagnitude * rhs.sqrMagnitude);
            return num < fp.precision ? fp.zero : fpmath.degrees(fpmath.acos(fpmath.clamp(fpmath.dot(lhs, rhs) / num, -fp.one, fp.one)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Angle(fp3 lhs, fp3 rhs) {
            var num = fpmath.sqrt(lhs.sqrMagnitude * rhs.sqrMagnitude);
            return num < fp.precision ? fp.zero : fpmath.degrees(fpmath.acos(fpmath.clamp(fpmath.dot(lhs, rhs) / num, -fp.one, fp.one)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp SignedAngle(fp2 lhs, fp2 rhs) {
            return VecMath.Angle(lhs, rhs) * fpmath.sign(lhs.x * rhs.y - lhs.y * rhs.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp SignedAngle(fp3 lhs, fp3 rhs, fp3 axis) {
            var num1 = VecMath.Angle(lhs, rhs);
            var num2 = (lhs.y * rhs.z - lhs.z * rhs.y);
            var num3 = (lhs.z * rhs.x - lhs.x * rhs.z);
            var num4 = (lhs.x * rhs.y - lhs.y * rhs.x);
            var num5 = fpmath.sign(axis.x * num2 + axis.y * num3 + axis.z * num4);
            return num1 * num5;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 Lerp(fp3 lhs, fp3 rhs, fp t) {
            return fpmath.lerp(lhs, rhs, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 Lerp(fp2 lhs, fp2 rhs, fp t) {
            return fpmath.lerp(lhs, rhs, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 Slerp(fp3 lhs, fp3 rhs, fp t) {

            var lhsMag = fpmath.length(lhs);
            var rhsMag = fpmath.length(rhs);

            if (lhsMag < VecMath.VECTOR3_EPSILON || rhsMag < VecMath.VECTOR3_EPSILON) {
                return VecMath.Lerp(lhs, rhs, t);
            }

            var lerpedMagnitude = fpmath.lerp(lhsMag, rhsMag, t);

            var dot = fpmath.dot(lhs, rhs) / (lhsMag * rhsMag);
            // direction is almost the same
            if (dot > 1.0F - VecMath.VECTOR3_EPSILON) {
                return VecMath.Lerp(lhs, rhs, t);
            }
            // directions are almost opposite
            else if (dot < -1.0F + VecMath.VECTOR3_EPSILON) {
                var lhsNorm = lhs / lhsMag;
                var axis = VecMath.OrthoNormalVectorFast(lhsNorm);
                var m = fpmatrix3x3.zero;
                m.SetAxisAngle(axis, fpmath.PI * t);
                var slerped = m.MultiplyVector3(lhsNorm);
                slerped *= lerpedMagnitude;
                return slerped;
            }
            // normal case
            else {
                var axis = fpmath.cross(lhs, rhs);
                var lhsNorm = lhs / lhsMag;
                axis = fpmath.normalizesafe(axis);
                var angle = fpmath.acos(dot) * t;
                var m = fpmatrix3x3.zero;
                m.SetAxisAngle(axis, angle);
                var slerped = m.MultiplyVector3(lhsNorm);
                slerped *= lerpedMagnitude;
                return slerped;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 RotateTowards(fp3 lhs, fp3 rhs, fp angleMove, fp magnitudeMove) {

            var lhsMag = fpmath.length(lhs);
            var rhsMag = fpmath.length(rhs);

            // both vectors are non-zero
            if (lhsMag > VecMath.VECTOR3_EPSILON && rhsMag > VecMath.VECTOR3_EPSILON) {
                fp3 lhsNorm = lhs / lhsMag;
                fp3 rhsNorm = rhs / rhsMag;

                var dot = fpmath.dot(lhsNorm, rhsNorm);
                // direction is almost the same
                if (dot > fp.one - VecMath.VECTOR3_EPSILON) {
                    return VecMath.MoveTowards(lhs, rhs, magnitudeMove);
                }
                // directions are almost opposite
                else if (dot < -fp.one + VecMath.VECTOR3_EPSILON) {
                    var axis = VecMath.OrthoNormalVectorFast(lhsNorm);
                    var m = fpmatrix3x3.zero;
                    m.SetAxisAngle(axis, angleMove);
                    var rotated = m.MultiplyVector3(lhsNorm);
                    rotated *= VecMath.ClampedMove(lhsMag, rhsMag, magnitudeMove);
                    return rotated;
                }
                // normal case
                else {
                    var angle = fpmath.acos(dot);
                    var axis = fpmath.normalize(fpmath.cross(lhsNorm, rhsNorm));
                    var m = fpmatrix3x3.zero;
                    m.SetAxisAngle(axis, fpmath.min(angleMove, angle));
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
        public static fp3 MoveTowards(fp3 lhs, fp3 rhs, fp clampedDistance) {

            var delta = rhs - lhs;
            var sqrDelta = fpmath.lengthsq(delta);
            var sqrClampedDistance = clampedDistance * clampedDistance;
            if (sqrDelta > sqrClampedDistance) {
                var deltaMag = fpmath.sqrt(sqrDelta);
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
        public static fp ClampedMove(fp lhs, fp rhs, fp clampedDelta) {
            var delta = rhs - lhs;
            if (delta > fp.zero) {
                return lhs + fpmath.min(delta, clampedDelta);
            } else {
                return lhs - fpmath.min(-delta, clampedDelta);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static fp3 OrthoNormalVectorFast(fp3 n) {
            fp3 res;
            if (fpmath.abs(n.z) > VecMath.K1_OVER_SQRT2) {
                // choose p in y-z plane
                var a = n.y * n.y + n.z * n.z;
                var k = fp.one / fpmath.sqrt(a);
                res.x = fp.zero;
                res.y = -n.z * k;
                res.z = n.y * k;
            } else {
                // choose p in x-y plane
                var a = n.x * n.x + n.y * n.y;
                var k = fp.one / fpmath.sqrt(a);
                res.x = -n.y * k;
                res.y = n.x * k;
                res.z = fp.zero;
            }

            return res;
        }

    }

}