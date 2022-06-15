using UnityEngine;

namespace ME.ECS.Essentials.GOAP {

    [CreateAssetMenu(menuName = "ME.ECS/Addons/GOAP/Group")]
    public class GOAPGroup : ScriptableObject {

        public GOAPAction[] actions;
        public GOAPGoal[] goals;

        internal void DoAwake() {

            for (int i = 0; i < this.actions.Length; ++i) {

                this.actions[i].DoAwake();

            }
            
        }

        internal void Dispose() {

            for (int i = 0; i < this.actions.Length; ++i) {

                this.actions[i].Dispose();

            }
            
        }

    }

}