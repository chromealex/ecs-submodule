#if FIXED_POINT_MATH
using math = ME.ECS.Mathematics.math;
using float3 = ME.ECS.Mathematics.float3;
using tfloat = sfloat;
#else
using math = Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;
using tfloat = System.Single;
#endif
using UnityEngine;

namespace ME.ECS.Essentials.GOAP.DefaultModules {

    [SerializedReferenceCaption("Default/GetNearest")]
    public class GetNearest : GOAPActionModule {

        public override ActionEvent requiredEvents => ActionEvent.OnAwake | ActionEvent.CanRunPrepare | ActionEvent.PerformBegin | ActionEvent.GetCost;

        public bool distanceDependant;

        [Description("Find nearest object by this filter.")]
        public FilterDataTypesOptional nearestFilter;
        [Tooltip("Apply this component on agent")]
        [ComponentDataType(ComponentDataTypeAttribute.Type.NoData)]
        public ComponentData<ISetTarget> applyOnAgent;

        private Filter filter;

        public override void OnAwake() {

            base.OnAwake();
            
            Filter.CreateFromData(this.nearestFilter).Push(ref this.filter);
            
        }

        public override tfloat GetCost(in Entity agent) {

            if (this.distanceDependant == true) {
                
                var obj = this.GetNearestEntity(in agent, out var factorCost);
                if (obj.IsAlive() == true) {

                    return factorCost * base.GetCost(in agent);
                    
                }
                
            }
            
            return base.GetCost(in agent);
            
        }

        public override bool CanRunPrepare(in Entity agent) {
            
            return this.filter.Count > 0;
            
        }

        public override void PerformBegin(in Entity agent) {
        
            base.PerformBegin(in agent);

            // Find nearest object and set it as target
            var obj = this.GetNearestEntity(in agent, out _);
            if (obj.IsAlive() == true) {
                
                var comp = this.applyOnAgent.component;
                comp.SetTarget(in obj);
                this.applyOnAgent.Apply(in agent);
                this.OnNearestFound(in agent, in obj);

            }

        }
        
        protected virtual void OnNearestFound(in Entity agent, in Entity obj) {}

        private Entity GetNearestEntity(in Entity agent, out tfloat distanceFactorCost) {

            distanceFactorCost = 1f;
            var pos = agent.GetPosition();
            Entity nearest = default;
            var dist = tfloat.MaxValue;
            tfloat maxDist = 0f;
            foreach (var obj in this.filter) {

                var d = math.distancesq(obj.GetPosition(), pos);

                if (d > maxDist) {
                    maxDist = d;
                }
                
                if (d < dist) {

                    nearest = obj;
                    dist = d;

                }

            }

            if (maxDist > 0f) {
                // this is a factor distance, not return a real distance value
                distanceFactorCost = dist / maxDist;
                
            }

            return nearest;

        }

    }

}