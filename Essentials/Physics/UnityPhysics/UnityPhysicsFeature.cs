using ME.ECS;

namespace ME.ECS.Essentials.Physics.Core {

    using Collisions.Components; using Collisions.Modules; using Collisions.Systems; using Collisions.Markers;
    
    namespace Collisions.Components {}
    namespace Collisions.Modules {}
    namespace Collisions.Systems {}
    namespace Collisions.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class UnityPhysicsFeature : Feature {

        [UnityEngine.TooltipAttribute("Should feature create oneshot-entity with collision data?")]
        public bool sendCollisionEvents = false;
        [UnityEngine.TooltipAttribute("Should feature create oneshot-entity with trigger data?")]
        public bool sendTriggerEvents = false;
        
        internal ME.ECS.Essentials.Physics.PhysicsWorld physicsWorldInternal;
        public ref ME.ECS.Essentials.Physics.PhysicsWorld physicsWorld => ref this.physicsWorldInternal;
        
        protected override void OnConstruct() {

            this.physicsWorldInternal = new ME.ECS.Essentials.Physics.PhysicsWorld(0, 0, 0);
            this.AddSystem<BuildPhysicsWorldSystem>();
            
        }

        protected override void OnDeconstruct() {
            
        }

    }

}