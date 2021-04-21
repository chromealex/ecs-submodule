namespace ME.ECSEditor {

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Transform.Position))]
    public class TransformPositionComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var x = property.FindPropertyRelative("x");
            var y = property.FindPropertyRelative("y");
            var z = property.FindPropertyRelative("z");
            var v = new UnityEngine.Vector3(x.floatValue, y.floatValue, z.floatValue);
            v = UnityEditor.EditorGUI.Vector3Field(position, label, v);
            x.floatValue = v.x;
            y.floatValue = v.y;
            z.floatValue = v.z;

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Transform.Rotation))]
    public class TransformRotationComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var x = property.FindPropertyRelative("x");
            var y = property.FindPropertyRelative("y");
            var z = property.FindPropertyRelative("z");
            var w = property.FindPropertyRelative("z");
            var v = new UnityEngine.Quaternion(x.floatValue, y.floatValue, z.floatValue, w.floatValue);
            v = UnityEngine.Quaternion.Euler(UnityEditor.EditorGUI.Vector3Field(position, label, v.eulerAngles));
            x.floatValue = v.x;
            y.floatValue = v.y;
            z.floatValue = v.z;
            w.floatValue = v.w;

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Transform.Scale))]
    public class TransformScaleComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var x = property.FindPropertyRelative("x");
            var y = property.FindPropertyRelative("y");
            var z = property.FindPropertyRelative("z");
            var v = new UnityEngine.Vector3(x.floatValue, y.floatValue, z.floatValue);
            v = UnityEditor.EditorGUI.Vector3Field(position, label, v);
            x.floatValue = v.x;
            y.floatValue = v.y;
            z.floatValue = v.z;

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Transform.Position2D))]
    public class TransformPosition2DComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var x = property.FindPropertyRelative("x");
            var y = property.FindPropertyRelative("y");
            var v = new UnityEngine.Vector2(x.floatValue, y.floatValue);
            v = UnityEditor.EditorGUI.Vector2Field(position, label, v);
            x.floatValue = v.x;
            y.floatValue = v.y;

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Transform.Rotation2D))]
    public class TransformRotation2DComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var v = property.FindPropertyRelative("x");
            v.floatValue = UnityEditor.EditorGUI.FloatField(position, label, v.floatValue);

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Transform.Scale2D))]
    public class TransformScale2DComponentEditor : UnityEditor.PropertyDrawer {

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {
            
            return 18f;
            
        }

        public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var x = property.FindPropertyRelative("x");
            var y = property.FindPropertyRelative("y");
            var v = new UnityEngine.Vector2(x.floatValue, y.floatValue);
            v = UnityEditor.EditorGUI.Vector2Field(position, label, v);
            x.floatValue = v.x;
            y.floatValue = v.y;

        }

    }

}