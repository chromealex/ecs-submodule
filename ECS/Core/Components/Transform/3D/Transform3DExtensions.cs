#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using FLOAT3 = UnityEngine.Vector3;
using QUATERNION = UnityEngine.Quaternion;

namespace ME.ECS {

    using Transform;

    public static class ECSTransform3DExtensions {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetLocalPosition(this in Entity child, in FLOAT3 position) {
            
            Worlds.currentWorld.SetData(in child, new Position() { value = position });

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetPosition(this in Entity child, in FLOAT3 position) {

            ref readonly var container = ref child.Read<Container>();
            if (container.entity.IsEmpty() == false) {

                var containerRotation = container.entity.GetRotation();
                var containerPosition = container.entity.GetPosition();
                child.SetLocalPosition(QUATERNION.Inverse(containerRotation) * (position - containerPosition));

            } else {

                child.SetLocalPosition(position);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetLocalScale(this in Entity child, in FLOAT3 scale) {

            Worlds.currentWorld.SetData(in child, scale.ToScaleStruct());

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FLOAT3 GetPosition(this in Entity child) {

            var position = child.Read<Position>().ToVector3();
            ref readonly var container = ref child.Read<Container>();
            while (container.entity.IsEmpty() == false) {

                position = container.entity.Read<Rotation>().ToQuaternion() * position;
                position += container.entity.Read<Position>().ToVector3();
                container = ref container.entity.Read<Container>();

            }

            return position;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FLOAT3 GetLocalPosition(this in Entity child) {

            return child.Read<Position>().ToVector3();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetLocalRotation(this in Entity child, in QUATERNION rotation) {

            Worlds.currentWorld.SetData(in child, rotation.ToRotationStruct());

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static QUATERNION GetLocalRotation(this in Entity child) {

            return child.Read<Rotation>().ToQuaternion();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FLOAT3 GetLocalScale(this in Entity child) {

            return child.Read<Scale>().ToVector3();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Position ToPositionStruct(this in FLOAT3 v) {

            return new Position() { value = v };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FLOAT3 ToVector3(this in Position v) {

            return v.value;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Rotation ToRotationStruct(this in QUATERNION v) {

            return new Rotation() { value = v };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static QUATERNION ToQuaternion(this in Rotation v) {

            return v.value;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Scale ToScaleStruct(this in FLOAT3 v) {

            return new Scale() { value = v };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static FLOAT3 ToVector3(this in Scale v) {

            return v.value;

        }

    }

}