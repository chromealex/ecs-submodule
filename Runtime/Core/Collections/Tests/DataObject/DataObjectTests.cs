namespace ME.ECS.Collections.Tests {

    public class DataObjectTests {

        [NUnit.Framework.TestAttribute]
        public void WriteDataInTwoTicks() {

            this.Initialize();

            var intObj = new ME.ECS.Collections.DataObject<int>(10);
            var src = intObj.GetHashCode();
            intObj.Get() = 15;

            Worlds.currentWorld.SetFromToTicks(0, 1);
            Worlds.currentWorld.UpdateLogic(0f);

            intObj.Get() = 201;

            NUnit.Framework.Assert.True(src != intObj.GetHashCode());

            this.DeInitialize();

        }

        [NUnit.Framework.TestAttribute]
        public void ReadData() {

            this.Initialize();

            const int intValue = 256;
            var intObj = new ME.ECS.Collections.DataObject<int>(intValue);

            NUnit.Framework.Assert.True(intValue == intObj.Read());
            
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
