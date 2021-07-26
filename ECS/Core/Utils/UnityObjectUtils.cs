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

        public static int GetObjectSize(object obj) {

            var size = 0;
            if (obj == null) return System.IntPtr.Size;
            
            var type = obj.GetType();
            var fields = type.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            foreach (var field in fields) {

                var fieldType = field.FieldType;
                if (fieldType.IsValueType == true) {

                    size += Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf(fieldType);

                } else if (fieldType.IsArray == true) {
                    
                    var arr = (System.Array)field.GetValue(obj);
                    for (int i = 0; i < arr.Length; ++i) {
                        size += UnityObjectUtils.GetObjectSize(arr.GetValue(i));
                    }

                }  else {
                    
                    var val = field.GetValue(obj);
                    size += UnityObjectUtils.GetObjectSize(val);
                    
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