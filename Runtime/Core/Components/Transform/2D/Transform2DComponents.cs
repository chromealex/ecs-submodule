#if FIXED_POINT_MATH
using FLOAT = ME.ECS.fp;
using FLOAT2 = ME.ECS.fp2;
using FLOAT3 = ME.ECS.fp3;
using FLOAT4 = ME.ECS.fp4;
using QUATERNION = ME.ECS.fpquaternion;
#else
using FLOAT = System.Single;
using FLOAT2 = UnityEngine.Vector2;
using FLOAT3 = UnityEngine.Vector3;
using FLOAT4 = UnityEngine.Vector4;
using QUATERNION = UnityEngine.Quaternion;
#endif

namespace ME.ECS.Transform {

    public struct Position2D : IComponent, IVersioned {

        public FLOAT2 value;

    }
    
    public struct Rotation2D : IComponent, IVersioned {

        public FLOAT value;

    }
    
    public struct Scale2D : IComponent, IVersioned {

        public FLOAT2 value;

    }

}