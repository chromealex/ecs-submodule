#if UNITY_MATHEMATICS
using RandomState = System.UInt32;
#else
using UnityEngine.Analytics;
using RandomState = UnityEngine.Random.State;
#endif

namespace ME.ECS
{
    public static class RandomUtils
    {
        /// <summary> Typical usage: RandomUtils.ThreadCheck(currentWorld.worldThread); </summary>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void ThreadCheck(World world)
        {
#if WORLD_THREAD_CHECK
            if (world.worldThread != System.Threading.Thread.CurrentThread)
            {
                WrongThreadException.Throw("Random", "this could cause sync problems");
            }
#endif
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UnityEngine.Vector3 GetRandomInSphere(this ref RandomState randomState,
            UnityEngine.Vector3 center, float maxRadius)
        {
#if UNITY_MATHEMATICS
            var rnd = new Unity.Mathematics.Random(randomState);
            var dir = ((UnityEngine.Vector3) rnd.NextFloat3(-1f, 1f)).normalized;
            randomState = rnd.state;
            var result = center + dir * maxRadius;
#else
            UnityEngine.Random.state = randomState;
            var result = center + UnityEngine.Random.insideUnitSphere * maxRadius;
            randomState = UnityEngine.Random.state;
#endif
            return result;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int GetRandomRange(this ref RandomState randomState, int from, int to)
        {
#if UNITY_MATHEMATICS
            var rnd = new Unity.Mathematics.Random(randomState);
            randomState = rnd.state;
            var result = rnd.NextInt(from, to);
#else
            UnityEngine.Random.state = randomState;
            var result = UnityEngine.Random.Range(from, to);
            randomState = UnityEngine.Random.state;
#endif
            return result;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static float GetRandomRange(this ref RandomState randomState, float from, float to)
        {
#if UNITY_MATHEMATICS
            var rnd = new Unity.Mathematics.Random(randomState);
            randomState = rnd.state;
            var result = rnd.NextFloat(from, to);
#else
            UnityEngine.Random.state = randomState;
            var result = UnityEngine.Random.Range(from, to);
            randomState = UnityEngine.Random.state;
#endif
            return result;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static float GetRandomValue(this ref RandomState randomState)
        {
#if UNITY_MATHEMATICS
            var rnd = new Unity.Mathematics.Random(randomState);
            randomState = rnd.state;
            var result = rnd.NextFloat(0f, 1f);
#else
            UnityEngine.Random.state = randomState;
            var result = UnityEngine.Random.value;
            randomState = UnityEngine.Random.state;
#endif
            return result;
        }

        public static void SetSeed(this ref RandomState randomState, uint seed)
        {
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
