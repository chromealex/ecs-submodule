
namespace ME.ECSEditor.Collections {

    [ME.ECSEditor.CustomFieldEditorAttribute(typeof(ME.ECS.Collections.IntrusiveList))]
    public class IntrusiveListEditor : ME.ECSEditor.ICustomFieldEditor {

        public bool DrawGUI(string caption, object instance, int instanceArrIndex, System.Reflection.FieldInfo fieldInfo, ref object value, bool typeCheckOnly,
                            bool hasMultipleDifferentValues) {

            if (typeCheckOnly == false) {

                GUILayoutExt.DrawHeader(caption);
                var obj = (ME.ECS.Collections.IntrusiveList)value;
                GUILayoutExt.DataLabel("Count: " + obj.Count);
                
                GUILayoutExt.Box(2f, 2f, () => {

                    var world = ME.ECS.Worlds.currentWorld;
                    if (world == null) {

                        UnityEngine.GUILayout.Label("List is empty");
                        return;

                    }

                    var worldEditor = new WorldsViewerEditor.WorldEditor();
                    worldEditor.world = world;
                    
                    var i = 0;
                    foreach (var item in obj) {

                        var val = (object)item;
                        GUILayoutExt.PropertyField(worldEditor, "Element " + i, item, -1, fieldInfo, typeof(ME.ECS.Entity), ref val, typeCheckOnly, hasMultipleDifferentValues);
                        ++i;
                        
                    }

                    if (i == 0) {
                        
                        UnityEngine.GUILayout.Label("List is empty");
                        
                    }

                });
                
            }

            return true;

        }

        public bool OnDrawGUI() {
            return false;
        }

        public T GetTarget<T>() {
            return default;
        }

    }

}