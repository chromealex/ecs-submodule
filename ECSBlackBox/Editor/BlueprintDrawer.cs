
namespace ME.ECSEditor.BlackBox {

    using ME.ECS.BlackBox;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(BlueprintInfo))]
    public class BlueprintDrawer : PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, UnityEngine.GUIContent label) {
            
            var link = property.FindPropertyRelative("link");
            var h = EditorGUI.GetPropertyHeight(link);
            if (link.objectReferenceValue is Blueprint blueprint && blueprint != null && blueprint.outputItem.box is OutputVariable outputVariable && outputVariable != null) h += h;

            return h;
            
        }

        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label) {

            var link = property.FindPropertyRelative("link");
            position.height = EditorGUI.GetPropertyHeight(link);
            EditorGUI.PropertyField(position, link, label);

            if (link.objectReferenceValue is Blueprint blueprint && blueprint != null && blueprint.outputItem.box is OutputVariable outputVariable && outputVariable != null) {
                
                position.y += EditorGUI.GetPropertyHeight(link);
                
                // Blueprint has output

                var so = new SerializedObject(blueprint);
                var item = so.FindProperty("outputItem");
                var boxProp = item.FindPropertyRelative("box");

                var offset = UnityEngine.Vector2.zero;
                if (BlackBoxContainerEditor.active != null) offset = BlackBoxContainerEditor.active.scrollPosition;
                var outPos = property.FindPropertyRelative("outputPosition");
                outPos.vector2Value = new UnityEngine.Vector2(position.x - offset.x + position.width, position.y - offset.y);

                var boxSo = new SerializedObject(boxProp.objectReferenceValue);
                var propType = outputVariable.type;
                
                var style = new UnityEngine.GUIStyle(UnityEditor.EditorStyles.miniBoldLabel);
                style.alignment = UnityEngine.TextAnchor.MiddleRight;
                UnityEditor.EditorGUI.LabelField(position, "output", style);
                if (BlackBoxContainerEditor.active != null) BlackBoxContainerEditor.active.DrawLink(boxSo, position.x + position.width, position.y, drawIn: true);
                UnityEditor.EditorGUI.LabelField(new UnityEngine.Rect(position.x + position.width + 20f, position.y, position.width, 20f), propType.ToString(), UnityEditor.EditorStyles.miniLabel);

            }

        }

    }

}