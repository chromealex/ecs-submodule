#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if FIXED_POINT_MATH
using FLOAT = ME.ECS.fp;
using FLOAT2 = ME.ECS.fp2;
using FLOAT3 = ME.ECS.fp3;
using FLOAT4 = ME.ECS.fp4;
using QUATERNION = ME.ECS.fpquaternion;
#else
using FLOAT = System.Single;
using FLOAT2 = UnityEngine.Vector2;
using FLOAT3 = UnityEngine.Vector3;
using FLOAT4 = UnityEngine.Vector4;
using QUATERNION = UnityEngine.Quaternion;
#endif

using System.Runtime.CompilerServices;

namespace ME.ECS {

    using Transform;

    public static class ECSTransform3DExtensions {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static void SetLocalPosition(this in Entity child, in FLOAT3 position) => Worlds.currentWorld.SetData(in child, new Position() { value = position });
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static void SetLocalRotation(this in Entity child, in QUATERNION rotation) => Worlds.currentWorld.SetData(in child, rotation.ToRotationStruct());
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static void SetLocalScale(this in Entity child, in FLOAT3 scale) => Worlds.currentWorld.SetData(in child, scale.ToScaleStruct());
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static FLOAT3 GetLocalPosition(this in Entity child) => child.Read<Position>().ToVector3();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static QUATERNION GetLocalRotation(this in Entity child) => child.Read<Rotation>().ToQuaternion();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static FLOAT3 GetLocalScale(this in Entity child) => child.Read<Scale>().ToVector3();
        
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static Position ToPositionStruct(this in FLOAT3 v) => new Position() { value = v };
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static FLOAT3 ToVector3(this in Position v) => v.value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static Rotation ToRotationStruct(this in QUATERNION v) => new Rotation() { value = v };
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static QUATERNION ToQuaternion(this in Rotation v) => v.value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static Scale ToScaleStruct(this in FLOAT3 v) => new Scale() { value = v };
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static FLOAT3 ToVector3(this in Scale v) => v.value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] private static FLOAT3 Multiply_INTERNAL(FLOAT3 v1, FLOAT3 v2) => new FLOAT3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static FLOAT3 GetInvScale_INTERNAL(in Entity entity) {
            
            if (entity.TryRead(out Scale component) == true) {

                var v = component.value;
                if (v.x != 0f) v.x = 1f / v.x;
                if (v.y != 0f) v.y = 1f / v.y;
                if (v.z != 0f) v.z = 1f / v.z;
                return v;

            }

            return FLOAT3.one;
            
        }
        
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static FLOAT3 GetScale_INTERNAL(in Entity entity) {

            if (entity.TryRead(out Scale component) == true) {

                return component.value;

            }

            return FLOAT3.one;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetPosition(this in Entity child, in FLOAT3 position) {

            ref readonly var container = ref child.Read<Container>();
            if (container.entity.IsEmpty() == false) {

                var containerRotation = container.entity.GetRotation();
                var containerPosition = container.entity.GetPosition();
                child.SetLocalPosition(ECSTransform3DExtensions.Multiply_INTERNAL(QUATERNION.Inverse(containerRotation) * ECSTransform3DExtensions.GetInvScale_INTERNAL(in container.entity), (position - containerPosition)));

            } else {

                child.SetLocalPosition(position);

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetRotation(this in Entity child, in QUATERNION rotation) {

            ref readonly var container = ref child.Read<Container>();
            if (container.entity.IsEmpty() == false) {

                var containerRotation = container.entity.GetRotation();
                var containerRotationInverse = QUATERNION.Inverse(containerRotation);
                child.SetLocalRotation(containerRotationInverse * rotation);

            } else {

                child.SetLocalRotation(rotation);

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static FLOAT3 GetPosition(this in Entity child) {

            var position = child.Read<Position>().ToVector3();
            ref readonly var container = ref child.Read<Container>();
            while (container.entity.IsEmpty() == false) {

                position = ECSTransform3DExtensions.Multiply_INTERNAL(container.entity.Read<Rotation>().ToQuaternion() * ECSTransform3DExtensions.GetScale_INTERNAL(in container.entity), position);
                position += container.entity.Read<Position>().ToVector3();
                container = ref container.entity.Read<Container>();

            }

            return position;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static QUATERNION GetRotation(this in Entity child) {
            
            var worldRot = child.Read<Rotation>().ToQuaternion();
            ref readonly var container = ref child.Read<Container>();
            while (container.entity.IsEmpty() == false) {

                worldRot = container.entity.Read<Rotation>().ToQuaternion() * worldRot;
                container = ref container.entity.Read<Container>();

            }

            return worldRot;

        }

    }

}
