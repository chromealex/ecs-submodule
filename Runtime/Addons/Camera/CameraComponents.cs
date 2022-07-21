
#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS.Camera {
    
    public struct Camera : IComponent, IVersioned {

        public bool perspective;
        public tfloat orthoSize;
        public tfloat fieldOfView;
        public tfloat aspect;
        public tfloat nearClipPlane;
        public tfloat farClipPlane;

    }

}