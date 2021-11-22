using ME.ECS;

namespace ME.ECS.Essentials.Input.Components {

    public interface IInputPointerComponent : IStructComponent {
        
        InputPointerData setData { set; get; }
        
    }

    public struct InputPointerClick : IInputPointerComponent {

        public InputPointerData data;
        public InputPointerData setData {
            set => this.data = value;
            get => this.data;
        }

    }

    public struct InputPointerDoubleClick : IInputPointerComponent {

        public InputPointerData data;
        public InputPointerData setData {
            set => this.data = value;
            get => this.data;
        }

    }

    public struct InputPointerDown : IInputPointerComponent {

        public InputPointerData data;
        public InputPointerData setData {
            set => this.data = value;
            get => this.data;
        }

    }

    public struct InputPointerUp : IInputPointerComponent {

        public InputPointerData data;
        public InputPointerData setData {
            set => this.data = value;
            get => this.data;
        }

    }

}