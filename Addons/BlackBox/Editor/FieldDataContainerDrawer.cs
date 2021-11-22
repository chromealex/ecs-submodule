
using ME.ECS.BlackBox;

namespace ME.ECSEditor.BlackBox {

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.BlackBox.FieldDataContainer))]
    public class FieldDataContainerDrawer : UnityEditor.PropertyDrawer {

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
                var propType = PropertyHelper.GetRefType(info.so.FindProperty(info.property).propertyType);
                
                var elemType = RefType.Generic;
                var box = (property.objectReferenceValue as BoxVariable);
                if (box != null) {

                    elemType = box.type;

                }

                position = info.position;
                
                if (BlackBoxContainerEditor.active != null) {
                    BlackBoxContainerEditor.active.DrawLink(property, position.x - 20f, position.y, drawIn: false, new BlackBoxContainerEditor.LinkParameters() {
                        inLink = true,
                        isWrong = propType != elemType,
                        boxType = typeof(ME.ECS.BlackBox.BoxVariable),
                    });
                }
                var labelStyle = new UnityEngine.GUIStyle(UnityEditor.EditorStyles.miniLabel);
                labelStyle.alignment = UnityEngine.TextAnchor.MiddleRight;
                UnityEditor.EditorGUI.LabelField(new UnityEngine.Rect(position.x - position.width - 20f, position.y, position.width, 20f), propType.ToString(), labelStyle);

            } else {

                var propType = RefType.Generic;
                if (value.propertyType == UnityEditor.SerializedPropertyType.ManagedReference) {

                    PropertyHelper.GetTypeFromManagedReferenceFullTypeName(value.managedReferenceFullTypename, out var propSysType);
                    propType = PropertyHelper.GetRefType(PropertyHelper.GetPropertyType(propSysType));
                    
                } else if (value.isArray == true) {

                    propType = RefType.ArraySize;

                }

                var box = property.objectReferenceValue as BoxVariable;
                if (box != null) {

                    box.type = propType;
                    UnityEditor.EditorUtility.SetDirty(box);
                    
                }
                
                if (BlackBoxContainerEditor.active != null) BlackBoxContainerEditor.active.DrawLink(property, position.x + position.width, position.y, drawIn: false);
                UnityEditor.EditorGUI.LabelField(new UnityEngine.Rect(position.x + position.width + 20f, position.y, position.width, 20f), propType.ToString(), UnityEditor.EditorStyles.miniLabel);

            }
            
        }
        
    }

}