using ME.ECS;

#if FIXED_POINT_MATH
using FLOAT2 = ME.ECS.fp2;
using FLOAT3 = ME.ECS.fp3;
using FLOAT4 = ME.ECS.fp4;
using QUATERNION = ME.ECS.fpquaternion;
#else
using FLOAT2 = UnityEngine.Vector2;
using FLOAT3 = UnityEngine.Vector3;
using FLOAT4 = UnityEngine.Vector4;
using QUATERNION = UnityEngine.Quaternion;
#endif

namespace ME.ECS.Essentials {

    using Input.Components; using Input.Modules; using Input.Systems; using Input.Markers;
    
    namespace Input.Components {}
    namespace Input.Modules {}
    namespace Input.Systems {}
    namespace Input.Markers {}

    public delegate InputPointerData MarkerModifier(InputPointerData data);
    public delegate bool GetWorldPointerCallback(int pointerId, UnityEngine.Camera camera, out FLOAT3 result);
    
    public interface IEventMarker : IMarker {}

    public interface IGestureMarker : IMarker {

        FLOAT3 worldPosition { get; }

    }

    public enum InputEventType {

        Any = 0,
        PointerDown,
        PointerUp,
        PointerClick,
        PointerDoubleClick,
        PointerDragBegin,
        PointerDragMove,
        PointerDragEnd,
        
        GesturePitchDown,
        GesturePitchMove,
        GesturePitchUp,

    }
    
    [System.Serializable]
    public struct InputPointerData {

        public int pointerId;
        public FLOAT3 worldPosition;
        public FLOAT3 pressWorldPosition;
        public InputEventType eventType;

        public InputPointerData(int pointerId, FLOAT3 worldPosition, InputEventType eventType) {

            this.pointerId = pointerId;
            this.worldPosition = worldPosition;
            this.pressWorldPosition = worldPosition;
            this.eventType = eventType;

        }

    }

    public struct InputAction<TMarker, TComponent> where TMarker : struct, ME.ECS.Essentials.Input.Input.Markers.IInputPointerMarker where TComponent : struct, IInputPointerComponent {

        private readonly ME.ECS.Network.INetworkModuleBase networkModule;
        private readonly RPCId rpcId;
        private readonly InputFeature networkObject;
        private readonly object tag;
        private readonly InputEventType inputEventType;
        private System.Func<Entity> getEntity;

        public InputAction(InputFeature feature, InputEventType inputEventType, ME.ECS.Network.INetworkModuleBase networkModule, System.Action<Entity, TMarker> rpc) {

            this.networkModule = networkModule;
            this.networkObject = feature;
            this.getEntity = null;
            this.rpcId = default;
            this.tag = default;
            this.inputEventType = inputEventType;
            
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

            if (this.getEntity != null) {

                var entity = this.getEntity.Invoke();
                this.networkModule.RPC(this.tag, this.rpcId, entity, marker);

            }

        }

        public void RPC(Entity player, TMarker marker) {

            if (this.networkObject.IsAllowed(player, this.inputEventType, marker.data.worldPosition) == true) {

                //UnityEngine.Debug.Log("RPC: " + marker + " :: " + typeof(TComponent) + " on entity " + player + ", position: " + marker.data.worldPosition);
                player.Set(new TComponent() {
                    setData = marker.data,
                }, ComponentLifetime.NotifyAllSystems);
                player.Set<HasAnyInput>(ComponentLifetime.NotifyAllSystems);

            }

        }

    }
    
    public struct InputGesture<TMarker, TComponent> where TMarker : struct, ME.ECS.Essentials.Input.Input.Markers.IInputGesture2FingersMarker where TComponent : struct, IInputGesture2FingersComponent {

        private readonly ME.ECS.Network.INetworkModuleBase networkModule;
        private readonly RPCId rpcId;
        private readonly InputFeature networkObject;
        private readonly object tag;
        private readonly InputEventType inputEventType;
        private System.Func<Entity> getEntity;
        
        public InputGesture(InputFeature feature, InputEventType inputEventType, ME.ECS.Network.INetworkModuleBase networkModule, System.Action<Entity, TMarker> rpc) {

            this.networkModule = networkModule;
            this.networkObject = feature;
            this.getEntity = null;
            this.rpcId = default;
            this.tag = default;
            this.inputEventType = inputEventType;
            
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

            if (this.getEntity != null) {

                var entity = this.getEntity.Invoke();
                this.networkModule.RPC(this.tag, this.rpcId, entity, marker);

            }
            
        }

        public void RPC(Entity player, TMarker marker) {

            if (this.networkObject.IsAllowed(player, this.inputEventType, marker.worldPosition) == true) {

                //UnityEngine.Debug.Log("RPC: " + marker + " :: " + typeof(TComponent) + " on entity " + player + ", position: " + marker.worldPosition);
                player.Set(new TComponent() {
                    setPointer1 = marker.pointer1,
                    setPointer2 = marker.pointer2,
                }, ComponentLifetime.NotifyAllSystems);
                player.Set<HasAnyInput>(ComponentLifetime.NotifyAllSystems);

            }

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
        
        [UnityEngine.Header("Gestures (Pitch)")]
        public bool gesturePitchUp;
        public bool gesturePitchDown;
        public bool gesturePitchMove;
        [UnityEngine.TooltipAttribute("Pitch drag move threshold in world meters")]
        public float gesturePitchMinDragThreshold;
        
        private InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerClick, InputPointerClick> pointerClickEvent;
        private InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDoubleClick, InputPointerDoubleClick> pointerDoubleClickEvent;
        private InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDragBegin, InputPointerDragBegin> pointerDragBeginEvent;
        private InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDragMove, InputPointerDragMove> pointerDragMoveEvent;
        private InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDragEnd, InputPointerDragEnd> pointerDragEndEvent;
        private InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerUp, InputPointerUp> pointerUpEvent;
        private InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDown, InputPointerDown> pointerDownEvent;
        private InputGesture<ME.ECS.Essentials.Input.Input.Markers.InputGesturePitchUp, InputGesturePitchUp> gesturePitchUpEvent;
        private InputGesture<ME.ECS.Essentials.Input.Input.Markers.InputGesturePitchDown, InputGesturePitchDown> gesturePitchDownEvent;
        private InputGesture<ME.ECS.Essentials.Input.Input.Markers.InputGesturePitchMove, InputGesturePitchMove> gesturePitchMoveEvent;
        
        private GetWorldPointerCallback getWorldPointerCallback;

        internal UnityEngine.Camera camera;
        private Filter inputMaskFilter;

        private System.Collections.Generic.Dictionary<InputEventType, MarkerModifier> actions = new System.Collections.Generic.Dictionary<InputEventType, MarkerModifier>();

        protected override void OnConstruct() {
            
            var net = this.world.GetModule<ME.ECS.Network.INetworkModuleBase>();
            net.RegisterObject(this);
            if (this.pointerClick == true) this.pointerClickEvent = new InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerClick, InputPointerClick>(this, InputEventType.PointerClick, net, this.RPC);
            if (this.pointerDoubleClick == true) this.pointerDoubleClickEvent = new InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDoubleClick, InputPointerDoubleClick>(this, InputEventType.PointerDoubleClick, net, this.RPC);
            if (this.pointerDragBegin == true) this.pointerDragBeginEvent = new InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDragBegin, InputPointerDragBegin>(this, InputEventType.PointerDragBegin, net, this.RPC);
            if (this.pointerDragMove == true) this.pointerDragMoveEvent = new InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDragMove, InputPointerDragMove>(this, InputEventType.PointerDragMove, net, this.RPC);
            if (this.pointerDragEnd == true) this.pointerDragEndEvent = new InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDragEnd, InputPointerDragEnd>(this, InputEventType.PointerDragEnd, net, this.RPC);
            if (this.pointerUp == true) this.pointerUpEvent = new InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerUp, InputPointerUp>(this, InputEventType.PointerUp, net, this.RPC);
            if (this.pointerDown == true) this.pointerDownEvent = new InputAction<ME.ECS.Essentials.Input.Input.Markers.InputPointerDown, InputPointerDown>(this, InputEventType.PointerDown, net, this.RPC);

            if (this.gesturePitchDown == true) this.gesturePitchDownEvent = new InputGesture<ME.ECS.Essentials.Input.Input.Markers.InputGesturePitchDown, InputGesturePitchDown>(this, InputEventType.GesturePitchDown, net, this.RPC);
            if (this.gesturePitchMove == true) this.gesturePitchMoveEvent = new InputGesture<ME.ECS.Essentials.Input.Input.Markers.InputGesturePitchMove, InputGesturePitchMove>(this, InputEventType.GesturePitchMove, net, this.RPC);
            if (this.gesturePitchUp == true) this.gesturePitchUpEvent = new InputGesture<ME.ECS.Essentials.Input.Input.Markers.InputGesturePitchUp, InputGesturePitchUp>(this, InputEventType.GesturePitchUp, net, this.RPC);

            this.AddModule<ME.ECS.Essentials.Input.Input.Modules.InputModule>();
            this.AddSystem<ME.ECS.Essentials.Input.Input.Systems.SendMessagesSystem>();
            
            this.inputMaskFilter = Filter.Create("InputFeature-MaskFilter").With<InputMask>().Push();

        }

        protected override void OnDeconstruct() {

            this.pointerClickEvent = default;
            this.pointerDoubleClickEvent = default;
            this.pointerDragBeginEvent = default;
            this.pointerDragMoveEvent = default;
            this.pointerDragEndEvent = default;
            this.pointerUpEvent = default;
            this.pointerDownEvent = default;
            
            this.gesturePitchDownEvent = default;
            this.gesturePitchMoveEvent = default;
            this.gesturePitchUpEvent = default;

            this.getWorldPointerCallback = default;

            this.actions.Clear();
            this.camera = null;

        }

        public bool IsAllowedUIEvents() {

            return this.world.ReadSharedData<InputAllowUI>().allow;

        }

        public bool IsAllowed(in Entity player, InputEventType inputEventType, in FLOAT3 worldPosition) {

            if (this.inputMaskFilter.Count == 0) return true;

            var prior = PoolSortedList<int, Entity>.Spawn(1);
            foreach (var entity in this.inputMaskFilter) {

                ref readonly var mask = ref entity.Read<InputMask>();
                if (mask.player == player &&
                    (mask.inputEventType == inputEventType || mask.inputEventType == InputEventType.Any)) {
                    
                    prior.Add(mask.priority, entity);
                    
                }
                
            }

            var isAllowed = true;
            if (prior.Count > 0) {

                var posRect = worldPosition.XZ();
                isAllowed = false;
                foreach (var kv in prior.Values) {

                    var entity = kv;
                    ref readonly var mask = ref entity.Read<InputMask>();
                    if (mask.allow == true) {

                        isAllowed = true;
                        if (mask.checkRect == true) {

                            var inMask = mask.rect.Contains(posRect);
                            if ((mask.insideRect == true && inMask == false) ||
                                (mask.insideRect == false && inMask == true)) {

                                isAllowed = false;

                            }

                        }

                    } else {

                        isAllowed = false;
                        if (mask.checkRect == true) {

                            var inMask = mask.rect.Contains(posRect);
                            if ((mask.insideRect == true && inMask == false) ||
                                (mask.insideRect == false && inMask == true)) {

                                isAllowed = true;

                            }

                        }

                    }

                }

            }

            PoolSortedList<int, Entity>.Recycle(ref prior);
            
            return isAllowed;
            
        }

        public InputPointerData RaiseMarkerCallback(InputPointerData data) {

            if (this.actions.TryGetValue(data.eventType, out var list) == true) {

                data = list.Invoke(data);
                
            }

            return data;

        }

        public void AddMarkerCallback(InputEventType eventType, MarkerModifier onMarkerCreate) {

            if (this.actions.TryGetValue(eventType, out var list) == true) {

                list += onMarkerCreate;
                this.actions[eventType] = list;

            } else {
            
                this.actions.Add(eventType, onMarkerCreate);
            
            }
            
        }

        public void RemoveMarkerCallback(InputEventType eventType, MarkerModifier onMarkerCreate) {

            if (this.actions.TryGetValue(eventType, out var list) == true) {

                list -= onMarkerCreate;
                this.actions[eventType] = list;

            }
            
        }

        public void SetMarkerCallback(InputEventType eventType, MarkerModifier onMarkerCreate) {

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
            
            if (this.gesturePitchDown == true) this.gesturePitchDownEvent.SetPlayerEntityReceiver(getEntity);
            if (this.gesturePitchMove == true) this.gesturePitchMoveEvent.SetPlayerEntityReceiver(getEntity);
            if (this.gesturePitchUp == true) this.gesturePitchUpEvent.SetPlayerEntityReceiver(getEntity);

        }

        public void Execute() {
            
            var networkModule = this.world.GetModule<ME.ECS.Network.INetworkModuleBase>();
            if (networkModule != null && networkModule.IsReplayMode() == true) return;
            
            if (this.pointerClick == true) this.pointerClickEvent.Execute();
            if (this.pointerDoubleClick == true) this.pointerDoubleClickEvent.Execute();
            if (this.pointerDragBegin == true) this.pointerDragBeginEvent.Execute();
            if (this.pointerDragMove == true) this.pointerDragMoveEvent.Execute();
            if (this.pointerDragEnd == true) this.pointerDragEndEvent.Execute();
            if (this.pointerUp == true) this.pointerUpEvent.Execute();
            if (this.pointerDown == true) this.pointerDownEvent.Execute();
            
            if (this.gesturePitchDown == true) this.gesturePitchDownEvent.Execute();
            if (this.gesturePitchMove == true) this.gesturePitchMoveEvent.Execute();
            if (this.gesturePitchUp == true) this.gesturePitchUpEvent.Execute();

        }
        
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputPointerClick marker) { this.pointerClickEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputPointerDoubleClick marker) { this.pointerDoubleClickEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputPointerDragBegin marker) { this.pointerDragBeginEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputPointerDragMove marker) { this.pointerDragMoveEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputPointerDragEnd marker) { this.pointerDragEndEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputPointerUp marker) { this.pointerUpEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputPointerDown marker) { this.pointerDownEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputGesturePitchDown marker) { this.gesturePitchDownEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputGesturePitchMove marker) { this.gesturePitchMoveEvent.RPC(player, marker); }
        private void RPC(Entity player, ME.ECS.Essentials.Input.Input.Markers.InputGesturePitchUp marker) { this.gesturePitchUpEvent.RPC(player, marker); }

        public void SetGetWorldPointerCallback(GetWorldPointerCallback callback) {

            this.getWorldPointerCallback = callback;

        }
        
        public bool GetWorldPointer(int pointerId, out FLOAT3 result) {

            result = default;
            if (this.camera == null) return false;

            if (this.getWorldPointerCallback != null) {

                return this.getWorldPointerCallback.Invoke(pointerId, this.camera, out result);

            }

            var pos = ME.ECS.Essentials.Input.InputUtils.GetPointerPosition(pointerId);
            var ray = this.camera.ScreenPointToRay(pos);
            if (UnityEngine.Physics.Raycast(ray, out var hit, this.raycastDistance, this.raycastMask) == true) {

                result = hit.point;
                return true;

            }

            return false;

        }
        
        public bool GetWorldPointer(out FLOAT3 result) {

            return this.GetWorldPointer(0, out result);
            
        }
        
    }

}