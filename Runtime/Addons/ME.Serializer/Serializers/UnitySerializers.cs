#if UNITY
using UnityEngine;
using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;
    
namespace ME.ECS.Serializer {

    public struct Vector2IntSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Vector2Int; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(Vector2Int); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (Vector2Int)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<Vector2Int>(stream);
            
        }

    }

    public struct Vector3IntSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Vector3Int; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(Vector3Int); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (Vector3Int)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<Vector3Int>(stream);
            
        }

    }

    public struct Vector2Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Vector2; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(Vector2); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (Vector2)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<Vector2>(stream);
            
        }

    }

    public struct Vector3Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Vector3; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(Vector3); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (Vector3)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<Vector3>(stream);
            
        }

    }

    public struct Vector4Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Vector4; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(Vector4); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (Vector4)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<Vector4>(stream);
            
        }

    }

    public struct QuaternionSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Quaternion; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(Quaternion); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (Quaternion)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<Quaternion>(stream);
            
        }

    }

}
#endif