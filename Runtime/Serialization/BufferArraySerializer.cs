using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

namespace ME.ECS.Serializer {

    public struct BufferArraySerializer : ITypeSerializer, ITypeSerializerInherit {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.BufferArray;

        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(ME.ECS.Collections.IBufferArray);

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            System.Type arrayType = obj.GetType();
            if (arrayType.IsGenericType == true) {

                arrayType = arrayType.GetGenericTypeDefinition();

            }

            var buffer = (ME.ECS.Collections.IBufferArray)obj;
            var arr = buffer.GetArray();
            if (arr == null) {
                
                packer.WriteByte((byte)TypeValue.Null);
                Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(arrayType));
                Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(obj.GetType().GenericTypeArguments[0]));
                
            } else {

                packer.WriteByte((byte)TypeValue.ObjectArray);

                var length = buffer.Count;

                Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(arrayType));
                Int32Serializer.PackDirect(packer, length);
                Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(arr.GetType().GetElementType()));
                for (var i = 0; i < length; ++i) {

                    packer.PackInternal(arr.GetValue(i));

                }

            }

        }

        [INLINE(256)] public object Unpack(Packer packer) {

            int arrayTypeId = -1;
            int typeId = -1;
            object p1 = null;
            object p2 = null;
            
            var type = packer.ReadByte();
            if (type == (byte)TypeValue.Null) {

                arrayTypeId = Int32Serializer.UnpackDirect(packer);
                typeId = Int32Serializer.UnpackDirect(packer);
                p1 = null;
                p2 = 0;

            } else {

                arrayTypeId = Int32Serializer.UnpackDirect(packer);
                var length = Int32Serializer.UnpackDirect(packer);
                typeId = Int32Serializer.UnpackDirect(packer);
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
                                                                                            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic,
                                                                                            null, new object[] {
                                                                                                p1, p2, -1,
                                                                                            }, System.Globalization.CultureInfo.InvariantCulture);

            return instance;

        }

    }

}