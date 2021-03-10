
namespace ME.ECS.BlackBox {

    [Category("Blueprints")]
    public class RunBlueprint : BoxNode {

        public Blueprint blueprint;
        
        public override void OnCreate() {
            
            this.blueprint.OnCreate();

        }
        
        public override Box Execute(in Entity entity, float deltaTime) {

            this.blueprint.Execute(in entity, deltaTime);
            
            return this.next;
            
        }

    }

}