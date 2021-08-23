namespace ME.ECS.Essentials.Input {

    using UnityEngine;
    using inp = UnityEngine.Input;
    
    public static class InputUtils {
        
        public static int GetPointersCount() {

            if (inp.touchSupported == false) {

                return 1;
                
            }

            return inp.touchCount;

        }
            
        public static Vector3 GetPointerPosition(int index) {

            if (inp.touchCount > 0 && inp.touchCount >= index + 1) {

                var touch = inp.GetTouch(index);
                return new Vector3(touch.position.x, touch.position.y, 0f);

            }

            return inp.mousePosition;

        }

        public static bool IsPointerUp(int index) {

            if (inp.touchCount > 0 && inp.touchCount >= index + 1) {

                var touch = inp.GetTouch(index);
                return touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;

            }

            return inp.GetMouseButtonUp(index);

        }
            
        public static bool IsPointerDown(int index) {

            if (inp.touchCount > 0 && inp.touchCount >= index + 1) {

                var touch = inp.GetTouch(index);
                return touch.phase == TouchPhase.Began;

            }

            return inp.GetMouseButtonDown(index);

        }

        public static bool IsPointerPressed(int index) {

            if (inp.touchCount > 0 && inp.touchCount >= index + 1) {

                var touch = inp.GetTouch(index);
                return touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary;

            }

            return inp.GetMouseButton(index);

        }

        public static bool IsPointerOverUI(int index) {

            if (inp.touchCount > 0 && inp.touchCount >= index + 1) {

                var touch = inp.GetTouch(index);
                if (touch.phase == TouchPhase.Began) {

                    return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId);

                }

                return false;

            }

            return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        }

    }

}