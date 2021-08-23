namespace ME.ECS.Essentials.Input.Components {

    public interface IInputGesture2FingersComponent : IStructComponent {
        
        InputPointerData setPointer1 { set; get; }
        InputPointerData setPointer2 { set; get; }
        
    }

    public struct InputGesturePitchUp : IInputGesture2FingersComponent {

        public InputPointerData pointer1;
        public InputPointerData pointer2;
        public InputPointerData setPointer1 {
            set => this.pointer1 = value;
            get => this.pointer1;
        }
        public InputPointerData setPointer2 {
            set => this.pointer2 = value;
            get => this.pointer2;
        }

    }

    public struct InputGesturePitchMove : IInputGesture2FingersComponent {

        public InputPointerData pointer1;
        public InputPointerData pointer2;
        public InputPointerData setPointer1 {
            set => this.pointer1 = value;
            get => this.pointer1;
        }
        public InputPointerData setPointer2 {
            set => this.pointer2 = value;
            get => this.pointer2;
        }

    }

    public struct InputGesturePitchDown : IInputGesture2FingersComponent {

        public InputPointerData pointer1;
        public InputPointerData pointer2;
        public InputPointerData setPointer1 {
            set => this.pointer1 = value;
            get => this.pointer1;
        }
        public InputPointerData setPointer2 {
            set => this.pointer2 = value;
            get => this.pointer2;
        }

    }

}