#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if VIEWS_MODULE_SUPPORT && GAMEOBJECT_VIEWS_MODULE_SUPPORT
namespace ME.ECS {
    
    using Views;
    using Views.Providers;

    public static class WorldAddressables {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ViewId RegisterViewSource(this World world, UnityEngine.AddressableAssets.AssetReference prefab) {

            return world.RegisterViewSource(new UnityGameObjectProviderInitializer(), prefab);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ViewId RegisterViewSource<TProvider>(this World world, TProvider providerInitializer, UnityEngine.AddressableAssets.AssetReference prefab) where TProvider : struct, IViewsProviderInitializer {

            var viewsModule = world.GetModule<ViewsModule>();
            return viewsModule.RegisterViewSource(providerInitializer, prefab);

        }

    }

}
#endif