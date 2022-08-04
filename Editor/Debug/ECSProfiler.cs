using System.Linq;

namespace ME.ECSEditor {

    [UnityEditor.InitializeOnLoadAttribute]
    public static class ECSProfiler {

        [System.SerializableAttribute]
        public struct SerializedData {
            public string m_Name;
            public System.Collections.Generic.List<ME.ECS.ECSProfiler.ProfilerCounterData> m_ChartCounters;
            public System.Collections.Generic.List<ME.ECS.ECSProfiler.ProfilerCounterData> m_DetailCounters;
        }

        [System.SerializableAttribute]
        public class SerializedDataCollection {
            public System.Collections.Generic.List<SerializedData> m_Modules = new System.Collections.Generic.List<SerializedData>();
        }
        
        public static void InitializeProfilerModules() {

            var k_DynamicModulesPreferenceKey = "ProfilerWindow.DynamicModules";
            var json = UnityEditor.EditorPrefs.GetString(k_DynamicModulesPreferenceKey);
            var serializedDynamicModules = UnityEngine.JsonUtility.FromJson<SerializedDataCollection>(json);
            if (serializedDynamicModules == null) {
                serializedDynamicModules = new SerializedDataCollection() {
                    m_Modules = new System.Collections.Generic.List<SerializedData>(),
                };
            }
            {

                {
                    var module = new SerializedData() {
                        m_Name = ME.ECS.ECSProfiler.caption,
                        m_ChartCounters = new System.Collections.Generic.List<ME.ECS.ECSProfiler.ProfilerCounterData>() {
                            ME.ECS.ECSProfiler.EntitiesCount_Data,
                            ME.ECS.ECSProfiler.SystemsCount_Data,
                            ME.ECS.ECSProfiler.ModulesCount_Data,
                            ME.ECS.ECSProfiler.ViewsCount_Data,
                            ME.ECS.ECSProfiler.LogicRollback_Data,
                            ME.ECS.ECSProfiler.LogicSystems_Data,
                            ME.ECS.ECSProfiler.VisualViews_Data,
                        },
                        m_DetailCounters = new System.Collections.Generic.List<ME.ECS.ECSProfiler.ProfilerCounterData>() {
                            ME.ECS.ECSProfiler.EntitiesCount_Data,
                            ME.ECS.ECSProfiler.SystemsCount_Data,
                            ME.ECS.ECSProfiler.ModulesCount_Data,
                            ME.ECS.ECSProfiler.ViewsCount_Data,
                            ME.ECS.ECSProfiler.LogicRollback_Data,
                            ME.ECS.ECSProfiler.LogicSystems_Data,
                            ME.ECS.ECSProfiler.VisualViews_Data,
                        },
                    };
                    if (serializedDynamicModules.m_Modules.Any(x => x.m_Name == module.m_Name) == false) serializedDynamicModules.m_Modules.Add(module);
                }

                {
                    var module = new SerializedData() {
                        m_Name = $"{ME.ECS.ECSProfiler.caption} Network",
                        m_ChartCounters = new System.Collections.Generic.List<ME.ECS.ECSProfiler.ProfilerCounterData>() {
                            ME.ECS.ECSProfiler.NetworkEventsSentCount_Data,
                            ME.ECS.ECSProfiler.NetworkEventsReceivedCount_Data,
                            ME.ECS.ECSProfiler.NetworkEventsSentBytes_Data,
                            ME.ECS.ECSProfiler.NetworkEventsReceivedBytes_Data,
                        },
                        m_DetailCounters = new System.Collections.Generic.List<ME.ECS.ECSProfiler.ProfilerCounterData>() {
                            ME.ECS.ECSProfiler.NetworkEventsSentCount_Data,
                            ME.ECS.ECSProfiler.NetworkEventsReceivedCount_Data,
                            ME.ECS.ECSProfiler.NetworkEventsSentBytes_Data,
                            ME.ECS.ECSProfiler.NetworkEventsReceivedBytes_Data,
                        },
                    };
                    if (serializedDynamicModules.m_Modules.Any(x => x.m_Name == module.m_Name) == false) serializedDynamicModules.m_Modules.Add(module);
                }

                {
                    var module = new SerializedData() {
                        m_Name = $"{ME.ECS.ECSProfiler.caption} Pools",
                        m_ChartCounters = new System.Collections.Generic.List<ME.ECS.ECSProfiler.ProfilerCounterData>() {
                            ME.ECS.ECSProfiler.PoolAllocation_Data,
                            ME.ECS.ECSProfiler.PoolUsed_Data,
                        },
                        m_DetailCounters = new System.Collections.Generic.List<ME.ECS.ECSProfiler.ProfilerCounterData>() {
                            ME.ECS.ECSProfiler.PoolAllocation_Data,
                            ME.ECS.ECSProfiler.PoolUsed_Data,
                        },
                    };
                    if (serializedDynamicModules.m_Modules.Any(x => x.m_Name == module.m_Name) == false) serializedDynamicModules.m_Modules.Add(module);
                }

                {
                    var module = new SerializedData() {
                        m_Name = $"{ME.ECS.ECSProfiler.caption} Allocator",
                        m_ChartCounters = new System.Collections.Generic.List<ME.ECS.ECSProfiler.ProfilerCounterData>() {
                            ME.ECS.ECSProfiler.MemoryAllocatorReserved_Data,
                            ME.ECS.ECSProfiler.MemoryAllocatorUsed_Data,
                            ME.ECS.ECSProfiler.MemoryAllocatorFree_Data,
                        },
                        m_DetailCounters = new System.Collections.Generic.List<ME.ECS.ECSProfiler.ProfilerCounterData>() {
                            ME.ECS.ECSProfiler.MemoryAllocatorReserved_Data,
                            ME.ECS.ECSProfiler.MemoryAllocatorUsed_Data,
                            ME.ECS.ECSProfiler.MemoryAllocatorFree_Data,
                        },
                    };
                    if (serializedDynamicModules.m_Modules.Any(x => x.m_Name == module.m_Name) == false) serializedDynamicModules.m_Modules.Add(module);
                }

                json = UnityEngine.JsonUtility.ToJson(serializedDynamicModules);

            }
            UnityEditor.EditorPrefs.SetString(k_DynamicModulesPreferenceKey, json);
            
        }
        
        static ECSProfiler() {

            InitializeProfilerModules();

        }

    }

}