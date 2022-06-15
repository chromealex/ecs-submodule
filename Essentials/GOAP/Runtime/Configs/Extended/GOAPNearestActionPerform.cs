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

        public FilterDataTypes nearestFilter;
        [ComponentDataType(ComponentDataTypeAttribute.Type.NoData)]
        public ComponentData<ISetTarget> moveComponent;
        
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
                
                var comp = this.moveComponent.component;
                comp.SetTarget(in obj);
                this.moveComponent.Apply(in agent);
                
            }

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