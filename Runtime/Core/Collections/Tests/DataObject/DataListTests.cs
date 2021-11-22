namespace ME.ECS.Collections.Tests {

    public class DataListTests {

        [NUnit.Framework.TestAttribute]
        public void WriteDataInTwoTicks() {

            this.Initialize();

            var intList = new ME.ECS.Collections.DataList<int>(10);
            var src = intList.GetHashCode();

            var list = intList.Get();
            for (int i = 0; i < list.Capacity; ++i) {

                list.Add(i);

            }

            Worlds.currentWorld.SetFromToTicks(0, 1);
            Worlds.currentWorld.UpdateLogic(0f);

            var list2 = intList.Get();
            for (int i = 0; i < list2.Count; ++i) {

                list2[i] = i;

            }

            NUnit.Framework.Assert.True(src != intList.GetHashCode());

            this.DeInitialize();

        }

        [NUnit.Framework.TestAttribute]
        public void ReadData() {

            this.Initialize();

            var intList = new ME.ECS.Collections.DataList<int>(10);
            var list = intList.Get();
            for (int i = 0; i < list.Capacity; ++i) {

                list.Add(i);

            }

            var rList = intList.Read();
            for (int i = 0; i < rList.Count; ++i) {

                NUnit.Framework.Assert.True(rList[i] == i);

            }

            this.DeInitialize();

        }

        public class TestState : State {}

        public class TestStatesHistoryModule : ME.ECS.StatesHistory.StatesHistoryModule<TestState> { }

        private World world;
        private void Initialize() {
        
            WorldUtilities.CreateWorld<TestState>(ref this.world, 0.033f);
            this.world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
            this.world.AddModule<TestStatesHistoryModule>();
            this.world.SetSeed(1u);
            this.world.SaveResetState<TestState>();
            
        }

        private void DeInitialize() {

            WorldUtilities.ReleaseWorld<TestState>(ref this.world);

        }

    }

}
