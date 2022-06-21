
using System.Reflection;

namespace ME.ECSEditor {

    using ME.ECS;
    using UnityEditor;
    using UnityEngine.UIElements;
    using UnityEditor.UIElements;
    using System.Collections.Generic;
    
    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.FilterDataTypesOptional))]
    public class FilterDataTypesOptionalEditor : UnityEditor.PropertyDrawer {

        private ComponentDataTypeAttribute GetAttr() {

            var attrs = this.fieldInfo.GetCustomAttributes(typeof(ComponentDataTypeAttribute), true);
            return (attrs.Length == 1 ? (ComponentDataTypeAttribute)attrs[0] : null);

        }

        public virtual ComponentDataTypeAttribute.Type GetDefaultDrawType() {

            return ComponentDataTypeAttribute.Type.WithData;

        }

        public virtual string GetSubName() {

            return "data";

        }

        private string GetTooltip(SerializedProperty property) {
            
            #if UNITY_2021_3_OR_NEWER || UNITY_2022_1_OR_NEWER
            var text = property.tooltip;
            #else
            var text = this.fieldInfo.GetCustomAttribute<UnityEngine.TooltipAttribute>(false)?.tooltip;
            #endif

            return text;
            
        }
        
        private string GetDescription(SerializedProperty property) {
            
            var text = this.fieldInfo.GetCustomAttribute<DescriptionAttribute>(true)?.text;
            return text;
            
        }

        private bool GetFoldoutState(SerializedProperty prop) {
            var key = "FieldDataTypes.Foldouts." + prop.propertyPath;
            return EditorPrefs.GetBool(key, false);
        }

        private void SetFoldoutState(SerializedProperty prop, bool state) {
            var key = "FieldDataTypes.Foldouts." + prop.propertyPath;
            EditorPrefs.SetBool(key, state);
        }

        private void UpdateLabel(SerializedProperty property, VisualElement foldout, string withLabel, string withoutLabel) {
            
            var toggle = foldout.Q(className: "unity-foldout__toggle");
            if (toggle != null) {
                
                var label = foldout.Q(className: "foldout-description") as Label ?? new Label();
                label.text = $"{withLabel}: {property.FindPropertyRelative("with").arraySize}, {withoutLabel}: {property.FindPropertyRelative("without").arraySize}";
                label.AddToClassList("foldout-description");
                toggle.Add(label);
                
            }
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property) {

            var attr = this.GetAttr();
            var drawType = this.GetDefaultDrawType();
            if (attr != null) {

                drawType = attr.type;

            }

            var labels = this.fieldInfo.GetCustomAttribute<FilterDataTypesLabelsAttribute>();
            if (labels == null) labels = new FilterDataTypesLabelsAttribute();

            var foldoutState = this.fieldInfo.GetCustomAttribute<FilterDataTypesFoldoutAttribute>();
            if (foldoutState == null) foldoutState = new FilterDataTypesFoldoutAttribute(true);

            var container = new VisualElement();
            container.styleSheets.Add(EditorUtilities.Load<StyleSheet>("Editor/Core/Filters/styles.uss", isRequired: true));
            container.AddToClassList("filter-data-container");
            VisualElement foldoutContainer;
            if (foldoutState.foldout == true) {
                var foldout = new Foldout();
                foldout.value = this.GetFoldoutState(property);
                var copy = property.Copy();
                foldout.RegisterValueChangedCallback((evt) => { this.SetFoldoutState(copy, evt.newValue); });
                foldout.text = $"{property.displayName}";
                foldout.tooltip = this.GetTooltip(property);
                foldout.AddToClassList("header");
                foldoutContainer = foldout;
            } else {
                foldoutContainer = new VisualElement();
            }
            container.Add(foldoutContainer);

            var descr = this.GetDescription(property);
            if (string.IsNullOrEmpty(descr) == false) {
                
                var description = new Label();
                description.text = descr;
                description.AddToClassList("description");
                foldoutContainer.contentContainer.Add(description);
                
            }

            this.UpdateLabel(property, foldoutContainer, labels.include.ToUpper(), labels.exclude.ToUpper());
            
            var contentContainer = new VisualElement();
            contentContainer.AddToClassList("content");
            {
                var list = new List<System.Type>();
                var usedComponents = new HashSet<System.Type>();
                var content = new VisualElement();
                content.AddToClassList("content-include");
                this.Redraw(foldoutContainer, labels, labels.include.ToUpper(), "with", this.GetSubName(), content, property, usedComponents, list, drawType);
                contentContainer.Add(content);
            }
            {
                var list = new List<System.Type>();
                var usedComponents = new HashSet<System.Type>();
                var content = new VisualElement();
                content.AddToClassList("content-exclude");
                this.Redraw(foldoutContainer, labels, labels.exclude.ToUpper(), "without", this.GetSubName(), content, property, usedComponents, list, drawType);
                contentContainer.Add(content);
            }
            foldoutContainer.contentContainer.Add(contentContainer);

            return container;

        }
        
        private void Redraw(VisualElement foldout, FilterDataTypesLabelsAttribute labels, string caption, string name, string subName, VisualElement container, SerializedProperty property, HashSet<System.Type> usedComponents, List<System.Type> list, ComponentDataTypeAttribute.Type drawType) {

            this.UpdateLabel(property, foldout, labels.include.ToUpper(), labels.exclude.ToUpper());

            container.Clear();

            var captionLabel = new Label(caption);
            captionLabel.AddToClassList("caption-header");
            captionLabel.AddToClassList("caption-header-" + name);
            container.Add(captionLabel);
            
            var innerContainer = new VisualElement();
            innerContainer.AddToClassList("inner-container");
            container.Add(innerContainer);
            
            var compType = typeof(IComponentBase);

            var subProperty = property.FindPropertyRelative(name);
            var size = subProperty.arraySize;
            for (int i = 0; i < size; ++i) {

                var registryBase = subProperty.GetArrayElementAtIndex(i);
                var registry = registryBase;
                if (subName != null) {
                    registry = registry.FindPropertyRelative(subName);
                }

                FilterDataTypesEditor.GetTypeFromManagedReferenceFullTypeName(registry.managedReferenceFullTypename, out var type);

                var dataContainer = new VisualElement();
                dataContainer.AddToClassList("data-container");
                dataContainer.AddToClassList(i % 2 == 0 ? "odd" : "even");

                var hor = new VisualElement();
                hor.AddToClassList("data-container-layout");
                dataContainer.Add(hor);
                
                var optional = registryBase.FindPropertyRelative("optional");
                if (optional != null && type != null) {

                    var hasFields = type.GetFields().Length > 0;

                    if (hasFields == true && Unity.Collections.LowLevel.Unsafe.UnsafeUtility.IsBlittable(type) == true) {

                        var obj = property.serializedObject;
                        var toggle = new Toggle("Check Data Equals");
                        toggle.AddToClassList("toggle-equals");
                        toggle.value = optional.boolValue;
                        toggle.RegisterValueChangedCallback(evt => {
                            obj.Update();
                            var prop = obj.FindProperty(property.propertyPath);
                            optional.boolValue = evt.newValue;
                            obj.ApplyModifiedProperties();
                            this.Redraw(foldout, labels, caption, name, subName, container, prop, usedComponents, list, drawType);
                        });
                        hor.Add(toggle);
                        
                    } else if (hasFields == true) {

                        var label = new Label("Not Blittable \u24D8");
                        label.AddToClassList("toggle-equals");
                        label.tooltip = "Type is not blittable, so you cannot use `Check Data Equals` option. Make type blittable to have this option.";
                        hor.Add(label);
                        optional.boolValue = false;

                    } else {
                            
                        optional.boolValue = false;

                    }

                    if (optional.boolValue == true) {
                        drawType = ComponentDataTypeAttribute.Type.WithData;
                    } else {
                        drawType = ComponentDataTypeAttribute.Type.NoData;
                    }
                    
                }
                
                if (type != null) {

                    usedComponents.Add(type);
                    list.Add(type);

                    {
                        var label = GUILayoutExt.GetStringCamelCaseSpace(type.Name);
                        var noDataLabel = new Label(label);
                        noDataLabel.AddToClassList("data-label");
                        hor.Add(noDataLabel);
                    }

                    if (drawType == ComponentDataTypeAttribute.Type.WithData) {

                        var copy = registry.Copy();
                        var initDepth = copy.depth;
                        if (copy.NextVisible(copy.hasChildren) == true) {
                            do {

                                if (copy.depth <= initDepth) break;
                                var prop = new PropertyField(copy);
                                prop.AddToClassList("data");
                                prop.Bind(registry.serializedObject);
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
                                dataContainer.Add(prop);

                            } while (copy.NextVisible(false) == true);
                        }
                        
                    }

                } else {

                    if (string.IsNullOrEmpty(registry.managedReferenceFullTypename) == true) {
                        
                        var obj = property.serializedObject;
                        obj.Update();
                        subProperty.DeleteArrayElementAtIndex(i);
                        obj.ApplyModifiedProperties();
                        --size;
                        continue;
                        
                    }
                    
                    var noDataLabel = new Label($"Component {registry.managedReferenceFullTypename} is missing");
                    noDataLabel.AddToClassList("no-data-label");
                    hor.Add(noDataLabel);
                    
                }
                
                innerContainer.Add(dataContainer);

            }

            {
                var obj = property.serializedObject;
                var button = GUILayoutExt.DrawAddComponentMenu(container, usedComponents, (addType, isUsed) => {

                    obj.Update();
                    var prop = obj.FindProperty(property.propertyPath);
                    var with = prop.FindPropertyRelative(name);
                    if (isUsed == true) {

                        usedComponents.Remove(addType);
                        with.DeleteArrayElementAtIndex(list.IndexOf(addType));
                        list.Remove(addType);

                    } else {

                        usedComponents.Add(addType);
                        list.Add(addType);
                        ++with.arraySize;
                        var item = with.GetArrayElementAtIndex(with.arraySize - 1);
                        if (subName != null) {
                            item = item.FindPropertyRelative(subName);
                        }

                        item.managedReferenceValue = (IComponentBase)System.Activator.CreateInstance(addType);

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

                    this.Redraw(foldout, labels, caption, name, subName, container, prop, usedComponents, list, drawType);

                }, showRuntime: true, caption: "Edit Components", where: (type) => { return compType.IsAssignableFrom(type); });
                innerContainer.Add(button);
            }

        }

    }

}
