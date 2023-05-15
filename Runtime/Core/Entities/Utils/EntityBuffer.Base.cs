#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using ME.ECS.Collections;
using Unity.Jobs;

namespace ME.ECS {

    using Unity.Burst;
    using Unity.Collections.LowLevel.Unsafe;
    using Collections.LowLevel;

    public static class DataBufferUtilsBase {

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushSetCreate_INTERNAL<T>(ref byte state, World world, in Entity entity, StorageType storageType, bool makeRequest) where T : struct, IComponentBase {

            var result = false;
            if (state == 0) {

                state = 1;

                if (world.currentState.structComponents.entitiesIndexer.GetCount(in world.currentState.allocator, entity.id) == 0 &&
                    (world.currentState.storage.flags.Get(in world.currentState.allocator, entity.id) & (byte)EntityFlag.DestroyWithoutComponents) != 0) {
                    entity.RemoveOneShot<IsEntityEmptyOneShot>();
                }

                if (storageType == StorageType.Default) {
                    world.currentState.structComponents.entitiesIndexer.Set(ref world.currentState.allocator, entity.id, AllComponentTypes<T>.typeId);
                } else if (storageType == StorageType.NoState) {
                    world.noStateData.storage.entitiesIndexer.Set(ref world.noStateData.allocator, entity.id, OneShotComponentTypes<T>.typeId);
                }

                if (ComponentTypes<T>.typeId >= 0) {

                    world.AddFilterByStructComponent<T>(ref world.currentState.allocator, in entity);

                }

                if (AllComponentTypes<T>.isOneShot == true) {

                    var task = new StructComponentsContainer.NextTickTask {
                        lifetime = ComponentLifetime.NotifyAllSystemsBelow,
                        storageType = StorageType.NoState,
                        secondsLifetime = 0f,
                        entity = entity,
                        dataIndex = OneShotComponentTypes<T>.typeId,
                    };

                    if (world.noStateData.storage.nextTickTasks.Add(ref world.noStateData.allocator, task) == false) {

                        task.Dispose(ref world.noStateData.allocator);

                    }

                }

                result = true;

            }

            #if !FILTERS_LAMBDA_DISABLED
            if (ComponentTypes<T>.isFilterLambda == true && ComponentTypes<T>.typeId >= 0) {

                world.ValidateFilterByStructComponent<T>(ref world.currentState.allocator, in entity, makeRequest);
                
            }
            #endif
            
            world.currentState.storage.versions.Increment(in world.currentState.allocator, in entity);
            
            return result;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushSetCreate_INTERNAL<T>(ref Component<T> bucket, World world, in Entity entity, StorageType storageType, bool makeRequest) where T : struct, IComponentBase {

            var result = false;
            if (bucket.state == 0) {

                bucket.state = 1;

                if (world.currentState.structComponents.entitiesIndexer.GetCount(in world.currentState.allocator, entity.id) == 0 &&
                    (world.currentState.storage.flags.Get(in world.currentState.allocator, entity.id) & (byte)EntityFlag.DestroyWithoutComponents) != 0) {
                    entity.RemoveOneShot<IsEntityEmptyOneShot>();
                }

                if (storageType == StorageType.Default) {
                    world.currentState.structComponents.entitiesIndexer.Set(ref world.currentState.allocator, entity.id, AllComponentTypes<T>.typeId);
                } else if (storageType == StorageType.NoState) {
                    world.noStateData.storage.entitiesIndexer.Set(ref world.noStateData.allocator, entity.id, OneShotComponentTypes<T>.typeId);
                }

                if (ComponentTypes<T>.typeId >= 0) {

                    world.AddFilterByStructComponent<T>(ref world.currentState.allocator, in entity);

                }

                if (AllComponentTypes<T>.isOneShot == true) {

                    var task = new StructComponentsContainer.NextTickTask {
                        lifetime = ComponentLifetime.NotifyAllSystemsBelow,
                        storageType = StorageType.NoState,
                        secondsLifetime = 0f,
                        entity = entity,
                        dataIndex = OneShotComponentTypes<T>.typeId,
                    };

                    if (world.noStateData.storage.nextTickTasks.Add(ref world.noStateData.allocator, task) == false) {

                        task.Dispose(ref world.noStateData.allocator);

                    }

                }

                result = true;

            }

            #if !FILTERS_LAMBDA_DISABLED
            if (ComponentTypes<T>.isFilterLambda == true && ComponentTypes<T>.typeId >= 0) {

                world.ValidateFilterByStructComponent<T>(ref world.currentState.allocator, in entity, makeRequest);
                
            }
            #endif
            
            world.currentState.storage.versions.Increment(in world.currentState.allocator, in entity);
            if (AllComponentTypes<T>.isTag == false) {
                if (AllComponentTypes<T>.isVersioned == true) {
                    bucket.version = (ushort)(long)world.GetCurrentTick();
                }
            }
            
            return result;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushRemoveCreate_INTERNAL<T>(ref Component<T> bucket, World world, in Entity entity, StorageType storageType) where T : struct, IComponentBase {

            var result = false;
            if (bucket.state > 0) {

                bucket.state = 0;
                if (storageType == StorageType.Default) {
                    world.currentState.structComponents.entitiesIndexer.Remove(ref world.currentState.allocator, entity.id, AllComponentTypes<T>.typeId);
                } else if (storageType == StorageType.NoState) {
                    world.noStateData.storage.entitiesIndexer.Remove(ref world.noStateData.allocator, entity.id, OneShotComponentTypes<T>.typeId);
                }
                
                if (ComponentTypes<T>.typeId >= 0) {

                    world.RemoveFilterByStructComponent<T>(ref world.currentState.allocator, in entity);

                }

                world.currentState.storage.versions.Increment(in world.currentState.allocator, in entity);
                
                if (world.currentState.structComponents.entitiesIndexer.GetCount(in world.currentState.allocator, entity.id) == 0 &&
                    (world.currentState.storage.flags.Get(in world.currentState.allocator, entity.id) & (byte)EntityFlag.DestroyWithoutComponents) != 0) {
                    entity.SetOneShot<IsEntityEmptyOneShot>();
                }

                result = true;

            }

            return result;

        }

    }

}
