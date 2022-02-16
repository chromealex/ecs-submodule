
namespace ME.ECS {
    
    public static partial class FPMath {
        
        /// <summary>
        ///   <para>Degrees-to-radians conversion constant (Read Only).</para>
        /// </summary>
        public const float Deg2Rad = 0.017453292f;
        /// <summary>
        ///   <para>Radians-to-degrees conversion constant (Read Only).</para>
        /// </summary>
        public const float Rad2Deg = 57.29578f;

        public static int FloorToInt(pfloat v) {
            return UnityEngine.Mathf.FloorToInt(v);
        }
        
        /// <summary>
        /// Subtracts y from x witout performing overflow checking. Should be inlined by the CLR.
        /// </summary>
        public static pfloat FastSub(pfloat x, pfloat y) {
            return new pfloat(x.v - y.v);
        }

        internal static long AddOverflowHelper(long x, long y, ref bool overflow) {
            var sum = x + y;
            // x + y overflows if sign(x) ^ sign(y) != sign(sum)
            overflow |= ((x ^ y ^ sum) & pfloat.MIN_VALUE) != 0;
            return sum;
        }

        /// <summary>
        /// Performs multiplication without checking for overflow.
        /// Useful for performance-critical code where the values are guaranteed not to cause overflow
        /// </summary>
        public static pfloat FastMul(pfloat x, pfloat y) {

            var xl = x.v;
            var yl = y.v;

            var xlo = (ulong)(xl & 0x00000000FFFFFFFF);
            var xhi = xl >> pfloat.FRACTIONAL_PLACES;
            var ylo = (ulong)(yl & 0x00000000FFFFFFFF);
            var yhi = yl >> pfloat.FRACTIONAL_PLACES;

            var lolo = xlo * ylo;
            var lohi = (long)xlo * yhi;
            var hilo = xhi * (long)ylo;
            var hihi = xhi * yhi;

            var loResult = lolo >> pfloat.FRACTIONAL_PLACES;
            var midResult1 = lohi;
            var midResult2 = hilo;
            var hiResult = hihi << pfloat.FRACTIONAL_PLACES;

            var sum = (long)loResult + midResult1 + midResult2 + hiResult;
            return new pfloat(sum);
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal static int CountLeadingZeroes(ulong x) {
            var result = 0;
            while ((x & 0xF000000000000000) == 0) {
                result += 4;
                x <<= 4;
            }

            while ((x & 0x8000000000000000) == 0) {
                result += 1;
                x <<= 1;
            }

            return result;
        }

        /// <summary>
        /// Returns 2 raised to the specified power.
        /// Provides at least 6 decimals of accuracy.
        /// </summary>
        internal static pfloat Pow2(pfloat x) {
            if (x.v == 0) {
                return pfloat.One;
            }

            // Avoid negative arguments by exploiting that exp(-x) = 1/exp(x).
            var neg = x.v < 0;
            if (neg) {
                x = -x;
            }

            if (x == pfloat.One) {
                return neg ? pfloat.One / (pfloat)2 : (pfloat)2;
            }

            if (x >= pfloat.Log2Max) {
                return neg ? pfloat.One / pfloat.MaxValue : pfloat.MaxValue;
            }

            if (x <= pfloat.Log2Min) {
                return neg ? pfloat.MaxValue : pfloat.Zero;
            }

            var integerPart = (int)FPMath.Floor(x);
            // Take fractional part of exponent
            x = new pfloat(x.v & 0x00000000FFFFFFFF);

            var result = pfloat.One;
            var term = pfloat.One;
            var i = 1;
            while (term.v != 0) {
                term = FPMath.FastMul(FPMath.FastMul(x, term), pfloat.Ln2) / (pfloat)i;
                result += term;
                i++;
            }

            result = pfloat.FromRaw(result.v << integerPart);
            if (neg) {
                result = pfloat.One / result;
            }

            return result;
        }

        /// <summary>
        /// Returns the base-2 logarithm of a specified number.
        /// Provides at least 9 decimals of accuracy.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The argument was non-positive
        /// </exception>
        internal static pfloat Log2(pfloat x) {
            if (x.v <= 0) {
                throw new System.ArgumentOutOfRangeException("Non-positive value passed to Ln", "x");
            }

            // This implementation is based on Clay. S. Turner's fast binary logarithm
            // algorithm (C. S. Turner,  "A Fast Binary Logarithm Algorithm", IEEE Signal
            //     Processing Mag., pp. 124,140, Sep. 2010.)

            long b = 1U << (pfloat.FRACTIONAL_PLACES - 1);
            long y = 0;

            var rawX = x.v;
            while (rawX < pfloat.ONE) {
                rawX <<= 1;
                y -= pfloat.ONE;
            }

            while (rawX >= pfloat.ONE << 1) {
                rawX >>= 1;
                y += pfloat.ONE;
            }

            var z = new pfloat(rawX);

            for (var i = 0; i < pfloat.FRACTIONAL_PLACES; i++) {
                z = FPMath.FastMul(z, z);
                if (z.v >= pfloat.ONE << 1) {
                    z = new pfloat(z.v >> 1);
                    y += b;
                }

                b >>= 1;
            }

            return new pfloat(y);
        }

        /// <summary>
        /// Returns the natural logarithm of a specified number.
        /// Provides at least 7 decimals of accuracy.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The argument was non-positive
        /// </exception>
        public static pfloat Ln(pfloat x) {
            return FPMath.FastMul(FPMath.Log2(x), pfloat.Ln2);
        }

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// Provides about 5 digits of accuracy for the result.
        /// </summary>
        /// <exception cref="System.DivideByZeroException">
        /// The base was zero, with a negative exponent
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The base was negative, with a non-zero exponent
        /// </exception>
        public static pfloat Pow(pfloat b, pfloat exp) {
            if (b == pfloat.One) {
                return pfloat.One;
            }

            if (exp.v == 0) {
                return pfloat.One;
            }

            if (b.v == 0) {
                if (exp.v < 0) {
                    throw new System.DivideByZeroException();
                }

                return pfloat.Zero;
            }

            var log2 = FPMath.Log2(b);
            return FPMath.Pow2(exp * log2);
        }

        [System.Diagnostics.ConditionalAttribute("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private static void ThrowNegativeValueException() {
            throw new System.ArgumentOutOfRangeException("Negative value passed to Sqrt", "x");
        }

        /// <summary>
        /// Returns the square root of a specified number.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The argument was negative.
        /// </exception>
        public static pfloat Sqrt(pfloat x) {
            var xl = x.v;
            if (xl < 0) {
                // We cannot represent infinities like Single and Double, and Sqrt is
                // mathematically undefined for x < 0. So we just throw an exception.
                ThrowNegativeValueException();
                return default;
            }

            var num = (ulong)xl;
            var result = 0UL;

            // second-to-top bit
            var bit = 1UL << (pfloat.NUM_BITS - 2);

            while (bit > num) {
                bit >>= 2;
            }

            // The main part is executed twice, in order to avoid
            // using 128 bit values in computations.
            for (var i = 0; i < 2; ++i) {
                // First we get the top 48 bits of the answer.
                while (bit != 0) {
                    if (num >= result + bit) {
                        num -= result + bit;
                        result = (result >> 1) + bit;
                    } else {
                        result = result >> 1;
                    }

                    bit >>= 2;
                }

                if (i == 0) {
                    // Then process it again to get the lowest 16 bits.
                    if (num > (1UL << (pfloat.NUM_BITS / 2)) - 1) {
                        // The remainder 'num' is too large to be shifted left
                        // by 32, so we have to add 1 to result manually and
                        // adjust 'num' accordingly.
                        // num = a - (result + 0.5)^2
                        //       = num + result^2 - (result + 0.5)^2
                        //       = num - result - 0.5
                        num -= result;
                        num = (num << (pfloat.NUM_BITS / 2)) - 0x80000000UL;
                        result = (result << (pfloat.NUM_BITS / 2)) + 0x80000000UL;
                    } else {
                        num <<= pfloat.NUM_BITS / 2;
                        result <<= pfloat.NUM_BITS / 2;
                    }

                    bit = 1UL << (pfloat.NUM_BITS / 2 - 2);
                }
            }

            // Finally, if next bit would have been 1, round the result upwards.
            if (num > result) {
                ++result;
            }

            return new pfloat((long)result);
        }

        /// <summary>
        /// Returns the Sine of x.
        /// The relative error is less than 1E-10 for x in [-2PI, 2PI], and less than 1E-7 in the worst case.
        /// </summary>
        public static pfloat Sin(pfloat x) {
            var clampedL = FPMath.ClampSinValue(x.v, out var flipHorizontal, out var flipVertical);
            var clamped = new pfloat(clampedL);

            // Find the two closest values in the LUT and perform linear interpolation
            // This is what kills the performance of this function on x86 - x64 is fine though
            var rawIndex = FPMath.FastMul(clamped, pfloat.LutInterval);
            var roundedIndex = FPMath.Round(rawIndex);
            var indexError = FPMath.FastSub(rawIndex, roundedIndex);

            var nearestValue = new pfloat(FPMathTables.SinLut[flipHorizontal ? FPMathTables.SinLut.Length - 1 - (int)roundedIndex : (int)roundedIndex]);
            var secondNearestValue =
                new pfloat(FPMathTables.SinLut[
                               flipHorizontal ? FPMathTables.SinLut.Length - 1 - (int)roundedIndex - FPMath.Sign(indexError) : (int)roundedIndex + FPMath.Sign(indexError)]);

            var delta = FPMath.FastMul(indexError, FPMath.FastAbs(FPMath.FastSub(nearestValue, secondNearestValue))).v;
            var interpolatedValue = nearestValue.v + (flipHorizontal ? -delta : delta);
            var finalValue = flipVertical ? -interpolatedValue : interpolatedValue;
            return new pfloat(finalValue);
        }

        public static pfloat Asin(pfloat value) {

            return UnityEngine.Mathf.Asin(value);

        }
        
        /// <summary>
        /// Returns a rough approximation of the Sine of x.
        /// This is at least 3 times faster than Sin() on x86 and slightly faster than Math.Sin(),
        /// however its accuracy is limited to 4-5 decimals, for small enough values of x.
        /// </summary>
        public static pfloat FastSin(pfloat x) {
            var clampedL = FPMath.ClampSinValue(x.v, out var flipHorizontal, out var flipVertical);

            // Here we use the fact that the SinLut table has a number of entries
            // equal to (PI_OVER_2 >> 15) to use the angle to index directly into it
            var rawIndex = (uint)(clampedL >> 15);
            if (rawIndex >= pfloat.LUT_SIZE) {
                rawIndex = pfloat.LUT_SIZE - 1;
            }

            var nearestValue = FPMathTables.SinLut[flipHorizontal ? FPMathTables.SinLut.Length - 1 - (int)rawIndex : (int)rawIndex];
            return new pfloat(flipVertical ? -nearestValue : nearestValue);
        }


        private static long ClampSinValue(long angle, out bool flipHorizontal, out bool flipVertical) {
            var largePI = 7244019458077122842;
            // Obtained from ((Fix64)1686629713.065252369824872831112M).m_rawValue
            // This is (2^29)*PI, where 29 is the largest N such that (2^N)*PI < MaxValue.
            // The idea is that this number contains way more precision than PI_TIMES_2,
            // and (((x % (2^29*PI)) % (2^28*PI)) % ... (2^1*PI) = x % (2 * PI)
            // In practice this gives us an error of about 1,25e-9 in the worst case scenario (Sin(MaxValue))
            // Whereas simply doing x % PI_TIMES_2 is the 2e-3 range.

            var clamped2Pi = angle;
            for (var i = 0; i < 29; ++i) {
                clamped2Pi %= largePI >> i;
            }

            if (angle < 0) {
                clamped2Pi += pfloat.PI_TIMES_2;
            }

            // The LUT contains values for 0 - PiOver2; every other value must be obtained by
            // vertical or horizontal mirroring
            flipVertical = clamped2Pi >= pfloat.PI;
            // obtain (angle % PI) from (angle % 2PI) - much faster than doing another modulo
            var clampedPi = clamped2Pi;
            while (clampedPi >= pfloat.PI) {
                clampedPi -= pfloat.PI;
            }

            flipHorizontal = clampedPi >= pfloat.PI_OVER_2;
            // obtain (angle % PI_OVER_2) from (angle % PI) - much faster than doing another modulo
            var clampedPiOver2 = clampedPi;
            if (clampedPiOver2 >= pfloat.PI_OVER_2) {
                clampedPiOver2 -= pfloat.PI_OVER_2;
            }

            return clampedPiOver2;
        }

        /// <summary>
        /// Returns the cosine of x.
        /// The relative error is less than 1E-10 for x in [-2PI, 2PI], and less than 1E-7 in the worst case.
        /// </summary>
        public static pfloat Cos(pfloat x) {
            var xl = x.v;
            var rawAngle = xl + (xl > 0 ? -pfloat.PI - pfloat.PI_OVER_2 : pfloat.PI_OVER_2);
            return FPMath.Sin(new pfloat(rawAngle));
        }

        /// <summary>
        /// Returns a rough approximation of the cosine of x.
        /// See FastSin for more details.
        /// </summary>
        public static pfloat FastCos(pfloat x) {
            var xl = x.v;
            var rawAngle = xl + (xl > 0 ? -pfloat.PI - pfloat.PI_OVER_2 : pfloat.PI_OVER_2);
            return FPMath.FastSin(new pfloat(rawAngle));
        }

        /// <summary>
        /// Returns the tangent of x.
        /// </summary>
        /// <remarks>
        /// This function is not well-tested. It may be wildly inaccurate.
        /// </remarks>
        public static pfloat Tan(pfloat x) {
            var clampedPi = x.v % pfloat.PI;
            var flip = false;
            if (clampedPi < 0) {
                clampedPi = -clampedPi;
                flip = true;
            }

            if (clampedPi > pfloat.PI_OVER_2) {
                flip = !flip;
                clampedPi = pfloat.PI_OVER_2 - (clampedPi - pfloat.PI_OVER_2);
            }

            var clamped = new pfloat(clampedPi);

            // Find the two closest values in the LUT and perform linear interpolation
            var rawIndex = FPMath.FastMul(clamped, pfloat.LutInterval);
            var roundedIndex = FPMath.Round(rawIndex);
            var indexError = FPMath.FastSub(rawIndex, roundedIndex);

            var nearestValue = new pfloat(FPMathTables.TanLut[(int)roundedIndex]);
            var secondNearestValue = new pfloat(FPMathTables.TanLut[(int)roundedIndex + FPMath.Sign(indexError)]);

            var delta = FPMath.FastMul(indexError, FPMath.FastAbs(FPMath.FastSub(nearestValue, secondNearestValue))).v;
            var interpolatedValue = nearestValue.v + delta;
            var finalValue = flip ? -interpolatedValue : interpolatedValue;
            return new pfloat(finalValue);
        }

        /// <summary>
        /// Returns the arccos of of the specified number, calculated using Atan and Sqrt
        /// This function has at least 7 decimals of accuracy.
        /// </summary>
        public static pfloat Acos(pfloat x) {
            
            if (x > pfloat.One) x -= (int)x;
            if (x < -pfloat.One) x -= (int)x;
            
            if (x < -pfloat.One || x > pfloat.One) {
                throw new System.ArgumentOutOfRangeException(nameof(x));
            }

            if (x.v == 0) {
                return pfloat.PiOver2;
            }

            var result = FPMath.Atan(FPMath.Sqrt(pfloat.One - x * x) / x);
            return x.v < 0 ? result + pfloat.Pi : result;
        }

        /// <summary>
        /// Returns the arctan of of the specified number, calculated using Euler series
        /// This function has at least 7 decimals of accuracy.
        /// </summary>
        public static pfloat Atan(pfloat z) {
            if (z.v == 0) {
                return pfloat.Zero;
            }

            // Force positive values for argument
            // Atan(-z) = -Atan(z).
            var neg = z.v < 0;
            if (neg) {
                z = -z;
            }

            pfloat result;
            var two = (pfloat)2;
            var three = (pfloat)3;

            var invert = z > pfloat.One;
            if (invert) {
                z = pfloat.One / z;
            }

            result = pfloat.One;
            var term = pfloat.One;

            var zSq = z * z;
            var zSq2 = zSq * two;
            var zSqPlusOne = zSq + pfloat.One;
            var zSq12 = zSqPlusOne * two;
            var dividend = zSq2;
            var divisor = zSqPlusOne * three;

            for (var i = 2; i < 30; ++i) {
                term *= dividend / divisor;
                result += term;

                dividend += zSq2;
                divisor += zSq12;

                if (term.v == 0) {
                    break;
                }
            }

            result = result * z / zSqPlusOne;

            if (invert) {
                result = pfloat.PiOver2 - result;
            }

            if (neg) {
                result = -result;
            }

            return result;
        }

        public static pfloat Atan2(pfloat y, pfloat x) {
            var yl = y.v;
            var xl = x.v;
            if (xl == 0) {
                if (yl > 0) {
                    return pfloat.PiOver2;
                }

                if (yl == 0) {
                    return pfloat.Zero;
                }

                return -pfloat.PiOver2;
            }

            pfloat atan;
            var z = y / x;

            // Deal with overflow
            if (pfloat.One + (pfloat)0.28M * z * z == pfloat.MaxValue) {
                return y < pfloat.Zero ? -pfloat.PiOver2 : pfloat.PiOver2;
            }

            if (FPMath.Abs(z) < pfloat.One) {
                atan = z / (pfloat.One + (pfloat)0.28M * z * z);
                if (xl < 0) {
                    if (yl < 0) {
                        return atan - pfloat.Pi;
                    }

                    return atan + pfloat.Pi;
                }
            } else {
                atan = pfloat.PiOver2 - z / (z * z + (pfloat)0.28M);
                if (yl < 0) {
                    return atan - pfloat.Pi;
                }
            }

            return atan;
        }

        /// <summary>
        /// Returns a number indicating the sign of a Fix64 number.
        /// Returns 1 if the value is positive, 0 if is 0, and -1 if it is negative.
        /// </summary>
        public static int Sign(pfloat value) {
            return
                value.v < 0 ? -1 :
                value.v > 0 ? 1 :
                0;
        }


        /// <summary>
        /// Returns the absolute value of a Fix64 number.
        /// Note: Abs(Fix64.MinValue) == Fix64.MaxValue.
        /// </summary>
        public static pfloat Abs(pfloat value) {
            if (value.v == pfloat.MIN_VALUE) {
                return pfloat.MaxValue;
            }

            // branchless implementation, see http://www.strchr.com/optimized_abs_function
            var mask = value.v >> 63;
            return new pfloat((value.v + mask) ^ mask);
        }

        /// <summary>
        /// Returns the absolute value of a Fix64 number.
        /// FastAbs(Fix64.MinValue) is undefined.
        /// </summary>
        public static pfloat FastAbs(pfloat value) {
            // branchless implementation, see http://www.strchr.com/optimized_abs_function
            var mask = value.v >> 63;
            return new pfloat((value.v + mask) ^ mask);
        }


        /// <summary>
        /// Returns the largest integer less than or equal to the specified number.
        /// </summary>
        public static pfloat Floor(pfloat value) {
            // Just zero out the fractional part
            return new pfloat((long)((ulong)value.v & 0xFFFFFFFF00000000));
        }

        /// <summary>
        /// Returns the smallest integral value that is greater than or equal to the specified number.
        /// </summary>
        public static pfloat Ceiling(pfloat value) {
            var hasFractionalPart = (value.v & 0x00000000FFFFFFFF) != 0;
            return hasFractionalPart ? FPMath.Floor(value) + pfloat.One : value;
        }

        /// <summary>
        /// Rounds a value to the nearest integral value.
        /// If the value is halfway between an even and an uneven value, returns the even value.
        /// </summary>
        public static pfloat Round(pfloat value) {
            var fractionalPart = value.v & 0x00000000FFFFFFFF;
            var integralPart = FPMath.Floor(value);
            if (fractionalPart < 0x80000000) {
                return integralPart;
            }

            if (fractionalPart > 0x80000000) {
                return integralPart + pfloat.One;
            }

            // if number is halfway between two values, round to the nearest even number
            // this is the method used by System.Math.Round().
            return (integralPart.v & pfloat.ONE) == 0
                       ? integralPart
                       : integralPart + pfloat.One;
        }

        public static pfloat Clamp01(in pfloat v) {

            return FPMath.Clamp(v, 0f, 1f);

        }

        public static pfloat Clamp(pfloat v, pfloat min, pfloat max) {

            if (v < min) v = min;
            if (v > max) v = max;
            return v;

        }
        
        public static pfloat Angle(FPVector2 from, FPVector2 to) {
        
            var num = FPMath.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
            return (float)num < 1.0000000036274937E-15 ? pfloat.Zero : FPMath.Acos(FPMath.Clamp(FPVector2.Dot(from, to) / num, -pfloat.One, pfloat.One)) * (pfloat)57.29578f;
            
        }

        public static float SignedAngle(FPVector2 from, FPVector2 to) => FPMath.Angle(from, to) * FPMath.Sign(from.x * to.y - from.y * to.x);

        public static pfloat Repeat(pfloat t, pfloat length) {

            return FPMath.Clamp(t - FPMath.Floor(t / length) * length, pfloat.Zero, length);

        }

    }
    
    public static class FPExtensions {

        public const float Deg2Rad = 0.017453292f;
        public const float Rad2Deg = 57.29578f;
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FPVector2 Abs(FPVector2 v) {

            return new FPVector2(FPMath.Abs(v.x), FPMath.Abs(v.y));

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FPVector3 XY(this FPVector2 v, float z = 0f) {

            return new FPVector3(v.x, v.y, z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FPVector3 XZ(this FPVector2 v, float y = 0f) {

            return new FPVector3(v.x, y, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FPVector2 XY(this FPVector3 v) {

            return new FPVector2(v.x, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FPVector2 XZ(this FPVector3 v) {

            return new FPVector2(v.x, v.z);

        }

        public static string ToStringDec(this pfloat value) {

            return value.ToString();

        }

        public static string ToStringDec(this FPVector2 value) {

            return value.x.ToStringDec() + "; " + value.y.ToStringDec();

        }

        public static string ToStringDec(this FPVector3 value) {

            return value.x.ToStringDec() + "; " + value.y.ToStringDec() + "; " + value.z.ToStringDec();

        }

        /*public static pfloat Angle(FPVector2 from, FPVector2 to) {
        
            var num = FPMath.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
            return (float)num < 1.0000000036274937E-15 ? pfloat.Zero : FPMath.Acos(FPMath.Clamp(FPVector2.Dot(from, to) / num, -pfloat.One, pfloat.One)) * (pfloat)57.29578f;
            
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
        
        public static pfloat Asin(pfloat value) {

            return UnityEngine.Mathf.Asin(value);

        }

        public static pfloat Acos(pfloat value) {

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

        public static int FloorToInt(pfloat v) {

            return UnityEngine.Mathf.FloorToInt(v);

        }*/

    }

}
