#if FIXED_POINT_MATH
using FLOAT2 = ME.ECS.FPVector2;
using FLOAT3 = ME.ECS.FPVector3;
using FLOAT4 = ME.ECS.FPVector4;
using QUATERNION = ME.ECS.FPQuaternion;
#else
using FLOAT2 = UnityEngine.Vector2;
using FLOAT3 = UnityEngine.Vector3;
using FLOAT4 = UnityEngine.Vector4;
using QUATERNION = UnityEngine.Quaternion;
#endif

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