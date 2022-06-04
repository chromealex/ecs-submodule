#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
#else
using Unity.Mathematics;
#endif

namespace ME.ECS.Views.Providers {

    public interface ISceneView {

        void Initialize(World world);

    }

    [System.Serializable]
    public struct TransformProperties {

        public enum ApplyType {

            _3D,
            _2D,

        }

        public enum Plane {

            XZ,
            XY,
            YZ,

        }
        
        public static TransformProperties Default => new TransformProperties() {
            type = ApplyType._3D,
            plane2d = Plane.XZ,
            position = true,
            rotation = true,
            scale = true,
        };

        public ApplyType type;
        public Plane plane2d;
        
        public bool position;
        public bool rotation;
        public bool scale;

        public void Apply(in Entity entity, UnityEngine.Transform transform) {

            var hasParent = false;
            if (transform.parent != null) {
                
                // We have an hierarchy
                var parentInitializers = transform.GetComponentsInParent<SceneViewInitializer>();
                if (parentInitializers.Length > 1) {

                    var parentInitializer = parentInitializers[1];
                    switch (this.type) {
                        case ApplyType._2D:
                            entity.SetParent2D(parentInitializer.runtimeEntity);
                            break;
                        case ApplyType._3D:
                            entity.SetParent(parentInitializer.runtimeEntity);
                            break;
                    }

                    hasParent = true;

                }

            }

            switch (this.type) {
                case ApplyType._2D:
                    if (this.position == true) entity.SetLocalPosition2D(this.GetPlaneVector(transform.localPosition, this.plane2d));
                    if (this.rotation == true) entity.SetLocalRotation2D(this.GetPlaneAngle(transform.localRotation, this.plane2d));
                    if (this.scale == true) entity.SetLocalScale2D(this.GetPlaneVector(transform.localScale, this.plane2d));
                    break;
                case ApplyType._3D:
                    if (this.position == true) entity.SetLocalPosition((float3)transform.localPosition);
                    if (this.rotation == true) entity.SetLocalRotation((quaternion)transform.localRotation);
                    if (this.scale == true) entity.SetLocalScale((float3)transform.localScale);
                    break;
            }

            if (hasParent == true) {
                
                transform.SetParent(null);
                
            }
            
        }

        private float GetPlaneAngle(UnityEngine.Quaternion rot, Plane plane) {
            
            switch (plane) {
                case Plane.XZ: return rot.eulerAngles.y;
                case Plane.XY: return rot.eulerAngles.z;
                case Plane.YZ: return rot.eulerAngles.x;
            }

            return 0f;
            
        }

        private Unity.Mathematics.float2 GetPlaneVector(UnityEngine.Vector3 vec, Plane plane) {

            switch (plane) {
                case Plane.XZ: return vec.XZ();
                case Plane.XY: return vec.XY();
                case Plane.YZ: return vec.YZ();
            }

            return default;

        }

    }
    
    public abstract class SceneViewInitializer : UnityEngine.MonoBehaviour, ISceneView {

        [System.NonSerializedAttribute]
        public Entity runtimeEntity;
        
        [UnityEngine.SpaceAttribute]
        public EntityFlag entityFlags;
        public ME.ECS.DataConfigs.DataConfig applyDataConfig;
        
        [UnityEngine.SpaceAttribute]
        public TransformProperties transformProperties = TransformProperties.Default;
        
        [UnityEngine.SpaceAttribute]
        public DestroyViewBehaviour destroyViewBehaviour;
        
        void ISceneView.Initialize(World world) { 
            
            var entity = new Entity(this.entityFlags);
            this.runtimeEntity = entity;
            if (this.applyDataConfig != null) this.applyDataConfig.Apply(entity);

            this.transformProperties.Apply(in entity, this.transform);
            
            this.OnInitialize(world, in entity);
            
        }

        protected abstract void OnInitialize(World world, in Entity entity);

    }

}