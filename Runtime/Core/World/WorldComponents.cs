#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed partial class World {

        private ref Entity sharedEntity => ref this.currentState.sharedEntity;
        private bool sharedEntityInitialized;
        
        partial void OnSpawnComponents() {

            this.sharedEntityInitialized = false;

        }

        partial void OnRecycleComponents() {

            this.sharedEntityInitialized = false;

        }
        
    }

}