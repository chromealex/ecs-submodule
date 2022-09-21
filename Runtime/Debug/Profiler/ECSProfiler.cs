
namespace ME.ECS {

    using Unity.Profiling;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;
    
    public static class ECSProfiler {

        [System.Serializable]
        public struct ProfilerCounterData {

            public string m_Category;
            public string m_Name;

            public ProfilerCounterData(ProfilerCategory category, string name) {
                this.m_Category = category.Name;
                this.m_Name = name;
            }

        }

        public static readonly string caption = "<b><color=#888>ME.ECS</color></b>";
        private static readonly ProfilerCategory category = new ProfilerCategory(caption);
        private static readonly ProfilerCategory categoryNetwork = new ProfilerCategory($"{caption}: Network");
        private static readonly ProfilerCategory categoryPools = new ProfilerCategory($"{caption} Pools");
        private static readonly ProfilerCategory categoryAllocator = new ProfilerCategory($"{caption} Allocator");

        public static readonly ProfilerCounterData EntitiesCount_Data = new ProfilerCounterData(ECSProfiler.category, "Entities Count");
        public static readonly ProfilerCounterData SystemsCount_Data = new ProfilerCounterData(ECSProfiler.category, "Systems Count");
        public static readonly ProfilerCounterData ModulesCount_Data = new ProfilerCounterData(ECSProfiler.category, "Modules Count");
        public static readonly ProfilerCounterData ViewsCount_Data = new ProfilerCounterData(ECSProfiler.category, "Views Count");
        public static readonly ProfilerCounterData LogicRollback_Data = new ProfilerCounterData(ECSProfiler.category, "Rollbacks (ms)");
        public static readonly ProfilerCounterData LogicSystems_Data = new ProfilerCounterData(ECSProfiler.category, "Systems (ms)");
        public static readonly ProfilerCounterData VisualViews_Data = new ProfilerCounterData(ECSProfiler.category, "Views (ms)");
        public static readonly ProfilerCounter<int> EntitiesCount = new ProfilerCounter<int>(ECSProfiler.category, EntitiesCount_Data.m_Name, ProfilerMarkerDataUnit.Count);
        public static readonly ProfilerCounter<int> SystemsCount = new ProfilerCounter<int>(ECSProfiler.category, SystemsCount_Data.m_Name, ProfilerMarkerDataUnit.Count);
        public static readonly ProfilerCounter<int> ModulesCount = new ProfilerCounter<int>(ECSProfiler.category, ModulesCount_Data.m_Name, ProfilerMarkerDataUnit.Count);
        public static readonly ProfilerCounter<int> ViewsCount = new ProfilerCounter<int>(ECSProfiler.category, ViewsCount_Data.m_Name, ProfilerMarkerDataUnit.Count);
        public static readonly ProfilerCounter<long> LogicRollback = new ProfilerCounter<long>(ECSProfiler.category, LogicRollback_Data.m_Name, ProfilerMarkerDataUnit.TimeNanoseconds);
        public static readonly ProfilerCounterValue<long> LogicSystems = new ProfilerCounterValue<long>(ECSProfiler.category, LogicSystems_Data.m_Name, ProfilerMarkerDataUnit.TimeNanoseconds, ProfilerCounterOptions.ResetToZeroOnFlush);
        public static readonly ProfilerCounterValue<long> VisualViews = new ProfilerCounterValue<long>(ECSProfiler.category, VisualViews_Data.m_Name, ProfilerMarkerDataUnit.TimeNanoseconds, ProfilerCounterOptions.ResetToZeroOnFlush);

        public static readonly ProfilerCounterData NetworkEventsSentCount_Data = new ProfilerCounterData(ECSProfiler.categoryNetwork, "Net: Events Sent Count");
        public static readonly ProfilerCounterData NetworkEventsReceivedCount_Data = new ProfilerCounterData(ECSProfiler.categoryNetwork, "Net: Events Received Count");
        public static readonly ProfilerCounterData NetworkEventsSentBytes_Data = new ProfilerCounterData(ECSProfiler.categoryNetwork, "Net: Bytes Sent");
        public static readonly ProfilerCounterData NetworkEventsReceivedBytes_Data = new ProfilerCounterData(ECSProfiler.categoryNetwork, "Net: Bytes Received");
        public static readonly ProfilerCounter<int> NetworkEventsSentCount = new ProfilerCounter<int>(ECSProfiler.categoryNetwork, NetworkEventsSentCount_Data.m_Name, ProfilerMarkerDataUnit.Count);
        public static readonly ProfilerCounter<int> NetworkEventsReceivedCount = new ProfilerCounter<int>(ECSProfiler.categoryNetwork, NetworkEventsReceivedCount_Data.m_Name, ProfilerMarkerDataUnit.Count);
        public static readonly ProfilerCounter<int> NetworkEventsSentBytes = new ProfilerCounter<int>(ECSProfiler.categoryNetwork, NetworkEventsSentBytes_Data.m_Name, ProfilerMarkerDataUnit.Bytes);
        public static readonly ProfilerCounter<int> NetworkEventsReceivedBytes = new ProfilerCounter<int>(ECSProfiler.categoryNetwork, NetworkEventsReceivedBytes_Data.m_Name, ProfilerMarkerDataUnit.Bytes);
        
        public static readonly ProfilerCounterData MemoryAllocatorReserved_Data = new ProfilerCounterData(ECSProfiler.categoryAllocator, "Allocator: Reserved (bytes)");
        public static readonly ProfilerCounterData MemoryAllocatorUsed_Data = new ProfilerCounterData(ECSProfiler.categoryAllocator, "Allocator: Used (bytes)");
        public static readonly ProfilerCounterData MemoryAllocatorFree_Data = new ProfilerCounterData(ECSProfiler.categoryAllocator, "Allocator: Free (bytes)");
        public static readonly ProfilerCounter<int> MemoryAllocatorReserved = new ProfilerCounter<int>(ECSProfiler.categoryAllocator, MemoryAllocatorReserved_Data.m_Name, ProfilerMarkerDataUnit.Bytes);
        public static readonly ProfilerCounter<int> MemoryAllocatorUsed = new ProfilerCounter<int>(ECSProfiler.categoryAllocator, MemoryAllocatorUsed_Data.m_Name, ProfilerMarkerDataUnit.Bytes);
        public static readonly ProfilerCounter<int> MemoryAllocatorFree = new ProfilerCounter<int>(ECSProfiler.categoryAllocator, MemoryAllocatorFree_Data.m_Name, ProfilerMarkerDataUnit.Bytes);

        public static readonly ProfilerCounterData PoolAllocation_Data = new ProfilerCounterData(ECSProfiler.categoryPools, "Pool: Allocated (bytes)");
        public static readonly ProfilerCounterData PoolUsed_Data = new ProfilerCounterData(ECSProfiler.categoryPools, "Pool: Used (bytes)");
        public static readonly ProfilerCounterValue<int> PoolAllocation = new ProfilerCounterValue<int>(ECSProfiler.categoryPools, PoolAllocation_Data.m_Name, ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.ResetToZeroOnFlush);
        public static readonly ProfilerCounter<int> PoolUsed = new ProfilerCounter<int>(ECSProfiler.categoryPools, PoolUsed_Data.m_Name, ProfilerMarkerDataUnit.Count);
        private static int poolAllocationPrev;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("ENABLE_PROFILER")]
        [Pure]
        public static void SampleWorld(World world) {
            
            ECSProfiler.EntitiesCount.Sample(world.GetEntitiesCount());
            
            var sysCount = 0;
            for (int i = 0; i < world.systemGroups.Length; ++i) {
                var group = world.systemGroups[i];
                if (group.runtimeSystem.allSystems == null) continue;
                sysCount += group.runtimeSystem.allSystems.Count;
            }
            ECSProfiler.SystemsCount.Sample(sysCount);
            
            ECSProfiler.ModulesCount.Sample(world.modules.Count);

            var viewModule = world.GetModule<ME.ECS.Views.IViewModule>();
            if (viewModule != null) {
            
                var renderersCount = 0;
                var data = viewModule.GetData();
                if (data.arr != null) {

                    for (int i = 0; i < data.Length; ++i) {

                        var views = data.arr[i];
                        renderersCount += views.Length;

                    }

                }
                ECSProfiler.ViewsCount.Sample(renderersCount);
                
            }

            var netModule = world.GetModule<ME.ECS.Network.INetworkModuleBase>();
            if (netModule != null) {
            
                ECSProfiler.NetworkEventsSentCount.Sample(netModule.GetEventsSentCount());
                ECSProfiler.NetworkEventsReceivedCount.Sample(netModule.GetEventsReceivedCount());
                ECSProfiler.NetworkEventsSentBytes.Sample(netModule.GetEventsBytesSentCount());
                ECSProfiler.NetworkEventsReceivedBytes.Sample(netModule.GetEventsBytesReceivedCount());
                
            }
            
            ECSProfiler.PoolUsed.Sample(PoolInternalBase.allocated - PoolInternalBase.deallocated);
            ECSProfiler.PoolAllocation.Value = PoolInternalBase.newAllocated - ECSProfiler.poolAllocationPrev;
            ECSProfiler.poolAllocationPrev = PoolInternalBase.newAllocated;
            
            ECSProfiler.MemoryAllocatorReserved.Sample(world.currentState.allocator.GetReservedSize());
            ECSProfiler.MemoryAllocatorUsed.Sample(world.currentState.allocator.GetUsedSize());
            ECSProfiler.MemoryAllocatorFree.Sample(world.currentState.allocator.GetFreeSize());

        }
        
    }

}