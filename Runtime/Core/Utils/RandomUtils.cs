#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using RandomState = System.UInt32;

#if FIXED_POINT_MATH
using FLOAT2 = ME.ECS.fp2;
using FLOAT3 = ME.ECS.fp3;
using FLOAT4 = ME.ECS.fp4;
using QUATERNION = ME.ECS.fpquaternion;
#else
using FLOAT2 = UnityEngine.Vector2;
using FLOAT3 = UnityEngine.Vector3;
using FLOAT4 = UnityEngine.Vector4;
using QUATERNION = UnityEngine.Quaternion;
#endif

namespace ME.ECS {

    public static class RandomUtils {

        /// <summary> Typical usage: RandomUtils.ThreadCheck(currentWorld.worldThread); </summary>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        [System.Diagnostics.Conditional("WORLD_THREAD_CHECK")]
        public static void ThreadCheck(World world) {
            #if WORLD_THREAD_CHECK
            if (world.worldThread != System.Threading.Thread.CurrentThread) {
                WrongThreadException.Throw("Random", "this could cause sync problems");
            }
            #endif
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FLOAT3 GetRandomInSphere(this ref RandomState randomState, FLOAT3 center, float maxRadius) {
            var rnd = new Unity.Mathematics.Random(randomState);
            var dir = ((UnityEngine.Vector3)rnd.NextFloat3(-1f, 1f)).normalized;
            randomState = rnd.state;
            return center + dir * rnd.NextFloat(-maxRadius, maxRadius);
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FLOAT2 GetRandomInCircle(this ref RandomState randomState, FLOAT2 center, float maxRadius) {
            var rnd = new Unity.Mathematics.Random(randomState);
            var dir = ((UnityEngine.Vector2)rnd.NextFloat2(-1f, 1f)).normalized;
            randomState = rnd.state;
            return center + dir * rnd.NextFloat(-maxRadius, maxRadius);
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int GetRandomRange(this ref RandomState randomState, int from, int to) {
            var rnd = new Unity.Mathematics.Random(randomState);
            var result = rnd.NextInt(from, to);
            randomState = rnd.state;
            return result;
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static float GetRandomRange(this ref RandomState randomState, float from, float to) {
            var rnd = new Unity.Mathematics.Random(randomState);
            var result = rnd.NextFloat(from, to);
            randomState = rnd.state;
            return result;
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static float GetRandomValue(this ref RandomState randomState) {
            var rnd = new Unity.Mathematics.Random(randomState);
            var result = rnd.NextFloat(0f, 1f);
            randomState = rnd.state;
            return result;
        }

        public static void SetSeed(this ref RandomState randomState, uint seed) {
            var rnd = new Unity.Mathematics.Random(seed);
            randomState = rnd.state;
        }

    }

}