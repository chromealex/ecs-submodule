#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS {

    using UnityEngine;
    
    public static class CameraExtensions {

        public static Camera.Camera CreateCameraComponent(this UnityEngine.Camera camera) {

            var component = new ME.ECS.Camera.Camera {
                perspective = (camera.orthographic == false),
                fieldOfView = camera.fieldOfView,
                aspect = camera.aspect,
                orthoSize = camera.orthographicSize,
                farClipPlane = camera.farClipPlane,
                nearClipPlane = camera.nearClipPlane
            };
            return component;

        }
        
        /// <summary>
        /// UnityEngine.Camera::ViewportToWorldPoint
        /// </summary>
        /// <param name="entity">Entity with ME.ECS.Camera.Camera component</param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static float3 ViewportToWorldPoint(this Entity entity, float3 position) {

            if (entity.Has<ME.ECS.Camera.Camera>() == false) return float3.zero;
            
            var camera = entity.Read<ME.ECS.Camera.Camera>();
            float4x4 projectionMatrix;
            if (camera.perspective == true) {

                projectionMatrix = float4x4.PerspectiveFov(camera.fieldOfView, camera.aspect, camera.nearClipPlane, camera.farClipPlane);
                
            } else {

                projectionMatrix = float4x4.OrthoOffCenter(-camera.orthoSize, camera.orthoSize, -camera.orthoSize, camera.orthoSize, camera.nearClipPlane, camera.farClipPlane);
                
            }
            
            var worldToCameraMatrix = float4x4.TRS(entity.GetPosition(), entity.GetRotation(), new float3(1f, 1f, 1f));
            
            var screenSize = new float2(Screen.width, Screen.height);
            position.x *= screenSize.x;
            position.y *= screenSize.y;
            
            float4x4 world2Screen = projectionMatrix * worldToCameraMatrix;
            float4x4 screen2World = math.inverse(world2Screen);
            float3 screenSpace = math.mul(world2Screen, new float4(position, 1f)).xyz;
            float3 worldSpace = math.mul(screen2World, new float4(screenSpace, 1f)).xyz;

            return worldSpace;

        }

        /// <summary>
        /// UnityEngine.Camera::ViewportToScreenPoint
        /// </summary>
        /// <param name="entity">Entity with ME.ECS.Camera.Camera component</param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static float3 ViewportToScreenPoint(this Entity entity, float3 position) {

            if (entity.Has<ME.ECS.Camera.Camera>() == false) return float3.zero;
            
            var camera = entity.Read<ME.ECS.Camera.Camera>();
            float4x4 projectionMatrix;
            if (camera.perspective == true) {
                
                projectionMatrix = float4x4.PerspectiveFov(camera.fieldOfView, camera.aspect, camera.nearClipPlane, camera.farClipPlane);
                
            } else {

                projectionMatrix = float4x4.OrthoOffCenter(-camera.orthoSize, camera.orthoSize, -camera.orthoSize, camera.orthoSize, camera.nearClipPlane, camera.farClipPlane);
                
            }
            
            var worldToCameraMatrix = float4x4.TRS(entity.GetPosition(), entity.GetRotation(), new float3(1f, 1f, 1f));
            
            var screenSize = new float2(Screen.width, Screen.height);
            position.x *= screenSize.x;
            position.y *= screenSize.y;
            
            float4x4 world2Screen = projectionMatrix * worldToCameraMatrix;
            //float4x4 screen2World = world2Screen.inverse;
            float3 screenSpace = math.mul(world2Screen, new float4(position, 1f)).xyz;
            //Vector3 worldSpace = screen2World.MultiplyPoint(screenSpace);

            return screenSpace;

        }

        /// <summary>
        /// UnityEngine.Camera::ScreenToWorldPoint
        /// </summary>
        /// <param name="entity">Entity with ME.ECS.Camera.Camera component</param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static float3 ScreenToWorldPoint(this Entity entity, float3 position) {

            if (entity.Has<ME.ECS.Camera.Camera>() == false) return float3.zero;
            
            var camera = entity.Read<ME.ECS.Camera.Camera>();
            float4x4 projectionMatrix;
            if (camera.perspective == true) {
                
                projectionMatrix = float4x4.PerspectiveFov(camera.fieldOfView, camera.aspect, camera.nearClipPlane, camera.farClipPlane);
                
            } else {

                projectionMatrix = float4x4.OrthoOffCenter(-camera.orthoSize, camera.orthoSize, -camera.orthoSize, camera.orthoSize, camera.nearClipPlane, camera.farClipPlane);
                
            }
            
            var worldToCameraMatrix = float4x4.TRS(entity.GetPosition(), entity.GetRotation(), new float3(1f, 1f, 1f));
            
            float4x4 world2Screen = projectionMatrix * worldToCameraMatrix;
            float4x4 screen2World = math.inverse(world2Screen);
            float3 screenSpace = math.mul(world2Screen, new float4(position, 1f)).xyz;
            float3 worldSpace = math.mul(screen2World, new float4(screenSpace, 1f)).xyz;

            return worldSpace;

        }

        /// <summary>
        /// UnityEngine.Camera::WorldToViewportPoint
        /// </summary>
        /// <param name="entity">Entity with ME.ECS.Camera.Camera component</param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static float3 WorldToViewportPoint(this Entity entity, float3 position) {

            if (entity.Has<ME.ECS.Camera.Camera>() == false) return float3.zero;
            
            var camera = entity.Read<ME.ECS.Camera.Camera>();
            float4x4 projectionMatrix;
            if (camera.perspective == true) {
                
                projectionMatrix = float4x4.PerspectiveFov(camera.fieldOfView, camera.aspect, camera.nearClipPlane, camera.farClipPlane);
                
            } else {

                projectionMatrix = float4x4.OrthoOffCenter(-camera.orthoSize, camera.orthoSize, -camera.orthoSize, camera.orthoSize, camera.nearClipPlane, camera.farClipPlane);
                
            }
            
            var worldToCameraMatrix = float4x4.TRS(entity.GetPosition(), entity.GetRotation(), new float3(1f, 1f, 1f));
            
            var screenSize = new float2(Screen.width, Screen.height);
            float4x4 world2Screen = projectionMatrix * worldToCameraMatrix;
            //float4x4 screen2World = world2Screen.inverse;
            float3 screenSpace = math.mul(world2Screen, new float4(position, 1f)).xyz;
            //Vector3 worldSpace = screen2World.MultiplyPoint(screenSpace);

            return new float3(screenSpace.x / screenSize.x, screenSpace.y / screenSize.y, screenSpace.z);

        }

        /// <summary>
        /// UnityEngine.Camera::WorldToScreenPoint
        /// </summary>
        /// <param name="entity">Entity with ME.ECS.Camera.Camera component</param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static float3 WorldToScreenPoint(this Entity entity, float3 position) {

            if (entity.Has<ME.ECS.Camera.Camera>() == false) return float3.zero;

            var camera = entity.Read<ME.ECS.Camera.Camera>();
            float4x4 projectionMatrix;
            if (camera.perspective == true) {
                
                projectionMatrix = float4x4.PerspectiveFov(camera.fieldOfView, camera.aspect, camera.nearClipPlane, camera.farClipPlane);
                
            } else {

                projectionMatrix = float4x4.OrthoOffCenter(-camera.orthoSize, camera.orthoSize, -camera.orthoSize, camera.orthoSize, camera.nearClipPlane, camera.farClipPlane);
                
            }
            
            var worldToCameraMatrix = float4x4.TRS(entity.GetPosition(), entity.GetRotation(), new float3(1f, 1f, 1f));
            
            float4x4 world2Screen = projectionMatrix * worldToCameraMatrix;
            //float4x4 screen2World = world2Screen.inverse;
            float3 screenSpace = math.mul(world2Screen, new float4(position, 1f)).xyz;
            //Vector3 worldSpace = screen2World.MultiplyPoint(screenSpace);

            return screenSpace;

        }

    }
}
