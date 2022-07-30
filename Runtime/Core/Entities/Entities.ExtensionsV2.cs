#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS {

    using Collections;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static partial class EntityExtensionsV2 {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity Remove<TComponent>(this in Entity entity) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.RemoveData<TComponent>(in entity);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool TryRead<TComponent>(this in Entity entity, out TComponent component) where TComponent : struct, IStructComponent {

            return Worlds.currentWorld.TryReadData<TComponent>(in entity, out component);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool Has<TComponent>(this in Entity entity) where TComponent : struct, IStructComponent {

            return Worlds.currentWorld.HasData<TComponent>(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref readonly TComponent Read<TComponent>(this in Entity entity) where TComponent : struct, IStructComponent {

            return ref Worlds.currentWorld.ReadData<TComponent>(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref TComponent Get<TComponent>(this in Entity entity) where TComponent : struct, IStructComponent {

            return ref Worlds.currentWorld.GetData<TComponent>(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity SetAs<TComponent>(this in Entity entity, DataConfigs.DataConfig source) where TComponent : struct, IStructComponent {

            if (source.TryRead(out TComponent c) == true) {

                if (AllComponentTypes<TComponent>.isTag == true) {

                    TComponent data = default;
                    entity.Set(data);

                } else {
                    
                    entity.Set(c);

                }

            } else {
                
                entity.Remove<TComponent>();
                
            }
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity SetAs<TComponent>(this in Entity entity, in Entity source) where TComponent : struct, IStructComponent {

            #if COMPONENTS_COPYABLE
            if (AllComponentTypes<TComponent>.isCopyable == true) {

                if (source.Has<TComponent>() == true) {

                    var id = AllComponentTypes<TComponent>.typeId;
                    var reg = Worlds.current.currentState.structComponents.list[id];
                    reg.CopyFrom(in source, in entity);

                } else {

                    entity.Remove<TComponent>();

                }

            } else
            #endif
            {
                
                if (AllComponentTypes<TComponent>.isTag == false) {

                    if (source.TryRead(out TComponent c) == true) {

                        entity.Set(c);

                    } else {

                        entity.Remove<TComponent>();

                    }

                } else {

                    if (source.Has<TComponent>() == true) {

                        entity.Set<TComponent>();

                    } else {

                        entity.Remove<TComponent>();

                    }

                }

            }

            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity Set<TComponent>(this in Entity entity) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData<TComponent>(in entity);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity Set<TComponent>(this in Entity entity, ComponentLifetime lifetime) where TComponent : unmanaged, IStructComponent {

            Worlds.currentWorld.SetData<TComponent>(in entity, lifetime);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity Set<TComponent>(this in Entity entity, in TComponent data) where TComponent : struct, IStructComponent {

            Worlds.currentWorld.SetData(in entity, in data);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity Set<TComponent>(this in Entity entity, in TComponent data, ComponentLifetime lifetime) where TComponent : unmanaged, IStructComponent {

            Worlds.currentWorld.SetData(in entity, in data, lifetime);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity Set<TComponent>(this in Entity entity, in TComponent data, ComponentLifetime lifetime, tfloat customLifetime) where TComponent : unmanaged, IStructComponent {

            Worlds.currentWorld.SetData(in entity, in data, lifetime, customLifetime);
            return entity;

        }

        #if !SHARED_COMPONENTS_DISABLED
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool HasShared<TComponent>(this in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {

            return Worlds.currentWorld.HasSharedData<TComponent>(in entity, groupId);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref readonly TComponent ReadShared<TComponent>(this in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {

            return ref Worlds.currentWorld.ReadSharedData<TComponent>(in entity, groupId);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref TComponent GetShared<TComponent>(this in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {

            return ref Worlds.currentWorld.GetSharedData<TComponent>(in entity, groupId);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity SetShared<TComponent>(this in Entity entity, in TComponent data, uint groupId = 0u) where TComponent : struct, IComponentShared {

            Worlds.currentWorld.SetSharedData(in entity, in data, groupId);
            return entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity RemoveShared<TComponent>(this in Entity entity, uint groupId = 0u) where TComponent : struct, IComponentShared {

            Worlds.currentWorld.RemoveSharedData<TComponent>(in entity, groupId);
            return entity;

        }
        #endif
        
    }

}