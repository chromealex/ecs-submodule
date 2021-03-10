
namespace ME.ECSEditor.BlackBox {

    using ME.ECS.BlackBox;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(BoxLinkAttribute))]
    public class BoxLinkAttributeDrawer : PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, UnityEngine.GUIContent label) {
            
            return base.GetPropertyHeight(property, label);
            
        }

        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label) {

            var attr = this.attribute as BoxLinkAttribute;
            var lbl = label;
            if (string.IsNullOrEmpty(attr.caption) == false) {
                
                lbl = new UnityEngine.GUIContent(attr.caption);
                
            }
            
            var style = new UnityEngine.GUIStyle(EditorStyles.miniBoldLabel);
            style.alignment = UnityEngine.TextAnchor.MiddleRight;
            EditorGUI.LabelField(position, lbl, style);
            if (BlackBoxContainerEditor.active != null) BlackBoxContainerEditor.active.DrawLink(property, position.x + position.width, position.y, drawIn: false);
            
        }

    }

}