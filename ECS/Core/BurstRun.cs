#define BURST_BLITTABLE_CHECK

namespace ME.ECS {

    using Unity.Burst;
    using Unity.Collections.LowLevel.Unsafe;
    
    unsafe delegate void FunctionPointerDelegate(ref void* data);

    public interface IBurst {

        void Execute();

    }

    public interface IBurstExecute { }

    public interface IBurstExecute<T0, T1> : IBurstExecute where T0 : struct, IStructComponent where T1 : struct, IStructComponent {

        Unity.Burst.FunctionPointer<BurstSystem<T0, T1>.SystemFunctionPointerDelegate> GetBurstMethod();

    }

    [BurstCompile(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
    public static unsafe class BurstSystem<T0, T1> where T0 : struct, IStructComponent where T1 : struct, IStructComponent {

        [BurstCompile(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
        public struct Temp {

            public FunctionPointer<SystemFunctionPointerDelegate> systemMethod;
            public Buffers.FilterBag<T0, T1> bag;

        }
        
        public delegate void SystemFunctionPointerDelegate(in Buffers.FilterBag<T0, T1> bag, ref T0 t0, ref T1 t1);

        [BurstCompile(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
        private static void Method(ref void* temp) {

            UnsafeUtility.CopyPtrToStructure(temp, out Temp j);
            j.systemMethod.Invoke(in j.bag, ref j.bag.GetT0(), ref j.bag.GetT1());
            UnsafeUtility.CopyStructureToPtr(ref j, temp);

        }

        public static void Run(IBurstExecute<T0, T1> system, Filter filter) {

            var systemMethod = system.GetBurstMethod();
            var temp = new Temp() {
                bag = new Buffers.FilterBag<T0, T1>(filter, Unity.Collections.Allocator.Temp),
                systemMethod = systemMethod,//UnsafeUtility.AddressOf(ref systemMethod),
            };
            var objAddr = UnsafeUtility.AddressOf(ref System.Runtime.CompilerServices.Unsafe.AsRef(temp));
            var m = BurstCompiler.CompileFunctionPointer((FunctionPointerDelegate)BurstSystem<T0, T1>.Method);
            m.Invoke(ref objAddr);
            temp.bag.Push();

        }

    }

    [BurstCompile(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
    public static unsafe class Burst<T> where T : struct, IBurst {

        private static FunctionPointer<FunctionPointerDelegate> cache;
        private static FunctionPointerDelegate cacheDelegate;
        
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

            if (Burst<T>.cache.IsCreated == false) {
                
                Burst<T>.cache = BurstCompiler.CompileFunctionPointer((FunctionPointerDelegate)Burst<T>.Method);
                Burst<T>.cacheDelegate = Burst<T>.cache.Invoke;

            }
            var objAddr = UnsafeUtility.AddressOf(ref System.Runtime.CompilerServices.Unsafe.AsRef(data));
            Burst<T>.cacheDelegate.Invoke(ref objAddr);

        }

        public static void RunNoCheck(ref T data) {

            var objAddr = UnsafeUtility.AddressOf(ref System.Runtime.CompilerServices.Unsafe.AsRef(data));
            Burst<T>.cacheDelegate.Invoke(ref objAddr);

        }

        public static void Prewarm() {

            #if BURST_BLITTABLE_CHECK
            if (UnsafeUtility.IsBlittable<T>() == false) {
                
                throw new System.Exception("T must be blittable");
                
            }
            #endif

            if (Burst<T>.cache.IsCreated == false) {
                
                Burst<T>.cache = BurstCompiler.CompileFunctionPointer((FunctionPointerDelegate)Burst<T>.Method);
                Burst<T>.cacheDelegate = Burst<T>.cache.Invoke;

            }

        }

    }

}