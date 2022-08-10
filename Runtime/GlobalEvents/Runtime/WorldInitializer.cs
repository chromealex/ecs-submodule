using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.GlobalEvents {

    public static class WorldInitializer {

        public static DisposeStatic disposeStatic = new DisposeStatic();
        
        static WorldInitializer() {
            
            WorldStaticCallbacks.RegisterCallbacks(InitWorld, DisposeWorld);
            WorldStaticCallbacks.RegisterCallbacks(OnWorldStep);
            
        }

        public class DisposeStatic {
            ~DisposeStatic() {
                WorldStaticCallbacks.UnRegisterCallbacks(InitWorld, DisposeWorld);
            }
        }

        public static void InitWorld(World world) {

            world.GetNoStateData().pluginsStorage.Add(ref world.GetNoStateData().allocator, new WorldStorage());

        }
        
        public static void DisposeWorld(World world) {
            
        }

        public static void OnWorldStep(World world, WorldStep step) {
            
            world.ProcessGlobalEvents(step == WorldStep.VisualTick ? GlobalEventType.Visual : GlobalEventType.Logic);
            
        }

    }

}