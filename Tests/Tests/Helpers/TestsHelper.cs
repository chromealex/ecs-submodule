using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Tests {

    public class EmptyState : State {}
    
    public class EmptyStatesHistoryModule : ME.ECS.StatesHistory.StatesHistoryModule<EmptyState> {

    }

    public class EmptyNetworkModule : ME.ECS.Network.NetworkModule<EmptyState> {

        protected override ME.ECS.Network.NetworkType GetNetworkType() {
            return ME.ECS.Network.NetworkType.RunLocal | ME.ECS.Network.NetworkType.SendToNet;
        }

    }

    public static class TestsHelper {
    
        public static void Do(
            System.Action<World> setupWorld = null,
            System.Action<World> initSystems = null,
            System.Action<World> beforeUpdate = null,
            System.Action<World> afterUpdate = null,
            int from = 0,
            int to = 2) {
        
            WorldUtilities.ResetTypeIds();
            
            ME.ECS.Pools.current = new ME.ECS.PoolImplementation(isNull: false);
            World world = null;
            WorldUtilities.CreateWorld<EmptyState>(ref world, 0.033f);
            {
                world.AddModule<EmptyStatesHistoryModule>();
                world.AddModule<EmptyNetworkModule>();
                world.SetState<EmptyState>(WorldUtilities.CreateState<EmptyState>());
                world.SetSeed(1u);
                {
                    ref var str = ref world.GetStructComponents();
                    ref var str2 = ref world.GetNoStateStructComponents();
                    CoreComponentsInitializer.InitTypeId();
                    CoreComponentsInitializer.Init(ref str, ref str2);
                    setupWorld?.Invoke(world);
                }
                
                initSystems?.Invoke(world);
                
            }
            world.SaveResetState<EmptyState>();
            
            beforeUpdate?.Invoke(world);
            
            world.SetFromToTicks(from, to);
            world.Update(2f);
            
            afterUpdate?.Invoke(world);
            
            WorldUtilities.ReleaseWorld<EmptyState>(ref world);

        }
        
    }

}