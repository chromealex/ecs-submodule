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

namespace ME.ECS.Transform {

    public struct Position : IComponent, IVersioned {

        public FLOAT3 value;

    }
    
    public struct Rotation : IComponent, IVersioned {

        public QUATERNION value;

    }
    
    public struct Scale : IComponent, IVersioned {

        public FLOAT3 value;

    }

}