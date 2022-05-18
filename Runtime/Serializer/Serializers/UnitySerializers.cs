using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Serializer {

    public struct Vector2IntSerializer : ITypeSerializer {

        public byte GetTypeValue() { return (byte)TypeValue.Vector2Int; }
        public System.Type GetTypeSerialized() { return typeof(Vector2Int); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (Vector2Int)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<Vector2Int>(stream);
            
        }

    }

    public struct Vector3IntSerializer : ITypeSerializer {

        public byte GetTypeValue() { return (byte)TypeValue.Vector3Int; }
        public System.Type GetTypeSerialized() { return typeof(Vector3Int); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (Vector3Int)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<Vector3Int>(stream);
            
        }

    }

    public struct Vector2Serializer : ITypeSerializer {

        public byte GetTypeValue() { return (byte)TypeValue.Vector2; }
        public System.Type GetTypeSerialized() { return typeof(Vector2); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (Vector2)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<Vector2>(stream);
            
        }

    }

    public struct Vector3Serializer : ITypeSerializer {

        public byte GetTypeValue() { return (byte)TypeValue.Vector3; }
        public System.Type GetTypeSerialized() { return typeof(Vector3); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (Vector3)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<Vector3>(stream);
            
        }

    }

    public struct Vector4Serializer : ITypeSerializer {

        public byte GetTypeValue() { return (byte)TypeValue.Vector4; }
        public System.Type GetTypeSerialized() { return typeof(Vector4); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (Vector4)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<Vector4>(stream);
            
        }

    }

    public struct QuaternionSerializer : ITypeSerializer {

        public byte GetTypeValue() { return (byte)TypeValue.Quaternion; }
        public System.Type GetTypeSerialized() { return typeof(Quaternion); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (Quaternion)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<Quaternion>(stream);
            
        }

    }

}
