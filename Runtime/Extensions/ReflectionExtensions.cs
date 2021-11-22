using System.Linq;

namespace ME.ECS.Extensions {

    public class TestIgnoreAttribute : System.Attribute {

        

    }
    
    public static class ReflectionEx {

        public static System.Type GetUnderlyingType(this System.Reflection.MemberInfo member) {
            System.Type type;
            switch (member.MemberType) {
                case System.Reflection.MemberTypes.Field:
                    type = ((System.Reflection.FieldInfo)member).FieldType;
                    break;

                case System.Reflection.MemberTypes.Property:
                    type = ((System.Reflection.PropertyInfo)member).PropertyType;
                    break;

                case System.Reflection.MemberTypes.Event:
                    type = ((System.Reflection.EventInfo)member).EventHandlerType;
                    break;

                default:
                    throw new System.ArgumentException("member must be if type FieldInfo, PropertyInfo or EventInfo", "member");
            }

            return System.Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// Gets fields and properties into one array.
        /// Order of properties / fields will be preserved in order of appearance in class / struct. (MetadataToken is used for sorting such cases)
        /// </summary>
        /// <param name="type">Type from which to get</param>
        /// <returns>array of fields and properties</returns>
        public static System.Reflection.MemberInfo[] GetFieldsAndProperties(this System.Type type) {
            var fps = new System.Collections.Generic.List<System.Reflection.MemberInfo>();
            fps.AddRange(type.GetFields());
            fps.AddRange(type.GetProperties());
            fps = fps.OrderBy(x => x.MetadataToken).ToList();
            return fps.ToArray();
        }

        public static object GetValue(this System.Reflection.MemberInfo member, object target) {
            if (member is System.Reflection.PropertyInfo) {
                return (member as System.Reflection.PropertyInfo).GetValue(target, null);
            } else if (member is System.Reflection.FieldInfo) {
                return (member as System.Reflection.FieldInfo).GetValue(target);
            } else {
                throw new System.Exception("member must be either PropertyInfo or FieldInfo");
            }
        }

        public static void SetValue(this System.Reflection.MemberInfo member, object target, object value) {
            if (member is System.Reflection.PropertyInfo) {
                (member as System.Reflection.PropertyInfo).SetValue(target, value, null);
            } else if (member is System.Reflection.FieldInfo) {
                (member as System.Reflection.FieldInfo).SetValue(target, value);
            } else {
                throw new System.Exception("destinationMember must be either PropertyInfo or FieldInfo");
            }
        }

        /// <summary>
        /// Deep clones specific object.
        /// Analogue can be found here: https://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-an-object-in-net-c-specifically
        /// This is now improved version (list support added)
        /// </summary>
        /// <param name="obj">object to be cloned</param>
        /// <returns>full copy of object.</returns>
        public static object DeepClone(this object obj) {
            if (obj == null) {
                return null;
            }

            var type = obj.GetType();

            if (obj is System.Collections.IList) {
                var list = (System.Collections.IList)obj;
                var newlist = (System.Collections.IList)System.Activator.CreateInstance(obj.GetType(), list.Count);

                foreach (var elem in list) {
                    newlist.Add(ReflectionEx.DeepClone(elem));
                }

                return newlist;
            } //if

            if (type.IsPrimitive || type == typeof(string)) {
                return obj;
            } else if (type.IsArray) {
                var elementType = System.Type.GetType(type.FullName.Replace("[]", string.Empty));
                var array = obj as System.Array;
                var copied = System.Array.CreateInstance(elementType, array.Length);

                for (var i = 0; i < array.Length; i++) {
                    copied.SetValue(ReflectionEx.DeepClone(array.GetValue(i)), i);
                }

                return System.Convert.ChangeType(copied, obj.GetType());
            } else if (type.IsClass || type.IsValueType) {
                var toret = System.Activator.CreateInstance(obj.GetType());

                var fields = type.GetFieldsAndProperties();
                foreach (var field in fields) {
                    // Don't clone parent back-reference classes. (Using special kind of naming 'parent' 
                    // to indicate child's parent class.
                    if (field.Name == "parent") {
                        continue;
                    }

                    var fieldValue = field.GetValue(obj);

                    if (fieldValue == null) {
                        continue;
                    }

                    field.SetValue(toret, fieldValue /*DeepClone(fieldValue)*/);
                }

                return toret;
            } else {
                // Don't know that type, don't know how to clone it.
                if (System.Diagnostics.Debugger.IsAttached) {
                    System.Diagnostics.Debugger.Break();
                }

                return null;
            }
        } //DeepClone

    }

}