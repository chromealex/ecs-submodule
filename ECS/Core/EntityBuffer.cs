#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using ME.ECS.Collections;
using Unity.Jobs;

namespace ME.ECS {

    public static class DataBufferUtils {

        public static void ForEach<T0>(this Filter filter, ME.ECS.Filters.R<T0> onEach)  where T0:struct,IStructComponentBase {
            
            var bag = new ME.ECS.Buffers.FilterBag<T0>(filter, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) {

                onEach.Invoke(in bag.GetEntity(i), ref bag.GetT0(i));

            }

        }
        
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