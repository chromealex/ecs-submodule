using UnityEngine;

namespace ME.ECS.Essentials.GOAP {

    [CreateAssetMenu(menuName = "ME.ECS/Addons/GOAP/Actions/Default", order = -1)]
    public class GOAPAction : ScriptableObject {

        public float cost = 1f;
        public PreconditionsData preconditions;
        public EffectsData effects;

        private Precondition? preconditionCache;
        private Effect? effectsCache;

        internal void Dispose() {

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