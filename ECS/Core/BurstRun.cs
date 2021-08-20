#if UNITY_EDITOR
#define BURST_BLITTABLE_CHECK
#endif

namespace ME.ECS {

    using Unity.Burst;
    using Unity.Collections.LowLevel.Unsafe;
    
    unsafe delegate void FunctionPointerDelegate(ref void* data);

    public interface IBurst {

        void Execute();

    }

    [BurstCompile(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
    public static unsafe class Burst<T> where T : struct, IBurst {

        private static FunctionPointer<FunctionPointerDelegate> cache;
        private static FunctionPointerDelegate cacheDelegate;
        private static bool isCreated;
        
        [BurstCompile(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
        private static void Method(ref void* data) {

            UnsafeUtility.CopyPtrToStructure(data, out T j);
            j.Execute();
            UnsafeUtility.CopyStructureToPtr(ref j, data);

        }

        public static void Run(ref T data) {

            #if BURST_BLITTABLE_CHECK
            if (UnsafeUtility.IsBlittable<T>() == false) {
                
                throw new System.Exception("T must be blittable");
                
            }
            #endif

            if (Burst<T>.isCreated == false) {
                
                Burst<T>.cache = BurstCompiler.CompileFunctionPointer((FunctionPointerDelegate)Burst<T>.Method);
                Burst<T>.isCreated = true;
                Burst<T>.cacheDelegate = Burst<T>.cache.Invoke;

            }
            var objAddr = UnsafeUtility.AddressOf(ref data);
            Burst<T>.cacheDelegate.Invoke(ref objAddr);

        }

        public static void RunNoCheck(ref T data) {

            var objAddr = UnsafeUtility.AddressOf(ref data);
            Burst<T>.cacheDelegate.Invoke(ref objAddr);

        }

        public static void Prewarm() {

            #if BURST_BLITTABLE_CHECK
            if (UnsafeUtility.IsBlittable<T>() == false) {
                
                throw new System.Exception("T must be blittable");
                
            }
            #endif

            if (Burst<T>.isCreated == false) {
                
                Burst<T>.cache = BurstCompiler.CompileFunctionPointer((FunctionPointerDelegate)Burst<T>.Method);
                Burst<T>.isCreated = true;
                Burst<T>.cacheDelegate = Burst<T>.cache.Invoke;

            }

        }

    }

}