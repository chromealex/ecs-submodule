using System.Linq;

namespace ME.ECS.BlackBox {

    public static class ReflectionHelper {

        public static System.Collections.Generic.Dictionary<System.Type, System.Reflection.FieldInfo[]> fieldInfoCache = new System.Collections.Generic.Dictionary<System.Type, System.Reflection.FieldInfo[]>();

        public static System.Reflection.FieldInfo[] GetCachedFields(this System.Type type,
                                                                     System.Reflection.BindingFlags flags =
                                                                         System.Reflection.BindingFlags.Instance |
                                                                         System.Reflection.BindingFlags.Public |
                                                                         System.Reflection.BindingFlags.NonPublic) {

            if (ReflectionHelper.fieldInfoCache.TryGetValue(type, out var fieldInfos) == false) {

                var fieldInfosArr = System.Linq.Enumerable.Cast<System.Reflection.FieldInfo>(System.Linq.Enumerable.Where(type.GetFields(flags), f => f.IsPublic == true));
                
                fieldInfos = fieldInfosArr
                             .OrderBy(x => x.Name)
                             .ToArray();

                ReflectionHelper.fieldInfoCache.Add(type, fieldInfos);

            }

            return fieldInfos;
        }

        public static System.Reflection.FieldInfo GetFieldViaPath (this System.Type type, object root, string path) {
            var flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
            var parent = type;
            var fi = parent.GetField(path, flags);
            var paths = path.Split('.');
     
            for (int i = 0; i < paths.Length; i++)
            {
                fi = parent.GetField(paths[i], flags);
                if (fi != null)
                {
                    // there are only two container field type that can be serialized:
                    // Array and List<T>
                    if (fi.FieldType.IsArray)
                    {
                        parent = fi.FieldType.GetElementType();
                        i += 2;
                        continue;
                    }
     
                    if (fi.FieldType.IsGenericType)
                    {
                        parent = fi.FieldType.GetGenericArguments()[0];
                        i += 2;
                        continue;
                    }

                    parent = fi.ReflectedType;//fi.GetValue(root).GetType();//.FieldType;
                }
                else
                {
                    break;
                }
     
            }
            if (fi == null)
            {
                if (type.BaseType != null)
                {
                    return GetFieldViaPath(type.BaseType, root, path);
                }
                else
                {
                    return null;
                }
            }
            return fi;
        }
        
    }

}