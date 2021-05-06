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

            const float buttonWidth = 50f;
            const float offsetRight = 30f;

            var labelRect = position;
            labelRect.x += EditorGUI.indentLevel * 14f;
            labelRect.width = EditorGUIUtility.labelWidth - labelRect.x;
            var contentRect = position;
            contentRect.x = labelRect.width + position.x + labelRect.x;
            contentRect.y -= 4f;
            contentRect.width = EditorGUIUtility.currentViewWidth - labelRect.width - buttonWidth - position.x - labelRect.x;
            var contentRectDescr = contentRect;
            contentRectDescr.y += 14f;
            var buttonRect = contentRect;
            buttonRect.x += contentRect.width - offsetRight;
            buttonRect.y += 36f - 24f;
            buttonRect.width = buttonWidth;
            buttonRect.height = 24f;
            
            GUI.Label(labelRect, label);
            if (entity.IsAliveWithBoundsCheck() == false) {

                using (new GUILayoutExt.GUIAlphaUsing(0.7f)) {
                    GUI.Label(contentRect, "Empty");
                }

                if (entity == Entity.Empty) {

                    GUI.Label(contentRectDescr, "---", EditorStyles.miniLabel);

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

                WorldsViewerEditor.SelectEntity(entity);

            }
            EditorGUI.EndDisabledGroup();

        }
        
        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var entity = property.GetSerializedValue<Entity>();
            EntityEditor.Draw(position, entity, label);
            
        }

    }

}