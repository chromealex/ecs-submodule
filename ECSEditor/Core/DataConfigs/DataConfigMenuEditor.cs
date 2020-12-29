using UnityEditor;

namespace ME.ECSEditor {

    public static class DataConfigUtils {

        [MenuItem("ME.ECS/Data Configs/Force Update")]
        public static void ForceUpdateConfigs() {
            
            AssetUtils.ForEachAsset<ME.ECS.DataConfigs.DataConfig>((path, config) => {
                config.OnValidate();
            });

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
        }
        
    }
    
}