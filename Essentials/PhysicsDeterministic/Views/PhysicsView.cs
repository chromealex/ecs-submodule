
#if GAMEOBJECT_VIEWS_MODULE_SUPPORT
namespace ME.ECS.Views {
    
    using Providers;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract class PhysicsView : MonoBehaviourView {

        new public UnityEngine.Rigidbody rigidbody;

        public void OnCollisionEnter(UnityEngine.Collision other) {

            this.entity.Set(new ME.ECS.Essentials.PhysicsDeterministic.Components.PhysicsOnCollisionEnter() {
                collision = other,
            });

        }

        public void OnCollisionExit(UnityEngine.Collision other) {

            this.entity.Set(new ME.ECS.Essentials.PhysicsDeterministic.Components.PhysicsOnCollisionExit() {
                collision = other,
            });

        }

        public void OnCollisionStay(UnityEngine.Collision other) {

            this.entity.Set(new ME.ECS.Essentials.PhysicsDeterministic.Components.PhysicsOnCollisionStay() {
                collision = other,
            });

        }

        public override void ApplyPhysicsState(float deltaTime) {

            this.entity.SetPosition(this.rigidbody.position);
            this.entity.SetRotation(this.rigidbody.rotation);

        }

    }

}
#endif