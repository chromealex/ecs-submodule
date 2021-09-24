using System.Linq;
using ME.ECS.Extensions;

namespace ME.ECSEditor.Tools {

    using System.Collections.Generic;
    
    public interface ITestBase {

        int priority { get; }
        bool IsValid(System.Type type);

    }

    public interface ITestEqualsChecker : ITestBase {

        bool CheckEquals(System.Reflection.MemberInfo rootType, ITester tester, object obj1, object obj2, string path, bool objectEqualsCheck);

    }
    
    public interface ITestGenerator : ITestBase {

        object Fill(ITester tester, object instance, System.Type type);
        
    }

    public interface ITester {

        void FillRandom(object instance);
        void FillRandom(ref object instance);
        object FillRandom(object instance, System.Type type);
        bool IsInstanceEquals(System.Reflection.MemberInfo rootType, object obj1, object obj2, string path, bool objectEqualsCheck);

    }

    public enum Status {

        None = 0,
        Failed,
        Passed,

    }

    public enum TestMethod {

        CopyFrom,
        Recycle,
        DirectCopy,
        Dispose,

    }

    public class TestInfo {

        public Status status;
        public TestMethod method;

        public TestInfo(TestMethod method) {
            
            this.method = method;
            
        }

    }

    public class TestItem {

        public System.Type type;
        public TestInfo[] tests;
        
    }

    public struct Info {

        public int all;
        public int passed;
        public int failed;

    }

    public class Tester : ITester {

        private List<TestItem> tests = new List<TestItem>();
        private List<ITestGenerator> generators = new List<ITestGenerator>();
        private List<ITestEqualsChecker> equalsCheckers = new List<ITestEqualsChecker>();

        public void SetTests(List<TestItem> tests) {
            
            this.tests.AddRange(tests);
            
        }

        public Info GetInfo() {

            var info = new Info();
            foreach (var item in this.tests) {

                foreach (var test in item.tests) {

                    if (test.status == Status.Passed) ++info.passed;
                    if (test.status == Status.Failed) ++info.failed;
                    ++info.all;

                }
                
            }
            
            return info;
            
        }

        public void Collect() {
        
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

        }

        public void RunAllTests(
            System.Action<int, TestItem> onPrepareTest,
            System.Action<int, TestItem> onCompleteTest,
            System.Action<int, TestItem, int> onTestMethodBegin,
            System.Action<int, TestItem, int, Status> onTestMethodEnd) {
            
            System.Threading.ThreadPool.QueueUserWorkItem((state) => {

                var i = 0;
                foreach (var testItem in this.tests) {

                    onPrepareTest.Invoke(i, testItem);
                    this.RunNoThread(i, testItem, onTestMethodBegin, onTestMethodEnd);
                    onCompleteTest.Invoke(i, testItem);
                    ++i;
                    
                }
                
            });

        }

        private class ThreadState {

            public TestItem testItem;
            public int index;

        }

        public void RunTest(TestItem testItem,
                            System.Action<int, TestItem> onPrepareTest,
                            System.Action<int, TestItem> onCompleteTest,
                            System.Action<int, TestItem, int> onTestMethodBegin,
                            System.Action<int, TestItem, int, Status> onTestMethodEnd) {
            
            var data = new ThreadState() {
                testItem = testItem,
                index = this.tests.IndexOf(testItem),
            };
            
            System.Threading.ThreadPool.QueueUserWorkItem((state) => {

                var st = (ThreadState)state;
                onPrepareTest.Invoke(st.index, st.testItem);
                this.RunNoThread(st.index, st.testItem, onTestMethodBegin, onTestMethodEnd);
                onCompleteTest.Invoke(st.index, st.testItem);
                
            }, data);
            
        }

        private bool RunTestByMethod(TestMethod testMethod, Context context) {

            var type = context.testItem.type;
            
            if (testMethod == TestMethod.DirectCopy) {

                try {
                
                    var methodFound = false;
                    var methods = type.GetMethods(
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                    foreach (var method in methods) {

                        if (method.Name.EndsWith("MemberwiseClone") == true &&
                            method.GetParameters().Length == 0) {

                            context.targetInstance = method.Invoke(context.instance, null);
                            methodFound = true;
                            break;

                        }

                    }

                    if (methodFound == true) {

                        if (this.IsInstanceEquals(type, context.instance, context.targetInstance, rootPath: type.Name, objectEqualsCheck: true) == true) {
                            return true;
                        } else {
                            return false;
                        }

                    }
                } catch (System.Exception ex) {
                    UnityEngine.Debug.LogException(ex);
                    return false;
                }
                
            } else if (testMethod == TestMethod.CopyFrom) {
                
                try {

                    var methodFound = false;
                    var methods = type.GetMethods(
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                    foreach (var method in methods) {

                        if (method.Name.EndsWith("CopyFrom") == true &&
                            method.GetParameters().Length == 1) {

                            var fName = method.GetParameters()[0].ParameterType.FullName.Replace("&", "");
                            if (System.Type.GetType(fName + ", " + method.GetParameters()[0].ParameterType.Assembly.FullName).IsAssignableFrom(type) == true) {

                                method.Invoke(context.targetInstance, new[] {
                                    context.instance,
                                });
                                methodFound = true;
                                break;

                            }

                        }

                    }

                    if (methodFound == true) {

                        if (this.IsInstanceEquals(type, context.instance, context.targetInstance, rootPath: type.Name) == true) {
                            return true;
                        } else {
                            return false;
                        }

                    }

                } catch (System.Exception ex) {
                    UnityEngine.Debug.LogException(ex);
                    return false;
                }

            } else if (testMethod == TestMethod.Recycle) {
                             
                 try {
                     
                     var methods = type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                     foreach (var method in methods) {
 
                         if (method.Name.EndsWith("Recycle") == true &&
                             method.GetParameters().Length == 0) {
 
                             method.Invoke(context.instance, null);
                             break;
 
                         }
 
                     }
                     
                     if (this.IsInstanceEquals(type, context.instance, context.defaultInstance, rootPath: type.Name) == true) {
                         return true;
                     } else {
                         return false;
                     }
                     
                 } catch (System.Exception ex) {
                     UnityEngine.Debug.LogException(ex);
                     return false;
                 }
                 
            } else if (testMethod == TestMethod.Dispose) {

                try {

                    var methods = type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                    foreach (var method in methods) {

                        if (method.Name.EndsWith("Dispose") == true &&
                            method.GetParameters().Length == 0) {

                            method.Invoke(context.instance, null);
                            break;

                        }

                    }

                    if (this.IsInstanceEquals(type, context.instance, context.defaultInstance, rootPath: type.Name) == true) {
                        return true;
                    } else {
                        return false;
                    }

                } catch (System.Exception ex) {
                    UnityEngine.Debug.LogException(ex);
                    return false;
                }

            }
            
            return false;

        }

        public class Context {

            public object defaultInstance;
            public object targetInstance;
            public object instance;
            public TestItem testItem;

        }

        private void SetAllFailed(TestItem testItem) {

            for (int i = 0; i < testItem.tests.Length; ++i) {

                var test = testItem.tests[i];
                test.status = Status.Failed;
                
            }

        }
        
        private bool RunNoThread(int index, TestItem testItem, System.Action<int, TestItem, int> onTestBegin, System.Action<int, TestItem, int, Status> onTestEnd) {

            var type = testItem.type;
            var poolMode = ME.ECS.Pools.isActive;
            ME.ECS.Pools.isActive = false;
            
            {
                
                var context = new Context() {
                    defaultInstance = System.Activator.CreateInstance(type),
                    targetInstance = System.Activator.CreateInstance(type),
                    instance = System.Activator.CreateInstance(type),
                    testItem = testItem,
                };
                
                try {

                    this.FillRandom(ref context.targetInstance);

                } catch (System.Exception ex) {
                    this.SetAllFailed(testItem);
                    UnityEngine.Debug.LogException(ex);
                    return false;
                }

                {
                    try {
                        
                        var methods = type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                        foreach (var method in methods) {

                            if (method.Name.EndsWith("Recycle") == true &&
                                method.GetParameters().Length == 0) {

                                method.Invoke(context.defaultInstance, null);
                                break;

                            }

                        }
                        
                    } catch (System.Exception ex) {
                        this.SetAllFailed(testItem);
                        UnityEngine.Debug.LogException(ex);
                    }

                }

                try {

                    this.FillRandom(ref context.instance);

                } catch (System.Exception ex) {
                    UnityEngine.Debug.LogException(ex);
                    this.SetAllFailed(testItem);
                    return false;
                }

                for (int i = 0; i < testItem.tests.Length; ++i) {

                    var test = testItem.tests[i];
                    onTestBegin.Invoke(index, testItem, i);
                    {
                        if (this.RunTestByMethod(test.method, context) == true) {
                            test.status = Status.Passed;
                        } else {
                            test.status = Status.Failed;
                        }
                    }
                    onTestEnd.Invoke(index, testItem, i, test.status);

                }

            }

            ME.ECS.Pools.isActive = poolMode;

            return true;

        }
        
        #region EQUALS
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

        public bool CheckShallowCopy(System.Reflection.FieldInfo rootType, string path, object obj1, object obj2, out bool skip) {

            skip = false;
            if (rootType.FieldType == typeof(string)) return true;

            var ignore = rootType.GetCustomAttributes(typeof(ME.ECS.GeneratorIgnoreManagedType), inherit: true);
            if (ignore.Length > 0) {
                skip = true;
                return true;
            }

            if (obj1 != null) {
                
                ignore = obj1.GetType().GetCustomAttributes(typeof(ME.ECS.GeneratorIgnoreManagedType), inherit: true);
                if (ignore.Length > 0) {
                    skip = true;
                    return true;
                }
                
            }

            if (obj1 != null && object.ReferenceEquals(obj1, obj2) == true) {
                
                if (rootType.FieldType.IsClass == false) return true;

                UnityEngine.Debug.LogWarning($"Test failed. Field `{path}` has ref copy problem: `{obj1}` and `{obj2}`.");
                return false;
            }

            return true;

        }

        public bool IsInstanceEquals(System.Reflection.MemberInfo rootType, object obj1Test, object obj2Test, string rootPath = null, bool objectEqualsCheck = false) {

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

                        if (generator.CheckEquals(rootType, this, obj1, obj2, kv.path, objectEqualsCheck) == false) {

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
                    var skip = false;
                    if (objectEqualsCheck == true && this.CheckShallowCopy(f1, path, v1, v2, out skip) == false) return false;
                    if (skip == false) queue.Enqueue(new ObjectInfo(v1, v2, path + "/" + f1.Name, obj1));

                }

            }

            return true;

        }
        #endregion
        
        #region FILL RANDOM
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

                    result = (ulong)rnd.Next(1, int.MaxValue);

                } else if (type == typeof(uint)) {

                    result = (uint)rnd.Next(1, int.MaxValue);

                } else if (type == typeof(bool)) {

                    result = true;

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
        #endregion
                
    }

}