
namespace ME.ECS.Collections.Tests {

    public class DataBufferArrayTests {
        
        [NUnit.Framework.TestAttribute]
        public void WriteDataInTwoTicks() {

            this.Initialize();

            var bufferArr = new ME.ECS.Collections.DataBufferArray<int>(10);
            ref var arr = ref bufferArr.Get();
            var src = arr.GetHashCode();
            for (int i = 0; i < arr.Length; ++i) {
                
                arr[i] = i;
                
            }
            
            Worlds.currentWorld.SetFromToTicks(0, 40);
            Worlds.currentWorld.UpdateLogic(0.033f);
            
            arr = ref bufferArr.Get();
            for (int i = 0; i < arr.Length; ++i) {
                
                arr[i] = i;
                
            }

            NUnit.Framework.Assert.True(src != arr.GetHashCode());

            this.DeInitialize();

        }

        [NUnit.Framework.TestAttribute]
        public void WriteData() {

            this.Initialize();

            var bufferArr = new ME.ECS.Collections.DataBufferArray<int>(10);
            ref var arr = ref bufferArr.Get();
            var src = arr.GetHashCode();
            for (int i = 0; i < arr.Length; ++i) {
                
                arr[i] = i;
                
            }
            NUnit.Framework.Assert.True(src == arr.GetHashCode());

            this.DeInitialize();

        }

        [NUnit.Framework.TestAttribute]
        public void ReadData() {

            this.Initialize();

            var bufferArr = new ME.ECS.Collections.DataBufferArray<int>(10);
            ref var arr = ref bufferArr.Get();
            for (int i = 0; i < arr.Length; ++i) {
                
                arr[i] = i;
                
            }

            for (int i = 0; i < arr.Length; ++i) {
                
                NUnit.Framework.Assert.True(arr[i] == i);
                
            }
            
            this.DeInitialize();

        }

        public class TestState : State { }

        public class TestStatesHistoryModule : ME.ECS.StatesHistory.StatesHistoryModule<TestState> {

            protected override uint GetTicksPerState() {
                return 2;
            }

            protected override uint GetQueueCapacity() {
                return 10;
            }

        }

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
