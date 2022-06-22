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

    [CreateAssetMenu(menuName = "ME.ECS/Addons/GOAP/Actions/Default", order = -1)]
    public class GOAPAction : ScriptableObject {

        [Tooltip("Larger cost means longer action")]
        [Min(0f)]
        public float cost = 1f;
        [UnityEngine.Serialization.FormerlySerializedAsAttribute("preconditions")] public ConditionsData conditions;
        public EffectsData effects;

        private Condition? conditionCache;
        private Effect? effectsCache;
        private bool isInitialized;

        private GOAPFeature feature;

        internal void DoAwake() {

            if (this.isInitialized == false) {

                this.isInitialized = true;
                
                Worlds.current.GetFeature(out this.feature);
                
                this.OnAwake();
            }

        }
        
        protected virtual void OnAwake() {
            
        }
        
        protected internal virtual void Dispose() {

            this.isInitialized = false;
            
            if (this.conditionCache.HasValue == true) {
                this.conditionCache.Value.Dispose();
                this.conditionCache = null;
            }

            if (this.effectsCache.HasValue == true) {
                this.effectsCache.Value.Dispose();
                this.effectsCache = null;
            }
            
        }

        public virtual bool CanRunPrepare(in Entity agent) => true;

        public virtual tfloat GetCost(in Entity agent) => this.cost;
        
        public virtual bool IsDone(in Entity agent) => true;
        
        public virtual void PerformBegin(in Entity agent) {
            if(this.feature.showDebug)
                UnityEngine.Debug.Log("PerformBegin: " + agent + " :: " + this);
        }

        public virtual void Perform(in Entity agent) {
            if(this.feature.showDebug)
                UnityEngine.Debug.Log("Perform: " + agent + " :: " + this);
        }

        public virtual void OnComplete(in Entity agent) {
            if(this.feature.showDebug)
                UnityEngine.Debug.Log("OnComplete: " + agent + " :: " + this);
        }

        public Condition GetPreconditions(Unity.Collections.Allocator allocator) {

            if (this.conditionCache.HasValue == false) {
                
                this.conditionCache = Condition.CreateFromData(this.conditions).Push(Unity.Collections.Allocator.Persistent);
                
            }

            return new Condition(this.conditionCache.Value, allocator);

        }

        public Effect GetEffects(Unity.Collections.Allocator allocator) {

            if (this.effectsCache == null) {
                
                this.effectsCache = Effect.CreateFromData(this.effects).Push(Unity.Collections.Allocator.Persistent);
                
            }

            return new Effect(this.effectsCache.Value, allocator);

        }

    }

}