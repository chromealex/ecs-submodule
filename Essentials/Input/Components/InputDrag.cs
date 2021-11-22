using ME.ECS;

namespace ME.ECS.Essentials.Input.Components {

    public struct InputPointerDragBegin : IInputPointerComponent {

        public InputPointerData data;
        public InputPointerData setData {
            set => this.data = value;
            get => this.data;
        }

    }

    public struct InputPointerDragMove : IInputPointerComponent {
    
        public InputPointerData data;
        public InputPointerData setData {
            set => this.data = value;
            get => this.data;
        }

    }

    public struct InputPointerDragEnd : IInputPointerComponent {
    
        public InputPointerData data;
        public InputPointerData setData {
            set => this.data = value;
            get => this.data;
        }

    }

}