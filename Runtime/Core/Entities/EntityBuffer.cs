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

            var result = false;
            ref var bucket = ref reg.components[entity.id];
            reg.RemoveData(in entity, ref bucket);
            ref var state = ref bucket.state;
            if (state > 0) {

                state = 0;
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
                reg.UpdateVersion(in entity);
                if (AllComponentTypes<T>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
                if (ComponentTypes<T>.isFilterVersioned == true) world.UpdateFilterByStructComponentVersioned<T>(in entity);

                if (world.currentState.structComponents.entitiesIndexer.GetCount(entity.id) == 0 &&
                    (world.currentState.storage.flags.Get(entity.id) & (byte)EntityFlag.DestroyWithoutComponents) != 0) {
                    entity.SetOneShot<IsEntityEmptyOneShot>();
                }

                result = true;

            }

            return result;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ref T PushGet_INTERNAL<T>(World world, in Entity entity, StructComponents<T> reg, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            ref var bucket = ref reg.components[entity.id];
            ref var state = ref bucket.state;
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
                
            }

            if (ComponentTypes<T>.isFilterLambda == true && ComponentTypes<T>.typeId >= 0) {

                world.ValidateFilterByStructComponent<T>(in entity, true);
                
            }
            
            world.currentState.storage.versions.Increment(in entity);
            reg.UpdateVersion(in entity);
            if (AllComponentTypes<T>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
            if (ComponentTypes<T>.isFilterVersioned == true) world.UpdateFilterByStructComponentVersioned<T>(in entity);

            return ref bucket.data;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushSet_INTERNAL<T>(World world, in Entity entity, StructComponents<T> reg, in T data, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            var result = false;
            ref var bucket = ref reg.components[entity.id];
            reg.Replace(ref bucket, in data);
            ref var state = ref bucket.state;
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

            if (ComponentTypes<T>.isFilterLambda == true && ComponentTypes<T>.typeId >= 0) {

                world.ValidateFilterByStructComponent<T>(in entity);
                
            }
            
            world.currentState.storage.versions.Increment(in entity);
            reg.UpdateVersion(in entity);
            if (AllComponentTypes<T>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
            if (ComponentTypes<T>.isFilterVersioned == true) world.UpdateFilterByStructComponentVersioned<T>(in entity);

            return result;

        }

    }
    
    public static class DataBlittableBufferUtils {

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushRemove_INTERNAL<T>(World world, in Entity entity, StructComponentsBlittable<T> reg, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            var result = false;
            ref var bucket = ref reg.components[entity.id];
            reg.RemoveData(in entity, ref bucket);
            ref var state = ref bucket.state;
            if (state > 0) {

                state = 0;
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
                reg.UpdateVersion(in entity);
                if (AllComponentTypes<T>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
                if (ComponentTypes<T>.isFilterVersioned == true) world.UpdateFilterByStructComponentVersioned<T>(in entity);

                if (world.currentState.structComponents.entitiesIndexer.GetCount(entity.id) == 0 &&
                    (world.currentState.storage.flags.Get(entity.id) & (byte)EntityFlag.DestroyWithoutComponents) != 0) {
                    entity.SetOneShot<IsEntityEmptyOneShot>();
                }

                result = true;

            }

            return result;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ref T PushGet_INTERNAL<T>(World world, in Entity entity, StructComponentsBlittable<T> reg, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            ref var bucket = ref reg.components[entity.id];
            ref var state = ref bucket.state;
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
                
            }

            if (ComponentTypes<T>.isFilterLambda == true && ComponentTypes<T>.typeId >= 0) {

                world.ValidateFilterByStructComponent<T>(in entity, true);
                
            }
            
            world.currentState.storage.versions.Increment(in entity);
            reg.UpdateVersion(in entity);
            if (AllComponentTypes<T>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
            if (ComponentTypes<T>.isFilterVersioned == true) world.UpdateFilterByStructComponentVersioned<T>(in entity);

            return ref bucket.data;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool PushSet_INTERNAL<T>(World world, in Entity entity, StructComponentsBlittable<T> reg, in T data, StorageType storageType = StorageType.Default) where T : struct, IComponentBase {

            var result = false;
            ref var bucket = ref reg.components[entity.id];
            reg.Replace(ref bucket, in data);
            ref var state = ref bucket.state;
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

            if (ComponentTypes<T>.isFilterLambda == true && ComponentTypes<T>.typeId >= 0) {

                world.ValidateFilterByStructComponent<T>(in entity);
                
            }
            
            world.currentState.storage.versions.Increment(in entity);
            reg.UpdateVersion(in entity);
            if (AllComponentTypes<T>.isVersionedNoState == true) ++reg.versionsNoState.arr[entity.id];
            if (ComponentTypes<T>.isFilterVersioned == true) world.UpdateFilterByStructComponentVersioned<T>(in entity);

            return result;

        }

    }
    
    public static class DataBlittableBurstBufferUtils {

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool NeedToPush<T>(Tick tick, ref EntityVersions entityVersions, int entityId, ref Component<T> bucket, in T data) where T : unmanaged, IComponentBase {

            if (bucket.state == 0 ||
                (ComponentTypes<T>.burstIsFilterLambda.Data == 1 && ComponentTypes<T>.burstTypeId.Data >= 0) ||
                AllComponentTypes<T>.burstIsVersionedNoState.Data == 1 ||
                ComponentTypes<T>.burstIsFilterVersioned.Data == 1) {

                return true;

            }

            entityVersions.Increment(entityId);
            bucket.data = data;
            bucket.version = tick;
            
            return false;

        }

    }
    
}
