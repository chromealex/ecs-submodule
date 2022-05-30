using ME.ECS;

namespace ME.ECS.Essentials.Physics.Components {

    public struct PhysicsEventOnCollision : IComponentOneShot {

        public UnityS.Physics.CollisionEvent data;

    }

    public struct PhysicsEventOnTrigger : IComponentOneShot {

        public UnityS.Physics.TriggerEvent data;

    }

}