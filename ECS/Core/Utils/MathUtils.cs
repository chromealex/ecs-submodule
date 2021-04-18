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

        public static string BytesCountToString(int count) {

            string[] sizes = { "B", "KB", "MB", "GB", "TB", "PB" };
            int order = 0;
            while (count >= 1024 && order < sizes.Length - 1) {

                ++order;
                count = count / 1024;

            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return string.Format("{0:0.##} {1}", count, sizes[order]);

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

        public static void GetOrientation(out int orientation, Vector2 dir) {

            const float step = 360f / 8f;
            const float stepHalf = step * 0.5f;

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
            var dot = Vector2.Dot(dir, direction.normalized);
            var angle = Mathf.Acos(dot);
            
            return angle < coneAngle * 0.5f;

        }

    }

}