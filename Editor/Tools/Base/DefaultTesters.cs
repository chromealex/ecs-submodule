namespace ME.ECSEditor.Tools {

    namespace Testers {

        public class StackTraceClass : ITestGenerator {

            public int priority => 100;

            public bool IsValid(System.Type type) {

                return type == typeof(System.Diagnostics.StackTrace);

            }

            public object Fill(ITester tester, object instance, System.Type type) {
            
                var item = Utils.CreateInstance(type);
                return item;
                
            }
            
        }

        public class Struct : ITestGenerator {

            public int priority => 100;

            public bool IsValid(System.Type type) {

                return type.IsPrimitive == false && type.IsValueType == true;

            }

            public object Fill(ITester tester, object instance, System.Type type) {
            
                var item = Utils.CreateInstance(type);
                tester.FillRandom(item);
                return item;
                
            }
            
        }

        public class UnityObject : ITestGenerator {

            public int priority => 5;

            public bool IsValid(System.Type type) {

                return type.IsClass == true && typeof(UnityEngine.Object).IsAssignableFrom(type);

            }

            public object Fill(ITester tester, object instance, System.Type type) {
                
                return null;

            }

        }

        public class Class : ITestGenerator {

            public int priority => 100;

            public bool IsValid(System.Type type) {

                return type.IsClass == true && type.IsAbstract == false;

            }

            public object Fill(ITester tester, object instance, System.Type type) {
            
                var ctor = type.GetConstructor(
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public,
                    null, new System.Type[] {}, null);
                if (ctor != null) {

                    var item = ctor.Invoke(new object[] {});
                    tester.FillRandom(item);
                    return item;

                }
                
                return null;

            }

        }

        public class EnumerableGeneric : ITestGenerator {

            public int priority => 2;
            
            public bool IsValid(System.Type type) {

                return typeof(System.Collections.IEnumerable).IsAssignableFrom(type) == true &&
                       type.IsGenericType == true && type.IsValueType == false;

            }

            public object Fill(ITester tester, object instance, System.Type type) {

                if (type.GetGenericTypeDefinition() == typeof(ME.ECS.Collections.ListCopyable<>)) {

                    var genericType = type.GenericTypeArguments[0];
                    var listType = type.GetGenericTypeDefinition().MakeGenericType(genericType);
                    var valIEnumerable = (System.Collections.IEnumerable)System.Activator.CreateInstance(listType, new object[] { 16 });
                    for (int j = 0; j < 16; ++j) {
                        var item = Utils.CreateInstance(genericType);
                        item = tester.FillRandom(item, genericType);
                        ((ME.ECS.Collections.IListCopyableBase)valIEnumerable).Add(item);
                    }

                    return valIEnumerable;

                }

                return null;

            }

        }

        public class DictionaryGeneric : ITestGenerator, ITestEqualsChecker {

            public int priority => 1;

            public bool IsValid(System.Type type) {
                
                return typeof(System.Collections.Generic.IDictionary<,>).IsAssignableFrom(type) == true;
                
            }

            public bool CheckEquals(System.Reflection.MemberInfo rootType, ITester tester, object obj1, object obj2, string path, bool objectEqualsCheck) {
                
                var dic1 = (System.Collections.IDictionary)obj1;
                var dic2 = (System.Collections.IDictionary)obj2;

                if (dic1.Count != dic2.Count) return false;

                foreach (var key in dic1.Keys) {
                    
                    if (dic2.Contains(key) == false) return false;
                    
                }

                foreach (var key in dic2.Keys) {
                    
                    if (dic1.Contains(key) == false) return false;
                    
                }

                var list = new System.Collections.Generic.List<object>();
                foreach (var val1 in dic1.Values) {

                    list.Add(val1);

                }
                
                var i = 0;
                foreach (var val2 in dic2.Values) {

                    if (tester.IsInstanceEquals(rootType, list[i], val2, path, objectEqualsCheck) == false) return false;
                    ++i;

                }

                return true;

            }

            public object Fill(ITester tester, object instance, System.Type type) {
            
                var genericTypeKey = type.GenericTypeArguments[0];
                var genericTypeValue = type.GenericTypeArguments[1];
                var listType = type.GetGenericTypeDefinition().MakeGenericType(genericTypeKey, genericTypeValue);
                var valIEnumerable = (System.Collections.IDictionary)System.Activator.CreateInstance(listType, new object[] { 16 });
                for (int j = 0; j < 16; ++j) {
                    object k = null;
                    object v = null;
                    if (genericTypeKey.IsAbstract == false) {
                        var itemKey = Utils.CreateInstance(genericTypeKey);
                        itemKey = tester.FillRandom(itemKey, genericTypeKey);
                        k = itemKey;
                        if (genericTypeValue.IsAbstract == false) {
                            var itemValue = Utils.CreateInstance(genericTypeValue);
                            itemValue = tester.FillRandom(itemValue, genericTypeValue);
                            v = itemValue;
                        }
                    } else {
                        if (genericTypeValue.IsAbstract == false) {
                            var itemValue = Utils.CreateInstance(genericTypeValue);
                            itemValue = tester.FillRandom(itemValue, genericTypeValue);
                            v = itemValue;
                        }
                    }
                    if (valIEnumerable.Contains(k) == false) valIEnumerable.Add(k, v);
                }

                return valIEnumerable;

            }

        }

        public class DictionaryCopyableGeneric : ITestGenerator, ITestEqualsChecker {

            public int priority => 1;

            public bool IsValid(System.Type type) {
                
                return typeof(ME.ECS.Collections.DictionaryCopyable<,>).IsAssignableFrom(type) == true;
                
            }

            public bool CheckEquals(System.Reflection.MemberInfo rootType, ITester tester, object obj1, object obj2, string path, bool objectEqualsCheck) {
                
                var dic1 = (System.Collections.IDictionary)obj1;
                var dic2 = (System.Collections.IDictionary)obj2;

                if (dic1.Count != dic2.Count) return false;

                foreach (var key in dic1.Keys) {
                    
                    if (dic2.Contains(key) == false) return false;
                    
                }

                foreach (var key in dic2.Keys) {
                    
                    if (dic1.Contains(key) == false) return false;
                    
                }

                var list = new System.Collections.Generic.List<object>();
                foreach (var val1 in dic1.Values) {

                    list.Add(val1);

                }
                
                var i = 0;
                foreach (var val2 in dic2.Values) {

                    if (tester.IsInstanceEquals(rootType, list[i], val2, path, objectEqualsCheck) == false) return false;
                    ++i;

                }

                return true;

            }

            public object Fill(ITester tester, object instance, System.Type type) {
            
                var genericTypeKey = type.GenericTypeArguments[0];
                var genericTypeValue = type.GenericTypeArguments[1];
                var listType = type.GetGenericTypeDefinition().MakeGenericType(genericTypeKey, genericTypeValue);
                var valIEnumerable = (System.Collections.IDictionary)System.Activator.CreateInstance(listType, new object[] { 16 });
                for (int j = 0; j < 16; ++j) {
                    object k = null;
                    object v = null;
                    if (genericTypeKey.IsAbstract == false) {
                        var itemKey = Utils.CreateInstance(genericTypeKey);
                        itemKey = tester.FillRandom(itemKey, genericTypeKey);
                        k = itemKey;
                        if (genericTypeValue.IsAbstract == false) {
                            var itemValue = Utils.CreateInstance(genericTypeValue);
                            itemValue = tester.FillRandom(itemValue, genericTypeValue);
                            v = itemValue;
                        }
                    } else {
                        if (genericTypeValue.IsAbstract == false) {
                            var itemValue = Utils.CreateInstance(genericTypeValue);
                            itemValue = tester.FillRandom(itemValue, genericTypeValue);
                            v = itemValue;
                        }
                    }
                    if (valIEnumerable.Contains(k) == false) valIEnumerable.Add(k, v);
                }

                return valIEnumerable;

            }

        }

        public class ArrayRank1 : ITestGenerator, ITestEqualsChecker {

            public int priority => 1;

            public bool IsValid(System.Type type) {
                
                return type.IsArray == true && type.GetArrayRank() == 1;
                
            }

            public object Fill(ITester tester, object instance, System.Type type) {

                return Utils.CreateArray(tester, type, type.GetElementType());
                
            }

            public bool CheckEquals(System.Reflection.MemberInfo rootType, ITester tester, object obj1, object obj2, string path, bool objectEqualsCheck) {
                
                var dic1 = (System.Array)obj1;
                var dic2 = (System.Array)obj2;

                if (dic1.Length != dic2.Length) return false;

                for (int i = 0; i < dic1.Length; ++i) {

                    if (tester.IsInstanceEquals(rootType, dic1.GetValue(i), dic2.GetValue(i), path, objectEqualsCheck) == false) return false;

                }

                return true;

            }

        }

        public class ListGeneric : ITestGenerator, ITestEqualsChecker {

            public int priority => 2;

            public bool IsValid(System.Type type) {

                return typeof(System.Collections.IList).IsAssignableFrom(type) == true && type.IsGenericType == true;

            }

            public bool CheckEquals(System.Reflection.MemberInfo rootType, ITester tester, object obj1, object obj2, string path, bool objectEqualsCheck) {
                
                var dic1 = (System.Collections.IList)obj1;
                var dic2 = (System.Collections.IList)obj2;

                if (dic1.Count != dic2.Count) return false;

                for (int i = 0; i < dic1.Count; ++i) {

                    if (tester.IsInstanceEquals(rootType, dic1[i], dic2[i], path, objectEqualsCheck) == false) return false;

                }

                return true;

            }

            public object Fill(ITester tester, object instance, System.Type type) {
                
                var genericType = type.GenericTypeArguments[0];
                var listType = type.GetGenericTypeDefinition().MakeGenericType(genericType);
                var ctor = listType.GetConstructor(
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public,
                    null, new System.Type[] { typeof(int) }, null);
                System.Collections.IList valIEnumerable = null;
                if (ctor != null) {
                    valIEnumerable = (System.Collections.IList)System.Activator.CreateInstance(listType, new object[] { 16 });
                } else {
                    ctor = listType.GetConstructor(
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public,
                        null, new System.Type[] {}, null);
                    valIEnumerable = (System.Collections.IList)ctor.Invoke(new object[] {});
                }

                for (int j = 0; j < 16; ++j) {
                    if (genericType.IsAbstract == false) {
                        var item = Utils.CreateInstance(genericType);
                        item = tester.FillRandom(item, genericType);
                        valIEnumerable.Add(item);
                    } else {
                        valIEnumerable.Add(null);
                    }
                }

                return valIEnumerable;

            }

        }

        public class BufferArrayGenerator : ITestGenerator, ITestEqualsChecker {

            public int priority => 2;

            public bool IsValid(System.Type type) {

                return typeof(ME.ECS.Collections.IBufferArray).IsAssignableFrom(type);

            }

            public bool CheckEquals(System.Reflection.MemberInfo rootType, ITester tester, object obj1, object obj2, string path, bool objectEqualsCheck) {
                
                var dic1 = (ME.ECS.Collections.IBufferArray)obj1;
                var dic2 = (ME.ECS.Collections.IBufferArray)obj2;

                if (dic1.Count != dic2.Count) return false;

                var arr1 = dic1.GetArray();
                var arr2 = dic2.GetArray();
                if (arr1 == null && arr2 == null) return true;
                if (arr1 != null && arr2 == null) return false;
                if (arr1 == null && arr2 != null) return false;

                for (int i = 0; i < arr1.Length; ++i) {

                    if (tester.IsInstanceEquals(rootType, arr1.GetValue(i), arr2.GetValue(i), path, objectEqualsCheck) == false) return false;

                }

                return true;

            }

            public object Fill(ITester tester, object instance, System.Type type) {

                return Utils.FillGenericType(tester, type, new object[] { 16 });
                
            }

        }

        public class NativeBufferArrayGenerator : ITestGenerator, ITestEqualsChecker {

            public int priority => 1;

            public bool IsValid(System.Type type) {

                if (type.IsGenericType == true && type.GetGenericTypeDefinition() == typeof(ME.ECS.Collections.NativeBufferArray<>)) return true;
                return typeof(ME.ECS.Collections.NativeBufferArray<>).IsAssignableFrom(type);

            }

            public bool CheckEquals(System.Reflection.MemberInfo rootType, ITester tester, object obj1, object obj2, string path, bool objectEqualsCheck) {
                
                var dic1 = (ME.ECS.Collections.IBufferArray)obj1;
                var dic2 = (ME.ECS.Collections.IBufferArray)obj2;

                if (dic1.Count != dic2.Count) return false;

                var arr1 = dic1.GetArray();
                var arr2 = dic2.GetArray();
                if (arr1 == null && arr2 == null) return true;
                if (arr1 != null && arr2 == null) return false;
                if (arr1 == null && arr2 != null) return false;
                
                for (int i = 0; i < arr1.Length; ++i) {

                    if (tester.IsInstanceEquals(rootType, arr1.GetValue(i), arr2.GetValue(i), path, objectEqualsCheck) == false) return false;

                }

                return true;

            }

            public object Fill(ITester tester, object instance, System.Type type) {

                return Utils.FillGenericType(tester, type, new object[1] { Utils.CreateArray(tester, type) });
                
            }

        }

        public class NativeArrayGenerator : ITestGenerator, ITestEqualsChecker {

            public int priority => 1;

            public bool IsValid(System.Type type) {

                if (type.IsGenericType == true && type.GetGenericTypeDefinition() == typeof(Unity.Collections.NativeArray<>)) return true;
                return typeof(Unity.Collections.NativeArray<>).IsAssignableFrom(type);

            }

            public bool CheckEquals(System.Reflection.MemberInfo rootType, ITester tester, object obj1, object obj2, string path, bool objectEqualsCheck) {
                
                var dic1 = (System.Collections.IEnumerable)obj1;
                var dic2 = (System.Collections.IEnumerable)obj2;

                var list1 = new System.Collections.Generic.List<object>();
                var cnt1 = 0;
                foreach (var item in dic1) {

                    ++cnt1;
                    list1.Add(item);

                }
                
                var list2 = new System.Collections.Generic.List<object>();
                var cnt2 = 0;
                foreach (var item in dic2) {

                    ++cnt2;
                    list2.Add(item);

                }
                
                if (cnt1 != cnt2) return false;

                for (int i = 0; i < list1.Count; ++i) {

                    if (tester.IsInstanceEquals(rootType, list1[i], list2[i], path, objectEqualsCheck) == false) return false;

                }

                return true;

            }

            public object Fill(ITester tester, object instance, System.Type type) {

                return Utils.FillGenericType(tester, type, new object[] { Unity.Collections.Allocator.Persistent });
                
            }

        }

    }
    
}