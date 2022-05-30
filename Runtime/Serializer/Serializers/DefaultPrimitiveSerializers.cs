using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

namespace ME.ECS.Serializer {

    public struct StringSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.String; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(string); }
        
        [INLINE(256)] public static void PackDirect(Packer packer, string obj) {

            var count = System.Text.Encoding.UTF8.GetMaxByteCount(obj.Length);
            packer.AddCapacity(count + 4);

            var pos = packer.GetPosition();
            var stream = packer.GetBuffer();
            var bytesCount = System.Text.Encoding.UTF8.GetBytes(obj, 0, obj.Length, stream, pos + 4);
            packer.SetPosition(pos);
            Int32Serializer.PackDirect(packer, bytesCount);
            packer.SetPosition(pos + 4 + bytesCount);
            
        }
        
        [INLINE(256)] public static string UnpackDirect(Packer packer) {

            var length = Int32Serializer.UnpackDirect(packer);
            var stream = packer.GetBuffer();
            var pos = packer.GetPosition();
            packer.SetPosition(pos + length);
            return System.Text.Encoding.UTF8.GetString(stream, pos, length);
            
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            StringSerializer.PackDirect(packer, (string)obj);
            
        }
        
        [INLINE(256)] public object Unpack(Packer packer) {

            return StringSerializer.UnpackDirect(packer);

        }

    }

    public struct CharSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Char; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(char); }
        
        [INLINE(256)] public static void PackDirect(Packer packer, char obj) {

            UInt16Serializer.PackDirect(packer, obj);
            
        }
        
        [INLINE(256)] public static char UnpackDirect(Packer packer) {

            return (char)UInt16Serializer.UnpackDirect(packer);
            
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            CharSerializer.PackDirect(packer, (char)obj);
            
        }
        
        [INLINE(256)] public object Unpack(Packer packer) {

            return CharSerializer.UnpackDirect(packer);

        }

    }

    public struct EnumSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Enum; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(System.Enum); }
        
        public void Pack(Packer stream, object obj) {

            var e = obj as System.Enum;
            var type = e.GetType().GetEnumUnderlyingType();

            if (type == typeof(int)) {
                
                stream.WriteByte((byte)TypeValue.Int32);
                var ser = new Int32Serializer();
                ser.Pack(stream, obj);
                
            } else if (type == typeof(short)) {
                
                stream.WriteByte((byte)TypeValue.Int16);
                var ser = new Int16Serializer();
                ser.Pack(stream, obj);
                
            } else if (type == typeof(long)) {
                
                stream.WriteByte((byte)TypeValue.Int64);
                var ser = new Int64Serializer();
                ser.Pack(stream, obj);
                
            } else if (type == typeof(uint)) {
                
                stream.WriteByte((byte)TypeValue.UInt32);
                var ser = new UInt32Serializer();
                ser.Pack(stream, obj);
                
            } else if (type == typeof(ushort)) {
                
                stream.WriteByte((byte)TypeValue.UInt16);
                var ser = new UInt16Serializer();
                ser.Pack(stream, obj);
                
            } else if (type == typeof(ulong)) {
                
                stream.WriteByte((byte)TypeValue.UInt64);
                var ser = new UInt64Serializer();
                ser.Pack(stream, obj);
                
            } else if (type == typeof(byte)) {
                
                stream.WriteByte((byte)TypeValue.Byte);
                var ser = new ByteSerializer();
                ser.Pack(stream, obj);
                
            } else if (type == typeof(sbyte)) {
                
                stream.WriteByte((byte)TypeValue.SByte);
                var ser = new SByteSerializer();
                ser.Pack(stream, obj);
                
            } else {
                
                stream.WriteByte((byte)TypeValue.String);
                var ser = new StringSerializer();
                ser.Pack(stream, obj);
                
            }

        }
        
        public object Unpack(Packer stream) {

            object res = null;
            var enumType = (TypeValue)stream.ReadByte();
            
            if (enumType == TypeValue.Int32) {
                
                var ser = new Int32Serializer();
                res = ser.Unpack(stream);
                
            } else if (enumType == TypeValue.Int16) {
                
                var ser = new Int16Serializer();
                res = ser.Unpack(stream);
                
            } else if (enumType == TypeValue.Int64) {
                
                var ser = new Int64Serializer();
                res = ser.Unpack(stream);
                
            } else if (enumType == TypeValue.UInt16) {
                
                var ser = new UInt16Serializer();
                res = ser.Unpack(stream);
                
            } else if (enumType == TypeValue.UInt32) {
                
                var ser = new UInt32Serializer();
                res = ser.Unpack(stream);
                
            } else if (enumType == TypeValue.UInt64) {
                
                var ser = new UInt64Serializer();
                res = ser.Unpack(stream);
                
            } else if (enumType == TypeValue.Byte) {
                
                var ser = new ByteSerializer();
                res = ser.Unpack(stream);
                
            } else if (enumType == TypeValue.SByte) {
                
                var ser = new SByteSerializer();
                res = ser.Unpack(stream);
                
            } else {
                
                var ser = new StringSerializer();
                var str = (string)ser.Unpack(stream);
                res = System.Enum.Parse(typeof(System.Enum), str);

            }
            
            return res;

        }

    }

    public struct UInt16Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.UInt16; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(System.UInt16); }
        
        [INLINE(256)] public static void PackDirect(Packer packer, ushort value) {

            const int size = 2;
            Serializer.PackBlittable(packer, value, size);
            
        }
        
        [INLINE(256)] public static ushort UnpackDirect(Packer packer) {

            const int size = 2;
            return Serializer.UnpackBlittable<ushort>(packer, size);
            
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            UInt16Serializer.PackDirect(packer, (ushort)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return UInt16Serializer.UnpackDirect(packer);

        }

    }

    public struct Int16Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Int16; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(System.Int16); }
        
        [INLINE(256)] public static void PackDirect(Packer packer, short value) {

            const int size = 2;
            Serializer.PackBlittable(packer, value, size);
            
        }
        
        [INLINE(256)] public static short UnpackDirect(Packer packer) {

            const int size = 2;
            return Serializer.UnpackBlittable<short>(packer, size);
            
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            Int16Serializer.PackDirect(packer, (short)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return Int16Serializer.UnpackDirect(packer);

        }

    }

    public struct Int32Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Int32; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(System.Int32); }
        
        public static unsafe void PackDirect(Packer packer, int value) {

            const int size = 4;
            Serializer.PackBlittable(packer, value, size);
            
        }
        
        [INLINE(256)] public static unsafe int UnpackDirect(Packer packer) {

            const int size = 4;
            return Serializer.UnpackBlittable<int>(packer, size);
            
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            Int32Serializer.PackDirect(packer, (int)obj);

        }
        
        [INLINE(256)] public object Unpack(Packer packer) {

            return Int32Serializer.UnpackDirect(packer);

        }

    }

    public struct UInt32Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.UInt32; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(System.UInt32); }
        
        public static unsafe void PackDirect(Packer packer, uint value) {

            const int size = 4;
            Serializer.PackBlittable(packer, value, size);
            
        }
        
        [INLINE(256)] public static unsafe uint UnpackDirect(Packer packer) {

            const int size = 4;
            return Serializer.UnpackBlittable<uint>(packer, size);
            
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            UInt32Serializer.PackDirect(packer, (uint)obj);

        }
        
        [INLINE(256)] public object Unpack(Packer packer) {

            return UInt32Serializer.UnpackDirect(packer);

        }

    }

    public struct Int64Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Int64; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(System.Int64); }
        
        [INLINE(256)] public static void PackDirect(Packer packer, long value) {

            const int size = 8;
            Serializer.PackBlittable(packer, value, size);
            
        }
        
        [INLINE(256)] public static long UnpackDirect(Packer packer) {

            const int size = 8;
            return Serializer.UnpackBlittable<long>(packer, size);
            
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            Int64Serializer.PackDirect(packer, (long)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return Int64Serializer.UnpackDirect(packer);

        }

    }

    public struct UInt64Serializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.UInt64; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(System.UInt64); }
        
        [INLINE(256)] public static void PackDirect(Packer packer, ulong value) {

            const int size = 8;
            Serializer.PackBlittable(packer, value, size);
            
        }
        
        [INLINE(256)] public static ulong UnpackDirect(Packer packer) {

            const int size = 8;
            return Serializer.UnpackBlittable<ulong>(packer, size);
            
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            UInt64Serializer.PackDirect(packer, (ulong)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return UInt64Serializer.UnpackDirect(packer);

        }

    }

    public struct DoubleSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Double; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(System.Double); }
        
        [INLINE(256)] public static void PackDirect(Packer packer, double value) {

            const int size = 8;
            Serializer.PackBlittable(packer, value, size);
            
        }
        
        [INLINE(256)] public static double UnpackDirect(Packer packer) {

            const int size = 8;
            return Serializer.UnpackBlittable<double>(packer, size);
            
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            DoubleSerializer.PackDirect(packer, (double)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return DoubleSerializer.UnpackDirect(packer);

        }

    }

    public struct FloatSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Float; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(System.Single); }

        [INLINE(256)] public static void PackDirect(Packer packer, float value) {

            const int size = 4;
            Serializer.PackBlittable(packer, value, size);
            
        }
        
        [INLINE(256)] public static float UnpackDirect(Packer packer) {

            const int size = 4;
            return Serializer.UnpackBlittable<float>(packer, size);
            
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            FloatSerializer.PackDirect(packer, (float)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return FloatSerializer.UnpackDirect(packer);

        }

    }

    public struct BooleanSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Boolean; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(System.Boolean); }

        [INLINE(256)] public static void PackDirect(Packer packer, bool obj) {

            byte b = (bool)obj == true ? (byte)1 : (byte)0;
            packer.WriteByte(b);

        }
        
        [INLINE(256)] public static bool UnpackDirect(Packer packer) {
            
            var b = packer.ReadByte();
            return b == 1 ? true : false;

        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            BooleanSerializer.PackDirect(packer, (bool)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return BooleanSerializer.UnpackDirect(packer);

        }

    }

    public struct ByteSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Byte; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(System.Byte); }

        [INLINE(256)] public static void PackDirect(Packer packer, byte obj) {

            packer.WriteByte(obj);

        }
        
        [INLINE(256)] public static byte UnpackDirect(Packer packer) {
            
            return packer.ReadByte();
            
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            ByteSerializer.PackDirect(packer, (byte)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return ByteSerializer.UnpackDirect(packer);

        }

    }

    public struct SByteSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.SByte; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(System.SByte); }

        [INLINE(256)] public static void PackDirect(Packer packer, sbyte obj) {

            packer.WriteByte((byte)obj);

        }
        
        [INLINE(256)] public static sbyte UnpackDirect(Packer packer) {

            return (sbyte)packer.ReadByte();
            
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            SByteSerializer.PackDirect(packer, (sbyte)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return SByteSerializer.UnpackDirect(packer);

        }

    }
    
    #if UNITY
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

}
