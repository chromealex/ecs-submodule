#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ME.ECS.Essentials.GOAP {

    public static class SerializeReferenceInspectorButton {

        public static readonly Color SerializedReferenceMenuBackgroundColor = new Color(0.1f, 0.55f, 0.9f, 1f);

        /// Must be drawn before DefaultProperty in order to receive input
        public static void DrawSelectionButtonForManagedReference(this SerializedProperty property, Rect position, IEnumerable<Func<Type, bool>> filters = null) =>
            property.DrawSelectionButtonForManagedReference(position, SerializedReferenceMenuBackgroundColor, filters);

        /// Must be drawn before DefaultProperty in order to receive input
        public static void DrawSelectionButtonForManagedReference(this SerializedProperty property,
                                                                  Rect position, Color color, IEnumerable<Func<Type, bool>> filters = null) {

            var backgroundColor = color;

            var buttonPosition = position;
            buttonPosition.x += EditorGUIUtility.labelWidth + 1 * EditorGUIUtility.standardVerticalSpacing;
            buttonPosition.width = position.width - EditorGUIUtility.labelWidth - 1 * EditorGUIUtility.standardVerticalSpacing;
            buttonPosition.height = EditorGUIUtility.singleLineHeight;

            var storedIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var storedColor = GUI.backgroundColor;
            //GUI.backgroundColor = backgroundColor; 


            var type = property.GetManagedReferenceType();
            var entryName = "[Null]";
            var tooltip = string.Empty;
            if (type != null) {
                var caption = type.GetCustomAttribute<SerializedReferenceCaptionAttribute>();
                if (caption == null) {
                    var assemblyName = type.Assembly.ToString().Split('(', ',')[0];
                    entryName = type.Name + " ( " + assemblyName + " )";
                    tooltip = entryName;
                } else {
                    entryName = caption.value;
                    tooltip = type.Name;
                }
            }

            if (GUI.Button(buttonPosition, new GUIContent(entryName, tooltip), EditorStyles.popup)) property.ShowContextMenuForManagedReference(buttonPosition, filters);

            GUI.backgroundColor = storedColor;
            EditorGUI.indentLevel = storedIndent;
        }

    }

}
#endif