
namespace ME.ECSEditor {

    using ME.ECS;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;
    
    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.FilterDataTypesOptional))]
    public class FilterDataTypesOptionalEditor : UnityEditor.PropertyDrawer {

        private const float headerHeight = 22f;
        private const float miniHeight = 16f;
        private const float lineHeight = 26f;
        private const float editButtonHeight = 40f;
        private const float marginBottom = 10f;

        private Rect DrawArray(UnityEngine.Rect position, SerializedProperty property, string name, ComponentDataTypeAttribute.Type drawType, string subName = null) {
            
            var list = new System.Collections.Generic.List<System.Type>();
            var usedComponents = new System.Collections.Generic.HashSet<System.Type>();
            {
                //var backStyle = new GUIStyle(EditorStyles.label);
                //backStyle.normal.background = Texture2D.whiteTexture;
                
                var with = property.FindPropertyRelative(name);
                var size = with.arraySize;
                for (int i = 0; i < size; ++i) {

                    var registryBase = with.GetArrayElementAtIndex(i);
                    var registry = registryBase;
                    if (subName != null) {
                        registry = registry.FindPropertyRelative(subName);
                    }
                    FilterDataTypesOptionalEditor.GetTypeFromManagedReferenceFullTypeName(registry.managedReferenceFullTypename, out var type);

                    if (type == null) {

                        Debug.Log("Not found: " + registry.managedReferenceFullTypename + ", " + registry.managedReferenceFieldTypename);
                        continue;

                    }
                    
                    list.Add(type);
                    usedComponents.Add(type);

                    position.height = FilterDataTypesOptionalEditor.lineHeight;

                    var backRect = EditorGUI.IndentedRect(position);
                    backRect.x -= 8f;
                    backRect.width += 8f;
                    
                    var optional = registryBase.FindPropertyRelative("optional");
                    if (optional != null) {

                        if (Unity.Collections.LowLevel.Unsafe.UnsafeUtility.IsBlittable(type) == true) {

                            const float optionalWidth = 30f;
                            var labelStyle = new GUIStyle(EditorStyles.miniLabel);
                            labelStyle.alignment = TextAnchor.MiddleRight;
                            labelStyle.padding = new RectOffset(0, (int)optionalWidth, 0, 0);
                            EditorGUI.LabelField(backRect, "Check Data Equals", labelStyle);
                            var optionalRect = backRect;
                            optionalRect.x += optionalRect.width - optionalWidth;
                            optionalRect.width = optionalWidth;
                            optional.boolValue = EditorGUI.Toggle(optionalRect, optional.boolValue);

                        } else {

                            var labelStyle = new GUIStyle(EditorStyles.miniLabel);
                            labelStyle.alignment = TextAnchor.MiddleRight;
                            EditorGUI.LabelField(backRect, new GUIContent("Not Blittable \u24D8", "Type is not blittable, so you cannot use `Check Data Equals` option. Make type blittable to have this option."), labelStyle);
                            optional.boolValue = false;

                        }

                        if (optional.boolValue == true) {
                            drawType = ComponentDataTypeAttribute.Type.WithData;
                        } else {
                            drawType = ComponentDataTypeAttribute.Type.NoData;
                        }

                    }

                    if (drawType == ComponentDataTypeAttribute.Type.WithData) {

                        var copy = registry.Copy();
                        var initDepth = copy.depth;
                        if (copy.NextVisible(copy.hasChildren) == true) {
                            ++EditorGUI.indentLevel;
                            do {

                                if (copy.depth <= initDepth) break;
                                var h = EditorGUI.GetPropertyHeight(copy, true);
                                backRect.height += h;

                            } while (copy.NextVisible(false) == true);
                            --EditorGUI.indentLevel;
                            backRect.height += 8f;
                        }

                    } else if (drawType == ComponentDataTypeAttribute.Type.NoData) { }
                    EditorGUI.DrawRect(backRect, new Color(0f, 0f, 0f, i % 2 == 0 ? 0.2f : 0.15f));
                    var separator = backRect;
                    separator.height = 1f;
                    EditorGUI.DrawRect(separator, new Color(1f, 1f, 1f, 0.05f));
                    
                    {
                        var componentName = GUILayoutExt.GetStringCamelCaseSpace(type.Name);
                        EditorGUI.LabelField(position, componentName, EditorStyles.boldLabel);
                    }

                    position.y += lineHeight;

                    if (drawType == ComponentDataTypeAttribute.Type.WithData) {

                        var copy = registry.Copy();
                        var initDepth = copy.depth;
                        if (copy.NextVisible(copy.hasChildren) == true) {
                            ++EditorGUI.indentLevel;
                            do {

                                if (copy.depth <= initDepth) break;
                                var h = EditorGUI.GetPropertyHeight(copy, true);
                                position.height = h;
                                EditorGUI.PropertyField(position, copy, true);
                                position.y += h;

                            } while (copy.NextVisible(false) == true);
                            --EditorGUI.indentLevel;
                            position.y += 8f;
                        }

                    } else if (drawType == ComponentDataTypeAttribute.Type.NoData) { }

                }
            }
            {
                var obj = property.serializedObject;
                position.height = editButtonHeight;
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
                        if (subName != null) {
                            item = item.FindPropertyRelative(subName);
                        }
                        item.managedReferenceValue = (IComponentBase)System.Activator.CreateInstance(addType);

                    }
                    obj.ApplyModifiedProperties();

                }, showRuntime: true);
            }

            position.y += FilterDataTypesOptionalEditor.editButtonHeight;
            return position;

        }
        
        private ComponentDataTypeAttribute GetAttr() {

            var attrs = this.fieldInfo.GetCustomAttributes(typeof(ComponentDataTypeAttribute), true);
            return (attrs.Length == 1 ? (ComponentDataTypeAttribute)attrs[0] : null);

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {

            var drawType = ComponentDataTypeAttribute.Type.NoData;
            if (this.GetAttr() is ComponentDataTypeAttribute attr) {

                drawType = attr.type;

            }
            
            var h = 0f;
            h += FilterDataTypesOptionalEditor.headerHeight;
            
            h += FilterDataTypesOptionalEditor.miniHeight;
            h += this.GetArrayHeight(property, "with", drawType, "data");
            h += editButtonHeight;
            
            h += FilterDataTypesOptionalEditor.miniHeight;
            h += this.GetArrayHeight(property, "without", ComponentDataTypeAttribute.Type.NoData);
            h += editButtonHeight;
            
            return h + marginBottom;
            
        }

        private float GetArrayHeight(SerializedProperty property, string name, ComponentDataTypeAttribute.Type drawType, string subName = null) {

            var h = 0f;
            var with = property.FindPropertyRelative(name);
            if (with != null && with.isArray == true) {

                var size = with.arraySize;
                for (int i = 0; i < size; ++i) {

                    var registryBase = with.GetArrayElementAtIndex(i);
                    var registry = registryBase;
                    if (subName != null) {
                        registry = registry.FindPropertyRelative(subName);
                    }
                    
                    h += FilterDataTypesOptionalEditor.lineHeight;
                    
                    var optional = registryBase.FindPropertyRelative("optional");
                    if (optional != null) {

                        if (optional.boolValue == true) {
                            drawType = ComponentDataTypeAttribute.Type.WithData;
                        } else {
                            drawType = ComponentDataTypeAttribute.Type.NoData;
                        }

                    }
                    
                    if (drawType == ComponentDataTypeAttribute.Type.WithData) {

                        var copy = registry.Copy();
                        var initDepth = copy.depth;
                        if (copy.NextVisible(copy.hasChildren) == true) {
                            do {

                                if (copy.depth <= initDepth) break;
                                h += EditorGUI.GetPropertyHeight(copy, true);

                            } while (copy.NextVisible(false) == true);

                            h += 8f;
                        }

                    } else if (drawType == ComponentDataTypeAttribute.Type.NoData) { }

                }

            }

            return h;

        }

        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label) {

            position.height -= marginBottom;
            
            var drawType = ComponentDataTypeAttribute.Type.NoData;
            if (this.GetAttr() is ComponentDataTypeAttribute attr) {

                drawType = attr.type;

            }

            const float pixel = 0.5f;
            const float pixel2 = 1f;
            const float alpha = 0.1f;
            const float alphaBack = 0.02f;
            
            var contentRect = EditorGUI.IndentedRect(position);
            var lineRect = EditorGUI.IndentedRect(position);
            var lineRectLeft = lineRect;
            lineRectLeft.width = pixel;
            lineRectLeft.height -= pixel2;
            lineRectLeft.y += pixel;
            var lineRectRight = lineRect;
            lineRectRight.x += lineRectRight.width;
            lineRectRight.height -= pixel2;
            lineRectRight.y += pixel;
            lineRectRight.width = pixel;
            var lineRectTop = lineRect;
            lineRectTop.height = pixel;
            lineRectTop.width -= pixel2;
            lineRectTop.x += pixel;
            var lineRectBottom = lineRect;
            lineRectBottom.y += lineRectBottom.height;
            lineRectBottom.height = pixel;
            lineRectBottom.width -= pixel2;
            lineRectBottom.x += pixel;
            EditorGUI.DrawRect(lineRectLeft, new Color(1f, 1f, 1f, alpha));
            EditorGUI.DrawRect(lineRectRight, new Color(1f, 1f, 1f, alpha));
            EditorGUI.DrawRect(lineRectTop, new Color(1f, 1f, 1f, alpha));
            EditorGUI.DrawRect(lineRectBottom, new Color(1f, 1f, 1f, alpha));

            contentRect.x += pixel;
            contentRect.width -= pixel2;
            contentRect.y += pixel;
            contentRect.height -= pixel2;
            EditorGUI.DrawRect(contentRect, new Color(1f, 1f, 1f, alphaBack));

            var backRect = EditorGUI.IndentedRect(position);
            position.height = FilterDataTypesOptionalEditor.headerHeight;
            EditorGUI.DrawRect(EditorGUI.IndentedRect(position), new Color(1f, 1f, 1f, alphaBack));
            position.x += 8f;
            position.width -= 8f;
            EditorGUI.LabelField(position, label, EditorStyles.boldLabel);
            position.y += FilterDataTypesOptionalEditor.headerHeight;
            
            position.height = FilterDataTypesOptionalEditor.miniHeight;
            {
                backRect.y = position.y;
                backRect.height = position.height;
                EditorGUI.DrawRect(backRect, new Color(0f, 0f, 0f, 0.1f));
                using (new GUILayoutExt.GUIColorUsing(new Color(1f, 1f, 1f, 0.5f))) {
                    EditorGUI.LabelField(position, "Include:", EditorStyles.miniLabel);
                }
            }

            position.y += FilterDataTypesOptionalEditor.miniHeight;
            position = this.DrawArray(position, property, "with", drawType, "data");
            
            position.height = FilterDataTypesOptionalEditor.miniHeight;
            {
                backRect.y = position.y;
                backRect.height = position.height;
                EditorGUI.DrawRect(backRect, new Color(0f, 0f, 0f, 0.1f));
                using (new GUILayoutExt.GUIColorUsing(new Color(1f, 1f, 1f, 0.5f))) {
                    EditorGUI.LabelField(position, "Exclude:", EditorStyles.miniLabel);
                }
            }
            position.y += FilterDataTypesOptionalEditor.miniHeight;
            position = this.DrawArray(position, property, "without", ComponentDataTypeAttribute.Type.NoData);
            
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
