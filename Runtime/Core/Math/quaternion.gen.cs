#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Unity.Mathematics;
    using System.Runtime.CompilerServices;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    #if MESSAGE_PACK_SUPPORT
    [MessagePack.MessagePackObjectAttribute()]
    #endif
    public struct fpquaternion : System.IEquatable<fpquaternion> {

        public static readonly fpquaternion identity = new fpquaternion(0, 0, 0, 1);
        public static readonly fpquaternion zero = new fpquaternion(0, 0, 0, 0);
        public static UnityEngine.Quaternion q;

        public fpquaternion normalized => fpquaternion.NormalizeSafe(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquaternion NormalizeSafe(fpquaternion q) {
            var mag = fpmath.length(q);
            if (mag < VecMath.VECTOR3_EPSILON) {
                return fpquaternion.identity;
            } else {
                return q / mag;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquaternion NormalizeFastEpsilonZero(fpquaternion q) {
            var m = fpmath.lengthsq(q);
            if (m < VecMath.VECTOR3_EPSILON) {
                return fpquaternion.zero;
            } else {
                return q * fpmath.rsqrt(m);
            }
        }

        #if MESSAGE_PACK_SUPPORT
        [MessagePack.Key(0)]
        #endif
        public fp x;
        #if MESSAGE_PACK_SUPPORT
        [MessagePack.Key(1)]
        #endif
        public fp y;
        #if MESSAGE_PACK_SUPPORT
        [MessagePack.Key(2)]
        #endif
        public fp z;
        #if MESSAGE_PACK_SUPPORT
        [MessagePack.Key(3)]
        #endif
        public fp w;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpquaternion(fp x, fp y, fp z, fp w) {

            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpquaternion(fp4 value) {

            this.x = value.x;
            this.y = value.y;
            this.z = value.z;
            this.w = value.w;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpquaternion(fp3x3 m) {

            var u = m.c0;
            var v = m.c1;
            var w = m.c2;

            var u_sign = (uint)u.x & 0x80000000;
            var t = v.y + (fp)((uint)w.z ^ u_sign);
            var u_mask = new uint4((int)u_sign >> 31);
            var t_mask = new uint4((int)t >> 31);
            var tr = 1.0f + fpmath.abs(u.x);

            var sign_flips = new uint4(0x00000000, 0x80000000, 0x80000000, 0x80000000) ^ (u_mask & new uint4(0x00000000, 0x80000000, 0x00000000, 0x80000000)) ^
                             (t_mask & new uint4(0x80000000, 0x80000000, 0x80000000, 0x00000000));

            var value = new float4(tr, u.y, w.x, v.z) + (float4)((uint4)new float4(t, v.x, u.z, w.y) ^ sign_flips); // +---, +++-, ++-+, +-++

            value = ((uint4)value & ~u_mask) | ((uint4)value.zwxy & u_mask);
            value = ((uint4)value.wzyx & ~t_mask) | ((uint4)value & t_mask);
            value = fpmath.normalize(value);

            this.x = value.x;
            this.y = value.y;
            this.z = value.z;
            this.w = value.w;

        }

        public override int GetHashCode() {

            return this.x.GetHashCode() ^ this.y.GetHashCode() ^ this.z.GetHashCode() ^ this.w.GetHashCode();

        }

        public bool Equals(fpquaternion other) {

            return this == other;

        }

        public override bool Equals(object obj) {

            if (obj is fpquaternion ent) {

                return this.Equals(ent);

            }

            return false;

        }

        #if MESSAGE_PACK_SUPPORT
        [MessagePack.IgnoreMemberAttribute]
        #endif
        public fp3 eulerAngles {
            get {

                fp3 angles;
                var sinr_cosp = 2 * (fpquaternion.q.w * fpquaternion.q.x + fpquaternion.q.y * fpquaternion.q.z);
                var cosr_cosp = 1 - 2 * (fpquaternion.q.x * fpquaternion.q.x + fpquaternion.q.y * fpquaternion.q.y);
                angles.x = fpmath.atan2(sinr_cosp, cosr_cosp);

                // pitch (y-axis rotation)
                var sinp = 2 * (fpquaternion.q.w * fpquaternion.q.y - fpquaternion.q.z * fpquaternion.q.x);
                if (fpmath.abs(sinp) >= 1f) {
                    angles.y = fpmath.PI / 2 * fpmath.sign(sinp); // use 90 degrees if out of range
                } else {
                    angles.y = fpmath.asin(sinp);
                }

                // yaw (z-axis rotation)
                var siny_cosp = 2 * (fpquaternion.q.w * fpquaternion.q.z + fpquaternion.q.x * fpquaternion.q.y);
                var cosy_cosp = 1 - 2 * (fpquaternion.q.y * fpquaternion.q.y + fpquaternion.q.z * fpquaternion.q.z);
                angles.z = fpmath.atan2(siny_cosp, cosy_cosp);

                return angles;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquaternion Euler(fp3 v) {

            return fpquaternion.Euler(v.x, v.y, v.z);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquaternion Euler(fp roll, fp pitch, fp yaw) {

            roll *= fpmath.PI / 180f;
            pitch *= fpmath.PI / 180f;
            yaw *= fpmath.PI / 180f;

            fpquaternion quaternion;
            var num9 = roll * 0.5f;
            var num6 = fpmath.sin(num9);
            var num5 = fpmath.cos(num9);
            var num8 = pitch * 0.5f;
            var num4 = fpmath.sin(num8);
            var num3 = fpmath.cos(num8);
            var num7 = yaw * 0.5f;
            var num2 = fpmath.sin(num7);
            var num = fpmath.cos(num7);
            quaternion.x = num * num4 * num5 + num2 * num3 * num6;
            quaternion.y = num2 * num3 * num5 - num * num4 * num6;
            quaternion.z = num * num3 * num6 - num2 * num4 * num5;
            quaternion.w = num * num3 * num5 + num2 * num4 * num6;
            return quaternion;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquaternion Slerp(fpquaternion quaternion1, fpquaternion quaternion2, fp amount) {

            fp num2;
            fp num3;
            fpquaternion quaternion;
            var num = amount;
            var num4 = quaternion1.x * quaternion2.x + quaternion1.y * quaternion2.y + quaternion1.z * quaternion2.z + quaternion1.w * quaternion2.w;
            var flag = false;
            if (num4 < 0f) {
                flag = true;
                num4 = -num4;
            }

            if (num4 > 0.999999f) {
                num3 = 1f - num;
                num2 = flag ? -num : num;
            } else {
                var num5 = fpmath.acos(num4);
                var num6 = 1.0f / fpmath.sin(num5);
                num3 = fpmath.sin((1f - num) * num5) * num6;
                num2 = flag ? -fpmath.sin(num * num5) * num6 : fpmath.sin(num * num5) * num6;
            }

            quaternion.x = num3 * quaternion1.x + num2 * quaternion2.x;
            quaternion.y = num3 * quaternion1.y + num2 * quaternion2.y;
            quaternion.z = num3 * quaternion1.z + num2 * quaternion2.z;
            quaternion.w = num3 * quaternion1.w + num2 * quaternion2.w;
            return quaternion;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquaternion Inverse(fpquaternion quaternion) {

            quaternion.Inverse();
            return quaternion;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Inverse() {

            if (this == fpquaternion.zero) return;
            var num2 = this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w;
            var num = 1f / num2;
            this.x = -this.x * num;
            this.y = -this.y * num;
            this.z = -this.z * num;
            this.w = this.w * num;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fpquaternion(UnityEngine.Quaternion v) {

            return new fpquaternion(v.x, v.y, v.z, v.w);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp4(fpquaternion v) {

            return new fp4(v.x, v.y, v.z, v.w);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquaternion operator *(fpquaternion lhs, fpquaternion rhs) {
            return new fpquaternion((float)((double)lhs.w * (double)rhs.x + (double)lhs.x * (double)rhs.w + (double)lhs.y * (double)rhs.z - (double)lhs.z * (double)rhs.y),
                                    (float)((double)lhs.w * (double)rhs.y + (double)lhs.y * (double)rhs.w + (double)lhs.z * (double)rhs.x - (double)lhs.x * (double)rhs.z),
                                    (float)((double)lhs.w * (double)rhs.z + (double)lhs.z * (double)rhs.w + (double)lhs.x * (double)rhs.y - (double)lhs.y * (double)rhs.x),
                                    (float)((double)lhs.w * (double)rhs.w - (double)lhs.x * (double)rhs.x - (double)lhs.y * (double)rhs.y - (double)lhs.z * (double)rhs.z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 operator *(fpquaternion rotation, fp3 point) {

            var num1 = rotation.x * 2f;
            var num2 = rotation.y * 2f;
            var num3 = rotation.z * 2f;
            var num4 = rotation.x * num1;
            var num5 = rotation.y * num2;
            var num6 = rotation.z * num3;
            var num7 = rotation.x * num2;
            var num8 = rotation.x * num3;
            var num9 = rotation.y * num3;
            var num10 = rotation.w * num1;
            var num11 = rotation.w * num2;
            var num12 = rotation.w * num3;
            fp3 vector3;
            vector3.x = (float)((1.0 - ((double)num5 + (double)num6)) * (double)point.x + ((double)num7 - (double)num12) * (double)point.y +
                                ((double)num8 + (double)num11) * (double)point.z);
            vector3.y = (float)(((double)num7 + (double)num12) * (double)point.x + (1.0 - ((double)num4 + (double)num6)) * (double)point.y +
                                ((double)num9 - (double)num10) * (double)point.z);
            vector3.z = (float)(((double)num8 - (double)num11) * (double)point.x + ((double)num9 + (double)num10) * (double)point.y +
                                (1.0 - ((double)num4 + (double)num5)) * (double)point.z);
            return vector3;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 operator *(fpquaternion rotation, fp2 point) {

            var num1 = rotation.x * 2f;
            var num2 = rotation.y * 2f;
            var num3 = rotation.z * 2f;
            var num4 = rotation.x * num1;
            var num5 = rotation.y * num2;
            var num6 = rotation.z * num3;
            var num7 = rotation.x * num2;
            var num12 = rotation.w * num3;
            fp2 vector2;
            vector2.x = (float)((1.0 - ((double)num5 + (double)num6)) * (double)point.x + ((double)num7 - (double)num12) * (double)point.y);
            vector2.y = (float)(((double)num7 + (double)num12) * (double)point.x + (1.0 - ((double)num4 + (double)num6)) * (double)point.y);
            return vector2;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UnityEngine.Quaternion(fpquaternion q) {

            return new UnityEngine.Quaternion(q.x, q.y, q.z, q.w);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(fpquaternion q1, fpquaternion q2) {

            return q1.x == q2.x && q1.y == q2.y && q1.z == q2.z && q1.w == q2.w;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(fpquaternion q1, fpquaternion q2) {

            return !(q1 == q2);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquaternion operator *(fpquaternion q, fp value) {

            return new fpquaternion(q.x * value, q.y * value, q.z * value, q.w * value);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquaternion operator /(fpquaternion q, fp value) {

            return new fpquaternion(q.x / value, q.y / value, q.z / value, q.w / value);

        }

    }

}