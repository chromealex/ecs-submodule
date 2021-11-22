
namespace ME.ECS.BlackBox {

    [Category("Conditions")]
    public class FilterCondition : Box {

        public override float width => 200f;

        public FilterDataTypes condition;
        
        private Filter conditionFilter;
        [BoxLink("If True")]
        public Box nextTrue;
        [BoxLink("If False")]
        public Box nextFalse;

        public override void OnCreate() {

            Filter.CreateFromData(this.condition).Push();

        }
        
        public override Box Execute(in Entity entity, float deltaTime) {

            if (this.conditionFilter.Contains(in entity) == true) {

                return this.nextTrue;

            }

            return this.nextFalse;

        }

    }

}