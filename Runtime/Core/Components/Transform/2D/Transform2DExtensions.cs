#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if FIXED_POINT_MATH
using FLOAT = ME.ECS.fp;
using FLOAT2 = ME.ECS.fp2;
using FLOAT3 = ME.ECS.fp3;
using FLOAT4 = ME.ECS.fp4;
using QUATERNION = ME.ECS.fpquaternion;
using static ME.ECS.fpmath;
#else
using FLOAT = System.Single;
using FLOAT2 = UnityEngine.Vector2;
using FLOAT3 = UnityEngine.Vector3;
using FLOAT4 = UnityEngine.Vector4;
using QUATERNION = UnityEngine.Quaternion;
using static Unity.Mathematics.math;
#endif

using System.Runtime.CompilerServices;

namespace ME.ECS {

    using Transform;

    public static class ECSTransform2DExtensions {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static void SetLocalPosition2D(this in Entity child, in FLOAT2 position) => Worlds.currentWorld.SetData(in child, new Position2D() { value = position });
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static void SetLocalRotation2D(this in Entity child, FLOAT rotation) => Worlds.currentWorld.SetData(in child, rotation.ToRotationStruct());
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static void SetLocalScale2D(this in Entity child, in FLOAT2 scale) => Worlds.currentWorld.SetData(in child, scale.ToScaleStruct());
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static FLOAT2 GetLocalPosition2D(this in Entity child) => child.Read<Position2D>().ToVector2();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static FLOAT GetLocalRotation2D(this in Entity child) => child.Read<Rotation2D>().ToQuaternion2D();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static UnityEngine.Vector2 GetLocalScale2D(this in Entity child) => child.Read<Scale2D>().ToVector2();
        
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static Position2D ToPositionStruct(this in FLOAT2 v) => new Position2D() { value = v };
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static FLOAT2 ToVector2(this in Position2D v) => v.value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static Rotation2D ToRotationStruct(this FLOAT v) => new Rotation2D() { value = v };
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static FLOAT ToQuaternion2D(this in Rotation2D v) => v.value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static Scale2D ToScaleStruct(this in FLOAT2 v) => new Scale2D() { value = v };
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static FLOAT2 ToVector2(this in Scale2D v) => v.value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] private static FLOAT2 Multiply_INTERNAL(FLOAT2 v1, FLOAT2 v2) => new FLOAT2(v1.x * v2.x, v1.y * v2.y);
        
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static FLOAT2 GetInvScale_INTERNAL(in Entity entity) {
            
            if (entity.TryRead(out Scale2D component) == true) {

                var v = component.value;
                if (v.x != 0f) v.x = 1f / v.x;
                if (v.y != 0f) v.y = 1f / v.y;
                return v;

            }

            return FLOAT2.one;
            
        }
        
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static FLOAT2 GetScale_INTERNAL(in Entity entity) {

            if (entity.TryRead(out Scale2D component) == true) {

                return component.value;

            }

            return FLOAT2.one;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static FLOAT2 Rotate(FLOAT degrees, FLOAT2 v) {

            var rad = radians(degrees);
            var s = sin(rad);
            var c = cos(rad);
            var tx = v.x;
            var ty = v.y;
            v.x = (c * tx) - (s * ty);
            v.y = (s * tx) + (c * ty);
            return v;
            
        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetPosition2D(this in Entity child, in FLOAT2 position) {

            ref readonly var container = ref child.Read<Container>();
            if (container.entity.IsEmpty() == false) {

                var angle = container.entity.GetRotation2D();
                var containerPosition = container.entity.GetPosition2D();
                child.SetLocalPosition2D(ECSTransform2DExtensions.Multiply_INTERNAL(ECSTransform2DExtensions.Rotate(angle, ECSTransform2DExtensions.GetInvScale_INTERNAL(in container.entity)), (position - containerPosition)));

            } else {

                child.SetLocalPosition2D(position);

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetRotation2D(this in Entity child, FLOAT rotation) {

            ref readonly var container = ref child.Read<Container>();
            if (container.entity.IsEmpty() == false) {

                var angle = container.entity.GetRotation2D();
                child.SetLocalRotation2D(rotation - angle);
                
            } else {

                child.SetLocalRotation2D(rotation);

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static FLOAT2 GetPosition2D(this in Entity child) {

            var position = child.Read<Position2D>().ToVector2();
            ref readonly var container = ref child.Read<Container>();
            while (container.entity.IsEmpty() == false) {

                var angle = container.entity.Read<Rotation2D>().ToQuaternion2D();
                position = ECSTransform2DExtensions.Multiply_INTERNAL(ECSTransform2DExtensions.Rotate(angle, ECSTransform2DExtensions.GetScale_INTERNAL(in container.entity)), position);
                position += container.entity.Read<Position2D>().ToVector2();
                container = ref container.entity.Read<Container>();

            }

            return position;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static FLOAT GetRotation2D(this in Entity child) {

            var worldRot = child.Read<Rotation2D>().ToQuaternion2D();
            ref readonly var container = ref child.Read<Container>();
            while (container.entity.IsEmpty() == false) {

                worldRot = container.entity.Read<Rotation2D>().ToQuaternion2D() + worldRot;
                container = ref container.entity.Read<Container>();

            }

            return worldRot;

        }

    }

}