#if FIXED_POINT_MATH
using math = ME.ECS.Mathematics.math;
using float3 = ME.ECS.Mathematics.float3;
#else
using math = Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;
#endif
using UnityEngine;

namespace ME.ECS.Essentials.GOAP.DefaultModules {

    [SerializedReferenceCaption("Default/Apply on Agent (On Begin)")]
    public class FilterOnBegin : GOAPActionModule {

        public override ActionEvent requiredEvents => ActionEvent.PerformBegin;

        [ComponentDataTypeAttribute(ComponentDataTypeAttribute.Type.WithData)]
        [FilterDataTypesLabelsAttribute("Add", "Remove")]
        public FilterDataTypes onBegin;

        public override void PerformBegin(in Entity agent) {
        
            base.PerformBegin(in agent);

            this.onBegin.Apply(in agent);

        }

    }

}