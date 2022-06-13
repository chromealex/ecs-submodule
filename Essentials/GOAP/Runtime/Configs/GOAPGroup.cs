using UnityEngine;

namespace ME.ECS.Essentials.GOAP {

    [CreateAssetMenu(menuName = "ME.ECS/Addons/GOAP/Group")]
    public class GOAPGroup : ScriptableObject {

        public GOAPAction[] actions;
        public GOAPGoal goal;

        internal void Dispose() {

            for (int i = 0; i < this.actions.Length; ++i) {

                this.actions[i].Dispose();

            }
            
        }

    }

}