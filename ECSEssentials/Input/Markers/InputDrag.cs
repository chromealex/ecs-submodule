using ME.ECS;

namespace ME.ECS.Essentials.Input.Input.Markers {
    
    public struct InputPointerDragBegin : IInputPointerMarker {
        
        public InputPointerData serializedData;

        public InputPointerData data {
            get => this.serializedData;
            set => this.serializedData = value;
        }

    }

    public struct InputPointerDragMove : IInputPointerMarker {
        
        public InputPointerData serializedData;
        public InputPointerData data {
            get => this.serializedData;
            set => this.serializedData = value;
        }
        
    }

    public struct InputPointerDragEnd : IInputPointerMarker {
        
        public InputPointerData serializedData;
        public InputPointerData data {
            get => this.serializedData;
            set => this.serializedData = value;
        }
        
    }

}