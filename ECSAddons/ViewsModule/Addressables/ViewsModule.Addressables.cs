#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if VIEWS_MODULE_SUPPORT
namespace ME.ECS {
    
    using Views;

    public static class ViewsModuleAddressables {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ViewId RegisterViewSource<TProvider>(this ViewsModule viewsModule, TProvider providerInitializer, UnityEngine.AddressableAssets.AssetReference addressablePrefab) where TProvider : struct, IViewsProviderInitializer {

            if (addressablePrefab == null) {

                ViewSourceIsNullException.Throw();

            }

            #if VIEWS_REGISTER_VIEW_SOURCE_CHECK_STATE
            if (viewsModule.world.HasStep(WorldStep.LogicTick) == true) {

                throw new InStateException();

            }
            #endif

            var handle = addressablePrefab.LoadAssetAsync<IView>();
            var prefab = handle.WaitForCompletion();
            return viewsModule.RegisterViewSource(providerInitializer, prefab);

        }

    }

}
#endif