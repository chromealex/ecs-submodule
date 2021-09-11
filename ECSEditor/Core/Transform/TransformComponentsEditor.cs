namespace ME.ECSEditor {

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Transform.Position))]
    public class TransformPositionComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var value = property.FindPropertyRelative("value");
            UnityEditor.EditorGUI.PropertyField(position, value, label);

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Transform.Rotation))]
    public class TransformRotationComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var value = property.FindPropertyRelative("value");
            UnityEditor.EditorGUI.PropertyField(position, value, label);

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Transform.Scale))]
    public class TransformScaleComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var value = property.FindPropertyRelative("value");
            UnityEditor.EditorGUI.PropertyField(position, value, label);

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Transform.Position2D))]
    public class TransformPosition2DComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var value = property.FindPropertyRelative("value");
            UnityEditor.EditorGUI.PropertyField(position, value, label);

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Transform.Rotation2D))]
    public class TransformRotation2DComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var value = property.FindPropertyRelative("value");
            UnityEditor.EditorGUI.PropertyField(position, value, label);

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Transform.Scale2D))]
    public class TransformScale2DComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var value = property.FindPropertyRelative("value");
            UnityEditor.EditorGUI.PropertyField(position, value, label);

        }

    }

}