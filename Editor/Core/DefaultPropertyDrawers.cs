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

[UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Views.ViewComponent))]
public class ViewComponentPropertyDrawer : UnityEditor.PropertyDrawer {

    public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

        var viewInfoProp = property.FindPropertyRelative("viewInfo");
        var viewInfo = viewInfoProp.GetSerializedValue<ME.ECS.Views.ViewInfo>();
        if (ME.ECS.Worlds.current != null) {

            var viewsModule = ME.ECS.Worlds.current.GetModule<ME.ECS.Views.ViewsModule>();
            var viewSource = viewsModule.GetViewSource(viewInfo.prefabSourceId);
            UnityEditor.EditorGUI.BeginDisabledGroup(true);
            UnityEditor.EditorGUI.ObjectField(position, new GUIContent("View"), (Object)viewSource, typeof(Object), allowSceneObjects: false);
            UnityEditor.EditorGUI.EndDisabledGroup();

        } else {

            GUI.Label(position, "View: " + viewInfo.prefabSourceId);

        }

    }

}
