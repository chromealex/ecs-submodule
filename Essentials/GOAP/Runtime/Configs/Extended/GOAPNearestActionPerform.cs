#if FIXED_POINT_MATH
using math = ME.ECS.Mathematics.math;
using float3 = ME.ECS.Mathematics.float3;
#else
using math = Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;
#endif
using UnityEngine;

namespace ME.ECS.Essentials.GOAP {

    public interface ISetTarget : IComponent {

        void SetTarget(in Entity target);

    }
    
    [CreateAssetMenu(menuName = "ME.ECS/Addons/GOAP/Actions/Perform Nearest")]
    public class GOAPNearestActionPerform : GOAPActionPerform {

        [Tooltip("Nearest object to find")]
        public FilterDataTypes nearestFilter;
        [ComponentDataTypeAttribute(ComponentDataTypeAttribute.Type.WithData)]
        [Tooltip("Apply this filter on object which GOAP has found")]
        [FilterDataTypesLabelsAttribute("Add", "Remove")]
        public FilterDataTypes applyOnNearest;
        [Tooltip("Apply this component on agent")]
        [UnityEngine.Serialization.FormerlySerializedAsAttribute("moveComponent")]
        [ComponentDataType(ComponentDataTypeAttribute.Type.NoData)]
        public ComponentData<ISetTarget> applyOnAgent;
        
        private Filter filter;

        protected override void OnAwake() {

            base.OnAwake();
            
            Filter.CreateFromData(this.nearestFilter).Push(ref this.filter);
            
        }
        
        public override void PerformBegin(in ME.ECS.Entity agent) {
        
            base.PerformBegin(in agent);

            // Find nearest object and set it as target
            var obj = this.GetNearest(in agent);
            if (obj.IsAlive() == true) {
                
                var comp = this.applyOnAgent.component;
                comp.SetTarget(in obj);
                this.applyOnNearest.Apply(in obj);
                this.applyOnAgent.Apply(in agent);
                
            }

        }

        public override void OnComplete(in Entity agent) {
            
            base.OnComplete(in agent);

            this.applyOnAgent.Remove(in agent);
            
        }

        private Entity GetNearest(in Entity agent) {

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