using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace ME.ECSEditor.Tools {

    public class CheckComponents : EditorWindow {

        [MenuItem("ME.ECS/Tools/Components Checker...")]
        public static void ShowWindow() {
            
            var window = CheckComponents.CreateInstance<CheckComponents>();
            window.titleContent = new UnityEngine.GUIContent("Components Checker");
            window.Show();
            
        }

        private TestsView view;
        
        public void OnEnable() {
            
            var container = new VisualElement();
            container.styleSheets.Add(EditorUtilities.Load<StyleSheet>("ECSEditor/Tools/Styles.uss", isRequired: true));

            var view = new TestsView(() => {

                var collectedComponents = new List<TestItem>();
                var asms = System.AppDomain.CurrentDomain.GetAssemblies();
                foreach (var asm in asms) {

                    var types = asm.GetTypes();
                    foreach (var type in types) {

                        if (type.IsValueType == true &&
                            typeof(ME.ECS.IStructComponent).IsAssignableFrom(type) == true &&
                            typeof(ME.ECS.IStructCopyableBase).IsAssignableFrom(type) == false &&
                            typeof(ME.ECS.IComponentStatic).IsAssignableFrom(type) == false &&
                            type.IsGenericType == false &&
                            type.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Length > 0) {
                        
                            collectedComponents.Add(new TestItem() {
                                type = type,
                                tests = new [] {
                                    new TestInfo(TestMethod.DirectCopy),
                                },
                            });
                            
                        }

                    }

                }
                
                return collectedComponents.OrderBy(x => x.type.Name).ToList();

            }, true);
            this.view = view;
            container.Add(view);
            
            this.rootVisualElement.Add(container);
            
        }

        public void Update() {

            this.view?.Update();

        }

    }

}