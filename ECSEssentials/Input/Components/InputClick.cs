using ME.ECS;

namespace ME.ECS.Essentials.Input.Components {

    public interface IInputPointerComponent : IStructComponent {
        
        InputPointerData setData { set; }
        
    }
    
    public struct InputPointerClick : IInputPointerComponent {

        public InputPointerData data;
        public InputPointerData setData {
            set => this.data = value;
        }

    }

    public struct InputPointerDoubleClick : IInputPointerComponent {

        public InputPointerData data;
        public InputPointerData setData {
            set => this.data = value;
        }

    }

    public struct InputPointerDown : IInputPointerComponent {

        public InputPointerData data;
        public InputPointerData setData {
            set => this.data = value;
        }

    }

    public struct InputPointerUp : IInputPointerComponent {

        public InputPointerData data;
        public InputPointerData setData {
            set => this.data = value;
        }

    }

}