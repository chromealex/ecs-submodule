
namespace ME.ECS.Tests {

    public static class CollectionHelpers {

        public static World PrepareWorld() {
            
            World world = null;
            WorldUtilities.CreateWorld<EmptyState>(ref world, 0.033f);
            {
                world.SetState<EmptyState>(WorldUtilities.CreateState<EmptyState>());
                world.SetSeed(1u);
                {
                    WorldUtilities.InitComponentTypeId<ME.ECS.Views.ViewComponent>(false);
                    WorldUtilities.InitComponentTypeId<ME.ECS.Name.Name>(false);
                    WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Nodes>(false);
                    WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveListNode>(false);
                    WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveHashSetBucket>(false);
                    world.GetStructComponents().Validate<ME.ECS.Views.ViewComponent>();
                    world.GetStructComponents().Validate<ME.ECS.Name.Name>();
                    world.GetStructComponents().Validate<ME.ECS.Transform.Nodes>();
                    world.GetStructComponents().Validate<ME.ECS.Collections.IntrusiveListNode>();
                    world.GetStructComponents().Validate<ME.ECS.Collections.IntrusiveHashSetBucket>();
                    ComponentsInitializerWorld.Setup((e) => {
                
                        e.ValidateData<ME.ECS.Views.ViewComponent>();
                        e.ValidateData<ME.ECS.Name.Name>();
                        e.ValidateData<ME.ECS.Transform.Nodes>();
                        e.ValidateData<ME.ECS.Collections.IntrusiveListNode>();
                        e.ValidateData<ME.ECS.Collections.IntrusiveHashSetBucket>();
                
                    });
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

            WorldUtilities.ReleaseWorld<EmptyState>(ref world);

        }

    }

}