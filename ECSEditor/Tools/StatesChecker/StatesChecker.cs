using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace ME.ECSEditor.Tools {

    public class StatesChecker : EditorWindow {

        [MenuItem("ME.ECS/Tools/States Checker...")]
        public static void ShowWindow() {
            
            var window = StatesChecker.CreateInstance<StatesChecker>();
            window.titleContent = new UnityEngine.GUIContent("States Checker");
            window.Show();
            
        }

        private VisualElement scrollView;
        private TestsView view;

        private System.Type FindType(System.Type baseType) {
            
            var asms = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asms) {

                var types = asm.GetTypes();
                foreach (var type in types) {

                    if (type.IsAbstract == false && baseType.IsAssignableFrom(type) == true) {

                        return type;

                    }

                }

            }

            return null;

        }

        private List<System.Type> FindTypes(System.Type baseType) {

            var list = new List<System.Type>();
            var asms = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asms) {

                var types = asm.GetTypes();
                foreach (var type in types) {

                    if (type.IsAbstract == false && baseType.IsAssignableFrom(type) == true) {

                        list.Add(type);

                    }

                }

            }

            return list;

        }

        public void OnEnable() {
            
            var container = new VisualElement();
            container.styleSheets.Add(EditorUtilities.Load<StyleSheet>("ECSEditor/Tools/Styles.uss", isRequired: true));

            var view = new TestsView(() => {

                var collectedComponents = new List<TestItem>();
                collectedComponents.Add(new TestItem() {
                    type = typeof(ME.ECS.StructComponentsContainer),
                    tests = new [] {
                        new TestInfo(TestMethod.CopyFrom),
                        new TestInfo(TestMethod.Recycle),
                    },
                });
                collectedComponents.Add(new TestItem() {
                    type = typeof(ME.ECS.Storage),
                    tests = new [] {
                        new TestInfo(TestMethod.CopyFrom),
                        new TestInfo(TestMethod.Recycle),
                    },
                });
                collectedComponents.Add(new TestItem() {
                    type = typeof(ME.ECS.FilterBurstData),
                    tests = new [] {
                        new TestInfo(TestMethod.CopyFrom),
                    },
                });
                collectedComponents.Add(new TestItem() {
                    type = typeof(ME.ECS.FiltersStorage),
                    tests = new [] {
                        new TestInfo(TestMethod.CopyFrom),
                        new TestInfo(TestMethod.Recycle),
                    },
                });
                collectedComponents.Add(new TestItem() {
                    type = this.FindType(typeof(ME.ECS.World)),
                    tests = new [] {
                        new TestInfo(TestMethod.Recycle),
                    },
                });
                var states = this.FindTypes(typeof(ME.ECS.State));
                foreach (var state in states) {
                    collectedComponents.Add(new TestItem() {
                        type = state,
                        tests = new [] {
                            new TestInfo(TestMethod.CopyFrom),
                            new TestInfo(TestMethod.Recycle),
                        },
                    });
                }

                return collectedComponents;

            }, false);

            this.view = view;
            container.Add(view);
            
            this.rootVisualElement.Add(container);
            
        }

        public void Update() {

            this.view?.Update();

        }

    }

}