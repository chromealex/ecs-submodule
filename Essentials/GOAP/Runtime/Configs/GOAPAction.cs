using UnityEngine;

namespace ME.ECS.Essentials.GOAP {

    [CreateAssetMenu(menuName = "ME.ECS/Addons/GOAP/Actions/Default", order = -1)]
    public class GOAPAction : ScriptableObject {

        [Tooltip("Larger cost means longer action")]
        [Min(0f)]
        public float cost = 1f;
        public PreconditionsData preconditions;
        public EffectsData effects;

        private Precondition? preconditionCache;
        private Effect? effectsCache;
        private bool isInitialized;

        internal void DoAwake() {

            if (this.isInitialized == false) {

                this.isInitialized = true;
                this.OnAwake();

            }

        }
        
        protected virtual void OnAwake() {
            
        }
        
        protected internal virtual void Dispose() {

            this.isInitialized = false;
            
            if (this.preconditionCache.HasValue == true) {
                this.preconditionCache.Value.Dispose();
                this.preconditionCache = null;
            }

            if (this.effectsCache.HasValue == true) {
                this.effectsCache.Value.Dispose();
                this.effectsCache = null;
            }
            
        }

        public virtual float GetCost(in Entity agent) => this.cost;
        
        public virtual bool IsDone(in Entity agent) => true;
        
        public virtual void PerformBegin(in Entity agent) {
            UnityEngine.Debug.Log("PerformBegin: " + agent + " :: " + this);
        }

        public virtual void Perform(in Entity agent) {
            UnityEngine.Debug.Log("Perform: " + agent + " :: " + this);
        }

        public virtual void OnComplete(in Entity agent) {
            UnityEngine.Debug.Log("OnComplete: " + agent + " :: " + this);
        }

        public Precondition GetPreconditions(Unity.Collections.Allocator allocator) {

            if (this.preconditionCache.HasValue == false) {
                
                this.preconditionCache = Precondition.CreateFromData(this.preconditions).Push(Unity.Collections.Allocator.Persistent);
                
            }

            return new Precondition(this.preconditionCache.Value, allocator);

        }

        public Effect GetEffects(Unity.Collections.Allocator allocator) {

            if (this.effectsCache == null) {
                
                this.effectsCache = Effect.CreateFromData(this.effects).Push(Unity.Collections.Allocator.Persistent);
                
            }

            return new Effect(this.effectsCache.Value, allocator);

        }

    }

}