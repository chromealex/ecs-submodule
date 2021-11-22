
using System.Linq;

namespace ME.ECSEditor {

    using ME.ECS;
    using UnityEditor;
    using UnityEngine;

    public struct ComponentDataProperty {

        public Rect position;
        public SerializedObject so;
        public string property;

    }
    
    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.ComponentData))]
    public class ComponentDataEditor : UnityEditor.PropertyDrawer {

        public readonly static System.Collections.Generic.List<ComponentDataProperty> lastDraw = new System.Collections.Generic.List<ComponentDataProperty>();

        private const float lineHeight = 22f;
        private const float editButtonHeight = 40f;

        private ComponentDataTypeAttribute GetAttr() {

            var attrs = this.fieldInfo.GetCustomAttributes(typeof(ComponentDataTypeAttribute), true);
            return (attrs.Length == 1 ? (ComponentDataTypeAttribute)attrs[0] : null);

        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {

            var drawType = ComponentDataTypeAttribute.Type.WithData;
            if (this.GetAttr() is ComponentDataTypeAttribute attr) {

                drawType = attr.type;

            }
            
            var h = 0f;
            var with = property.FindPropertyRelative("component");
            FilterDataTypesEditor.GetTypeFromManagedReferenceFullTypeName(with.managedReferenceFullTypename, out var type);
            if (type == null) {
                
                h += ComponentDataEditor.lineHeight;
                
            } else {

                h += ComponentDataEditor.lineHeight;
                if (drawType == ComponentDataTypeAttribute.Type.WithData) {

                    var initDepth = with.depth;
                    if (with.NextVisible(with.hasChildren) == true) {
                        do {

                            if (with.depth < initDepth) break;
                            h += EditorGUI.GetPropertyHeight(with, true);

                        } while (with.NextVisible(false) == true);
                    }

                } else if (drawType == ComponentDataTypeAttribute.Type.NoData) {
                    
                }

            }

            return h + editButtonHeight;
            
        }

        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label) {

            ComponentDataEditor.lastDraw.Clear();
            
            var drawType = ComponentDataTypeAttribute.Type.WithData;
            if (this.GetAttr() is ComponentDataTypeAttribute attr) {

                drawType = attr.type;

            }

            var name = "component";
            var usedComponents = new System.Collections.Generic.HashSet<System.Type>();
            {
                var backStyle = new GUIStyle(EditorStyles.label);
                backStyle.normal.background = Texture2D.whiteTexture;

                var with = property.FindPropertyRelative(name);
                FilterDataTypesEditor.GetTypeFromManagedReferenceFullTypeName(with.managedReferenceFullTypename, out var type);

                position.height = ComponentDataEditor.lineHeight;
                if (type == null) {

                    //Debug.Log("Not found: " + with.managedReferenceFullTypename + ", " + with.managedReferenceFieldTypename);
                    EditorGUI.LabelField(position, "Component is not defined");

                    position.y += ComponentDataEditor.lineHeight;

                } else {

                    usedComponents.Add(type);
                    
                    EditorGUI.LabelField(position, type.Name, EditorStyles.boldLabel);
                    position.y += ComponentDataEditor.lineHeight;

                    if (drawType == ComponentDataTypeAttribute.Type.WithData) {

                        using (new GUILayoutExt.GUIBackgroundAlphaUsing(0.5f)) {

                            var isDirty = false;
                            {
                                //var componentName = GUILayoutExt.GetStringCamelCaseSpace(type.Name);
                                var initDepth = with.depth;
                                if (with.NextVisible(with.hasChildren) == true) {
                                    do {

                                        if (with.depth < initDepth) break;

                                        ComponentDataEditor.lastDraw.Add(new ComponentDataProperty() {
                                            position = position,
                                            property = with.propertyPath,
                                            so = with.serializedObject,
                                        });

                                        EditorGUI.BeginChangeCheck();
                                        EditorGUI.PropertyField(position, with, true);
                                        position.y += EditorGUI.GetPropertyHeight(with, true);
                                        if (EditorGUI.EndChangeCheck() == true) {

                                            isDirty = true;

                                        }

                                    } while (with.NextVisible(false) == true);
                                }

                            }

                            if (isDirty == true) {

                                var obj = property.serializedObject;
                                if (obj.targetObject is IValidateEditor validateEditor) {

                                    obj.ApplyModifiedProperties();
                                    obj.Update();
                                    validateEditor.OnValidateEditor();
                                    EditorUtility.SetDirty(obj.targetObject);
                                    obj.ApplyModifiedProperties();
                                    obj.Update();

                                }

                            }

                        }

                    }

                }

            }
            {
                var obj = property.serializedObject;
                GUILayoutExt.DrawAddComponentMenu(position, usedComponents, (addType, isUsed) => {
                    
                    obj.Update();
                    var prop = obj.FindProperty(property.propertyPath);
                    var with = prop.FindPropertyRelative(name);
                    if (isUsed == true) {

                        usedComponents.Remove(addType);
                        with.managedReferenceValue = null;

                    } else {

                        usedComponents.Add(addType);
                        with.managedReferenceValue = (IStructComponentBase)System.Activator.CreateInstance(addType);

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

                }, showRuntime: true);
            }

        }
        
        internal static bool GetTypeFromManagedReferenceFullTypeName(string managedReferenceFullTypename, out System.Type managedReferenceInstanceType)
        {
            managedReferenceInstanceType = null;

            var parts = managedReferenceFullTypename.Split(' ');
            if (parts.Length == 2)
            {
                var assemblyPart = parts[0];
                var nsClassnamePart = parts[1];
                managedReferenceInstanceType = System.Type.GetType($"{nsClassnamePart}, {assemblyPart}");
            }

            return managedReferenceInstanceType != null;
        }

    }

}
