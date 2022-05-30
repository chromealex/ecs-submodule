#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using RandomState = System.UInt32;

#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
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
        public static float3 GetRandomInSphere(this ref RandomState randomState, float3 center, tfloat maxRadius) {
            var rnd = new Random(randomState);
            var dir = math.normalize(rnd.NextFloat3(new float3(-1f, -1f, -1f), new float3(1f, 1f, 1f)));
            randomState = rnd.state;
            return center + dir * rnd.NextFloat(-maxRadius, maxRadius);
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static float2 GetRandomInCircle(this ref RandomState randomState, float2 center, tfloat maxRadius) {
            var rnd = new Random(randomState);
            var dir = math.normalize(rnd.NextFloat2(new float2(-1f, -1f), new float2(1f, 1f)));
            randomState = rnd.state;
            return center + dir * rnd.NextFloat(-maxRadius, maxRadius);
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int GetRandomRange(this ref RandomState randomState, int from, int to) {
            var rnd = new Random(randomState);
            var result = rnd.NextInt(from, to);
            randomState = rnd.state;
            return result;
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static tfloat GetRandomRange(this ref RandomState randomState, tfloat from, tfloat to) {
            var rnd = new Random(randomState);
            var result = rnd.NextFloat(from, to);
            randomState = rnd.state;
            return result;
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static tfloat GetRandomValue(this ref RandomState randomState) {
            var rnd = new Random(randomState);
            var result = rnd.NextFloat(0f, 1f);
            randomState = rnd.state;
            return result;
        }

        public static void SetSeed(this ref RandomState randomState, uint seed) {
            var rnd = new Random(seed);
            randomState = rnd.state;
        }

    }

}