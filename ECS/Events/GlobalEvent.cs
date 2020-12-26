using UnityEngine;

namespace ME.ECS {

    [CreateAssetMenu(menuName = "ME.ECS/Global Event")]
    public class GlobalEvent : ScriptableObject {

        public GlobalEvent[] callOthers = new GlobalEvent[0];
        
        public delegate void GlobalEventDelegate(in Entity entity);
        public event GlobalEventDelegate events;
        
        public void Subscribe(GlobalEventDelegate callback) {

            this.events += callback;

        }
        
        public void Unsubscribe(GlobalEventDelegate callback) {
            
            this.events -= callback;
            
        }

        public void Run(in Entity entity) {
            
            if (this.events != null) this.events.Invoke(entity);
            
        }
        
        public void Execute(in Entity entity) {
            
            for (int i = 0; i < this.callOthers.Length; ++i) this.callOthers[i].Execute(in entity);

            Worlds.currentWorld.RegisterGlobalEventFrame(this, in entity);
            
        }

    }

}
