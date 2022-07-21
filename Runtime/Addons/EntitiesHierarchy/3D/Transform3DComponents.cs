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
    public struct Position : IComponent, IVersioned {

        public float3 value;

    }
    
    [ComponentGroup(typeof(TransformComponentConstants.GroupInfo))]
    [ComponentOrder(2)]
    public struct Rotation : IComponent, IVersioned {

        public quaternion value;

    }
    
    [ComponentGroup(typeof(TransformComponentConstants.GroupInfo))]
    [ComponentOrder(3)]
    public struct Scale : IComponent, IVersioned {

        public float3 value;

    }

}