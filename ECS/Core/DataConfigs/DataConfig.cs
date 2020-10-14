using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.DataConfigs {

    [CreateAssetMenu(menuName = "ME.ECS/Data Config")]
    public class DataConfig : ScriptableObject {

        [SerializeReference]
        public IStructComponent[] structComponents = new IStructComponent[0];
        [SerializeReference]
        public IComponent[] components = new IComponent[0];

        public int[] structComponentsDataTypeIds = new int[0];
        public int[] structComponentsComponentTypeIds = new int[0];
        public int[] componentsTypeIds = new int[0];
        
        public void Apply(in Entity entity) {

            var world = Worlds.currentWorld;
            for (int i = 0; i < this.structComponents.Length; ++i) {

                world.SetData(in entity, in this.structComponents[i], in this.structComponentsDataTypeIds[i], in this.structComponentsComponentTypeIds[i]);

            }

            for (int i = 0; i < this.components.Length; ++i) {

                world.AddComponent(entity, this.components[i], this.componentsTypeIds[i]);

            }
            
            // Update filters
            {
                world.UpdateFilters(in entity);
            }

        }

        public T Get<T>() where T : struct, IStructComponent {

            var idx = System.Array.IndexOf(this.structComponentsDataTypeIds, AllComponentTypes<T>.typeId);
            if (idx >= 0) {

                return (T)this.structComponents[idx];

            }

            return default;

        }
        
        public void OnValidate() {

            if (Application.isPlaying == true) return;
            
            this.OnScriptLoad();
            
        }

        public void OnScriptLoad() {

            if (Application.isPlaying == true) return;

            var allAsms = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in allAsms) {

                var asmType = asm.GetType("ME.ECS.ComponentsInitializer");
                if (asmType != null) {

                    var m = asmType.GetMethod("InitTypeId", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                    if (m == null) continue;
                    
                    m.Invoke(null, null);
                    
                    {
                        
                        this.structComponentsDataTypeIds = new int[this.structComponents.Length];
                        this.structComponentsComponentTypeIds = new int[this.structComponents.Length];
                        for (int i = 0; i < this.structComponents.Length; ++i) {

                            var obj = this.structComponents[i];
                            if (obj == null) {

                                this.structComponentsDataTypeIds[i] = -1;
                                continue;
                                
                            }
                            
                            var type = obj.GetType();
                            var allId = ComponentTypesRegistry.allTypeId[type];
                            this.structComponentsDataTypeIds[i] = allId;

                            if (ComponentTypesRegistry.typeId.TryGetValue(type, out var componentIndex) == true) {

                                this.structComponentsComponentTypeIds[i] = componentIndex;

                            } else {

                                this.structComponentsComponentTypeIds[i] = -1;

                            }
                            
                        }
                        
                    }
                    
                    {
                        
                        this.componentsTypeIds = new int[this.components.Length];
                        for (int i = 0; i < this.components.Length; ++i) {

                            var obj = this.components[i];
                            if (obj == null) {

                                this.componentsTypeIds[i] = -1;
                                continue;
                                
                            }
                            
                            var type = obj.GetType();
                            if (ComponentTypesRegistry.typeId.TryGetValue(type, out var componentIndex) == true) {

                                this.componentsTypeIds[i] = componentIndex;

                            } else {

                                this.componentsTypeIds[i] = -1;

                            }

                        }
                        
                    }
                    break;

                }

            }

        }
        
        #if UNITY_EDITOR
        [UnityEditor.Callbacks.DidReloadScripts]
        public static void OnScriptsReloaded() {

            var configs = Resources.FindObjectsOfTypeAll<DataConfig>();
            foreach (var config in configs) config.OnScriptLoad();

        }
        #endif

    }

}
