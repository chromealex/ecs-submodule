using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME {

    public static class WeakRefShorts {

        public static void weak(object obj, bool stacktrace = true) {
            
            WeakRef.Reg(obj, stacktrace);
            
        }

        public static void unweak(object obj) {
            
            WeakRef.UnReg(obj);
            
        }

    }

    public static class WeakRef {

        public struct WeakItem {

            public System.WeakReference reference;
            public System.Type type;
            public string stack;

        }

        private static readonly List<WeakItem> items = new List<WeakItem>();

        [System.Diagnostics.ConditionalAttribute("ME_ECS_COLLECT_WEAK_REFERENCES")]
        public static void Reg(object obj, bool stacktrace = true) {

            if (obj == null) return;
            WeakRef.items.Add(new WeakItem() {
                reference = new System.WeakReference(obj),
                type = obj.GetType(),
                stack = (stacktrace == true ? System.Environment.StackTrace : string.Empty),
            });

            if (WeakRef.items.Count > 50) {
                
                WeakRef.items.RemoveAll(x => x.reference.IsAlive == false);
                
            }

        }

        [System.Diagnostics.ConditionalAttribute("ME_ECS_COLLECT_WEAK_REFERENCES")]
        public static void UnReg(object obj) {

            if (obj == null) return;
            WeakRef.items.RemoveAll(x => x.reference.Target == obj);

        }

        #if UNITY_EDITOR
        [UnityEditor.MenuItem("ME.ECS/Debug/List Weak References")]
        public static void ListMenu() {
            
            System.GC.Collect();
            WeakRef.List();
            
        }
        #endif

        public static void List() {

            System.GC.Collect();
            System.GC.Collect();
            System.GC.Collect();
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
            System.GC.Collect();
            System.GC.Collect();
            
            var prev = UnityEngine.Application.GetStackTraceLogType(LogType.Log);
            UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Log, StackTraceLogType.None);
            var cnt = 0;
            foreach (var item in items) {

                if (item.reference.IsAlive == true) {

                    ++cnt;
                    if (cnt > 20) break;
                    var target = item.reference.Target;
                    if (target != null) {
                        if (target is Object obj) {
                            Debug.Log(item.type.Name + ": " + obj + " (" + obj.GetInstanceID() + ")" + "\n" + item.stack);
                        } else {
                            Debug.Log(item.type.Name + ": " + target + " (" + target.GetHashCode() + ")" + "\n" + item.stack);
                        }
                    } else {
                        Debug.Log(item.type.Name + ": " + target + "\n" + item.stack);
                    }
                    
                }
                
            }
            UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Log, prev);
            
        }

    }

}