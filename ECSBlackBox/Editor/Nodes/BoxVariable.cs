namespace ME.ECSEditor.BlackBox.Nodes {
    
    using UnityEditor;

    [ME.ECSEditor.BlackBox.BBCustomEditor(typeof(ME.ECS.BlackBox.BoxVariable))]
    public class BoxVariableEditor : ICustomEditor {

        public float GetHeight(SerializedObject obj) {

            var labelHeight = EditorGUIUtility.singleLineHeight;
            var prop = obj.FindProperty("caption");
            return EditorGUI.GetPropertyHeight(prop) + labelHeight;

        }

        public void OnGUI(UnityEngine.Rect rect, SerializedObject obj) {
            
            var target = obj.targetObject as ME.ECS.BlackBox.BoxVariable;
            var labelHeight = EditorGUIUtility.singleLineHeight;
            rect.height = labelHeight;
            EditorGUI.LabelField(rect, "Type: " + target.type.ToString());
            var prop = obj.FindProperty("caption");
            rect.height = EditorGUI.GetPropertyHeight(prop);
            rect.y += labelHeight;
            EditorGUI.PropertyField(rect, prop);
            
        }

    }

}