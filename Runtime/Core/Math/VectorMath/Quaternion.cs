namespace ME.ECS {

    using UnityEngine;
    using System.Runtime.CompilerServices;

    public static partial class VecMath {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion LookRotation(Vector3 forward, Vector3 up) {
            return VecMath.LookRotationSafe(forward, up);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion FromToRotation(Vector3 forward, Vector3 up) {
            return VecMath.FromToRotationSafe(forward, up);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquaternion LookRotation(fp3 forward, fp3 up) {
            var t = fpmath.normalize(fpmath.cross(up, forward));
            return new fpquaternion(new fp3x3(t, fpmath.cross(forward, t), forward));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquaternion LookRotationSafe(fp3 forward, fp3 up) {
            var forwardLengthSq = fpmath.dot(forward, forward);
            var upLengthSq = fpmath.dot(up, up);

            forward *= fpmath.rsqrt(forwardLengthSq);
            up *= fpmath.rsqrt(upLengthSq);

            var t = fpmath.cross(up, forward);
            var tLengthSq = fpmath.dot(t, t);
            t *= fpmath.rsqrt(tLengthSq);

            var mn = fpmath.min(fpmath.min(forwardLengthSq, upLengthSq), tLengthSq);
            var mx = fpmath.max(fpmath.max(forwardLengthSq, upLengthSq), tLengthSq);

            var accept = mn > 1e-35f && mx < 1e35f && fpmath.isfinite(forwardLengthSq) && fpmath.isfinite(upLengthSq) && fpmath.isfinite(tLengthSq);
            return new fpquaternion(fpmath.select(new fp4(0.0f, 0.0f, 0.0f, 1.0f), (fp4)new fpquaternion(new fp3x3(t, fpmath.cross(forward, t), forward)), accept));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquaternion FromToRotationSafe(fp3 lhs, fp3 rhs) {
            var lhsMag = fpmath.length(lhs);
            var rhsMag = fpmath.length(rhs);
            if (lhsMag < VecMath.VECTOR3_EPSILON || rhsMag < VecMath.VECTOR3_EPSILON) {
                return fpquaternion.identity;
            } else {
                return VecMath.FromToRotation(lhs / lhsMag, rhs / rhsMag);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquaternion FromToRotation(fp3 from, fp3 to) {
            var m = new fpmatrix3x3();
            m.SetFromToRotation(from, to);
            var q = fpquaternion.zero;
            VecMath.MatrixToQuaternion(m, ref q);
            return q;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MatrixToQuaternion(fpmatrix3x3 kRot, ref fpquaternion q) {
            // Algorithm in Ken Shoemake's article in 1987 SIGGRAPH course notes
            // article "Quaternionf Calculus and Fast Animation".
            var fTrace = kRot.Get(0, 0) + kRot.Get(1, 1) + kRot.Get(2, 2);
            fp fRoot;

            if (fTrace > 0.0f) {
                // |w| > 1/2, may as well choose w > 1/2
                fRoot = fpmath.sqrt(fTrace + 1.0f); // 2w
                q.w = 0.5f * fRoot;
                fRoot = 0.5f / fRoot; // 1/(4w)
                q.x = (kRot.Get(2, 1) - kRot.Get(1, 2)) * fRoot;
                q.y = (kRot.Get(0, 2) - kRot.Get(2, 0)) * fRoot;
                q.z = (kRot.Get(1, 0) - kRot.Get(0, 1)) * fRoot;
            } else {
                // |w| <= 1/2
                var sINext = new Vector3Int(1, 2, 0);
                var i = 0;
                if (kRot.Get(1, 1) > kRot.Get(0, 0)) {
                    i = 1;
                }

                if (kRot.Get(2, 2) > kRot.Get(i, i)) {
                    i = 2;
                }

                var j = sINext[i];
                var k = sINext[j];

                fRoot = fpmath.sqrt(kRot.Get(i, i) - kRot.Get(j, j) - kRot.Get(k, k) + 1.0f);
                var apkQuat = new fp3(q.x, q.y, q.z);
                apkQuat[i] = 0.5f * fRoot;
                fRoot = 0.5f / fRoot;
                q.w = (kRot.Get(k, j) - kRot.Get(j, k)) * fRoot;
                apkQuat[j] = (kRot.Get(j, i) + kRot.Get(i, j)) * fRoot;
                apkQuat[k] = (kRot.Get(k, i) + kRot.Get(i, k)) * fRoot;
                q.x = apkQuat[0];
                q.y = apkQuat[1];
                q.z = apkQuat[2];
            }

            q = q.normalized;
        }

    }

}