
namespace ME.ECS.Tests {

    using System.Linq;

    public class IntrusiveListTests {

        public class TestState : State {

            

        }

        private World PrepareWorld() {
            
            World world = null;
            WorldUtilities.CreateWorld<TestState>(ref world, 0.033f);
            {
                world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
                {
                    WorldUtilities.InitComponentTypeId<ME.ECS.Views.ViewComponent>(false);
                    WorldUtilities.InitComponentTypeId<ME.ECS.Name.Name>(false);
                    WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveListNode>(false);
                    world.GetStructComponents().Validate<ME.ECS.Views.ViewComponent>();
                    world.GetStructComponents().Validate<ME.ECS.Name.Name>();
                    world.GetStructComponents().Validate<ME.ECS.Collections.IntrusiveListNode>();
                    ComponentsInitializerWorld.Setup((e) => {
                
                        e.ValidateData<ME.ECS.Views.ViewComponent>();
                        e.ValidateData<ME.ECS.Name.Name>();
                        e.ValidateData<ME.ECS.Collections.IntrusiveListNode>();
                
                    });
                }
            }
            
            WorldUtilities.SetWorld(world);

            return world;

        }

        private void CompleteWorld(World world) {
            
            world.SaveResetState<TestState>();
            
            world.SetFromToTicks(0, 1);
            world.Update(1f);

        }

        [NUnit.Framework.TestAttribute]
        public void Add() {

            var world = this.PrepareWorld();
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(new Entity("data1"));
            list.Add(new Entity("data2"));
            list.Add(new Entity("data3"));
            list.Add(new Entity("data4"));
            list.Add(new Entity("data5"));
            
            UnityEngine.Debug.Assert(list.Count == 5);

            this.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Remove() {

            var world = this.PrepareWorld();

            var first = new Entity("data1");
            var e = new Entity("data3");
            var last = new Entity("data5");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(first);
            list.Add(new Entity("data2"));
            list.Add(e);
            list.Add(new Entity("data4"));
            list.Add(last);

            list.Remove(e);
            UnityEngine.Debug.Assert(list.Count == 4);

            list.Remove(first);
            UnityEngine.Debug.Assert(list.Count == 3);

            list.Remove(last);
            UnityEngine.Debug.Assert(list.Count == 2);

            this.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Insert() {

            var world = this.PrepareWorld();

            var first = new Entity("data1");
            var e = new Entity("data3");
            var last = new Entity("data5");
            var insert = new Entity("data6");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(first);
            list.Add(new Entity("data2"));
            list.Add(e);
            list.Add(new Entity("data4"));
            list.Add(last);

            UnityEngine.Debug.Assert(list.Insert(insert, 2));
            UnityEngine.Debug.Assert(list.Count == 6);

            UnityEngine.Debug.Assert(list.Insert(insert, 6));
            UnityEngine.Debug.Assert(list.Count == 7);

            UnityEngine.Debug.Assert(list.Insert(insert, 0));
            UnityEngine.Debug.Assert(list.Count == 8);

            UnityEngine.Debug.Assert(list.Insert(insert, 10) == false);
            UnityEngine.Debug.Assert(list.Count == 8);

            this.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Clear() {

            var world = this.PrepareWorld();

            var first = new Entity("data1");
            var e = new Entity("data3");
            var last = new Entity("data5");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(first);
            list.Add(new Entity("data2"));
            list.Add(e);
            list.Add(new Entity("data4"));
            list.Add(last);

            list.Clear();
            UnityEngine.Debug.Assert(list.Count == 0);

            this.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Contains() {

            var world = this.PrepareWorld();

            var first = new Entity("data1");
            var e = new Entity("data3");
            var notE = new Entity("data3.1");
            var last = new Entity("data5");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(first);
            list.Add(new Entity("data2"));
            list.Add(e);
            list.Add(new Entity("data4"));
            list.Add(last);

            UnityEngine.Debug.Assert(list.Contains(e) == true);
            UnityEngine.Debug.Assert(list.Contains(notE) == false);

            this.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void RemoveAll() {

            var world = this.PrepareWorld();

            var first = new Entity("data1");
            var e = new Entity("data3");
            var notE = new Entity("data3.1");
            var last = new Entity("data5");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(first);
            list.Add(e);
            list.Add(new Entity("data2"));
            list.Add(e);
            list.Add(new Entity("data4"));
            list.Add(e);
            list.Add(last);

            UnityEngine.Debug.Assert(list.RemoveAll(notE) == false);
            UnityEngine.Debug.Assert(list.Count == 7);

            UnityEngine.Debug.Assert(list.RemoveAll(e) == true);
            UnityEngine.Debug.Assert(list.Count == 4);

            this.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void RemoveAt() {

            var world = this.PrepareWorld();

            var first = new Entity("data1");
            var e = new Entity("data3");
            var last = new Entity("data5");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(first);
            list.Add(e);
            list.Add(new Entity("data2"));
            list.Add(e);
            list.Add(new Entity("data4"));
            list.Add(e);
            list.Add(last);

            UnityEngine.Debug.Assert(list.RemoveAt(3) == true);
            UnityEngine.Debug.Assert(list.Count == 6);

            UnityEngine.Debug.Assert(list.RemoveAt(10) == false);
            UnityEngine.Debug.Assert(list.Count == 6);

            UnityEngine.Debug.Assert(list.RemoveAt(0) == true);
            UnityEngine.Debug.Assert(list.Count == 5);

            this.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void RemoveRange() {

            var world = this.PrepareWorld();

            var first = new Entity("data1");
            var e = new Entity("data3");
            var last = new Entity("data5");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(first);
            list.Add(e);
            list.Add(new Entity("data2"));
            list.Add(e);
            list.Add(new Entity("data4"));
            list.Add(e);
            list.Add(last);

            UnityEngine.Debug.Assert(list.RemoveRange(0, 2) == true);
            UnityEngine.Debug.Assert(list.Count == 5);

            UnityEngine.Debug.Assert(list.RemoveRange(-2, 2) == false);
            UnityEngine.Debug.Assert(list.Count == 5);

            UnityEngine.Debug.Assert(list.RemoveRange(-2, -1) == false);
            UnityEngine.Debug.Assert(list.Count == 5);

            UnityEngine.Debug.Assert(list.RemoveRange(3, 10) == true);
            UnityEngine.Debug.Assert(list.Count == 3);

            this.CompleteWorld(world);

        }

    }

}