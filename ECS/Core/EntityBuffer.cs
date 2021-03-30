#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using ME.ECS.Collections;

namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct DataBuffer<T> where T : struct, IStructComponent {

        [Unity.Collections.NativeDisableParallelForRestriction] private NativeArrayBurst<T> arr;
        [Unity.Collections.NativeDisableParallelForRestriction] private NativeArrayBurst<byte> ops;
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public DataBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) {

            var reg = (StructComponents<T>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T>()];
            this.arr = new NativeArrayBurst<T>(reg.components.data.arr, allocator);
            this.ops = new NativeArrayBurst<byte>(reg.components.data.arr.Length, allocator);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int Push(World world, ME.ECS.Collections.BufferArray<Entity> arr, int max, ME.ECS.Collections.NativeArrayBurst<int> filterEntities) {

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
                        if (world.currentState.filters.HasInAnyFilter<T>() == true) {

                            world.currentState.storage.archetypes.Remove<T>(in entity);
                            world.UpdateFilterByStructComponent<T>(in entity);

                        }

                        System.Threading.Interlocked.Decrement(ref world.currentState.structComponents.count);
                        
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
                        if (world.currentState.filters.HasInAnyFilter<T>() == true) {

                            world.currentState.storage.archetypes.Set<T>(in entity);
                            world.UpdateFilterByStructComponent<T>(in entity);

                        }

                        System.Threading.Interlocked.Increment(ref world.currentState.structComponents.count);

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
            this.ops.Dispose();
            
        }

        public void Remove(int entityId) {

            this.ops[entityId] |= 0x4;
            
        }

        public void Set(int entityId, in T data) {

            this.ops[entityId] |= 0x2;
            this.arr[entityId] = data;
            
        }

        public ref T Get(int entityId) {

            this.ops[entityId] |= 0x2;
            return ref this.arr.GetRef(entityId);

        }

        public ref readonly T Read(int entityId) {

            return ref this.arr.GetRef(entityId);

        }

    }

}