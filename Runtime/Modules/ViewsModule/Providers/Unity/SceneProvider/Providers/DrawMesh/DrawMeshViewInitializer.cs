namespace ME.ECS.Views.Providers {

    public class DrawMeshViewInitializer : SceneViewInitializerBase {

        public DrawMeshViewSourceBase view;
        
        protected override void OnInitialize(World world, in Entity entity) {
            
            if (this.view != null) {
                var viewId = world.RegisterViewSource(this.view);
                // For DrawMeshProvider there is no AssignView method
                // TODO: Make AssignView common for all providers
                entity.InstantiateView(viewId);
                this.gameObject.SetActive(false);
            }
            
        }

        public override void OnValidate() {
            
            base.OnValidate();

            if (this.view == null) this.view = this.GetComponent<DrawMeshViewSourceBase>();

        }

    }

}