using System.Linq;

namespace ME.ECSEditor {

    using UnityEngine;
    using UnityEditor;
    using ME.ECS;

    [UnityEditor.CustomEditor(typeof(ME.ECS.DataConfigs.DataConfigTemplate), true)]
    [CanEditMultipleObjects]
    public class DataConfigTemplateEditor : DataConfigEditor {

        private SerializedProperty editorComment;
        
        public void OnEnable() {

            this.editorComment = this.serializedObject.FindProperty("editorComment");

        }

        public override void OnInspectorGUI() {
            
            EditorGUILayout.PropertyField(this.editorComment);
            
            base.OnInspectorGUI();
            
        }

    }

}