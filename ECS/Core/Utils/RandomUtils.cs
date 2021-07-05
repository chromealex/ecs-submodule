#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if UNITY_MATHEMATICS
using RandomState = System.UInt32;
#else
using RandomState = UnityEngine.Random.State;
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
        public static UnityEngine.Vector3 GetRandomInSphere(this ref RandomState randomState, UnityEngine.Vector3 center, float maxRadius) {
            #if UNITY_MATHEMATICS
            var rnd = new Unity.Mathematics.Random(randomState);
            var dir = ((UnityEngine.Vector3)rnd.NextFloat3(-1f, 1f)).normalized;
            randomState = rnd.state;
            var result = center + dir * rnd.NextFloat(-maxRadius, maxRadius);
            #else
            UnityEngine.Random.state = randomState;
            var result = center + UnityEngine.Random.insideUnitSphere * maxRadius;
            randomState = UnityEngine.Random.state;
            #endif
            return result;
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector2 GetRandomInCircle(this ref RandomState randomState, UnityEngine.Vector2 center, float maxRadius) {
            #if UNITY_MATHEMATICS
            var rnd = new Unity.Mathematics.Random(randomState);
            var dir = ((UnityEngine.Vector2)rnd.NextFloat2(-1f, 1f)).normalized;
            randomState = rnd.state;
            var result = center + dir * rnd.NextFloat(-maxRadius, maxRadius);
            #else
            UnityEngine.Random.state = randomState;
            var result = center + UnityEngine.Random.insideUnitCircle * maxRadius;
            randomState = UnityEngine.Random.state;
            #endif
            return result;
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int GetRandomRange(this ref RandomState randomState, int from, int to) {
            #if UNITY_MATHEMATICS
            var rnd = new Unity.Mathematics.Random(randomState);
            var result = rnd.NextInt(from, to);
            randomState = rnd.state;
            #else
            UnityEngine.Random.state = randomState;
            var result = UnityEngine.Random.Range(from, to);
            randomState = UnityEngine.Random.state;
            #endif
            return result;
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static float GetRandomRange(this ref RandomState randomState, float from, float to) {
            #if UNITY_MATHEMATICS
            var rnd = new Unity.Mathematics.Random(randomState);
            var result = rnd.NextFloat(from, to);
            randomState = rnd.state;
            #else
            UnityEngine.Random.state = randomState;
            var result = UnityEngine.Random.Range(from, to);
            randomState = UnityEngine.Random.state;
            #endif
            return result;
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static float GetRandomValue(this ref RandomState randomState) {
            #if UNITY_MATHEMATICS
            var rnd = new Unity.Mathematics.Random(randomState);
            var result = rnd.NextFloat(0f, 1f);
            randomState = rnd.state;
            #else
            UnityEngine.Random.state = randomState;
            var result = UnityEngine.Random.value;
            randomState = UnityEngine.Random.state;
            #endif
            return result;
        }

        public static void SetSeed(this ref RandomState randomState, uint seed) {
            #if UNITY_MATHEMATICS
            var rnd = new Unity.Mathematics.Random(seed);
            randomState = rnd.state;
            #else
            UnityEngine.Random.InitState((int)seed);
            randomState = UnityEngine.Random.state;
            #endif
        }

    }

}