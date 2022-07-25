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

            public ForEachTask<T> ParallelFor(int batchCount = 64) {

                this.parallelFor = true;
                this.batchCount = batchCount;
                return this;

            }

            public void Do() {
                
                this.task.Invoke(in this, in this.filter, this.callback);
                
            }

        }

    }
    
    public static class DataBufferUtils {

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushRemove_INTERNAL<T>(World world, in Entity entity, StructComponents<T> reg, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            ref var bucket = ref reg.components[entity.id];
            reg.RemoveData(in entity, ref bucket);
            ref var state = ref bucket.state;
            return DataBufferUtilsBase.PushRemoveCreate_INTERNAL(ref state, world, in entity, reg, storageType);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ref T PushGet_INTERNAL<T>(World world, in Entity entity, StructComponents<T> reg, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            ref var bucket = ref reg.components[entity.id];
            ref var state = ref bucket.state;
            DataBufferUtilsBase.PushSetCreate_INTERNAL<T>(ref state, world, reg, in entity, storageType, makeRequest: true);
            return ref bucket.data;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushSet_INTERNAL<T>(World world, in Entity entity, StructComponents<T> reg, in T data, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            ref var bucket = ref reg.components[entity.id];
            reg.Replace(ref bucket, in data);
            ref var state = ref bucket.state;
            return DataBufferUtilsBase.PushSetCreate_INTERNAL<T>(ref state, world, reg, in entity, storageType, makeRequest: false);

        }

    }
    
    public static class DataBlittableBurstBufferUtils {

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool NeedToPush<T>(in ME.ECS.Collections.V3.MemoryAllocator allocator, Tick tick, ref EntityVersions entityVersions, int entityId, ref Component<T> bucket, in T data) where T : unmanaged, IComponentBase {

            if (bucket.state == 0 ||
                (
                    #if !FILTERS_LAMBDA_DISABLED
                    ComponentTypes<T>.burstIsFilterLambda.Data == 1 &&
                    #endif
                 ComponentTypes<T>.burstTypeId.Data >= 0) ||
                AllComponentTypes<T>.burstIsVersionedNoState.Data == 1 ||
                ComponentTypes<T>.burstIsFilterVersioned.Data == 1) {

                return true;

            }

            entityVersions.Increment(in allocator, entityId);
            bucket.data = data;
            bucket.version = tick;
            
            return false;

        }

    }
    
}
