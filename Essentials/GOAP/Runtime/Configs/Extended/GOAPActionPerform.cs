using UnityEngine;

namespace ME.ECS.Essentials.GOAP {

    [CreateAssetMenu(menuName = "ME.ECS/Addons/GOAP/Actions/Perform")]
    public class GOAPActionPerform : GOAPAction {

        [ComponentDataTypeAttribute(ComponentDataTypeAttribute.Type.WithData)]
        [FilterDataTypesLabelsAttribute("Add", "Remove")]
        public FilterDataTypes onBegin;
        
        [ComponentDataTypeAttribute(ComponentDataTypeAttribute.Type.NoData)]
        [Tooltip("This action will perform while entity will not match this filter")]
        public FilterDataTypes performWhileNot;
        
        [ComponentDataTypeAttribute(ComponentDataTypeAttribute.Type.WithData)]
        [FilterDataTypesLabelsAttribute("Add", "Remove")]
        public FilterDataTypes onComplete;

        public override bool IsDone(in Entity agent) {

            return this.performWhileNot.Has(in agent);

        }

        public override void OnComplete(in Entity agent) {
            
            base.OnComplete(in agent);
            
            this.onComplete.Apply(in agent);
            
        }

        public override void PerformBegin(in Entity agent) {
            
            base.PerformBegin(in agent);

            this.onBegin.Apply(in agent);

        }

    }

}