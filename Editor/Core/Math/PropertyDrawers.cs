using System.Collections;
using System.Collections.Generic;
using ME.ECSEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEditor;
using ME.ECS.Mathematics;

namespace ME.ECSEditor {

    using ME.ECS;

    internal static class Helper {

        public static sfloat GetFPValue(SerializedProperty property) {
            
            var val = property.FindPropertyRelative("rawValue");
            return sfloat.FromRaw((uint)val.longValue);
            
        }
        
        public static void SetFPValue(SerializedProperty property, sfloat value) {
            
            var val = property.FindPropertyRelative("rawValue");
            val.longValue = value.RawValue;
            
        }

    }
    
    [UnityEditor.CustomPropertyDrawer(typeof(sfloat))]
    public class FpPropertyDrawer : UnityEditor.PropertyDrawer {

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

            var fp = Helper.GetFPValue(property);
            var v = UnityEditor.EditorGUI.FloatField(position, label, (float)fp);
            Helper.SetFPValue(property, v);

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(float2))]
    public class Fp2PropertyDrawer : UnityEditor.PropertyDrawer {

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

            var fpx = Helper.GetFPValue(property.FindPropertyRelative("x"));
            var fpy = Helper.GetFPValue(property.FindPropertyRelative("y"));
            var v = UnityEditor.EditorGUI.Vector2Field(position, label, new Vector2((float)fpx, (float)fpy));
            Helper.SetFPValue(property.FindPropertyRelative("x"), v.x);
            Helper.SetFPValue(property.FindPropertyRelative("y"), v.y);
            
        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(float3))]
    public class Fp3PropertyDrawer : UnityEditor.PropertyDrawer {

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

            var fpx = Helper.GetFPValue(property.FindPropertyRelative("x"));
            var fpy = Helper.GetFPValue(property.FindPropertyRelative("y"));
            var fpz = Helper.GetFPValue(property.FindPropertyRelative("z"));
            var v = UnityEditor.EditorGUI.Vector3Field(position, label, new Vector3((float)fpx, (float)fpy, (float)fpz));
            Helper.SetFPValue(property.FindPropertyRelative("x"), v.x);
            Helper.SetFPValue(property.FindPropertyRelative("y"), v.y);
            Helper.SetFPValue(property.FindPropertyRelative("z"), v.z);
            
        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(float4))]
    public class Fp4PropertyDrawer : UnityEditor.PropertyDrawer {

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

            var fpx = Helper.GetFPValue(property.FindPropertyRelative("x"));
            var fpy = Helper.GetFPValue(property.FindPropertyRelative("y"));
            var fpz = Helper.GetFPValue(property.FindPropertyRelative("z"));
            var fpw = Helper.GetFPValue(property.FindPropertyRelative("w"));
            var v = UnityEditor.EditorGUI.Vector4Field(position, label, new Vector4((float)fpx, (float)fpy, (float)fpz, (float)fpw));
            Helper.SetFPValue(property.FindPropertyRelative("x"), v.x);
            Helper.SetFPValue(property.FindPropertyRelative("y"), v.y);
            Helper.SetFPValue(property.FindPropertyRelative("z"), v.z);
            Helper.SetFPValue(property.FindPropertyRelative("w"), v.w);

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(quaternion))]
    public class FpQuaternionPropertyDrawer : UnityEditor.PropertyDrawer {

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

            var fpx = Helper.GetFPValue(property.FindPropertyRelative("value").FindPropertyRelative("x"));
            var fpy = Helper.GetFPValue(property.FindPropertyRelative("value").FindPropertyRelative("y"));
            var fpz = Helper.GetFPValue(property.FindPropertyRelative("value").FindPropertyRelative("z"));
            var fpw = Helper.GetFPValue(property.FindPropertyRelative("value").FindPropertyRelative("w"));
            var q = new Quaternion((float)fpx, (float)fpy, (float)fpz, (float)fpw);
            var v = UnityEditor.EditorGUI.Vector3Field(position, label, q.eulerAngles);
            q = Quaternion.Euler(v);
            Helper.SetFPValue(property.FindPropertyRelative("value").FindPropertyRelative("x"), q.x);
            Helper.SetFPValue(property.FindPropertyRelative("value").FindPropertyRelative("y"), q.y);
            Helper.SetFPValue(property.FindPropertyRelative("value").FindPropertyRelative("z"), q.z);
            Helper.SetFPValue(property.FindPropertyRelative("value").FindPropertyRelative("w"), q.w);
            
        }

    }

}