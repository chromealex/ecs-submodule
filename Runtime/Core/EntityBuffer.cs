#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using ME.ECS.Collections;
using Unity.Jobs;

namespace ME.ECS {

    using Unity.Burst;
    using Unity.Collections.LowLevel.Unsafe;
    
    public static class ForEachUtils {

        internal unsafe delegate void InternalDelegate(void* fn, void* bagPtr);
        internal unsafe delegate void InternalParallelForDelegate(void* fn, void* bagPtr, int index);

        public struct ForEachTask<T> {
            
            internal delegate void ForEachTaskDelegate(in ForEachTask<T> task, in Filter filter, T callback);

            internal bool withBurst;
            internal bool parallelFor;
            internal int batchCount;
            private ForEachTaskDelegate task;
            private Filter filter;
            private T callback;

            internal ForEachTask(in Filter filter, T callback, ForEachTaskDelegate task) {

                this.task = task;
                this.withBurst = false;
                this.filter = filter;
                this.callback = callback;

                this.parallelFor = false;
                this.batchCount = 64;

            }

            public ForEachTask<T> WithBurst() {

                this.withBurst = true;
                return this;

            }

            /*public ForEachTask<T> ParallelFor(int batchCount = 64) {

                this.parallelFor = true;
                this.batchCount = batchCount;
                return this;

            }*/

            public void Do() {
                
                this.task.Invoke(in this, in this.filter, this.callback);
                
            }

        }

    }
    
    public static class DataBufferUtils {

        /*internal static class Jobs<T0> where T0 : struct, IStructComponentBase {
            
            [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
            public unsafe struct Job : IJob {

                [NativeDisableUnsafePtrRestriction]
                public System.IntPtr fn;
                public FunctionPointer<ForEachUtils.InternalDelegate> func;
                public ME.ECS.Buffers.FilterBag<T0> bag;

                public void Execute() {
                    var ptr = UnsafeUtility.AddressOf(ref this.bag);
                    this.func.Invoke((void*)this.fn, ptr);
                    UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
                }
            
                [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
                public static void Run(void* fn, void* bagPtr) {
                    UnsafeUtility.CopyPtrToStructure(bagPtr, out ME.ECS.Buffers.FilterBag<T0> bag);
                    var del = new FunctionPointer<ME.ECS.Filters.R<T0>>((System.IntPtr)fn);
                    for (int i = 0; i < bag.Length; ++i) {
                        del.Invoke(in bag.GetEntity(i), ref bag.GetT0(i));
                    }
                    UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
                }

            }

            [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
            public unsafe struct JobParallelFor : IJobParallelFor {

                [NativeDisableUnsafePtrRestriction]
                public System.IntPtr fn;
                public FunctionPointer<ForEachUtils.InternalParallelForDelegate> func;
                public ME.ECS.Buffers.FilterBag<T0> bag;

                public void Execute(int index) {
                    var ptr = UnsafeUtility.AddressOf(ref this.bag);
                    this.func.Invoke((void*)this.fn, ptr, index);
                    UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
                }
            
                [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
                public static void Run(void* fn, void* bagPtr, int index) {
                    UnsafeUtility.CopyPtrToStructure(bagPtr, out ME.ECS.Buffers.FilterBag<T0> bag);
                    var del = new FunctionPointer<ME.ECS.Filters.R<T0>>((System.IntPtr)fn);
                    del.Invoke(in bag.GetEntity(index), ref bag.GetT0(index));
                    UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
                }

            }

            public unsafe struct JobParallelForNoBurst : IJobParallelFor {

                [NativeDisableUnsafePtrRestriction]
                public System.IntPtr fn;
                public FunctionPointer<ForEachUtils.InternalParallelForDelegate> func;
                public ME.ECS.Buffers.FilterBag<T0> bag;

                public void Execute(int index) {
                    var ptr = UnsafeUtility.AddressOf(ref this.bag);
                    this.func.Invoke((void*)this.fn, ptr, index);
                    UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
                }
            
                public static void Run(void* fn, void* bagPtr, int index) {
                    UnsafeUtility.CopyPtrToStructure(bagPtr, out ME.ECS.Buffers.FilterBag<T0> bag);
                    var del = new FunctionPointer<ME.ECS.Filters.R<T0>>((System.IntPtr)fn);
                    del.Invoke(in bag.GetEntity(index), ref bag.GetT0(index));
                    UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
                }

            }

        }

        public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.R<T0>> ForEach<T0>(this in Filter filter, ME.ECS.Filters.R<T0> onEach)  where T0:struct,IStructComponentBase {
            
            return new ForEachUtils.ForEachTask<ME.ECS.Filters.R<T0>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.R<T0>> task, in Filter filterInternal, ME.ECS.Filters.R<T0> onEachInternal) => {

                if (task.withBurst == true) {

                    if (task.parallelFor == true) {
                        
                        var bag = new ME.ECS.Buffers.FilterBag<T0>(filterInternal, Unity.Collections.Allocator.TempJob);
                        var handle = System.Runtime.InteropServices.GCHandle.Alloc(onEachInternal);
                        var job = new Jobs<T0>.JobParallelFor() {
                            fn = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(onEachInternal),
                            func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalParallelForDelegate>(Jobs<T0>.JobParallelFor.Run),
                            bag = bag,
                        };
                        job.Schedule(bag.Length, task.batchCount).Complete();
                        handle.Free();
                        bag.Push();

                    } else {

                        var bag = new ME.ECS.Buffers.FilterBag<T0>(filterInternal, Unity.Collections.Allocator.TempJob);
                        var handle = System.Runtime.InteropServices.GCHandle.Alloc(onEachInternal);
                        var job = new Jobs<T0>.Job() {
                            fn = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(onEachInternal),
                            func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(Jobs<T0>.Job.Run),
                            bag = bag,
                        };
                        job.Schedule().Complete();
                        handle.Free();
                        bag.Push();

                    }

                } else {

                    if (task.parallelFor == true) {
                        
                        var bag = new ME.ECS.Buffers.FilterBag<T0>(filterInternal, Unity.Collections.Allocator.TempJob);
                        var handle = System.Runtime.InteropServices.GCHandle.Alloc(onEachInternal);
                        var job = new Jobs<T0>.JobParallelForNoBurst() {
                            fn = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(onEachInternal),
                            func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalParallelForDelegate>(Jobs<T0>.JobParallelForNoBurst.Run),
                            bag = bag,
                        };
                        job.Schedule(bag.Length, task.batchCount).Complete();
                        handle.Free();
                        bag.Push();

                    } else {

                        var bag = new ME.ECS.Buffers.FilterBag<T0>(filterInternal, Unity.Collections.Allocator.Persistent);
                        for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), ref bag.GetT0(i));
                        bag.Push();

                    }

                }

            });
            
        }*/
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void PushRemove_INTERNAL<T>(World world, in Entity entity, StructComponents<T> reg) where T : struct, IStructComponentBase {
            
            ref var bucket = ref reg.components[entity.id];
            reg.RemoveData(ref bucket);
            ref var state = ref bucket.state;
            if (state > 0) {

                state = 0;
                if (ComponentTypes<T>.typeId >= 0) {

                    world.currentState.storage.archetypes.Remove<T>(in entity);
                    world.UpdateFilterByStructComponent<T>(in entity);

                }

            }
                    
            world.currentState.storage.versions.Increment(in entity);
            if (ComponentTypes<T>.isFilterVersioned == true) world.UpdateFilterByStructComponentVersioned<T>(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void PushSet_INTERNAL<T>(World world, in Entity entity, StructComponents<T> reg, in T data) where T : struct, IStructComponentBase {
            
            ref var bucket = ref reg.components[entity.id];
            reg.Replace(ref bucket, in data);
            ref var state = ref bucket.state;
            if (state == 0) {

                state = 1;
                if (ComponentTypes<T>.typeId >= 0) {

                    world.currentState.storage.archetypes.Set<T>(in entity);
                    world.UpdateFilterByStructComponent<T>(in entity);

                }

            }

            world.currentState.storage.versions.Increment(in entity);
            reg.UpdateVersion(in entity);
            if (AllComponentTypes<T>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
            if (ComponentTypes<T>.isFilterVersioned == true) world.UpdateFilterByStructComponentVersioned<T>(in entity);
            
        }

    }
    
}