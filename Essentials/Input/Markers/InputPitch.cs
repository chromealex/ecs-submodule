#if FIXED_POINT_MATH
using FLOAT2 = ME.ECS.fp2;
using FLOAT3 = ME.ECS.fp3;
using FLOAT4 = ME.ECS.fp4;
using QUATERNION = ME.ECS.fpquaternion;
#else
using FLOAT2 = UnityEngine.Vector2;
using FLOAT3 = UnityEngine.Vector3;
using FLOAT4 = UnityEngine.Vector4;
using QUATERNION = UnityEngine.Quaternion;
#endif

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

        public FLOAT3 worldPosition => (this.serializedPointer1.worldPosition + this.serializedPointer2.worldPosition) * 0.5f;

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

        public FLOAT3 worldPosition => (this.serializedPointer1.worldPosition + this.serializedPointer2.worldPosition) * 0.5f;

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

        public FLOAT3 worldPosition => (this.serializedPointer1.worldPosition + this.serializedPointer2.worldPosition) * 0.5f;

    }

}