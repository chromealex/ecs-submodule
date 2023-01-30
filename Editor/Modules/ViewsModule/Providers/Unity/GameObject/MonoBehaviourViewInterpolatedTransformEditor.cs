//#if GAMEOBJECT_VIEWS_MODULE_SUPPORT
using UnityEditor;
using ME.ECS.Views.Providers;
using ME.ECS;

namespace ME.ECSEditor {

    [UnityEditor.CustomPropertyDrawer(typeof(InterpolatedTransform.Settings))]
    public class MonoBehaviourViewInterpolatedTransformEditor : PropertyDrawer {

        private static string TOOLTIP_MOVEMENT_SPEED = "Movement default speed.";
        private static string TOOLTIP_ROTATION_SPEED = "Rotation default speed.";
        
        private static System.Reflection.MethodInfo contextWidthMethodInfo;
        
        private float GetWidth() {

            if (MonoBehaviourViewInterpolatedTransformEditor.contextWidthMethodInfo == null) {

                var method = typeof(EditorGUIUtility).GetProperty("contextWidth", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetGetMethod(true);
                MonoBehaviourViewInterpolatedTransformEditor.contextWidthMethodInfo = method;

            }

            return (float)MonoBehaviourViewInterpolatedTransformEditor.contextWidthMethodInfo.Invoke(null, null) - 26f * 2f;

        }
        
        public override float GetPropertyHeight(SerializedProperty property, UnityEngine.GUIContent label) {

            var h = 0f;
            h += this.DrawTooltipTipHeight(MonoBehaviourViewInterpolatedTransformEditor.TOOLTIP_MOVEMENT_SPEED);
            h += this.DrawTooltipTipHeight(MonoBehaviourViewInterpolatedTransformEditor.TOOLTIP_ROTATION_SPEED);
            if (property.isExpanded == true) {
            
                return EditorGUI.GetPropertyHeight(property, true) + h;

            }

            return EditorGUI.GetPropertyHeight(property, true);

        }

        private float DrawTooltipTipHeight(string label) {
            
            var width = this.GetWidth();
            var style = EditorStyles.miniLabel;
            style.wordWrap = true;
            style.normal.textColor = new UnityEngine.Color(1f, 1f, 1f, 0.4f);
            return style.CalcHeight(new UnityEngine.GUIContent(label), width);
            
        }

        private UnityEngine.Rect DrawTooltipTip(UnityEngine.Rect rect, string label) {

            var width = this.GetWidth();
            var style = EditorStyles.miniLabel;
            style.normal.textColor = new UnityEngine.Color(1f, 1f, 1f, 0.4f);
            style.wordWrap = true;
            rect.height = style.CalcHeight(new UnityEngine.GUIContent(label), width);
            EditorGUI.LabelField(rect, label, style);
            rect.y += rect.height;
            
            return rect;

        }

        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label) {
            
            var rect = position;
            
            var enabled = property.FindPropertyRelative("enabled");
            var str = new System.Collections.Generic.List<string>();
            if (enabled.boolValue == true) {
                str.Add("Enabled");
            } else {
                str.Add("Disabled");
            }
            if (str.Count > 0) label.text += " (" + string.Join(", ", str) + ")";

            rect.height = EditorGUI.GetPropertyHeight(property, false);
            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label, true, EditorStyles.foldout);
            rect.y += rect.height;
            rect.y += 4f;
            if (property.isExpanded == true) {

                ++EditorGUI.indentLevel;
                var isDisabled = true;
                {
                    var prop = property.FindPropertyRelative("enabled");
                    rect.height = EditorGUI.GetPropertyHeight(prop);
                    EditorGUI.PropertyField(rect, prop);
                    rect.y += rect.height;
                    isDisabled = prop.boolValue == false;
                }

                EditorGUI.BeginDisabledGroup(isDisabled);
                {
                    var prop = property.FindPropertyRelative("movementSpeed");
                    rect.height = EditorGUI.GetPropertyHeight(prop);
                    EditorGUI.PropertyField(rect, prop);
                    rect.y += rect.height;
                    rect = this.DrawTooltipTip(rect, MonoBehaviourViewInterpolatedTransformEditor.TOOLTIP_MOVEMENT_SPEED);
                }
                
                {
                    var prop = property.FindPropertyRelative("rotationSpeed");
                    rect.height = EditorGUI.GetPropertyHeight(prop);
                    EditorGUI.PropertyField(rect, prop);
                    rect.y += rect.height;
                    rect = this.DrawTooltipTip(rect, MonoBehaviourViewInterpolatedTransformEditor.TOOLTIP_ROTATION_SPEED);
                }
                EditorGUI.EndDisabledGroup();
                --EditorGUI.indentLevel;

            }
            
        }

    }

}
//#endif