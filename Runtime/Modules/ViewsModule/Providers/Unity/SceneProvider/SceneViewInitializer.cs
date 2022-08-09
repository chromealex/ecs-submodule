#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
#else
using Unity.Mathematics;
#endif

namespace ME.ECS.Views.Providers {

    public interface ISceneView {

        void Initialize(World world);

    }

    public static class SceneViewInitializer {

        static SceneViewInitializer() {

            InitializerBase.RegisterSceneCallback(InitializeScene);

        }
        
        private static void InitializeScene(World world, bool callLateInitialization) {
            
            var sceneEntityViews = InitializerBase.FindObjectsOfType<ME.ECS.Views.Providers.SceneViewInitializerBase>();
            var list = PoolList<ME.ECS.Views.Providers.SceneViewInitializerBase>.Spawn(sceneEntityViews.Length);
            for (int i = 0; i < sceneEntityViews.Length; ++i) {

                var view = sceneEntityViews[i];
                if (view != null) {

                    var parent = view.GetComponentsInParent<ME.ECS.Views.Providers.SceneViewInitializerBase>(true);
                    if (parent.Length <= 1) {

                        list.Add(view);

                    }

                }

            }
            for (int i = 0; i < list.Count; ++i) {

                var view = list[i];
                ((ME.ECS.Views.Providers.ISceneView)view).Initialize(world);
                
            }
            
            PoolList<ME.ECS.Views.Providers.SceneViewInitializerBase>.Recycle(ref list);

        }

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

        public void Apply(in Entity entity, in Entity rootEntity, SceneViewInitializerBase initializer, World world) {

            var transform = initializer.transform;
            switch (this.type) {
                case ApplyType._2D:
                    if (this.position == true) entity.SetLocalPosition2D(this.GetPlaneVector(transform.localPosition, this.plane2d));
                    if (this.rotation == true) entity.SetLocalRotation2D(this.GetPlaneAngle(transform.localRotation, this.plane2d));
                    if (this.scale == true) entity.SetLocalScale2D(this.GetPlaneVector(transform.localScale, this.plane2d));
                    entity.SetParent2D(in rootEntity, false);
                    break;
                case ApplyType._3D:
                    if (this.position == true) entity.SetLocalPosition((float3)transform.localPosition);
                    if (this.rotation == true) entity.SetLocalRotation((quaternion)transform.localRotation);
                    if (this.scale == true) entity.SetLocalScale((float3)transform.localScale);
                    entity.SetParent(in rootEntity, false);
                    break;
            }

            transform.SetParent(null);

            if (transform.childCount > 0) {

                var results = PoolList<SceneViewInitializerBase>.Spawn(10);
                transform.GetComponentsInChildren(false, results);
                for (int i = 0; i < results.Count; ++i) {
                    
                    var childInitializer = results[i];
                    if (childInitializer == initializer) continue;
                    
                    var parents = PoolList<SceneViewInitializerBase>.Spawn(10);
                    childInitializer.GetComponentsInParent(false, parents);
                    if (parents.Contains(initializer) == true) {
                        
                        PoolList<SceneViewInitializerBase>.Recycle(ref parents);
                        
                        // Run just for the one level
                        childInitializer.Initialize_INTERNAL(world, in entity);
                        
                    } else {
                        
                        PoolList<SceneViewInitializerBase>.Recycle(ref parents);
                        
                    }

                }
                PoolList<SceneViewInitializerBase>.Recycle(ref results);
                
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
    
    public abstract class SceneViewInitializerBase : UnityEngine.MonoBehaviour, ISceneView {

        [UnityEngine.SpaceAttribute]
        public bool applyName = true;
        public EntityFlag entityFlags;
        public ME.ECS.DataConfigs.DataConfig applyDataConfig;
        
        [UnityEngine.SpaceAttribute]
        public TransformProperties transformProperties = TransformProperties.Default;

        [UnityEngine.SpaceAttribute]
        public SceneSourceComponent[] sceneSourceComponents; 
        
        public virtual void OnValidate() {

            var list = new System.Collections.Generic.List<SceneSourceComponent>();
            var components = this.GetComponentsInChildren<SceneSourceComponent>(false);
            foreach (var component in components) {

                var initializers = component.GetComponentsInParent<SceneViewInitializerBase>(true);
                if (initializers.Length > 0 && initializers[0] == this) {
                    
                    list.Add(component);
                    
                }

            }

            this.sceneSourceComponents = list.ToArray();

        }

        void ISceneView.Initialize(World world) {

            this.Initialize_INTERNAL(world, in Entity.Null);

        }

        internal void Initialize_INTERNAL(World world, in Entity rootEntity) {

            var entity = new Entity(this.applyName == true ? this.name : null, this.entityFlags);
            if (this.applyDataConfig != null) this.applyDataConfig.Apply(entity);

            this.transformProperties.Apply(in entity, in rootEntity, this, world);

            for (int i = 0; i < this.sceneSourceComponents.Length; ++i) {

                var sceneSourceComponent = this.sceneSourceComponents[i];
                if (sceneSourceComponent != null) {

                    sceneSourceComponent.Apply(world, in entity);

                }

            }

            this.OnInitialize(world, in entity);

        }

        protected abstract void OnInitialize(World world, in Entity entity);

    }

}