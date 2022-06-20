using ME.ECS;

namespace ME.ECS.Essentials.GOAP {

    using GOAP.Components; using GOAP.Modules; using GOAP.Systems; using GOAP.Markers;
    
    namespace Components {}
    namespace Modules {}
    namespace Systems {}
    namespace Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class GOAPFeature : Feature {

        public bool showDebug;

        protected override void OnConstruct() {

            this.AddModule<GOAPModule>();
            this.AddSystem<GOAPPlannerSystem>();
            this.AddSystem<GOAPAutoResolveSystem>();

        }

        protected override void OnDeconstruct() {
            
        }

    }

}