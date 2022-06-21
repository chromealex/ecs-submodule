using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEditor;
using ME.ECSEditor;

namespace ME.ECS.Essentials.GOAP.Editor {

    [CustomPropertyDrawer(typeof(ConditionsData))]
    public class ConditionsDrawer : PropertyDrawer {

        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            
            var container = new VisualElement();
            container.styleSheets.Add(EditorUtilities.Load<StyleSheet>("Essentials/GOAP/Editor/styles.uss", isRequired: true));
            container.AddToClassList("preconditions-container");
            var filter = property.FindPropertyRelative("filter");
            var layout = new VisualElement();
            layout.AddToClassList("layout-container");
            container.Add(layout);
            
            var icon = new Image();
            icon.AddToClassList("icon");
            icon.image = EditorUtilities.Load<Texture>("Essentials/GOAP/Editor/Icons/input-icon.png");
            layout.Add(icon);
            {
                var elem = new VisualElement();
                elem.AddToClassList("right-content");
                layout.Add(elem);
                
                var description = new Label("Conditions");
                description.AddToClassList("header");
                elem.Add(description);
                
                var prop = new PropertyField(filter);
                elem.Add(prop);

            }
            return container;
            
        }

    }

}