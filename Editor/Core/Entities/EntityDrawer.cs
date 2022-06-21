namespace ME.ECSEditor {
    
    using ME.ECS;
    using UnityEngine;
    using UnityEditor;

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Entity))]
    public class EntityEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 36f;
            
        }

        public static float GetHeight(Entity entity, GUIContent label) {
            
            return 36f;
            
        }

        public static void Draw(Rect position, Entity entity, GUIContent label) {

            const float buttonWidth = 60f;

            var origPosition = position;
            var intendedPosition = EditorGUI.IndentedRect(position);

            position.x = intendedPosition.x;
            var labelRect = position;
            labelRect.width = EditorGUIUtility.labelWidth - EditorGUI.indentLevel * 15f - 1f;
            var contentRect = position;
            contentRect.x = labelRect.x + labelRect.width;
            contentRect.y -= 4f;
            contentRect.width = origPosition.width - labelRect.width - buttonWidth - labelRect.x;
            var contentRectDescr = contentRect;
            contentRectDescr.y += 14f;
            var buttonRect = contentRect;
            buttonRect.x = contentRect.x + contentRect.width;
            buttonRect.y += 36f - 24f;
            buttonRect.width = buttonWidth;
            buttonRect.height = 24f;
            
            GUI.Label(labelRect, label);
            if (entity.IsAliveWithBoundsCheck() == false) {

                using (new GUILayoutExt.GUIAlphaUsing(0.7f)) {
                    GUI.Label(contentRect, "Null");
                }

                if (entity == Entity.Null) {

                    GUI.Label(contentRectDescr, "\u2014", EditorStyles.miniLabel);

                } else {
                    
                    GUI.Label(contentRectDescr, entity.ToSmallString(), EditorStyles.miniLabel);

                }

            } else {
                
                var customName = (entity.IsAlive() == true ? entity.Read<ME.ECS.Name.Name>().value : string.Empty);
                using (new GUILayoutExt.GUIAlphaUsing(0.7f)) {
                    GUI.Label(contentRect, string.IsNullOrEmpty(customName) == false ? customName : "Unnamed");
                }

                GUI.Label(contentRectDescr, entity.ToSmallString(), EditorStyles.miniLabel);
                        
            }
            
            EditorGUI.BeginDisabledGroup(entity.IsAliveWithBoundsCheck() == false);
            if (GUI.Button(buttonRect, "Select") == true) {

                EditorUtilities.SelectEntity(entity);

            }
            EditorGUI.EndDisabledGroup();

        }
        
        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var entity = property.GetSerializedValue<Entity>();
            EntityEditor.Draw(position, entity, label);
            
        }

    }

}