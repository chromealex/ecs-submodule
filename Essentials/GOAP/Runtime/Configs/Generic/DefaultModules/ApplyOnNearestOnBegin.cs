#if FIXED_POINT_MATH
using math = ME.ECS.Mathematics.math;
using float3 = ME.ECS.Mathematics.float3;
#else
using math = Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;
#endif
using UnityEngine;

namespace ME.ECS.Essentials.GOAP.DefaultModules {

    [SerializedReferenceCaption("Default/Apply on Nearest (On Begin)")]
    public class ApplyOnNearestOnBegin : GOAPActionModule {

        public override ActionEvent requiredEvents => ActionEvent.PerformBegin;

        [ComponentDataType(ComponentDataTypeAttribute.Type.NoData)]
        public ComponentData<ISetTarget> readNearestFromAgent;
        [FilterDataTypesLabelsAttribute("Add", "Remove")]
        public FilterDataTypes applyOnNearestFromAgent;
        
        public override void PerformBegin(in Entity agent) {

            base.PerformBegin(in agent);

            if (this.readNearestFromAgent.TryRead(in agent, out var nearestComponent) == true) {

                var nearestObj = nearestComponent.GetTarget();
                if (nearestObj.IsAlive() == true) {

                    this.applyOnNearestFromAgent.Apply(nearestObj);

                }

            }

        }

    }

}