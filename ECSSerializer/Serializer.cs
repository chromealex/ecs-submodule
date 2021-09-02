using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Serializer {

    public interface ISerializationObject {

        void OnAfterSerialization();

    }
    
    public interface ITypeSerializerInherit {

        

    }

    public interface ITypeSerializer {

        byte GetTypeValue();
        System.Type GetTypeSerialized();
        
        void   Pack(Packer packer, object obj);
        object Unpack(Packer packer);

    }

    
    
    public enum TypeValue : byte {

        Null = 200,
        PackerObject  = 255,
        Meta          = 254,
        MetaType      = 253,
        MetaTypeArray = 252,

        Boolean = 251,
        String  = 250,
        Enum    = 249,
        Byte    = 248,
        SByte   = 247,
        Int16   = 246,
        Int32   = 245,
        Int64   = 244,
        UInt16  = 243,
        UInt32  = 242,
        UInt64  = 241,
        Float   = 240,
        Double  = 239,

        ByteArray   = 238,
        ObjectArray = 237,
        GenericList = 236,
        
        Vector2Int = 235,
        Vector3Int = 234,
        Vector2    = 233,
        Vector3    = 232,
        Vector4    = 231,
        Quaternion = 230,

        Generic     = 229,
        GenericDictionary = 228,
        
        HistoryEvent  = 227,
        Char  = 226,
        View = 225,
        
        BufferArray = 224,
        DisposeSentinel = 223,
        
    }
    
    [System.AttributeUsageAttribute(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public class SerializeFieldAttribute : System.Attribute {

        

    }

    public struct Serializers : System.IDisposable {

        public struct Item {

            public byte typeValue;
            public System.Action<Packer, object> pack;
            public System.Func<Packer, object> unpack;

            public Item Fill<T>(T serializer) where T : struct, ITypeSerializer {

                this.typeValue = serializer.GetTypeValue();
                this.pack = serializer.Pack;
                this.unpack = serializer.Unpack;
                
                return this;

            }

            public Item Fill(ITypeSerializer serializer) {

                this.typeValue = serializer.GetTypeValue();
                this.pack = serializer.Pack;
                this.unpack = serializer.Unpack;
                
                return this;

            }

        }

        private static Dictionary<int, bool> initialized = new Dictionary<int, bool>();
        private static int idIncrement;

        private int id;
        private Dictionary<System.Type, Item> serializers;
        private Dictionary<System.Type, Item> serializersBaseType;
        private Dictionary<byte, Item> serializersByTypeValue;

        public void Init(int capacity) {

            if (this.id == 0) this.id = ++Serializers.idIncrement;
            if (this.serializers == null) this.serializers = PoolDictionary<System.Type, Item>.Spawn(capacity);
            if (this.serializersBaseType == null) this.serializersBaseType = PoolDictionary<System.Type, Item>.Spawn(capacity);
            if (this.serializersByTypeValue == null) this.serializersByTypeValue = PoolDictionary<byte, Item>.Spawn(capacity);
            
            if (Serializers.initialized.ContainsKey(this.id) == false) Serializers.initialized.Add(this.id, true);
            
        }

        public void Dispose() {
            
            if (Serializers.initialized.TryGetValue(this.id, out var state) == false || state == false) return;

            if (this.serializers != null) PoolDictionary<System.Type, Item>.Recycle(ref this.serializers);
            if (this.serializersBaseType != null) PoolDictionary<System.Type, Item>.Recycle(ref this.serializersBaseType);
            if (this.serializersByTypeValue != null) PoolDictionary<byte, Item>.Recycle(ref this.serializersByTypeValue);
            
            Serializers.initialized.Remove(this.id);

        }
        
        public void Add(ref Serializers serializers) {

            if (Serializers.initialized.TryGetValue(this.id, out var state) == false || state == false) return;
            
            this.Init(32);
            serializers.Init(32);

            foreach (var kv in serializers.serializers) {
                
                if (this.serializers.ContainsKey(kv.Key) == false) this.serializers.Add(kv.Key, kv.Value);
                
            }
            
            foreach (var kv in serializers.serializersBaseType) {
                
                if (this.serializersBaseType.ContainsKey(kv.Key) == false) this.serializersBaseType.Add(kv.Key, kv.Value);
                
            }

            foreach (var kv in serializers.serializersByTypeValue) {
                
                if (this.serializersByTypeValue.ContainsKey(kv.Key) == false) this.serializersByTypeValue.Add(kv.Key, kv.Value);
                
            }

        }
        
        public void Add<T>(T serializer) where T : struct, ITypeSerializer {

            #if UNITY_EDITOR
            if (serializer.GetTypeSerialized().IsGenericTypeDefinition) {
                //https://github.com/mono/mono/issues/7095
                throw new System.Exception("Using GenericTypeDefinition for Serializer Type cause crash on Mono Runtime. Do not use it.");
            }
            #endif
            
            this.Init(32);

            this.serializers.Add(serializer.GetTypeSerialized(), new Item().Fill(serializer));
            if (serializer is ITypeSerializerInherit) this.serializersBaseType.Add(serializer.GetTypeSerialized(), new Item().Fill(serializer));
            this.serializersByTypeValue.Add(serializer.GetTypeValue(), new Item().Fill(serializer));

        }

        public void Add(ITypeSerializer serializer) {

            #if UNITY_EDITOR
            if (serializer.GetTypeSerialized().IsGenericTypeDefinition) {
                //https://github.com/mono/mono/issues/7095
                throw new System.Exception("Using GenericTypeDefinition for Serializer Type cause crash on Mono Runtime. Do not use it.");
            }
            #endif
            
            this.Init(32);

            this.serializers.Add(serializer.GetTypeSerialized(), new Item().Fill(serializer));
            if (serializer is ITypeSerializerInherit) this.serializersBaseType.Add(serializer.GetTypeSerialized(), new Item().Fill(serializer));
            this.serializersByTypeValue.Add(serializer.GetTypeValue(), new Item().Fill(serializer));

        }

        public bool TryGetValue(byte type, out Item serializer) {

            return this.serializersByTypeValue.TryGetValue(type, out serializer);

        }

        public bool TryGetValue(System.Type type, out Item serializer) {

            if (this.serializers.TryGetValue(type, out serializer) == true) {

                return true;

            }
            
            foreach (var kv in this.serializersBaseType) {
                
                if (kv.Key.IsAssignableFrom(type) == true) {

                    serializer = kv.Value;
                    return true;

                }
                
            }
            
            return this.serializers.TryGetValue(typeof(GenericSerializer), out serializer);

        }

    }

    public enum SerializerMode {

        SerializableOnlyFields,
        AllFields,

    }

    public static class Serializer {

        public const int BUFFER_CAPACITY = 1024;

        public static SerializerMode mode = SerializerMode.SerializableOnlyFields;

        public static Serializers GetInternalSerializers() {
            
            var serializers = new Serializers();
            serializers.Add(new PackerObjectSerializer());
            serializers.Add(new MetaSerializer());
            serializers.Add(new MetaTypeSerializer());
            serializers.Add(new MetaTypeArraySerializer());

            return serializers;

        }

        public static Serializers GetDefaultSerializers() {

            var serializers = new Serializers();
            
            serializers.Add(new GenericSerializer());
            
            serializers.Add(new ByteSerializer());
            serializers.Add(new SByteSerializer());
            
            serializers.Add(new FloatSerializer());
            serializers.Add(new DoubleSerializer());
            
            serializers.Add(new Int16Serializer());
            serializers.Add(new Int32Serializer());
            serializers.Add(new Int64Serializer());
            
            serializers.Add(new UInt16Serializer());
            serializers.Add(new UInt32Serializer());
            serializers.Add(new UInt64Serializer());
            
            serializers.Add(new BooleanSerializer());
            serializers.Add(new StringSerializer());
            serializers.Add(new EnumSerializer());

            serializers.Add(new Vector2IntSerializer());
            serializers.Add(new Vector3IntSerializer());
            serializers.Add(new Vector2Serializer());
            serializers.Add(new Vector3Serializer());
            serializers.Add(new Vector4Serializer());
            serializers.Add(new QuaternionSerializer());

            serializers.Add(new ByteArraySerializer());
            serializers.Add(new ObjectArraySerializer());
            serializers.Add(new GenericListSerializer());
            serializers.Add(new GenericDictionarySerializer());
            
            return serializers;

        }

        public static byte[] PackStatic<T>(T obj, Serializers staticSerializers) {
            
            var packer = new Packer(staticSerializers, new System.IO.MemoryStream(Serializer.BUFFER_CAPACITY));

            var serializer = new GenericSerializer();
            serializer.Pack(packer, obj, typeof(T));

            var result = packer.ToArray();
            packer.Dispose();
            return result;

        }

        public static T UnpackStatic<T>(byte[] bytes, Serializers staticSerializers) {

            var packer = Serializer.SetupDefaultPacker(staticSerializers, bytes);

            var serializer = new GenericSerializer();
            var result = (T)serializer.Unpack(packer, typeof(T));
            packer.Dispose();
            return result;

        }

        public static byte[] Pack<T>(T obj, System.Type type, Serializers customSerializers) {

            var serializersInternal = Serializer.GetInternalSerializers();
            var serializers = Serializer.GetDefaultSerializers();
            serializers.Add(ref serializersInternal);
            serializers.Add(ref customSerializers);
            serializersInternal.Dispose();
            customSerializers.Dispose();

            var packer = new Packer(serializers, new System.IO.MemoryStream(Serializer.BUFFER_CAPACITY));

            var serializer = new GenericSerializer();
            serializer.Pack(packer, obj, type);

            var bytes = packer.ToArray();
            
            serializers.Dispose();
            packer.Dispose();
            
            return bytes;

        }

        public static byte[] Pack<T>(T obj) {

            return Serializer.Pack(obj, obj.GetType(), new Serializers());

        }

        public static byte[] Pack<T>(T obj, Serializers customSerializers) {

            return Serializer.Pack(obj, obj.GetType(), customSerializers);

        }

        public static byte[] Pack<T>(Serializers allSerializers, T obj) {

            byte[] bytes = null;
            var packer = new Packer(allSerializers, new System.IO.MemoryStream(Serializer.BUFFER_CAPACITY));

            var serializer = new GenericSerializer();
            serializer.Pack(packer, obj, typeof(T));

            bytes = packer.ToArray();
            packer.Dispose();
            allSerializers.Dispose();
            return bytes;

        }

        public static T Unpack<T>(byte[] bytes) {

            return Serializer.Unpack<T>(bytes, new Serializers());

        }

        public static T Unpack<T>(byte[] bytes, T objectToOverwrite) where T : class {

            return Serializer.Unpack<T>(bytes, new Serializers(), objectToOverwrite);

        }

        public static T Unpack<T>(byte[] bytes, Serializers customSerializers) {

            var packer = Serializer.SetupDefaultPacker(bytes, customSerializers);

            var serializer = new GenericSerializer();
            var instance   = (T)serializer.Unpack(packer, typeof(T));
            customSerializers.Dispose();
            packer.serializers.Dispose();
            packer.Dispose();
            return instance;

        }

        public static T Unpack<T>(Serializers allSerializers, byte[] bytes) {

            var packer = Serializer.SetupDefaultPacker(allSerializers, bytes);

            var serializer = new GenericSerializer();
            var instance   = (T)serializer.Unpack(packer, typeof(T));
            allSerializers.Dispose();
            packer.Dispose();
            return instance;

        }

        public static T Unpack<T>(byte[] bytes, Serializers customSerializers, T objectToOverwrite) where T : class {
            
            var packer = Serializer.SetupDefaultPacker(bytes, customSerializers);
            new GenericSerializer().Unpack(packer, objectToOverwrite);
            customSerializers.Dispose();
            packer.serializers.Dispose();
            packer.Dispose();
            return objectToOverwrite;

        }

        public static Packer SetupDefaultPacker(byte[] bytes, Serializers customSerializers) {
            
            var serializersInternal = Serializer.GetInternalSerializers();
            var serializers = Serializer.GetDefaultSerializers();
            serializers.Add(ref serializersInternal);
            serializers.Add(ref customSerializers);
            serializersInternal.Dispose();
            customSerializers.Dispose();

            System.IO.MemoryStream stream;
            if (bytes == null) {
                stream = new System.IO.MemoryStream(Serializer.BUFFER_CAPACITY);
            } else {
                stream = new System.IO.MemoryStream(bytes);
            }

            return Packer.FromStream(serializers, stream);
        }

        public static Packer SetupDefaultPacker(Serializers allSerializers, byte[] bytes) {
            
            System.IO.MemoryStream stream;
            if (bytes == null) {
                stream = new System.IO.MemoryStream(Serializer.BUFFER_CAPACITY);
            } else {
                stream = new System.IO.MemoryStream(bytes);
            }

            return Packer.FromStream(allSerializers, stream);
        }

    }

    public class Packer {

        public struct PackerObject {

            public Meta meta;
            public byte[] data;

        }

        public struct MetaType {

            public int id;
            public string type;

        }

        public struct Meta {

            internal int metaTypeId;
            internal Dictionary<System.Type, MetaType> meta;

            public static Meta Create() {

                return new Meta() {
                    metaTypeId = 0,
                    meta = PoolDictionary<System.Type, MetaType>.Spawn(8),
                };

            }

            public void Dispose() {

                PoolDictionary<System.Type, MetaType>.Recycle(ref this.meta);

            }

            public System.Type GetMetaType(int typeId) {

                foreach (var kv in this.meta) {

                    if (kv.Value.id == typeId) {

                        return kv.Key;

                    }

                }

                return null;

            }

            public bool TryGetValue(System.Type type, out MetaType metaType) {

                return this.meta.TryGetValue(type, out metaType);

            }

            public int Add(System.Type type) {

                var typeId = new MetaType();
                typeId.id = ++this.metaTypeId;
                typeId.type = type.FullName;
                this.meta.Add(type, typeId);

                return typeId.id;

            }

        }

        internal Serializers serializers;
        private System.IO.MemoryStream stream;
        private Meta meta;

        public static Packer FromStream(Serializers serializers, System.IO.MemoryStream stream) {

            var packer = new Packer(serializers, stream);
            var packerObject = PackerObjectSerializer.UnpackDirect(packer);
            packer.meta = packerObject.meta;
            packer.stream = new System.IO.MemoryStream(packerObject.data);

            return packer;

        }

        public Packer(Serializers serializers, System.IO.MemoryStream stream) {

            this.meta = Meta.Create();
            this.serializers = serializers;
            this.stream = stream;

        }

        public void Dispose() {

            this.meta.Dispose();

        }

        public byte[] ToArray() {

            var obj = new PackerObject();
            obj.meta = this.meta;
            obj.data = this.stream.ToArray();

            byte[] output = null;
            using (var stream = new System.IO.MemoryStream(this.stream.Capacity)) {

                var packer = new Packer(this.serializers, stream);
                PackerObjectSerializer.PackDirect(packer, obj);

                output = stream.ToArray();

            }

            return output;

        }

        public System.Type GetMetaType(int typeId) {

            if (typeId < 0) {
                
                var valueType = (TypeValue)(-typeId - 1);
                switch (valueType) {
                    
                    case TypeValue.Int16: return typeof(System.Int16);
                    case TypeValue.Int32: return typeof(System.Int32);
                    case TypeValue.Int64: return typeof(System.Int64);
                    case TypeValue.UInt16: return typeof(System.UInt16);
                    case TypeValue.UInt32: return typeof(System.UInt32);
                    case TypeValue.UInt64: return typeof(System.UInt64);
                    case TypeValue.Byte: return typeof(byte);
                    case TypeValue.SByte: return typeof(sbyte);
                    case TypeValue.Float: return typeof(float);
                    case TypeValue.Double: return typeof(double);
                    case TypeValue.Boolean: return typeof(bool);
                    case TypeValue.String: return typeof(string);
                    case TypeValue.Enum: return typeof(System.Enum);
                    
                }
                
            }
            
            return this.meta.GetMetaType(typeId);

        }

        public int GetMetaTypeId(System.Type type) {

            var isPrimitive = false;
            int pValue = 0;
            if (type == typeof(System.Int16)) {

                isPrimitive = true;
                pValue = (int)TypeValue.Int16;

            } else if (type == typeof(System.Int32)) {

                isPrimitive = true;
                pValue = (int)TypeValue.Int32;

            } else if (type == typeof(System.Int64)) {

                isPrimitive = true;
                pValue = (int)TypeValue.Int64;

            } else if (type == typeof(System.UInt16)) {

                isPrimitive = true;
                pValue = (int)TypeValue.UInt16;

            } else if (type == typeof(System.UInt32)) {

                isPrimitive = true;
                pValue = (int)TypeValue.UInt32;

            } else if (type == typeof(System.UInt64)) {

                isPrimitive = true;
                pValue = (int)TypeValue.UInt64;

            } else if (type == typeof(System.Boolean)) {

                isPrimitive = true;
                pValue = (int)TypeValue.Boolean;

            } else if (type == typeof(System.String)) {

                isPrimitive = true;
                pValue = (int)TypeValue.String;

            } else if (type == typeof(System.Byte)) {

                isPrimitive = true;
                pValue = (int)TypeValue.Byte;

            } else if (type == typeof(System.SByte)) {

                isPrimitive = true;
                pValue = (int)TypeValue.SByte;

            } else if (type == typeof(System.Single)) {

                isPrimitive = true;
                pValue = (int)TypeValue.Float;

            } else if (type == typeof(System.Double)) {

                isPrimitive = true;
                pValue = (int)TypeValue.Double;

            } else if (type == typeof(System.Enum)) {

                isPrimitive = true;
                pValue = (int)TypeValue.Enum;

            }

            if (isPrimitive == true) {

                pValue = -pValue - 1;
                return pValue;

            }
            
            if (this.meta.TryGetValue(type, out var typeId) == true) {

                return typeId.id;

            }

            return this.meta.Add(type);

        }

        public byte ReadByte() {

            return (byte)this.stream.ReadByte();

        }

        public void ReadBytes(byte[] output) {

            this.stream.Read(output, 0, output.Length);

        }

        public void WriteByte(byte @byte) {

            this.stream.WriteByte(@byte);
            
        }

        public void WriteBytes(byte[] bytes) {

            this.stream.Write(bytes, 0, bytes.Length);
            
        }

        public void PackInternal<T>(T root) {
            
            if (root == null) {

                this.WriteByte((byte)TypeValue.Null);
                return;

            }

            this.PackInternal(root, typeof(T));
            
        }

        public void PackInternal<T>(T root, System.Type rootType) {
            
            if (this.serializers.TryGetValue(rootType, out var serializer) == true) {

                this.WriteByte(serializer.typeValue);
                serializer.pack.Invoke(this, root);

            } else {

                if (rootType.IsArray == true) {
                    
                    // Custom array type
                    
                    return;

                }
                
                if (rootType.IsPrimitive == true || rootType.IsArray == true) {

                    Debug.LogError("Pack type has failed: " + rootType);
                    return;

                }

                var fields = rootType.GetCachedFields();
                for (int i = 0; i < fields.Length; ++i) {

                    var val = fields[i].GetValue(root);
                    var type = fields[i].GetFieldType();
                    if (this.serializers.TryGetValue(type, out var ser) == true) {

                        this.WriteByte(ser.typeValue);
                        ser.pack.Invoke(this, val);

                    } else {

                        this.PackInternal(val);

                    }

                }

            }
            
        }

        public void PackInternal(object root) {

            if (root == null) {

                this.WriteByte((byte)TypeValue.Null);
                return;

            }
            
            var rootType = root.GetType();
            this.PackInternal(root, rootType);

        }

        public T UnpackInternal<T>() {
            
            T obj = default;
            var type = this.ReadByte();
            if (type == (byte)TypeValue.Null) {

                return default;

            }
            
            if (this.serializers.TryGetValue(type, out var ser) == true) {

                obj = (T)ser.unpack.Invoke(this);
                if (typeof(ISerializationObject).IsAssignableFrom(typeof(T)) == true) {

                    ((ISerializationObject)obj).OnAfterSerialization();

                }

            } else {

                Debug.LogWarning("Unknown type: " + type);

            }

            return obj;
            
        }

        public object UnpackInternal() {

            return this.UnpackInternal<object>();
            
        }

    }

}