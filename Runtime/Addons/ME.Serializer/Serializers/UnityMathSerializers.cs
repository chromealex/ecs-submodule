#if UNITY
#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif
using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

namespace ME.ECS.Serializer {

    #if FIXED_POINT_MATH
    public struct FPSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.FPFloat; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(sfloat); }

        [INLINE(256)] public static void PackDirect(Packer packer, sfloat obj) {

            UInt32Serializer.PackDirect(packer, obj.RawValue);
            
        }
        
        [INLINE(256)] public static sfloat UnpackDirect(Packer packer) {

            return sfloat.FromRaw(UInt32Serializer.UnpackDirect(packer));

        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            FPSerializer.PackDirect(packer, (sfloat)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return FPSerializer.UnpackDirect(packer);

        }

    }
    #endif

    public struct Int2Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Int2; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(int2); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (int2)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<int2>(stream);
            
        }

    }

    public struct Int3Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Int3; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(int3); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (int3)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<int3>(stream);
            
        }

    }

    public struct Float2Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Float2; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(float2); }
        
        public void Pack(Packer stream, object obj) {

            var v = (float2)obj;
            Serializer.PackSingle(stream, v.x);
            Serializer.PackSingle(stream, v.y);
            
        }
        
        public object Unpack(Packer stream) {

            float2 res = default;
            res.x = Serializer.UnpackSingle(stream);
            res.y = Serializer.UnpackSingle(stream);
            return res;
            
        }

    }

    public struct Float3Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Float3; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(float3); }
        
        public void Pack(Packer stream, object obj) {

            var v = (float3)obj;
            Serializer.PackSingle(stream, v.x);
            Serializer.PackSingle(stream, v.y);
            Serializer.PackSingle(stream, v.z);
            
        }
        
        public object Unpack(Packer stream) {

            float3 res = default;
            res.x = Serializer.UnpackSingle(stream);
            res.y = Serializer.UnpackSingle(stream);
            res.z = Serializer.UnpackSingle(stream);
            return res;
            
        }

    }

    public struct Float4Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Float4; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(float4); }
        
        public void Pack(Packer stream, object obj) {

            var v = (float4)obj;
            Serializer.PackSingle(stream, v.x);
            Serializer.PackSingle(stream, v.y);
            Serializer.PackSingle(stream, v.z);
            Serializer.PackSingle(stream, v.w);
            
        }
        
        public object Unpack(Packer stream) {

            float4 res = default;
            res.x = Serializer.UnpackSingle(stream);
            res.y = Serializer.UnpackSingle(stream);
            res.z = Serializer.UnpackSingle(stream);
            res.w = Serializer.UnpackSingle(stream);
            return res;
            
        }

    }

    public struct QuaternionMathSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.FQuaternion; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(quaternion); }
        
        public void Pack(Packer stream, object obj) {

            var v = (quaternion)obj;
            Serializer.PackSingle(stream, v.value.x);
            Serializer.PackSingle(stream, v.value.y);
            Serializer.PackSingle(stream, v.value.z);
            Serializer.PackSingle(stream, v.value.w);
            
        }
        
        public object Unpack(Packer stream) {

            quaternion res = default;
            res.value.x = Serializer.UnpackSingle(stream);
            res.value.y = Serializer.UnpackSingle(stream);
            res.value.z = Serializer.UnpackSingle(stream);
            res.value.w = Serializer.UnpackSingle(stream);
            return res;
            
        }

    }

}
#endif