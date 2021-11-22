using UnityEngine;

namespace ME.ECSEditor.BlackBox {

    using ME.ECS.BlackBox;
    
    public static class PropertyHelper {

        public static bool GetTypeFromManagedReferenceFullTypeName(string managedReferenceFullTypename, out System.Type managedReferenceInstanceType) {
            
            managedReferenceInstanceType = null;

            var parts = managedReferenceFullTypename.Split(' ');
            if (parts.Length == 2) {
                var assemblyPart = parts[0];
                var nsClassnamePart = parts[1];
                managedReferenceInstanceType = System.Type.GetType($"{nsClassnamePart}, {assemblyPart}");
            }

            return managedReferenceInstanceType != null;
            
        }

        public static RefType GetRefType(UnityEditor.SerializedPropertyType type) {

            return (RefType)(int)type;

        }
        
        public static UnityEditor.SerializedPropertyType GetPropertyType(System.Type type) {

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

    }
    
    public static class RectExtensions {

        public static Vector2 TopLeft(this Rect rect) {
            return new Vector2(rect.xMin, rect.yMin);
        }

        public static Rect FitTo(this Rect rect, Rect fitTo) {
            
            var r = new Rect(rect);
            var factor = Mathf.Min(fitTo.width / rect.width, fitTo.height / rect.height);
            r.size *= factor;
            r.center = fitTo.center;
            return r;
            
        }

        public static Rect ScaleSizeBy(this Rect rect, float scale) {
            return rect.ScaleSizeBy(scale, rect.center);
        }

        public static Rect ScaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint) {
            Rect result = rect;
            result.x -= pivotPoint.x;
            result.y -= pivotPoint.y;
            result.xMin *= scale;
            result.xMax *= scale;
            result.yMin *= scale;
            result.yMax *= scale;
            result.x += pivotPoint.x;
            result.y += pivotPoint.y;
            return result;
        }

        public static Rect ScaleSizeBy(this Rect rect, Vector2 scale) {
            return rect.ScaleSizeBy(scale, rect.center);
        }

        public static Rect ScaleSizeBy(this Rect rect, Vector2 scale, Vector2 pivotPoint) {
            Rect result = rect;
            result.x -= pivotPoint.x;
            result.y -= pivotPoint.y;
            result.xMin *= scale.x;
            result.xMax *= scale.x;
            result.yMin *= scale.y;
            result.yMax *= scale.y;
            result.x += pivotPoint.x;
            result.y += pivotPoint.y;
            return result;
        }

    }

    public static class EditorZoomArea {

        private const float kEditorWindowTabHeight = 21.0f;
        private static Matrix4x4 _prevGuiMatrix;

        public static Rect Begin(float zoomScale, Rect screenCoordsArea) {
            GUI.EndGroup(); // End the group Unity begins automatically for an EditorWindow to clip out the window tab. This allows us to draw outside of the size of the EditorWindow.

            Rect clippedArea = screenCoordsArea.ScaleSizeBy(1.0f / zoomScale, screenCoordsArea.TopLeft());
            clippedArea.y += kEditorWindowTabHeight;
            GUI.BeginGroup(clippedArea);

            _prevGuiMatrix = GUI.matrix;
            Matrix4x4 translation = Matrix4x4.TRS(clippedArea.TopLeft(), Quaternion.identity, Vector3.one);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1.0f));
            GUI.matrix = translation * scale * translation.inverse * GUI.matrix;

            return clippedArea;
        }

        public static void End() {
            GUI.matrix = _prevGuiMatrix;
            GUI.EndGroup();
            GUI.BeginGroup(new Rect(0.0f, kEditorWindowTabHeight, Screen.width, Screen.height));
        }

    }

}