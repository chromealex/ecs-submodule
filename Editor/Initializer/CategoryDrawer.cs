using UnityEngine;
using ME.ECS;
using UnityEditor;

namespace ME.ECSEditor {

    [CustomPropertyDrawer(typeof(ME.ECS.FeaturesListCategories))]
    public class CategoriesDrawer : PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            
            var items = property.FindPropertyRelative("items");
            var h = EditorUtilities.GetPropertyHeight(items, true, new GUIContent("Features"));
            return h;
            
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
            var h = 0f;//EditorUtilities.GetPropertyHeight(header, true, new GUIContent("Caption")) + 4f;
            if (items.arraySize == 0) {
                if (items.isExpanded == true) {
                    return 80f + 20f;
                } else {
                    return 40f;
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
            EditorGUI.PropertyField(position, items, new GUIContent(header.stringValue), true);

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

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            
            //var enabled = property.FindPropertyRelative("enabled");
            var items = property.FindPropertyRelative("feature");
            var innerFeatures = property.FindPropertyRelative("innerFeatures");
            var h = EditorUtilities.GetPropertyHeight(items, true, GUIContent.none);
            if (items.objectReferenceValue is FeatureBase featureBase && featureBase.editorComment.Length > 0) {

                var width = EditorGUIUtility.currentViewWidth;
                h += EditorStyles.miniLabel.CalcHeight(new GUIContent(featureBase.editorComment), width);
                
            }

            h += 4f * 2f;

            //h += EditorUtilities.GetPropertyHeight(innerFeatures, true, new GUIContent("Sub Features"));
            
            return h;
            
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            
            position.y += 4f;

            var enabled = property.FindPropertyRelative("enabled");
            var items = property.FindPropertyRelative("feature");
            var innerFeatures = property.FindPropertyRelative("innerFeatures");
            var enabledRect = position;
            enabledRect.height = EditorGUIUtility.singleLineHeight;
            enabledRect.width = 30f;
            enabled.boolValue = EditorGUI.ToggleLeft(enabledRect, GUIContent.none, enabled.boolValue);

            var featureRect = position;
            featureRect.height = EditorGUIUtility.singleLineHeight;
            featureRect.x += 30f;
            featureRect.width -= 30f;
            
            EditorGUI.BeginDisabledGroup(enabled.boolValue == false);
            EditorGUI.PropertyField(featureRect, items, GUIContent.none, true);
            EditorGUI.EndDisabledGroup();

            position.y += featureRect.height;
            position.height -= featureRect.height;
            
            if (items.objectReferenceValue is FeatureBase featureBase && featureBase.editorComment.Length > 0) {
                
                var width = EditorGUIUtility.currentViewWidth;
                var h = EditorStyles.miniLabel.CalcHeight(new GUIContent(featureBase.editorComment), width);
                var commentRect = position;
                commentRect.height = h;
                EditorGUI.LabelField(commentRect, featureBase.editorComment, EditorStyles.miniLabel);

                position.y += commentRect.height;
                position.height -= commentRect.height;

            }

            //EditorGUI.PropertyField(position, innerFeatures, new GUIContent("Sub Features"), true);

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
