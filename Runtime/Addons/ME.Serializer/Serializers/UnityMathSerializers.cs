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

            Serializer.PackBlittable(stream, (float2)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<float2>(stream);
            
        }

    }

    public struct Float3Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Float3; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(float3); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (float3)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<float3>(stream);
            
        }

    }

    public struct Float4Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Float4; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(float4); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (float4)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<float4>(stream);
            
        }

    }

    public struct QuaternionMathSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.FQuaternion; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(quaternion); }
        
        public void Pack(Packer stream, object obj) {

            Serializer.PackBlittable(stream, (quaternion)obj);
            
        }
        
        public object Unpack(Packer stream) {

            return Serializer.UnpackBlittable<quaternion>(stream);
            
        }

    }

}
#endif