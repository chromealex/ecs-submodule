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

        public void Execute() {
            
            this.Execute(in Entity.Empty, World.GlobalEventType.Visual);
            
        }

        public void Execute(in Entity entity) {
            
            this.Execute(in entity, World.GlobalEventType.Visual);
            
        }

        public void Execute(in Entity entity, World.GlobalEventType globalEventType) {
            
            for (int i = 0; i < this.callOthers.Length; ++i) this.callOthers[i].Execute(in entity, globalEventType);

            Worlds.currentWorld.RegisterGlobalEventFrame(this, in entity, globalEventType);
            
        }

    }

}
