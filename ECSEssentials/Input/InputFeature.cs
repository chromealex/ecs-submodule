using ME.ECS;

namespace ME.ECS.Essentials {

    using Input.Components; using Input.Modules; using Input.Systems; using Input.Markers;
    
    namespace Input.Components {}
    namespace Input.Modules {}
    namespace Input.Systems {}
    namespace Input.Markers {}

    public enum InputEventType {

        Unknown = 0,
        PointerDown,
        PointerUp,
        PointerClick,
        PointerDragBegin,
        PointerDragMove,
        PointerDragEnd,

    }
    
    [System.Serializable]
    public struct InputPointerData {

        public int pointerId;
        public UnityEngine.Vector3 worldPosition;
        public UnityEngine.Vector3 pressWorldPosition;
        public InputEventType eventType;

        public InputPointerData(int pointerId, UnityEngine.Vector3 worldPosition, InputEventType eventType) {

            this.pointerId = pointerId;
            this.worldPosition = worldPosition;
            this.pressWorldPosition = worldPosition;
            this.eventType = eventType;

        }

    }

    public struct InputAction<TMarker, TComponent> where TMarker : struct, ME.ECS.Essentials.Input.Input.Markers.IInputPointerMarker where TComponent : struct, IInputPointerComponent {

        private readonly ME.ECS.Network.INetworkModuleBase networkModule;
        private readonly RPCId rpcId;
        private readonly object networkObject;
        private readonly object tag;
        private System.Func<Entity> getEntity;
        
        public InputAction(InputFeature feature, ME.ECS.Network.INetworkModuleBase networkModule, System.Action<Entity, TMarker> rpc) {

            this.networkModule = networkModule;
            this.networkObject = feature;
            this.getEntity = null;
            this.rpcId = default;
            this.tag = default;
            
            this.tag = this.networkObject;
            this.rpcId = this.networkModule.RegisterRPC(rpc.Method);

        }

        public void SetPlayerEntityReceiver(System.Func<Entity> getEntity) {

            this.getEntity = getEntity;

        }

        public void Execute() {

            var world = Worlds.currentWorld;
            if (world.GetMarker(out TMarker marker) == true) {

                this.Execute(marker);

            }

        }

        private void Execute(TMarker marker) {

            var entity = this.getEntity.Invoke();
            this.networkModule.RPC(this.tag, this.rpcId, entity, marker);

        }

        public void RPC(Entity player, TMarker marker) {
            
            //UnityEngine.Debug.Log("RPC: " + marker + " :: " + typeof(TComponent) + " on entity " + player);
            player.Set(new TComponent() {
                setData = marker.data,
            }, ComponentLifetime.NotifyAllSystems);
            
        }

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class InputFeature : Feature {

        [UnityEngine.Header("World raycast")]
        public UnityEngine.LayerMask raycastMask = -1;
        public float raycastDistance = 1000f;

        [UnityEngine.Header("Events")]
        public bool pointerClick;
        public bool pointerUp;
        public bool pointerDown;

        [UnityEngine.Header("Events (Double-Click)")]
        public bool pointerDoubleClick;
        [UnityEngine.MinAttribute(0.01f)]
        [UnityEngine.TooltipAttribute("Double click threshold in seconds")]
        public float doubleClickThreshold = 0.2f;
        public float doubleClickMaxDistance = 1f;
        
        [UnityEngine.Header("Events (Drag)")]
        public bool pointerDragBegin;
        public bool pointerDragMove;
        public bool pointerDragEnd;
        [UnityEngine.MinAttribute(0.01f)]
        [UnityEngine.TooltipAttribute("Drag begin threshold in world meters")]
        public float dragBeginThreshold = 0.2f;
        [UnityEngine.MinAttribute(0.01f)]
        [UnityEngine.TooltipAttribute("Drag move threshold in world meters")]
        public float dragMoveThreshold = 0.2f;
        
        private InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerClick, InputPointerClick> pointerClickEvent;
        private InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDoubleClick, InputPointerDoubleClick> pointerDoubleClickEvent;
        private InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDragBegin, InputPointerDragBegin> pointerDragBeginEvent;
        private InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDragMove, InputPointerDragMove> pointerDragMoveEvent;
        private InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDragEnd, InputPointerDragEnd> pointerDragEndEvent;
        private InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerUp, InputPointerUp> pointerUpEvent;
        private InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDown, InputPointerDown> pointerDownEvent;

        internal UnityEngine.Camera camera;

        private System.Collections.Generic.Dictionary<InputEventType, System.Action<InputPointerData>> actions = new System.Collections.Generic.Dictionary<InputEventType, System.Action<InputPointerData>>();

        public void RaiseMarkerCallback(InputPointerData data) {

            if (this.actions.TryGetValue(data.eventType, out var list) == true) {

                list.Invoke(data);
                
            }
            
        }

        public void AddMarkerCallback(InputEventType eventType, System.Action<InputPointerData> onMarkerCreate) {

            if (this.actions.TryGetValue(eventType, out var list) == true) {

                list += onMarkerCreate;
                this.actions[eventType] = list;

            } else {
            
                this.actions.Add(eventType, onMarkerCreate);
            
            }
            
        }

        public void RemoveMarkerCallback(InputEventType eventType, System.Action<InputPointerData> onMarkerCreate) {

            if (this.actions.TryGetValue(eventType, out var list) == true) {

                list -= onMarkerCreate;
                this.actions[eventType] = list;

            }
            
        }

        public void SetMarkerCallback(InputEventType eventType, System.Action<InputPointerData> onMarkerCreate) {

            if (this.actions.ContainsKey(eventType) == true) {

                this.actions[eventType] = onMarkerCreate;

            } else {
            
                this.actions.Add(eventType, onMarkerCreate);
            
            }
            
        }

        public void SetCamera(UnityEngine.Camera camera) {
            
            this.camera = camera;
            
        }

        public void SetPlayerEntityReceiver(System.Func<Entity> getEntity) {

            if (this.pointerClick == true) this.pointerClickEvent.SetPlayerEntityReceiver(getEntity);
            if (this.pointerDoubleClick == true) this.pointerDoubleClickEvent.SetPlayerEntityReceiver(getEntity);
            if (this.pointerDragBegin == true) this.pointerDragBeginEvent.SetPlayerEntityReceiver(getEntity);
            if (this.pointerDragMove == true) this.pointerDragMoveEvent.SetPlayerEntityReceiver(getEntity);
            if (this.pointerDragEnd == true) this.pointerDragEndEvent.SetPlayerEntityReceiver(getEntity);
            if (this.pointerUp == true) this.pointerUpEvent.SetPlayerEntityReceiver(getEntity);
            if (this.pointerDown == true) this.pointerDownEvent.SetPlayerEntityReceiver(getEntity);

        }

        public void Execute() {
            
            if (this.pointerClick == true) this.pointerClickEvent.Execute();
            if (this.pointerDoubleClick == true) this.pointerDoubleClickEvent.Execute();
            if (this.pointerDragBegin == true) this.pointerDragBeginEvent.Execute();
            if (this.pointerDragMove == true) this.pointerDragMoveEvent.Execute();
            if (this.pointerDragEnd == true) this.pointerDragEndEvent.Execute();
            if (this.pointerUp == true) this.pointerUpEvent.Execute();
            if (this.pointerDown == true) this.pointerDownEvent.Execute();

        }
        
        protected override void OnConstruct() {
            
            var net = this.world.GetModule<ME.ECS.Network.INetworkModuleBase>();
            net.RegisterObject(this);
            if (this.pointerClick == true) this.pointerClickEvent = new InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerClick, InputPointerClick>(this, net, this.RPC);
            if (this.pointerDoubleClick == true) this.pointerDoubleClickEvent = new InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDoubleClick, InputPointerDoubleClick>(this, net, this.RPC);
            if (this.pointerDragBegin == true) this.pointerDragBeginEvent = new InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDragBegin, InputPointerDragBegin>(this, net, this.RPC);
            if (this.pointerDragMove == true) this.pointerDragMoveEvent = new InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDragMove, InputPointerDragMove>(this, net, this.RPC);
            if (this.pointerDragEnd == true) this.pointerDragEndEvent = new InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDragEnd, InputPointerDragEnd>(this, net, this.RPC);
            if (this.pointerUp == true) this.pointerUpEvent = new InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerUp, InputPointerUp>(this, net, this.RPC);
            if (this.pointerDown == true) this.pointerDownEvent = new InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDown, InputPointerDown>(this, net, this.RPC);

            this.AddModule<ME.ECS.Essentials.Input.Input.Modules.InputModule>();
            this.AddSystem<ME.ECS.Essentials.Input.Input.Systems.SendMessagesSystem>();

        }

        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputPointerClick marker) { this.pointerClickEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputPointerDoubleClick marker) { this.pointerDoubleClickEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputPointerDragBegin marker) { this.pointerDragBeginEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputPointerDragMove marker) { this.pointerDragMoveEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputPointerDragEnd marker) { this.pointerDragEndEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputPointerUp marker) { this.pointerUpEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputPointerDown marker) { this.pointerDownEvent.RPC(player, marker); }

        protected override void OnDeconstruct() {
            
            this.actions.Clear();
            this.camera = null;

        }

    }

}