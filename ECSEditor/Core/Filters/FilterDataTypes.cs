
using System.Linq;

namespace ME.ECSEditor {

    using ME.ECS;
    using UnityEditor;
    using UnityEngine;
    
    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.FilterDataTypes))]
    public class FilterDataTypesEditor : UnityEditor.PropertyDrawer {

        private const float headerHeight = 22f;
        private const float lineHeight = 22f;
        private const float editButtonHeight = 40f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {

            var h = 0f;
            h += FilterDataTypesEditor.headerHeight;
            var with = property.FindPropertyRelative("with");
            h += with.arraySize * lineHeight;

            h += FilterDataTypesEditor.headerHeight;
            var without = property.FindPropertyRelative("without");
            h += without.arraySize * lineHeight;

            return h + editButtonHeight + editButtonHeight;
            
        }

        private Rect DrawArray(UnityEngine.Rect position, SerializedProperty property, string name) {
            
            var list = new System.Collections.Generic.List<System.Type>();
            var usedComponents = new System.Collections.Generic.HashSet<System.Type>();
            {
                var backStyle = new GUIStyle(EditorStyles.label);
                backStyle.normal.background = Texture2D.whiteTexture;

                var with = property.FindPropertyRelative(name);
                for (int i = 0; i < with.arraySize; ++i) {

                    var registry = with.GetArrayElementAtIndex(i);
                    FilterDataTypesEditor.GetTypeFromManagedReferenceFullTypeName(registry.managedReferenceFullTypename, out var type);

                    if (type == null) {

                        Debug.Log("Not found: " + registry.managedReferenceFullTypename + ", " + registry.managedReferenceFieldTypename);
                        continue;

                    }
                    
                    list.Add(type);
                    usedComponents.Add(type);

                    using (new GUILayoutExt.GUIBackgroundAlphaUsing(i % 2 == 0 ? 0f : 0.05f)) {

                        {
                            var componentName = GUILayoutExt.GetStringCamelCaseSpace(type.Name);

                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUI.Toggle(position, componentName, true, EditorStyles.toggle);
                            EditorGUI.EndDisabledGroup();

                        }

                    }

                    position.y += lineHeight;

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
                        with.DeleteArrayElementAtIndex(list.IndexOf(addType));
                        list.Remove(addType);

                    } else {

                        usedComponents.Add(addType);
                        list.Add(addType);
                        ++with.arraySize;
                        var item = with.GetArrayElementAtIndex(with.arraySize - 1);
                        item.managedReferenceValue = (IStructComponentBase)System.Activator.CreateInstance(addType);

                    }
                    obj.ApplyModifiedProperties();

                }, showRuntime: true);
            }

            position.y += FilterDataTypesEditor.editButtonHeight;
            return position;

        }

        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label) {

            position.height = FilterDataTypesEditor.headerHeight;
            EditorGUI.LabelField(position, "Entity must include:", EditorStyles.boldLabel);
            position.y += FilterDataTypesEditor.headerHeight;
            position = this.DrawArray(position, property, "with");
            
            position.height = FilterDataTypesEditor.headerHeight;
            EditorGUI.LabelField(position, "Entity must exclude:", EditorStyles.boldLabel);
            position.y += FilterDataTypesEditor.headerHeight;
            position = this.DrawArray(position, property, "without");
            
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
