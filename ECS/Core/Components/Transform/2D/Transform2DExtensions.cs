#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using FLOAT2 = UnityEngine.Vector2;
using QUATERNION = UnityEngine.Quaternion;

namespace ME.ECS {

    using Transform;

    public static class ECSTransform2DExtensions {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetLocalPosition2D(this in Entity child, in FLOAT2 position) {

            Worlds.currentWorld.SetData(in child, new Position2D() { value = position });

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetPosition2D(this in Entity child, in FLOAT2 position) {

            ref readonly var container = ref child.Read<Container>();
            if (container.entity.IsEmpty() == false) {

                var containerRotation = QUATERNION.Euler(0f, 0f, container.entity.GetRotation2D());
                var containerPosition = container.entity.GetPosition2D();
                child.SetLocalPosition2D(QUATERNION.Inverse(containerRotation) * (position - containerPosition));

            } else {

                child.SetLocalPosition2D(position);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetRotation2D(this in Entity child, float rotation) {

            ref readonly var container = ref child.Read<Container>();
            if (container.entity.IsEmpty() == false) {

                var containerRotation = QUATERNION.Euler(0f, 0f, container.entity.GetRotation2D());
                var containerRotationInverse = QUATERNION.Inverse(containerRotation);
                child.SetLocalRotation2D((containerRotationInverse * QUATERNION.Euler(0f, 0f, rotation)).eulerAngles.z);
                
            } else {

                child.SetLocalRotation2D(rotation);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetLocalScale(this in Entity child, in FLOAT2 scale) {

            Worlds.currentWorld.SetData(in child, scale.ToScaleStruct());

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FLOAT2 GetPosition2D(this in Entity child) {

            var position = child.Read<Position2D>().ToVector2();
            ref readonly var container = ref child.Read<Container>();
            while (container.entity.IsEmpty() == false) {

                var angle = container.entity.Read<Rotation2D>().ToQuaternion2D();
                position = QUATERNION.Euler(0f, angle, 0f) * position;
                position += container.entity.Read<Position2D>().ToVector2();
                container = ref container.entity.Read<Container>();

            }

            return position;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FLOAT2 GetLocalPosition2D(this in Entity child) {

            return child.Read<Position2D>().ToVector2();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetLocalRotation2D(this in Entity child, float rotation) {

            Worlds.currentWorld.SetData(in child, rotation.ToRotationStruct());

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static float GetLocalRotation2D(this in Entity child) {

            return child.Read<Rotation2D>().ToQuaternion2D();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static float GetRotation2D(this in Entity child) {

            var worldRot = child.Read<Rotation2D>().ToQuaternion2D();
            ref readonly var container = ref child.Read<Container>();
            while (container.entity.IsEmpty() == false) {

                worldRot = container.entity.Read<Rotation2D>().ToQuaternion2D() * worldRot;
                container = ref container.entity.Read<Container>();

            }

            return worldRot;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector2 GetLocalScale2D(this in Entity child) {

            return child.Read<Scale2D>().ToVector2();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Position2D ToPositionStruct(this in FLOAT2 v) {

            return new Position2D() { value = v };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FLOAT2 ToVector2(this in Position2D v) {

            return v.value;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Rotation2D ToRotationStruct(this float v) {

            return new Rotation2D() { value = v };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static float ToQuaternion2D(this in Rotation2D v) {

            return v.value;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Scale2D ToScaleStruct(this in FLOAT2 v) {

            return new Scale2D() { value = v };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FLOAT2 ToVector2(this in Scale2D v) {

            return v.value;

        }

    }

}