namespace ME.ECSEditor {
    
    using ME.ECS;
    using UnityEngine;
    using UnityEditor;

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Entity))]
    public class EntityEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 24f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            const float buttonWidth = 50f;
            const float offsetRight = 30f;

            var entity = property.GetSerializedValue<Entity>();
            var labelRect = position;
            labelRect.x += EditorGUI.indentLevel * 14f;
            labelRect.width = EditorGUIUtility.labelWidth - labelRect.x;
            var contentRect = position;
            contentRect.x = labelRect.width + position.x + labelRect.x;
            contentRect.y -= 4f;
            contentRect.width = EditorGUIUtility.currentViewWidth - labelRect.width - buttonWidth - position.x - labelRect.x;
            var contentRectDescr = contentRect;
            contentRectDescr.y += 11f;
            var buttonRect = contentRect;
            buttonRect.x += contentRect.width - offsetRight;
            buttonRect.y += 4f;
            buttonRect.width = buttonWidth;
            
            GUI.Label(labelRect, label);
            if (entity == Entity.Empty) {
			            
                GUI.Label(contentRect, "Empty");
			            
            } else {
                
                var customName = (entity.IsAlive() == true ? entity.Read<ME.ECS.Name.Name>().value : string.Empty);
                GUI.Label(contentRect, string.IsNullOrEmpty(customName) == false ? customName : "Unnamed");
                GUI.Label(contentRectDescr, entity.ToSmallString(), EditorStyles.miniLabel);
                        
            }
            
            EditorGUI.BeginDisabledGroup(entity == Entity.Empty);
            if (GUI.Button(buttonRect, "Select") == true) {

                WorldsViewerEditor.SelectEntity(entity);

            }
            EditorGUI.EndDisabledGroup();
            
        }

    }

}