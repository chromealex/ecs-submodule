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
            
            if (WorldUtilities.IsComponentAsTag<T>() == false) reg.components[entity.id] = default;
            ref var state = ref reg.componentsStates.arr[entity.id];
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
            
            if (WorldUtilities.IsComponentAsTag<T>() == false) reg.components[entity.id] = data;
            ref var state = ref reg.componentsStates.arr[entity.id];
            if (state == 0) {

                state = 1;
                if (ComponentTypes<T>.typeId >= 0) {

                    world.currentState.storage.archetypes.Set<T>(in entity);
                    world.UpdateFilterByStructComponent<T>(in entity);

                }

            }

            world.currentState.storage.versions.Increment(in entity);
            if (AllComponentTypes<T>.isVersioned == true) reg.versions.arr[entity.id] = world.GetCurrentTick();
            if (AllComponentTypes<T>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
            if (ComponentTypes<T>.isFilterVersioned == true) world.UpdateFilterByStructComponentVersioned<T>(in entity);
            
        }

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct DataBuffer<T> where T : struct, IStructComponentBase {

        [Unity.Collections.NativeDisableParallelForRestriction] private Unity.Collections.NativeArray<T> arr;
        [Unity.Collections.NativeDisableParallelForRestriction] private Unity.Collections.NativeArray<byte> contains;
        [Unity.Collections.NativeDisableParallelForRestriction] private Unity.Collections.NativeArray<byte> ops;
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public DataBuffer(World world, ME.ECS.Collections.NativeBufferArray<Entity> arr, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) {

            var reg = (StructComponents<T>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T>()];
            reg.Merge();
            this.arr = new Unity.Collections.NativeArray<T>(reg.componentsStates.Length, allocator);
            if (reg.components.Length > 0) Unity.Collections.NativeArray<T>.Copy(reg.components.data.arr, this.arr, reg.componentsStates.Length);
            this.contains = new Unity.Collections.NativeArray<byte>(reg.componentsStates.arr, allocator);
            this.ops = new Unity.Collections.NativeArray<byte>(reg.componentsStates.arr.Length, allocator);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int Push(World world, ME.ECS.Collections.NativeBufferArray<Entity> arr, int max, Unity.Collections.NativeArray<int> filterEntities) {

            //var changedCount = 0;
            var isTag = WorldUtilities.IsComponentAsTag<T>();
            var reg = (StructComponents<T>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T>()];
            for (int i = 0; i < filterEntities.Length; ++i) {

                var entityId = filterEntities[i];
                var op = this.ops[entityId];
                if (op == 0) continue;

                var entity = arr.arr[entityId];
                if ((op & 0x4) != 0) {

                    // Remove
                    
                    if (isTag == false) reg.components[entity.id] = default;
                    ref var state = ref reg.componentsStates.arr[entity.id];
                    if (state > 0) {

                        state = 0;
                        if (ComponentTypes<T>.typeId >= 0) {

                            world.currentState.storage.archetypes.Remove<T>(in entity);
                            world.UpdateFilterByStructComponent<T>(in entity);

                        }

                    }
                    
                    world.currentState.storage.versions.Increment(in entity);
                    if (ComponentTypes<T>.isFilterVersioned == true) world.UpdateFilterByStructComponentVersioned<T>(in entity);
                    //++changedCount;
                    
                } else if ((op & 0x2) != 0) {

                    // Set

                    if (isTag == false) reg.components[entity.id] = this.arr[entity.id];
                    ref var state = ref reg.componentsStates.arr[entity.id];
                    if (state == 0) {

                        state = 1;
                        if (ComponentTypes<T>.typeId >= 0) {

                            world.currentState.storage.archetypes.Set<T>(in entity);
                            world.UpdateFilterByStructComponent<T>(in entity);

                        }

                    }

                    world.currentState.storage.versions.Increment(in entity);
                    if (AllComponentTypes<T>.isVersioned == true) reg.versions.arr[entity.id] = world.GetCurrentTick();
                    if (AllComponentTypes<T>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
                    if (ComponentTypes<T>.isFilterVersioned == true) world.UpdateFilterByStructComponentVersioned<T>(in entity);
                    //++changedCount;

                }
                
            }
            
            this.Dispose();

            return 0;

        }

        public void Dispose() {
            
            this.arr.Dispose();
            this.contains.Dispose();
            this.ops.Dispose();
            
        }

        public void Remove(int entityId) {

            this.ops[entityId] |= 0x4;
            this.contains[entityId] = 0;

        }

        public void Set(int entityId, in T data) {

            this.ops[entityId] |= 0x2;
            this.arr[entityId] = data;
            this.contains[entityId] = 1;
            
        }

        public ref T Get(int entityId) {

            this.ops[entityId] |= 0x2;
            return ref this.arr.GetRef(entityId);

        }

        public ref readonly T Read(int entityId) {

            return ref this.arr.GetRef(entityId);

        }

        public bool Has(int entityId) {

            return this.contains.GetRefRead(entityId) > 0;

        }

    }

}