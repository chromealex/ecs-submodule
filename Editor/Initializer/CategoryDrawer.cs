using UnityEngine;
using ME.ECS;
using UnityEditor;

namespace ME.ECSEditor {

    [CustomPropertyDrawer(typeof(ME.ECS.FeaturesListCategories))]
    public class CategoriesDrawer : PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            
            var items = property.FindPropertyRelative("items");
            var h = EditorUtilities.GetPropertyHeight(items, true, new GUIContent("Features"));
            return h + 50f;
            
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            
            var items = property.FindPropertyRelative("items");
            EditorGUI.PropertyField(position, items, new GUIContent("Features"), true);

        }

    }

    [CustomPropertyDrawer(typeof(ME.ECS.FeaturesListCategory))]
    public class CategoryDrawer : PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            
            var items = property.FindPropertyRelative("features").FindPropertyRelative("features");
            var header = property.FindPropertyRelative("folderCaption");
            var h = 20f;//EditorUtilities.GetPropertyHeight(header, true, new GUIContent("Caption")) + 4f;
            if (items.arraySize == 0) {
                if (items.isExpanded == true) {
                    h += 80f + 20f;
                    return h;
                } else {
                    h += 40f;
                    return h;
                }
            }
            h += EditorUtilities.GetPropertyHeight(items, true, new GUIContent(header.stringValue));
            h += 20f;
            return h;
            
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            
            var items = property.FindPropertyRelative("features").FindPropertyRelative("features");
            var header = property.FindPropertyRelative("folderCaption");
            var headerRect = position;
            headerRect.y += 10f;
            headerRect.height = EditorUtilities.GetPropertyHeight(header, true, new GUIContent("Caption")) + 4f;

            position.y = headerRect.y;
            position.y += 20f;
            EditorGUI.PropertyField(position, items, GUIContent.none, true);

            var captionRect = headerRect;
            captionRect.x += 4f;
            captionRect.width -= 4f * 2f;
            var backStyle = new GUIStyle("RL Header");
            if (UnityEngine.Event.current.type == UnityEngine.EventType.Repaint) backStyle.Draw(headerRect, false, false, false, false);
            header.stringValue = EditorGUI.TextField(captionRect, string.Empty, header.stringValue, EditorStyles.boldLabel);
            
        }

    }

    [CustomPropertyDrawer(typeof(FeaturesList.FeatureData))]
    public class FeatureDataDrawer : PropertyDrawer {

        private static System.Reflection.MethodInfo contextWidthMethodInfo;
        
        private float GetWidth() {

            if (FeatureDataDrawer.contextWidthMethodInfo == null) {

                var method = typeof(EditorGUIUtility).GetProperty("contextWidth", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetGetMethod(true);
                FeatureDataDrawer.contextWidthMethodInfo = method;

            }

            return (float)FeatureDataDrawer.contextWidthMethodInfo.Invoke(null, null) - 80f - 28f;

        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            
            //var enabled = property.FindPropertyRelative("enabled");
            var items = property.FindPropertyRelative("feature");
            var innerFeatures = property.FindPropertyRelative("innerFeatures");
            var h = EditorUtilities.GetPropertyHeight(items, true, GUIContent.none);
            if (items.objectReferenceValue is FeatureBase featureBase && featureBase.editorComment.Length > 0) {

                var width = this.GetWidth();
                var style = new GUIStyle(EditorStyles.miniLabel);
                style.wordWrap = true;
                style.fixedWidth = 0f;
                style.stretchWidth = false;
                h += style.CalcHeight(new GUIContent(featureBase.editorComment), width);
                
            }

            h += 4f * 2f;

            //h += EditorUtilities.GetPropertyHeight(innerFeatures, true, new GUIContent("Sub Features"));
            
            return h;
            
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

            position.y += 4f;

            var enabled = property.FindPropertyRelative("enabled");
            var items = property.FindPropertyRelative("feature");
            //var innerFeatures = property.FindPropertyRelative("innerFeatures");
            var enabledRect = position;
            enabledRect.height = EditorGUIUtility.singleLineHeight;
            enabledRect.width = 30f;
            enabled.boolValue = EditorGUI.ToggleLeft(enabledRect, GUIContent.none, enabled.boolValue);

            var featureRect = position;
            featureRect.height = EditorGUIUtility.singleLineHeight;
            featureRect.x += 30f;
            featureRect.width -= 30f;

            using (new GUILayoutExt.GUIAlphaUsing(enabled.boolValue == false ? 0.5f : 1f)) {

                EditorGUI.PropertyField(featureRect, items, GUIContent.none, true);

                var featureBase = items.objectReferenceValue as FeatureBase;
                if (featureBase != null) {

                    var offset = 0f;
                    {
                        var systems = this.GetSystems(featureBase);
                        if (systems.Count > 0) {

                            var icon = EditorUtilities.Load<Texture2D>("Editor/EditorResources/icon-system.png");
                            
                            var systemsLabelRect = featureRect;
                            var style = new GUIStyle(EditorStyles.label);
                            style.fontSize = 10;
                            systemsLabelRect.width = style.CalcSize(new GUIContent(systems.Count.ToString(), icon)).x;
                            systemsLabelRect.x += featureRect.width - systemsLabelRect.width - 18f;
                            systemsLabelRect.y += 2f;
                            systemsLabelRect.height -= 4f;

                            var tooltip = string.Empty;
                            foreach (var sys in systems) {
                                tooltip += sys.typeName + "\n";
                            }

                            offset += systemsLabelRect.width;

                            FeatureDataDrawer.DrawLabel(systemsLabelRect, new GUIContent(systems.Count.ToString(), icon, tooltip.Trim()), new Color(0.5f, 0.5f, 0.5f));

                        }
                    }
                    {
                        var modules = this.GetModules(featureBase);
                        if (modules.Count > 0) {

                            var icon = EditorUtilities.Load<Texture2D>("Editor/EditorResources/icon-module.png");

                            var systemsLabelRect = featureRect;
                            var style = new GUIStyle(EditorStyles.label);
                            style.fontSize = 10;
                            systemsLabelRect.width = style.CalcSize(new GUIContent(modules.Count.ToString(), icon)).x;
                            systemsLabelRect.x += featureRect.width - systemsLabelRect.width - 18f - offset - 1f;
                            systemsLabelRect.y += 2f;
                            systemsLabelRect.height -= 4f;

                            var tooltip = string.Empty;
                            foreach (var sys in modules) {
                                tooltip += sys.typeName + "\n";
                            }

                            FeatureDataDrawer.DrawLabel(systemsLabelRect, new GUIContent(modules.Count.ToString(), icon, tooltip.Trim()), new Color(0.5f, 0.5f, 0.8f));

                        }
                    }
                    //EditorStyles.helpBox.Draw(systemsLabelRect, "SS", false, false, false, false);

                    position.y += featureRect.height;
                    position.height -= featureRect.height;

                    if (string.IsNullOrEmpty(featureBase?.editorComment) == false) {

                        var width = this.GetWidth();
                        var style = new GUIStyle(EditorStyles.miniLabel);
                        style.wordWrap = true;
                        style.fixedWidth = 0f;
                        style.stretchWidth = false;
                        var h = style.CalcHeight(new GUIContent(featureBase.editorComment), width);
                        var commentRect = position;
                        commentRect.height = h;
                        EditorGUI.LabelField(commentRect, featureBase.editorComment, style);

                        position.y += commentRect.height;
                        position.height -= commentRect.height;

                    }

                }

            }

            //EditorGUI.PropertyField(position, innerFeatures, new GUIContent("Sub Features"), true);

        }

        public static void DrawLabel(Rect rect, GUIContent label, Color color) {

            var c = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            var style = new GUIStyle(EditorStyles.label);
            style.fontSize = 10;
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleCenter;
            GUI.Label(rect, label, style);
            GUI.color = c;

        }
        
        private static System.Collections.Generic.Dictionary<FeatureBase, System.Collections.Generic.List<InitializerEditor.ObjectInfo>> cacheSystems = new System.Collections.Generic.Dictionary<FeatureBase, System.Collections.Generic.List<InitializerEditor.ObjectInfo>>();
        private System.Collections.Generic.List<InitializerEditor.ObjectInfo> GetSystems(FeatureBase feature) {

            if (cacheSystems.TryGetValue(feature, out var list) == false) {

                list = new System.Collections.Generic.List<InitializerEditor.ObjectInfo>();
                var script = MonoScript.FromScriptableObject(feature);
                var text = script.text;

                var matches = System.Text.RegularExpressions.Regex.Matches(text, @"AddSystem\s*\<(.*?)\>");
                foreach (System.Text.RegularExpressions.Match match in matches) {

                    if (match.Groups.Count > 0) {

                        var systemType = match.Groups[1].Value;
                        var spl = systemType.Split('.');
                        systemType = spl[spl.Length - 1];
                        list.Add(new InitializerEditor.ObjectInfo() { typeName = systemType, type = System.Type.GetType(systemType) });

                    }

                }

                cacheSystems.Add(feature, list);
                
            }

            return list;

        }

        private static System.Collections.Generic.Dictionary<FeatureBase, System.Collections.Generic.List<InitializerEditor.ObjectInfo>> cacheModules = new System.Collections.Generic.Dictionary<FeatureBase, System.Collections.Generic.List<InitializerEditor.ObjectInfo>>();
        private System.Collections.Generic.List<InitializerEditor.ObjectInfo> GetModules(FeatureBase feature) {

            if (cacheModules.TryGetValue(feature, out var list) == false) {

                list = new System.Collections.Generic.List<InitializerEditor.ObjectInfo>();
                var script = MonoScript.FromScriptableObject(feature);
                var text = script.text;

                var matches = System.Text.RegularExpressions.Regex.Matches(text, @"AddModule\s*\<(.*?)\>");
                foreach (System.Text.RegularExpressions.Match match in matches) {

                    if (match.Groups.Count > 0) {

                        var systemType = match.Groups[1].Value;
                        var spl = systemType.Split('.');
                        systemType = spl[spl.Length - 1];
                        list.Add(new InitializerEditor.ObjectInfo() { typeName = systemType, type = System.Type.GetType(systemType) });

                    }

                }

                cacheModules.Add(feature, list);
                
            }

            return list;

        }

    }

    /*[CustomPropertyDrawer(typeof(FeaturesList.SubFeatures))]
    public class SubFeaturesDrawer : PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            
            var items = property.FindPropertyRelative("innerFeatures");
            var h = EditorUtilities.GetPropertyHeight(items, true, new GUIContent("Sub Features"));
            return h;
            
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            
            var items = property.FindPropertyRelative("innerFeatures");
            EditorGUI.PropertyField(position, items, new GUIContent("Sub Features"), true);

        }

    }*/

}
