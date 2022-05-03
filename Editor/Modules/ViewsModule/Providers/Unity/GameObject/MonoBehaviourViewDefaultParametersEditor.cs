//#if GAMEOBJECT_VIEWS_MODULE_SUPPORT
using UnityEditor;
using ME.ECS.Views.Providers;
using ME.ECS;

namespace ME.ECSEditor {

    [UnityEditor.CustomPropertyDrawer(typeof(MonoBehaviourViewBase.DefaultParameters))]
    public class MonoBehaviourViewDefaultParametersEditor : PropertyDrawer {

        private static string TOOLTIP_DESPAWN = "Time to despawn view before it has been pooled.";
        private static string TOOLTIP_CUSTOM_VIEW_ID = "Do you want to use custom pool id?\nUseful if you want to store unique instance of the same prefab source in different storages.";
        private static string TOOLTIP_CACHE = "Do you want to use cache pooling for this view?";
        private static string TOOLTIP_CACHE_TIMEOUT = "Time to get ready this instance to be used again after it has been despawned.";
        
        private static System.Reflection.MethodInfo contextWidthMethodInfo;
        
        private float GetWidth() {

            if (MonoBehaviourViewDefaultParametersEditor.contextWidthMethodInfo == null) {

                var method = typeof(EditorGUIUtility).GetProperty("contextWidth", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetGetMethod(true);
                MonoBehaviourViewDefaultParametersEditor.contextWidthMethodInfo = method;

            }

            return (float)MonoBehaviourViewDefaultParametersEditor.contextWidthMethodInfo.Invoke(null, null) - 26f * 2f;

        }
        
        public override float GetPropertyHeight(SerializedProperty property, UnityEngine.GUIContent label) {

            var h = 0f;
            h += this.DrawTooltipTipHeight(TOOLTIP_DESPAWN);
            h += this.DrawTooltipTipHeight(TOOLTIP_CUSTOM_VIEW_ID);
            h += this.DrawTooltipTipHeight(TOOLTIP_CACHE);
            h += this.DrawTooltipTipHeight(TOOLTIP_CACHE_TIMEOUT);
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

            rect.height = EditorGUI.GetPropertyHeight(property, false);
            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label, true, EditorStyles.foldout);
            rect.y += rect.height;
            rect.y += 4f;
            if (property.isExpanded == true) {

                ++EditorGUI.indentLevel;
                var isDisabled = true;
                {
                    var prop = property.FindPropertyRelative("useDespawnTime");
                    rect.height = EditorGUI.GetPropertyHeight(prop);
                    EditorGUI.PropertyField(rect, prop);
                    rect.y += rect.height;
                    isDisabled = prop.boolValue == false;
                }

                EditorGUI.BeginDisabledGroup(isDisabled);
                {
                    var prop = property.FindPropertyRelative("despawnTime");
                    rect.height = EditorGUI.GetPropertyHeight(prop);
                    EditorGUI.PropertyField(rect, prop);
                    rect.y += rect.height;
                    rect = this.DrawTooltipTip(rect, TOOLTIP_DESPAWN);
                }
                EditorGUI.EndDisabledGroup();
                
                {
                    var prop = property.FindPropertyRelative("cacheCustomViewId");
                    rect.height = EditorGUI.GetPropertyHeight(prop);
                    EditorGUI.PropertyField(rect, prop);
                    rect.y += rect.height;
                    rect = this.DrawTooltipTip(rect, TOOLTIP_CUSTOM_VIEW_ID);
                }

                {
                    var prop = property.FindPropertyRelative("useCache");
                    rect.height = EditorGUI.GetPropertyHeight(prop);
                    EditorGUI.PropertyField(rect, prop);
                    rect.y += rect.height;
                    isDisabled = prop.boolValue == false;
                    rect = this.DrawTooltipTip(rect, TOOLTIP_CACHE);
                }

                EditorGUI.BeginDisabledGroup(isDisabled);
                var isDisabledCacheTimeout = true;
                {
                    var prop = property.FindPropertyRelative("useCacheTimeout");
                    rect.height = EditorGUI.GetPropertyHeight(prop);
                    EditorGUI.PropertyField(rect, prop);
                    rect.y += rect.height;
                    isDisabledCacheTimeout = prop.boolValue == false;
                }

                EditorGUI.BeginDisabledGroup(isDisabledCacheTimeout);
                {
                    var prop = property.FindPropertyRelative("cacheTimeout");
                    rect.height = EditorGUI.GetPropertyHeight(prop);
                    EditorGUI.PropertyField(rect, prop);
                    rect.y += rect.height;
                    rect = this.DrawTooltipTip(rect, TOOLTIP_CACHE_TIMEOUT);
                }
                EditorGUI.EndDisabledGroup();
                EditorGUI.EndDisabledGroup();
                --EditorGUI.indentLevel;

            }
            
        }

    }

}
//#endif