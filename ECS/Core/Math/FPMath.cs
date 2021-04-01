
namespace ME.ECS {

    public static class FPMath {

        public const float Deg2Rad = 0.017453292f;
        public const float Rad2Deg = 57.29578f;
        
        public static pfloat Angle(FPVector2 from, FPVector2 to) {
        
            var num = FPMath.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
            return (float)num < 1.0000000036274937E-15 ? pfloat.Zero : FPMath.ACos(FPMath.Clamp(FPVector2.Dot(from, to) / num, -pfloat.One, pfloat.One)) * (pfloat)57.29578f;
            
        }

        public static float SignedAngle(FPVector2 from, FPVector2 to) => FPMath.Angle(from, to) * FPMath.Sign(from.x * to.y - from.y * to.x);
        
        public static pfloat Repeat(pfloat t, pfloat length) {

            return FPMath.Clamp(t - FPMath.Floor(t / length) * length, pfloat.Zero, length);

        }
        
        public static pfloat Min(pfloat v1, pfloat v2) {

            return v1 < v2 ? v1 : v2;

        }

        public static pfloat Max(pfloat v1, pfloat v2) {

            return v1 > v2 ? v1 : v2;

        }

        public static pfloat Abs(pfloat value) {

            return pfloat.Abs(value);

        }
        
        public static pfloat Atan2(pfloat y, pfloat x) {
            
            return pfloat.Atan2(y, x);
            
        }
        
        public static pfloat Floor(pfloat value) {

            return pfloat.Floor(value);

        }
        
        public static pfloat Round(pfloat value) {

            return pfloat.Round(value);

        }

        public static pfloat Pow(pfloat value, pfloat exp) {

            return pfloat.Pow(value, exp);

        }

        public static int Sign(pfloat value) {

            return pfloat.Sign(value);

        }
        
        public static pfloat Atan(pfloat z) {

            return pfloat.Atan(z);

        }
        
        public static pfloat ASin(pfloat value) {

            return UnityEngine.Mathf.Asin(value);

        }

        public static pfloat ACos(pfloat value) {

            return pfloat.Acos(value);
            
        }

        public static pfloat Cos(pfloat value) {

            return pfloat.Cos(value);

        }

        public static pfloat Sin(pfloat value) {
            
            return pfloat.Sin(value);

        }
        
        public static pfloat Lerp(pfloat a, pfloat b, pfloat t) {
         
            return a + (b - a) * FPMath.Clamp01(t);

        }

        public static pfloat Clamp01(pfloat value) {

            return FPMath.Clamp(value, 0f, 1f);

        }

        public static pfloat Clamp(pfloat value, pfloat min, pfloat max) {
            
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
            
        }
        
        public static pfloat Sqrt(pfloat v) {

            return pfloat.Sqrt(v);

        }
        
    }

}
