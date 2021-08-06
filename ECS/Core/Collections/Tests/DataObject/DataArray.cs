
namespace ME.ECS.Collections.Tests {

    public class DataArrayTests {
        
        [NUnit.Framework.TestAttribute]
        public void WriteDataInTwoTicks() {

            this.Initialize();

            var arr = new ME.ECS.Collections.DataArray<int>(10);
            var src = arr.GetHashCode();
            for (int i = 0; i < arr.Length; ++i) {
                
                arr[i] = i;
                
            }
            
            Worlds.currentWorld.SetFromToTicks(0, 1);
            Worlds.currentWorld.UpdateLogic(0f);
            
            for (int i = 0; i < arr.Length; ++i) {
                
                arr[i] = i;
                
            }

            NUnit.Framework.Assert.True(src != arr.GetHashCode());

            this.DeInitialize();

        }

        [NUnit.Framework.TestAttribute]
        public void WriteData() {

            this.Initialize();

            var arr = new ME.ECS.Collections.DataArray<int>(10);
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

            var arr = new ME.ECS.Collections.DataArray<int>(10);
            for (int i = 0; i < arr.Length; ++i) {
                
                arr[i] = i;
                
            }

            for (int i = 0; i < arr.Length; ++i) {
                
                NUnit.Framework.Assert.True(arr[i] == i);
                
            }
            
            this.DeInitialize();

        }

        private class TestState : State {

            

        }

        private World world;
        private void Initialize() {
        
            WorldUtilities.CreateWorld<TestState>(ref this.world, 0.033f);
            this.world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
            this.world.SetSeed(1u);
            this.world.SaveResetState<TestState>();
            
        }

        private void DeInitialize() {
            
            WorldUtilities.ReleaseWorld<TestState>(ref this.world);
            
        }

    }

}