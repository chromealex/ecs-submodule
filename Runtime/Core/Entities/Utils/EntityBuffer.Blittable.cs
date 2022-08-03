#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using ME.ECS.Collections;
using Unity.Jobs;

namespace ME.ECS {

    using Unity.Burst;
    using Unity.Collections.LowLevel.Unsafe;
    
    public static class DataBlittableBufferUtils {

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushRemove_INTERNAL<T>(World world, in Entity entity, StructComponentsBlittable<T> reg, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            ref var bucket = ref reg.components[entity.id];
            reg.RemoveData(in entity, ref bucket);
            ref var state = ref bucket.state;
            return DataBufferUtilsBase.PushRemoveCreate_INTERNAL(ref state, world, in entity, reg, storageType);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushSet_INTERNAL<T>(World world, in Entity entity, StructComponentsBlittable<T> reg, in T data, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            ref var bucket = ref reg.components[entity.id];
            reg.Replace(ref bucket, in data);
            ref var state = ref bucket.state;
            return DataBufferUtilsBase.PushSetCreate_INTERNAL(ref state, world, reg, in entity, storageType, makeRequest: false);
            
        }

    }

    public static class DataUnmanagedBufferUtils {

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushRemove_INTERNAL<T>(World world, in Entity entity, StructComponentsUnmanaged<T> reg, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            ref var bucket = ref reg.Get(in entity);
            reg.RemoveData(in entity, ref bucket);
            ref var state = ref bucket.state;
            return DataBufferUtilsBase.PushRemoveCreate_INTERNAL(ref state, world, in entity, reg, storageType);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushSet_INTERNAL<T>(World world, in Entity entity, StructComponentsUnmanaged<T> reg, in T data, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            ref var bucket = ref reg.Get(in entity);
            reg.Replace(ref bucket, in data);
            ref var state = ref bucket.state;
            return DataBufferUtilsBase.PushSetCreate_INTERNAL(ref state, world, reg, in entity, storageType, makeRequest: false);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushRemove_INTERNAL<T>(World world, in Entity entity, ref ME.ECS.Collections.V3.MemoryAllocator allocator, ref UnmanagedComponentsStorage.Item<T> reg) where T : struct, IComponentBase {

            ref var bucket = ref reg.components.Get(ref allocator, entity.id);
            reg.RemoveData(ref bucket);
            ref var state = ref bucket.state;
            var stReg = (StructComponentsBase<T>)world.currentState.structComponents.list[AllComponentTypes<T>.typeId];
            var result = DataBufferUtilsBase.PushRemoveCreate_INTERNAL(ref state, world, in entity, stReg, StorageType.Default);
            reg.components.Remove(ref allocator, entity.id);
            return result;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void PushSet_INTERNAL<T>(World world, in Entity entity, ref ME.ECS.Collections.V3.MemoryAllocator allocator, ref UnmanagedComponentsStorage.Item<T> reg, in T data) where T : struct, IComponentBase {

            ref var bucket = ref reg.components.Get(ref allocator, entity.id);
            reg.Replace(ref bucket, in data);
            ref var state = ref bucket.state;
            var stReg = (StructComponentsBase<T>)world.currentState.structComponents.list[AllComponentTypes<T>.typeId];
            DataBufferUtilsBase.PushSetCreate_INTERNAL(ref state, world, stReg, in entity, StorageType.Default, makeRequest: false);

        }

    }

    public static class DataUnmanagedDisposableBufferUtils {

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushRemove_INTERNAL<T>(World world, in Entity entity, StructComponentsUnmanaged<T> reg, StorageType storageType = StorageType.Default) where T : struct, IComponentDisposable<T> {

            ref var bucket = ref reg.Get(in entity);
            reg.RemoveData(in entity, ref bucket);
            ref var state = ref bucket.state;
            return DataBufferUtilsBase.PushRemoveCreate_INTERNAL(ref state, world, in entity, reg, storageType);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushSet_INTERNAL<T>(World world, in Entity entity, StructComponentsUnmanaged<T> reg, in T data, StorageType storageType = StorageType.Default) where T : struct, IComponentDisposable<T> {

            ref var bucket = ref reg.Get(in entity);
            reg.Replace(ref bucket, in data);
            ref var state = ref bucket.state;
            return DataBufferUtilsBase.PushSetCreate_INTERNAL(ref state, world, reg, in entity, storageType, makeRequest: false);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushRemove_INTERNAL<T>(World world, in Entity entity, ref ME.ECS.Collections.V3.MemoryAllocator allocator, ref UnmanagedComponentsStorage.ItemDisposable<T> reg) where T : struct, IComponentDisposable<T> {

            ref var bucket = ref reg.components.Get(ref allocator, entity.id);
            reg.RemoveData(ref allocator, ref bucket);
            ref var state = ref bucket.state;
            var stReg = (StructComponentsBase<T>)world.currentState.structComponents.list[AllComponentTypes<T>.typeId];
            var result = DataBufferUtilsBase.PushRemoveCreate_INTERNAL(ref state, world, in entity, stReg, StorageType.Default);
            reg.components.Remove(ref allocator, entity.id);
            return result;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void PushSet_INTERNAL<T>(World world, in Entity entity, ref ME.ECS.Collections.V3.MemoryAllocator allocator, ref UnmanagedComponentsStorage.ItemDisposable<T> reg, in T data) where T : struct, IComponentDisposable<T> {

            ref var bucket = ref reg.components.Get(ref allocator, entity.id);
            reg.Replace(ref allocator, ref bucket, in data);
            ref var state = ref bucket.state;
            var stReg = (StructComponentsBase<T>)world.currentState.structComponents.list[AllComponentTypes<T>.typeId];
            DataBufferUtilsBase.PushSetCreate_INTERNAL(ref state, world, stReg, in entity, StorageType.Default, makeRequest: false);

        }

    }

}
