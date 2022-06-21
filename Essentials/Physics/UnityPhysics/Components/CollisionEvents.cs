using ME.ECS;

namespace ME.ECS.Essentials.Physics.Components {

    public struct PhysicsEventOnCollision : IComponentOneShot {

        public ME.ECS.Essentials.Physics.CollisionEvent data;

    }

    public struct PhysicsEventOnTrigger : IComponentOneShot {

        public ME.ECS.Essentials.Physics.TriggerEvent data;

    }

}