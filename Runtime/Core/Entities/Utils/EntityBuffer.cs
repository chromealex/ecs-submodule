#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using ME.ECS.Collections;
using Unity.Jobs;

namespace ME.ECS {

    using Unity.Burst;
    using Unity.Collections.LowLevel.Unsafe;
    
    public static class DataBufferUtils {

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushRemove_INTERNAL<T>(World world, in Entity entity, StructComponents<T> reg, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            ref var bucket = ref reg.components[entity.id];
            reg.RemoveData(in entity, ref bucket);
            return DataBufferUtilsBase.PushRemoveCreate_INTERNAL(ref bucket, world, in entity, storageType);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ref T PushGet_INTERNAL<T>(World world, in Entity entity, StructComponents<T> reg, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            ref var bucket = ref reg.components[entity.id];
            ref var state = ref bucket.state;
            DataBufferUtilsBase.PushSetCreate_INTERNAL<T>(ref state, world, in entity, storageType, makeRequest: true);
            return ref bucket.data;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushSet_INTERNAL<T>(World world, in Entity entity, StructComponents<T> reg, in T data, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            ref var bucket = ref reg.components[entity.id];
            reg.Replace(ref bucket, in data);
            ref var state = ref bucket.state;
            return DataBufferUtilsBase.PushSetCreate_INTERNAL<T>(ref state, world, in entity, storageType, makeRequest: false);

        }

    }
    
    public static class DataBlittableBurstBufferUtils {

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool NeedToPush<T>(in ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator, Tick tick, ref EntityVersions entityVersions, int entityId, ref Component<T> bucket, in T data) where T : unmanaged, IComponentBase {

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
            bucket.state = 1;
            bucket.data = data;
            bucket.version = (ushort)(long)tick;
            
            return false;

        }

    }
    
}
