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
        if (iterator.NextVisible(true)) {
            
            do {

                var field = iterator.GetField();
                if (field != null && field.GetCustomAttribute<NonReorderableAttribute>() == null) {
                    
                    // Draw default array
                    var copy = iterator.Copy();
                    
                    var subContainer = new VisualElement();
                    subContainer.AddToClassList("default-array-container");
                    container.Add(subContainer);

                    {
                        var foldout = new Foldout();
                        foldout.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
                        foldout.value = true;
                        foldout.text = copy.displayName;
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
                            var list = new UnityEditorInternal.ReorderableList(this.serializedObject, copy);
                            list.headerHeight = 0f;
                            list.drawHeaderCallback = (rect) => { };
                            var edit = list.GetType().GetField("m_IsEditable", BindingFlags.Instance | BindingFlags.NonPublic);
                            edit.SetValue(list, true);

                            var propertyField = new IMGUIContainer(() => { list.DoLayoutList(); });
                            foldout.contentContainer.Add(propertyField);
                        }
                    }

                } else {

                    var propertyField = new PropertyField(iterator.Copy()) { name = "PropertyField:" + iterator.propertyPath };
                    container.Add(propertyField);
                    
                    if (iterator.propertyPath == "m_Script" && this.serializedObject.targetObject != null) {
                        // I'm not sure about this line
                        // because I every time I want to change script - I need to use Debug Mode. Why? ;)
                        // So just change the color, but leave it enabled
                        propertyField.AddToClassList("script-field");
                        //propertyField.SetEnabled(false);
                    }

                }
                
            } while (iterator.NextVisible(false));
            
        }

        return container;
        
    }

}