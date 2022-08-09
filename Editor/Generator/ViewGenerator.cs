namespace ME.ECSEditor {
    
    public class ViewGenerator {

        public static void Generate(string dir) {
            
            var feature = ME.ECS.ViewComponentsInitializer.GetFeature();
            if (feature == null) {

                dir = $"{dir}/Resources";
                var path = $"{dir}/{ME.ECS.ViewComponentsConstants.FILE_NAME}.asset";
                if (System.IO.Directory.Exists(dir) == false) System.IO.Directory.CreateDirectory(dir);
                feature = UnityEngine.ScriptableObject.CreateInstance<ME.ECS.Views.Features.ViewIndexerFeature>();
                UnityEditor.AssetDatabase.CreateAsset(feature, path);
                UnityEditor.AssetDatabase.ImportAsset(path);

                UnityEngine.Debug.Log($"ViewGenerator {path} feature created");

            }

        }

    }

}
