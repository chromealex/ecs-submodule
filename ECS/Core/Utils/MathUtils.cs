#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using UnityEngine;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class VectorExt {

        public static string ToFullString(this Vector2 vec) {

            return $"{vec.x};{vec.y}";

        }

        public static string ToFullString(this Vector3 vec) {

            return $"{vec.x};{vec.y};{vec.z}";

        }

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
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Vector2 Abs(Vector2 v) {

            return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Vector3 XY(this Vector2 v, float z = 0f) {

            return new Vector3(v.x, v.y, z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Vector3 XZ(this Vector2 v, float y = 0f) {

            return new Vector3(v.x, y, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Vector2 XY(this Vector3 v) {

            return new Vector2(v.x, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Vector2 XZ(this Vector3 v) {

            return new Vector2(v.x, v.z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Vector3Int XY(this Vector2Int v, int z = 0) {

            return new Vector3Int(v.x, v.y, z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Vector3Int XZ(this Vector2Int v, int y = 0) {

            return new Vector3Int(v.x, y, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Vector2Int XY(this Vector3Int v) {

            return new Vector2Int(v.x, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Vector2Int XZ(this Vector3Int v) {

            return new Vector2Int(v.x, v.z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Vector2 Rotate(this Vector2 v, float degrees) {

            float radians = degrees * Mathf.Deg2Rad;
            float sin = Mathf.Sin(radians);
            float cos = Mathf.Cos(radians);

            float tx = v.x;
            float ty = v.y;

            return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);

        }

        public static Vector3Int Rotate(this Vector3Int vec, int sector) {

            var v = (Vector2)vec.XZ();
            v = v.Rotate(sector * 90f);
            return new Vector3Int((int)v.x, vec.y, (int)v.y);
            
        }

        private static Vector3Int Rotate90(this Vector3Int vec) {
            
            var p = vec;
            vec.x = p.z;
            vec.z = -p.x;
            return vec;

        }

        public static Vector3Int RotateBySector(this Vector3Int vecUp, Vector3 dir) {

            var p = vecUp;
            var x = Mathf.Abs(dir.x);
            var z = Mathf.Abs(dir.z);

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

                public Vector2 center;
                
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

                private float DistanceToCenterSq(Circle pCircle) {
                    return (pCircle.mCenter - this.center).sqrMagnitude;
                }

            }
            
            public Entity data;
            public Vector2 mCenter;
            public float mRadius;
 
            public Circle(Entity data, Vector2 iCenter, float radius) {
                this.data = data;
                this.mCenter = iCenter;
                this.mRadius = radius;
            }
            
        }
        
        public ME.ECS.Collections.ListCopyable<Circle> circles;
        public Vector2 mPackingCenter;
        public float mMinSeparation;

        public CirclePacker(Vector2 mPackingCenter, float mMinSeparation) {
            
            this.circles = PoolListCopyable<Circle>.Spawn(10);
            this.mPackingCenter = mPackingCenter;
            this.mMinSeparation = mMinSeparation;

        }

        public void Dispose() {
            
            PoolListCopyable<Circle>.Recycle(ref this.circles);
            
        }

        public void Add(Entity data, Vector2 center, float radius) {
            
            this.circles.Add(new Circle(data, center, radius));
            
        }

        public void Do(int iterationCounter) {

            // Sort circles based on the distance to center
            var instance = PoolClass<Circle.CirclesComparer>.Spawn();
            instance.center = this.mPackingCenter;
            System.Array.Sort(this.circles.innerArray.arr, 0, this.circles.Count, instance);
            PoolClass<Circle.CirclesComparer>.Recycle(ref instance);

            for (int k = 0; k < iterationCounter; ++k) {

                var minSeparationSq = this.mMinSeparation * this.mMinSeparation;
                for (var i = 0; i < this.circles.Count - 1; i++) {
                    for (var j = i + 1; j < this.circles.Count; j++) {
                        if (i == j) {
                            continue;
                        }

                        Vector2 AB = this.circles[j].mCenter - this.circles[i].mCenter;
                        float r = this.circles[i].mRadius + this.circles[j].mRadius;

                        // Length squared = (dx * dx) + (dy * dy);
                        var d = AB.sqrMagnitude - minSeparationSq;
                        float minSepSq = System.Math.Min(d, minSeparationSq);
                        d -= minSepSq;

                        if (d < r * r - 0.01f) {
                            AB.Normalize();

                            AB *= (float)((r - System.Math.Sqrt(d)) * 0.5f);

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

        public static string ToStringDec(this Vector2 value) {

            return value.x.ToStringDec() + "; " + value.y.ToStringDec();

        }

        public static string ToStringDec(this Vector3 value) {

            return value.x.ToStringDec() + "; " + value.y.ToStringDec() + "; " + value.z.ToStringDec();

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

        public static bool IsUnityMathematicsUsed() {

            #if UNITY_MATHEMATICS
            return true;
            #else
            return false;
            #endif

        }

        public static Vector3 GetSpiralPointByIndex(Vector3 center, int index, float radius = 1f) {

            var offset = MathUtils.GetSpiralPointByIndex(Vector2Int.zero, index);
            return center + new Vector3(offset.x * radius, 0f, offset.y * radius);

        }

        public static Vector2Int GetSpiralPointByIndex(Vector2Int center, int index) {

            if (index == 0) return center;

            // given n an index in the squared spiral
            // p the sum of point in inner square
            // a the position on the current square
            // n = p + a

            var pos = Vector2Int.zero;
            var n = index;
            var r = Mathf.FloorToInt((Mathf.Sqrt(n + 1) - 1) / 2) + 1;

            // compute radius : inverse arithmetic sum of 8+16+24+...=
            var p = (8 * r * (r - 1)) / 2;
            // compute total point on radius -1 : arithmetic sum of 8+16+24+...

            var en = r * 2;
            // points by face

            var a = (1 + n - p) % (r * 8);
            // compute de position and shift it so the first is (-r,-r) but (-r+1,-r)
            // so square can connect

            //var pos = [0, 0, r];
            switch (Mathf.FloorToInt(a / (r * 2f))) {
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

        public static int GetOrientation(Vector2 dir) {

            MathUtils.GetOrientation(out var d, dir);
            return d;

        }

        public static int GetOrientation(Vector2 from, Vector2 to) {

            MathUtils.GetOrientation(out var d, to - from);
            return d;

        }

        public static void GetOrientation(out int orientation, Vector2 dir, int steps = 8) {

            float step = 360f / steps;
            float stepHalf = step * 0.5f;

            var ang = System.Math.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + stepHalf;
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

        public static bool IsPositionInRange(Vector3 from, Vector3 target, float minRange, float maxRange) {

            var dir = target - from;
            var distanceSqr = dir.sqrMagnitude;
            if (distanceSqr <= maxRange * maxRange &&
                distanceSqr >= minRange * minRange) {

                return true;

            }

            return false;

        }

        public static Vector3 GetNearestPositionToTarget(Vector3 from, Vector3 target, float minRange, float maxRange) {

            var dir = target - from;
            var distanceSqr = dir.sqrMagnitude;
            if (distanceSqr <= maxRange * maxRange &&
                distanceSqr >= minRange * minRange) {

                return from;

            }

            var tRange = maxRange;
            if (distanceSqr < minRange * minRange) {

                tRange = minRange;

            }

            var ray = new Ray(target, from - target);
            return ray.GetPoint(tRange);

        }

        public static bool IsTargetInCone(in Vector2 position, in Vector2 direction, in Vector2 target, float coneAngle) {

            if (coneAngle <= 0f) return true;

            coneAngle *= Mathf.Deg2Rad;
            
            var dir = (target - position).normalized;
            var dot = Mathf.Clamp(Vector2.Dot(dir, direction.normalized), -1f, 1f);
            var angle = Mathf.Acos(dot);
            
            return angle < coneAngle * 0.5f;

        }
        
        private static float[] Factorial = new float[] {
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
        
        private static float Binomial(int n, int i) {
            
            float a1 = MathUtils.Factorial[n];
            float a2 = MathUtils.Factorial[i];
            float a3 = MathUtils.Factorial[n - i];
            var ni = a1 / (a2 * a3);
            return ni;
            
        }
        
        public static float Bernstein(int n, int i, float t) {
            
            float ti = Mathf.Pow(t, i);
            float tnMinusI = Mathf.Pow((1 - t), (n - i));

            float basis = MathUtils.Binomial(n, i) * ti * tnMinusI;
            return basis;
            
        }
        
        public static Vector3 CalculateBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
        
            return (Mathf.Pow(1 - t, 3) * p0) + (3 * Mathf.Pow(1 - t, 2) * t * p1) + (3 * (1 - t) * t * t * p2) + (t * t * t * p3);
            
        }

    }

}