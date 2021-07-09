using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace ME.ECS.DataConfigGenerator {

    // Really simple JSON parser in ~300 lines
    // - Attempts to parse JSON files with minimal GC allocation
    // - Nice and simple "[1,2,3]".FromJson<List<int>>() API
    // - Classes and structs can be parsed too!
    //      class Foo { public int Value; }
    //      "{\"Value\":10}".FromJson<Foo>()
    // - Can parse JSON without type information into Dictionary<string,object> and List<object> e.g.
    //      "[1,2,3]".FromJson<object>().GetType() == typeof(List<object>)
    //      "{\"Value\":10}".FromJson<object>().GetType() == typeof(Dictionary<string,object>)
    // - No JIT Emit support to support AOT compilation on iOS
    // - Attempts are made to NOT throw an exception if the JSON is corrupted or invalid: returns null instead.
    // - Only public fields and property setters on classes/structs will be written to
    //
    // Limitations:
    // - No JIT Emit support to parse structures quickly
    // - Limited to parsing <2GB JSON files (due to int.MaxValue)
    // - Parsing of abstract classes or interfaces is NOT supported and will throw an exception.
    public static class JSONParser {

        [ThreadStatic]
        private static Stack<List<string>> splitArrayPool;
        [ThreadStatic]
        private static StringBuilder stringBuilder;
        [ThreadStatic]
        private static Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoCache;
        [ThreadStatic]
        private static Dictionary<Type, Dictionary<string, PropertyInfo>> propertyInfoCache;

        public static object FromJson(string json, System.Type type) {
            // Initialize, if needed, the ThreadStatic variables
            if (JSONParser.propertyInfoCache == null) {
                JSONParser.propertyInfoCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
            }

            if (JSONParser.fieldInfoCache == null) {
                JSONParser.fieldInfoCache = new Dictionary<Type, Dictionary<string, FieldInfo>>();
            }

            if (JSONParser.stringBuilder == null) {
                JSONParser.stringBuilder = new StringBuilder();
            }

            if (JSONParser.splitArrayPool == null) {
                JSONParser.splitArrayPool = new Stack<List<string>>();
            }

            //Remove all whitespace not within strings to make parsing simpler
            JSONParser.stringBuilder.Length = 0;
            for (var i = 0; i < json.Length; i++) {
                var c = json[i];
                if (c == '"') {
                    i = JSONParser.AppendUntilStringEnd(true, i, json);
                    continue;
                }

                if (char.IsWhiteSpace(c)) {
                    continue;
                }

                JSONParser.stringBuilder.Append(c);
            }

            //Parse the thing!
            return JSONParser.ParseValue(type, JSONParser.stringBuilder.ToString());
        }

        public static T FromJson<T>(string json) {
            return (T)FromJson(json, typeof(T));
        }

        private static int AppendUntilStringEnd(bool appendEscapeCharacter, int startIdx, string json) {
            JSONParser.stringBuilder.Append(json[startIdx]);
            for (var i = startIdx + 1; i < json.Length; i++) {
                if (json[i] == '\\') {
                    if (appendEscapeCharacter) {
                        JSONParser.stringBuilder.Append(json[i]);
                    }

                    JSONParser.stringBuilder.Append(json[i + 1]);
                    i++; //Skip next character as it is escaped
                } else if (json[i] == '"') {
                    JSONParser.stringBuilder.Append(json[i]);
                    return i;
                } else {
                    JSONParser.stringBuilder.Append(json[i]);
                }
            }

            return json.Length - 1;
        }

        //Splits { <value>:<value>, <value>:<value> } and [ <value>, <value> ] into a list of <value> strings
        private static List<string> Split(string json) {
            var splitArray = JSONParser.splitArrayPool.Count > 0 ? JSONParser.splitArrayPool.Pop() : new List<string>();
            splitArray.Clear();
            if (json.Length == 2) {
                return splitArray;
            }

            var parseDepth = 0;
            JSONParser.stringBuilder.Length = 0;
            for (var i = 1; i < json.Length - 1; i++) {
                switch (json[i]) {
                    case '[':
                    case '{':
                        parseDepth++;
                        break;

                    case ']':
                    case '}':
                        parseDepth--;
                        break;

                    case '"':
                        i = JSONParser.AppendUntilStringEnd(true, i, json);
                        continue;

                    case ',':
                    case ':':
                        if (parseDepth == 0) {
                            splitArray.Add(JSONParser.stringBuilder.ToString());
                            JSONParser.stringBuilder.Length = 0;
                            continue;
                        }

                        break;
                }

                JSONParser.stringBuilder.Append(json[i]);
            }

            splitArray.Add(JSONParser.stringBuilder.ToString());

            return splitArray;
        }

        internal static object ParseValue(Type type, string json) {
            {
                var resJson = json;
                if (resJson[0] == '"') {
                    resJson = resJson.Substring(1, resJson.Length - 2);
                }

                foreach (var parser in ME.ECS.DataConfigGenerator.DataConfigGenerator.parsers) {

                    if (parser.IsValid(type) == true) {

                        try {
                            if (parser.Parse(resJson, type, out var result) == true) {
                                return result;
                            }

                            //if (parser.Parse(data, componentType, fieldName, fieldType, out result) == true) return true;
                        } catch (System.Exception ex) {
                            UnityEngine.Debug.LogError($"Parser `{parser}` failed with exception: {ex.Message}");
                        }

                    }

                }
            }

            if (type == typeof(string)) {
                if (json.Length <= 2) {
                    return string.Empty;
                }

                var parseStringBuilder = new StringBuilder(json.Length);
                for (var i = 1; i < json.Length - 1; ++i) {
                    if (json[i] == '\\' && i + 1 < json.Length - 1) {
                        var j = "\"\\nrtbf/".IndexOf(json[i + 1]);
                        if (j >= 0) {
                            parseStringBuilder.Append("\"\\\n\r\t\b\f/"[j]);
                            ++i;
                            continue;
                        }

                        if (json[i + 1] == 'u' && i + 5 < json.Length - 1) {
                            UInt32 c = 0;
                            if (UInt32.TryParse(json.Substring(i + 2, 4), System.Globalization.NumberStyles.AllowHexSpecifier, null, out c)) {
                                parseStringBuilder.Append((char)c);
                                i += 5;
                                continue;
                            }
                        }
                    }

                    parseStringBuilder.Append(json[i]);
                }

                return parseStringBuilder.ToString();
            }

            if (type.IsPrimitive) {
                var result = Convert.ChangeType(json, type, System.Globalization.CultureInfo.InvariantCulture);
                return result;
            }

            if (type == typeof(decimal)) {
                decimal result;
                decimal.TryParse(json, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);
                return result;
            }

            if (json == "null") {
                return null;
            }

            if (type.IsEnum) {
                if (json[0] == '"') {
                    json = json.Substring(1, json.Length - 2);
                }

                try {
                    return Enum.Parse(type, json, false);
                } catch {
                    return 0;
                }
            }

            if (type.IsArray) {
                var arrayType = type.GetElementType();
                if (json[0] != '[' || json[json.Length - 1] != ']') {
                    return null;
                }

                var elems = JSONParser.Split(json);
                var newArray = Array.CreateInstance(arrayType, elems.Count);
                for (var i = 0; i < elems.Count; i++) {
                    newArray.SetValue(JSONParser.ParseValue(arrayType, elems[i]), i);
                }

                JSONParser.splitArrayPool.Push(elems);
                return newArray;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) {
                var listType = type.GetGenericArguments()[0];
                if (json[0] != '[' || json[json.Length - 1] != ']') {
                    return null;
                }

                var elems = JSONParser.Split(json);
                var list = (IList)type.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { elems.Count });
                for (var i = 0; i < elems.Count; i++) {
                    list.Add(JSONParser.ParseValue(listType, elems[i]));
                }

                JSONParser.splitArrayPool.Push(elems);
                return list;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>)) {
                Type keyType, valueType;
                {
                    var args = type.GetGenericArguments();
                    keyType = args[0];
                    valueType = args[1];
                }

                //Refuse to parse dictionary keys that aren't of type string
                if (keyType != typeof(string)) {
                    return null;
                }

                //Must be a valid dictionary element
                if (json[0] != '{' || json[json.Length - 1] != '}') {
                    return null;
                }

                //The list is split into key/value pairs only, this means the split must be divisible by 2 to be valid JSON
                var elems = JSONParser.Split(json);
                if (elems.Count % 2 != 0) {
                    return null;
                }

                var dictionary = (IDictionary)type.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { elems.Count / 2 });
                for (var i = 0; i < elems.Count; i += 2) {
                    if (elems[i].Length <= 2) {
                        continue;
                    }

                    var keyValue = elems[i].Substring(1, elems[i].Length - 2);
                    var val = JSONParser.ParseValue(valueType, elems[i + 1]);
                    dictionary[keyValue] = val;
                }

                return dictionary;
            }

            if (type == typeof(object)) {
                return JSONParser.ParseAnonymousValue(json);
            }

            if (json[0] == '{' && json[json.Length - 1] == '}') {
                return JSONParser.ParseObject(type, json);
            }

            return null;
        }

        private static object ParseAnonymousValue(string json) {
            if (json.Length == 0) {
                return null;
            }

            if (json[0] == '{' && json[json.Length - 1] == '}') {
                var elems = JSONParser.Split(json);
                if (elems.Count % 2 != 0) {
                    return null;
                }

                var dict = new Dictionary<string, object>(elems.Count / 2);
                for (var i = 0; i < elems.Count; i += 2) {
                    dict[elems[i].Substring(1, elems[i].Length - 2)] = JSONParser.ParseAnonymousValue(elems[i + 1]);
                }

                return dict;
            }

            if (json[0] == '[' && json[json.Length - 1] == ']') {
                var items = JSONParser.Split(json);
                var finalList = new List<object>(items.Count);
                for (var i = 0; i < items.Count; i++) {
                    finalList.Add(JSONParser.ParseAnonymousValue(items[i]));
                }

                return finalList;
            }

            if (json[0] == '"' && json[json.Length - 1] == '"') {
                var str = json.Substring(1, json.Length - 2);
                return str.Replace("\\", string.Empty);
            }

            if (char.IsDigit(json[0]) || json[0] == '-') {
                if (json.Contains(".")) {
                    double result;
                    double.TryParse(json, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);
                    return result;
                } else {
                    int result;
                    int.TryParse(json, out result);
                    return result;
                }
            }

            if (json == "true") {
                return true;
            }

            if (json == "false") {
                return false;
            }

            // handles json == "null" as well as invalid JSON
            return null;
        }

        private static Dictionary<string, T> CreateMemberNameDictionary<T>(T[] members) where T : MemberInfo {
            var nameToMember = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < members.Length; i++) {
                var member = members[i];
                if (member.IsDefined(typeof(IgnoreDataMemberAttribute), true)) {
                    continue;
                }

                var name = member.Name;
                if (member.IsDefined(typeof(DataMemberAttribute), true)) {
                    var dataMemberAttribute = (DataMemberAttribute)Attribute.GetCustomAttribute(member, typeof(DataMemberAttribute), true);
                    if (!string.IsNullOrEmpty(dataMemberAttribute.Name)) {
                        name = dataMemberAttribute.Name;
                    }
                }

                nameToMember.Add(name, member);
            }

            return nameToMember;
        }

        private static object ParseObject(Type type, string json) {
            var instance = FormatterServices.GetUninitializedObject(type);

            //The list is split into key/value pairs only, this means the split must be divisible by 2 to be valid JSON
            var elems = JSONParser.Split(json);
            if (elems.Count % 2 != 0) {
                return instance;
            }

            Dictionary<string, FieldInfo> nameToField;
            Dictionary<string, PropertyInfo> nameToProperty;
            if (!JSONParser.fieldInfoCache.TryGetValue(type, out nameToField)) {
                nameToField = JSONParser.CreateMemberNameDictionary(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy));
                JSONParser.fieldInfoCache.Add(type, nameToField);
            }

            if (!JSONParser.propertyInfoCache.TryGetValue(type, out nameToProperty)) {
                nameToProperty = JSONParser.CreateMemberNameDictionary(type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy));
                JSONParser.propertyInfoCache.Add(type, nameToProperty);
            }

            for (var i = 0; i < elems.Count; i += 2) {
                if (elems[i].Length <= 2) {
                    continue;
                }

                var key = elems[i].Substring(1, elems[i].Length - 2);
                var value = elems[i + 1];

                FieldInfo fieldInfo;
                PropertyInfo propertyInfo;
                if (nameToField.TryGetValue(key, out fieldInfo)) {
                    fieldInfo.SetValue(instance, JSONParser.ParseValue(fieldInfo.FieldType, value));
                } else if (nameToProperty.TryGetValue(key, out propertyInfo)) {
                    propertyInfo.SetValue(instance, JSONParser.ParseValue(propertyInfo.PropertyType, value), null);
                }
            }

            return instance;
        }

    }

}