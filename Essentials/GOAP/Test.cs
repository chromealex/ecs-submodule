namespace ME.ECS.Essentials.GOAP {

    public struct TestComponent : IComponent {}
    
    public class Test : UnityEngine.MonoBehaviour {

        public GOAPGroup group;
        
        public void OnDrawGizmos() {

            /*var actions = new Action[] {
                new Action() {
                    name = "- => 3",
                    cost = 6f,
                    effects = Effect.Create().With<CreateTool>().Push(),
                },
                new Action() {
                    name = "have 7",
                    cost = 1f,
                    preconditions = new Precondition() {
                        hasComponents = new []{ 2, 7, },
                    },
                    effects = new Effect() {
                        hasComponents = new []{ 3, },
                    },
                },
                new Action() {
                    name = "2 => 3",
                    cost = 3f,
                    preconditions = new Precondition() {
                        hasComponents = new []{ 2, },
                    },
                    effects = new Effect() {
                        hasComponents = new []{ 3, },
                    },
                },
                new Action() {
                    name = "1 => 4",
                    cost = 1f,
                    preconditions = new Precondition() {
                        hasComponents = new []{ 1, },
                    },
                    effects = new Effect() {
                        hasComponents = new []{ 4, },
                    },
                },
                new Action() {
                    name = "4 => 3",
                    cost = 2f,
                    preconditions = new Precondition() {
                        hasComponents = new []{ 4, },
                    },
                    effects = new Effect() {
                        hasComponents = new []{ 3, },
                    },
                },
                new Action() {
                    name = "start 7",
                    cost = 1f,
                    precondition = new PreconditionFilter() {
                        hasComponents = new []{ 7, },
                    },
                    effect = new Effect() {
                        hasComponents = new []{ 3, },
                    },
                },
                new Action() {
                    name = "- => 1",
                    cost = 1f,
                    effects = new Effect() {
                        hasComponents = new []{ 1, },
                    },
                },
                new Action() {
                    name = "1 => 2",
                    cost = 1f,
                    preconditions = new Precondition() {
                        hasComponents = new []{ 1, },
                    },
                    effects = new Effect() {
                        hasComponents = new []{ 2, },
                    },
                },
            };
            
            var goal = Goal.Create<CreateTool>();
            */
            
            TestsHelper.Do((w) => {

                WorldUtilities.InitComponentTypeId<TestComponent>(true);
                /*WorldUtilities.InitComponentTypeId<HasWood>(true);
                WorldUtilities.InitComponentTypeId<HasOre>(true);
                WorldUtilities.InitComponentTypeId<HasTool>(true);
                WorldUtilities.InitComponentTypeId<HasFirewood>(true);
                WorldUtilities.InitComponentTypeId<CreateTool>(true);*/

                WorldUtilities.InitComponentTypeId<GOAPEntityGroup>(false);
                WorldUtilities.InitComponentTypeId<GOAPEntityGoal>(false);
                WorldUtilities.InitComponentTypeId<GOAPEntityPlan>(false);

                ref var st = ref w.GetStructComponents();
                st.Validate<TestComponent>();
                /*st.Validate<HasWood>();
                st.Validate<HasOre>();
                st.Validate<HasTool>();
                st.Validate<HasFirewood>();
                st.Validate<CreateTool>();*/
                st.Validate<GOAPEntityGroup>();
                st.Validate<GOAPEntityGoal>();
                st.Validate<GOAPEntityPlan>();
                
                w.SetEntitiesCapacity(100);
                
            }, (w) => {
                
                var group = new SystemGroup("group");
                group.AddSystem<ME.ECS.Essentials.GOAP.Systems.GOAPPlannerSystem>();

                w.AddModule<ME.ECS.Essentials.GOAP.Modules.GOAPModule>();

                var entity = new Entity(EntityFlag.None);
                entity.SetGOAPGroup(this.group);
                entity.Set(new GOAPEntityGoal() {
                    goal = Goal.Create<TestComponent>(),
                });

                /*var planner = new Planner();
                var sw = System.Diagnostics.Stopwatch.StartNew();
                sw.Start();
                var plan = planner.GetPlan(w, actions, goal, entity);
                sw.Stop();
                UnityEngine.Debug.Log(plan + " in " + sw.ElapsedMilliseconds + "ms");
                plan.Dispose();

                for (int i = 0; i < actions.Length; ++i) {
                    actions[i].Dispose();
                }*/

            });
            
        }

    }
    
    public class EmptyState : State {}
    public class EmptyStatesHistoryModule : ME.ECS.StatesHistory.StatesHistoryModule<EmptyState> {}
    public class EmptyNetworkModule : ME.ECS.Network.NetworkModule<EmptyState> {}
    
    public static class TestsHelper {
    
        public static void Do(
            System.Action<World> setupWorld = null,
            System.Action<World> initSystems = null,
            System.Action<World> beforeUpdate = null,
            System.Action<World> afterUpdate = null,
            int from = 0,
            int to = 2) {
        
            WorldUtilities.ResetTypeIds();
            
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