
namespace ME.ECS.Essentials.Input.Components {

    public struct InputMask : IStructComponent {

        public bool allow;
        public Entity player;
        public bool checkRect;
        public bool insideRect;
        public UnityEngine.Rect rect;
        public InputEventType inputEventType;
        public int priority;

    }

    public struct InputAllowUI : IStructComponent {

        public bool allow;

    }
    
    public struct HasAnyInput : IStructComponent {}
    
}