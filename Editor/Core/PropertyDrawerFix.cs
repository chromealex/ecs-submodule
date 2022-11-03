#if UNITY_2020_3_OR_NEWER
#elif UNITY_2020_1_OR_NEWER
using System.Reflection;
using ME.ECSEditor;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(Object), true, isFallback = true)]
public class DefaultEditor : UnityEditor.Editor {

    public override VisualElement CreateInspectorGUI() {
        
        var container = new VisualElement();
        container.styleSheets.Add(EditorUtilities.Load<StyleSheet>("Editor/Core/default.uss", false));
        container.AddToClassList("default-container");

        var iterator = this.serializedObject.GetIterator();
        DrawFields(iterator, container);

        return container;
        
    }

    public static void DrawFields(SerializedProperty iterator, VisualElement container) {

        var so = iterator.serializedObject;
        if (iterator.NextVisible(true)) {
            
            do {

                var field = iterator.GetField();
                if ((field == null || field.FieldType != typeof(string)) &&
                    iterator.isArray == true &&
                    field != null &&
                    field.GetCustomAttribute<NonReorderableAttribute>() == null) {
                    
                    // Draw default array
                    var copy = iterator.Copy();
                    var singleType = field.FieldType.GetElementType();
                    
                    var subContainer = new VisualElement();
                    subContainer.AddToClassList("default-array-container");
                    container.Add(subContainer);

                    {
                        var foldout = new Foldout();
                        foldout.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
                        foldout.value = true;
                        foldout.text = copy.displayName;
                        var toggle = foldout.Q(className: "unity-toggle");
                        {
                            toggle.RegisterCallback<DragEnterEvent>((evt) => {
                                if (HasDragForType(DragAndDrop.objectReferences, singleType) == false) return;
                                toggle.AddToClassList("default-toggle-drag-over");
                            });
                            toggle.RegisterCallback<DragLeaveEvent>((evt) => {
                                toggle.RemoveFromClassList("default-toggle-drag-over");
                            });
                            toggle.RegisterCallback<DragUpdatedEvent>((evt) => {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                            });
                            toggle.RegisterCallback<DragPerformEvent>((evt) => {
                                toggle.RemoveFromClassList("default-toggle-drag-over");
                                if (DragAndDrop.objectReferences.Length > 0) {
                                    if (HasDragForType(DragAndDrop.objectReferences, singleType) == false) return;
                                    copy.serializedObject.Update();
                                    var cnt = copy.arraySize;
                                    copy.arraySize += DragAndDrop.objectReferences.Length;
                                    for (int i = 0; i < DragAndDrop.objectReferences.Length; ++i) {
                                        var prop = copy.GetArrayElementAtIndex(cnt + i);
                                        prop.objectReferenceValue = DragAndDrop.objectReferences[i];
                                    }

                                    copy.serializedObject.ApplyModifiedProperties();
                                }
                                DragAndDrop.AcceptDrag();
                            });
                        }
                        subContainer.Add(foldout);

                        {
                            var arraySize = new IntegerField();
                            arraySize.AddToClassList("default-array-arraysize");
                            arraySize.value = copy.arraySize;
                            var tempVal = arraySize.value;
                            arraySize.RegisterCallback<FocusOutEvent>((evt) => {

                                copy.serializedObject.Update();
                                copy.arraySize = tempVal;
                                copy.serializedObject.ApplyModifiedProperties();

                            });
                            arraySize.RegisterValueChangedCallback((evt) => { tempVal = evt.newValue; });
                            foldout.Q(className: "unity-toggle").Add(arraySize);
                        }

                        {
                            var list = new UnityEditorInternal.ReorderableList(so, copy);
                            list.headerHeight = 0f;
                            list.drawHeaderCallback = (rect) => { };
                            list.elementHeightCallback = (index) => {
                                return EditorGUI.GetPropertyHeight(copy.GetArrayElementAtIndex(index), true);
                            };
                            list.drawElementCallback = (rect, index, active, focused) => {
                                EditorGUI.PropertyField(rect, copy.GetArrayElementAtIndex(index), true);
                            };
                            var edit = list.GetType().GetField("m_IsEditable", BindingFlags.Instance | BindingFlags.NonPublic);
                            edit.SetValue(list, true);

                            var propertyField = new IMGUIContainer(() => { list.DoLayoutList(); });
                            foldout.contentContainer.Add(propertyField);
                        }
                    }

                } else {

                    var propertyField = new PropertyField(iterator.Copy()) { name = "PropertyField:" + iterator.propertyPath };
                    container.Add(propertyField);
                    
                    if (iterator.propertyPath == "m_Script" && so.targetObject != null) {
                        // I'm not sure about this line
                        // because I every time I want to change script - I need to use Debug Mode. Why? ;)
                        // So just change the color, but leave it enabled
                        propertyField.AddToClassList("script-field");
                        //propertyField.SetEnabled(false);
                    }

                }
                
            } while (iterator.NextVisible(false));
            
        }
        
    }
    
    private static bool HasDragForType(Object[] objectReferences, System.Type fieldType) {

        for (int i = 0; i < objectReferences.Length; ++i) {

            var obj = objectReferences[i];
            var type = obj.GetType();
            if (fieldType.IsAssignableFrom(type) == true) return true;

        }

        return false;

    }

}
#endif