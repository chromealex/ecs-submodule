#if FIXED_POINT_MATH
using math = ME.ECS.Mathematics.math;
using float3 = ME.ECS.Mathematics.float3;
#else
using math = Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;
#endif
using UnityEngine;

namespace ME.ECS.Essentials.GOAP.DefaultModules {

    [SerializedReferenceCaption("Default/GetNearest")]
    public class GetNearest : GOAPActionModule {

        public override ActionEvent requiredEvents => ActionEvent.OnAwake | ActionEvent.CanRunPrepare | ActionEvent.PerformBegin;

        [Description("Find nearest object by this filter.")]
        public FilterDataTypes nearestFilter;
        [Tooltip("Apply this component on agent")]
        [ComponentDataType(ComponentDataTypeAttribute.Type.NoData)]
        public ComponentData<ISetTarget> applyOnAgent;
        
        private Filter filter;

        public override void OnAwake() {

            base.OnAwake();
            
            Filter.CreateFromData(this.nearestFilter).Push(ref this.filter);
            
        }

        public override bool CanRunPrepare(in Entity agent) {
            
            return this.filter.Count > 0;
            
        }

        public override void PerformBegin(in Entity agent) {
        
            base.PerformBegin(in agent);

            // Find nearest object and set it as target
            var obj = this.GetNearestEntity(in agent);
            if (obj.IsAlive() == true) {
                
                var comp = this.applyOnAgent.component;
                comp.SetTarget(in obj);
                this.applyOnAgent.Apply(in agent);
                this.OnNearestFound(in agent, in obj);

            }

        }
        
        protected virtual void OnNearestFound(in Entity agent, in Entity obj) {}

        private Entity GetNearestEntity(in Entity agent) {

            var pos = agent.GetPosition();
            Entity nearest = default;
            var dist = sfloat.MaxValue;
            foreach (var obj in this.filter) {

                var d = math.distancesq(obj.GetPosition(), pos);
                if (d < dist) {

                    nearest = obj;
                    dist = d;

                }

            }

            return nearest;

        }

    }

}