
#if FIXED_POINT_MATH
using MATH = ME.ECS.fpmath;
using FLOAT = ME.ECS.fp;
using FLOAT2 = ME.ECS.fp2;
using FLOAT3 = ME.ECS.fp3;
using FLOAT4 = ME.ECS.fp4;
using QUATERNION = ME.ECS.fpquaternion;
#else
using MATH = Unity.Mathematics.math;
using FLOAT = System.Single;
using FLOAT2 = UnityEngine.Vector2;
using FLOAT3 = UnityEngine.Vector3;
using FLOAT4 = UnityEngine.Vector4;
using QUATERNION = UnityEngine.Quaternion;
#endif

namespace ME.ECS.Camera {
    
    public struct Camera : IComponent, IVersioned {

        public bool perspective;
        public FLOAT orthoSize;
        public FLOAT fieldOfView;
        public FLOAT aspect;
        public FLOAT nearClipPlane;
        public FLOAT farClipPlane;

    }

}