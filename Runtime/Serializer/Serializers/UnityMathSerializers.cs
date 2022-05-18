using UnityEngine;
using Unity.Mathematics;

namespace ME.ECS.Serializer {

    public struct Int2Serializer : ITypeSerializer {

        public byte GetTypeValue() { return (byte)TypeValue.Int2; }
        public System.Type GetTypeSerialized() { return typeof(int2); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (int2)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<int2>(stream);
            
        }

    }

    public struct Int3Serializer : ITypeSerializer {

        public byte GetTypeValue() { return (byte)TypeValue.Int3; }
        public System.Type GetTypeSerialized() { return typeof(int3); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (int3)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<int3>(stream);
            
        }

    }

    public struct Float2Serializer : ITypeSerializer {

        public byte GetTypeValue() { return (byte)TypeValue.Float2; }
        public System.Type GetTypeSerialized() { return typeof(float2); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (float2)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<float2>(stream);
            
        }

    }

    public struct Float3Serializer : ITypeSerializer {

        public byte GetTypeValue() { return (byte)TypeValue.Float3; }
        public System.Type GetTypeSerialized() { return typeof(float3); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (float3)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<float3>(stream);
            
        }

    }

    public struct Float4Serializer : ITypeSerializer {

        public byte GetTypeValue() { return (byte)TypeValue.Float4; }
        public System.Type GetTypeSerialized() { return typeof(float4); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (float4)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<float4>(stream);
            
        }

    }

    public struct QuaternionMathSerializer : ITypeSerializer {

        public byte GetTypeValue() { return (byte)TypeValue.FQuaternion; }
        public System.Type GetTypeSerialized() { return typeof(quaternion); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (quaternion)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<quaternion>(stream);
            
        }

    }

}
