namespace ME.ECS {

    using Unity.Profiling;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;
    
    public static class ECSProfiler {

        private static readonly ProfilerCategory category = new ProfilerCategory("ME.ECS");
        private static readonly ProfilerCategory categoryNetwork = new ProfilerCategory("ME.ECS: Network");
        private static readonly ProfilerCategory categoryPools = new ProfilerCategory("ME.ECS: Pools");

        public static readonly ProfilerCounter<int> EntitiesCount = new ProfilerCounter<int>(ECSProfiler.category, "Entities Count", ProfilerMarkerDataUnit.Count);
        public static readonly ProfilerCounter<int> SystemsCount = new ProfilerCounter<int>(ECSProfiler.category, "Systems Count", ProfilerMarkerDataUnit.Count);
        public static readonly ProfilerCounter<int> ModulesCount = new ProfilerCounter<int>(ECSProfiler.category, "Modules Count", ProfilerMarkerDataUnit.Count);
        public static readonly ProfilerCounter<int> ViewsCount = new ProfilerCounter<int>(ECSProfiler.category, "Views Count", ProfilerMarkerDataUnit.Count);
        public static readonly ProfilerCounter<long> LogicRollback = new ProfilerCounter<long>(ECSProfiler.category, "Rollbacks", ProfilerMarkerDataUnit.TimeNanoseconds);
        public static readonly ProfilerCounterValue<long> LogicSystems = new ProfilerCounterValue<long>(ECSProfiler.category, "Systems", ProfilerMarkerDataUnit.TimeNanoseconds, ProfilerCounterOptions.ResetToZeroOnFlush);
        public static readonly ProfilerCounterValue<long> VisualViews = new ProfilerCounterValue<long>(ECSProfiler.category, "Views", ProfilerMarkerDataUnit.TimeNanoseconds, ProfilerCounterOptions.ResetToZeroOnFlush);

        public static readonly ProfilerCounter<int> NetworkEventsSentCount = new ProfilerCounter<int>(ECSProfiler.categoryNetwork, "Net: Events Sent", ProfilerMarkerDataUnit.Count);
        public static readonly ProfilerCounter<int> NetworkEventsReceivedCount = new ProfilerCounter<int>(ECSProfiler.categoryNetwork, "Net: Events Received", ProfilerMarkerDataUnit.Count);
        public static readonly ProfilerCounter<int> NetworkEventsSentBytes = new ProfilerCounter<int>(ECSProfiler.categoryNetwork, "Net: Bytes Sent", ProfilerMarkerDataUnit.Bytes);
        public static readonly ProfilerCounter<int> NetworkEventsReceivedBytes = new ProfilerCounter<int>(ECSProfiler.categoryNetwork, "Net: Bytes Received", ProfilerMarkerDataUnit.Bytes);
        
        public static readonly ProfilerCounterValue<int> PoolAllocation = new ProfilerCounterValue<int>(ECSProfiler.categoryPools, "Pool: Allocation", ProfilerMarkerDataUnit.Count, ProfilerCounterOptions.ResetToZeroOnFlush);
        private static int poolAllocationPrev;
        public static readonly ProfilerCounter<int> PoolUsed = new ProfilerCounter<int>(ECSProfiler.categoryPools, "Pool: Used", ProfilerMarkerDataUnit.Count);

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

        }
        
    }

}