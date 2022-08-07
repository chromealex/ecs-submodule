using System.Collections.Generic;

namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class Worlds {

        public static World currentWorld;
        public static World current;
        
        public static readonly List<World> registeredWorlds = new List<World>();

        public static bool isInDeInitialization;
        public static void DeInitializeBegin() {

            Worlds.isInDeInitialization = true;

        }

        public static void DeInitializeEnd() {
            
            Worlds.isInDeInitialization = false;
            
        }

        public static void Register(World world) {
            
            Worlds.registeredWorlds.Add(world);
            
        }
        
        public static void UnRegister(World world, int id) {
            
            if (Worlds.registeredWorlds != null) Worlds.registeredWorlds.Remove(world);

            if (world == Worlds.currentWorld) {

                Worlds.currentWorld = null;
                Worlds.current = null;

            }
            
        }

    }

}