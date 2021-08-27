using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace ME.ECSEditor.Tools {

    public class CheckCopyableComponents : EditorWindow {

        [MenuItem("ME.ECS/Tools/Copyable Components Checker...")]
        public static void Show() {
            
            var window = CheckCopyableComponents.CreateInstance<CheckCopyableComponents>();
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
        
        private VisualElement scrollView;
        private Label status;

        public void OnEnable() {
            
            var container = new VisualElement();
            container.styleSheets.Add(EditorUtilities.Load<StyleSheet>("ECSEditor/Tools/Styles.uss", isRequired: true));

            {
                var collectButton = new Button(() => {
                    
                    this.collectedComponents.Clear();
                    var asms = System.AppDomain.CurrentDomain.GetAssemblies();
                    foreach (var asm in asms) {

                        var types = asm.GetTypes();
                        foreach (var type in types) {

                            if (type.IsValueType == true && typeof(ME.ECS.IStructCopyableBase).IsAssignableFrom(type) == true) {
                            
                                this.collectedComponents.Add(new TestItem() {
                                    type = type,
                                });
                                
                            }

                        }

                    }

                    this.DrawComponents(this.scrollView.contentContainer);

                });
                collectButton.text = "Collect Components";
                collectButton.AddToClassList("collect-button");
                container.Add(collectButton);
                
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
                    item.text = $"Fields: {testItem.type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Length}";
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
                    status0.text = $"{CheckCopyableComponents.TEST1_CAPTION} Test: Running";
                    item.Add(status0);
                    var status1 = new Label();
                    status1.AddToClassList("test-status-none");
                    status1.text = $"{CheckCopyableComponents.TEST1_CAPTION} Test: No Status";
                    item.Add(status1);
                    var status2 = new Label();
                    status2.AddToClassList("test-status-passed");
                    status2.text = $"{CheckCopyableComponents.TEST1_CAPTION} Test: Passed";
                    item.Add(status2);
                    var status3 = new Label();
                    status3.AddToClassList("test-status-failed");
                    status3.text = $"{CheckCopyableComponents.TEST1_CAPTION} Test: Failed";
                    item.Add(status3);
                    tests.Add(item);
                }
                {
                    var item = new VisualElement();
                    item.AddToClassList("test2");
                    item.AddToClassList("test-label");
                    var status0 = new Label();
                    status0.AddToClassList("test-status-running");
                    status0.text = $"{CheckCopyableComponents.TEST2_CAPTION} Test: Running";
                    item.Add(status0);
                    var status1 = new Label();
                    status1.AddToClassList("test-status-none");
                    status1.text = $"{CheckCopyableComponents.TEST2_CAPTION} Test: No Status";
                    item.Add(status1);
                    var status2 = new Label();
                    status2.AddToClassList("test-status-passed");
                    status2.text = $"{CheckCopyableComponents.TEST2_CAPTION} Test: Passed";
                    item.Add(status2);
                    var status3 = new Label();
                    status3.AddToClassList("test-status-failed");
                    status3.text = $"{CheckCopyableComponents.TEST2_CAPTION} Test: Failed";
                    item.Add(status3);
                    tests.Add(item);
                }
                itemContainer.Add(tests);
                
                container.Add(itemContainer);
                
            }
            
        }

        private void FillRandom(object instance) {

            var type = instance.GetType();
            var fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            for (int i = 0; i < fields.Length; ++i) {

                var field = fields[i];
                var val = field.GetValue(instance);
                var result = this.FillRandom(val, field.FieldType);
                field.SetValue(instance, result);

            }
                
        }

        private object FillRandom(object instance, System.Type type) {
            
            var result = instance;
            if (type == typeof(string)) {

                var str = "RANDOM STRING DATA";
                result = str;

            } else if (typeof(ME.ECS.Collections.IBufferArray).IsAssignableFrom(type) == true) {
                
                var genericType = type.GenericTypeArguments[0]; 
                var listType = typeof(ME.ECS.Collections.BufferArray<>).MakeGenericType(genericType);
                var arr = System.Array.CreateInstance(genericType, 16);
                for (int j = 0; j < 16; ++j) {
                    if (genericType.IsAbstract == false) {
                        var item = System.Activator.CreateInstance(genericType);
                        item = this.FillRandom(item, genericType);
                        arr.SetValue(item, j);
                    } else {
                        arr.SetValue(null, j);
                    }
                }

                var ctor = listType.GetConstructor(
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public,
                    null, new System.Type[] { arr.GetType(), typeof(int) }, null);
                result = ctor.Invoke(new object[] { arr, 16 });
                
            } else if (typeof(IDictionary).IsAssignableFrom(type) == true && type.GetGenericTypeDefinition() == typeof(Dictionary<,>)) {
                
                var genericTypeKey = type.GenericTypeArguments[0];
                var genericTypeValue = type.GenericTypeArguments[1];
                var listType = typeof(Dictionary<,>).MakeGenericType(genericTypeKey, genericTypeValue);
                var valIEnumerable = (IDictionary)System.Activator.CreateInstance(listType, new object[] { 16 });
                for (int j = 0; j < 16; ++j) {
                    object k = null;
                    object v = null;
                    if (genericTypeKey.IsAbstract == false) {
                        var itemKey = System.Activator.CreateInstance(genericTypeKey);
                        itemKey = this.FillRandom(itemKey, genericTypeKey);
                        k = itemKey;
                        if (genericTypeValue.IsAbstract == false) {
                            var itemValue = System.Activator.CreateInstance(genericTypeValue);
                            itemValue = this.FillRandom(itemValue, genericTypeValue);
                            v = itemValue;
                        }
                    } else {
                        if (genericTypeValue.IsAbstract == false) {
                            var itemValue = System.Activator.CreateInstance(genericTypeValue);
                            itemValue = this.FillRandom(itemValue, genericTypeValue);
                            v = itemValue;
                        }
                    }
                    if (valIEnumerable.Contains(k) == false) valIEnumerable.Add(k, v);
                }

                result = valIEnumerable;

            } else if (typeof(IList).IsAssignableFrom(type) == true && type.GetGenericTypeDefinition() == typeof(List<>)) {
                
                var genericType = type.GenericTypeArguments[0];
                var listType = typeof(List<>).MakeGenericType(genericType);
                var valIEnumerable = (IList)System.Activator.CreateInstance(listType, new object[] { 16 });
                for (int j = 0; j < 16; ++j) {
                    if (genericType.IsAbstract == false) {
                        var item = System.Activator.CreateInstance(genericType);
                        item = this.FillRandom(item, genericType);
                        valIEnumerable.Add(item);
                    } else {
                        valIEnumerable.Add(null);
                    }
                }

                result = valIEnumerable;

            } else if (typeof(IEnumerable).IsAssignableFrom(type) == true) {

                if (type.GetGenericTypeDefinition() == typeof(ME.ECS.Collections.ListCopyable<>)) {

                    var genericType = type.GenericTypeArguments[0];
                    var listType = typeof(ME.ECS.Collections.ListCopyable<>).MakeGenericType(genericType);
                    var valIEnumerable = (IEnumerable)System.Activator.CreateInstance(listType, new object[] { 16 });
                    for (int j = 0; j < 16; ++j) {
                        var item = System.Activator.CreateInstance(genericType);
                        item = this.FillRandom(item, genericType);
                        ((ME.ECS.Collections.IListCopyableBase)valIEnumerable).Add(item);
                    }

                    result = valIEnumerable;

                }
                
            } else if (type.IsArray == true && type.GetArrayRank() == 1) {

                var genericType = type.GetElementType();
                var arr = System.Array.CreateInstance(genericType, 16);
                for (int j = 0; j < 16; ++j) {
                    if (genericType.IsAbstract == false) {
                        var item = System.Activator.CreateInstance(genericType);
                        item = this.FillRandom(item, genericType);
                        arr.SetValue(item, j);
                    } else {
                        arr.SetValue(null, j);
                    }
                }

                result = arr;

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

            } else if (type.IsPrimitive == false && type.IsValueType == true) {

                this.FillRandom(instance);
                result = instance;

            }

            return result;

        }

        public void RunAllTests() {

            System.Threading.ThreadPool.QueueUserWorkItem((state) => {

                var i = 0;
                foreach (var testItem in this.collectedComponents) {

                    var statuses = this.RunNoThread(testItem.type, testItem.element);
                    testItem.test1Status = statuses.Item1;
                    testItem.test2Status = statuses.Item2;
                    ++i;
                    
                }
                
            });

        }

        public void RunTest(TestItem testItem) {
            
            System.Threading.ThreadPool.QueueUserWorkItem((state) => {

                var st = (ThreadState)state;
                var type = st.testItem.type;
                var container = st.testItem.element;
                var statuses = this.RunNoThread(type, container);
                st.testItem.test1Status = statuses.Item1;
                st.testItem.test2Status = statuses.Item2;

            }, new ThreadState() {
                testItem = testItem,
            });

        }

        private System.Tuple<Status, Status> RunNoThread(System.Type type, Button container) {
            
            container.AddToClassList("status-checking");

            var test1Status = Status.None;
            var test2Status = Status.None;

            try {

                {
                    var defaultInstance = (ME.ECS.IStructCopyableBase)System.Activator.CreateInstance(type);
                    var targetInstance = (ME.ECS.IStructCopyableBase)System.Activator.CreateInstance(type);
                    var instance = (ME.ECS.IStructCopyableBase)System.Activator.CreateInstance(type);
                    this.FillRandom(instance);

                    var lbl1 = container.Q(className: "test1");
                    var lbl2 = container.Q(className: "test2");
                    lbl1.AddToClassList("status-checking");

                    {
                        var methods = type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                        foreach (var method in methods) {

                            if (method.Name.EndsWith("CopyFrom") == true) {

                                method.Invoke(targetInstance, new[] {
                                    instance,
                                });
                                break;

                            }

                        }

                        if (this.IsInstancesEquals(instance, targetInstance) == true) {
                            lbl1.AddToClassList("status-success");
                            test1Status = Status.Passed;
                        } else {
                            lbl1.AddToClassList("status-failed");
                            test1Status = Status.Passed;
                        }
                    }

                    lbl1.RemoveFromClassList("status-checking");

                    lbl2.AddToClassList("status-checking");

                    {
                        var methods = type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                        foreach (var method in methods) {

                            if (method.Name.EndsWith("Recycle") == true) {

                                method.Invoke(instance, null);
                                break;

                            }

                        }

                        if (this.IsInstancesEquals(instance, defaultInstance) == true) {
                            lbl2.AddToClassList("status-success");
                            test2Status = Status.Passed;
                        } else {
                            lbl2.AddToClassList("status-failed");
                            test2Status = Status.Failed;
                        }
                    }

                    lbl2.RemoveFromClassList("status-checking");

                }

            } catch (System.Exception ex) {
                
                Debug.LogError(ex);
                
            }

            container.RemoveFromClassList("status-checking");

            return new System.Tuple<Status, Status>(test1Status, test2Status);

        }

        private bool IsInstancesEquals(object obj1, object obj2) {
            
            var packedSource = ME.ECS.Serializer.Serializer.Pack(obj1);
            var packedTarget = ME.ECS.Serializer.Serializer.Pack(obj2);

            var isEquals = (packedSource.Length == packedTarget.Length);
            if (isEquals == true) {

                for (int i = 0; i < packedSource.Length; ++i) {

                    if (packedTarget[i] != packedSource[i]) {

                        return false;

                    }

                }

            }

            return true;

        }
        
    }

}