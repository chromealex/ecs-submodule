namespace ME.ECS.Serializer {

    public struct BufferArraySerializer : ITypeSerializer, ITypeSerializerInherit {

        public byte GetTypeValue() => (byte)TypeValue.BufferArray;

        public System.Type GetTypeSerialized() => typeof(ME.ECS.Collections.IBufferArray);

        public void Pack(Packer packer, object obj) {

            System.Type arrayType = obj.GetType();
            if (arrayType.IsGenericType == true) {

                arrayType = arrayType.GetGenericTypeDefinition();

            }

            var buffer = (ME.ECS.Collections.IBufferArray)obj;
            var arr = buffer.GetArray();
            if (arr == null) {
                
                packer.WriteByte((byte)TypeValue.Null);
                var int32 = new Int32Serializer();
                int32.Pack(packer, packer.GetMetaTypeId(arrayType));
                int32.Pack(packer, packer.GetMetaTypeId(obj.GetType().GenericTypeArguments[0]));
                
            } else {

                packer.WriteByte((byte)TypeValue.ObjectArray);

                var length = buffer.Count;

                var int32 = new Int32Serializer();
                int32.Pack(packer, packer.GetMetaTypeId(arrayType));
                int32.Pack(packer, length);
                int32.Pack(packer, packer.GetMetaTypeId(arr.GetType().GetElementType()));
                for (var i = 0; i < length; ++i) {

                    packer.PackInternal(arr.GetValue(i));

                }

            }

        }

        public object Unpack(Packer packer) {

            int arrayTypeId = -1;
            int typeId = -1;
            object p1 = null;
            object p2 = null;
            
            var type = packer.ReadByte();
            if (type == (byte)TypeValue.Null) {

                var int32 = new Int32Serializer();
                arrayTypeId = (int)int32.Unpack(packer);
                typeId = (int)int32.Unpack(packer);
                p1 = null;
                p2 = 0;

            } else {

                var int32 = new Int32Serializer();
                arrayTypeId = (int)int32.Unpack(packer);
                var length = (int)int32.Unpack(packer);
                typeId = (int)int32.Unpack(packer);
                var elementType = packer.GetMetaType(typeId);

                var poolArrayType = typeof(PoolArray<>).MakeGenericType(elementType);
                var spawnMethod = poolArrayType.GetMethod("Spawn", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                var bufferArray = (ME.ECS.Collections.IBufferArray)spawnMethod.Invoke(null, new object[] { length, false });
                var arr = bufferArray.GetArray();
                for (var i = 0; i < length; ++i) {

                    arr.SetValue(packer.UnpackInternal(), i);
                    
                }

                p1 = arr;
                p2 = length;

            }

            var arrayType = packer.GetMetaType(arrayTypeId);
            var constructedType = arrayType.MakeGenericType(packer.GetMetaType(typeId));
            var instance = (ME.ECS.Collections.IBufferArray)System.Activator.CreateInstance(constructedType,
                                                                                            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                                                                                            null, new object[] {
                                                                                                p1, p2, -1,
                                                                                            }, System.Globalization.CultureInfo.InvariantCulture);

            return instance;

        }

    }

}