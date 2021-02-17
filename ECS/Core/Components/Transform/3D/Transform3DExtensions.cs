#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Transform;

    public static class ECSTransform3DExtensions {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetLocalPosition(this in Entity child, in UnityEngine.Vector3 position) {

            Worlds.currentWorld.SetData(in child, new Position() { x = position.x, y = position.y, z = position.z });

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetPosition(this in Entity child, in UnityEngine.Vector3 position) {

            var container = child.GetData<Container>(createIfNotExists: false);
            if (container.entity.IsEmpty() == false) {

                var containerRotation = container.entity.GetRotation();
                var containerPosition = container.entity.GetPosition();
                child.SetLocalPosition(UnityEngine.Quaternion.Inverse(containerRotation) * (position - containerPosition));

            } else {

                child.SetLocalPosition(position);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetRotation(this in Entity child, in UnityEngine.Quaternion rotation) {

            var container = Worlds.currentWorld.GetData<Container>(in child, createIfNotExists: false);
            if (container.entity.IsEmpty() == false) {

                var containerRotation = container.entity.GetRotation();
                var containerRotationInverse = UnityEngine.Quaternion.Inverse(containerRotation);
                child.SetLocalRotation(containerRotationInverse * rotation);

            } else {

                child.SetLocalRotation(rotation);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetLocalScale(this in Entity child, in UnityEngine.Vector3 scale) {

            Worlds.currentWorld.SetData(in child, scale.ToScaleStruct());

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector3 GetPosition(this in Entity child) {

            var position = Worlds.currentWorld.GetData<Position>(in child, createIfNotExists: false).ToVector3();
            var current = Worlds.currentWorld.GetData<Container>(in child, createIfNotExists: false).entity;
            while (current.IsEmpty() == false) {

                position = Worlds.currentWorld.GetData<Rotation>(in current, createIfNotExists: false).ToQuaternion() * position;
                position += Worlds.currentWorld.GetData<Position>(in current, createIfNotExists: false).ToVector3();
                current = Worlds.currentWorld.GetData<Container>(in current, createIfNotExists: false).entity;

            }

            return position;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector3 GetLocalPosition(this in Entity child) {

            return Worlds.currentWorld.GetData<Position>(in child, createIfNotExists: false).ToVector3();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetLocalRotation(this in Entity child, in UnityEngine.Quaternion rotation) {

            Worlds.currentWorld.SetData(in child, rotation.ToRotationStruct());

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Quaternion GetLocalRotation(this in Entity child) {

            return Worlds.currentWorld.GetData<Rotation>(in child, createIfNotExists: false).ToQuaternion();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Quaternion GetRotation(this in Entity child) {

            var worldRot = Worlds.currentWorld.GetData<Rotation>(in child, createIfNotExists: false).ToQuaternion(); //child.GetLocalRotation();
            var current = Worlds.currentWorld.GetData<Container>(in child, createIfNotExists: false).entity;
            while (current.IsEmpty() == false) {

                worldRot = Worlds.currentWorld.GetData<Rotation>(in current, createIfNotExists: false).ToQuaternion() * worldRot;
                current = Worlds.currentWorld.GetData<Container>(in current, createIfNotExists: false).entity;

            }

            return worldRot;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector3 GetLocalScale(this in Entity child) {

            return Worlds.currentWorld.GetData<Scale>(in child, createIfNotExists: false).ToVector3();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Position ToPositionStruct(this in UnityEngine.Vector3 v) {

            return new Position() { x = v.x, y = v.y, z = v.z };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector3 ToVector3(this in Position v) {

            return new UnityEngine.Vector3() { x = v.x, y = v.y, z = v.z };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Rotation ToRotationStruct(this in UnityEngine.Quaternion v) {

            return new Rotation() { x = v.x, y = v.y, z = v.z, w = v.w };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Quaternion ToQuaternion(this in Rotation v) {

            return new UnityEngine.Quaternion() { x = (float)v.x, y = (float)v.y, z = (float)v.z, w = (float)v.w };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Scale ToScaleStruct(this in UnityEngine.Vector3 v) {

            return new Scale() { x = v.x, y = v.y, z = v.z };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector3 ToVector3(this in Scale v) {

            return new UnityEngine.Vector3() { x = v.x, y = v.y, z = v.z };

        }

    }

}