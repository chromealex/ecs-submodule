
namespace ME.ECS.Collections.Tests {

    using System.Linq;

    public static class Helpers {

        private class TestState : State {

            

        }

        public static World PrepareWorld() {
            
            World world = null;
            WorldUtilities.CreateWorld<TestState>(ref world, 0.033f);
            {
                world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
                world.SetSeed(1u);
                {
                    WorldUtilities.InitComponentTypeId<ME.ECS.Views.ViewComponent>(false);
                    WorldUtilities.InitComponentTypeId<ME.ECS.Name.Name>(false);
                    WorldUtilities.InitComponentTypeId<ME.ECS.Transform.Childs>(false);
                    WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveListNode>(false);
                    WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveHashSetBucket>(false);
                    world.GetStructComponents().Validate<ME.ECS.Views.ViewComponent>();
                    world.GetStructComponents().Validate<ME.ECS.Name.Name>();
                    world.GetStructComponents().Validate<ME.ECS.Transform.Childs>();
                    world.GetStructComponents().Validate<ME.ECS.Collections.IntrusiveListNode>();
                    world.GetStructComponents().Validate<ME.ECS.Collections.IntrusiveHashSetBucket>();
                    ComponentsInitializerWorld.Setup((e) => {
                
                        e.ValidateData<ME.ECS.Views.ViewComponent>();
                        e.ValidateData<ME.ECS.Name.Name>();
                        e.ValidateData<ME.ECS.Transform.Childs>();
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
            
            world.SaveResetState<TestState>();
            
            world.SetFromToTicks(0, 1);
            world.Update(1f);

            WorldUtilities.ReleaseWorld<TestState>(ref world);

        }

    }

}