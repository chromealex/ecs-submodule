//#define BURST_BLITTABLE_CHECK

namespace ME.ECSBurst {
    
    using Unity.Burst;
    using Unity.Collections.LowLevel.Unsafe;
    using System.Runtime.CompilerServices;

    public unsafe delegate void FunctionPointerDelegate(ref void* data);

    [BurstCompile(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
    public static unsafe class Burst<T> where T : struct, IAdvanceTick {

        internal static FunctionPointer<FunctionPointerDelegate> cache;
        internal static FunctionPointerDelegate cacheDelegate;
        internal static bool isCreated;
        
        [BurstCompile(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
        [AOT.MonoPInvokeCallbackAttribute(typeof(FunctionPointerDelegate))]
        private static void Method(ref void* data) {

            UnsafeUtility.CopyPtrToStructure(data, out T j);
            UnityEngine.Debug.Log("Exec");
            //j.AdvanceTick();
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

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
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