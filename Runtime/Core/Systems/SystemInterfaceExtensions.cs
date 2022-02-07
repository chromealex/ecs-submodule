namespace ME.ECS {

    public static class SystemInterfaceExtensions {

        public static void GetFeature<T>(this ISystemFilter system, out T feature) where T : FeatureBase {
            
            system.world.GetFeature(out feature);
            
        }

        public static void GetFeature<T>(this ISystem system, out T feature) where T : FeatureBase {
            
            system.world.GetFeature(out feature);
            
        }

        public static T GetFeature<T>(this ISystemFilter system) where T : FeatureBase {
            
            return system.world.GetFeature<T>();
            
        }

        public static T GetFeature<T>(this ISystem system) where T : FeatureBase {
            
            return system.world.GetFeature<T>();
            
        }

    }

}