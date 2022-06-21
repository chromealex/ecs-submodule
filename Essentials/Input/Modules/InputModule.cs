#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS.Essentials.Input.Input.Modules {
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class InputModule : IModule, IUpdate {
        
        private InputFeature feature;

        private float3 pressWorldPos;
        private bool isPressed;
        private bool dragBegin;
        private float3 lastDragPos;
        
        private float3 pressWorldPosClick;
        private bool sendClickWaiter;
        private int clicksCount;
        private float prevPressedTime;

        private bool isPitchDown;

        private float3 gesturePitchPointer1LastPos;
        private float3 gesturePitchPointer2LastPos;

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

                var hasWorldPos = this.feature.GetWorldPointer(out var worldPos);

                var dt = UnityEngine.Time.realtimeSinceStartup - this.prevPressedTime;
                if (hasWorldPos == true &&
                    dt <= this.feature.doubleClickThreshold &&
                    (math.distancesq(this.pressWorldPosClick, worldPos) <= this.feature.doubleClickMaxDistance * this.feature.doubleClickMaxDistance)) {

                    var data = this.feature.RaiseMarkerCallback(new InputPointerData(0, this.pressWorldPosClick, InputEventType.PointerDoubleClick));
                    this.world.AddMarker(new Markers.InputPointerDoubleClick() {
                        data = data,
                    });

                    this.sendClickWaiter = false;
                    this.clicksCount = 0;

                }

            }

            if (InputUtils.GetPointersCount() == 2 || this.isPitchDown == true) {

                var forceMove = false;
                if (this.isPitchDown == false &&
                    (InputUtils.IsPointerDown(0) == true ||
                     InputUtils.IsPointerDown(1) == true)) {

                    if (this.feature.GetWorldPointer(0, out var wp1) == true &&
                        this.feature.GetWorldPointer(1, out var wp2) == true) {

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

                    if (this.feature.GetWorldPointer(0, out var wp1) == false) wp1 = this.gesturePitchPointer1LastPos;
                    if (this.feature.GetWorldPointer(1, out var wp2) == false) wp2 = this.gesturePitchPointer2LastPos;

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

                    if (this.feature.GetWorldPointer(0, out var wp1) == true &&
                        this.feature.GetWorldPointer(1, out var wp2) == true) {

                        if (forceMove == true ||
                            (math.distancesq(this.gesturePitchPointer1LastPos, wp1) >= this.feature.gesturePitchMinDragThreshold) ||
                            (math.distancesq(this.gesturePitchPointer2LastPos, wp2) >= this.feature.gesturePitchMinDragThreshold)) {

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

            for (int pointerId = 0; pointerId < 3; ++pointerId) {

                if (InputUtils.IsPointerDown(pointerId) == true) {

                    if (this.feature.IsAllowedUIEvents() == true || InputUtils.IsPointerOverUI(0) == false) {

                        if (this.feature.GetWorldPointer(out var worldPos) == true) {

                            var data = this.feature.RaiseMarkerCallback(new InputPointerData(pointerId, worldPos, InputEventType.PointerDown));
                            this.world.AddMarker(new Markers.InputPointerDown() {
                                data = data,
                            });
                            this.pressWorldPos = worldPos;
                            this.lastDragPos = this.pressWorldPos;
                            this.isPressed = true;

                        }

                    }

                }

                if (InputUtils.IsPointerPressed(pointerId) == true && this.isPressed == true) {

                    if (this.feature.GetWorldPointer(out var worldPos) == true) {

                        if (this.dragBegin == false &&
                            math.distancesq(worldPos, this.pressWorldPos) >= this.feature.dragBeginThreshold * this.feature.dragBeginThreshold) {

                            this.dragBegin = true;
                            var data = this.feature.RaiseMarkerCallback(new InputPointerData(pointerId, this.pressWorldPos, InputEventType.PointerDragBegin));
                            this.world.AddMarker(new Markers.InputPointerDragBegin() {
                                data = data,
                            });

                        }

                    }

                }

                if (this.dragBegin == true) {

                    if (this.feature.GetWorldPointer(out var worldPos) == true) {

                        if (math.distancesq(this.lastDragPos, worldPos) > this.feature.dragMoveThreshold * this.feature.dragMoveThreshold) {

                            var data = this.feature.RaiseMarkerCallback(new InputPointerData(pointerId, worldPos, InputEventType.PointerDragMove) { pressWorldPosition = this.pressWorldPos });
                            this.world.AddMarker(new Markers.InputPointerDragMove() {
                                data = data,
                            });

                            this.lastDragPos = worldPos;

                        }

                    }

                }

                if (InputUtils.IsPointerUp(pointerId) == true) {

                    this.world.AddMarker(new Markers.InputPointerUp() {
                        data = new InputPointerData(pointerId, this.lastDragPos, InputEventType.PointerUp),
                    });

                    var hasWorldPos = this.feature.GetWorldPointer(out var worldPos);

                    if (this.dragBegin == true) {

                        if (hasWorldPos == true) {

                            var data = this.feature.RaiseMarkerCallback(new InputPointerData(pointerId, worldPos, InputEventType.PointerDragEnd) { pressWorldPosition = this.pressWorldPos });
                            this.world.AddMarker(new Markers.InputPointerDragEnd() {
                                data = data,
                            });

                        } else {

                            var data = this.feature.RaiseMarkerCallback(new InputPointerData(pointerId, this.lastDragPos, InputEventType.PointerDragEnd) { pressWorldPosition = this.pressWorldPos });
                            this.world.AddMarker(new Markers.InputPointerDragEnd() {
                                data = data,
                            });
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
                                (math.distancesq(this.pressWorldPosClick, worldPos) <= this.feature.doubleClickMaxDistance * this.feature.doubleClickMaxDistance)) {

                                {
                                    var data = this.feature.RaiseMarkerCallback(new InputPointerData(pointerId, worldPos, InputEventType.PointerClick));
                                    this.world.AddMarker(new Markers.InputPointerClick() {
                                        data = data,
                                    });
                                }

                                {
                                    var data = this.feature.RaiseMarkerCallback(new InputPointerData(pointerId, worldPos, InputEventType.PointerDoubleClick));
                                    this.world.AddMarker(new Markers.InputPointerDoubleClick() {
                                        data = data,
                                    });
                                }

                                this.clicksCount = 0;

                            } else {

                                var data = this.feature.RaiseMarkerCallback(new InputPointerData(pointerId, this.pressWorldPos, InputEventType.PointerClick));
                                this.world.AddMarker(new Markers.InputPointerClick() {
                                    data = data,
                                });
                                this.pressWorldPosClick = this.pressWorldPos;

                                this.clicksCount = 0;
                                this.prevPressedTime = UnityEngine.Time.realtimeSinceStartup;

                            }

                        }

                    }

                    this.isPressed = false;

                }

            }

        }

    }
    
}
