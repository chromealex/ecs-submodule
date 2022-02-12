#if VIEWS_MODULE_SUPPORT
using ME.ECS.Views;
using UnityEngine;

namespace ME.ECSEditor {

    [ComponentCustomEditor(typeof(IViewComponent))]
    public class ViewComponentEditor : ME.ECSEditor.IGUIEditor<IViewComponent> {

        public IViewComponent target { get; set; }
        public IViewComponent[] targets { get; set; }

        public T GetTarget<T>() {

            return (T)(object)this.target;

        }

        bool IGUIEditorBase.OnDrawGUI() {
                
            UnityEngine.GUILayout.Label("Prefab Source Id: " + this.target.GetViewInfo().prefabSourceId.ToString());

            return false;

        }

    }

    [ComponentCustomEditor(typeof(IViewModuleBase))]
    public class ViewsModuleEditor : ME.ECSEditor.IGUIEditor<IViewModuleBase> {

        public IViewModuleBase target { get; set; }
        public IViewModuleBase[] targets { get; set; }

        public T GetTarget<T>() {

            return (T)(object)this.target;

        }

        bool IGUIEditorBase.OnDrawGUI() {
                
            var style = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label);
            style.richText = true;

            var renderersCount = 0;
            var data = this.target.GetData();
            if (data.arr != null) {

                for (int i = 0; i < data.Length; ++i) {

                    var views = data.arr[i];
                    renderersCount += views.Length;

                }

            }

            UnityEngine.GUILayout.Label("<b>Alive Views:</b> " + renderersCount.ToString(), style);

            return false;

        }

    }

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.Views.ViewComponent))]
    public class ViewComponentPropertyDrawer : UnityEditor.PropertyDrawer {

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

            var viewInfoProp = property.FindPropertyRelative("viewInfo");
            var viewInfo = viewInfoProp.GetSerializedValue<ME.ECS.Views.ViewInfo>();
            if (ME.ECS.Worlds.current != null) {

                var viewsModule = ME.ECS.Worlds.current.GetModule<ME.ECS.Views.ViewsModule>();
                var viewSource = viewsModule.GetViewSource(viewInfo.prefabSourceId);
                var viewsModuleInt = (IViewModuleBase)viewsModule;
                var arr = viewsModuleInt.GetData();
                var hasInstance = false;
                if (viewInfo.entity.id < arr.Length) {
                    var item = arr[viewInfo.entity.id];
                    if (item.mainView != null) {
                        var view = item.mainView as ME.ECS.Views.Providers.MonoBehaviourViewBase;
                        if (view != null) {
                            
                            UnityEditor.EditorGUI.BeginDisabledGroup(true);
                            UnityEditor.EditorGUI.ObjectField(position, new GUIContent("View"), (Object)viewSource, typeof(Object), allowSceneObjects: false);
                            UnityEditor.EditorGUI.EndDisabledGroup();
                            hasInstance = true;

                        }
                    }
                }

                if (hasInstance == false) {

                    UnityEditor.EditorGUI.BeginDisabledGroup(true);
                    UnityEditor.EditorGUI.ObjectField(position, new GUIContent("View Source"), (Object)viewSource, typeof(Object), allowSceneObjects: false);
                    UnityEditor.EditorGUI.EndDisabledGroup();

                }

            } else {

                GUI.Label(position, "View Source Id: " + viewInfo.prefabSourceId);

            }

        }

    }

}
#endif