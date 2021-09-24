namespace ME.ECSEditor.Tools {

    public static class Utils {

        public static object FillGenericType(ITester tester, System.Type type, object[] ctorValues) {

            object result = null;
            var genericType = type.GenericTypeArguments[0]; 
            var listType = type.GetGenericTypeDefinition().MakeGenericType(genericType);
            var arr = Utils.CreateArray(tester, type);

            var newTypes = (ctorValues == null ? null : new System.Type[ctorValues.Length + 1]);
            var newValues = (ctorValues == null ? null : new object[ctorValues.Length + 1]);
            if (ctorValues != null) {
                
                newTypes[0] = arr.GetType();
                for (int i = 0; i < ctorValues.Length; ++i) {

                    newTypes[i + 1] = ctorValues[i].GetType();

                }
                
                newValues[0] = arr;
                System.Array.Copy(ctorValues, 0, newValues, 1, ctorValues.Length);

            }

            var ctor = listType.GetConstructor(
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public,
                null, newTypes, null);
            if (ctor != null) {
                result = ctor.Invoke(newValues);
            } else {
                ctor = listType.GetConstructor(
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public,
                    null, new System.Type[] { arr.GetType() }, null);
                result = ctor.Invoke(new object[] { arr });
            }

            return result;

        }

        public static object CreateArray(ITester tester, System.Type type, System.Type elementType = null) {
            
            var genericType = elementType ?? type.GenericTypeArguments[0];
            var arr = System.Array.CreateInstance(genericType, 16);
            for (int j = 0; j < 16; ++j) {
                if (genericType.IsAbstract == false) {
                    var item = Utils.CreateInstance(genericType);
                    item = tester.FillRandom(item, genericType);
                    arr.SetValue(item, j);
                } else {
                    arr.SetValue(null, j);
                }
            }

            return arr;

        }

        public static object CreateInstance(System.Type type) {

            if (type.IsValueType == true) return System.Activator.CreateInstance(type);
            
            var ctorLess = type.GetConstructor(
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public,
                null, new System.Type[] {}, null);
            if (ctorLess != null) {
                return System.Activator.CreateInstance(type);
            }

            return null;

        }

    }

}