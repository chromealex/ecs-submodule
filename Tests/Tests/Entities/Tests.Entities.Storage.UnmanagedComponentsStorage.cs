
namespace ME.ECS.Tests {

    public class Tests_Entities_Storage_UnmanagedComponentsStorage {

        public struct TestComponent {

            public int value;

        }
        
        [NUnit.Framework.Test]
        [NUnit.Framework.RepeatAttribute(20)]
        public void Initialize() {

            ME.ECS.Pools.current = new ME.ECS.PoolImplementation(isNull: false);
            var reg = new UnmanagedComponentsStorage();
            reg.Initialize();
            try {
                WorldUtilities.InitComponentTypeId<TestComponent>();
                reg.Validate<TestComponent>();
                for (int i = 0; i < 100; ++i) {
                    reg.Validate<TestComponent>(i);
                }
            } finally {
                reg.Dispose();
            }

        }

        [NUnit.Framework.TestAttribute]
        [NUnit.Framework.RepeatAttribute(20)]
        public void Set() {

            ME.ECS.Pools.current = new ME.ECS.PoolImplementation(isNull: false);
            var reg = new UnmanagedComponentsStorage();
            reg.Initialize();
            try {
                WorldUtilities.InitComponentTypeId<TestComponent>();
                reg.Validate<TestComponent>();
                for (int i = 0; i < 100; ++i) {
                    reg.Validate<TestComponent>(i);
                }

                reg.Set(1, new TestComponent() {
                    value = 123,
                });
                var data = reg.Read<TestComponent>(1);
                NUnit.Framework.Assert.AreEqual(data.value, 123);
            } finally {
                reg.Dispose();
            }

        }

        [NUnit.Framework.TestAttribute]
        [NUnit.Framework.RepeatAttribute(20)]
        public void Read() {

            ME.ECS.Pools.current = new ME.ECS.PoolImplementation(isNull: false);
            var reg = new UnmanagedComponentsStorage();
            reg.Initialize();
            try {
                WorldUtilities.InitComponentTypeId<TestComponent>();
                reg.Validate<TestComponent>();
                for (int i = 0; i < 100; ++i) {
                    reg.Validate<TestComponent>(i);
                }

                var data = reg.Read<TestComponent>(1);
                NUnit.Framework.Assert.AreEqual(data.value, 0);
                var data2 = reg.Read<TestComponent>(2);
                NUnit.Framework.Assert.AreEqual(data2.value, 0);
            } finally {
                reg.Dispose();
            }

        }

        [NUnit.Framework.TestAttribute]
        [NUnit.Framework.RepeatAttribute(20)]
        public void Get() {

            ME.ECS.Pools.current = new ME.ECS.PoolImplementation(isNull: false);
            var reg = new UnmanagedComponentsStorage();
            reg.Initialize();
            try {
                WorldUtilities.InitComponentTypeId<TestComponent>();
                reg.Validate<TestComponent>();
                for (int i = 0; i < 100; ++i) {
                    reg.Validate<TestComponent>(i);
                }

                ref var data = ref reg.Get<TestComponent>(1);
                NUnit.Framework.Assert.AreEqual(data.value, 0);
                var data2 = reg.Read<TestComponent>(2);
                NUnit.Framework.Assert.AreEqual(data2.value, 0);
                data.value = 123;
                var data3 = reg.Read<TestComponent>(1);
                NUnit.Framework.Assert.AreEqual(data3.value, 123);

            } finally {
                reg.Dispose();
            }

        }

        [NUnit.Framework.TestAttribute]
        [NUnit.Framework.RepeatAttribute(20)]
        public void Has() {

            ME.ECS.Pools.current = new ME.ECS.PoolImplementation(isNull: false);
            var reg = new UnmanagedComponentsStorage();
            reg.Initialize();
            try {
                WorldUtilities.InitComponentTypeId<TestComponent>();
                reg.Validate<TestComponent>();
                for (int i = 0; i < 100; ++i) {
                    reg.Validate<TestComponent>(i);
                }

                reg.Set(1, new TestComponent());
                NUnit.Framework.Assert.True(reg.Has<TestComponent>(1));
                NUnit.Framework.Assert.False(reg.Has<TestComponent>(2));

            } finally {
                reg.Dispose();
            }

        }

    }

}