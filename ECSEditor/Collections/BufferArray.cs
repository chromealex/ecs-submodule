
namespace ME.ECSEditor.Collections {

    /*[UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Collections.BufferArray<>))]
    public class BufferArrayPropertyEditor : UnityEditor.PropertyDrawer {

        private const float EMPTY_HEIGHT = 30f;

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var h = 22f + 18f;
            
            var list = property.GetSerializedValue<ME.ECS.Collections.IBufferArray>();
            if (list.Count == 0) {

                h += BufferArrayPropertyEditor.EMPTY_HEIGHT;

            } else {
                
                var arr = property.FindPropertyRelative("arr").FindPropertyRelative("data");
                var arrSource = arr.Copy();
                arr.NextVisible(true);
                for (int i = 0; i < arrSource.arraySize; ++i) {

                    h += UnityEditor.EditorGUI.GetPropertyHeight(arrSource.GetArrayElementAtIndex(i), true);
                    
                }
                h += UnityEditor.EditorGUI.GetPropertyHeight(arr, true);
                
            }

            return h;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var headerRect = position;
            headerRect.height = 22f;

            var countRect = position;
            countRect.y += headerRect.height;
            countRect.height = 18f;

            var contentRect = position;
            contentRect.y += headerRect.height + countRect.height;
            contentRect.height -= headerRect.height + countRect.height;

            var size = property.FindPropertyRelative("Length");
            var arr = property.FindPropertyRelative("arr").FindPropertyRelative("data");
            size.intValue = arr.arraySize;
            
            UnityEngine.GUI.Label(headerRect, label, UnityEditor.EditorStyles.boldLabel);
            UnityEditor.EditorGUI.PropertyField(countRect, size, new UnityEngine.GUIContent("Size"));
            
            if (size.intValue < 0) size.intValue = 0;
            arr.arraySize = size.intValue;
            if (size.intValue == 0) {

                UnityEngine.GUI.Label(contentRect, "Array is empty");

            } else {

                var rect = contentRect;
                var arrSource = arr.Copy();
                arr.NextVisible(true);
                for (int i = 0; i < size.intValue; ++i) {

                    var h = UnityEditor.EditorGUI.GetPropertyHeight(property, true);
                    UnityEditor.EditorGUI.PropertyField(rect, arrSource.GetArrayElementAtIndex(i), true);
                    rect.y += h;

                }
                
            }

        }

    }*/

}