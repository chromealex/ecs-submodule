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

    public static class ECSTransform2DExtensions {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static void SetLocalPosition2D(this in Entity child, in float2 position) => Worlds.currentWorld.SetData(in child, new Position2D() { value = position });
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static void SetLocalRotation2D(this in Entity child, tfloat rotation) => Worlds.currentWorld.SetData(in child, new Rotation2D() { value = rotation });
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static void SetLocalScale2D(this in Entity child, in float2 scale) => Worlds.currentWorld.SetData(in child, new Scale2D() { value = scale });
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static float2 GetLocalPosition2D(this in Entity child) => child.Read<Position2D>().value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static tfloat GetLocalRotation2D(this in Entity child) => child.Read<Rotation2D>().value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)] public static float2 GetLocalScale2D(this in Entity child) => child.Read<Scale2D>().value;
        
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static float2 GetInvScale_INTERNAL(in Entity entity) {
            
            if (entity.TryRead(out Scale2D component) == true) {

                var v = component.value;
                if (v.x != 0f) v.x = 1f / v.x;
                if (v.y != 0f) v.y = 1f / v.y;
                return v;

            }

            return new float2(1f, 1f);
            
        }
        
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static float2 GetScale_INTERNAL(in Entity entity) {

            if (entity.TryRead(out Scale2D component) == true) {

                return component.value;

            }

            return new float2(1f, 1f);

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static float2 Rotate(tfloat degrees, float2 v) {

            var rad = math.radians(degrees);
            var s = math.sin(rad);
            var c = math.cos(rad);
            var tx = v.x;
            var ty = v.y;
            v.x = (c * tx) - (s * ty);
            v.y = (s * tx) + (c * ty);
            return v;
            
        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetPosition2D(this in Entity child, in float2 position) {

            ref readonly var container = ref child.Read<Container>();
            if (container.entity.IsEmpty() == false) {

                var angle = container.entity.GetRotation2D();
                var containerPosition = container.entity.GetPosition2D();
                child.SetLocalPosition2D(ECSTransform2DExtensions.Rotate(angle, ECSTransform2DExtensions.GetInvScale_INTERNAL(in container.entity) * (position - containerPosition)));

            } else {

                child.SetLocalPosition2D(position);

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetRotation2D(this in Entity child, tfloat rotation) {

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
        public static float2 GetPosition2D(this in Entity child) {

            var position = child.Read<Position2D>().value;
            ref readonly var container = ref child.Read<Container>();
            while (container.entity.IsEmpty() == false) {

                var angle = container.entity.Read<Rotation2D>().value;
                position = ECSTransform2DExtensions.Rotate(angle, ECSTransform2DExtensions.GetScale_INTERNAL(in container.entity) * position);
                position += container.entity.Read<Position2D>().value;
                container = ref container.entity.Read<Container>();

            }

            return position;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static tfloat GetRotation2D(this in Entity child) {

            var worldRot = child.Read<Rotation2D>().value;
            ref readonly var container = ref child.Read<Container>();
            while (container.entity.IsEmpty() == false) {

                worldRot = container.entity.Read<Rotation2D>().value + worldRot;
                container = ref container.entity.Read<Container>();

            }

            return worldRot;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public static float2 GetScale2D(this in Entity child) {

            var scale = child.Read<Scale2D>().value;
            ref readonly var container = ref child.Read<Container>();
            while (container.entity.IsEmpty() == false) {

                scale *= container.entity.Read<Scale2D>().value;
                container = ref container.entity.Read<Container>();

            }

            return scale;

        }

    }

}