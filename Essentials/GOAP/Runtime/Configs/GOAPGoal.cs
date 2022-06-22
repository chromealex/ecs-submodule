using UnityEngine;

namespace ME.ECS.Essentials.GOAP {

    [CreateAssetMenu(menuName = "ME.ECS/Addons/GOAP/Goal")]
    public class GOAPGoal : ScriptableObject {

        [Tooltip("Larger weight means better goal")]
        [Min(0f)]
        public float weight = 1f;
        public GoalData data;

        public virtual float GetWeight(in Entity agent) {

            //if agent has<HP>().value < 50% return float.Max
            //if agent has<HP>().value < 20% return float.Max
            //if agent has<ATTACKED>() return float.Max
            return this.weight;

        }
        
    }

}