using FLOAT2 = UnityEngine.Vector2;
using FLOAT3 = UnityEngine.Vector3;
using FLOAT4 = UnityEngine.Vector4;
using QUATERNION = UnityEngine.Quaternion;

namespace ME.ECS.Transform {

    public struct Position2D : IStructComponent {

        public FLOAT2 value;

    }
    
    public struct Rotation2D : IStructComponent {

        public float value;

    }
    
    public struct Scale2D : IStructComponent {

        public FLOAT2 value;

    }

}