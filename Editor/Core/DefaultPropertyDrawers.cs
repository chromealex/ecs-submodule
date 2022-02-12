using System.Collections;
using System.Collections.Generic;
using ME.ECSEditor;
using UnityEditor.UIElements;
using UnityEngine;

[UnityEditor.CustomPropertyDrawer(typeof(Quaternion))]
public class RotationPropertyDrawer : UnityEditor.PropertyDrawer {

    public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

        var rot = property;
        var euler = rot.quaternionValue.eulerAngles;
        euler = UnityEditor.EditorGUI.Vector3Field(position, "Rotation", euler);
        rot.quaternionValue = Quaternion.Euler(euler);

    }

}
