using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECSEditor {
    
    using UnityEditor;

    [CustomPropertyDrawer(typeof(ME.ECS.ConfigId<>))]
    public class ConfigIdDrawer : PropertyDrawer {

        public override UnityEngine.UIElements.VisualElement CreatePropertyGUI(SerializedProperty property) {

            var id = property.FindPropertyRelative("id");
            var element = new UnityEngine.UIElements.VisualElement();
            var feature = ME.ECS.DataConfigs.DataConfigComponentsInitializer.GetFeature();
            if (feature != null) {
                
                var moveAttr = this.attribute as ME.ECS.ConfigIdReadFromAttribute;
                if (moveAttr != null) {

                    var fieldName = moveAttr.fieldName;
                    var config = property.GetSerializedValue<ME.ECS.DataConfigs.DataConfig>(fieldName);
                    if (config != null) {
                        
                        property.serializedObject.Update();
                        var newFile = (ME.ECS.DataConfigs.DataConfig)config;
                        id.intValue = feature.AddOrGet(newFile);
                        property.serializedObject.ApplyModifiedProperties();
                        
                    }

                }
                
                var file = feature.Get(id.intValue);
                var propertyField = new UnityEditor.UIElements.ObjectField();
                propertyField.objectType = typeof(ME.ECS.DataConfigs.DataConfig);
                propertyField.allowSceneObjects = false;
                propertyField.value = file;
                propertyField.label = property.displayName;
                propertyField.RegisterCallback<UnityEngine.UIElements.ChangeEvent<Object>>(evt => {

                    property.serializedObject.Update();
                    var newFile = (ME.ECS.DataConfigs.DataConfig)evt.newValue;
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