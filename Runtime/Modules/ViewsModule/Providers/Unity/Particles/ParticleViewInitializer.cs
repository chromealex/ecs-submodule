namespace ME.ECS.Views.Providers {

    public class ParticleViewInitializer : SceneViewInitializer {

        public ParticleViewSourceBase view;
        
        protected override void OnInitialize(World world, in Entity entity) {
            
            if (this.view != null) {
                var viewId = world.RegisterViewSource(this.view);
                entity.AssignView(viewId, this.destroyViewBehaviour);
            }
            
        }

        public override void OnValidate() {
            
            base.OnValidate();

            if (this.view == null) this.view = this.GetComponent<ParticleViewSourceBase>();

        }

    }

}