
namespace ME.ECS.BlackBox {

    [Category("Blueprints")]
    public class RunBlueprint : BoxNode, IBlueprintContainerOutput {

        public BlueprintInfo blueprint;
        
        public override void OnCreate() {
            
            this.blueprint.link.OnCreate();

        }
        
        public override Box Execute(in Entity entity, float deltaTime) {

            this.blueprint.link.Execute(in entity, deltaTime);
            
            return this.next;
            
        }

        public ref Blueprint.Item GetOutputItem(Box box, out UnityEngine.Vector2 position) {

            if (this.blueprint.link != null) {

                position = this.blueprint.outputPosition;
                return ref this.blueprint.link.outputItem;
                
            }

            position = default;
            return ref Blueprint.defaultItem;

        }

    }

}