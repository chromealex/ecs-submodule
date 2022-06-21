#if FIXED_POINT_MATH
using math = ME.ECS.Mathematics.math;
using float3 = ME.ECS.Mathematics.float3;
#else
using math = Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;
#endif
using UnityEngine;

namespace ME.ECS.Essentials.GOAP.DefaultModules {

    [SerializedReferenceCaption("Default/Perform while not")]
    public class PerformWhileNot : GOAPActionModule {

        public override ActionEvent requiredEvents => ActionEvent.IsDone;

        [ComponentDataTypeAttribute(ComponentDataTypeAttribute.Type.NoData)]
        [Tooltip("This action will perform while entity will not match this filter")]
        public FilterDataTypes performWhileNot;
        
        public override bool IsDone(in Entity agent) {

            return this.performWhileNot.Has(in agent);

        }

    }

}