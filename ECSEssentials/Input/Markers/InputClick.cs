using ME.ECS;

namespace ME.ECS.Essentials.Input.Input.Markers {

    public interface IInputPointerMarker : IEventMarker {

        InputPointerData data { get; }

    }

    public struct InputPointerClick : IInputPointerMarker {

        public InputPointerData serializedData;
        public InputPointerData data {
            get => this.serializedData;
            set => this.serializedData = value;
        }

    }

    public struct InputPointerDoubleClick : IInputPointerMarker {

        public InputPointerData serializedData;
        public InputPointerData data {
            get => this.serializedData;
            set => this.serializedData = value;
        }

    }

    public struct InputPointerDown : IInputPointerMarker {
        
        public InputPointerData serializedData;
        public InputPointerData data {
            get => this.serializedData;
            set => this.serializedData = value;
        }
        
    }

    public struct InputPointerMove : IInputPointerMarker {
        
        public InputPointerData serializedData;
        public InputPointerData data {
            get => this.serializedData;
            set => this.serializedData = value;
        }
        
    }

    public struct InputPointerUp : IInputPointerMarker {
        
        public InputPointerData serializedData;
        public InputPointerData data {
            get => this.serializedData;
            set => this.serializedData = value;
        }
        
    }

}