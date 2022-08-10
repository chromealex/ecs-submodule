using ME.ECS.GlobalEvents;
using UnityEngine;

namespace ME.ECS {

    [CreateAssetMenu(menuName = "ME.ECS/Global Event")]
    public class GlobalEvent : ScriptableObject {

        private static System.Collections.Generic.Dictionary<uint, GlobalEvent> staticEvents = new System.Collections.Generic.Dictionary<uint, GlobalEvent>();

        public uint id;
        public bool debugMode;
        public bool callOnRollback = true;
        public GlobalEvent[] callOthers = new GlobalEvent[0];
        
        public delegate void GlobalEventDelegate(in Entity entity);
        public event GlobalEventDelegate events;

        public static GlobalEvent GetEventById(uint id) {

            if (GlobalEvent.staticEvents.TryGetValue(id, out var value) == true) {

                return value;

            }

            return null;

        }
        
        public override int GetHashCode() {
            
            return this.id > 0u ? (int)this.id : base.GetHashCode();
            
        }

        #if UNITY_EDITOR
        public void OnValidate() {

            if (this.id == 0u) {

                var path = UnityEditor.AssetDatabase.GetAssetPath(this);
                this.id = MathUtils.GetHash(path);
                UnityEditor.EditorUtility.SetDirty(this);

            }
            
        }
        #endif

        public static void ResetCache() {

            foreach (var item in GlobalEvent.staticEvents) {

                item.Value.events = null;

            }

            GlobalEvent.staticEvents.Clear();

        }

        private static GlobalEvent GetInstance(GlobalEvent globalEvent) {

            if (globalEvent.id > 0u) {

                if (GlobalEvent.staticEvents.TryGetValue(globalEvent.id, out var evt) == true && evt != null) {
                    
                    return evt;
                    
                }

                GlobalEvent.staticEvents.Remove(globalEvent.id);
                GlobalEvent.staticEvents.Add(globalEvent.id, globalEvent);
                
            }

            return globalEvent;

        }

        public virtual void Subscribe(GlobalEventDelegate callback) {

            var evt = GlobalEvent.GetInstance(this);
            evt.events += callback;

        }
        
        public virtual void Unsubscribe(GlobalEventDelegate callback) {

            var evt = GlobalEvent.GetInstance(this);
            evt.events -= callback;
            
        }

        public virtual void Run(in Entity entity) {
            
            var evt = GlobalEvent.GetInstance(this);
            if (evt.events != null) evt.events.Invoke(entity);
            
        }

        public bool Cancel() {
            
            return this.Cancel(in Entity.Empty, ME.ECS.GlobalEvents.GlobalEventType.Visual);
            
        }

        public bool Cancel(in Entity entity) {
            
            return this.Cancel(in entity, ME.ECS.GlobalEvents.GlobalEventType.Visual);
            
        }

        public virtual bool Cancel(in Entity entity, ME.ECS.GlobalEvents.GlobalEventType globalEventType) {
            
            var evt = GlobalEvent.GetInstance(this);
            
            for (int i = 0; i < evt.callOthers.Length; ++i) evt.callOthers[i].Cancel(in entity, globalEventType);

            return Worlds.currentWorld.CancelGlobalEvent(evt, in entity, globalEventType);
            
        }

        public void Execute() {
            
            this.Execute(in Entity.Empty, GlobalEventType.Visual);
            
        }

        public void Execute(in Entity entity) {
            
            this.Execute(in entity, GlobalEventType.Visual);
            
        }

        public virtual void Execute(in Entity entity, GlobalEventType globalEventType) {
            
            var evt = GlobalEvent.GetInstance(this);
            
            // If we are reverting - skip visual events
            if (this.callOnRollback == false && globalEventType == GlobalEventType.Visual) {

                if (Worlds.currentWorld.HasResetState() == true) {

                    if (Worlds.currentWorld.GetModule<ME.ECS.Network.INetworkModuleBase>().IsReverting() == true) {

                        return;
                        
                    }

                }

            }

            if (evt.debugMode == true) {
                
                UnityEngine.Debug.Log($"[GlobalEvent] Execute called on {evt.name} (#{evt.id}) with entity {entity}");
                
            }
            
            for (int i = 0; i < evt.callOthers.Length; ++i) evt.callOthers[i].Execute(in entity, globalEventType);

            Worlds.currentWorld.RegisterGlobalEvent(evt, in entity, globalEventType);
            
        }

    }

}
