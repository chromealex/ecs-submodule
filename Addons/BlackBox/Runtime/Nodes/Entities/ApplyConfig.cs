
namespace ME.ECS.BlackBox {

    [Category("Entity")]
    public class ApplyConfig : BoxNode {

        public ME.ECS.DataConfigs.DataConfig dataConfig;
        
        public override void OnCreate() {

        }
        
        public override Box Execute(in Entity entity, float deltaTime) {

            this.dataConfig.Apply(in entity);
            
            return this.next;
            
        }

    }

}