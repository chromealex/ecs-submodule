using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ME.ECS.Pathfinding.Editor {

    [UnityEditor.CustomPropertyDrawer(typeof(NavMeshAreaAttribute))]
    public class NavMeshAreaPropertyDrawer : UnityEditor.PropertyDrawer {

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

            var areaIndex = -1;
            var areaNames = GameObjectUtility.GetNavMeshAreaNames();
            for (var i = 0; i < areaNames.Length; i++) {
                var areaValue = GameObjectUtility.GetNavMeshAreaFromName(areaNames[i]);
                if (areaValue == property.intValue) areaIndex = i;
            }

            ArrayUtility.Add(ref areaNames, "");
            ArrayUtility.Add(ref areaNames, "Open Area Settings...");

            EditorGUI.BeginProperty(position, GUIContent.none, property);

            EditorGUI.BeginChangeCheck();
            areaIndex = EditorGUI.Popup(position, label.text, areaIndex, areaNames);

            if (EditorGUI.EndChangeCheck()) {
                if (areaIndex >= 0 && areaIndex < areaNames.Length - 2)
                    property.intValue = GameObjectUtility.GetNavMeshAreaFromName(areaNames[areaIndex]);
                else if (areaIndex == areaNames.Length - 1) UnityEditor.AI.NavMeshEditorHelpers.OpenAreaSettings();
            }

            EditorGUI.EndProperty();
        }

    }

}