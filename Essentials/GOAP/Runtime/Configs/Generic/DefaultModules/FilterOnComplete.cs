#if FIXED_POINT_MATH
using math = ME.ECS.Mathematics.math;
using float3 = ME.ECS.Mathematics.float3;
#else
using math = Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;
#endif
using UnityEngine;

namespace ME.ECS.Essentials.GOAP.DefaultModules {

    [SerializedReferenceCaption("Default/Apply on Agent (On Complete)")]
    public class FilterOnComplete : GOAPActionModule {

        public override ActionEvent requiredEvents => ActionEvent.PerformComplete;

        [ComponentDataTypeAttribute(ComponentDataTypeAttribute.Type.WithData)]
        [FilterDataTypesLabelsAttribute("Add", "Remove")]
        public FilterDataTypes onComplete;

        public override void OnComplete(in Entity agent) {
        
            base.OnComplete(in agent);

            this.onComplete.Apply(in agent);

        }

    }

}