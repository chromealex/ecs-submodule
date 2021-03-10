
namespace ME.ECS.BlackBox {

    [Category("Entity")]
    public class ReadComponent : BoxNode {

        [ComponentDataType(ComponentDataTypeAttribute.Type.NoData)]
        public ComponentData component;
        public FieldDataContainer data;

        private int registryIndex;

        public override void OnValidateEditor() {

            if (this.component.component == null) {
                
                this.data = new FieldDataContainer();
                
            } else {

                this.data.Validate(this.component.component);
                
            }

        }
        
        public override void OnCreate() {
            
            ComponentTypesRegistry.allTypeId.TryGetValue(this.component.component.GetType(), out this.registryIndex);

        }
        
        public override Box Execute(in Entity entity, float deltaTime) {

            var data = Worlds.currentWorld.ReadData(in entity, this.registryIndex);
            this.data.Process(data);

            return this.next;
            
        }

    }

}