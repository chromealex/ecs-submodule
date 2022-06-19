using UnityEngine;

namespace ME.ECS.Essentials.GOAP {

    public enum ActionEvent {

        PerformBegin,
        Perform,
        PerformComplete,
        CanRunPrepare,
        GetCost,
        IsDone,
        OnAwake,
        OnDispose,

    }
    
    [System.Serializable]
    public struct ModuleGroup {

        public ActionEvent actionEvent;
        [SerializeReference]
        public GOAPActionModule[] items;

        public void Run(in Entity agent, ActionEvent evt) {

            if (this.actionEvent == evt) {

                for (int i = 0; i < this.items.Length; ++i) {

                    var item = this.items[i];
                    if (item == null) continue;

                    switch (evt) {
                        
                        case ActionEvent.PerformBegin:
                            item.PerformBegin(in agent);
                            break;

                        case ActionEvent.Perform:
                            item.Perform(in agent);
                            break;

                        case ActionEvent.PerformComplete:
                            item.OnComplete(in agent);
                            break;

                        case ActionEvent.OnAwake:
                            item.OnAwake();
                            break;

                        case ActionEvent.OnDispose:
                            item.OnDispose();
                            break;

                    }

                }
                
            }
            
        }

        public bool RunBool(in Entity agent, ActionEvent evt, out bool result) {

            result = false;
            if (this.actionEvent == evt) {

                var found = false;
                for (int i = 0; i < this.items.Length; ++i) {

                    var item = this.items[i];
                    if (item == null) continue;

                    switch (evt) {
                        
                        case ActionEvent.IsDone:
                            result = item.IsDone(in agent);
                            found = true;
                            break;

                        case ActionEvent.CanRunPrepare:
                            result = item.CanRunPrepare(in agent);
                            found = true;
                            break;

                    }

                }

                return found;

            }

            return false;

        }

        public bool RunFloat(in Entity agent, ActionEvent evt, out float result) {

            result = 0f;
            if (this.actionEvent == evt) {

                var found = false;
                for (int i = 0; i < this.items.Length; ++i) {

                    var item = this.items[i];
                    if (item == null) continue;

                    switch (evt) {
                        
                        case ActionEvent.GetCost:
                            result = item.GetCost(in agent);
                            found = true;
                            break;

                    }

                }

                return found;

            }

            return false;

        }

    }
    
    [System.Serializable]
    public abstract class GOAPActionModule {

        public virtual void Perform(in Entity agent) {
        }

        public virtual float GetCost(in Entity agent) {
            return 1f;
        }

        public virtual bool IsDone(in Entity agent) {
            return false;
        }

        public virtual void PerformBegin(in Entity agent) {
        }

        public virtual void OnComplete(in Entity agent) {
        }

        public virtual bool CanRunPrepare(in Entity agent) {
            return true;
        }

        public virtual void OnDispose() {
        }

        public virtual void OnAwake() {
        }

    }

    [CreateAssetMenu(menuName = "ME.ECS/Addons/GOAP/Actions/Generic", order = -1)]
    public class GOAPGenericAction : GOAPAction {

        public ModuleGroup[] items;

        private void DoActions(in Entity agent, ActionEvent evt) {
            for (int i = 0; i < this.items.Length; ++i) {
                this.items[i].Run(in agent, evt);
            }
        }

        private bool DoActionsBool(in Entity agent, ActionEvent evt, out bool result) {
            result = default;
            var found = false;
            for (int i = 0; i < this.items.Length; ++i) {
                if (this.items[i].RunBool(in agent, evt, out result) == true) {
                    found = true;
                }
            }

            return found;
        }

        private bool DoActionsFloat(in Entity agent, ActionEvent evt, out float result) {
            result = default;
            var found = false;
            for (int i = 0; i < this.items.Length; ++i) {
                if (this.items[i].RunFloat(in agent, evt, out result) == true) {
                    found = true;
                }
            }

            return found;
        }

        public override void Perform(in Entity agent) {
            base.Perform(in agent);
            this.DoActions(in agent, ActionEvent.Perform);
        }

        public override float GetCost(in Entity agent) {
            if (this.DoActionsFloat(in agent, ActionEvent.GetCost, out var result) == true) {
                return result;
            }
            return base.GetCost(in agent);
        }

        public override bool IsDone(in Entity agent) {
            if (this.DoActionsBool(in agent, ActionEvent.IsDone, out var result) == true) {
                return result;
            }
            return base.IsDone(in agent);
        }

        public override void PerformBegin(in Entity agent) {
            base.PerformBegin(in agent);
            this.DoActions(in agent, ActionEvent.PerformBegin);
        }

        public override void OnComplete(in Entity agent) {
            base.OnComplete(in agent);
            this.DoActions(in agent, ActionEvent.PerformComplete);
        }

        protected internal override void Dispose() {
            base.Dispose();
            this.DoActions(Entity.Null, ActionEvent.OnDispose);
        }

        public override bool CanRunPrepare(in Entity agent) {
            if (this.DoActionsBool(in agent, ActionEvent.CanRunPrepare, out var result) == true) {
                return result;
            }
            return base.CanRunPrepare(in agent);
        }

    }

}