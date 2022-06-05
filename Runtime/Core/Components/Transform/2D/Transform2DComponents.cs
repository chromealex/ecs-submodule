#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS.Transform {

    [ComponentGroup(typeof(TransformComponentConstants.GroupInfo))]
    [ComponentOrder(1)]
    public struct Position2D : IComponent, IVersioned {

        public float2 value;

    }
    
    [ComponentGroup(typeof(TransformComponentConstants.GroupInfo))]
    [ComponentOrder(2)]
    public struct Rotation2D : IComponent, IVersioned {

        public tfloat value;

    }
    
    [ComponentGroup(typeof(TransformComponentConstants.GroupInfo))]
    [ComponentOrder(3)]
    public struct Scale2D : IComponent, IVersioned {

        public float2 value;

    }

}