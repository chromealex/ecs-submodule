namespace ME.ECS {

    using Unity.Mathematics;

    public static class UnityQuaternionExt {

        private static float3 FromQ2(quaternion quaternion) {

            var q1 = quaternion.value;
            var sqw = q1.w * q1.w;
            var sqx = q1.x * q1.x;
            var sqy = q1.y * q1.y;
            var sqz = q1.z * q1.z;
            var unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            var test = q1.x * q1.w - q1.y * q1.z;
            float3 v;

            if (test > 0.4995f * unit) { // singularity at north pole
                v.y = 2f * math.atan2(q1.y, q1.x);
                v.x = math.PI / 2;
                v.z = 0;
                return UnityQuaternionExt.NormalizeAngles(math.degrees(v));
            }

            if (test < -0.4995f * unit) { // singularity at south pole
                v.y = -2f * math.atan2(q1.y, q1.x);
                v.x = -math.PI / 2;
                v.z = 0;
                return UnityQuaternionExt.NormalizeAngles(math.degrees(v));
            }

            var q = new quaternion(q1.w, q1.z, q1.x, q1.y).value;
            v.y = (float)math.atan2(2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w)); // Yaw
            v.x = (float)math.asin(2f * (q.x * q.z - q.w * q.y)); // Pitch
            v.z = (float)math.atan2(2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z)); // Roll
            return UnityQuaternionExt.NormalizeAngles(math.degrees(v));
        }

        private static float3 NormalizeAngles(float3 angles) {
            angles.x = UnityQuaternionExt.NormalizeAngle(angles.x);
            angles.y = UnityQuaternionExt.NormalizeAngle(angles.y);
            angles.z = UnityQuaternionExt.NormalizeAngle(angles.z);
            return angles;
        }

        private static float NormalizeAngle(float angle) {
            while (angle > 360f) {
                angle -= 360f;
            }

            while (angle < 0f) {
                angle += 360f;
            }

            return angle;
        }

        public static float3 ToEuler(this quaternion quaternion) {

            return FromQ2(quaternion);
        }

    }

}