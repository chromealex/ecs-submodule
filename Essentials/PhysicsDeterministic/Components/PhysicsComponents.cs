namespace ME.ECS.Essentials.PhysicsDeterministic.Components {

    public struct PhysicsRigidbody : IComponent {}
    
    public struct PhysicsOnCollisionEnter : IComponent {

        [GeneratorIgnoreManagedType]
        public UnityEngine.Collision collision;

    }

    public struct PhysicsOnCollisionExit : IComponent {

        [GeneratorIgnoreManagedType]
        public UnityEngine.Collision collision;

    }

    public struct PhysicsOnCollisionStay : IComponent {

        [GeneratorIgnoreManagedType]
        public UnityEngine.Collision collision;

    }

}