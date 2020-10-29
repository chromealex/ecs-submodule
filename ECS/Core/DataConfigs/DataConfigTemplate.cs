using UnityEngine;

namespace ME.ECS.DataConfigs {

    [CreateAssetMenu(menuName = "ME.ECS/Data Config Template")]
    public class DataConfigTemplate : DataConfig {

        [TextArea]
        public string editorComment;

    }

}