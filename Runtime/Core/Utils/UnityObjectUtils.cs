#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using UnityEngine;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class UnityObjectUtils {

        public static unsafe int GetObjectSize(object obj, System.Collections.Generic.HashSet<object> visited = null, bool collectUnityObjects = false) {

            if (visited == null) visited = new System.Collections.Generic.HashSet<object>();

            var ptr = obj;
            if (visited.Contains(ptr) == true) return 0;
            visited.Add(ptr);

            var pointerSize = System.IntPtr.Size;
            var size = 0;
            if (obj == null) return pointerSize;

            var charSize = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<char>();
            
            var type = obj.GetType();
            if (collectUnityObjects == false && typeof(UnityEngine.Object).IsAssignableFrom(type) == true) {

                size += pointerSize;

            } else if (type.IsEnum == true) {

                size += sizeof(int);

            } else if (type.IsPointer == true) {

                size += pointerSize;

            } else if (type.IsArray == false && type.IsValueType == true && (type.IsPrimitive == true || Unity.Collections.LowLevel.Unsafe.UnsafeUtility.IsBlittable(type) == true)) {
                
                size += System.Runtime.InteropServices.Marshal.SizeOf(obj);
                
            } else {

                var fields = type.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (fields.Length == 0 && type.IsValueType == true) {

                    size += System.Runtime.InteropServices.Marshal.SizeOf(obj);

                } else {

                    foreach (var field in fields) {

                        var fieldType = field.FieldType;
                        if (collectUnityObjects == false && typeof(UnityEngine.Object).IsAssignableFrom(fieldType) == true) {
                            
                            size += pointerSize;
                            continue;
                            
                        }
                        if (fieldType.IsEnum == true) {

                            size += sizeof(int);

                        } else if (fieldType.IsPointer == true) {

                            size += pointerSize;

                        } else if (fieldType.IsArray == false && fieldType.IsValueType == true && (fieldType.IsPrimitive == true || Unity.Collections.LowLevel.Unsafe.UnsafeUtility.IsBlittable(fieldType) == true)) {
                            
                            size += System.Runtime.InteropServices.Marshal.SizeOf(fieldType);

                        } else if (fieldType == typeof(string)) {

                            var str = (string)field.GetValue(obj);
                            if (str != null) {

                                size += charSize * str.Length;

                            }

                        } else if (fieldType.IsValueType == true) {

                            size += UnityObjectUtils.GetObjectSize(field.GetValue(obj), visited); //Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf(fieldType);

                        } else if (fieldType.IsArray == true) {

                            var arr = (System.Array)field.GetValue(obj);
                            if (arr != null) {

                                for (int i = 0; i < arr.Length; ++i) {
                                    size += UnityObjectUtils.GetObjectSize(arr.GetValue(i), visited);
                                }

                            }

                        } else {

                            size += UnityObjectUtils.GetObjectSize(field.GetValue(obj), visited);

                        }

                    }

                }

            }

            return size;

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Destroy(Object obj) {

            #if UNITY_EDITOR

            if (Application.isEditor) {

                Object.DestroyImmediate(obj);
                return;

            }

            #endif

            Object.Destroy(obj);

        }

    }

}