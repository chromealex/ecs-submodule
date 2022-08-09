using System.Linq;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ME.ECSEditor {

    using UnityEditor;
    using ME.ECS;
    
    public interface IEditorContainer {

        void Save();
        void OnComponentMenu(GenericMenu menu, int index);

    }

    public interface IDataConfigEditor {

        void OnRemoveComponentFromRemoveList(System.Type type);
        void OnRemoveComponent(System.Type type);

    }

    public class GUIExt {
        
        public static void BuildInspectorProperties(IEditorContainer editor, System.Collections.Generic.HashSet<System.Type> usedComponents, SerializedProperty obj,
                                                    UnityEngine.UIElements.VisualElement container, bool noFields, System.Action<int, PropertyField> onBuild = null) {

            GUIExt.BuildInspectorPropertiesElement(string.Empty, editor, usedComponents, obj, container, noFields, onBuild, true);

        }

        public static void Search(string search, VisualElement container) {
            
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

        public static void BuildInspectorPropertiesElement(string elementPath, IEditorContainer editor, System.Collections.Generic.HashSet<System.Type> usedComponents, SerializedProperty obj, UnityEngine.UIElements.VisualElement container, bool noFields, System.Action<int, UnityEditor.UIElements.PropertyField> onBuild = null, bool drawGroups = false) {

            var curGroup = string.Empty;
            obj = obj.Copy();
            container.Clear();
            var source = obj.Copy();
            SerializedProperty iterator = obj;
            if (iterator.NextVisible(true) == false) return;
            var depth = iterator.depth;
            if (iterator.NextVisible(true) == false) return;

            var props = new System.Collections.Generic.List<SerializedProperty>();
            var it = iterator.Copy();
            do {
                if (it.depth < depth) continue;
                props.Add(it.Copy());
            } while (it.NextVisible(false));
            
            if (props.Count == 0) return;

            props = props.OrderBy(x => {
                var val = x.GetValue();
                if (val == null) return 0;
                var groupAttr = val.GetType().GetCustomAttribute<ComponentGroupAttribute>(true);
                if (groupAttr != null) {

                    return groupAttr.order;

                }

                return 0;
            }).ThenBy(x => {
                
                var val = x.GetValue();
                if (val == null) return 0;
                var orderAttr = val.GetType().GetCustomAttribute<ComponentOrderAttribute>(true);
                if (orderAttr != null) {

                    return orderAttr.order;

                }

                return 0;

            }).ToList();
            
            var i = 0;
            //var iteratorNext = iterator.Copy();
            SerializedProperty iteratorNext = null;
            UnityEngine.UIElements.Foldout header = null;
            do {

                iteratorNext = props[i];
                
                if (string.IsNullOrEmpty(elementPath) == false) {

                    iterator = iteratorNext.FindPropertyRelative(elementPath);
                    if (iterator == null) iterator = iteratorNext;

                } else {

                    iterator = iteratorNext;

                }

                if (iterator.propertyType != SerializedPropertyType.ManagedReference) {
                    ++i;
                    continue;
                }

                if (drawGroups == true) {

                    var val = iterator.GetValue();
                    var groupAttr = val == null ? null : val.GetType().GetCustomAttribute<ComponentGroupAttribute>(true);
                    if (groupAttr != null) {

                        if (groupAttr.name != curGroup) {

                            curGroup = groupAttr.name;
                            var key = $"ME.ECS.Foldouts.EntityDebugComponent.Group.{curGroup}";
                            var bColor = groupAttr.color;
                            bColor.a = 0.1f;
                            // Draw header
                            header = new UnityEngine.UIElements.Foldout();
                            header.SetValueWithoutNotify(EditorPrefs.GetBool(key, false));
                            header.text = curGroup;
                            header.AddToClassList("header-group");
                            var backColor = header.style.backgroundColor;
                            backColor.value = bColor;
                            header.style.backgroundColor = backColor;
                            container.Add(header);

                            header.RegisterValueChangedCallback(evt => { EditorPrefs.SetBool(key, evt.newValue); });

                        }

                    } else {

                        header = null;

                    }

                }

                var element = new VisualElement();
                element.AddToClassList("element");

                var itCopy = iterator.Copy();
                GetTypeFromManagedReferenceFullTypeName(iterator.managedReferenceFullTypename, out var type);
                element.AddToClassList(i % 2 == 0 ? "even" : "odd");
                element.RegisterCallback<UnityEngine.UIElements.ContextClickEvent, int>((evt, idx) => {
                    
                    var menu = new GenericMenu();
                    if (usedComponents != null) {
                        
                        menu.AddItem(new UnityEngine.GUIContent("Delete"), false, () => {
                            
                            RemoveComponent((IDataConfigEditor)editor, usedComponents, source, type, noFields);
                            editor.Save();
                            BuildInspectorProperties(editor, usedComponents, source, container, noFields);

                        });

                        menu.AddItem(new UnityEngine.GUIContent("Copy JSON"), false, () => {

                            var instance = itCopy.GetValue();
                            var json = UnityEngine.JsonUtility.ToJson(instance, true);
                            EditorGUIUtility.systemCopyBuffer = json;
                            
                        });

                    }

                    editor.OnComponentMenu(menu, idx);
                    menu.ShowAsContext();
                    
                }, i);
                
                if (type != null && usedComponents?.Contains(type) == false) usedComponents?.Add(type);
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
                    #if UNITY_2021_OR_NEWER
					// Because of a bug with PropertyField label
                    if (iterator.hasVisibleChildren == true) {

                        var childs = iterator.Copy();
                        //var height = EditorUtilities.GetPropertyHeight(childs, true, new GUIContent(label));
                        var cnt = EditorUtilities.GetPropertyChildCount(childs);
                        if (cnt == 1) iterator.NextVisible(true);

                    }
                    #endif

                    var propertyField = new UnityEditor.UIElements.PropertyField(iterator.Copy(), label);
                    propertyField.BindProperty(iterator);
                    onBuild?.Invoke(i, propertyField);
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
                        element.AddToClassList("has-static-component");
                        element.Add(label);
                        
                    }

                    #if !SHARED_COMPONENTS_DISABLED
                    if (typeof(IComponentShared).IsAssignableFrom(type) == true) {
                        
                        var label = new UnityEngine.UIElements.Label("Shared");
                        label.AddToClassList("shared-component");
                        element.AddToClassList("has-shared-component");
                        element.Add(label);
                        
                    }
                    #endif

                }

                if (header == null) {

                    container.Add(element);

                } else {
                    
                    header.contentContainer.Add(element);
                    
                }

                ++i;

            } while (/*iteratorNext.NextVisible(false) == true*/i < props.Count /*&& depth <= iteratorNext.depth*/);
            
        }

        public static void RemoveComponent(IDataConfigEditor editor, System.Collections.Generic.HashSet<System.Type> usedComponents, SerializedProperty source, System.Type type, bool noFields) {
            
            var copy = source.Copy();
            var i = 0;
            var enterChildren = true;
            while (copy.NextVisible(enterChildren) == true) {

                enterChildren = false;
                if (copy.propertyType != SerializedPropertyType.ManagedReference) continue;

                GetTypeFromManagedReferenceFullTypeName(copy.managedReferenceFullTypename, out var compType);
                if (compType == type) {

                    source.serializedObject.Update();
                    usedComponents.Remove(type);
                    source.DeleteArrayElementAtIndex(i);
                    source.serializedObject.ApplyModifiedProperties();
                                
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