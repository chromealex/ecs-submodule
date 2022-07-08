#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

using System.Runtime.CompilerServices;

namespace ME.ECS {

    using Transform;

    public static class ECSTransform3DExtensions {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static void SetLocalPosition(this in Entity child, in float3 position) => Worlds.currentWorld.SetData(in child, new Position() { value = position });
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static void SetLocalRotation(this in Entity child, in quaternion rotation) => Worlds.currentWorld.SetData(in child, new Rotation() { value = rotation });
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static void SetLocalScale(this in Entity child, in float3 scale) => Worlds.currentWorld.SetData(in child, new Scale() { value = scale });
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static float3 GetLocalPosition(this in Entity child) => child.Read<Position>().value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static quaternion GetLocalRotation(this in Entity child) => child.Read<Rotation>().value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static float3 GetLocalScale(this in Entity child) => child.Read<Scale>().value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static float3 GetInvScale_INTERNAL(in Entity entity) {
            
            if (entity.TryRead(out Scale component) == true) {

                var v = component.value;
                if (v.x != 0f) v.x = 1f / v.x;
                if (v.y != 0f) v.y = 1f / v.y;
                if (v.z != 0f) v.z = 1f / v.z;
                return v;

            }

            return new float3(1f, 1f, 1f);
            
        }
        
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static float3 GetScale_INTERNAL(in Entity entity) {

            if (entity.TryRead(out Scale component) == true) {

                return component.value;

            }

            return new float3(1f, 1f, 1f);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetPosition(this in Entity child, in float3 position) {

            ref readonly var container = ref child.Read<Container>();
            if (container.entity.IsEmpty() == false) {

                var containerRotation = container.entity.GetRotation();
                var containerPosition = container.entity.GetPosition();
                child.SetLocalPosition(math.mul(math.inverse(containerRotation), ECSTransform3DExtensions.GetInvScale_INTERNAL(in container.entity) * (position - containerPosition)));

            } else {

                child.SetLocalPosition(position);

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetRotation(this in Entity child, in quaternion rotation) {

            ref readonly var container = ref child.Read<Container>();
            if (container.entity.IsEmpty() == false) {

                var containerRotation = container.entity.GetRotation();
                var containerRotationInverse = math.inverse(containerRotation);
                child.SetLocalRotation(math.mul(containerRotationInverse, rotation));

            } else {

                child.SetLocalRotation(rotation);

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static float3 GetPosition(this in Entity child) {

            var position = child.Read<Position>().value;
            ref readonly var container = ref child.Read<Container>();
            while (container.entity.IsEmpty() == false) {

                quaternion worldRot;
                if (container.entity.TryRead<Rotation>(out var worldComponent) == true) {
                    worldRot = worldComponent.value;
                } else {
                    worldRot = quaternion.identity;
                }

                position = math.mul(worldRot, ECSTransform3DExtensions.GetScale_INTERNAL(in container.entity) * position);
                position += container.entity.Read<Position>().value;
                container = ref container.entity.Read<Container>();

            }

            return position;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static quaternion GetRotation(this in Entity child) {

            quaternion worldRot;
            if (child.TryRead<Rotation>(out var worldComponent) == true) {
                worldRot = worldComponent.value;
            } else {
                worldRot = quaternion.identity;
            }
            ref readonly var container = ref child.Read<Container>();
            while (container.entity.IsEmpty() == false) {

                worldRot = math.mul(container.entity.Read<Rotation>().value, worldRot);
                container = ref container.entity.Read<Container>();

            }

            return worldRot;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static float3 GetScale(this in Entity child) {

            var scale = child.Read<Scale>().value;
            ref readonly var container = ref child.Read<Container>();
            while (container.entity.IsEmpty() == false) {

                scale *= container.entity.Read<Scale>().value;
                container = ref container.entity.Read<Container>();

            }

            return scale;

        }

    }

}
