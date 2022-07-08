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
        Entity GetTarget();

    }
    
    [CreateAssetMenu(menuName = "ME.ECS/Addons/GOAP/Actions/Perform Nearest")]
    public class GOAPNearestActionPerform : GOAPActionPerform {

        [Description("Find nearest object by this filter.")]
        public FilterDataTypesOptional nearestFilter;
        [ComponentDataTypeAttribute(ComponentDataTypeAttribute.Type.WithData)]
        [Tooltip("Apply this data on object which GOAP has found")]
        [FilterDataTypesLabelsAttribute("Add", "Remove")]
        public FilterDataTypes applyOnNearest;
        [Tooltip("Apply this component on agent")]
        [ComponentDataType(ComponentDataTypeAttribute.Type.NoData)]
        public ComponentData<ISetTarget> applyOnAgent;
        
        [ComponentDataType(ComponentDataTypeAttribute.Type.NoData)]
        public ComponentData<ISetTarget> readNearestFromAgent;
        [FilterDataTypesLabelsAttribute("Add", "Remove")]
        public FilterDataTypes applyOnNearestFromAgent;
        
        private Filter filter;

        protected override void OnAwake() {

            base.OnAwake();
            
            Filter.CreateFromData(this.nearestFilter).Push(ref this.filter);
            
        }

        public override bool CanRunPrepare(in Entity agent) {
            
            return this.filter.Count > 0;
            
        }

        public override void PerformBegin(in Entity agent) {
        
            base.PerformBegin(in agent);

            if (this.readNearestFromAgent.TryRead(in agent, out var nearestComponent) == true) {
                
                var nearestObj = nearestComponent.GetTarget();
                if (nearestObj.IsAlive() == true) {
                    
                    this.applyOnNearestFromAgent.Apply(nearestObj);
                    
                }
                
            }

            // Find nearest object and set it as target
            var obj = this.GetNearest(in agent);
            if (obj.IsAlive() == true) {
                
                var comp = this.applyOnAgent.component;
                comp.SetTarget(in obj);
                this.applyOnNearest.Apply(in obj);
                this.applyOnAgent.Apply(in agent);
                this.OnNearestFound(in agent, in obj);

            }

        }

        public virtual void OnNearestFound(in Entity agent, in Entity nearest) {}

        public override void OnComplete(in Entity agent) {
            
            base.OnComplete(in agent);
            
            // this.applyOnAgent.Remove(in agent);
            
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