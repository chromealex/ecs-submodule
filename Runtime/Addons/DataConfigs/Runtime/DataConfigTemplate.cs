using UnityEngine;

namespace ME.ECS.DataConfigs {

    [CreateAssetMenu(menuName = "ME.ECS/Data Config Template")]
    public class DataConfigTemplate : DataConfig {

        [TextArea]
        public string editorComment;

        public System.Collections.Generic.List<DataConfig> usedIn = new System.Collections.Generic.List<DataConfig>();

        public void Use(DataConfig config) {
            
            if (this.usedIn.Contains(config) == false) this.usedIn.Add(config);
            
        }

        public void UnUse(DataConfig config) {
            
            this.usedIn.Remove(config);
            
        }

        public void UpdateValue(int index) {
            
            foreach (var config in this.usedIn) {
                
                if (config != null) config.UpdateValue(this.structComponents[index]);
                
            }
            
        }

        public void OnAddComponent(System.Type type) {

            foreach (var config in this.usedIn) {
                
                if (config != null) config.OnAddToTemplate(this, type);
                
            }
            
        }

        public void OnRemoveComponent(System.Type type) {

            foreach (var config in this.usedIn) {
                
                if (config != null) config.OnRemoveFromTemplate(this, type);
                
            }
            
        }

        public void OnAddComponentRemoveList(System.Type type) {

            foreach (var config in this.usedIn) {
                
                if (config != null) config.OnAddToTemplateRemoveList(this, type);
                
            }
            
        }

        public void OnRemoveComponentRemoveList(System.Type type) {

            foreach (var config in this.usedIn) {
                
                if (config != null) config.OnRemoveFromTemplateRemoveList(this, type);
                
            }
            
        }

    }

}