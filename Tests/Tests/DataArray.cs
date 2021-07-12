
namespace ME.ECS.Tests {

    public class DataArrayTests {
        
        [NUnit.Framework.TestAttribute]
        public void WriteData() {

            var arr = new ME.ECS.Collections.DataArray<int>(10);
            var src = arr.GetHashCode();
            for (int i = 0; i < arr.Length; ++i) {
                
                arr[i] = i;
                
            }
            NUnit.Framework.Assert.True(src != arr.GetHashCode());

        }

        [NUnit.Framework.TestAttribute]
        public void ReadData() {

            var arr = new ME.ECS.Collections.DataArray<int>(10);
            for (int i = 0; i < arr.Length; ++i) {
                
                arr[i] = i;
                
            }

            for (int i = 0; i < arr.Length; ++i) {
                
                NUnit.Framework.Assert.True(arr[i] == i);
                
            }
            
        }

    }

}