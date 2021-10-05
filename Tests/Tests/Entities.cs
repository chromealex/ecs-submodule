
namespace ME.ECS.Tests {

    public class Entities {
        
        private class TestState : State {}
        private struct TestComponent : IStructComponent {}

        private struct TestOneShotComponent : IComponentOneShot {}

        private struct TestComponentCopyable : IStructCopyable<TestComponentCopyable> {

            public int data;
            
            public void CopyFrom(in TestComponentCopyable other) {

                this.data = other.data;

            }

            public void OnRecycle() {

                this.data = default;

            }

        }

        private class TestOneShotSystem : ISystem, IAdvanceTick {

            public Entity entity;
            public int count;

            public World world { get; set; }

            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").With<TestOneShotComponent>().Push();
                this.entity = this.world.AddEntity();
                this.entity.SetOneShot(new TestOneShotComponent());
                this.count = 1;

            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {
                
                NUnit.Framework.Assert.IsTrue(this.filter.Count == this.count);
                this.count = 0;

            }

        }

        private class TestSystem : ISystem, IAdvanceTick {

            public World world { get; set; }

            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").With<TestComponent>().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {
                
                var entity = this.world.AddEntity();
                entity.Set(new TestComponent());
                
                this.filter.ApplyAllRequests();
                foreach (var ent in this.filter) {
                
                    var ent2 = this.world.AddEntity();
                    UnityEngine.Debug.Log(ent);
                    ent2.Set(new TestComponent());
                    UnityEngine.Debug.Log(ent2);
                    
                    ent.Destroy();
                    
                    var ent3 = this.world.AddEntity();
                    ent3.Set(new TestComponent());
                    UnityEngine.Debug.Log(ent3);
                
                }
                this.filter.ApplyAllRequests();
                foreach (var ent in this.filter) {
                    
                    UnityEngine.Debug.LogWarning(ent);
                    
                }

            }

        }

        private class TestStatesHistoryModule : ME.ECS.StatesHistory.StatesHistoryModule<TestState> {

        }

        private class TestNetworkModule : ME.ECS.Network.NetworkModule<TestState> {

            protected override ME.ECS.Network.NetworkType GetNetworkType() {
                return ME.ECS.Network.NetworkType.RunLocal | ME.ECS.Network.NetworkType.SendToNet;
            }

        }
        
        [NUnit.Framework.TestAttribute]
        public void AddOneShot() {

            World world = null;
            WorldUtilities.CreateWorld<TestState>(ref world, 0.033f);
            var sys = new TestOneShotSystem();
            {
                world.AddModule<TestStatesHistoryModule>();
                world.AddModule<TestNetworkModule>();
                world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
                world.SetSeed(1u);
                {
                    WorldUtilities.InitComponentTypeId<TestOneShotComponent>(false, isOneShot: true);
                    ComponentsInitializerWorld.Setup((e) => {
                
                        e.ValidateDataOneShot<TestOneShotComponent>();
                
                    });
                }
                {
                    
                    var group = new SystemGroup(world, "TestGroup");
                    group.AddSystem(sys);

                }
            }
            world.SaveResetState<TestState>();
            
            NUnit.Framework.Assert.IsTrue(sys.entity.HasOneShot<TestOneShotComponent>());
            
            world.SetFromToTicks(0, 2);
            world.Update(2f);
            
            NUnit.Framework.Assert.IsFalse(sys.entity.HasOneShot<TestOneShotComponent>());
            
            WorldUtilities.ReleaseWorld<TestState>(ref world);

        }

        [NUnit.Framework.TestAttribute]
        public void AddRemoveWithFilter() {

            World world = null;
            WorldUtilities.CreateWorld<TestState>(ref world, 0.033f);
            {
                world.AddModule<TestStatesHistoryModule>();
                world.AddModule<TestNetworkModule>();
                world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
                world.SetSeed(1u);
                {
                    WorldUtilities.InitComponentTypeId<TestComponent>(false);
                    ComponentsInitializerWorld.Setup((e) => {
                
                        e.ValidateData<TestComponent>();
                
                    });
                }
                {
                    
                    var group = new SystemGroup(world, "TestGroup");
                    group.AddSystem(new TestSystem());

                }
            }
            world.SaveResetState<TestState>();
            
            world.SetFromToTicks(0, 1);
            world.Update(1f);
            
            WorldUtilities.ReleaseWorld<TestState>(ref world);

        }

        [NUnit.Framework.TestAttribute]
        public void AddRemove() {
            
            var st = new Storage();
            st.Initialize(100);

            var entity = st.Alloc();
            NUnit.Framework.Assert.AreEqual(entity.id, 0);
            NUnit.Framework.Assert.AreEqual(entity.generation, 2);

            NUnit.Framework.Assert.AreEqual(st.AliveCount, 1);
            NUnit.Framework.Assert.AreEqual(st.DeadCount, 0);

            NUnit.Framework.Assert.IsTrue(st.IsAlive(entity.id, entity.generation));

            st.Dealloc(entity);
            st.ApplyDead();
            st.IncrementGeneration(entity);

            NUnit.Framework.Assert.IsTrue(st.IsAlive(entity.id, entity.generation) == false);

            NUnit.Framework.Assert.AreEqual(st.AliveCount, 0);
            NUnit.Framework.Assert.AreEqual(st.DeadCount, 1);

            {
                var entity2 = st.Alloc();
                NUnit.Framework.Assert.AreEqual(entity2.id, 0);
                NUnit.Framework.Assert.AreEqual(entity2.generation, 4);

                NUnit.Framework.Assert.AreEqual(st.AliveCount, 1);
                NUnit.Framework.Assert.AreEqual(st.DeadCount, 0);
            }
            
            st.Recycle();

        }

        [NUnit.Framework.TestAttribute]
        public void AddRemoveMulti() {

            var st = new Storage();
            st.Initialize(20);

            var list = new System.Collections.Generic.List<Entity>();
            var v = 2;
            for (int j = 0; j < 10; ++j) {

                list.Clear();
                for (int i = 0; i < 10000; ++i) {

                    var entity = st.Alloc();
                    list.Add(entity);
                    //NUnit.Framework.Assert.AreEqual(entity.id, i);
                    NUnit.Framework.Assert.AreEqual(entity.generation, v);

                    //NUnit.Framework.Assert.AreEqual(st.AliveCount, i + 1);

                    NUnit.Framework.Assert.IsTrue(st.IsAlive(entity.id, entity.generation));

                }

                for (int i = 0; i < list.Count; ++i) {

                    st.Dealloc(list[i]);
                    st.IncrementGeneration(list[i]);

                }
                
                for (int i = 0; i < 10000; ++i) {

                    var entity = st.Alloc();
                    list.Add(entity);
                    //NUnit.Framework.Assert.AreEqual(entity.id, i);
                    NUnit.Framework.Assert.AreEqual(entity.generation, v);

                    //NUnit.Framework.Assert.AreEqual(st.AliveCount, i + 1);

                    NUnit.Framework.Assert.IsTrue(st.IsAlive(entity.id, entity.generation));

                }

                for (int i = 0; i < list.Count; ++i) {

                    st.Dealloc(list[i]);
                    st.IncrementGeneration(list[i]);

                }

                st.ApplyDead();
                v += 2;

            }
            
            UnityEngine.Debug.Log("Stats: " + st.AliveCount + " :: " + st.DeadCount);

            st.Recycle();

        }

    }

}