
namespace ME.ECS.BlackBox {

    [Category("Entity/Views")]
    public class InstantiateDrawMeshView : BoxNode {

        public ME.ECS.Views.Providers.DrawMeshViewSourceBase viewSource;
        private ViewId viewId;
        
        public override void OnCreate() {

            this.viewId = Worlds.currentWorld.RegisterViewSource(this.viewSource);

        }
        
        public override Box Execute(in Entity entity, float deltaTime) {

            entity.InstantiateView(this.viewId);
            
            return this.next;
            
        }

    }

}