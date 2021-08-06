namespace ME.ECS.Collections.Tests {

    public class DataDictionaryTests {

        [NUnit.Framework.TestAttribute]
        public void WriteDataInTwoTicks() {

            this.Initialize();

            var intDic = new ME.ECS.Collections.DataDictionary<int, int>(10);
            var src = intDic.GetHashCode();

            var dic = intDic.Get();
            for (int i = 0; i < 10; ++i) {

                dic.Add(i, 10 - i);

            }

            Worlds.currentWorld.SetFromToTicks(0, 1);
            Worlds.currentWorld.UpdateLogic(0f);

            var dic2 = intDic.Get();
            for (int i = 0; i < dic2.Count; ++i) {

                dic2[i] = i;

            }

            NUnit.Framework.Assert.True(src != intDic.GetHashCode());

            this.DeInitialize();

        }

        [NUnit.Framework.TestAttribute]
        public void WriteData() {

            this.Initialize();

            var intDic = new ME.ECS.Collections.DataDictionary<int, int>(10);
            var src = intDic.GetHashCode();

            var newDic = PoolDictionaryCopyable<int, int>.Spawn(10);
            intDic.Set(newDic);

            NUnit.Framework.Assert.True(src == intDic.GetHashCode());

            this.DeInitialize();

        }

        [NUnit.Framework.TestAttribute]
        public void ReadData() {

            this.Initialize();

            var intDic = new ME.ECS.Collections.DataDictionary<int, int>(10);
            var dic = intDic.Get();
            for (int i = 0; i < 10; ++i) {

                dic.Add(i, 10 - i);

            }

            var rDic = intDic.Read();
            for (int i = 0; i < rDic.Count; ++i) {

                NUnit.Framework.Assert.True(rDic[i] == 10 - i);

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
