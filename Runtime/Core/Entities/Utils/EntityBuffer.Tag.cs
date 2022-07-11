#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using ME.ECS.Collections;
using Unity.Jobs;

namespace ME.ECS {

    using Unity.Burst;
    using Unity.Collections.LowLevel.Unsafe;
    
    public static class DataTagBufferUtils {

        //[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushRemove_INTERNAL<T>(World world, in Entity entity, StructComponentsTag<T> reg, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            var result = false;
            ref var state = ref reg.componentStates[entity.id];
            if (state > 0) {

                reg.RemoveData(in entity, ref state);
                
                if (storageType == StorageType.Default) {
                    world.currentState.structComponents.entitiesIndexer.Remove(entity.id, AllComponentTypes<T>.typeId);
                } else if (storageType == StorageType.NoState) {
                    world.structComponentsNoState.entitiesIndexer.Remove(entity.id, OneShotComponentTypes<T>.typeId);
                }
                
                if (ComponentTypes<T>.typeId >= 0) {

                    world.currentState.storage.archetypes.Remove<T>(in entity);
                    world.RemoveFilterByStructComponent<T>(in entity);
                    world.UpdateFilterByStructComponent<T>(in entity);

                }

                world.currentState.storage.versions.Increment(in entity);
                //reg.UpdateVersion(in entity);
                #if !COMPONENTS_VERSION_NO_STATE_DISABLED
                if (AllComponentTypes<T>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
                #endif
                #if FILTERS_STORAGE_LEGACY
                if (ComponentTypes<T>.isFilterVersioned == true) world.UpdateFilterByStructComponentVersioned<T>(in entity);
                #endif

                if (world.currentState.structComponents.entitiesIndexer.GetCount(entity.id) == 0 &&
                    (world.currentState.storage.flags.Get(entity.id) & (byte)EntityFlag.DestroyWithoutComponents) != 0) {
                    entity.SetOneShot<IsEntityEmptyOneShot>();
                }

                result = true;

            }

            return result;

        }

        //[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushSet_INTERNAL<T>(World world, in Entity entity, StructComponentsTag<T> reg, in T data, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            var result = false;
            ref var state = ref reg.componentStates[entity.id];
            if (state == 0) {

                state = 1;
                
                if (world.currentState.structComponents.entitiesIndexer.GetCount(entity.id) == 0 &&
                    (world.currentState.storage.flags.Get(entity.id) & (byte)EntityFlag.DestroyWithoutComponents) != 0) {
                    entity.RemoveOneShot<IsEntityEmptyOneShot>();
                }

                if (storageType == StorageType.Default) {
                    world.currentState.structComponents.entitiesIndexer.Set(entity.id, AllComponentTypes<T>.typeId);
                } else if (storageType == StorageType.NoState) {
                    world.structComponentsNoState.entitiesIndexer.Set(entity.id, OneShotComponentTypes<T>.typeId);
                }

                if (ComponentTypes<T>.typeId >= 0) {

                    world.currentState.storage.archetypes.Set<T>(in entity);
                    world.AddFilterByStructComponent<T>(in entity);
                    world.UpdateFilterByStructComponent<T>(in entity);

                }

                if (AllComponentTypes<T>.isOneShot == true) {
                    
                    var task = new StructComponentsContainer.NextTickTask {
                        lifetime = ComponentLifetime.NotifyAllSystemsBelow,
                        storageType = StorageType.NoState,
                        secondsLifetime = 0f,
                        entity = entity,
                        dataIndex = OneShotComponentTypes<T>.typeId,
                    };

                    if (world.structComponentsNoState.nextTickTasks.Add(task) == false) {

                        task.Recycle();

                    }

                }

                result = true;

            }

            // Don't need lambda when use tag components
            /*
            if (ComponentTypes<T>.isFilterLambda == true && ComponentTypes<T>.typeId >= 0) {

                world.ValidateFilterByStructComponent<T>(in entity);
                
            }*/
            
            world.currentState.storage.versions.Increment(in entity);
            //reg.UpdateVersion(in entity);
            #if !COMPONENTS_VERSION_NO_STATE_DISABLED
            if (AllComponentTypes<T>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
            #endif
            #if FILTERS_STORAGE_LEGACY
            if (ComponentTypes<T>.isFilterVersioned == true) world.UpdateFilterByStructComponentVersioned<T>(in entity);
            #endif

            return result;

        }

    }
    
}
