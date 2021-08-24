using System.Linq;
using System.Reflection;
using ME.ECS.Extensions;

namespace ME.ECSEditor {

    using UnityEngine;
    using UnityEditor;
    using ME.ECS;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;

    [UnityEditor.CustomEditor(typeof(ME.ECS.DataConfigs.DataConfig), true)]
    [CanEditMultipleObjects]
    public class DataConfigEditor : Editor {

        private VisualElement rootVisualElement;
        private SerializedObject serializedObjectCopy;

        private static IMGUIContainer CreateTemplatesButton(DataConfigEditor editor, 
                                                            System.Collections.Generic.HashSet<ME.ECS.DataConfigs.DataConfigTemplate> usedComponents,
                                                            VisualElement rootElement,
                                                            VisualElement templatesContainer,
                                                            SerializedProperty source,
                                                            SerializedObject so,
                                                            System.Action<SerializedObject, ME.ECS.DataConfigs.DataConfigTemplate> onAddTemplate,
                                                            System.Action<SerializedObject, ME.ECS.DataConfigs.DataConfigTemplate> onRemoveTemplate) {

            var container = new IMGUIContainer(() => {
                
                GUILayoutExt.DrawManageDataConfigTemplateMenu(usedComponents, (template, isUsed) => {

                    var path = AssetDatabase.GetAssetPath(template);
                    var guid = AssetDatabase.AssetPathToGUID(path);
                    if (string.IsNullOrEmpty(guid) == true) return;

                    if (isUsed == true) {

                        var copy = source.Copy();
                        var i = 0;
                        var enterChildren = true;
                        while (copy.NextVisible(enterChildren) == true) {

                            enterChildren = false;

                            if (copy.propertyType != SerializedPropertyType.String) continue;
                            
                            if (copy.stringValue == guid) {

                                usedComponents.Remove(template);
                                source.DeleteArrayElementAtIndex(i);
                                so.ApplyModifiedProperties();
                                onRemoveTemplate.Invoke(so, template);
                                break;

                            }

                            ++i;

                        }
                        
                    } else {

                        usedComponents.Add(template);
                        onAddTemplate.Invoke(so, template);
                        
                        ++source.arraySize;
                        var elem = source.GetArrayElementAtIndex(source.arraySize - 1);
                        elem.stringValue = guid;
                        so.ApplyModifiedProperties();
                        
                    }

                    editor.Save();
                    BuildContainer(editor, rootElement, so);

                });
                
            });
            container.AddToClassList("add-template-menu-button-imgui");

            return container;

        }
        
        private static IMGUIContainer CreateButton(DataConfigEditor editor, System.Collections.Generic.HashSet<System.Type> usedComponents, SerializedProperty source, VisualElement elements, bool noFields) {
            
            var addMenuButton = new IMGUIContainer(() => {

                GUILayoutExt.DrawAddComponentMenu(usedComponents, (type, isUsed) => {

                    if (isUsed == false) {

                        usedComponents.Add(type);

                        ++source.arraySize;
                        var elem = source.GetArrayElementAtIndex(source.arraySize - 1);
                        elem.managedReferenceValue = System.Activator.CreateInstance(type);

                        if (noFields == true) {

                            editor.OnAddComponentFromRemoveList(type);
                            
                        } else {
                        
                            editor.OnAddComponent(type);

                        }
                        
                    } else {

                        var copy = source.Copy();
                        var i = 0;
                        var enterChildren = true;
                        while (copy.NextVisible(enterChildren) == true) {

                            enterChildren = false;
                            if (copy.propertyType != SerializedPropertyType.ManagedReference) continue;

                            GetTypeFromManagedReferenceFullTypeName(copy.managedReferenceFullTypename, out var compType);
                            if (compType == type) {

                                usedComponents.Remove(type);
                                source.DeleteArrayElementAtIndex(i);
                                
                                if (noFields == true) {

                                    editor.OnRemoveComponentFromRemoveList(type);
                            
                                } else {
                        
                                    editor.OnRemoveComponent(type);

                                }

                                break;

                            }

                            ++i;

                        }
                        
                    }

                    source.serializedObject.ApplyModifiedProperties();
                    editor.Save();
                    BuildInspectorProperties(editor, usedComponents, source, elements, noFields);

                });

            });
            addMenuButton.AddToClassList("add-component-menu-button-imgui");

            return addMenuButton;

        }

        protected virtual void OnEnable() {
            
            this.serializedObjectCopy = this.serializedObject;

        }

        public override UnityEngine.UIElements.VisualElement CreateInspectorGUI() {
            
            var container = new UnityEngine.UIElements.VisualElement();
            this.rootVisualElement = container;
            container.styleSheets.Add(EditorUtilities.Load<UnityEngine.UIElements.StyleSheet>("ECSEditor/Core/DataConfigs/styles.uss", isRequired: true));
            container.Bind(this.serializedObjectCopy);

            BuildContainer(this, container, this.serializedObjectCopy);
            
            return container;

        }

        public virtual void OnComponentMenu(GenericMenu menu, int index) { }
        public virtual void OnAddComponentFromRemoveList(System.Type type) { }
        public virtual void OnRemoveComponentFromRemoveList(System.Type type) { }
        public virtual void OnAddComponent(System.Type type) { }
        public virtual void OnRemoveComponent(System.Type type) { }

        public virtual void BuildUsedTemplates(VisualElement container, SerializedObject so) {
            
            var header = new Label("Used Templates:");
            header.AddToClassList("header");
            container.Add(header);

            var templatesContainer = new VisualElement();
            container.Add(templatesContainer);
            var source = so.FindProperty("templates");

            var usedComponents = new System.Collections.Generic.HashSet<ME.ECS.DataConfigs.DataConfigTemplate>();
            BuildTemplates(usedComponents, templatesContainer, source);
            
            container.Add(CreateTemplatesButton(this, usedComponents, container, templatesContainer, source, so, OnAddTemplate, OnRemoveTemplate));
            
        }

        public static void BuildContainer(DataConfigEditor editor, VisualElement container, SerializedObject so) {
            
            var structComponents = new VisualElement();
            var removeStructComponents = new VisualElement();

            container.Clear();

            var searchField = new ToolbarSearchField();
            searchField.AddToClassList("search-field");
            searchField.RegisterValueChangedCallback((evt) => {

                var search = evt.newValue.ToLower();
                Search(search, structComponents);
                Search(search, removeStructComponents);
                
            });
            container.Add(searchField);

            {
                var header = new Label("Components:");
                header.AddToClassList("header");
                container.Add(header);
                container.Add(structComponents);
                var usedComponents = new System.Collections.Generic.HashSet<System.Type>();
                var source = so.FindProperty("structComponents");
                BuildInspectorProperties(editor, usedComponents, source, structComponents, noFields: false);
                container.Add(CreateButton(editor, usedComponents, source, structComponents, noFields: false));
            }

            {
                var header = new Label("Remove Components:");
                header.AddToClassList("header");
                container.Add(header);
                container.Add(removeStructComponents);
                var usedComponents = new System.Collections.Generic.HashSet<System.Type>();
                var source = so.FindProperty("removeStructComponents");
                BuildInspectorProperties(editor, usedComponents, source, removeStructComponents, noFields: true);
                container.Add(CreateButton(editor, usedComponents, source, removeStructComponents, noFields: true));
            }

            editor.BuildUsedTemplates(container, so);

        }

        private static void OnAddTemplate(SerializedObject so, ME.ECS.DataConfigs.DataConfigTemplate template) {
            
            var config = so.targetObject as ME.ECS.DataConfigs.DataConfig;
            config.AddTemplate(template);
            so.UpdateIfRequiredOrScript();

        }

        private static void OnRemoveTemplate(SerializedObject so, ME.ECS.DataConfigs.DataConfigTemplate template) {
            
            var config = so.targetObject as ME.ECS.DataConfigs.DataConfig;
            var allTemplates = new System.Collections.Generic.HashSet<ME.ECS.DataConfigs.DataConfigTemplate>();
            foreach (var guid in config.templates) {

                if (string.IsNullOrEmpty(guid) == true) continue;
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(path) == true) continue;
                var temp = AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfigTemplate>(path);
                if (temp == null) continue;
                allTemplates.Add(temp);

            }
            config.RemoveTemplate(template, allTemplates);
            so.UpdateIfRequiredOrScript();

        }

        public void Save() {

            foreach (var target in this.targets) {
                
                ((ME.ECS.DataConfigs.DataConfig)target).Save();
                
            }
            
        }

        private static void BuildTemplates(System.Collections.Generic.HashSet<ME.ECS.DataConfigs.DataConfigTemplate> usedComponents, VisualElement templatesContainer, SerializedProperty source) {
            
            source = source.Copy();
            templatesContainer.Clear();
            templatesContainer.AddToClassList("templates-container");
                
            for (int i = 0; i < source.arraySize; ++i) {
                    
                var elem = source.GetArrayElementAtIndex(i);
                var guid = elem.stringValue;
                if (string.IsNullOrEmpty(guid) == true) continue;
                        
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(path) == true) continue;
                        
                var template = AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfigTemplate>(path);
                if (template == null) continue;
                    
                if (usedComponents.Contains(template) == false) usedComponents.Add(template);
                    
                var button = new Button(() => {
                        
                    EditorGUIUtility.PingObject(template);

                });
                button.AddToClassList("template-button");
                button.text = template.name;
                templatesContainer.Add(button);

            }

        }

        private static void Search(string search, VisualElement container) {
            
            var isEmpty = string.IsNullOrEmpty(search);
            var build = container.Query().Class("element").Build();
            var i = 0;
            build.ForEach((elem) => {

                var oddEven = false;
                if (isEmpty == true) {
                    elem.RemoveFromClassList("search-not-found");
                    elem.RemoveFromClassList("search-found");
                    oddEven = true;
                } else if (elem.name.ToLower().Contains(search) == true) {
                    elem.RemoveFromClassList("search-not-found");
                    elem.AddToClassList("search-found");
                    oddEven = true;
                } else {
                    elem.RemoveFromClassList("search-found");
                    elem.AddToClassList("search-not-found");
                }

                if (oddEven == true) {

                    elem.RemoveFromClassList("odd");
                    elem.RemoveFromClassList("even");
                    elem.AddToClassList(i % 2 == 0 ? "even" : "odd");

                    ++i;
                        
                }

            });

        }

        private static void BuildInspectorProperties(DataConfigEditor editor, System.Collections.Generic.HashSet<System.Type> usedComponents, SerializedProperty obj, UnityEngine.UIElements.VisualElement container, bool noFields) {

            obj = obj.Copy();
            container.Clear();
            var source = obj.Copy();
            SerializedProperty iterator = obj;
            if (iterator.NextVisible(true) == false) return;
            iterator.NextVisible(true);
            var depth = iterator.depth;
            var i = 0;
            do {

                if (iterator.propertyType != SerializedPropertyType.ManagedReference) continue;

                var element = new VisualElement();
                element.AddToClassList("element");

                GetTypeFromManagedReferenceFullTypeName(iterator.managedReferenceFullTypename, out var type);
                element.AddToClassList("element");
                element.AddToClassList(i % 2 == 0 ? "even" : "odd");
                element.RegisterCallback<UnityEngine.UIElements.ContextClickEvent, int>((evt, idx) => {
                    
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Delete"), false, () => {

                        usedComponents.Remove(type);
                        source.DeleteArrayElementAtIndex(idx);
                        source.serializedObject.ApplyModifiedProperties();
                        editor.Save();
                        BuildInspectorProperties(editor, usedComponents, source, container, noFields);

                    });
                    editor.OnComponentMenu(menu, idx);
                    menu.ShowAsContext();
                    
                }, i);
                
                if (type != null && usedComponents.Contains(type) == false) usedComponents.Add(type);
                if (type == null) {

                    var label = new UnityEngine.UIElements.Label("MISSING: " + iterator.managedReferenceFullTypename);
                    element.name = "missing";
                    label.AddToClassList("inner-element");
                    label.AddToClassList("missing-label");
                    element.Add(label);

                } else if (iterator.hasVisibleChildren == false || noFields == true) {

                    var horizontal = new UnityEngine.UIElements.VisualElement();
                    horizontal.AddToClassList("inner-element");
                    horizontal.AddToClassList("no-fields-container");
                    element.name = type.Name;
                    
                    var toggle = new UnityEngine.UIElements.Toggle();
                    toggle.AddToClassList("no-fields-toggle");
                    toggle.SetEnabled(false);
                    toggle.SetValueWithoutNotify(true);
                    horizontal.Add(toggle);

                    var label = new UnityEngine.UIElements.Label(GUILayoutExt.GetStringCamelCaseSpace(type.Name));
                    label.AddToClassList("no-fields-label");
                    horizontal.Add(label);

                    element.Add(horizontal);

                } else {
                    
                    var label = GUILayoutExt.GetStringCamelCaseSpace(type.Name);
                    if (iterator.hasVisibleChildren == true) {

                        var childs = iterator.Copy();
                        //var height = EditorGUI.GetPropertyHeight(childs, false);
                        var cnt = childs.CountInProperty();
                        if (cnt == 2/* || (height <= 22f && childs.isExpanded == false)*/) iterator.NextVisible(true);

                    }
                    
                    var propertyField = new PropertyField(iterator.Copy(), label);
                    propertyField.BindProperty(iterator.Copy());
                    propertyField.AddToClassList("property-field");
                    propertyField.AddToClassList("inner-element");
                    element.name = type.Name;
                    element.Add(propertyField);
                    
                }

                if (type != null) {
                    
                    var helps = type.GetCustomAttributes(typeof(ComponentHelpAttribute), false);
                    if (helps.Length > 0) {

                        var label = new UnityEngine.UIElements.Label(((ComponentHelpAttribute)helps[0]).comment);
                        label.AddToClassList("comment");
                        element.Add(label);
                        
                    }

                    if (typeof(IComponentStatic).IsAssignableFrom(type) == true) {
                        
                        var label = new UnityEngine.UIElements.Label("Static");
                        label.AddToClassList("static-component");
                        element.Add(label);
                        
                    }

                    if (typeof(IComponentShared).IsAssignableFrom(type) == true) {
                        
                        var label = new UnityEngine.UIElements.Label("Shared");
                        label.AddToClassList("shared-component");
                        element.Add(label);
                        
                    }

                }
                
                container.Add(element);
                ++i;

            } while (iterator.NextVisible(false) == true && depth <= iterator.depth);
            
        }

        internal static bool GetTypeFromManagedReferenceFullTypeName(string managedReferenceFullTypename, out System.Type managedReferenceInstanceType) {
            
            managedReferenceInstanceType = null;
            var parts = managedReferenceFullTypename.Split(' ');
            if (parts.Length == 2) {
                var assemblyPart = parts[0];
                var nsClassnamePart = parts[1];
                managedReferenceInstanceType = System.Type.GetType($"{nsClassnamePart}, {assemblyPart}");
            }

            return managedReferenceInstanceType != null;
            
        }
        
    }

}
