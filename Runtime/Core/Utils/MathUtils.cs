#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class VectorExt {

        public static string ToFullString(this float2 vec) {

            return $"{vec.x};{vec.y}";

        }

        public static string ToFullString(this float3 vec) {

            return $"{vec.x};{vec.y};{vec.z}";

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static float2 Abs(float2 v) {

            return new float2(math.abs(v.x), math.abs(v.y));

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static float2 Rotate(this float2 v, tfloat degrees) {

            tfloat radians = math.radians(degrees);
            tfloat sin = math.sin(radians);
            tfloat cos = math.cos(radians);

            tfloat tx = v.x;
            tfloat ty = v.y;

            return new float2(cos * tx - sin * ty, sin * tx + cos * ty);

        }

        public static int3 Rotate(this int3 vec, int sector) {

            var v = (float2)vec.XZ();
            v = v.Rotate(sector * 90f);
            return new int3((int)v.x, vec.y, (int)v.y);
            
        }

        private static int3 Rotate90(this int3 vec) {
            
            var p = vec;
            vec.x = p.z;
            vec.z = -p.x;
            return vec;

        }

        public static int3 RotateBySector(this int3 vecUp, float3 dir) {

            var p = vecUp;
            var x = math.abs(dir.x);
            var z = math.abs(dir.z);

            if (dir.x >= 0f &&
                x >= z) {
                
                // right
                vecUp = vecUp.Rotate90();

            } else if (dir.z >= 0f &&
                       z >= x) {
                
                // up
                
            } else if (dir.z <= 0f &&
                       z >= x) {
                
                // down
                vecUp = vecUp.Rotate90();
                vecUp = vecUp.Rotate90();

            } else if (dir.x <= 0f &&
                       x >= z) {
                
                // left
                vecUp = vecUp.Rotate90();
                vecUp = vecUp.Rotate90();
                vecUp = vecUp.Rotate90();

            }
            
            return vecUp;
            
        }

    }

    public struct CirclePacker {

        public struct Circle {

            public class CirclesComparer : System.Collections.Generic.IComparer<Circle> {

                public float2 center;
                
                public int Compare(Circle x, Circle y) {
                    var d1 = this.DistanceToCenterSq(x);
                    var d2 = this.DistanceToCenterSq(y);
                    if (d1 < d2) {
                        return 1;
                    } else if (d1 > d2) {
                        return -1;
                    } else {
                        return 0;
                    }
                }

                private tfloat DistanceToCenterSq(Circle pCircle) {
                    return math.distancesq(pCircle.mCenter, this.center);
                }

            }
            
            public Entity data;
            public float2 mCenter;
            public tfloat mRadius;
 
            public Circle(Entity data, float2 iCenter, tfloat radius) {
                this.data = data;
                this.mCenter = iCenter;
                this.mRadius = radius;
            }
            
        }
        
        public ME.ECS.Collections.ListCopyable<Circle> circles;
        public float2 mPackingCenter;
        public tfloat mMinSeparation;

        public CirclePacker(float2 mPackingCenter, tfloat mMinSeparation) {
            
            this.circles = PoolListCopyable<Circle>.Spawn(10);
            this.mPackingCenter = mPackingCenter;
            this.mMinSeparation = mMinSeparation;

        }

        public void Dispose() {
            
            PoolListCopyable<Circle>.Recycle(ref this.circles);
            
        }

        public void Add(Entity data, float2 center, tfloat radius) {
            
            this.circles.Add(new Circle(data, center, radius));
            
        }

        public void Do(int iterationCounter) {

            // Sort circles based on the distance to center
            var instance = PoolClass<Circle.CirclesComparer>.Spawn();
            instance.center = this.mPackingCenter;
            System.Array.Sort(this.circles.innerArray, 0, this.circles.Count, instance);
            PoolClass<Circle.CirclesComparer>.Recycle(ref instance);

            for (int k = 0; k < iterationCounter; ++k) {

                var minSeparationSq = this.mMinSeparation * this.mMinSeparation;
                for (var i = 0; i < this.circles.Count - 1; i++) {
                    for (var j = i + 1; j < this.circles.Count; j++) {
                        if (i == j) {
                            continue;
                        }

                        float2 AB = this.circles[j].mCenter - this.circles[i].mCenter;
                        tfloat r = this.circles[i].mRadius + this.circles[j].mRadius;

                        // Length squared = (dx * dx) + (dy * dy);
                        var d = math.lengthsq(AB) - minSeparationSq;
                        tfloat minSepSq = math.min(d, minSeparationSq);
                        d -= minSepSq;

                        if (d < r * r - 0.01f) {
                            AB = math.normalize(AB);

                            AB *= (float)((r - math.sqrt(d)) * 0.5f);

                            this.circles[j].mCenter += AB;
                            this.circles[i].mCenter -= AB;

                        }
                    }
                }

                var damping = 0.1f / (float)iterationCounter;
                for (var i = 0; i < this.circles.Count; i++) {
                    var v = this.circles[i].mCenter - this.mPackingCenter;
                    v *= damping;
                    this.circles[i].mCenter -= v;
                }
            }
        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class MathUtils {

        public static int GetScheduleBatchCount(int count) {

            const int batch = 64;

            var batchCount = count / batch;
            if (batchCount == 0) batchCount = 1;
            if (count <= 10 && batchCount == 1) {

                return batchCount;

            } else if (batchCount == 1) {

                batchCount = 2;

            }
            
            return batchCount;

        }

        public static float2 GetPointOnCircle(float2 point, float2 center, tfloat radius) {
            
            var vX = point.x - center.x;
            var vY = point.y - center.y;
            var magV = math.sqrt(vX * vX + vY * vY);
            var aX = center.x + vX / magV * radius;
            var aY = center.y + vY / magV * radius;
            
            return new float2(aX, aY);
            
        }
        
        public static uint GetHash(string str) {
            
            uint hash = 0;
            foreach (char c in str) {

                hash = 31 * hash + c;

            }
            return hash;

        }

        public static string ToStringDec(this float value) {

            long lVal = System.BitConverter.DoubleToInt64Bits(value);
            return lVal.ToString("X");

        }

        #if FIXED_POINT_MATH
        public static string ToStringDec(this sfloat value) {

            uint lVal = value.RawValue;
            return lVal.ToString("X");

        }
        #endif

        public static string ToStringDec(this float2 value) {

            return value.x.ToStringDec() + "; " + value.y.ToStringDec();

        }

        public static string ToStringDec(this float3 value) {

            return value.x.ToStringDec() + "; " + value.y.ToStringDec() + "; " + value.z.ToStringDec();

        }

        public static float3 GetSpiralPointByIndex(float3 center, int index, float radius = 1f) {

            var offset = MathUtils.GetSpiralPointByIndex(UnityEngine.Vector2Int.zero, index);
            return center + new float3(offset.x * radius, 0f, offset.y * radius);

        }

        public static UnityEngine.Vector2Int GetSpiralPointByIndex(UnityEngine.Vector2Int center, int index) {

            if (index == 0) return center;

            // given n an index in the squared spiral
            // p the sum of point in inner square
            // a the position on the current square
            // n = p + a

            var pos = UnityEngine.Vector2Int.zero;
            var n = index;
            var r = (int)math.floor((math.sqrt(n + 1) - 1) / 2) + 1;

            // compute radius : inverse arithmetic sum of 8+16+24+...=
            var p = (8 * r * (r - 1)) / 2;
            // compute total point on radius -1 : arithmetic sum of 8+16+24+...

            var en = r * 2;
            // points by face

            var a = (1 + n - p) % (r * 8);
            // compute de position and shift it so the first is (-r,-r) but (-r+1,-r)
            // so square can connect

            //var pos = [0, 0, r];
            switch ((int)math.floor(a / (r * 2f))) {
                // find the face : 0 top, 1 right, 2, bottom, 3 left
                case 0: {
                    pos[0] = a - r;
                    pos[1] = -r;
                }
                    break;

                case 1: {
                    pos[0] = r;
                    pos[1] = (a % en) - r;

                }
                    break;

                case 2: {
                    pos[0] = r - (a % en);
                    pos[1] = r;
                }
                    break;

                case 3: {
                    pos[0] = -r;
                    pos[1] = r - (a % en);
                }
                    break;
            }

            return center + pos;

        }

        public static string SecondsToString(double seconds) {

            var parts = new System.Collections.Generic.List<string>();
            System.Action<int, string, string> add = (val, unit, format) => {
                if (val > 0) parts.Add(val.ToString(format) + unit);
            };
            var t = System.TimeSpan.FromSeconds(seconds);

            add(t.Days, "d", "#");
            add(t.Hours, "h", "#");
            add(t.Minutes, "m", "#");
            add(t.Seconds, "s", "#");
            add(t.Milliseconds, "ms", "000");

            return string.Join(" ", parts);

        }

        public static string BytesCountToString(int count, bool showPrevValue = false) {

            float cnt = count;
            string[] sizes = { "B", "KB", "MB", "GB", "TB", "PB" };
            int order = 0;
            float prevCount = cnt;
            while (cnt >= 1024f && order < sizes.Length - 1) {

                ++order;
                prevCount = cnt;
                cnt = cnt / 1024f;

            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            if (showPrevValue == true && order > 0) {
                
                return string.Format("{0:0.##} {1} ({2:0.##} {3})", cnt, sizes[order], prevCount, sizes[order - 1]);

            }
            
            return string.Format("{0:0.##} {1}", cnt, sizes[order]);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static uint GetKey0(ulong a) {

            return (uint)(a & uint.MaxValue);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static uint GetKey1(ulong a) {

            return (uint)(a >> 32);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static long GetKey(int a1, int a2) {

            return (((long)a2 << 32) | (uint)a1);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ulong GetKey(uint a1, uint a2) {

            return (((ulong)a2 << 32) | (uint)a1);

        }

        [INLINE(256)]
        public static int ConvertOrientationToRawIndex(int orientation) {

            switch (orientation) {

                case 0:
                    return 1;

                case 1:
                    return 2;

                case 2:
                    return 4;

                case 3:
                    return 7;

                case 4:
                    return 6;

                case 5:
                    return 5;

                case 6:
                    return 3;

                case 7:
                    return 0;

            }

            return 0;

        }

        [INLINE(256)]
        public static int ConvertRawIndexToOrientation(int orientation) {

            switch (orientation) {

                case 1:
                    return 0;

                case 2:
                    return 1;

                case 4:
                    return 2;

                case 7:
                    return 3;

                case 6:
                    return 4;

                case 5:
                    return 5;

                case 3:
                    return 6;

                case 0:
                    return 7;

            }

            return 0;

        }

        [INLINE(256)]
        public static int ConvertFileIndexToOrientation(int fileIndex) {

            switch (fileIndex) {

                case 0:
                    return 1;

                case 1:
                    return 2;

                case 2:
                    return 4;

                case 3:
                    return 7;

                case 4:
                    return 6;

                case 5:
                    return 5;

                case 6:
                    return 3;

                case 7:
                    return 0;

            }

            return 0;

        }

        [INLINE(256)]
        public static int GetOrientation(float2 dir) {

            MathUtils.GetOrientation(out var d, dir);
            return d;

        }

        [INLINE(256)]
        public static int GetOrientation(float2 from, float2 to) {

            MathUtils.GetOrientation(out var d, to - from);
            return d;

        }

        [INLINE(256)]
        public static void GetOrientation(out int orientation, float2 dir, int steps = 8) {

            float step = 360f / steps;
            float stepHalf = step * 0.5f;

            var ang = math.degrees(math.atan2(dir.y, dir.x)) + stepHalf;
            if (ang < 0f) ang = 360f + ang;
            if (ang > 360f) ang -= 360f;

            var sector = (int)(ang / step);

            switch (sector) {

                case 0:
                    orientation = 2;
                    break;

                case 1:
                    orientation = 1;
                    break;

                case 2:
                    orientation = 0;
                    break;

                case 3:
                    orientation = 7;
                    break;

                case 4:
                    orientation = 6;
                    break;

                case 5:
                    orientation = 5;
                    break;

                case 6:
                    orientation = 4;
                    break;

                case 7:
                    orientation = 3;
                    break;

                default:
                    orientation = 0;
                    break;
            }

        }

        [INLINE(256)]
        public static bool IsPositionInRange(float3 from, float3 target, float minRange, float maxRange) {

            var dir = target - from;
            var distanceSqr = math.lengthsq(dir);
            if (distanceSqr <= maxRange * maxRange &&
                distanceSqr >= minRange * minRange) {

                return true;

            }

            return false;

        }

        [INLINE(256)]
        public static float3 GetNearestPositionToTarget(float3 from, float3 target, float minRange, float maxRange) {

            var dir = target - from;
            var distanceSqr = math.lengthsq(dir);
            if (distanceSqr <= maxRange * maxRange &&
                distanceSqr >= minRange * minRange) {

                return from;

            }

            var tRange = maxRange;
            if (distanceSqr < minRange * minRange) {

                tRange = minRange;

            }

            var ray = new UnityEngine.Ray((Unity.Mathematics.float3)target, (Unity.Mathematics.float3)(from - target));
            return (float3)(Unity.Mathematics.float3)ray.GetPoint(tRange);

        }

        [INLINE(256)]
        public static bool IsTargetInCone(in float2 position, in float2 direction, in float2 target, tfloat coneAngle) {

            if (coneAngle <= 0f) return true;

            coneAngle = math.radians(coneAngle);
            
            var dir = math.normalize(target - position);
            var dot = math.clamp(math.dot(dir, math.normalize(direction)), -1f, 1f);
            var angle = math.acos(dot);
            
            return angle < coneAngle * 0.5f;

        }
        
        private static tfloat[] Factorial = new tfloat[] {
            1.0f,
            1.0f,
            2.0f,
            6.0f,
            24.0f,
            120.0f,
            720.0f,
            5040.0f,
            40320.0f,
            362880.0f,
            3628800.0f,
            39916800.0f,
            479001600.0f,
            6227020800.0f,
            87178291200.0f,
            1307674368000.0f,
            20922789888000.0f,
        };
        
        private static tfloat Binomial(int n, int i) {
            
            tfloat a1 = MathUtils.Factorial[n];
            tfloat a2 = MathUtils.Factorial[i];
            tfloat a3 = MathUtils.Factorial[n - i];
            var ni = a1 / (a2 * a3);
            return ni;
            
        }
        
        public static tfloat Bernstein(int n, int i, tfloat t) {
            
            tfloat ti = math.pow(t, i);
            tfloat tnMinusI = math.pow((1 - t), (n - i));

            tfloat basis = MathUtils.Binomial(n, i) * ti * tnMinusI;
            return basis;
            
        }
        
        public static float3 CalculateBezier(float3 p0, float3 p1, float3 p2, float3 p3, tfloat t) {
        
            return (math.pow(1 - t, 3) * p0) + (3 * math.pow(1 - t, 2) * t * p1) + (3 * (1 - t) * t * t * p2) + (t * t * t * p3);
            
        }
        
        public static float3 CalculateBezierPoint(float3 p1, float3 p2, float3 p3, float3 p4, tfloat t) {
            
            tfloat tPower3 = t * t * t;
            tfloat tPower2 = t * t;
            tfloat oneMinusT = 1 - t;
            tfloat oneMinusTPower3 = oneMinusT * oneMinusT*oneMinusT;
            tfloat oneMinusTPower2 = oneMinusT * oneMinusT;
            var p = float2.zero;
            p.x = oneMinusTPower3 * p1.x + (3f * oneMinusTPower2 * t * p2.x) + (3f * oneMinusT * tPower2 * p3.x) + tPower3 * p4.x;
            p.y = oneMinusTPower3 * p1.y + (3f * oneMinusTPower2 * t * p2.y) + (3f * oneMinusT * tPower2 * p3.y) + tPower3 * p4.y; 
            return p.XY();
            
        }

    }

}