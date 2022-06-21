namespace ME.ECS.Views.Providers {

    public abstract class SceneSourceComponent : UnityEngine.MonoBehaviour {

        public abstract void Apply(World world, in Entity entity);
        
        public virtual void OnDrawGizmos() {}

    }

}