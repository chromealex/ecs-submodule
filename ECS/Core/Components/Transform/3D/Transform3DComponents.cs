using FLOAT2 = UnityEngine.Vector2;
using FLOAT3 = UnityEngine.Vector3;
using FLOAT4 = UnityEngine.Vector4;
using QUATERNION = UnityEngine.Quaternion;

namespace ME.ECS.Transform {

    public struct Position : IStructComponent {

        public FLOAT3 value;

    }
    
    public struct Rotation : IStructComponent {

        public QUATERNION value;

    }
    
    public struct Scale : IStructComponent {

        public FLOAT3 value;

    }

}