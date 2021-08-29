using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace ME.ECSEditor.Tools {

    public interface ITestBase {

        int priority { get; }
        bool IsValid(System.Type type);

    }

    public interface ITestEqualsChecker : ITestBase {

        bool CheckEquals(ITester tester, object obj1, object obj2, string path);

    }
    
    public interface ITestGenerator : ITestBase {

        object Fill(ITester tester, object instance, System.Type type);
        
    }

    public interface ITester {

        void FillRandom(object instance);
        void FillRandom(ref object instance);
        object FillRandom(object instance, System.Type type);
        bool IsInstanceEquals(object obj1, object obj2, string path);

    }

    public class StatesChecker : EditorWindow, ITester {

        [MenuItem("ME.ECS/Tools/States Checker...")]
        public static void Show() {
            
            var window = StatesChecker.CreateInstance<StatesChecker>();
            window.titleContent = new UnityEngine.GUIContent("States Checker");
            ((EditorWindow)window).Show();
            
        }

        public enum Status {

            None = 0,
            Failed,
            Passed,

        }

        public class TestItem {

            public System.Type type;
            public Status test1Status;
            public Status test2Status;
            
            public Button element;
            
        }

        private static string TEST1_CAPTION = "CopyFrom";
        private static string TEST2_CAPTION = "Recycle";
        private List<TestItem> collectedComponents = new List<TestItem>();
        private List<ITestGenerator> generators = new List<ITestGenerator>();
        private List<ITestEqualsChecker> equalsCheckers = new List<ITestEqualsChecker>();
        
        private VisualElement scrollView;
        private Label status;

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

            {
                this.generators.Clear();
                this.equalsCheckers.Clear();
                var asms = System.AppDomain.CurrentDomain.GetAssemblies();
                foreach (var asm in asms) {

                    var types = asm.GetTypes();
                    foreach (var type in types) {

                        if (typeof(ITestGenerator).IsAssignableFrom(type) == true && (type.IsClass == true || type.IsValueType == true)) {
                            
                            this.generators.Add((ITestGenerator)System.Activator.CreateInstance(type));
                                
                        }
                        
                        if (typeof(ITestEqualsChecker).IsAssignableFrom(type) == true && (type.IsClass == true || type.IsValueType == true)) {
                            
                            this.equalsCheckers.Add((ITestEqualsChecker)System.Activator.CreateInstance(type));
                            
                        }

                    }

                }
                this.generators = this.generators.OrderBy(x => x.priority).ToList();
                this.equalsCheckers = this.equalsCheckers.OrderBy(x => x.priority).ToList();

                this.collectedComponents.Clear();
                this.collectedComponents.Add(new TestItem() {
                    type = typeof(ME.ECS.StructComponentsContainer),
                });
                this.collectedComponents.Add(new TestItem() {
                    type = typeof(ME.ECS.Storage),
                });
                this.collectedComponents.Add(new TestItem() {
                    type = typeof(ME.ECS.FiltersStorage),
                });
                this.collectedComponents.Add(new TestItem() {
                    type = this.FindType(typeof(ME.ECS.World)),
                });
                var states = this.FindTypes(typeof(ME.ECS.State));
                foreach (var state in states) {
                    this.collectedComponents.Add(new TestItem() {
                        type = state,
                    });
                }

                var runAllButton = new Button(this.RunAllTests);
                runAllButton.text = "Run All Tests";
                runAllButton.AddToClassList("run-all-button");
                container.Add(runAllButton);

                var status = new Label();
                container.Add(status);
                status.AddToClassList("summary-status");
                status.text = $"All: -, Passed: -, Failed: -";
                this.status = status;

            }

            {
                this.scrollView = new ScrollView();
                this.scrollView.AddToClassList("scroll-view");
                container.Add(this.scrollView);
            }
            
            this.DrawComponents(this.scrollView.contentContainer);

            this.rootVisualElement.Add(container);
            
        }

        public void Update() {

            var passed = 0;
            var failed = 0;
            var all = 0;
            foreach (var item in this.collectedComponents) {
                
                if (item.test1Status == Status.Failed) ++failed;
                if (item.test1Status == Status.Passed) ++passed;
                ++all;
                
                if (item.test2Status == Status.Failed) ++failed;
                if (item.test2Status == Status.Passed) ++passed;
                ++all;

            }
            
            this.status.text = $"All: {all}, Passed: {passed}, Failed: {failed}";

        }

        private class ThreadState {

            public TestItem testItem;

        }
        
        private void DrawComponents(VisualElement container) {
            
            container.Clear();

            foreach (var testItem in this.collectedComponents) {
                        
                var itemContainer = new Button();
                testItem.element = itemContainer;
                
                itemContainer.RegisterCallback<ClickEvent, TestItem>((evt, t) => {
                    
                    this.RunTest(t);
                    
                }, testItem);
                itemContainer.AddToClassList("component-item");
                {
                    var item = new Label();
                    item.AddToClassList("caption");
                    item.text = testItem.type.Name;
                    itemContainer.Add(item);
                }
                {
                    var item = new Label();
                    item.AddToClassList("type");
                    item.text = testItem.type.FullName;
                    itemContainer.Add(item);
                }
                {
                    var item = new Label();
                    item.AddToClassList("fields");
                    item.text = $"Fields: {testItem.type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Length}";
                    itemContainer.Add(item);
                }
                
                var tests = new VisualElement();
                tests.AddToClassList("tests");
                {
                    var item = new VisualElement();
                    item.AddToClassList("test1");
                    item.AddToClassList("test-label");
                    var status0 = new Label();
                    status0.AddToClassList("test-status-running");
                    status0.text = $"{StatesChecker.TEST1_CAPTION} Test: Running";
                    item.Add(status0);
                    var status1 = new Label();
                    status1.AddToClassList("test-status-none");
                    status1.text = $"{StatesChecker.TEST1_CAPTION} Test: No Status";
                    item.Add(status1);
                    var status2 = new Label();
                    status2.AddToClassList("test-status-passed");
                    status2.text = $"{StatesChecker.TEST1_CAPTION} Test: Passed";
                    item.Add(status2);
                    var status3 = new Label();
                    status3.AddToClassList("test-status-failed");
                    status3.text = $"{StatesChecker.TEST1_CAPTION} Test: Failed";
                    item.Add(status3);
                    tests.Add(item);
                }
                {
                    var item = new VisualElement();
                    item.AddToClassList("test2");
                    item.AddToClassList("test-label");
                    var status0 = new Label();
                    status0.AddToClassList("test-status-running");
                    status0.text = $"{StatesChecker.TEST2_CAPTION} Test: Running";
                    item.Add(status0);
                    var status1 = new Label();
                    status1.AddToClassList("test-status-none");
                    status1.text = $"{StatesChecker.TEST2_CAPTION} Test: No Status";
                    item.Add(status1);
                    var status2 = new Label();
                    status2.AddToClassList("test-status-passed");
                    status2.text = $"{StatesChecker.TEST2_CAPTION} Test: Passed";
                    item.Add(status2);
                    var status3 = new Label();
                    status3.AddToClassList("test-status-failed");
                    status3.text = $"{StatesChecker.TEST2_CAPTION} Test: Failed";
                    item.Add(status3);
                    tests.Add(item);
                }
                itemContainer.Add(tests);
                
                container.Add(itemContainer);
                
            }
            
        }

        private bool IsIgnored(System.Reflection.FieldInfo fieldInfo) {

            var attrs = fieldInfo.GetCustomAttributes(typeof(ME.ECS.Extensions.TestIgnoreAttribute), inherit: false);
            if (attrs.Length > 0) return true;

            return false;

        }

        public void FillRandom(ref object instance) {

            var type = instance.GetType();
            foreach (var generator in this.generators) {

                if (generator.IsValid(type) == true) {

                    instance = generator.Fill(this, instance, type);
                    return;

                }

            }
            
            this.FillRandom(instance);

        }
        
        public void FillRandom(object instance) {

            var type = instance.GetType();
            var fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            for (int i = 0; i < fields.Length; ++i) {

                var field = fields[i];
                var val = field.GetValue(instance);
                var result = this.FillRandom(val, field.FieldType);
                field.SetValue(instance, result);

            }
                
        }

        public object FillRandom(object instance, System.Type type) {
            
            var result = instance;
            if (type == typeof(System.IntPtr)) {

                result = null;
                return result;

            } else if (type == typeof(string)) {

                var str = "RANDOM STRING DATA";
                result = str;
                return result;

            } else if (type.IsInterface == true || type.IsAbstract == true) {

                result = null;
                return result;

            } else if (type.IsPrimitive == true) {

                var rnd = new System.Random();
                if (type == typeof(int)) {

                    result = rnd.Next(int.MinValue, int.MaxValue);

                } else if (type == typeof(float)) {

                    result = (float)rnd.NextDouble();

                } else if (type == typeof(double)) {

                    result = (double)rnd.NextDouble();

                } else if (type == typeof(long)) {

                    result = (long)rnd.Next();

                } else if (type == typeof(short)) {

                    result = (short)rnd.Next(short.MinValue, short.MaxValue);

                } else if (type == typeof(byte)) {

                    result = (byte)rnd.Next(byte.MinValue, byte.MaxValue);

                } else if (type == typeof(sbyte)) {

                    result = (sbyte)rnd.Next(sbyte.MinValue, sbyte.MaxValue);

                } else if (type == typeof(ushort)) {

                    result = (ushort)rnd.Next(ushort.MinValue, ushort.MaxValue);

                } else if (type == typeof(ulong)) {

                    result = (ulong)rnd.Next(0, int.MaxValue);

                } else if (type == typeof(uint)) {

                    result = (uint)rnd.Next(0, int.MaxValue);

                } else if (type == typeof(bool)) {

                    result = rnd.Next(0, 100) >= 50 ? true : false;

                } else if (type == typeof(decimal)) {

                    result = (decimal)rnd.NextDouble();

                }
                
                return result;

            }

            foreach (var generator in this.generators) {

                if (generator.IsValid(type) == true) {

                    return generator.Fill(this, instance, type);

                }
                
            }
            
            return result;

        }

        public void RunAllTests() {

            System.Threading.ThreadPool.QueueUserWorkItem((state) => {

                var i = 0;
                foreach (var testItem in this.collectedComponents) {

                    this.Prepare(testItem);
                    var statuses = this.RunNoThread(testItem.type, testItem.element, (idx) => this.OnTestBegin(testItem, idx), (idx, status) => this.OnTestEnd(testItem, idx, status));
                    testItem.test1Status = statuses.Item1;
                    testItem.test2Status = statuses.Item2;
                    this.Complete(testItem);
                    ++i;
                    
                }
                
            });

        }

        public void RunTest(TestItem testItem) {

            var data = new ThreadState() {
                testItem = testItem,
            };

            var st = data;
            var type = st.testItem.type;
            var container = st.testItem.element;
            this.Prepare(st.testItem);
            var statuses = this.RunNoThread(type, container, (idx) => this.OnTestBegin(st.testItem, idx), (idx, status) => this.OnTestEnd(st.testItem, idx, status));
            st.testItem.test1Status = statuses.Item1;
            st.testItem.test2Status = statuses.Item2;
            this.Complete(st.testItem);
            
            /*
            System.Threading.ThreadPool.QueueUserWorkItem((state) => {

                var st = (ThreadState)state;
                var type = st.testItem.type;
                var container = st.testItem.element;
                this.Prepare(st.testItem);
                var statuses = this.RunNoThread(type, container, (idx) => this.OnTestBegin(st.testItem, idx), (idx, status) => this.OnTestEnd(st.testItem, idx, status));
                st.testItem.test1Status = statuses.Item1;
                st.testItem.test2Status = statuses.Item2;
                this.Complete(st.testItem);
                
            }, data);
            */

        }

        private void OnTestBegin(TestItem test, int index) {
            
            var container = test.element;
            var lbl = container.Q(className: "test" + (index + 1));
            lbl.AddToClassList("status-checking");
            
        }

        private void OnTestEnd(TestItem test, int index, Status status) {
            
            var container = test.element;
            var lbl = container.Q(className: "test" + (index + 1));
            lbl.RemoveFromClassList("status-checking");
            if (status == Status.Passed) {
                lbl.AddToClassList("status-success");
            } else if (status == Status.Failed) {
                lbl.AddToClassList("status-failed");
            }

        }

        private void Prepare(TestItem test) {

            var container = test.element;
            var lbl1 = container.Q(className: "test1");
            lbl1.RemoveFromClassList("status-success");
            lbl1.RemoveFromClassList("status-failed");
            var lbl2 = container.Q(className: "test2");
            lbl2.RemoveFromClassList("status-success");
            lbl2.RemoveFromClassList("status-failed");

        }

        private void Complete(TestItem test) {
            
            var container = test.element;
            
            var lbl1 = container.Q(className: "test1");
            if (test.test1Status == Status.Passed) {
                lbl1.AddToClassList("status-success");
            } else if (test.test1Status == Status.Failed) {
                lbl1.AddToClassList("status-failed");
            }

            var lbl2 = container.Q(className: "test2");
            if (test.test2Status == Status.Passed) {
                lbl2.AddToClassList("status-success");
            } else if (test.test2Status == Status.Failed) {
                lbl2.AddToClassList("status-failed");
            }

        }

        private System.Tuple<Status, Status> RunNoThread(System.Type type, Button container, System.Action<int> onTestBegin, System.Action<int, Status> onTestEnd) {
            
            container.AddToClassList("status-checking");

            var test1Status = Status.None;
            var test2Status = Status.None;

            var poolMode = ME.ECS.Pools.isActive;
            ME.ECS.Pools.isActive = false;
            //try {

                {
                    
                    var defaultInstance = System.Activator.CreateInstance(type);
                    var targetInstance = System.Activator.CreateInstance(type);
                    var instance = System.Activator.CreateInstance(type);
                    
                    try {

                        this.FillRandom(ref targetInstance);

                    } catch (System.Exception ex) {
                        UnityEngine.Debug.LogException(ex);
                        container.RemoveFromClassList("status-checking");
                        return new System.Tuple<Status, Status>(Status.Failed, Status.Failed);
                    }

                    {
                        try {
                            
                            var methods = type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                            foreach (var method in methods) {

                                if (method.Name.EndsWith("Recycle") == true &&
                                    method.GetParameters().Length == 0) {

                                    method.Invoke(defaultInstance, null);
                                    break;

                                }

                            }
                            
                        } catch (System.Exception ex) {
                            UnityEngine.Debug.LogException(ex);
                        }

                    }

                    try {

                        this.FillRandom(ref instance);

                    } catch (System.Exception ex) {
                        UnityEngine.Debug.LogException(ex);
                        container.RemoveFromClassList("status-checking");
                        return new System.Tuple<Status, Status>(Status.Failed, Status.Failed);
                    }

                    onTestBegin.Invoke(0);

                    {
                        try {

                            var methodFound = false;
                            var methods = type.GetMethods(
                                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                            foreach (var method in methods) {

                                if (method.Name.EndsWith("CopyFrom") == true &&
                                    method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType.IsAssignableFrom(type) == true) {

                                    method.Invoke(targetInstance, new[] {
                                        instance,
                                    });
                                    methodFound = true;
                                    break;

                                }

                            }

                            if (methodFound == true) {

                                if (this.IsInstanceEquals(instance, targetInstance) == true) {
                                    test1Status = Status.Passed;
                                } else {
                                    test1Status = Status.Failed;
                                }

                            }

                        } catch (System.Exception ex) {
                            UnityEngine.Debug.LogException(ex);
                            test1Status = Status.Failed;
                        }

                    }

                    onTestEnd.Invoke(0, test1Status);

                    onTestBegin.Invoke(1);
                    
                    {
                        try {
                            
                            var methods = type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                            foreach (var method in methods) {

                                if (method.Name.EndsWith("Recycle") == true &&
                                    method.GetParameters().Length == 0) {

                                    method.Invoke(instance, null);
                                    break;

                                }

                            }
                            
                            if (this.IsInstanceEquals(instance, defaultInstance) == true) {
                                test2Status = Status.Passed;
                            } else {
                                test2Status = Status.Failed;
                            }
                            
                        } catch (System.Exception ex) {
                            UnityEngine.Debug.LogException(ex);
                            test2Status = Status.Failed;
                        }

                    }
                    
                    onTestEnd.Invoke(1, test2Status);

                }

            /*} catch (System.Exception ex) {
                
                Debug.LogError(ex);
                
            }*/
            ME.ECS.Pools.isActive = poolMode;

            container.RemoveFromClassList("status-checking");

            return new System.Tuple<Status, Status>(test1Status, test2Status);

        }

        public struct ObjectInfo {

            public object parent;
            public string path;
            public object obj1;
            public object obj2;

            public ObjectInfo(object obj1, object obj2, string path, object parent) {
                this.obj1 = obj1;
                this.obj2 = obj2;
                this.path = path;
                this.parent = parent;
            }

        }

        public bool IsInstanceEquals(object obj1Test, object obj2Test, string rootPath = null) {

            var queue = new Queue<ObjectInfo>();
            queue.Enqueue(new ObjectInfo(obj1Test, obj2Test, rootPath ?? string.Empty, null));
            while (queue.Count > 0) {

                var kv = queue.Dequeue();
                var obj1 = kv.obj1;
                var obj2 = kv.obj2;
                var path = kv.path;
                
                if (obj1 == null && obj2 == null) continue;
                if (obj1 == null && obj2 != null) {
                    UnityEngine.Debug.LogWarning($"Test failed. Field `{kv.path}` has different values: `{obj1}` and `{obj2}`.");
                    return false;
                }

                if (obj1 != null && obj2 == null) {
                    UnityEngine.Debug.LogWarning($"Test failed. Field `{kv.path}` has different values: `{obj1}` and `{obj2}`.");
                    return false;
                }

                var t1 = obj1.GetType();
                var t2 = obj2.GetType();
                if (t1.IsPointer == true) continue;
                if (t1 == typeof(System.IntPtr)) continue;
                if (t1 != t2) {
                    UnityEngine.Debug.LogWarning($"Test failed. Field `{kv.path}` has different types: `{t1}` and `{t2}`.");
                    return false;
                }
                
                if (t1.IsPrimitive == true) {

                    if (object.Equals(obj1, obj2) == false) {
                        UnityEngine.Debug.LogWarning($"Test failed. Field `{kv.path}` has different values: `{obj1}` and `{obj2}`.");
                        return false;
                    }
                    continue;

                }

                var generatorIsEquals = false;
                foreach (var generator in this.equalsCheckers) {

                    if (generator.IsValid(t1) == true) {

                        if (generator.CheckEquals(this, obj1, obj2, kv.path) == false) {

                            UnityEngine.Debug.LogWarning($"Test failed. Field `{kv.path}` has different values in IEqualsChecker `{generator}`.");
                            return false;

                        }

                        generatorIsEquals = true;
                        break;

                    }
                    
                }
                
                if (generatorIsEquals == true) continue;
                
                var fields1 = t1.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                var fields2 = t2.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (fields1.Length != fields2.Length) {
                    UnityEngine.Debug.LogWarning($"Test failed. Field `{kv.path}` has different fieldsCount `{fields1.Length}` and `{fields2.Length}`.");
                    return false;
                }

                for (int i = 0; i < fields1.Length; ++i) {

                    var f1 = fields1[i];
                    var f2 = fields2[i];
                    if (f1.FieldType != f2.FieldType) {
                        UnityEngine.Debug.LogWarning($"Test failed. Field `{kv.path}` has different types: `{f1.FieldType}` and `{f2.FieldType}`.");
                        return false;
                    }
                    if (f1.FieldType.IsPointer == true) continue;
                    if (f1.FieldType == typeof(System.IntPtr)) continue;
                    if (this.IsIgnored(f1) == true) continue;

                    var v1 = f1.GetValue(obj1);
                    var v2 = f2.GetValue(obj2);
                    queue.Enqueue(new ObjectInfo(v1, v2, path + "/" + f1.Name, obj1));

                }

            }

            return true;

        }
        
    }

}