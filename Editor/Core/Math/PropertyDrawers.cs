using System.Collections;
using System.Collections.Generic;
using ME.ECSEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEditor;

namespace ME.ECSEditor {

    using ME.ECS;

    internal static class Helper {

        public static fp GetFPValue(SerializedProperty property) {
            
            var val = property.FindPropertyRelative("m_rawValue");
            return new fp(val.longValue);
            
        }
        
        public static void SetFPValue(SerializedProperty property, fp value) {
            
            var val = property.FindPropertyRelative("m_rawValue");
            val.longValue = value.RawValue;
            
        }

    }
    
    [UnityEditor.CustomPropertyDrawer(typeof(fp))]
    public class FpPropertyDrawer : UnityEditor.PropertyDrawer {

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

            var fp = Helper.GetFPValue(property);
            var v = UnityEditor.EditorGUI.FloatField(position, label, fp);
            Helper.SetFPValue(property, v);

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(fp2))]
    public class Fp2PropertyDrawer : UnityEditor.PropertyDrawer {

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

            var fpx = Helper.GetFPValue(property.FindPropertyRelative("x"));
            var fpy = Helper.GetFPValue(property.FindPropertyRelative("y"));
            var v = UnityEditor.EditorGUI.Vector2Field(position, label, new Vector2(fpx, fpy));
            Helper.SetFPValue(property.FindPropertyRelative("x"), v.x);
            Helper.SetFPValue(property.FindPropertyRelative("y"), v.y);
            
        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(fp3))]
    public class Fp3PropertyDrawer : UnityEditor.PropertyDrawer {

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

            var fpx = Helper.GetFPValue(property.FindPropertyRelative("x"));
            var fpy = Helper.GetFPValue(property.FindPropertyRelative("y"));
            var fpz = Helper.GetFPValue(property.FindPropertyRelative("z"));
            var v = UnityEditor.EditorGUI.Vector3Field(position, label, new Vector3(fpx, fpy, fpz));
            Helper.SetFPValue(property.FindPropertyRelative("x"), v.x);
            Helper.SetFPValue(property.FindPropertyRelative("y"), v.y);
            Helper.SetFPValue(property.FindPropertyRelative("z"), v.z);
            
        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(fp4))]
    public class Fp4PropertyDrawer : UnityEditor.PropertyDrawer {

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

            var fpx = Helper.GetFPValue(property.FindPropertyRelative("x"));
            var fpy = Helper.GetFPValue(property.FindPropertyRelative("y"));
            var fpz = Helper.GetFPValue(property.FindPropertyRelative("z"));
            var fpw = Helper.GetFPValue(property.FindPropertyRelative("w"));
            var v = UnityEditor.EditorGUI.Vector4Field(position, label, new Vector4(fpx, fpy, fpz, fpw));
            Helper.SetFPValue(property.FindPropertyRelative("x"), v.x);
            Helper.SetFPValue(property.FindPropertyRelative("y"), v.y);
            Helper.SetFPValue(property.FindPropertyRelative("z"), v.z);
            Helper.SetFPValue(property.FindPropertyRelative("w"), v.w);

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(fpquaternion))]
    public class FpQuaternionPropertyDrawer : UnityEditor.PropertyDrawer {

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

            var fpx = Helper.GetFPValue(property.FindPropertyRelative("x"));
            var fpy = Helper.GetFPValue(property.FindPropertyRelative("y"));
            var fpz = Helper.GetFPValue(property.FindPropertyRelative("z"));
            var fpw = Helper.GetFPValue(property.FindPropertyRelative("w"));
            var q = new Quaternion(fpx, fpy, fpz, fpw);
            var v = UnityEditor.EditorGUI.Vector3Field(position, label, q.eulerAngles);
            q = Quaternion.Euler(v);
            Helper.SetFPValue(property.FindPropertyRelative("x"), q.x);
            Helper.SetFPValue(property.FindPropertyRelative("y"), q.y);
            Helper.SetFPValue(property.FindPropertyRelative("z"), q.z);
            Helper.SetFPValue(property.FindPropertyRelative("w"), q.w);
            
        }

    }

}