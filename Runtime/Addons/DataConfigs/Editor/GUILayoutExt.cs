namespace ME.ECSEditor.DataConfigs {
    
    using UnityEditor;
    using UnityEngine;

    public class GUILayoutExt {
        
	    public static void DrawManageDataConfigTemplateMenu(System.Collections.Generic.HashSet<ME.ECS.DataConfigs.DataConfigTemplate> usedComponents, System.Action<ME.ECS.DataConfigs.DataConfigTemplate, bool> onAdd) {
		    
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fontSize = 12;
            style.fixedWidth = 230;
            style.fixedHeight = 23;
 
            var rect = GUILayoutUtility.GetLastRect();
 
            if (GUILayout.Button("Manage Templates", style)) {
                
                rect.y += 26f;
                rect.x += rect.width;
                rect.width = style.fixedWidth;
                
                var v2 = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
                rect.x = v2.x;
                rect.y = v2.y;
                rect.height = 320f;
                
                var popup = new Popup() {
	                title = "Components",
	                autoHeight = false,
	                screenRect = rect,
	                searchText = string.Empty,
	                separator = '.',
	                
                };
                var arr = AssetDatabase.FindAssets("t:DataConfigTemplate");
                foreach (var guid in arr) {

	                var path = AssetDatabase.GUIDToAssetPath(guid);
	                var template = AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfigTemplate>(path);
	                var isUsed = usedComponents.Contains(template);
	                var caption = template.name;

	                System.Action<PopupWindowAnim.PopupItem> onItemSelect = (item) => {
		                
		                isUsed = usedComponents.Contains(template);
		                onAdd.Invoke(template, isUsed);
		                
		                isUsed = usedComponents.Contains(template);
		                var tex = isUsed == true ? EditorStyles.toggle.onNormal.scaledBackgrounds[0] : EditorStyles.toggle.normal.scaledBackgrounds[0];
		                item.image = tex;
		                
	                };
	                
	                if (isUsed == true) popup.Item("Used." + caption, isUsed == true ? EditorStyles.toggle.onNormal.scaledBackgrounds[0] : EditorStyles.toggle.normal.scaledBackgrounds[0], onItemSelect, searchable: false);
	                popup.Item(caption, isUsed == true ? EditorStyles.toggle.onNormal.scaledBackgrounds[0] : EditorStyles.toggle.normal.scaledBackgrounds[0], onItemSelect);

                }
                popup.Show();

            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
 
	    }

    }

}