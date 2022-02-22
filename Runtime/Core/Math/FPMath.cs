using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace ME.ECS {

    public static partial class fpmath {

        public static readonly fp E = 2.7182818284590451d;
        public static readonly fp PI = new fp(fpmath.RAW_PI);
        public static readonly fp PI_TIMES_2 = new fp(fpmath.RAW_PI_TIMES_2);
        public static readonly fp PI_OVER_2 = new fp(fpmath.RAW_PI_OVER_2);
        public static readonly fp PI_INV = 0.3183098861837906715377675267d;
        public static readonly fp PI_OVER_2_INV = 0.6366197723675813430755350535d;
        public static readonly fp SQRT2 = 1.4142135623730952d;
        public static readonly fp LN10 = 2.3025850929940459d;
        public static readonly fp LN2 = 0.69314718055994529d;
        public static readonly fp LOG10E = 0.43429448190325182d;
        public static readonly fp LOG2E = 1.4426950408889634d;

        private const long RAW_PI_TIMES_2 = 0x6487ED511;
        private const long RAW_PI = 0x3243F6A88;
        private const long RAW_PI_OVER_2 = 0x1921FB544;

        private const string NOT_SUPPORTED_YET = "Not supported yet.";

        /// <summary>Returns the bit pattern of a fp as a uint.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint asuint(fp x) {
            return (uint)math.asint((uint)x);
        }

        /// <summary>Returns the bit pattern of a fp2 as a uint2.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint2 asuint(fp2 x) {
            return math.uint2(fpmath.asuint(x.x), fpmath.asuint(x.y));
        }

        /// <summary>Returns the bit pattern of a fp3 as a uint3.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint3 asuint(fp3 x) {
            return math.uint3(fpmath.asuint(x.x), fpmath.asuint(x.y), fpmath.asuint(x.z));
        }

        /// <summary>Returns the bit pattern of a fp4 as a uint4.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint4 asuint(fp4 x) {
            return math.uint4(fpmath.asuint(x.x), fpmath.asuint(x.y), fpmath.asuint(x.z), fpmath.asuint(x.w));
        }


        /// <summary>Returns the absolute value of a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp abs(fp x) {
            return fp.Abs(x);
        }

        /// <summary>Returns the componentwise absolute value of a fp2 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 abs(fp2 x) {
            return new fp2(fp.Abs(x.x), fp.Abs(x.y));
        }

        /// <summary>Returns the componentwise absolute value of a fp3 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 abs(fp3 x) {
            return new fp3(fp.Abs(x.x), fp.Abs(x.y), fp.Abs(x.z));
        }

        /// <summary>Returns the componentwise absolute value of a fp4 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 abs(fp4 x) {
            return new fp4(fp.Abs(x.x), fp.Abs(x.y), fp.Abs(x.z), fp.Abs(x.w));
        }


        /// <summary>Returns the sine of a fp value.</summary>
        public static fp sin(fp x) {
            return fp.Sin(x);
        }

        /// <summary>Returns the arc sine of a fp value.</summary>
        public static fp asin(fp x) {
            return fp.Asin(x);
        }

        /// <summary>Returns the arc cos of a fp value.</summary>
        public static fp acos(fp x) {
            return fp.Acos(x);
        }

        /// <summary>Returns the componentwise sine of a fp2 vector.</summary>
        public static fp2 sin(fp2 x) {
            return new fp2(fpmath.sin(x.x), fpmath.sin(x.y));
        }

        /// <summary>Returns the componentwise sine of a fp3 vector.</summary>
        public static fp3 sin(fp3 x) {
            return new fp3(fpmath.sin(x.x), fpmath.sin(x.y), fpmath.sin(x.z));
        }

        /// <summary>Returns the componentwise sine of a fp4 vector.</summary>
        public static fp4 sin(fp4 x) {
            return new fp4(fpmath.sin(x.x), fpmath.sin(x.y), fpmath.sin(x.z), fpmath.sin(x.w));
        }


        /// <summary>Returns the cosine of a fp value.</summary>
        public static fp cos(fp x) {
            return fp.Cos(x);
        }

        /// <summary>Returns the componentwise cosine of a fp2 vector.</summary>
        public static fp2 cos(fp2 x) {
            return new fp2(fpmath.cos(x.x), fpmath.cos(x.y));
        }

        /// <summary>Returns the componentwise cosine of a fp3 vector.</summary>
        public static fp3 cos(fp3 x) {
            return new fp3(fpmath.cos(x.x), fpmath.cos(x.y), fpmath.cos(x.z));
        }

        /// <summary>Returns the componentwise cosine of a fp4 vector.</summary>
        public static fp4 cos(fp4 x) {
            return new fp4(fpmath.cos(x.x), fpmath.cos(x.y), fpmath.cos(x.z), fpmath.cos(x.w));
        }


        /// <summary>Returns the tangent of a fp value.</summary>
        public static fp tan(fp x) {
            return fp.Tan(x);
        }

        /// <summary>Returns the componentwise tangent of a fp2 vector.</summary>
        public static fp2 tan(fp2 x) {
            return new fp2(fpmath.tan(x.x), fpmath.tan(x.y));
        }

        /// <summary>Returns the componentwise tangent of a fp3 vector.</summary>
        public static fp3 tan(fp3 x) {
            return new fp3(fpmath.tan(x.x), fpmath.tan(x.y), fpmath.tan(x.z));
        }

        /// <summary>Returns the componentwise tangent of a fp4 vector.</summary>
        public static fp4 tan(fp4 x) {
            return new fp4(fpmath.tan(x.x), fpmath.tan(x.y), fpmath.tan(x.z), fpmath.tan(x.w));
        }


        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the hyperbolic tangent of a fp value.</summary>
        public static fp tanh(fp x) {
            throw new System.NotImplementedException("fp doesn't support tanh");
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise hyperbolic tangent of a fp2 vector.</summary>
        public static fp2 tanh(fp2 x) {
            return new fp2(fpmath.tanh(x.x), fpmath.tanh(x.y));
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise hyperbolic tangent of a fp3 vector.</summary>
        public static fp3 tanh(fp3 x) {
            return new fp3(fpmath.tanh(x.x), fpmath.tanh(x.y), fpmath.tanh(x.z));
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise hyperbolic tangent of a fp4 vector.</summary>
        public static fp4 tanh(fp4 x) {
            return new fp4(fpmath.tanh(x.x), fpmath.tanh(x.y), fpmath.tanh(x.z), fpmath.tanh(x.w));
        }


        /// <summary>Returns the dot product of two fp values. Equivalent to multiplication.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp dot(fp x, fp y) {
            return x * y;
        }

        /// <summary>Returns the dot product of two fp2 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp dot(fp2 x, fp2 y) {
            return x.x * y.x + x.y * y.y;
        }

        /// <summary>Returns the dot product of two fp3 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp dot(fp3 x, fp3 y) {
            return x.x * y.x + x.y * y.y + x.z * y.z;
        }

        /// <summary>Returns the dot product of two fp4 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp dot(fp4 x, fp4 y) {
            return x.x * y.x + x.y * y.y + x.z * y.z + x.w * y.w;
        }

        /// <summary>Returns the dot product of two fp4 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp dot(fpquaternion x, fpquaternion y) {
            return x.x * y.x + x.y * y.y + x.z * y.z + x.w * y.w;
        }


        /// <summary>Returns the result of clamping the fp value x into the interval [0, 1].</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp saturate(fp x) {
            return fpmath.clamp(x, fp.zero, fp.one);
        }

        /// <summary>Returns the result of a componentwise clamping of the fp2 vector x into the interval [0, 1].</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 saturate(fp2 x) {
            return fpmath.clamp(x, new fp2(fp.zero), new fp2(fp.one));
        }

        /// <summary>Returns the result of a componentwise clamping of the fp3 vector x into the interval [0, 1].</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 saturate(fp3 x) {
            return fpmath.clamp(x, new fp3(fp.zero), new fp3(fp.one));
        }

        /// <summary>Returns the result of a componentwise clamping of the fp4 vector x into the interval [0, 1].</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 saturate(fp4 x) {
            return fpmath.clamp(x, new fp4(fp.zero), new fp4(fp.one));
        }


        /// <summary>Returns the result of clamping the value x into the interval [a, b], where x, a and b are fp values.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp clamp(fp x, fp a, fp b) {
            return fpmath.max(a, fpmath.min(b, x));
        }

        /// <summary>Returns the result of a componentwise clamping of the value x into the interval [a, b], where x, a and b are fp2 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 clamp(fp2 x, fp2 a, fp2 b) {
            return fpmath.max(a, fpmath.min(b, x));
        }

        /// <summary>Returns the result of a componentwise clamping of the value x into the interval [a, b], where x, a and b are fp3 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 clamp(fp3 x, fp3 a, fp3 b) {
            return fpmath.max(a, fpmath.min(b, x));
        }

        /// <summary>Returns the result of a componentwise clamping of the value x into the interval [a, b], where x, a and b are fp4 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 clamp(fp4 x, fp4 a, fp4 b) {
            return fpmath.max(a, fpmath.min(b, x));
        }


        /// <summary>Returns the result of a multiply-add operation (a * b + c) on 3 fp values.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp mad(fp a, fp b, fp c) {
            return a * b + c;
        }

        /// <summary>Returns the result of a componentwise multiply-add operation (a * b + c) on 3 fp2 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 mad(fp2 a, fp2 b, fp2 c) {
            return a * b + c;
        }

        /// <summary>Returns the result of a componentwise multiply-add operation (a * b + c) on 3 fp3 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 mad(fp3 a, fp3 b, fp3 c) {
            return a * b + c;
        }

        /// <summary>Returns the result of a componentwise multiply-add operation (a * b + c) on 3 fp4 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 mad(fp4 a, fp4 b, fp4 c) {
            return a * b + c;
        }


        /// <summary>Returns the result of a non-clamping linear remapping of a value x from [a, b] to [c, d].</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp remap(fp a, fp b, fp c, fp d, fp x) {
            return fpmath.lerp(c, d, fpmath.unlerp(a, b, x));
        }

        /// <summary>Returns the componentwise result of a non-clamping linear remapping of a value x from [a, b] to [c, d].</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 remap(fp2 a, fp2 b, fp2 c, fp2 d, fp2 x) {
            return fpmath.lerp(c, d, fpmath.unlerp(a, b, x));
        }

        /// <summary>Returns the componentwise result of a non-clamping linear remapping of a value x from [a, b] to [c, d].</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 remap(fp3 a, fp3 b, fp3 c, fp3 d, fp3 x) {
            return fpmath.lerp(c, d, fpmath.unlerp(a, b, x));
        }

        /// <summary>Returns the componentwise result of a non-clamping linear remapping of a value x from [a, b] to [c, d].</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 remap(fp4 a, fp4 b, fp4 c, fp4 d, fp4 x) {
            return fpmath.lerp(c, d, fpmath.unlerp(a, b, x));
        }


        /// <summary>Returns the result of normalizing a fping point value x to a range [a, b]. The opposite of lerp. Equivalent to (x - a) / (b - a).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp unlerp(fp a, fp b, fp x) {
            return (x - a) / (b - a);
        }

        /// <summary>Returns the componentwise result of normalizing a fping point value x to a range [a, b]. The opposite of lerp. Equivalent to (x - a) / (b - a).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 unlerp(fp2 a, fp2 b, fp2 x) {
            return (x - a) / (b - a);
        }

        /// <summary>Returns the componentwise result of normalizing a fping point value x to a range [a, b]. The opposite of lerp. Equivalent to (x - a) / (b - a).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 unlerp(fp3 a, fp3 b, fp3 x) {
            return (x - a) / (b - a);
        }

        /// <summary>Returns the componentwise result of normalizing a fping point value x to a range [a, b]. The opposite of lerp. Equivalent to (x - a) / (b - a).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 unlerp(fp4 a, fp4 b, fp4 x) {
            return (x - a) / (b - a);
        }

        public static fp repeat(fp t, fp length) {
            return fpmath.clamp(t - fpmath.floor(t / length) * length, 0.0f, length);
        }

        public static fp lerpangle(fp a, fp b, fp t) {
            var num = fpmath.repeat(b - a, 360f);
            if (num > (fp)180.0f) num -= (fp)360f;
            return a + num * fpmath.clamp(t, 0f, 1f);
        }
        
        /// <summary>Returns the result of linearly interpolating from x to y using the interpolation parameter s.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp lerp(fp x, fp y, fp s) {
            return x + s * (y - x);
        }

        /// <summary>Returns the result of a componentwise linear interpolating from x to y using the interpolation parameter s.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 lerp(fp2 x, fp2 y, fp s) {
            return x + s * (y - x);
        }

        /// <summary>Returns the result of a componentwise linear interpolating from x to y using the interpolation parameter s.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 lerp(fp3 x, fp3 y, fp s) {
            return x + s * (y - x);
        }

        /// <summary>Returns the result of a componentwise linear interpolating from x to y using the interpolation parameter s.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 lerp(fp4 x, fp4 y, fp s) {
            return x + s * (y - x);
        }


        /// <summary>Returns the result of a componentwise linear interpolating from x to y using the corresponding components of the interpolation parameter s.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 lerp(fp2 x, fp2 y, fp2 s) {
            return x + s * (y - x);
        }

        /// <summary>Returns the result of a componentwise linear interpolating from x to y using the corresponding components of the interpolation parameter s.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 lerp(fp3 x, fp3 y, fp3 s) {
            return x + s * (y - x);
        }

        /// <summary>Returns the result of a componentwise linear interpolating from x to y using the corresponding components of the interpolation parameter s.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 lerp(fp4 x, fp4 y, fp4 s) {
            return x + s * (y - x);
        }


        /// <summary>Returns the maximum of two fp values.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp max(fp x, fp y) {
            return x > y ? x : y;
        }

        /// <summary>Returns the componentwise maximum of two fp2 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 max(fp2 x, fp2 y) {
            return new fp2(fpmath.max(x.x, y.x), fpmath.max(x.y, y.y));
        }

        /// <summary>Returns the componentwise maximum of two fp3 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 max(fp3 x, fp3 y) {
            return new fp3(fpmath.max(x.x, y.x), fpmath.max(x.y, y.y), fpmath.max(x.z, y.z));
        }

        /// <summary>Returns the componentwise maximum of two fp4 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 max(fp4 x, fp4 y) {
            return new fp4(fpmath.max(x.x, y.x), fpmath.max(x.y, y.y), fpmath.max(x.z, y.z), fpmath.max(x.w, y.w));
        }


        /// <summary>Returns the minimum of two fp values.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp min(fp x, fp y) {
            return x < y ? x : y;
        }

        /// <summary>Returns the componentwise minimum of two fp2 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 min(fp2 x, fp2 y) {
            return new fp2(fpmath.min(x.x, y.x), fpmath.min(x.y, y.y));
        }

        /// <summary>Returns the componentwise minimum of two fp3 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 min(fp3 x, fp3 y) {
            return new fp3(fpmath.min(x.x, y.x), fpmath.min(x.y, y.y), fpmath.min(x.z, y.z));
        }

        /// <summary>Returns the componentwise minimum of two fp4 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 min(fp4 x, fp4 y) {
            return new fp4(fpmath.min(x.x, y.x), fpmath.min(x.y, y.y), fpmath.min(x.z, y.z), fpmath.min(x.w, y.w));
        }


        /// <summary>Returns true if the input fp is a NaN (not a number) fping point value, false otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool isnan(fp x) {
            return false;
        }

        /// <summary>Returns a bool2 indicating for each component of a fp2 whether it is a NaN (not a number) fping point value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 isnan(fp2 x) {
            return false;
        }

        /// <summary>Returns a bool3 indicating for each component of a fp3 whether it is a NaN (not a number) fping point value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3 isnan(fp3 x) {
            return false;
        }

        /// <summary>Returns a bool4 indicating for each component of a fp4 whether it is a NaN (not a number) fping point value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4 isnan(fp4 x) {
            return false;
        }


        /// <summary>Returns true if the input fp is an infinite fping point value, false otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool isinf(fp x) {
            return false;
        }

        /// <summary>Returns a bool2 indicating for each component of a fp2 whether it is an infinite fping point value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 isinf(fp2 x) {
            return false;
        }

        /// <summary>Returns a bool3 indicating for each component of a fp3 whether it is an infinite fping point value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3 isinf(fp3 x) {
            return false;
        }

        /// <summary>Returns a bool4 indicating for each component of a fp4 whether it is an infinite fping point value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4 isinf(fp4 x) {
            return false;
        }


        /// <summary>Returns true if the input fp is a finite fping point value, false otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool isfinite(fp x) {
            return true;
        }

        /// <summary>Returns a bool2 indicating for each component of a fp2 whether it is a finite fping point value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 isfinite(fp2 x) {
            return true;
        }

        /// <summary>Returns a bool3 indicating for each component of a fp3 whether it is a finite fping point value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3 isfinite(fp3 x) {
            return true;
        }

        /// <summary>Returns a bool4 indicating for each component of a fp4 whether it is a finite fping point value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4 isfinite(fp4 x) {
            return true;
        }


        /// <summary>Returns the arctangent of a fp value.</summary>
        public static fp atan(fp x) {
            return fp.Atan(x);
        }

        /// <summary>Returns the componentwise arctangent of a fp2 vector.</summary>
        public static fp2 atan(fp2 x) {
            return new fp2(fpmath.atan(x.x), fpmath.atan(x.y));
        }

        /// <summary>Returns the componentwise arctangent of a fp3 vector.</summary>
        public static fp3 atan(fp3 x) {
            return new fp3(fpmath.atan(x.x), fpmath.atan(x.y), fpmath.atan(x.z));
        }

        /// <summary>Returns the componentwise arctangent of a fp4 vector.</summary>
        public static fp4 atan(fp4 x) {
            return new fp4(fpmath.atan(x.x), fpmath.atan(x.y), fpmath.atan(x.z), fpmath.atan(x.w));
        }


        /// <summary>Returns the 2-argument arctangent of a pair of fp values.</summary>
        public static fp atan2(fp y, fp x) {
            return fp.Atan2(y, x);
        }

        /// <summary>Returns the componentwise 2-argument arctangent of a pair of fps2 vectors.</summary>
        public static fp2 atan2(fp2 y, fp2 x) {
            return new fp2(fpmath.atan2(y.x, x.x), fpmath.atan2(y.y, x.y));
        }

        /// <summary>Returns the componentwise 2-argument arctangent of a pair of fps3 vectors.</summary>
        public static fp3 atan2(fp3 y, fp3 x) {
            return new fp3(fpmath.atan2(y.x, x.x), fpmath.atan2(y.y, x.y), fpmath.atan2(y.z, x.z));
        }

        /// <summary>Returns the componentwise 2-argument arctangent of a pair of fps4 vectors.</summary>
        public static fp4 atan2(fp4 y, fp4 x) {
            return new fp4(fpmath.atan2(y.x, x.x), fpmath.atan2(y.y, x.y), fpmath.atan2(y.z, x.z), fpmath.atan2(y.w, x.w));
        }


        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the hyperbolic cosine of a fp value.</summary>
        public static fp cosh(fp x) {
            throw new System.NotImplementedException("fp doesn't support cosh");
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise hyperbolic cosine of a fp2 vector.</summary>
        public static fp2 cosh(fp2 x) {
            return new fp2(fpmath.cosh(x.x), fpmath.cosh(x.y));
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise hyperbolic cosine of a fp3 vector.</summary>
        public static fp3 cosh(fp3 x) {
            return new fp3(fpmath.cosh(x.x), fpmath.cosh(x.y), fpmath.cosh(x.z));
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise hyperbolic cosine of a fp4 vector.</summary>
        public static fp4 cosh(fp4 x) {
            return new fp4(fpmath.cosh(x.x), fpmath.cosh(x.y), fpmath.cosh(x.z), fpmath.cosh(x.w));
        }


        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the hyperbolic sine of a fp value.</summary>
        public static fp sinh(fp x) {
            throw new System.NotImplementedException("fp doesn't support sinh");
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise hyperbolic sine of a fp2 vector.</summary>
        public static fp2 sinh(fp2 x) {
            return new fp2(fpmath.sinh(x.x), fpmath.sinh(x.y));
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise hyperbolic sine of a fp3 vector.</summary>
        public static fp3 sinh(fp3 x) {
            return new fp3(fpmath.sinh(x.x), fpmath.sinh(x.y), fpmath.sinh(x.z));
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise hyperbolic sine of a fp4 vector.</summary>
        public static fp4 sinh(fp4 x) {
            return new fp4(fpmath.sinh(x.x), fpmath.sinh(x.y), fpmath.sinh(x.z), fpmath.sinh(x.w));
        }


        /// <summary>Returns the result of rounding a fp value up to the nearest integral value less or equal to the original value.</summary>
        public static fp floor(fp x) {
            return fp.Floor(x);
        }

        /// <summary>Returns the result of rounding each component of a fp2 vector value down to the nearest value less or equal to the original value.</summary>
        public static fp2 floor(fp2 x) {
            return new fp2(fpmath.floor(x.x), fpmath.floor(x.y));
        }

        /// <summary>Returns the result of rounding each component of a fp3 vector value down to the nearest value less or equal to the original value.</summary>
        public static fp3 floor(fp3 x) {
            return new fp3(fpmath.floor(x.x), fpmath.floor(x.y), fpmath.floor(x.z));
        }

        /// <summary>Returns the result of rounding each component of a fp4 vector value down to the nearest value less or equal to the original value.</summary>
        public static fp4 floor(fp4 x) {
            return new fp4(fpmath.floor(x.x), fpmath.floor(x.y), fpmath.floor(x.z), fpmath.floor(x.w));
        }


        /// <summary>Returns the result of rounding a fp value up to the nearest integral value greater or equal to the original value.</summary>
        public static fp ceil(fp x) {
            return fp.Ceiling(x);
        }

        /// <summary>Returns the result of rounding each component of a fp2 vector value up to the nearest value greater or equal to the original value.</summary>
        public static fp2 ceil(fp2 x) {
            return new fp2(fpmath.ceil(x.x), fpmath.ceil(x.y));
        }

        /// <summary>Returns the result of rounding each component of a fp3 vector value up to the nearest value greater or equal to the original value.</summary>
        public static fp3 ceil(fp3 x) {
            return new fp3(fpmath.ceil(x.x), fpmath.ceil(x.y), fpmath.ceil(x.z));
        }

        /// <summary>Returns the result of rounding each component of a fp4 vector value up to the nearest value greater or equal to the original value.</summary>
        public static fp4 ceil(fp4 x) {
            return new fp4(fpmath.ceil(x.x), fpmath.ceil(x.y), fpmath.ceil(x.z), fpmath.ceil(x.w));
        }


        /// <summary>Returns the result of rounding a fp value to the nearest integral value.</summary>
        public static fp round(fp x) {
            return fp.Round(x);
        }

        /// <summary>Returns the result of rounding each component of a fp2 vector value to the nearest integral value.</summary>
        public static fp2 round(fp2 x) {
            return new fp2(fpmath.round(x.x), fpmath.round(x.y));
        }

        /// <summary>Returns the result of rounding each component of a fp3 vector value to the nearest integral value.</summary>
        public static fp3 round(fp3 x) {
            return new fp3(fpmath.round(x.x), fpmath.round(x.y), fpmath.round(x.z));
        }

        /// <summary>Returns the result of rounding each component of a fp4 vector value to the nearest integral value.</summary>
        public static fp4 round(fp4 x) {
            return new fp4(fpmath.round(x.x), fpmath.round(x.y), fpmath.round(x.z), fpmath.round(x.w));
        }


        /// <summary>Returns the result of truncating a fp value to an integral fp value.</summary>
        public static fp trunc(fp x) {
            return fp.Truncate(x);
        }

        /// <summary>Returns the result of a componentwise truncation of a fp2 value to an integral fp2 value.</summary>
        public static fp2 trunc(fp2 x) {
            return new fp2(fpmath.trunc(x.x), fpmath.trunc(x.y));
        }

        /// <summary>Returns the result of a componentwise truncation of a fp3 value to an integral fp3 value.</summary>
        public static fp3 trunc(fp3 x) {
            return new fp3(fpmath.trunc(x.x), fpmath.trunc(x.y), fpmath.trunc(x.z));
        }

        /// <summary>Returns the result of a componentwise truncation of a fp4 value to an integral fp4 value.</summary>
        public static fp4 trunc(fp4 x) {
            return new fp4(fpmath.trunc(x.x), fpmath.trunc(x.y), fpmath.trunc(x.z), fpmath.trunc(x.w));
        }


        /// <summary>Returns the fractional part of a fp value.</summary>
        public static fp frac(fp x) {
            return x - fpmath.floor(x);
        }

        /// <summary>Returns the componentwise fractional parts of a fp2 vector.</summary>
        public static fp2 frac(fp2 x) {
            return x - fpmath.floor(x);
        }

        /// <summary>Returns the componentwise fractional parts of a fp3 vector.</summary>
        public static fp3 frac(fp3 x) {
            return x - fpmath.floor(x);
        }

        /// <summary>Returns the componentwise fractional parts of a fp4 vector.</summary>
        public static fp4 frac(fp4 x) {
            return x - fpmath.floor(x);
        }


        /// <summary>Returns the reciprocal a fp value.</summary>
        public static fp rcp(fp x) {
            return fp.one / x;
        }

        /// <summary>Returns the componentwise reciprocal a fp2 vector.</summary>
        public static fp2 rcp(fp2 x) {
            return fp.one / x;
        }

        /// <summary>Returns the componentwise reciprocal a fp3 vector.</summary>
        public static fp3 rcp(fp3 x) {
            return fp.one / x;
        }

        /// <summary>Returns the componentwise reciprocal a fp4 vector.</summary>
        public static fp4 rcp(fp4 x) {
            return fp.one / x;
        }


        /// <summary>Returns the sign of a fp value. -1.0f if it is less than zero, 0.0f if it is zero and 1.0f if it greater than zero.</summary>
        public static fp sign(fp x) {
            return fp.Sign(x);
        }

        /// <summary>Returns the componentwise sign of a fp2 value. 1.0f for positive components, 0.0f for zero components and -1.0f for negative components.</summary>
        public static fp2 sign(fp2 x) {
            return new fp2(fpmath.sign(x.x), fpmath.sign(x.y));
        }

        /// <summary>Returns the componentwise sign of a fp3 value. 1.0f for positive components, 0.0f for zero components and -1.0f for negative components.</summary>
        public static fp3 sign(fp3 x) {
            return new fp3(fpmath.sign(x.x), fpmath.sign(x.y), fpmath.sign(x.z));
        }

        /// <summary>Returns the componentwise sign of a fp4 value. 1.0f for positive components, 0.0f for zero components and -1.0f for negative components.</summary>
        public static fp4 sign(fp4 x) {
            return new fp4(fpmath.sign(x.x), fpmath.sign(x.y), fpmath.sign(x.z), fpmath.sign(x.w));
        }


        /// <summary>Returns x raised to the power y.</summary>
        public static fp pow(fp x, fp y) {
            return fp.Pow(x, y);
        }

        /// <summary>Returns the componentwise result of raising x to the power y.</summary>
        public static fp2 pow(fp2 x, fp2 y) {
            return new fp2(fpmath.pow(x.x, y.x), fpmath.pow(x.y, y.y));
        }

        /// <summary>Returns the componentwise result of raising x to the power y.</summary>
        public static fp3 pow(fp3 x, fp3 y) {
            return new fp3(fpmath.pow(x.x, y.x), fpmath.pow(x.y, y.y), fpmath.pow(x.z, y.z));
        }

        /// <summary>Returns the componentwise result of raising x to the power y.</summary>
        public static fp4 pow(fp4 x, fp4 y) {
            return new fp4(fpmath.pow(x.x, y.x), fpmath.pow(x.y, y.y), fpmath.pow(x.z, y.z), fpmath.pow(x.w, y.w));
        }


        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the base-e exponential of x.</summary>
        public static fp exp(fp x) {
            throw new System.NotImplementedException("fp doesn't support exp");
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise base-e exponential of x.</summary>
        public static fp2 exp(fp2 x) {
            return new fp2(fpmath.exp(x.x), fpmath.exp(x.y));
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise base-e exponential of x.</summary>
        public static fp3 exp(fp3 x) {
            return new fp3(fpmath.exp(x.x), fpmath.exp(x.y), fpmath.exp(x.z));
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise base-e exponential of x.</summary>
        public static fp4 exp(fp4 x) {
            return new fp4(fpmath.exp(x.x), fpmath.exp(x.y), fpmath.exp(x.z), fpmath.exp(x.w));
        }


        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the base-2 exponential of x.</summary>
        public static fp exp2(fp x) {
            return fpmath.exp(x * 0.69314718d);
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise base-2 exponential of x.</summary>
        public static fp2 exp2(fp2 x) {
            return new fp2(fpmath.exp2(x.x), fpmath.exp2(x.y));
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise base-2 exponential of x.</summary>
        public static fp3 exp2(fp3 x) {
            return new fp3(fpmath.exp2(x.x), fpmath.exp2(x.y), fpmath.exp2(x.z));
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise base-2 exponential of x.</summary>
        public static fp4 exp2(fp4 x) {
            return new fp4(fpmath.exp2(x.x), fpmath.exp2(x.y), fpmath.exp2(x.z), fpmath.exp2(x.w));
        }


        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the base-10 exponential of x.</summary>
        public static fp exp10(fp x) {
            return fpmath.exp(x * 2.30258509d);
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise base-10 exponential of x.</summary>
        public static fp2 exp10(fp2 x) {
            return new fp2(fpmath.exp10(x.x), fpmath.exp10(x.y));
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise base-10 exponential of x.</summary>
        public static fp3 exp10(fp3 x) {
            return new fp3(fpmath.exp10(x.x), fpmath.exp10(x.y), fpmath.exp10(x.z));
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise base-10 exponential of x.</summary>
        public static fp4 exp10(fp4 x) {
            return new fp4(fpmath.exp10(x.x), fpmath.exp10(x.y), fpmath.exp10(x.z), fpmath.exp10(x.w));
        }


        /// <summary>Returns the natural logarithm of a fp value.</summary>
        public static fp log(fp x) {
            return fp.Ln(x);
        }

        /// <summary>Returns the componentwise natural logarithm of a fp2 vector.</summary>
        public static fp2 log(fp2 x) {
            return new fp2(fpmath.log(x.x), fpmath.log(x.y));
        }

        /// <summary>Returns the componentwise natural logarithm of a fp3 vector.</summary>
        public static fp3 log(fp3 x) {
            return new fp3(fpmath.log(x.x), fpmath.log(x.y), fpmath.log(x.z));
        }

        /// <summary>Returns the componentwise natural logarithm of a fp4 vector.</summary>
        public static fp4 log(fp4 x) {
            return new fp4(fpmath.log(x.x), fpmath.log(x.y), fpmath.log(x.z), fpmath.log(x.w));
        }


        /// <summary>Returns the base-2 logarithm of a fp value.</summary>
        public static fp log2(fp x) {
            return fp.Log2(x);
        }

        /// <summary>Returns the componentwise base-2 logarithm of a fp2 vector.</summary>
        public static fp2 log2(fp2 x) {
            return new fp2(fpmath.log2(x.x), fpmath.log2(x.y));
        }

        /// <summary>Returns the componentwise base-2 logarithm of a fp3 vector.</summary>
        public static fp3 log2(fp3 x) {
            return new fp3(fpmath.log2(x.x), fpmath.log2(x.y), fpmath.log2(x.z));
        }

        /// <summary>Returns the componentwise base-2 logarithm of a fp4 vector.</summary>
        public static fp4 log2(fp4 x) {
            return new fp4(fpmath.log2(x.x), fpmath.log2(x.y), fpmath.log2(x.z), fpmath.log2(x.w));
        }


        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the base-10 logarithm of a fp value.</summary>
        public static fp log10(fp x) {
            throw new System.NotImplementedException("fp doesn't support log10");
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise base-10 logarithm of a fp2 vector.</summary>
        public static fp2 log10(fp2 x) {
            return new fp2(fpmath.log10(x.x), fpmath.log10(x.y));
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise base-10 logarithm of a fp3 vector.</summary>
        public static fp3 log10(fp3 x) {
            return new fp3(fpmath.log10(x.x), fpmath.log10(x.y), fpmath.log10(x.z));
        }

        [System.Obsolete(fpmath.NOT_SUPPORTED_YET, true)]
        /// <summary>Returns the componentwise base-10 logarithm of a fp4 vector.</summary>
        public static fp4 log10(fp4 x) {
            return new fp4(fpmath.log10(x.x), fpmath.log10(x.y), fpmath.log10(x.z), fpmath.log10(x.w));
        }


        /// <summary>Returns the fping point remainder of x/y.</summary>
        public static fp fmod(fp x, fp y) {
            return x % y;
        }

        /// <summary>Returns the componentwise fping point remainder of x/y.</summary>
        public static fp2 fmod(fp2 x, fp2 y) {
            return new fp2(x.x % y.x, x.y % y.y);
        }

        /// <summary>Returns the componentwise fping point remainder of x/y.</summary>
        public static fp3 fmod(fp3 x, fp3 y) {
            return new fp3(x.x % y.x, x.y % y.y, x.z % y.z);
        }

        /// <summary>Returns the componentwise fping point remainder of x/y.</summary>
        public static fp4 fmod(fp4 x, fp4 y) {
            return new fp4(x.x % y.x, x.y % y.y, x.z % y.z, x.w % y.w);
        }


        /// <summary>Splits a fp value into an integral part i and a fractional part that gets returned. Both parts take the sign of the input.</summary>
        public static fp modf(fp x, out fp i) {
            i = fpmath.trunc(x);
            return x - i;
        }

        /// <summary>
        // Performs a componentwise split of a fp2 vector into an integral part i and a fractional part that gets returned.
        // Both parts take the sign of the corresponding input component.
        // </summary>
        public static fp2 modf(fp2 x, out fp2 i) {
            i = fpmath.trunc(x);
            return x - i;
        }

        /// <summary>
        // Performs a componentwise split of a fp3 vector into an integral part i and a fractional part that gets returned.
        // Both parts take the sign of the corresponding input component.
        // </summary>
        public static fp3 modf(fp3 x, out fp3 i) {
            i = fpmath.trunc(x);
            return x - i;
        }

        /// <summary>
        // Performs a componentwise split of a fp4 vector into an integral part i and a fractional part that gets returned.
        // Both parts take the sign of the corresponding input component.
        // </summary>
        public static fp4 modf(fp4 x, out fp4 i) {
            i = fpmath.trunc(x);
            return x - i;
        }


        /// <summary>Returns the square root of a fp value.</summary>
        public static fp sqrt(fp x) {
            return fp.Sqrt(x);
        }

        /// <summary>Returns the componentwise square root of a fp2 vector.</summary>
        public static fp2 sqrt(fp2 x) {
            return new fp2(fpmath.sqrt(x.x), fpmath.sqrt(x.y));
        }

        /// <summary>Returns the componentwise square root of a fp3 vector.</summary>
        public static fp3 sqrt(fp3 x) {
            return new fp3(fpmath.sqrt(x.x), fpmath.sqrt(x.y), fpmath.sqrt(x.z));
        }

        /// <summary>Returns the componentwise square root of a fp4 vector.</summary>
        public static fp4 sqrt(fp4 x) {
            return new fp4(fpmath.sqrt(x.x), fpmath.sqrt(x.y), fpmath.sqrt(x.z), fpmath.sqrt(x.w));
        }


        /// <summary>Returns the reciprocal square root of a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp rsqrt(fp x) {
            return fp.one / fpmath.sqrt(x);
        }

        /// <summary>Returns the componentwise reciprocal square root of a fp2 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 rsqrt(fp2 x) {
            return fp.one / fpmath.sqrt(x);
        }

        /// <summary>Returns the componentwise reciprocal square root of a fp3 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 rsqrt(fp3 x) {
            return fp.one / fpmath.sqrt(x);
        }

        /// <summary>Returns the componentwise reciprocal square root of a fp4 vector</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 rsqrt(fp4 x) {
            return fp.one / fpmath.sqrt(x);
        }


        /// <summary>Returns a normalized version of the fp2 vector x by scaling it by 1 / length(x).</summary>
        public static fp2 normalize(fp2 x) {
            return fpmath.rsqrt(fpmath.dot(x, x)) * x;
        }

        /// <summary>Returns a normalized version of the fp3 vector x by scaling it by 1 / length(x).</summary>
        public static fp3 normalize(fp3 x) {
            return fpmath.rsqrt(fpmath.dot(x, x)) * x;
        }

        /// <summary>Returns a normalized version of the fp4 vector x by scaling it by 1 / length(x).</summary>
        public static fp4 normalize(fp4 x) {
            return fpmath.rsqrt(fpmath.dot(x, x)) * x;
        }

        /// <summary>
        /// Returns a safe normalized version of the fp2 vector x by scaling it by 1 / length(x).
        /// Returns the given default value when 1 / length(x) does not produce a finite number.
        /// </summary>
        public static fp2 normalizesafe(fp2 x, fp2 defaultvalue = new fp2()) {
            var len = fpmath.dot(x, x);
            if (len > 0.00000001d) return x * fpmath.rsqrt(len);
            return defaultvalue;
        }

        /// <summary>
        /// Returns a safe normalized version of the fp3 vector x by scaling it by 1 / length(x).
        /// Returns the given default value when 1 / length(x) does not produce a finite number.
        /// </summary>
        public static fp3 normalizesafe(fp3 x, fp3 defaultvalue = new fp3()) {
            var len = fpmath.dot(x, x);
            if (len > 0.00000001d) return x * fpmath.rsqrt(len);
            return defaultvalue;
        }

        /// <summary>
        /// Returns a safe normalized version of the fp4 vector x by scaling it by 1 / length(x).
        /// Returns the given default value when 1 / length(x) does not produce a finite number.
        /// </summary>
        public static fp4 normalizesafe(fp4 x, fp4 defaultvalue = new fp4()) {
            var len = fpmath.dot(x, x);
            if (len > 0.00000001d) return x * fpmath.rsqrt(len);
            return defaultvalue;
        }


        /// <summary>Returns the length of a fp value. Equivalent to the absolute value.</summary>
        public static fp length(fpquaternion x) {
            return fpmath.sqrt(fpmath.dot(x, x));
        }

        /// <summary>Returns the length of a fp value. Equivalent to the absolute value.</summary>
        public static fp length(fp x) {
            return fpmath.abs(x);
        }

        /// <summary>Returns the length of a fp2 vector.</summary>
        public static fp length(fp2 x) {
            return fpmath.sqrt(fpmath.dot(x, x));
        }

        /// <summary>Returns the length of a fp3 vector.</summary>
        public static fp length(fp3 x) {
            return fpmath.sqrt(fpmath.dot(x, x));
        }

        /// <summary>Returns the length of a fp4 vector.</summary>
        public static fp length(fp4 x) {
            return fpmath.sqrt(fpmath.dot(x, x));
        }


        /// <summary>Returns the squared length of a fp value. Equivalent to squaring the value.</summary>
        public static fp lengthsq(fp x) {
            return x * x;
        }

        /// <summary>Returns the squared length of a fp2 vector.</summary>
        public static fp lengthsq(fp2 x) {
            return fpmath.dot(x, x);
        }

        /// <summary>Returns the squared length of a fp3 vector.</summary>
        public static fp lengthsq(fp3 x) {
            return fpmath.dot(x, x);
        }

        /// <summary>Returns the squared length of a fp4 vector.</summary>
        public static fp lengthsq(fp4 x) {
            return fpmath.dot(x, x);
        }


        /// <summary>Returns the distance between two fp values.</summary>
        public static fp distance(fp x, fp y) {
            return fpmath.abs(y - x);
        }

        /// <summary>Returns the distance between two fp2 vectors.</summary>
        public static fp distance(fp2 x, fp2 y) {
            return fpmath.length(y - x);
        }

        /// <summary>Returns the distance between two fp3 vectors.</summary>
        public static fp distance(fp3 x, fp3 y) {
            return fpmath.length(y - x);
        }

        /// <summary>Returns the distance between two fp4 vectors.</summary>
        public static fp distance(fp4 x, fp4 y) {
            return fpmath.length(y - x);
        }


        /// <summary>Returns the distance between two fp values.</summary>
        public static fp distancesq(fp x, fp y) {
            return (y - x) * (y - x);
        }

        /// <summary>Returns the distance between two fp2 vectors.</summary>
        public static fp distancesq(fp2 x, fp2 y) {
            return fpmath.lengthsq(y - x);
        }

        /// <summary>Returns the distance between two fp3 vectors.</summary>
        public static fp distancesq(fp3 x, fp3 y) {
            return fpmath.lengthsq(y - x);
        }

        /// <summary>Returns the distance between two fp4 vectors.</summary>
        public static fp distancesq(fp4 x, fp4 y) {
            return fpmath.lengthsq(y - x);
        }


        /// <summary>Returns the cross product of two fp3 vectors.</summary>
        public static fp3 cross(fp3 x, fp3 y) {
            return (x * y.yzx - x.yzx * y).yzx;
        }


        /// <summary>Returns a smooth Hermite interpolation between 0.0f and 1.0f when x is in [a, b].</summary>
        public static fp smoothstep(fp a, fp b, fp x) {
            var t = fpmath.saturate((x - a) / (b - a));
            return t * t * ((fp)3 - (fp)2 * t);
        }

        /// <summary>Returns a componentwise smooth Hermite interpolation between 0.0f and 1.0f when x is in [a, b].</summary>
        public static fp2 smoothstep(fp2 a, fp2 b, fp2 x) {
            var t = fpmath.saturate((x - a) / (b - a));
            return t * t * ((fp)3 - (fp)2 * t);
        }

        /// <summary>Returns a componentwise smooth Hermite interpolation between 0.0f and 1.0f when x is in [a, b].</summary>
        public static fp3 smoothstep(fp3 a, fp3 b, fp3 x) {
            var t = fpmath.saturate((x - a) / (b - a));
            return t * t * ((fp)3 - (fp)2 * t);
        }

        /// <summary>Returns a componentwise smooth Hermite interpolation between 0.0f and 1.0f when x is in [a, b].</summary>
        public static fp4 smoothstep(fp4 a, fp4 b, fp4 x) {
            var t = fpmath.saturate((x - a) / (b - a));
            return t * t * ((fp)3 - (fp)2 * t);
        }


        /// <summary>Returns true if any component of the input fp2 vector is non-zero, false otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(fp2 x) {
            return x.x != fp.zero || x.y != fp.zero;
        }

        /// <summary>Returns true if any component of the input fp3 vector is non-zero, false otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(fp3 x) {
            return x.x != fp.zero || x.y != fp.zero || x.z != fp.zero;
        }

        /// <summary>Returns true if any component of the input fp4 vector is non-zero, false otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(fp4 x) {
            return x.x != fp.zero || x.y != fp.zero || x.z != fp.zero || x.w != fp.zero;
        }


        /// <summary>Returns true if all components of the input fp2 vector are non-zero, false otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(fp2 x) {
            return x.x != fp.zero && x.y != fp.zero;
        }

        /// <summary>Returns true if all components of the input fp3 vector are non-zero, false otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(fp3 x) {
            return x.x != fp.zero && x.y != fp.zero && x.z != fp.zero;
        }

        /// <summary>Returns true if all components of the input fp4 vector are non-zero, false otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(fp4 x) {
            return x.x != fp.zero && x.y != fp.zero && x.z != fp.zero && x.w != fp.zero;
        }


        /// <summary>Returns b if c is true, a otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp select(fp a, fp b, bool c) {
            return c ? b : a;
        }

        /// <summary>Returns b if c is true, a otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 select(fp2 a, fp2 b, bool c) {
            return c ? b : a;
        }

        /// <summary>Returns b if c is true, a otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 select(fp3 a, fp3 b, bool c) {
            return c ? b : a;
        }

        /// <summary>Returns b if c is true, a otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 select(fp4 a, fp4 b, bool c) {
            return c ? b : a;
        }


        /// <summary>
        /// Returns a componentwise selection between two fp2 vectors a and b based on a bool2 selection mask c.
        /// Per component, the component from b is selected when c is true, otherwise the component from a is selected.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 select(fp2 a, fp2 b, bool2 c) {
            return new fp2(c.x ? b.x : a.x, c.y ? b.y : a.y);
        }

        /// <summary>
        /// Returns a componentwise selection between two fp3 vectors a and b based on a bool3 selection mask c.
        /// Per component, the component from b is selected when c is true, otherwise the component from a is selected.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 select(fp3 a, fp3 b, bool3 c) {
            return new fp3(c.x ? b.x : a.x, c.y ? b.y : a.y, c.z ? b.z : a.z);
        }

        /// <summary>
        /// Returns a componentwise selection between two fp4 vectors a and b based on a bool4 selection mask c.
        /// Per component, the component from b is selected when c is true, otherwise the component from a is selected.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 select(fp4 a, fp4 b, bool4 c) {
            return new fp4(c.x ? b.x : a.x, c.y ? b.y : a.y, c.z ? b.z : a.z, c.w ? b.w : a.w);
        }


        /// <summary>Computes a step function. Returns 1.0f when x >= y, 0.0f otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp step(fp y, fp x) {
            return fpmath.@select(fp.zero, fp.one, x >= y);
        }

        /// <summary>Returns the result of a componentwise step function where each component is 1.0f when x >= y and 0.0f otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 step(fp2 y, fp2 x) {
            return fpmath.@select(fpmath.fp2(fp.zero), fpmath.fp2(fp.one), x >= y);
        }

        /// <summary>Returns the result of a componentwise step function where each component is fp.one when x >= y and fp.zero otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 step(fp3 y, fp3 x) {
            return fpmath.@select(fpmath.fp3(fp.zero), fpmath.fp3(fp.one), x >= y);
        }

        /// <summary>Returns the result of a componentwise step function where each component is fp.one when x >= y and fp.zero otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 step(fp4 y, fp4 x) {
            return fpmath.@select(fpmath.fp4(fp.zero), fpmath.fp4(fp.one), x >= y);
        }


        /// <summary>Given an incident vector i and a normal vector n, returns the reflection vector r = i - 2.0f * dot(i, n) * n.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 reflect(fp2 i, fp2 n) {
            return i - 2 * n * fpmath.dot(i, n);
        }

        /// <summary>Given an incident vector i and a normal vector n, returns the reflection vector r = i - 2.0f * dot(i, n) * n.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 reflect(fp3 i, fp3 n) {
            return i - 2 * n * fpmath.dot(i, n);
        }

        /// <summary>Given an incident vector i and a normal vector n, returns the reflection vector r = i - 2.0f * dot(i, n) * n.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 reflect(fp4 i, fp4 n) {
            return i - 2 * n * fpmath.dot(i, n);
        }


        /// <summary>Returns the refraction vector given the incident vector i, the normal vector n and the refraction index eta.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 refract(fp2 i, fp2 n, fp eta) {
            var ni = fpmath.dot(n, i);
            var k = fp.one - eta * eta * (fp.one - ni * ni);
            return fpmath.@select(fp.zero, eta * i - (eta * ni + fpmath.sqrt(k)) * n, k >= 0f);
        }

        /// <summary>Returns the refraction vector given the incident vector i, the normal vector n and the refraction index eta.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 refract(fp3 i, fp3 n, fp eta) {
            var ni = fpmath.dot(n, i);
            var k = fp.one - eta * eta * (fp.one - ni * ni);
            return fpmath.@select(fp.zero, eta * i - (eta * ni + fpmath.sqrt(k)) * n, k >= 0f);
        }

        /// <summary>Returns the refraction vector given the incident vector i, the normal vector n and the refraction index eta.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 refract(fp4 i, fp4 n, fp eta) {
            var ni = fpmath.dot(n, i);
            var k = fp.one - eta * eta * (fp.one - ni * ni);
            return fpmath.@select(fp.zero, eta * i - (eta * ni + fpmath.sqrt(k)) * n, k >= 0f);
        }


        /// <summary>Conditionally flips a vector n to face in the direction of i. Returns n if dot(i, ng) < 0, -n otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 faceforward(fp2 n, fp2 i, fp2 ng) {
            return fpmath.@select(n, -n, fpmath.dot(ng, i) >= fp.zero);
        }

        /// <summary>Conditionally flips a vector n to face in the direction of i. Returns n if dot(i, ng) < 0, -n otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 faceforward(fp3 n, fp3 i, fp3 ng) {
            return fpmath.@select(n, -n, fpmath.dot(ng, i) >= fp.zero);
        }

        /// <summary>Conditionally flips a vector n to face in the direction of i. Returns n if dot(i, ng) < 0, -n otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 faceforward(fp4 n, fp4 i, fp4 ng) {
            return fpmath.@select(n, -n, fpmath.dot(ng, i) >= fp.zero);
        }


        /// <summary>Returns the sine and cosine of the input fp value x through the out parameters s and c.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void sincos(fp x, out fp s, out fp c) {
            s = fpmath.sin(x);
            c = fpmath.cos(x);
        }

        /// <summary>Returns the componentwise sine and cosine of the input fp2 vector x through the out parameters s and c.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void sincos(fp2 x, out fp2 s, out fp2 c) {
            s = fpmath.sin(x);
            c = fpmath.cos(x);
        }

        /// <summary>Returns the componentwise sine and cosine of the input fp3 vector x through the out parameters s and c.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void sincos(fp3 x, out fp3 s, out fp3 c) {
            s = fpmath.sin(x);
            c = fpmath.cos(x);
        }

        /// <summary>Returns the componentwise sine and cosine of the input fp4 vector x through the out parameters s and c.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void sincos(fp4 x, out fp4 s, out fp4 c) {
            s = fpmath.sin(x);
            c = fpmath.cos(x);
        }


        /// <summary>Returns the result of converting a fp value from degrees to radians.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp radians(fp x) {
            return x * 0.0174532925d;
        }

        /// <summary>Returns the result of a componentwise conversion of a fp2 vector from degrees to radians.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 radians(fp2 x) {
            return x * 0.0174532925d;
        }

        /// <summary>Returns the result of a componentwise conversion of a fp3 vector from degrees to radians.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 radians(fp3 x) {
            return x * 0.0174532925d;
        }

        /// <summary>Returns the result of a componentwise conversion of a fp4 vector from degrees to radians.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 radians(fp4 x) {
            return x * 0.0174532925d;
        }


        /// <summary>Returns the result of converting a double value from radians to degrees.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp degrees(fp x) {
            return x * 57.295779513d;
        }

        /// <summary>Returns the result of a componentwise conversion of a double2 vector from radians to degrees.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 degrees(fp2 x) {
            return x * 57.295779513d;
        }

        /// <summary>Returns the result of a componentwise conversion of a double3 vector from radians to degrees.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3 degrees(fp3 x) {
            return x * 57.295779513d;
        }

        /// <summary>Returns the result of a componentwise conversion of a double4 vector from radians to degrees.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4 degrees(fp4 x) {
            return x * 57.295779513d;
        }


        /// <summary>Returns the minimum component of a fp2 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp cmin(fp2 x) {
            return fpmath.min(x.x, x.y);
        }

        /// <summary>Returns the minimum component of a fp3 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp cmin(fp3 x) {
            return fpmath.min(fpmath.min(x.x, x.y), x.z);
        }

        /// <summary>Returns the maximum component of a fp3 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp cmin(fp4 x) {
            return fpmath.min(fpmath.min(x.x, x.y), fpmath.min(x.z, x.w));
        }


        /// <summary>Returns the maximum component of a fp2 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp cmax(fp2 x) {
            return fpmath.max(x.x, x.y);
        }

        /// <summary>Returns the maximum component of a fp3 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp cmax(fp3 x) {
            return fpmath.max(fpmath.max(x.x, x.y), x.z);
        }

        /// <summary>Returns the maximum component of a fp4 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp cmax(fp4 x) {
            return fpmath.max(fpmath.max(x.x, x.y), fpmath.max(x.z, x.w));
        }


        /// <summary>Returns the horizontal sum of components of a fp2 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp csum(fp2 x) {
            return x.x + x.y;
        }

        /// <summary>Returns the horizontal sum of components of a fp3 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp csum(fp3 x) {
            return x.x + x.y + x.z;
        }

        /// <summary>Returns the horizontal sum of components of a fp4 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp csum(fp4 x) {
            return x.x + x.y + (x.z + x.w);
        }

    }

}