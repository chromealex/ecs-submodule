namespace ME.ECSEditor {

    [UnityEditor.CustomPropertyDrawer(typeof(Unity.Mathematics.quaternion))]
    public class TransformMathQuaternionComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return UnityEditor.EditorGUIUtility.wideMode == false ? 36f : 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var value = property.FindPropertyRelative("value");
            var x = value.FindPropertyRelative("x");
            var y = value.FindPropertyRelative("y");
            var z = value.FindPropertyRelative("z");
            var w = value.FindPropertyRelative("w");
            var v = new UnityEngine.Quaternion(x.floatValue, y.floatValue, z.floatValue, w.floatValue).eulerAngles;
            v = UnityEditor.EditorGUI.Vector3Field(position, label, v);
            var q = UnityEngine.Quaternion.Euler(v);
            value.FindPropertyRelative("x").floatValue = q.x;
            value.FindPropertyRelative("y").floatValue = q.y;
            value.FindPropertyRelative("z").floatValue = q.z;
            value.FindPropertyRelative("w").floatValue = q.w;
            
        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Mathematics.quaternion))]
    public class TransformFPQuaternionComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return UnityEditor.EditorGUIUtility.wideMode == false ? 36f : 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var value = property.FindPropertyRelative("value");
            var x = value.FindPropertyRelative("x");
            var y = value.FindPropertyRelative("y");
            var z = value.FindPropertyRelative("z");
            var w = value.FindPropertyRelative("w");
            var v = new UnityEngine.Quaternion(x.floatValue, y.floatValue, z.floatValue, w.floatValue).eulerAngles;
            v = UnityEditor.EditorGUI.Vector3Field(position, label, v);
            var q = UnityEngine.Quaternion.Euler(v);
            value.FindPropertyRelative("x").floatValue = q.x;
            value.FindPropertyRelative("y").floatValue = q.y;
            value.FindPropertyRelative("z").floatValue = q.z;
            value.FindPropertyRelative("w").floatValue = q.w;
            
        }

    }
/*
    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Transform.Position))]
    public class TransformPositionComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var value = property.FindPropertyRelative("value");
            UnityEditor.EditorGUI.indentLevel = 0;
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
            var x = value.FindPropertyRelative("x");
            var y = value.FindPropertyRelative("y");
            var z = value.FindPropertyRelative("z");
            var w = value.FindPropertyRelative("w");
            var v = new UnityEngine.Quaternion(x.floatValue, y.floatValue, z.floatValue, w.floatValue).eulerAngles;
            UnityEditor.EditorGUI.indentLevel = 0;
            v = UnityEditor.EditorGUI.Vector3Field(position, label, v);
            var q = UnityEngine.Quaternion.Euler(v);
            value.FindPropertyRelative("x").floatValue = q.x;
            value.FindPropertyRelative("y").floatValue = q.y;
            value.FindPropertyRelative("z").floatValue = q.z;
            value.FindPropertyRelative("w").floatValue = q.w;
            
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

    }*/

}