using UnityEngine;

namespace ME.ECS {

    [CreateAssetMenu(menuName = "ME.ECS/Global Event")]
    public class GlobalEvent : ScriptableObject {

        public bool debugMode;
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

        public bool Cancel() {
            
            return this.Cancel(in Entity.Empty, World.GlobalEventType.Visual);
            
        }

        public bool Cancel(in Entity entity) {
            
            return this.Cancel(in entity, World.GlobalEventType.Visual);
            
        }

        public bool Cancel(in Entity entity, World.GlobalEventType globalEventType) {
            
            for (int i = 0; i < this.callOthers.Length; ++i) this.callOthers[i].Cancel(in entity, globalEventType);

            return Worlds.currentWorld.CancelGlobalEvent(this, in entity, globalEventType);
            
        }

        public void Execute() {
            
            this.Execute(in Entity.Empty, World.GlobalEventType.Visual);
            
        }

        public void Execute(in Entity entity) {
            
            this.Execute(in entity, World.GlobalEventType.Visual);
            
        }

        public void Execute(in Entity entity, World.GlobalEventType globalEventType) {
            
            // If we are reverting - skip visual events
            if (globalEventType == World.GlobalEventType.Visual) {

                if (Worlds.currentWorld.HasResetState() == true) {

                    if (Worlds.currentWorld.GetModule<ME.ECS.Network.INetworkModuleBase>().IsReverting() == true) {

                        return;
                        
                    }

                }

            }

            if (this.debugMode == true) {
                
                UnityEngine.Debug.Log($"[GlobalEvent] Execute called on {this.name} with entity {entity}");
                
            }
            
            for (int i = 0; i < this.callOthers.Length; ++i) this.callOthers[i].Execute(in entity, globalEventType);

            Worlds.currentWorld.RegisterGlobalEvent(this, in entity, globalEventType);
            
        }

    }

}
