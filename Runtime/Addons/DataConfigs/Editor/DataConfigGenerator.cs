namespace ME.ECSEditor {
    
    using ME.ECS.DataConfigs;

    [UnityEditor.InitializeOnLoadAttribute]
    public static class DataConfigGenerator {

        static DataConfigGenerator() {

            Generator.onGenerate += Generate;

        }
        
        public static void Generate(string dir) {
            
            var feature = DataConfigComponentsInitializer.GetFeature();
            if (feature == null) {

                dir = $"{dir}/Resources";
                var path = $"{dir}/{DataConfigConstants.FILE_NAME}.asset";
                if (System.IO.Directory.Exists(dir) == false) System.IO.Directory.CreateDirectory(dir);
                feature = UnityEngine.ScriptableObject.CreateInstance<DataConfigIndexerFeature>();
                UnityEditor.AssetDatabase.CreateAsset(feature, path);
                UnityEditor.AssetDatabase.ImportAsset(path);

                UnityEngine.Debug.Log($"DataConfigGenerator {path} feature created");

            }

        }

    }

}
