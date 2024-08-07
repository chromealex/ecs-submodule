﻿
namespace ME.ECS.Tests {

    public static class CollectionHelpers {

        public static World PrepareWorld() {
            
            WorldUtilities.ResetTypeIds();
            
            World world = null;
            WorldUtilities.CreateWorld<EmptyState>(ref world, 0.033f);
            {
                world.SetState<EmptyState>(WorldUtilities.CreateState<EmptyState>());
                world.SetSeed(1u);
                {
                    CoreComponentsInitializer.InitTypeId();
                    CoreComponentsInitializer.Init(world.GetState(), ref world.GetNoStateData());
                    //world.SetEntitiesCapacity(1000);
                }
            }
            
            WorldUtilities.SetWorld(world);

            return world;

        }

        public static void CompleteWorld(World world) {
            
            world.SaveResetState<EmptyState>();
            
            world.SetFromToTicks(0, 1);
            world.Update(1f);

            ComponentsInitializerWorld.Setup(null);
            WorldUtilities.ReleaseWorld<EmptyState>(ref world);

        }

    }

}