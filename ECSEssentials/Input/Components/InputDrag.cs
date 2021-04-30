using ME.ECS;

namespace ME.ECS.Essentials.Input.Components {

    public struct InputPointerDragBegin : IInputPointerComponent {

        public InputPointerData data { get; set; }

    }

    public struct InputPointerDragMove : IInputPointerComponent {
    
        public InputPointerData data { get; set; }

    }

    public struct InputPointerDragEnd : IInputPointerComponent {
    
        public InputPointerData data { get; set; }

    }

}