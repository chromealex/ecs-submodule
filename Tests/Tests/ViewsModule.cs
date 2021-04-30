
using TestView1 = ECS.submodule.Tests.TestView1;
using TestView2 = ECS.submodule.Tests.TestView2;

namespace ME.ECS.Tests {

    public class ViewsModule {

        private class TestState : State {}

        public struct HasView : IStructComponentBase {} 

        private class TestSystem : ISystem, IAdvanceTick {

            public World world { get; set; }

            public ViewId viewId;

            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").Without<HasView>().With<ME.ECS.Views.ViewComponent>().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {

                foreach (var entity in this.filter) {

                    UnityEngine.Debug.Log("Set new view");
                    entity.DestroyAllViews();
                    entity.InstantiateView(this.viewId);

                }

            }

        }

        [NUnit.Framework.TestAttribute]
        public void AddRemoveWithTheSameEntity() {

            var prefab1 = UnityEngine.Resources.Load<TestView1>("TestView1");
            var prefab2 = UnityEngine.Resources.Load<TestView2>("TestView2");

            Entity testEntity;
            World world = null;
            WorldUtilities.CreateWorld<TestState>(ref world, 0.033f);
            {
                world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
                world.SetSeed(1u);
                {
                    ref var str = ref world.GetStructComponents();
                    CoreComponentsInitializer.InitTypeId();
                    CoreComponentsInitializer.Init(ref str);
                    WorldUtilities.InitComponentTypeId<HasView>();
                    str.Validate<HasView>();
                    testEntity = new Entity("Test");
                    var viewId1 = world.RegisterViewSource(prefab1);
                    testEntity.InstantiateView(viewId1);
                    
                    var group = new SystemGroup(world, "TestGroup");
                    group.AddSystem(new TestSystem() { viewId = world.RegisterViewSource(prefab2) });

                }
            }
            world.SaveResetState<TestState>();
            
            world.SetFromToTicks(0, 1);
            
            world.PreUpdate(1f);
            world.Update(1f);
            world.LateUpdate(1f);

            var viewsModule = world.GetModule<ME.ECS.Views.ViewsModule>() as ME.ECS.Views.IViewModuleBase;
            var list = viewsModule.GetData();
            var rendering = viewsModule.GetRendering();

            foreach (var item in list) {

                if (item.isNotEmpty == true) UnityEngine.Debug.Log(item.mainView);

            }
            
            foreach (var item in rendering) {

                UnityEngine.Debug.Log(item);

            }
            
        }

    }
}
