#if FIXED_POINT_MATH
using math = ME.ECS.Mathematics.math;
using float3 = ME.ECS.Mathematics.float3;
using tfloat = sfloat;
#else
using math = Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;
using tfloat = System.Single;
#endif

using UnityEngine;

namespace ME.ECS.Essentials.GOAP {

    [System.Flags]
    public enum ActionEvent {

        None = 0,
        PerformBegin = 1 << 0,
        Perform = 1 << 1,
        PerformComplete = 1 << 2,
        CanRunPrepare = 1 << 3,
        GetCost = 1 << 4,
        IsDone = 1 << 5,
        OnAwake = 1 << 6,
        OnDispose = 1 << 7,

    }
    
    [System.Serializable]
    public struct ModuleGroup {

        public void Run(GOAPActionModule[] items, in Entity agent, ActionEvent evt) {

            for (int i = 0; i < items.Length; ++i) {

                var item = items[i];
                if (item == null) continue;

                switch (evt) {
                    
                    case ActionEvent.PerformBegin:
                        if ((item.requiredEvents & evt) != 0) item.PerformBegin(in agent);
                        break;

                    case ActionEvent.Perform:
                        if ((item.requiredEvents & evt) != 0) item.Perform(in agent);
                        break;

                    case ActionEvent.PerformComplete:
                        if ((item.requiredEvents & evt) != 0) item.OnComplete(in agent);
                        break;

                    case ActionEvent.OnAwake:
                        if ((item.requiredEvents & evt) != 0) item.OnAwake();
                        break;

                    case ActionEvent.OnDispose:
                        if ((item.requiredEvents & evt) != 0) item.OnDispose();
                        break;

                }

            }

        }

        public bool RunBool(GOAPActionModule[] items, in Entity agent, ActionEvent evt, out bool result) {

            result = false;
            var found = false;
            for (int i = 0; i < items.Length; ++i) {

                var item = items[i];
                if (item == null) continue;

                switch (evt) {
                    
                    case ActionEvent.IsDone:
                        if ((item.requiredEvents & evt) != 0) {
                            result = item.IsDone(in agent);
                            found = true;
                        }

                        break;

                    case ActionEvent.CanRunPrepare:
                        if ((item.requiredEvents & evt) != 0) {
                            result = item.CanRunPrepare(in agent);
                            found = true;
                        }

                        break;

                }

            }

            return found;

        }

        public bool RunFloat(GOAPActionModule[] items, in Entity agent, ActionEvent evt, out tfloat result) {

            result = 0f;
            var found = false;
            for (int i = 0; i < items.Length; ++i) {

                var item = items[i];
                if (item == null) continue;

                switch (evt) {
                    
                    case ActionEvent.GetCost:
                        if ((item.requiredEvents & evt) != 0) {
                            result = item.GetCost(in agent);
                            found = true;
                        }

                        break;

                }

            }

            return found;

        }

    }
    
    [System.Serializable]
    public abstract class GOAPActionModule {

        public virtual ActionEvent requiredEvents => ActionEvent.None;

        public virtual void Perform(in Entity agent) {
        }

        public virtual tfloat GetCost(in Entity agent) {
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

        [SerializeReference]
        [SerializeReferenceButton]
        public GOAPActionModule[] items;

        protected override void OnAwake() {
            base.OnAwake();
            this.DoActions(Entity.Null, ActionEvent.OnAwake);
        }
        
        private void DoActions(in Entity agent, ActionEvent evt) {
            new ModuleGroup().Run(this.items, in agent, evt);
        }

        private bool DoActionsBool(in Entity agent, ActionEvent evt, out bool result) {
            result = default;
            return new ModuleGroup().RunBool(this.items, in agent, evt, out result) == true;
        }

        private bool DoActionsFloat(in Entity agent, ActionEvent evt, out tfloat result) {
            result = default;
            return new ModuleGroup().RunFloat(this.items, in agent, evt, out result);
        }

        public override void Perform(in Entity agent) {
            base.Perform(in agent);
            this.DoActions(in agent, ActionEvent.Perform);
        }

        public override tfloat GetCost(in Entity agent) {
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