namespace ME.ECS.Essentials.PhysicsDeterministic.Components {

    public struct PhysicsRigidbody : IStructComponent {}
    
    public struct PhysicsOnCollisionEnter : IStructComponent {

        [GeneratorIgnoreManagedType]
        public UnityEngine.Collision collision;

    }

    public struct PhysicsOnCollisionExit : IStructComponent {

        [GeneratorIgnoreManagedType]
        public UnityEngine.Collision collision;

    }

    public struct PhysicsOnCollisionStay : IStructComponent {

        [GeneratorIgnoreManagedType]
        public UnityEngine.Collision collision;

    }

}