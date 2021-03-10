
using ME.ECS.BlackBox;

namespace ME.ECSEditor.BlackBox {

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.BlackBox.FieldDataContainer))]
    public class FieldDataContainerDrawer : UnityEditor.PropertyDrawer {

        private static readonly System.Collections.Generic.Dictionary<BoxVariable, UnityEditor.SerializedPropertyType> varsToType = new System.Collections.Generic.Dictionary<BoxVariable, UnityEditor.SerializedPropertyType>();
        
        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            var isInput = property.FindPropertyRelative("isInput").boolValue;
            if (isInput == true) return 0f;
            
            var h = 0f;
            var data = property.FindPropertyRelative("data");
            for (int i = 0; i < data.arraySize; ++i) {
                
                var element = data.GetArrayElementAtIndex(i);
                var isArray = element.FindPropertyRelative("isArray");
                var key = (isArray.boolValue == true ? "valueArr" : "value");
                var value = element.FindPropertyRelative(key);
                h += UnityEditor.EditorGUI.GetPropertyHeight(value, true);
                
            }

            return h;

        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var data = property.FindPropertyRelative("data");
            var vars = property.FindPropertyRelative("boxVars");
            var captions = property.FindPropertyRelative("captions");
            var isInput = property.FindPropertyRelative("isInput").boolValue;
            for (int i = 0; i < data.arraySize; ++i) {

                var element = data.GetArrayElementAtIndex(i);
                var isArray = element.FindPropertyRelative("isArray");
                var key = (isArray.boolValue == true ? "valueArr" : "value");
                
                var value = element.FindPropertyRelative(key);
                if (isInput == false) UnityEditor.EditorGUI.PropertyField(position, value, new UnityEngine.GUIContent(GUILayoutExt.GetStringCamelCaseSpace(captions.GetArrayElementAtIndex(i).stringValue)), true);
                var h = UnityEditor.EditorGUI.GetPropertyHeight(value, true);
                position.height = UnityEditor.EditorGUI.GetPropertyHeight(value, false);
                this.DrawLink(ref position, i, vars.GetArrayElementAtIndex(i), value, isInput);
                position.y += h;

            }
            
        }

        private void DrawLink(ref UnityEngine.Rect position, int index, UnityEditor.SerializedProperty property, UnityEditor.SerializedProperty value, bool isInput) {

            if (isInput == true) {

                var lastDraw = ComponentDataEditor.lastDraw;
                var info = lastDraw[index];
                var propType = info.so.FindProperty(info.property).propertyType;
                
                var elemType = UnityEditor.SerializedPropertyType.Generic;
                var box = (property.objectReferenceValue as BoxVariable);
                if (box != null) {

                    if (FieldDataContainerDrawer.varsToType.TryGetValue(box, out elemType) == false) {

                        elemType = UnityEditor.SerializedPropertyType.Generic;

                    }
                    
                }

                position = info.position;
                
                if (BlackBoxContainerEditor.active != null) {
                    BlackBoxContainerEditor.active.DrawLink(property, position.x - 20f, position.y, drawIn: false, new BlackBoxContainerEditor.LinkParameters() {
                        inLink = true,
                        isWrong = propType != elemType,
                        boxType = typeof(ME.ECS.BlackBox.BoxVariable),
                    });
                }

            } else {

                UnityEditor.SerializedPropertyType propType = UnityEditor.SerializedPropertyType.Generic;
                if (value.propertyType == UnityEditor.SerializedPropertyType.ManagedReference) {

                    FieldDataContainerDrawer.GetTypeFromManagedReferenceFullTypeName(value.managedReferenceFullTypename, out var propSysType);
                    propType = this.GetPropertyType(propSysType);
                    
                } else if (value.isArray == true) {

                    propType = UnityEditor.SerializedPropertyType.ArraySize;

                }

                var box = property.objectReferenceValue as BoxVariable;
                if (box != null) {

                    if (FieldDataContainerDrawer.varsToType.ContainsKey(box) == false) {

                        FieldDataContainerDrawer.varsToType.Add(box, propType);

                    } else {

                        FieldDataContainerDrawer.varsToType[box] = propType;

                    }

                }
                
                var style = new UnityEngine.GUIStyle(UnityEditor.EditorStyles.miniBoldLabel);
                style.alignment = UnityEngine.TextAnchor.MiddleRight;
                UnityEditor.EditorGUI.LabelField(position, "var", style);
                if (BlackBoxContainerEditor.active != null) BlackBoxContainerEditor.active.DrawLink(property, position.x + position.width, position.y, drawIn: false);

            }
            
        }
        
        internal UnityEditor.SerializedPropertyType GetPropertyType(System.Type type) {

            if (type == null) return UnityEditor.SerializedPropertyType.Generic;
            if (type == typeof(int)) return UnityEditor.SerializedPropertyType.Integer;
            if (type == typeof(bool)) return UnityEditor.SerializedPropertyType.Boolean;
            if (type == typeof(float)) return UnityEditor.SerializedPropertyType.Float;
            if (type == typeof(string)) return UnityEditor.SerializedPropertyType.String;
            if (type == typeof(UnityEngine.Color)) return UnityEditor.SerializedPropertyType.Color;
            if (type == typeof(UnityEngine.Color32)) return UnityEditor.SerializedPropertyType.Color;
            if (type.IsAssignableFrom(typeof(UnityEngine.Object)) == true) return UnityEditor.SerializedPropertyType.ObjectReference;
            if (type == typeof(UnityEngine.LayerMask)) return UnityEditor.SerializedPropertyType.LayerMask;
            if (type.IsEnum == true) return UnityEditor.SerializedPropertyType.Enum;
            if (type == typeof(UnityEngine.Vector2)) return UnityEditor.SerializedPropertyType.Vector2;
            if (type == typeof(UnityEngine.Vector3)) return UnityEditor.SerializedPropertyType.Vector3;
            if (type == typeof(UnityEngine.Vector4)) return UnityEditor.SerializedPropertyType.Vector4;
            if (type == typeof(UnityEngine.Vector2Int)) return UnityEditor.SerializedPropertyType.Vector2Int;
            if (type == typeof(UnityEngine.Vector3Int)) return UnityEditor.SerializedPropertyType.Vector3Int;
            if (type == typeof(UnityEngine.Rect)) return UnityEditor.SerializedPropertyType.Rect;
            if (type == typeof(UnityEngine.RectInt)) return UnityEditor.SerializedPropertyType.RectInt;
            if (type == typeof(char)) return UnityEditor.SerializedPropertyType.Character;
            if (type == typeof(UnityEngine.Bounds)) return UnityEditor.SerializedPropertyType.Bounds;
            if (type == typeof(UnityEngine.BoundsInt)) return UnityEditor.SerializedPropertyType.BoundsInt;
            if (type == typeof(UnityEngine.AnimationCurve)) return UnityEditor.SerializedPropertyType.AnimationCurve;
            if (type == typeof(UnityEngine.Gradient)) return UnityEditor.SerializedPropertyType.Gradient;
            if (type == typeof(UnityEngine.Quaternion)) return UnityEditor.SerializedPropertyType.Quaternion;

            return UnityEditor.SerializedPropertyType.Generic;

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