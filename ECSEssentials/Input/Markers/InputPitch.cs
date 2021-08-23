namespace ME.ECS.Essentials.Input.Input.Markers {

    public interface IInputGesture2FingersMarker : IGestureMarker {
        
        InputPointerData pointer1 { set; get; }
        InputPointerData pointer2 { set; get; }
        
    }

    public struct InputGesturePitchUp : IInputGesture2FingersMarker {

        public InputPointerData serializedPointer1;
        public InputPointerData serializedPointer2;
        public InputPointerData pointer1 {
            get => this.serializedPointer1;
            set => this.serializedPointer1 = value;
        }
        public InputPointerData pointer2 {
            get => this.serializedPointer2;
            set => this.serializedPointer2 = value;
        }

        public UnityEngine.Vector3 worldPosition => (this.serializedPointer1.worldPosition + this.serializedPointer2.worldPosition) * 0.5f;

    }

    public struct InputGesturePitchDown : IInputGesture2FingersMarker {

        public InputPointerData serializedPointer1;
        public InputPointerData serializedPointer2;
        public InputPointerData pointer1 {
            get => this.serializedPointer1;
            set => this.serializedPointer1 = value;
        }
        public InputPointerData pointer2 {
            get => this.serializedPointer2;
            set => this.serializedPointer2 = value;
        }

        public UnityEngine.Vector3 worldPosition => (this.serializedPointer1.worldPosition + this.serializedPointer2.worldPosition) * 0.5f;

    }

    public struct InputGesturePitchMove : IInputGesture2FingersMarker {

        public InputPointerData serializedPointer1;
        public InputPointerData serializedPointer2;
        public InputPointerData pointer1 {
            get => this.serializedPointer1;
            set => this.serializedPointer1 = value;
        }
        public InputPointerData pointer2 {
            get => this.serializedPointer2;
            set => this.serializedPointer2 = value;
        }

        public UnityEngine.Vector3 worldPosition => (this.serializedPointer1.worldPosition + this.serializedPointer2.worldPosition) * 0.5f;

    }

}