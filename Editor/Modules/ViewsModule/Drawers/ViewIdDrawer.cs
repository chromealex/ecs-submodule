using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ME.ECSEditor {
    
    using UnityEditor;

    [CustomPropertyDrawer(typeof(ME.ECS.ViewId<>))]
    public class ViewIdDrawer : PropertyDrawer {

        public override UnityEngine.UIElements.VisualElement CreatePropertyGUI(SerializedProperty property) {

            var id = property.FindPropertyRelative("id");
            var element = new UnityEngine.UIElements.VisualElement();
            var feature = ME.ECS.ViewComponentsInitializer.GetFeature();
            if (feature != null) {
                
                var moveAttr = this.fieldInfo.GetCustomAttribute<ME.ECS.ViewIdReadFromAttribute>();
                if (moveAttr != null) {

                    var fieldName = moveAttr.fieldName;
                    var config = property.GetSerializedValue<ME.ECS.Views.ViewBase>(fieldName);
                    if (config != null) {
                        
                        property.serializedObject.Update();
                        var newFile = config;
                        id.intValue = feature.AddOrGet(newFile);
                        property.serializedObject.ApplyModifiedProperties();
                        
                    }

                }
                
                var file = feature.Get(id.intValue);
                var propertyField = new UnityEditor.UIElements.ObjectField();
                propertyField.objectType = typeof(ME.ECS.Views.ViewBase);
                propertyField.allowSceneObjects = false;
                propertyField.value = file;
                propertyField.label = property.displayName;
                propertyField.RegisterCallback<UnityEngine.UIElements.ChangeEvent<Object>>(evt => {

                    property.serializedObject.Update();
                    var newFile = (ME.ECS.Views.ViewBase)evt.newValue;
                    id.intValue = feature.AddOrGet(newFile);
                    property.serializedObject.ApplyModifiedProperties();

                });
                element.Add(propertyField);
            } else {
                var propertyField = new UnityEditor.UIElements.PropertyField(id);
                element.Add(propertyField);
            }
            return element;
            
        }

    }

}