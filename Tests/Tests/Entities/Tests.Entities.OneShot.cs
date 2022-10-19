using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Tests {

    public class Tests_Entities_OneShot {

        public struct TestOneShotComponent : IComponentOneShot {

            public int a;

        }

        private class TestOneShotSystem_Set : ISystem, IAdvanceTick {

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

        private class TestOneShotSystem_Get : ISystem, IAdvanceTick {

            public Entity entity;
            public int count;

            public World world { get; set; }

            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").With<TestOneShotComponent>().Push();
                this.entity = this.world.AddEntity();
                this.entity.GetOneShot<TestOneShotComponent>();
                this.count = 1;

            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {
                
                NUnit.Framework.Assert.IsTrue(this.filter.Count == this.count);
                this.count = 0;

            }

        }

        [NUnit.Framework.TestAttribute]
        public void OneShot_Set() {

            TestsHelper.Do((w) => {
                
                WorldUtilities.InitComponentTypeId<TestOneShotComponent>(false, isOneShot: true);
                ComponentsInitializerWorld.Setup((e) => {
                
                    e.ValidateDataOneShot<TestOneShotComponent>();
                
                });
                
            }, (w) => {
                
                var group = new SystemGroup(w, "TestGroup");
                var sys = new TestOneShotSystem_Set();
                group.AddSystem(sys);

            }, (w) => {

                var sys = w.GetSystem<TestOneShotSystem_Set>();
                NUnit.Framework.Assert.IsTrue(sys.entity.HasOneShot<TestOneShotComponent>());

            }, (w) => {

                var sys = w.GetSystem<TestOneShotSystem_Set>();
                NUnit.Framework.Assert.IsFalse(sys.entity.HasOneShot<TestOneShotComponent>());

            });
            
        }

        [NUnit.Framework.TestAttribute]
        public void OneShot_Get() {

            TestsHelper.Do((w) => {
                
                WorldUtilities.InitComponentTypeId<TestOneShotComponent>(false, isOneShot: true);
                ComponentsInitializerWorld.Setup((e) => {
                
                    e.ValidateDataOneShot<TestOneShotComponent>();
                
                });
                
            }, (w) => {
                
                var group = new SystemGroup(w, "TestGroup");
                var sys = new TestOneShotSystem_Get();
                group.AddSystem(sys);

            }, (w) => {

                var sys = w.GetSystem<TestOneShotSystem_Get>();
                NUnit.Framework.Assert.IsTrue(sys.entity.HasOneShot<TestOneShotComponent>());

            }, (w) => {

                var sys = w.GetSystem<TestOneShotSystem_Get>();
                NUnit.Framework.Assert.IsFalse(sys.entity.HasOneShot<TestOneShotComponent>());

            });
            
        }

    }

}