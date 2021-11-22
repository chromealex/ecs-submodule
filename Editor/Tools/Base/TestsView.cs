namespace ME.ECSEditor.Tools {

    using UnityEngine.UIElements;
    using System.Collections.Generic;
    
    public class TestsView : VisualElement {

        private ScrollView scrollView;

        private List<TestItem> collectedComponents = new List<TestItem>();
        private Tester tester;

        private List<Button> buttons = new List<Button>();
        private Label status;

        public TestsView(System.Func<List<TestItem>> collect, bool drawCollectButton) {

            this.tester = new Tester();
            this.tester.Collect();
            
            System.Action collectEvent = () => {

                this.collectedComponents = collect.Invoke();
                this.tester.SetTests(this.collectedComponents);

                this.DrawComponents(this.scrollView.contentContainer);

            };

            var container = this;
            {
                if (drawCollectButton == true) {

                    var collectButton = new Button(collectEvent);
                    collectButton.text = "Collect Components";
                    collectButton.AddToClassList("collect-button");
                    container.Add(collectButton);

                }

                var runAllButton = new Button(this.RunAllTests);
                runAllButton.text = "Run All Tests";
                runAllButton.AddToClassList("run-all-button");
                container.Add(runAllButton);

                var status = new Label();
                container.Add(status);
                status.AddToClassList("summary-status");
                status.text = $"All: -, Passed: -, Failed: -";
                this.status = status;

            }

            {
                this.scrollView = new ScrollView();
                this.scrollView.AddToClassList("scroll-view");
                container.Add(this.scrollView);
            }

            if (drawCollectButton == false) {
                
                collectEvent.Invoke();

            }

        }
        
        public void Update() {

            var info = this.tester.GetInfo();
            this.status.text = $"All: <b><color=#fff>{info.all}</color></b>, Passed: <b><color=#6f6>{info.passed}</color></b>, Failed: <b><color=#f66>{info.failed}</color></b>";

        }

        private void DrawComponents(VisualElement container) {
            
            container.Clear();

            this.buttons.Clear();
            foreach (var testItem in this.collectedComponents) {
                        
                var itemContainer = new Button();
                this.buttons.Add(itemContainer);
                itemContainer.RegisterCallback<ClickEvent, TestItem>((evt, t) => {
                    
                    this.RunTest(t);
                    
                }, testItem);
                itemContainer.AddToClassList("component-item");
                {
                    var item = new Label();
                    item.AddToClassList("caption");
                    item.text = testItem.type.Name;
                    itemContainer.Add(item);
                }
                {
                    var item = new Label();
                    item.AddToClassList("type");
                    item.text = testItem.type.FullName;
                    itemContainer.Add(item);
                }
                {
                    var item = new Label();
                    item.AddToClassList("fields");
                    item.text = $"Fields: {testItem.type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Length}";
                    itemContainer.Add(item);
                }
                
                var tests = new VisualElement();
                tests.AddToClassList("tests");
                var i = 0;
                foreach (var test in testItem.tests) {

                    var caption = test.method.ToString();
                    var item = new VisualElement();
                    item.AddToClassList("test" + (i + 1));
                    item.AddToClassList("test-label");
                    var status0 = new Label();
                    status0.AddToClassList("test-status-running");
                    status0.text = $"{caption} Test: Running";
                    item.Add(status0);
                    var status1 = new Label();
                    status1.AddToClassList("test-status-none");
                    status1.text = $"{caption} Test: No Status";
                    item.Add(status1);
                    var status2 = new Label();
                    status2.AddToClassList("test-status-passed");
                    status2.text = $"{caption} Test: Passed";
                    item.Add(status2);
                    var status3 = new Label();
                    status3.AddToClassList("test-status-failed");
                    status3.text = $"{caption} Test: Failed";
                    item.Add(status3);
                    tests.Add(item);
                    
                    ++i;
                    
                }
                itemContainer.Add(tests);
                
                container.Add(itemContainer);
                
            }
            
        }

        public void RunAllTests() {

            this.tester.RunAllTests(
                onPrepareTest: this.Prepare,
                onCompleteTest: this.Complete,
                onTestMethodBegin: this.OnTestBegin,
                onTestMethodEnd: this.OnTestEnd);
            
        }

        public void RunTest(TestItem testItem) {

            this.tester.RunTest(testItem,
                onPrepareTest: this.Prepare,
                onCompleteTest: this.Complete,
                onTestMethodBegin: this.OnTestBegin,
                onTestMethodEnd: this.OnTestEnd);
            
        }

        private void OnTestBegin(int testIndex, TestItem test, int index) {
            
            var container = this.buttons[testIndex];
            var lbl = container.Q(className: "test" + (index + 1));
            lbl.AddToClassList("status-checking");
            
        }

        private void OnTestEnd(int testIndex, TestItem test, int index, Status status) {
            
            var container = this.buttons[testIndex];
            var lbl = container.Q(className: "test" + (index + 1));
            lbl.RemoveFromClassList("status-checking");
            if (status == Status.Passed) {
                lbl.AddToClassList("status-success");
            } else if (status == Status.Failed) {
                lbl.AddToClassList("status-failed");
            }

        }

        private void Prepare(int testIndex, TestItem test) {

            var container = this.buttons[testIndex];
            container.AddToClassList("status-checking");
            for (int i = 0; i < test.tests.Length; ++i) {
            
                var lbl = container.Q(className: "test" + (i + 1));
                lbl.RemoveFromClassList("status-success");
                lbl.RemoveFromClassList("status-failed");

            }
            
        }

        private void Complete(int testIndex, TestItem test) {
            
            var container = this.buttons[testIndex];
            container.RemoveFromClassList("status-checking");
            for (int i = 0; i < test.tests.Length; ++i) {

                var status = test.tests[i].status;
                var lbl = container.Q(className: "test" + (i + 1));
                if (status == Status.Passed) {
                    lbl.AddToClassList("status-success");
                } else if (status == Status.Failed) {
                    lbl.AddToClassList("status-failed");
                }

            }
            
        }

    }

}