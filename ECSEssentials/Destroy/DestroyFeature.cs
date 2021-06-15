using ME.ECS;

namespace ME.ECS.Essentials {

    using Destroy.Components; using Destroy.Modules; using Destroy.Systems; using Destroy.Markers;
    
    namespace Destroy.Components {}
    namespace Destroy.Modules {}
    namespace Destroy.Systems {}
    namespace Destroy.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class DestroyFeature : Feature {

        protected override void OnConstruct() {

            this.AddSystem<DestroySystem>();
            this.AddSystem<DestroyByTimeSystem>();

        }

        protected override void OnDeconstruct() {
            
        }

    }

}