using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ME.ECS.Pathfinding.Editor {

    [UnityEditor.CustomPropertyDrawer(typeof(NavMeshAgentTypeIdAttribute))]
    public class NavMeshAgentTypeIdPropertyDrawer : UnityEditor.PropertyDrawer {

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

            var index = -1;
            var count = UnityEngine.AI.NavMesh.GetSettingsCount();
            var agentTypeNames = new string[count + 2];
            for (var i = 0; i < count; i++) {
                var id = UnityEngine.AI.NavMesh.GetSettingsByIndex(i).agentTypeID;
                var name = UnityEngine.AI.NavMesh.GetSettingsNameFromID(id);
                agentTypeNames[i] = name;
                if (id == property.intValue) {
                    index = i;
                }
            }

            agentTypeNames[count] = "";
            agentTypeNames[count + 1] = "Open Agent Settings...";

            var validAgentType = index != -1;
            if (!validAgentType) {
                EditorGUILayout.HelpBox("Agent Type invalid.", MessageType.Warning);
            }

            var rect = position;
            
            EditorGUI.BeginChangeCheck();
            index = EditorGUI.Popup(rect, label.text, index, agentTypeNames);
            if (EditorGUI.EndChangeCheck()) {
                if (index >= 0 && index < count) {
                    var id = UnityEngine.AI.NavMesh.GetSettingsByIndex(index).agentTypeID;
                    property.intValue = id;
                } else if (index == count + 1) {
                    UnityEditor.AI.NavMeshEditorHelpers.OpenAgentSettings(-1);
                }
            }

        }

    }

}