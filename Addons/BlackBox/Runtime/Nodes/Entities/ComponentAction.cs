
namespace ME.ECS.BlackBox {

    [Category("Entity")]
    public class ComponentAction : BoxNode {

        public enum Action {

            Set,
            Remove,

        }

        public Action action;
        public ComponentData component;
        public FieldDataContainer data;
        private int registryIndex;
        private int componentIndex;
        
        public override void OnValidateEditor() {

            if (this.component.component == null) {
                
                this.data = new FieldDataContainer();
                
            } else {

                this.data.isInput = true;
                this.data.Validate(this.component.component);
                
            }

        }

        public override void OnCreate() {

            ComponentTypesRegistry.allTypeId.TryGetValue(this.component.component.GetType(), out this.registryIndex);
            ComponentTypesRegistry.typeId.TryGetValue(this.component.component.GetType(), out this.componentIndex);

        }
        
        public override Box Execute(in Entity entity, float deltaTime) {

            if (this.action == Action.Set) {

                this.data.Apply(ref this.component.component);
                Worlds.currentWorld.SetData(in entity, this.component.component, this.registryIndex, this.componentIndex);

            } else if (this.action == Action.Remove) {
                
                Worlds.currentWorld.RemoveData(in entity, this.registryIndex);

            }

            return this.next;
            
        }

    }

}
