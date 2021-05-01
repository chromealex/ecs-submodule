using ME.ECS;

namespace ME.ECS.Essentials.Input.Components {

    public struct InputPointerDragBegin : IInputPointerComponent {

        public InputPointerData data;
        public InputPointerData setData {
            set => this.data = value;
        }

    }

    public struct InputPointerDragMove : IInputPointerComponent {
    
        public InputPointerData data;
        public InputPointerData setData {
            set => this.data = value;
        }

    }

    public struct InputPointerDragEnd : IInputPointerComponent {
    
        public InputPointerData data;
        public InputPointerData setData {
            set => this.data = value;
        }

    }

}