using System.Reflection;
using ME.ECSEditor;
using UnityEditor; 
using UnityEngine;

namespace ME.ECS.Essentials.GOAP {

    [CustomPropertyDrawer(typeof(SerializeReferenceButtonAttribute))]
    public class SerializeReferenceButtonAttributeDrawer : PropertyDrawer {

        [System.Serializable]
        private class Temp : MonoBehaviour {

            [SerializeReference]
            public object temp;

        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, false);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {


            EditorGUI.BeginProperty(position, label, property);

            var labelPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelPosition, label);

            var typeRestrictions = SerializedReferenceUIDefaultTypeRestrictions.GetAllBuiltInTypeRestrictions(fieldInfo);
            property.DrawSelectionButtonForManagedReference(position, typeRestrictions);

            //EditorGUI.PropertyField(position, property, GUIContent.none, true);

            EditorGUI.EndProperty();
        }

        public override UnityEngine.UIElements.VisualElement CreatePropertyGUI(SerializedProperty property) {
            
            var container = new UnityEngine.UIElements.VisualElement();
            var propContainer = new UnityEngine.UIElements.VisualElement();
            var original = property.Copy();
            var sourceProp = property.Copy();
            /*var label = new GUIContent(property.displayName);
            var prevType = property.managedReferenceFullTypename;
            var imgui = new UnityEngine.UIElements.IMGUIContainer(() => {
                var rect = EditorGUILayout.GetControlRect(true, this.GetPropertyHeight(property, label));
                this.OnGUI(rect, property, label);
                if (prevType != property.managedReferenceFullTypename) {
                    prevType = property.managedReferenceFullTypename;
                    EditorApplication.delayCall += () => { this.RebuildProp(original.serializedObject.FindProperty(original.propertyPath), propContainer); };
                }
            });
            container.Add(imgui);*/

            {
                var dropdown = new UnityEngine.UIElements.DropdownField();
                var types = new System.Collections.Generic.List<System.Type>();
                types.Add(null);
                dropdown.choices = new System.Collections.Generic.List<string>() {
                    "[Null]",
                };
                var appropriateTypes = property.GetAppropriateTypesForAssigningToManagedReference(null);
                foreach (var type in appropriateTypes) {
                    types.Add(type);
                    var entryName = "Unknown";
                    if (type != null) {
                        var caption = type.GetCustomAttribute<SerializedReferenceCaptionAttribute>();
                        if (caption == null) {
                            var assemblyName = type.Assembly.ToString().Split('(', ',')[0];
                            entryName = type.Name + " ( " + assemblyName + " )";
                        } else {
                            entryName = caption.value;
                        }
                    }
                    dropdown.choices.Add(entryName);
                }

                {
                    var type = property.GetManagedReferenceType();
                    var entryName = "[Null]";
                    //var tooltip = string.Empty;
                    if (type != null) {
                        var caption = type.GetCustomAttribute<SerializedReferenceCaptionAttribute>();
                        if (caption == null) {
                            var assemblyName = type.Assembly.ToString().Split('(', ',')[0];
                            entryName = type.Name + " ( " + assemblyName + " )";
                            //tooltip = entryName;
                        } else {
                            entryName = caption.value;
                            //tooltip = type.Name;
                        }
                    }
                    dropdown.index = dropdown.choices.IndexOf(entryName);
                }
                dropdown.RegisterCallback<UnityEngine.UIElements.ChangeEvent<string>>(evt => {

                    var idx = dropdown.choices.IndexOf(evt.newValue);
                    var prop = original.serializedObject.FindProperty(original.propertyPath);
                    prop.serializedObject.Update();
                    var newType = types[idx];
                    prop.managedReferenceValue = newType == null ? null : System.Activator.CreateInstance(newType);
                    prop.serializedObject.ApplyModifiedProperties();
                    this.RebuildProp(original.serializedObject.FindProperty(original.propertyPath), propContainer);
                    var objs = Selection.objects;
                    Selection.activeObject = null;
                    
                    EditorApplication.delayCall = () => {
                        EditorApplication.delayCall = () => { Selection.objects = objs; };
                    };

                });

                container.Add(dropdown);
            }
            container.Add(propContainer);

            this.RebuildProp(sourceProp, propContainer);

            return container;
            
        }

        private void RebuildProp(SerializedProperty sourceProp, UnityEngine.UIElements.VisualElement container) {
            
            container.Clear();
            var depth = sourceProp.depth;
            if (sourceProp.NextVisible(true) == true) {
                do {

                    if (sourceProp.depth <= depth) break;
                    var prop = new UnityEditor.UIElements.PropertyField(sourceProp);
                    container.Add(prop);

                } while (sourceProp.NextVisible(false) == true);
            }
            container.MarkDirtyRepaint();
            
        }

    }

}