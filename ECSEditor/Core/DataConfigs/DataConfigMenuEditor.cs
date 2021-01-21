using UnityEditor;

namespace ME.ECSEditor {

    public static class DataConfigUtils {

        [MenuItem("ME.ECS/Data Configs/Force Update")]
        public static void ForceUpdateConfigs() {
            
            var paths = new System.Collections.Generic.List<string>();
            AssetUtils.ForEachAsset<ME.ECS.DataConfigs.DataConfig>((path, config) => {
                config.OnValidate();
                paths.Add(AssetDatabase.GetAssetPath(config));
            });

            AssetDatabase.ForceReserializeAssets(paths);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
        }
        
    }
    
}