namespace ME.ECS.Essentials.PhysicsDeterministic.Systems {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class PhysicsRemoveOnceSystem : ISystemFilter {
        
        public World world { get; set; }
        
        void ISystemBase.OnConstruct() {}
        
        void ISystemBase.OnDeconstruct() {}
        
        bool ISystemFilter.jobs => true;
        int ISystemFilter.jobsBatchCount => 64;
        Filter ISystemFilter.filter { get; set; }
        Filter ISystemFilter.CreateFilter() {
            
            return Filter.Create("Filter-PhysicsRemoveOnceSystem")
                         .With<ME.ECS.Essentials.PhysicsDeterministic.Components.PhysicsRigidbody>()
                         .Push();
            
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime) {

            entity.Remove<ME.ECS.Essentials.PhysicsDeterministic.Components.PhysicsOnCollisionEnter>();
            entity.Remove<ME.ECS.Essentials.PhysicsDeterministic.Components.PhysicsOnCollisionExit>();
            entity.Remove<ME.ECS.Essentials.PhysicsDeterministic.Components.PhysicsOnCollisionStay>();

        }
    
    }
    
}