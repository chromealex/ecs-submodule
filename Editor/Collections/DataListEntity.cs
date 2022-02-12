using ME.ECS;

namespace ME.ECSEditor.Collections {

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Collections.DataList<Entity>))]
    public class DataListEntityListPropertyEditor : UnityEditor.PropertyDrawer {

        private const float EMPTY_HEIGHT = 30f;

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label) {

            var h = 22f + 18f;
            var list = property.GetSerializedValue<ME.ECS.Collections.DataList<Entity>>();
            var world = ME.ECS.Worlds.currentWorld;
            if (world == null || list.Count == 0) {

                h += DataListEntityListPropertyEditor.EMPTY_HEIGHT;

            } else {
                
                var i = 0;
                if (list.IsCreated() == true) {
                    foreach (var item in list.Read()) {

                        var elementLabel = new UnityEngine.GUIContent($"Element {i}");
                        h += EntityEditor.GetHeight(item, elementLabel);
                        ++i;

                    }
                }

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

            var list = property.GetSerializedValue<ME.ECS.Collections.DataList<Entity>>();
            
            UnityEngine.GUI.Label(headerRect, label, UnityEditor.EditorStyles.boldLabel);
            UnityEngine.GUI.Label(countRect, $"Count: {list.Count}");
            var world = ME.ECS.Worlds.currentWorld;
            if (world == null || list.Count == 0) {

                UnityEngine.GUI.Label(contentRect, "List is empty");

            } else {

                var rect = contentRect;
                var i = 0;
                if (list.IsCreated() == true) {
                    foreach (var item in list.Read()) {

                        var elementLabel = new UnityEngine.GUIContent($"Element {i}");
                        var h = EntityEditor.GetHeight(item, elementLabel);
                        rect.height = h;
                        EntityEditor.Draw(rect, item, elementLabel);
                        rect.y += h;
                        ++i;

                    }
                }

            }

        }

    }

}