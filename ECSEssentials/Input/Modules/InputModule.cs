
namespace ME.ECS.Essentials.Input.Input.Modules {
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class InputModule : IModule, IUpdate {
        
        private InputFeature feature;

        private UnityEngine.Vector3 pressWorldPos;
        private bool isPressed;
        private bool dragBegin;
        private UnityEngine.Vector3 lastDragPos;
        
        private UnityEngine.Vector3 pressWorldPosClick;
        private bool sendClickWaiter;
        private int clicksCount;
        private float prevPressedTime;

        private bool isPitchDown;

        private UnityEngine.Vector3 gesturePitchPointer1LastPos;
        private UnityEngine.Vector3 gesturePitchPointer2LastPos;

        public World world { get; set; }
        
        void IModuleBase.OnConstruct() {
            
            this.feature = this.world.GetFeature<InputFeature>();
            
        }

        void IModuleBase.OnDeconstruct() {

            this.isPressed = false;
            this.dragBegin = false;

        }

        void IUpdate.Update(in float deltaTime) {

            if (this.sendClickWaiter == true) {
                
                var hasWorldPos = this.GetWorldPointer(out var worldPos);

                var dt = UnityEngine.Time.realtimeSinceStartup - this.prevPressedTime;
                if (hasWorldPos == true &&
                    dt <= this.feature.doubleClickThreshold &&
                    ((this.pressWorldPosClick - worldPos).sqrMagnitude <= this.feature.doubleClickMaxDistance * this.feature.doubleClickMaxDistance)) {

                    var data = new InputPointerData(0, this.pressWorldPosClick, InputEventType.PointerDoubleClick);
                    this.world.AddMarker(new Markers.InputPointerDoubleClick() {
                        data = data,
                    });
                    this.feature.RaiseMarkerCallback(data);

                    this.sendClickWaiter = false;
                    this.clicksCount = 0;

                }
                
            }
            
            if (InputUtils.GetPointersCount() == 2 || this.isPitchDown == true) {

                var forceMove = false;
                if (this.isPitchDown == false &&
                    (InputUtils.IsPointerDown(0) == true ||
                    InputUtils.IsPointerDown(1) == true)) {

                    if (this.GetWorldPointer(0, out var wp1) == true &&
                        this.GetWorldPointer(1, out var wp2) == true) {

                        this.world.AddMarker(new Markers.InputGesturePitchDown() {
                            pointer1 = new InputPointerData(0, wp1, InputEventType.PointerDown),
                            pointer2 = new InputPointerData(1, wp2, InputEventType.PointerDown),
                        });
                        this.isPitchDown = true;

                        forceMove = true;

                    }

                }
                
                if (this.isPitchDown == true &&
                    (InputUtils.IsPointerUp(0) == true ||
                    InputUtils.IsPointerUp(1) == true)) {

                    if (this.GetWorldPointer(0, out var wp1) == false) wp1 = this.gesturePitchPointer1LastPos;
                    if (this.GetWorldPointer(1, out var wp2) == false) wp2 = this.gesturePitchPointer2LastPos;

                    this.world.AddMarker(new Markers.InputGesturePitchUp() {
                        pointer1 = new InputPointerData(0, wp1, InputEventType.PointerUp),
                        pointer2 = new InputPointerData(1, wp2, InputEventType.PointerUp),
                    });
                    this.isPitchDown = false;
                    return;

                }
                
                if (this.isPitchDown == true &&
                    (InputUtils.IsPointerPressed(0) == true ||
                    InputUtils.IsPointerPressed(1) == true)) {

                    if (this.GetWorldPointer(0, out var wp1) == true &&
                        this.GetWorldPointer(1, out var wp2) == true) {

                        if (forceMove == true ||
                            (this.gesturePitchPointer1LastPos - wp1).sqrMagnitude >= this.feature.gesturePitchMinDragThreshold ||
                            (this.gesturePitchPointer2LastPos - wp2).sqrMagnitude >= this.feature.gesturePitchMinDragThreshold) {

                            this.world.AddMarker(new Markers.InputGesturePitchMove() {
                                pointer1 = new InputPointerData(0, wp1, InputEventType.PointerDragMove),
                                pointer2 = new InputPointerData(1, wp2, InputEventType.PointerDragMove),
                            });
                            
                            this.gesturePitchPointer1LastPos = wp1;
                            this.gesturePitchPointer2LastPos = wp2;

                        }

                    }

                }

                if (this.isPitchDown == true) return;

            }
            
            if (InputUtils.IsPointerDown(0) == true) {

                if (this.feature.IsAllowedUIEvents() == true || InputUtils.IsPointerOverUI(0) == false) {

                    if (this.GetWorldPointer(out var worldPos) == true) {

                        var data = new InputPointerData(0, worldPos, InputEventType.PointerDown);
                        this.world.AddMarker(new Markers.InputPointerDown() {
                            data = data,
                        });
                        this.feature.RaiseMarkerCallback(data);
                        this.pressWorldPos = worldPos;
                        this.lastDragPos = this.pressWorldPos;
                        this.isPressed = true;
                        
                    }
                    
                }
                
            }

            if (InputUtils.IsPointerPressed(0) == true && this.isPressed == true) {

                if (this.GetWorldPointer(out var worldPos) == true) {

                    if (this.dragBegin == false && (worldPos - this.pressWorldPos).sqrMagnitude >= this.feature.dragBeginThreshold * this.feature.dragBeginThreshold) {

                        this.dragBegin = true;
                        var data = new InputPointerData(0, this.pressWorldPos, InputEventType.PointerDragBegin);
                        this.world.AddMarker(new Markers.InputPointerDragBegin() {
                            data = data,
                        });
                        this.feature.RaiseMarkerCallback(data);

                    }

                }

            }
            
            if (this.dragBegin == true) {

                if (this.GetWorldPointer(out var worldPos) == true) {

                    if ((this.lastDragPos - worldPos).sqrMagnitude > this.feature.dragMoveThreshold * this.feature.dragMoveThreshold) {

                        var data = new InputPointerData(0, worldPos, InputEventType.PointerDragMove) { pressWorldPosition = this.pressWorldPos };
                        this.world.AddMarker(new Markers.InputPointerDragMove() {
                            data = data,
                        });
                        this.feature.RaiseMarkerCallback(data);

                        this.lastDragPos = worldPos;

                    }

                }

            }

            if (InputUtils.IsPointerUp(0) == true) {

                this.world.AddMarker(new Markers.InputPointerUp() {
                    data = new InputPointerData(0, this.lastDragPos, InputEventType.PointerUp),
                });
                
                var hasWorldPos = this.GetWorldPointer(out var worldPos);
                
                if (this.dragBegin == true) {

                    if (hasWorldPos == true) {

                        var data = new InputPointerData(0, worldPos, InputEventType.PointerDragEnd) { pressWorldPosition = this.pressWorldPos };
                        this.world.AddMarker(new Markers.InputPointerDragEnd() {
                            data = data,
                        });
                        this.feature.RaiseMarkerCallback(data);

                    } else {
                        
                        var data = new InputPointerData(0, this.lastDragPos, InputEventType.PointerDragEnd) { pressWorldPosition = this.pressWorldPos };
                        this.world.AddMarker(new Markers.InputPointerDragEnd() {
                            data = data,
                        });
                        this.feature.RaiseMarkerCallback(data);
                        worldPos = this.lastDragPos;

                    }

                    ++this.clicksCount;
                    //this.sendClickWaiter = true;
                    this.prevPressedTime = UnityEngine.Time.realtimeSinceStartup;
                    this.pressWorldPosClick = worldPos;
                    
                    this.dragBegin = false;

                } else if (this.isPressed == true) {

                    ++this.clicksCount;

                    if (hasWorldPos == true) {

                        var dt = UnityEngine.Time.realtimeSinceStartup - this.prevPressedTime;
                        if (dt <= this.feature.doubleClickThreshold &&
                            ((this.pressWorldPosClick - worldPos).sqrMagnitude <= this.feature.doubleClickMaxDistance * this.feature.doubleClickMaxDistance)) {

                            {
                                var data = new InputPointerData(0, worldPos, InputEventType.PointerClick);
                                this.world.AddMarker(new Markers.InputPointerClick() {
                                    data = data,
                                });
                                this.feature.RaiseMarkerCallback(data);
                            }

                            {
                                var data = new InputPointerData(0, worldPos, InputEventType.PointerDoubleClick);
                                this.world.AddMarker(new Markers.InputPointerDoubleClick() {
                                    data = data,
                                });
                                this.feature.RaiseMarkerCallback(data);
                            }
                            
                            this.clicksCount = 0;

                        } else {
                            
                            var data = new InputPointerData(0, this.pressWorldPos, InputEventType.PointerClick);
                            this.world.AddMarker(new Markers.InputPointerClick() {
                                data = data,
                            });
                            this.feature.RaiseMarkerCallback(data);
                            this.pressWorldPosClick = this.pressWorldPos;

                            this.clicksCount = 0;
                            this.prevPressedTime = UnityEngine.Time.realtimeSinceStartup;

                        }

                    }

                }
                
                this.isPressed = false;
                
            }

        }

        private bool GetWorldPointer(int pointerId, out UnityEngine.Vector3 result) {

            result = default;
            if (this.feature.camera == null) return false;

            var pos = InputUtils.GetPointerPosition(pointerId);
            var ray = this.feature.camera.ScreenPointToRay(pos);
            if (UnityEngine.Physics.Raycast(ray, out var hit, this.feature.raycastDistance, this.feature.raycastMask) == true) {

                result = hit.point;
                return true;

            }

            return false;

        }
        
        private bool GetWorldPointer(out UnityEngine.Vector3 result) {

            result = default;
            if (this.feature.camera == null) return false;

            var pos = InputUtils.GetPointerPosition(0);
            var ray = this.feature.camera.ScreenPointToRay(pos);
            if (UnityEngine.Physics.Raycast(ray, out var hit, this.feature.raycastDistance, this.feature.raycastMask) == true) {

                result = hit.point;
                return true;

            }

            return false;

        }
        
    }
    
}
