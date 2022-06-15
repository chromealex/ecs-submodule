namespace ME.ECSEditor {

    using ME.ECS;
    using UnityEditor;
    using UnityEngine.UIElements;
    using UnityEditor.UIElements;
    using System.Collections.Generic;

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.ComponentData<>))]
    public class ComponentDataGenericEditor : UnityEditor.PropertyDrawer {

        private ComponentDataTypeAttribute GetAttr() {

            var attrs = this.fieldInfo.GetCustomAttributes(typeof(ComponentDataTypeAttribute), true);
            return (attrs.Length == 1 ? (ComponentDataTypeAttribute)attrs[0] : null);

        }

        public virtual System.Type GetGenericType() {

            return this.fieldInfo.FieldType.GetGenericArguments()[0];

        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property) {

            var usedComponents = new HashSet<System.Type>();
            var attr = this.GetAttr();
            var drawType = ComponentDataTypeAttribute.Type.WithData;
            if (attr != null) {

                drawType = attr.type;

            }

            var container = new VisualElement();
            container.styleSheets.Add(EditorUtilities.Load<StyleSheet>("Editor/Core/Filters/styles.uss", isRequired: true));
            container.AddToClassList("component-data-container");
            var header = new Label(property.displayName);
            header.AddToClassList("header");
            container.Add(header);

            var content = new VisualElement();
            content.AddToClassList("content");
            this.Redraw(content, property, usedComponents, drawType);
            container.Add(content);
            
            return container;

        }
        
        private void Redraw(VisualElement container, SerializedProperty property, HashSet<System.Type> usedComponents, ComponentDataTypeAttribute.Type drawType) {

            container.Clear();
            
            var name = "component";
            var data = property.FindPropertyRelative(name);
            FilterDataTypesEditor.GetTypeFromManagedReferenceFullTypeName(data.managedReferenceFullTypename, out var type);

            var compType = this.GetGenericType();
            if (type != null) {

                usedComponents.Add(type);
                
                if (drawType == ComponentDataTypeAttribute.Type.WithData) {

                    var prop = new PropertyField(data);
                    prop.Bind(data.serializedObject);
                    prop.RegisterValueChangeCallback((changed) => {
                        
                        var obj = changed.changedProperty.serializedObject;
                        if (obj.targetObject is IValidateEditor validateEditor) {

                            obj.ApplyModifiedProperties();
                            obj.Update();
                            validateEditor.OnValidateEditor();
                            EditorUtility.SetDirty(obj.targetObject);
                            obj.ApplyModifiedProperties();
                            obj.Update();

                        }

                    });
                    prop.AddToClassList("data");
                    container.Add(prop);
                    
                } else {
                    
                    var noDataLabel = new Label(type.Name);
                    noDataLabel.AddToClassList("data-label");
                    container.Add(noDataLabel);

                }
            
            } else {
            
                var noDataLabel = new Label("Component is not defined.");
                noDataLabel.AddToClassList("no-data-label");
                container.Add(noDataLabel);
                var dataType = new VisualElement();
                dataType.AddToClassList("no-data-type-container");
                {
                    var componentRequiredLabel = new Label("Required type:");
                    componentRequiredLabel.AddToClassList("no-data-type-required-label");
                    dataType.Add(componentRequiredLabel);
                    var componentRequiredType = new Label(compType.Name);
                    componentRequiredType.AddToClassList("no-data-type-required");
                    dataType.Add(componentRequiredType);
                }
                container.Add(dataType);

            }
            
            var obj = property.serializedObject;
            var button = GUILayoutExt.DrawAddComponentMenu(container, usedComponents, (addType, isUsed) => {
                
                obj.Update();
                var prop = obj.FindProperty(property.propertyPath);
                var with = prop.FindPropertyRelative(name);
                if (isUsed == true) {

                    usedComponents.Remove(addType);
                    with.managedReferenceValue = null;

                } else {

                    usedComponents.Add(addType);
                    with.managedReferenceValue = (IComponentBase)System.Activator.CreateInstance(addType);

                }

                if (obj.targetObject is IValidateEditor validateEditor) {
                    
                    obj.ApplyModifiedProperties();
                    obj.Update();
                    validateEditor.OnValidateEditor();
                    EditorUtility.SetDirty(obj.targetObject);
                    obj.ApplyModifiedProperties();
                    obj.Update();
                    
                }
                obj.ApplyModifiedProperties();
                
                this.Redraw(container, prop, usedComponents, drawType);

            }, showRuntime: true, caption: "Change Component", where: (type) => {

                return compType.IsAssignableFrom(type);

            });
            container.Add(button);
            
        }

    }

}
