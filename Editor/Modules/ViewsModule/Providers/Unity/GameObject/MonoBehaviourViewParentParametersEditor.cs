//#if GAMEOBJECT_VIEWS_MODULE_SUPPORT
using UnityEditor;
using ME.ECS.Views.Providers;
using ME.ECS;

namespace ME.ECSEditor {

    [UnityEditor.CustomPropertyDrawer(typeof(MonoBehaviourViewBase.ParentParameters.Item))]
    public class MonoBehaviourViewParentParametersRootEditor : PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, UnityEngine.GUIContent label) {

            return EditorGUIUtility.singleLineHeight;

        }

        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label) {
            
            var rect = position;
            var parentId = property.FindPropertyRelative("parentId");
            var tr = property.FindPropertyRelative("transform");

            var space = 10f;
            const float w1 = 60f;
            const float w2 = 40f;
            var fullWidth = rect.width - space;
            rect.width = fullWidth * 0.3f;
            {
                var fWidth = rect.width;
                rect.width = w1;
                EditorGUI.LabelField(rect, "Parent Id");
                rect.x += rect.width;
                rect.width = fWidth - w1;
                EditorGUI.PropertyField(rect, parentId, new UnityEngine.GUIContent(), true);
            }
            rect.x += space;
            rect.x += rect.width;
            rect.width = fullWidth * 0.7f;
            {
                var fWidth = rect.width;
                rect.width = w2;
                EditorGUI.LabelField(rect, "Root");
                rect.x += rect.width;
                rect.width = fWidth - w2;
                EditorGUI.PropertyField(rect, tr, new UnityEngine.GUIContent(), true);
            }

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(MonoBehaviourViewBase.ParentParameters))]
    public class MonoBehaviourViewParentParametersEditor : PropertyDrawer {

        private static string TOOLTIP_PARENT_ENABLED = "Enable auto-parenting for this view.";
        private static string TOOLTIP_PARENT_ID = "Parent Id for this view.";
        private static string TOOLTIP_ROOTS = "Transform custom roots while use entity.SetParent() API.";
        
        private static System.Reflection.MethodInfo contextWidthMethodInfo;
        
        private float GetWidth() {

            if (MonoBehaviourViewParentParametersEditor.contextWidthMethodInfo == null) {

                var method = typeof(EditorGUIUtility).GetProperty("contextWidth", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetGetMethod(true);
                MonoBehaviourViewParentParametersEditor.contextWidthMethodInfo = method;

            }

            return (float)MonoBehaviourViewParentParametersEditor.contextWidthMethodInfo.Invoke(null, null) - 26f * 2f;

        }
        
        public override float GetPropertyHeight(SerializedProperty property, UnityEngine.GUIContent label) {

            var h = 0f;
            h += this.DrawTooltipTipHeight(MonoBehaviourViewParentParametersEditor.TOOLTIP_PARENT_ENABLED);
            h += this.DrawTooltipTipHeight(MonoBehaviourViewParentParametersEditor.TOOLTIP_PARENT_ID);
            h += this.DrawTooltipTipHeight(MonoBehaviourViewParentParametersEditor.TOOLTIP_ROOTS);
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
            var parentId = property.FindPropertyRelative("parentId");
            var roots = property.FindPropertyRelative("roots");
            var str = new System.Collections.Generic.List<string>();
            if (enabled.boolValue == true) {
                str.Add("Enabled");
            } else {
                str.Add("Disabled");
            }
            if (parentId.intValue != 0) str.Add("Id: " + parentId.intValue);
            if (roots.arraySize > 0) str.Add("Roots: " + roots.arraySize);
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
                    EditorGUI.PropertyField(rect, prop, true);
                    rect.y += rect.height;
                    rect = this.DrawTooltipTip(rect, MonoBehaviourViewParentParametersEditor.TOOLTIP_PARENT_ENABLED);
                    isDisabled = prop.boolValue == false;
                }

                EditorGUI.BeginDisabledGroup(isDisabled);
                {
                    var prop = property.FindPropertyRelative("parentId");
                    rect.height = EditorGUI.GetPropertyHeight(prop);
                    EditorGUI.PropertyField(rect, prop, true);
                    rect.y += rect.height;
                    rect = this.DrawTooltipTip(rect, MonoBehaviourViewParentParametersEditor.TOOLTIP_PARENT_ID);
                }
                {
                    var prop = property.FindPropertyRelative("roots");
                    rect.height = EditorGUI.GetPropertyHeight(prop);
                    EditorGUI.PropertyField(rect, prop, true);
                    rect.y += rect.height;
                    rect = this.DrawTooltipTip(rect, MonoBehaviourViewParentParametersEditor.TOOLTIP_ROOTS);
                }
                EditorGUI.EndDisabledGroup();
                --EditorGUI.indentLevel;

            }
            
        }

    }

}
//#endif