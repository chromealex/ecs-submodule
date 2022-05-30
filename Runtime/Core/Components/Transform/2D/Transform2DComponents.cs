#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS.Transform {

    public struct Position2D : IComponent, IVersioned {

        public float2 value;

    }
    
    public struct Rotation2D : IComponent, IVersioned {

        public tfloat value;

    }
    
    public struct Scale2D : IComponent, IVersioned {

        public float2 value;

    }

}