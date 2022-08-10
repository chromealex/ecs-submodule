using System.Collections.Generic;
using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

namespace ME.ECS.Serializer {

    public interface ISerializationObject {

        void OnAfterSerialization();

    }

    public interface ITypeSerializerInherit { }

    public interface ITypeSerializer {

        byte GetTypeValue();
        System.Type GetTypeSerialized();

        void Pack(Packer packer, object obj);
        object Unpack(Packer packer);

    }

    public enum TypeValue : byte {

        Null = 190,
        PackerObject = 255,
        Meta = 254,
        MetaType = 253,
        MetaTypeArray = 252,

        Boolean = 251,
        String = 250,
        Enum = 249,
        Byte = 248,
        SByte = 247,
        Int16 = 246,
        Int32 = 245,
        Int64 = 244,
        UInt16 = 243,
        UInt32 = 242,
        UInt64 = 241,
        Float = 240,
        Double = 239,

        ByteArray = 238,
        ObjectArray = 237,
        GenericList = 236,

        Vector2Int = 235,
        Vector3Int = 234,
        Vector2 = 233,
        Vector3 = 232,
        Vector4 = 231,
        Quaternion = 230,

        Generic = 229,
        GenericDictionary = 228,
        GenericULongDictionary = 227,
        GenericIntDictionary = 226,

        HistoryEvent = 225,
        Char = 224,
        View = 223,

        BufferArray = 222,
        DisposeSentinel = 221,
        FPFloat = 220,

        Int2 = 219,
        Int3 = 218,
        Float2 = 217,
        Float3 = 216,
        Float4 = 215,
        FQuaternion = 214,

        SByteArray = 213,
        Int16Array = 212,
        Int32Array = 211,
        Int64Array = 210,
        UInt16Array = 209,
        UInt32Array = 208,
        UInt64Array = 207,
        FloatArray = 206,
        DoubleArray = 205,
        
        Tick = 204,
        ViewId = 203,
        RPCId = 202,
        NextTickTask = 201,
        UnsafeData = 200,
        MemoryAllocator = 199,

    }

    [System.AttributeUsageAttribute(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public class SerializeFieldAttribute : System.Attribute { }

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
        internal Dictionary<System.Type, int> metaCache;
        private int id;
        private Dictionary<System.Type, Item> serializers;
        private Dictionary<System.Type, Item> serializersBaseType;
        private Dictionary<byte, Item> serializersByTypeValue;

        public void Init(int capacity) {

            if (this.id == 0) {
                this.id = ++Serializers.idIncrement;
            }

            if (this.serializers == null) {
                this.serializers = PoolDictionary<System.Type, Item>.Spawn(capacity);
            }

            if (this.serializersBaseType == null) {
                this.serializersBaseType = PoolDictionary<System.Type, Item>.Spawn(capacity);
            }

            if (this.serializersByTypeValue == null) {
                this.serializersByTypeValue = PoolDictionary<byte, Item>.Spawn(capacity);
            }

            if (this.metaCache == null) {
                this.metaCache = PoolDictionary<System.Type, int>.Spawn(capacity);
            }

            if (Serializers.initialized.ContainsKey(this.id) == false) {
                Serializers.initialized.Add(this.id, true);
            }

        }

        public void Dispose() {

            if (Serializers.initialized.TryGetValue(this.id, out var state) == false || state == false) {
                return;
            }

            if (this.serializers != null) {
                PoolDictionary<System.Type, Item>.Recycle(ref this.serializers);
            }

            if (this.serializersBaseType != null) {
                PoolDictionary<System.Type, Item>.Recycle(ref this.serializersBaseType);
            }

            if (this.serializersByTypeValue != null) {
                PoolDictionary<byte, Item>.Recycle(ref this.serializersByTypeValue);
            }

            if (this.metaCache != null) {
                PoolDictionary<System.Type, int>.Recycle(ref this.metaCache);
            }

            Serializers.initialized.Remove(this.id);

        }

        public void AddMeta(System.Type type, int typeId) {

            this.Init(32);
            this.metaCache.Add(type, typeId);

        }

        [INLINE(256)]
        public void Add(ref Serializers serializers) {

            if (Serializers.initialized.TryGetValue(this.id, out var state) == false || state == false) {
                return;
            }

            this.Init(32);
            serializers.Init(32);

            foreach (var kv in serializers.metaCache) {

                if (this.metaCache.ContainsKey(kv.Key) == false) {
                    this.metaCache.Add(kv.Key, kv.Value);
                }

            }

            foreach (var kv in serializers.serializers) {

                if (this.serializers.ContainsKey(kv.Key) == false) {
                    this.serializers.Add(kv.Key, kv.Value);
                }

            }

            foreach (var kv in serializers.serializersBaseType) {

                if (this.serializersBaseType.ContainsKey(kv.Key) == false) {
                    this.serializersBaseType.Add(kv.Key, kv.Value);
                }

            }

            foreach (var kv in serializers.serializersByTypeValue) {

                if (this.serializersByTypeValue.ContainsKey(kv.Key) == false) {
                    this.serializersByTypeValue.Add(kv.Key, kv.Value);
                }

            }

        }

        [INLINE(256)]
        public void Add<T>(T serializer) where T : struct, ITypeSerializer {

            #if UNITY_EDITOR
            if (serializer.GetTypeSerialized().IsGenericTypeDefinition) {
                //https://github.com/mono/mono/issues/7095
                throw new System.Exception("Using GenericTypeDefinition for Serializer Type cause crash on Mono Runtime. Do not use it.");
            }
            #endif

            this.Init(32);

            this.serializers.Add(serializer.GetTypeSerialized(), new Item().Fill(serializer));
            if (serializer is ITypeSerializerInherit) {
                this.serializersBaseType.Add(serializer.GetTypeSerialized(), new Item().Fill(serializer));
            }

            this.serializersByTypeValue.Add(serializer.GetTypeValue(), new Item().Fill(serializer));

        }

        [INLINE(256)]
        public void Add(ITypeSerializer serializer) {

            #if UNITY_EDITOR
            if (serializer.GetTypeSerialized().IsGenericTypeDefinition) {
                //https://github.com/mono/mono/issues/7095
                throw new System.Exception("Using GenericTypeDefinition for Serializer Type cause crash on Mono Runtime. Do not use it.");
            }
            #endif

            this.Init(32);

            this.serializers.Add(serializer.GetTypeSerialized(), new Item().Fill(serializer));
            if (serializer is ITypeSerializerInherit) {
                this.serializersBaseType.Add(serializer.GetTypeSerialized(), new Item().Fill(serializer));
            }

            this.serializersByTypeValue.Add(serializer.GetTypeValue(), new Item().Fill(serializer));

        }

        [INLINE(256)]
        public bool TryGetValue(byte type, out Item serializer) {

            return this.serializersByTypeValue.TryGetValue(type, out serializer);

        }

        [INLINE(256)]
        public bool TryGetValue(System.Type type, out Item serializer) {

            if (type.IsEnum == true) {
                type = typeof(System.Enum);
            }

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

        [INLINE(256)]
        public static Serializers GetInternalSerializers() {

            var serializers = new Serializers();
            serializers.Add(new PackerObjectSerializer());
            serializers.Add(new MetaSerializer());
            serializers.Add(new MetaTypeSerializer());
            serializers.Add(new MetaTypeArraySerializer());

            return serializers;

        }

        [INLINE(256)]
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

            serializers.Add(new FloatArraySerializer());
            serializers.Add(new DoubleArraySerializer());
            serializers.Add(new Int16ArraySerializer());
            serializers.Add(new Int32ArraySerializer());
            serializers.Add(new Int64ArraySerializer());
            serializers.Add(new UInt16ArraySerializer());
            serializers.Add(new UInt32ArraySerializer());
            serializers.Add(new UInt64ArraySerializer());
            serializers.Add(new SByteArraySerializer());

            serializers.Add(new ByteArraySerializer());
            serializers.Add(new ObjectArraySerializer());
            serializers.Add(new GenericListSerializer());
            serializers.Add(new GenericDictionarySerializer());

            #if UNITY
            #if FIXED_POINT_MATH
            serializers.Add(new FPSerializer());
            #endif

            serializers.Add(new Vector2IntSerializer());
            serializers.Add(new Vector3IntSerializer());
            serializers.Add(new Vector2Serializer());
            serializers.Add(new Vector3Serializer());
            serializers.Add(new Vector4Serializer());
            serializers.Add(new QuaternionSerializer());
            
            serializers.Add(new Int2Serializer());
            serializers.Add(new Int3Serializer());
            serializers.Add(new Float2Serializer());
            serializers.Add(new Float3Serializer());
            serializers.Add(new Float4Serializer());
            serializers.Add(new QuaternionMathSerializer());
            #endif

            return serializers;

        }

        [INLINE(256)]
        public static byte[] PackStatic<T>(T obj, Serializers staticSerializers) {

            var packer = new Packer(staticSerializers, new SerializerStream(Serializer.BUFFER_CAPACITY));

            packer.PackInternal(obj, typeof(T));

            var result = packer.ToArray();
            packer.Dispose();
            return result;

        }

        [INLINE(256)]
        public static T UnpackStatic<T>(byte[] bytes, Serializers staticSerializers) {

            var packer = Serializer.SetupDefaultPacker(staticSerializers, bytes);

            var result = (T)packer.UnpackInternal();
            packer.Dispose();
            return result;

        }

        [INLINE(256)]
        public static byte[] Pack<T>(T obj, System.Type type, Serializers customSerializers, int capacity = Serializer.BUFFER_CAPACITY) {

            var serializersInternal = Serializer.GetInternalSerializers();
            var serializers = Serializer.GetDefaultSerializers();
            serializers.Add(ref serializersInternal);
            serializers.Add(ref customSerializers);
            serializersInternal.Dispose();
            customSerializers.Dispose();

            var packer = new Packer(serializers, new SerializerStream(capacity));

            packer.PackInternal(obj, type);

            var bytes = packer.ToArray();

            serializers.Dispose();
            packer.Dispose();

            return bytes;

        }

        [INLINE(256)]
        public static byte[] Pack<T>(T obj) {

            return Serializer.Pack(obj, obj.GetType(), new Serializers());

        }

        [INLINE(256)]
        public static byte[] Pack<T>(T obj, Serializers customSerializers, int capacity = Serializer.BUFFER_CAPACITY) {

            return Serializer.Pack(obj, obj.GetType(), customSerializers, capacity);

        }

        [INLINE(256)]
        public static byte[] Pack<T>(Serializers allSerializers, T obj, int capacity = Serializer.BUFFER_CAPACITY) {

            byte[] bytes = null;
            var packer = new Packer(allSerializers, new SerializerStream(capacity));

            packer.PackInternal(obj, typeof(T));

            bytes = packer.ToArray();
            packer.Dispose();
            allSerializers.Dispose();
            return bytes;

        }

        [INLINE(256)]
        public static T Unpack<T>(byte[] bytes) {

            return Serializer.Unpack<T>(bytes, new Serializers());

        }

        [INLINE(256)]
        public static T Unpack<T>(byte[] bytes, T objectToOverwrite) where T : class {

            return Serializer.Unpack<T>(bytes, new Serializers(), objectToOverwrite);

        }

        [INLINE(256)]
        public static T Unpack<T>(byte[] bytes, Serializers customSerializers) {

            var packer = Serializer.SetupDefaultPacker(bytes, customSerializers);

            var instance = packer.UnpackInternal<T>();
            customSerializers.Dispose();
            packer.serializers.Dispose();
            packer.Dispose();
            return instance;

        }

        [INLINE(256)]
        public static T Unpack<T>(Serializers allSerializers, byte[] bytes) {

            var packer = Serializer.SetupDefaultPacker(allSerializers, bytes);

            var instance = packer.UnpackInternal<T>();
            allSerializers.Dispose();
            packer.Dispose();
            return instance;

        }

        [INLINE(256)]
        public static T Unpack<T>(byte[] bytes, Serializers customSerializers, T objectToOverwrite) where T : class {

            var packer = Serializer.SetupDefaultPacker(bytes, customSerializers);
            new GenericSerializer().Unpack(packer, objectToOverwrite);
            customSerializers.Dispose();
            packer.serializers.Dispose();
            packer.Dispose();
            return objectToOverwrite;

        }

        [INLINE(256)]
        public static Packer SetupDefaultPacker(byte[] bytes, Serializers customSerializers) {

            var serializersInternal = Serializer.GetInternalSerializers();
            var serializers = Serializer.GetDefaultSerializers();
            serializers.Add(ref serializersInternal);
            serializers.Add(ref customSerializers);
            serializersInternal.Dispose();
            customSerializers.Dispose();

            SerializerStream stream;
            if (bytes == null) {
                stream = new SerializerStream(Serializer.BUFFER_CAPACITY);
            } else {
                stream = new SerializerStream(bytes);
            }

            return Packer.FromStream(serializers, stream);
            
        }

        [INLINE(256)]
        public static Packer SetupDefaultPacker(Serializers allSerializers, byte[] bytes) {

            SerializerStream stream;
            if (bytes == null) {
                stream = new SerializerStream(Serializer.BUFFER_CAPACITY);
            } else {
                stream = new SerializerStream(bytes);
            }

            return Packer.FromStream(allSerializers, stream);
            
        }

        [INLINE(256)]
        public static unsafe void PackBlittable<T>(Packer packer, T data, int size) where T : unmanaged {

            var pos = packer.GetPositionAndMove(size);
            var buffer = packer.GetBuffer();
            fixed (byte* ptr = buffer) {
                *(T*)(ptr + pos) = data;
            }
            
        }

        [INLINE(256)]
        public static unsafe T UnpackBlittable<T>(Packer packer, int size) where T : unmanaged {

            var pos = packer.GetPositionAndMove(size);
            var buffer = packer.GetBuffer();
            fixed (byte* ptr = buffer) {
                return *(T*)(ptr + pos);
            }

        }
        
        [INLINE(256)]
        public static unsafe void PackSingle(Packer packer, float value) {
            
            var pos = packer.GetPositionAndMove(4);
            var buffer = packer.GetBuffer();
            if (pos % 4 == 0) {
                fixed (byte* ptr = buffer) {
                    *(float*)(ptr + pos) = value;
                }
            } else {
                uint num = *(uint*)(&value);
                buffer[pos] = (byte)num;
                buffer[pos + 1] = (byte)(num >> 8);
                buffer[pos + 2] = (byte)(num >> 16);
                buffer[pos + 3] = (byte)(num >> 24);
            }

        }

        public static unsafe float UnpackSingle(Packer packer) {
            
            var pos = packer.GetPositionAndMove(4);
            var buffer = packer.GetBuffer();
            if (pos % 4 == 0) {
                fixed (byte* ptr = buffer) {
                    return *(float*)(ptr + pos);
                }
            } else {
                uint num = (uint)((int)buffer[pos] | (int)buffer[pos + 1] << 8 | (int)buffer[pos + 2] << 16 | (int)buffer[pos + 3] << 24);
                return *(float*)(&num);
            }
            
        }

        [INLINE(256)]
        public static unsafe void PackDouble(Packer packer, double value) {
            
            var pos = packer.GetPositionAndMove(8);
            var buffer = packer.GetBuffer();
            if (pos % 8 == 0) {
                fixed (byte* ptr = buffer)
                {
                    *(double*)(ptr + pos) = value;
                }
            } else {
                ulong num = (ulong)(*(long*)(&value));
                buffer[pos] = (byte)num;
                buffer[pos + 1] = (byte)(num >> 8);
                buffer[pos + 2] = (byte)(num >> 16);
                buffer[pos + 3] = (byte)(num >> 24);
                buffer[pos + 4] = (byte)(num >> 32);
                buffer[pos + 5] = (byte)(num >> 40);
                buffer[pos + 6] = (byte)(num >> 48);
                buffer[pos + 7] = (byte)(num >> 56);
            }

        }

        [INLINE(256)]
        public static unsafe double UnpackDouble(Packer packer) {
            
            var pos = packer.GetPositionAndMove(8);
            var buffer = packer.GetBuffer();
            if (pos % 8 == 0) {
                fixed (byte* ptr = buffer) {
                    return *(double*)(ptr + pos);
                }
            } else {
                uint num = (uint)((int)buffer[pos] | (int)buffer[pos + 1] << 8 | (int)buffer[pos + 2] << 16 | (int)buffer[pos + 3] << 24);
                ulong num2 = (ulong)((int)buffer[pos + 4] | (int)buffer[pos + 5] << 8 | (int)buffer[pos + 6] << 16 | (int)buffer[pos + 7] << 24) << 32 | (ulong)num;
                return *(double*)(&num2);
            }
            
        }

        [INLINE(256)]
        public static unsafe void PackBlittable<T>(Packer packer, T data) where T : unmanaged {

            Serializer.PackBlittable(packer, data, sizeof(T));

        }

        [INLINE(256)]
        public static unsafe T UnpackBlittable<T>(Packer packer) where T : unmanaged {

            return Serializer.UnpackBlittable<T>(packer, sizeof(T));

        }

        [INLINE(256)]
        public static unsafe void PackBlittableArrayPrimitives<T>(Packer packer, T[] arr) where T : unmanaged {

            Int32Serializer.PackDirect(packer, arr.Length);
            var writeSize = arr.Length * sizeof(T);
            var pos = packer.GetPositionAndMove(writeSize);
            var buffer = packer.GetBuffer();
            System.Buffer.BlockCopy(arr, 0, buffer, pos, writeSize);

        }

        [INLINE(256)]
        public static unsafe T[] UnpackBlittableArrayPrimitives<T>(Packer packer) where T : unmanaged {

            var length = Int32Serializer.UnpackDirect(packer);
            var arr = new T[length];
            var readSize = arr.Length * sizeof(T);
            var pos = packer.GetPositionAndMove(readSize);
            var buffer = packer.GetBuffer();
            System.Buffer.BlockCopy(buffer, pos, arr, 0, readSize);

            return arr;

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
            internal Dictionary<int, System.Type> typeById;
            internal Dictionary<System.Type, int> idByType;

            public static Meta Create() {

                return new Meta() {
                    metaTypeId = 0,
                    meta = PoolDictionary<System.Type, MetaType>.Spawn(8),
                    typeById = PoolDictionary<int, System.Type>.Spawn(8),
                    idByType = PoolDictionary<System.Type, int>.Spawn(8),
                };

            }

            public void Dispose() {

                PoolDictionary<int, System.Type>.Recycle(ref this.typeById);
                PoolDictionary<System.Type, MetaType>.Recycle(ref this.meta);
                PoolDictionary<System.Type, int>.Recycle(ref this.idByType);

            }

            [INLINE(256)]
            public System.Type GetMetaType(int typeId) {

                this.typeById.TryGetValue(typeId, out var type);
                return type;

            }

            [INLINE(256)]
            public bool TryGetValue(System.Type type, out MetaType metaType) {

                if (this.idByType.TryGetValue(type, out var id) == true) {
                    metaType = new MetaType() { id = id, };
                    return true;
                }

                return this.meta.TryGetValue(type, out metaType);

            }

            [INLINE(256)]
            public int Add(System.Type type) {

                if (this.idByType.TryGetValue(type, out var id) == true) {
                    return id;
                }

                var typeId = new MetaType();
                typeId.id = ++this.metaTypeId;
                typeId.type = type.FullName;
                this.meta.Add(type, typeId);
                this.typeById.Add(typeId.id, type);

                return typeId.id;

            }

            [INLINE(256)]
            public void LoadTypes(Dictionary<System.Type, int> serializersMetaCache) {
                foreach (var kv in serializersMetaCache) {
                    this.idByType.Add(kv.Key, kv.Value);
                    this.typeById.Add(kv.Value, kv.Key);
                }
            }

        }

        internal Serializers serializers;
        private SerializerStream stream;
        private Meta meta;

        [INLINE(256)]
        public void SetCapacity(int capacity) {
            this.SetCapacity_INTERNAL(capacity);
        }

        [INLINE(256)]
        public void AddCapacity(int capacity) {
            this.SetCapacity_INTERNAL(this.stream.Position + capacity);
        }

        [INLINE(256)]
        private void SetCapacity_INTERNAL(int capacity) {

            if (capacity < this.stream.Capacity) {
                return;
            }

            this.stream.Capacity = capacity;

        }

        [INLINE(256)]
        public void SetPosition(int pointer) {
            this.stream.Position = pointer;
        }

        [INLINE(256)]
        public int GetPosition() {
            return this.stream.Position;
        }

        [INLINE(256)]
        public byte[] GetBuffer() {
            return this.stream.GetBuffer();
        }

        [INLINE(256)]
        public byte[] GetBufferToWrite(int bytes) {

            this.AddCapacity(bytes);
            return this.stream.GetBuffer();

        }

        [INLINE(256)]
        public int GetPositionAndMove(int bytes) {

            var pos = this.stream.Position;
            this.AddCapacity(bytes);
            this.stream.Position = pos + bytes;
            return pos;

        }

        public static Packer FromStream(Serializers serializers, SerializerStream stream) {

            var packer = new Packer(serializers, stream);
            var packerObject = PackerObjectSerializer.UnpackDirect(packer);
            packer.meta = packerObject.meta;
            packer.stream = new SerializerStream(packerObject.data);

            packer.meta.LoadTypes(serializers.metaCache);

            return packer;

        }

        public Packer(Serializers serializers, SerializerStream stream) {

            this.meta = Meta.Create();
            this.serializers = serializers;
            this.stream = stream;

            this.meta.LoadTypes(serializers.metaCache);

        }

        [INLINE(256)]
        public void Dispose() {

            this.meta.Dispose();

        }

        [INLINE(256)]
        public byte[] ToArray() {

            var obj = new PackerObject();
            obj.meta = this.meta;
            obj.data = this.stream.ToArray();

            byte[] output = null;
            using (var stream = new SerializerStream(this.stream.Capacity)) {

                var packer = new Packer(this.serializers, stream);
                PackerObjectSerializer.PackDirect(packer, obj);

                output = stream.ToArray();

            }

            return output;

        }

        [INLINE(256)]
        public System.Type GetMetaType(int typeId) {

            if (typeId < 0) {

                var valueType = (TypeValue)(-typeId - 1);
                switch (valueType) {

                    case TypeValue.Int16:
                        return typeof(System.Int16);

                    case TypeValue.Int32:
                        return typeof(System.Int32);

                    case TypeValue.Int64:
                        return typeof(System.Int64);

                    case TypeValue.UInt16:
                        return typeof(System.UInt16);

                    case TypeValue.UInt32:
                        return typeof(System.UInt32);

                    case TypeValue.UInt64:
                        return typeof(System.UInt64);

                    case TypeValue.Byte:
                        return typeof(byte);

                    case TypeValue.SByte:
                        return typeof(sbyte);

                    case TypeValue.Float:
                        return typeof(float);

                    case TypeValue.Double:
                        return typeof(double);

                    case TypeValue.Boolean:
                        return typeof(bool);

                    case TypeValue.String:
                        return typeof(string);

                    case TypeValue.Enum:
                        return typeof(System.Enum);

                }

            }

            return this.meta.GetMetaType(typeId);

        }

        [INLINE(256)]
        public int GetMetaTypeId(System.Type type) {

            var isPrimitive = false;
            var pValue = 0;
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

        [INLINE(256)]
        public byte ReadByte() {

            return this.stream.ReadByte();

        }

        [INLINE(256)]
        public void ReadBytes(byte[] output) {

            this.stream.Read(output, 0, output.Length);

        }

        [INLINE(256)]
        public void ReadBytes(byte[] output, int length) {

            this.stream.Read(output, 0, length);

        }

        [INLINE(256)]
        public void WriteByte(byte @byte) {

            this.stream.WriteByte(@byte);

        }

        [INLINE(256)]
        public void WriteBytes(byte[] bytes) {

            this.stream.Write(bytes, 0, bytes.Length);

        }

        [INLINE(256)]
        public void WriteBytes(byte[] bytes, int length) {

            this.stream.Write(bytes, 0, length);

        }

        [INLINE(256)]
        public void PackInternal<T>(T root) {

            if (root == null) {

                this.WriteByte((byte)TypeValue.Null);
                return;

            }

            this.PackInternal(root, typeof(T));

        }

        [INLINE(256)]
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

                    UnityEngine.Debug.LogError("Pack type has failed: " + rootType);
                    return;

                }

                var fields = rootType.GetCachedFields();
                for (var i = 0; i < fields.Length; ++i) {

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

        [INLINE(256)]
        public void PackInternal(object root) {

            if (root == null) {

                this.WriteByte((byte)TypeValue.Null);
                return;

            }

            var rootType = root.GetType();
            this.PackInternal(root, rootType);

        }

        [INLINE(256)]
        public void UnpackInternal<T>(out T obj) {
            obj = this.UnpackInternal<T>();
        }

        [INLINE(256)]
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

                UnityEngine.Debug.LogWarning("Unknown type: " + type);

            }

            return obj;

        }

        [INLINE(256)]
        public object UnpackInternal() {

            return this.UnpackInternal<object>();

        }

    }

}