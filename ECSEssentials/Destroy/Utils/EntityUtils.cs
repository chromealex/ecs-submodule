#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using ME.ECS;

namespace ME.ECS.Essentials {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class EntityUtils {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool IsDestroyed(this in Entity entity) {

            return entity.IsAlive() == false ||
                   entity.Has<ME.ECS.Essentials.Destroy.Components.Destroy>() == true ||
                   entity.Has<ME.ECS.Essentials.Destroy.Components.DestroyByTime>() == true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Destroy(this in Entity entity, float lifetime) {
            
            entity.Set(new ME.ECS.Essentials.Destroy.Components.DestroyByTime() {
                time = lifetime,
            });
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetDestroy(this in Entity entity, float lifetime) {
            
            entity.Set(new ME.ECS.Essentials.Destroy.Components.DestroyByTime() {
                time = lifetime,
            });
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetDestroy(this in Entity entity) {

            entity.Set(new ME.ECS.Essentials.Destroy.Components.Destroy(), ComponentLifetime.NotifyAllSystems);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetDestroyLifetime(this in Entity entity, float lifetime) {
            
            entity.Set(new ME.ECS.Essentials.Destroy.Components.DestroyByTime() {
                time = lifetime,
            });
            
        }

    }

}
