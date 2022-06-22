using UnityEngine;

namespace ME.ECS.Essentials.GOAP {

    [CreateAssetMenu(menuName = "ME.ECS/Addons/GOAP/Goal")]
    public class GOAPGoal : ScriptableObject {

        [Tooltip("Larger weight means better goal")]
        [Min(0f)]
        public float weight = 1f;
        public GoalData data;

        public virtual float GetWeight(in Entity agent) {

            return this.weight;

        }
        
    }

}