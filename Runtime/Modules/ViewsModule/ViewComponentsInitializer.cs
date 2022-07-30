namespace ME.ECS {

    public class ViewIdReadFromAttribute : UnityEngine.PropertyAttribute {

        public string fieldName;
        
        public ViewIdReadFromAttribute(string fieldName) {

            this.fieldName = fieldName;

        }

    }

    [System.Serializable]
    public struct ViewId<T> where T : ME.ECS.Views.ViewBase {

        [UnityEngine.SerializeField]
        [ME.ECS.Serializer.SerializeField]
        internal int id;

        public ViewId(T config) {
            
            var module = Worlds.current.GetFeature<ME.ECS.Views.Features.ViewIndexerFeature>();
            this = module.RegisterConfig(config);

        }

        public ViewId(int id) {

            this.id = id;

        }

        public T GetData() {

            var module = Worlds.current.GetFeature<ME.ECS.Views.Features.ViewIndexerFeature>();
            return module.GetData(this);

        }
        
        public static implicit operator ME.ECS.ViewId<T>(T config) {
            return new ME.ECS.ViewId<T>(config);
        }

        public static implicit operator T(ME.ECS.ViewId<T> config) {
            return config.GetData();
        }

    }

    public static class ViewComponentsConstants {

        public const string FILE_NAME = "ME.ECS.ViewIndexer";

    }

    public class ViewComponentsInitializer {

        public static void InitTypeId() {
            
            WorldUtilities.InitComponentTypeId<ME.ECS.Views.ViewComponent>(false, isBlittable: true);
            
        }

        public static ME.ECS.Views.Features.ViewIndexerFeature GetFeature() {
            
            try {
                
                return UnityEngine.Resources.Load<ME.ECS.Views.Features.ViewIndexerFeature>(ViewComponentsConstants.FILE_NAME);
                
            } catch (System.Exception) {

                return null;

            }

        }

        public static void Init(State state) {
    
            state.structComponents.ValidateUnmanaged<ME.ECS.Views.ViewComponent>(ref state.allocator, false);
            #if !STATIC_API_DISABLED
            var feature = ViewComponentsInitializer.GetFeature();
            if (feature == null) {

                E.FILE_NOT_FOUND($"Feature `ViewIndexerFeature` not found. Create it at path `Resources/{ViewComponentsConstants.FILE_NAME}`.");

            }
            Worlds.current.AddFeature(feature, true);
            #endif

        }
    
        public static void Init(in Entity entity) {

            entity.ValidateDataUnmanaged<ME.ECS.Views.ViewComponent>(false);
            
        }

    }

}