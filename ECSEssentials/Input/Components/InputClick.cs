using ME.ECS;

namespace ME.ECS.Essentials.Input.Components {

    public interface IInputPointerComponent : IStructComponent {
        
        InputPointerData data { set; }
        
    }
    
    public struct InputPointerClick : IInputPointerComponent {

        public InputPointerData data { get; set; }

    }

    public struct InputPointerDoubleClick : IInputPointerComponent {

        public InputPointerData data { get; set; }

    }

    public struct InputPointerDown : IInputPointerComponent {

        public InputPointerData data { get; set; }

    }

    public struct InputPointerUp : IInputPointerComponent {

        public InputPointerData data { get; set; }

    }

}