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

            var v = (Vector2)obj;
            Serializer.PackSingle(stream, v.x);
            Serializer.PackSingle(stream, v.y);
            
        }
        
        public object Unpack(Packer stream) {

            Vector2 res = default;
            res.x = Serializer.UnpackSingle(stream);
            res.y = Serializer.UnpackSingle(stream);
            return res;
            
        }

    }

    public struct Vector3Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Vector3; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(Vector3); }
        
        public void Pack(Packer stream, object obj) {

            var v = (Vector3)obj;
            Serializer.PackSingle(stream, v.x);
            Serializer.PackSingle(stream, v.y);
            Serializer.PackSingle(stream, v.z);
            
        }
        
        public object Unpack(Packer stream) {

            Vector3 res = default;
            res.x = Serializer.UnpackSingle(stream);
            res.y = Serializer.UnpackSingle(stream);
            res.z = Serializer.UnpackSingle(stream);
            return res;
            
        }

    }

    public struct Vector4Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Vector4; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(Vector4); }
        
        public void Pack(Packer stream, object obj) {

            var v = (Vector4)obj;
            Serializer.PackSingle(stream, v.x);
            Serializer.PackSingle(stream, v.y);
            Serializer.PackSingle(stream, v.z);
            Serializer.PackSingle(stream, v.w);
            
        }
        
        public object Unpack(Packer stream) {

            Vector4 res = default;
            res.x = Serializer.UnpackSingle(stream);
            res.y = Serializer.UnpackSingle(stream);
            res.z = Serializer.UnpackSingle(stream);
            res.w = Serializer.UnpackSingle(stream);
            return res;
            
        }

    }

    public struct QuaternionSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Quaternion; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(Quaternion); }
        
        public void Pack(Packer stream, object obj) {

            var v = (Quaternion)obj;
            Serializer.PackSingle(stream, v.x);
            Serializer.PackSingle(stream, v.y);
            Serializer.PackSingle(stream, v.z);
            Serializer.PackSingle(stream, v.w);
            
        }
        
        public object Unpack(Packer stream) {

            Quaternion res = default;
            res.x = Serializer.UnpackSingle(stream);
            res.y = Serializer.UnpackSingle(stream);
            res.z = Serializer.UnpackSingle(stream);
            res.w = Serializer.UnpackSingle(stream);
            return res;
            
        }

    }
    
    public struct BoundsSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Bounds; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(Bounds); }
        
        public void Pack(Packer stream, object obj) {

            var v = (Bounds)obj;
            var center = v.center;
            Serializer.PackSingle(stream, center.x);
            Serializer.PackSingle(stream, center.y);
            Serializer.PackSingle(stream, center.z);
            var extents = v.extents;
            Serializer.PackSingle(stream, extents.x);
            Serializer.PackSingle(stream, extents.y);
            Serializer.PackSingle(stream, extents.z);
            
        }
        
        public object Unpack(Packer stream) {

            Bounds res = default;
            Vector3 center = default;
            center.x = Serializer.UnpackSingle(stream);
            center.y = Serializer.UnpackSingle(stream);
            center.z = Serializer.UnpackSingle(stream);
            Vector3 extents = default;
            extents.x = Serializer.UnpackSingle(stream);
            extents.y = Serializer.UnpackSingle(stream);
            extents.z = Serializer.UnpackSingle(stream);

            res.center = center;
            res.extents = extents;

            return res;

        }

    }

}
#endif