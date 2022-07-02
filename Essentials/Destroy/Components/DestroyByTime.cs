#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif
using ME.ECS;

namespace ME.ECS.Essentials.Destroy.Components {

    public struct DestroyByTime : IComponent {

        public tfloat time;

    }
    
}