#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS.Transform {

    public struct Position : IComponent, IVersioned {

        public float3 value;

    }
    
    public struct Rotation : IComponent, IVersioned {

        public quaternion value;

    }
    
    public struct Scale : IComponent, IVersioned {

        public float3 value;

    }

}