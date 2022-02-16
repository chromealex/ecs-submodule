#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    #if MESSAGE_PACK_SUPPORT
    [MessagePack.MessagePackObjectAttribute()]
    #endif
    public readonly struct pfloat : System.IEquatable<pfloat>, System.IComparable<pfloat> {

        #if MESSAGE_PACK_SUPPORT
        [MessagePack.Key(0)]
        #endif
        public readonly long v;

        // Precision of this type is 2^-32, that is 2,3283064365386962890625E-10
        //public static readonly decimal Precision = new pfloat(1L); //0.00000000023283064365386962890625m;
        public static pfloat MaxValue => new pfloat(pfloat.MAX_VALUE);
        public static pfloat MinValue => new pfloat(pfloat.MIN_VALUE);
        public static pfloat One => new pfloat(pfloat.ONE);
        public static pfloat Zero => new pfloat();
        public static pfloat Epsilon => (double)1.0000000036274937E-15M;
        /// <summary>
        /// The value of Pi
        /// </summary>
        public static readonly pfloat Pi = new pfloat(pfloat.PI);
        public static readonly pfloat PiOver2 = new pfloat(pfloat.PI_OVER_2);
        public static readonly pfloat PiTimes2 = new pfloat(pfloat.PI_TIMES_2);
        public static readonly pfloat PiInv = (pfloat)0.3183098861837906715377675267M;
        public static readonly pfloat PiOver2Inv = (pfloat)0.6366197723675813430755350535M;
        internal static readonly pfloat Log2Max = new pfloat(pfloat.LOG2MAX);
        internal static readonly pfloat Log2Min = new pfloat(pfloat.LOG2MIN);
        internal static readonly pfloat Ln2 = new pfloat(pfloat.LN2);

        internal static readonly pfloat LutInterval = (pfloat)(pfloat.LUT_SIZE - 1) / pfloat.PiOver2;
        internal const long MAX_VALUE = long.MaxValue;
        internal const long MIN_VALUE = long.MinValue;
        internal const int NUM_BITS = 64;
        internal const int FRACTIONAL_PLACES = 32;
        internal const long ONE = 1L << pfloat.FRACTIONAL_PLACES;
        internal const long PI_TIMES_2 = 0x6487ED511;
        internal const long PI = 0x3243F6A88;
        internal const long PI_OVER_2 = 0x1921FB544;
        internal const long LN2 = 0xB17217F7;
        internal const long LOG2MAX = 0x1F00000000;
        internal const long LOG2MIN = -0x2000000000;
        internal const int LUT_SIZE = (int)(pfloat.PI_OVER_2 >> 15);

        /// <summary>
        /// Adds x and y. Performs saturating addition, i.e. in case of overflow, 
        /// rounds to MinValue or MaxValue depending on sign of operands.
        /// </summary>
        public static pfloat operator +(pfloat x, pfloat y) {
            var xl = x.v;
            var yl = y.v;
            var sum = xl + yl;
            // if signs of operands are equal and signs of sum and x are different
            if ((~(xl ^ yl) & (xl ^ sum) & pfloat.MIN_VALUE) != 0) {
                sum = xl > 0 ? pfloat.MAX_VALUE : pfloat.MIN_VALUE;
            }

            return new pfloat(sum);
        }

        /// <summary>
        /// Adds x and y witout performing overflow checking. Should be inlined by the CLR.
        /// </summary>
        public static pfloat FastAdd(pfloat x, pfloat y) {
            return new pfloat(x.v + y.v);
        }

        /// <summary>
        /// Subtracts y from x. Performs saturating substraction, i.e. in case of overflow, 
        /// rounds to MinValue or MaxValue depending on sign of operands.
        /// </summary>
        public static pfloat operator -(pfloat x, pfloat y) {
            var xl = x.v;
            var yl = y.v;
            var diff = xl - yl;
            // if signs of operands are different and signs of sum and x are different
            if (((xl ^ yl) & (xl ^ diff) & pfloat.MIN_VALUE) != 0) {
                diff = xl < 0 ? pfloat.MIN_VALUE : pfloat.MAX_VALUE;
            }

            return new pfloat(diff);
        }

        public static pfloat operator *(pfloat x, pfloat y) {

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

            var overflow = false;
            var sum = FPMath.AddOverflowHelper((long)loResult, midResult1, ref overflow);
            sum = FPMath.AddOverflowHelper(sum, midResult2, ref overflow);
            sum = FPMath.AddOverflowHelper(sum, hiResult, ref overflow);

            var opSignsEqual = ((xl ^ yl) & pfloat.MIN_VALUE) == 0;

            // if signs of operands are equal and sign of result is negative,
            // then multiplication overflowed positively
            // the reverse is also true
            if (opSignsEqual) {
                if (sum < 0 || overflow && xl > 0) {
                    return pfloat.MaxValue;
                }
            } else {
                if (sum > 0) {
                    return pfloat.MinValue;
                }
            }

            // if the top 32 bits of hihi (unused in the result) are neither all 0s or 1s,
            // then this means the result overflowed.
            var topCarry = hihi >> pfloat.FRACTIONAL_PLACES;
            if (topCarry != 0 && topCarry != -1) {
                return opSignsEqual ? pfloat.MaxValue : pfloat.MinValue;
            }

            // If signs differ, both operands' magnitudes are greater than 1,
            // and the result is greater than the negative operand, then there was negative overflow.
            if (!opSignsEqual) {
                long posOp, negOp;
                if (xl > yl) {
                    posOp = xl;
                    negOp = yl;
                } else {
                    posOp = yl;
                    negOp = xl;
                }

                if (sum > negOp && negOp < -pfloat.ONE && posOp > pfloat.ONE) {
                    return pfloat.MinValue;
                }
            }

            return new pfloat(sum);
        }

        public static pfloat operator /(pfloat x, pfloat y) {
            var xl = x.v;
            var yl = y.v;

            var remainder = (ulong)(xl >= 0 ? xl : -xl);
            var divider = (ulong)(yl >= 0 ? yl : -yl);
            var quotient = 0UL;
            var bitPos = pfloat.NUM_BITS / 2 + 1;


            // If the divider is divisible by 2^n, take advantage of it.
            while ((divider & 0xF) == 0 && bitPos >= 4) {
                divider >>= 4;
                bitPos -= 4;
            }

            while (remainder != 0 && bitPos >= 0) {
                var shift = FPMath.CountLeadingZeroes(remainder);
                if (shift > bitPos) {
                    shift = bitPos;
                }

                remainder <<= shift;
                bitPos -= shift;

                var div = remainder / divider;
                remainder = remainder % divider;
                quotient += div << bitPos;

                // Detect overflow
                if ((div & ~(0xFFFFFFFFFFFFFFFF >> bitPos)) != 0) {
                    return ((xl ^ yl) & pfloat.MIN_VALUE) == 0 ? pfloat.MaxValue : pfloat.MinValue;
                }

                remainder <<= 1;
                --bitPos;
            }

            // rounding
            ++quotient;
            var result = (long)(quotient >> 1);
            if (((xl ^ yl) & pfloat.MIN_VALUE) != 0) {
                result = -result;
            }

            return new pfloat(result);
        }

        public static pfloat operator %(pfloat x, pfloat y) {
            return new pfloat(
                (x.v == pfloat.MIN_VALUE) & (y.v == -1) ? 0 : x.v % y.v);
        }

        /// <summary>
        /// Performs modulo as fast as possible; throws if x == MinValue and y == -1.
        /// Use the operator (%) for a more reliable but slower modulo.
        /// </summary>
        public static pfloat FastMod(pfloat x, pfloat y) {
            return new pfloat(x.v % y.v);
        }

        public static pfloat operator -(pfloat x) {
            return x.v == pfloat.MIN_VALUE ? pfloat.MaxValue : new pfloat(-x.v);
        }

        public static bool operator ==(pfloat x, pfloat y) {
            return x.v == y.v;
        }

        public static bool operator !=(pfloat x, pfloat y) {
            return x.v != y.v;
        }

        public static bool operator >(pfloat x, pfloat y) {
            return x.v > y.v;
        }

        public static bool operator <(pfloat x, pfloat y) {
            return x.v < y.v;
        }

        public static bool operator >=(pfloat x, pfloat y) {
            return x.v >= y.v;
        }

        public static bool operator <=(pfloat x, pfloat y) {
            return x.v <= y.v;
        }


        public static implicit operator pfloat(long value) {
            return new pfloat(value * pfloat.ONE);
        }

        public static implicit operator long(pfloat value) {
            return value.v >> pfloat.FRACTIONAL_PLACES;
        }

        public static implicit operator pfloat(float value) {
            return new pfloat((long)(value * pfloat.ONE));
        }

        public static implicit operator float(pfloat value) {
            return (float)value.v / pfloat.ONE;
        }

        public static implicit operator pfloat(double value) {
            return new pfloat((long)(value * pfloat.ONE));
        }

        public static implicit operator double(pfloat value) {
            return (double)value.v / pfloat.ONE;
        }

        public static implicit operator pfloat(decimal value) {
            return new pfloat((long)(value * pfloat.ONE));
        }

        public static implicit operator decimal(pfloat value) {
            return (decimal)value.v / pfloat.ONE;
        }

        public override bool Equals(object obj) {
            return obj is pfloat && ((pfloat)obj).v == this.v;
        }

        public override int GetHashCode() {
            return this.v.GetHashCode();
        }

        public bool Equals(pfloat other) {
            return this.v == other.v;
        }

        public int CompareTo(pfloat other) {
            return this.v.CompareTo(other.v);
        }

        public override string ToString() {
            // Up to 10 decimal places
            return ((decimal)this).ToString();
            //return this.v.ToString();
        }

        public static pfloat FromRaw(long rawValue) {
            return new pfloat(rawValue);
        }

        internal static void GenerateSinLut() {
            using (var writer = new System.IO.StreamWriter("Fix64SinLut.cs")) {
                writer.Write(
                    @"namespace FixMath.NET 
{
    partial struct Fix64 
    {
        public static readonly long[] SinLut = new[] 
        {");
                var lineCounter = 0;
                for (var i = 0; i < pfloat.LUT_SIZE; ++i) {
                    var angle = i * System.Math.PI * 0.5 / (pfloat.LUT_SIZE - 1);
                    if (lineCounter++ % 8 == 0) {
                        writer.WriteLine();
                        writer.Write("            ");
                    }

                    var sin = System.Math.Sin(angle);
                    var rawValue = ((pfloat)sin).v;
                    writer.Write(string.Format("0x{0:X}L, ", rawValue));
                }

                writer.Write(
                    @"
        };
    }
}");
            }
        }

        internal static void GenerateTanLut() {
            using (var writer = new System.IO.StreamWriter("Fix64TanLut.cs")) {
                writer.Write(
                    @"namespace FixMath.NET 
{
    partial struct Fix64 
    {
        public static readonly long[] TanLut = new[] 
        {");
                var lineCounter = 0;
                for (var i = 0; i < pfloat.LUT_SIZE; ++i) {
                    var angle = i * System.Math.PI * 0.5 / (pfloat.LUT_SIZE - 1);
                    if (lineCounter++ % 8 == 0) {
                        writer.WriteLine();
                        writer.Write("            ");
                    }

                    var tan = System.Math.Tan(angle);
                    if (tan > (double)pfloat.MaxValue || tan < 0.0) {
                        tan = (double)pfloat.MaxValue;
                    }

                    var rawValue = ((decimal)tan > (decimal)pfloat.MaxValue || tan < 0.0 ? pfloat.MaxValue : (pfloat)tan).v;
                    writer.Write(string.Format("0x{0:X}L, ", rawValue));
                }

                writer.Write(
                    @"
        };
    }
}");
            }
        }

        // turn into a Console Application and use this to generate the look-up tables
        //static void Main(string[] args)
        //{
        //    GenerateSinLut();
        //    GenerateTanLut();
        //}

        /// <summary>
        /// The underlying integer representation
        /// </summary>
        #if MESSAGE_PACK_SUPPORT
        [MessagePack.IgnoreMemberAttribute]
        #endif
        public long RawValue {
            get {
                return this.v;
            }
        }

        /// <summary>
        /// This is the constructor from raw value; it can only be used interally.
        /// </summary>
        /// <param name="rawValue"></param>
        #if MESSAGE_PACK_SUPPORT
        [MessagePack.SerializationConstructorAttribute]
        #endif
        public pfloat(long rawValue) {
            this.v = rawValue;
        }

        public pfloat(int value) {
            this.v = value * pfloat.ONE;
        }

        public pfloat(pfloat value) {
            this.v = value.v;
        }

        public pfloat(float value) {
            this.v = ((pfloat)value).v;
        }

    }
    
}