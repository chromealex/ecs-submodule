using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace ME.ECS.Essentials.GOAP.Editor {

    [UnityEditor.CustomEditor(typeof(GOAPGenericAction))]
    public class GOAPGenericActionEditor : UnityEditor.Editor {

        public override VisualElement CreateInspectorGUI() {

            var cost = this.serializedObject.FindProperty("cost");
            var conditions = this.serializedObject.FindProperty("conditions");
            var effects = this.serializedObject.FindProperty("effects");
            var items = this.serializedObject.FindProperty("items");
            
            var visualElement = new VisualElement();
            visualElement.Add(new PropertyField(cost));
            visualElement.Add(new PropertyField(conditions));
            visualElement.Add(new PropertyField(effects));
            visualElement.Add(new PropertyField(items));
            return visualElement;
            
        }

    }

}