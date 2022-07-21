using System.Collections;
using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

namespace ME.ECS.Serializer {

    public struct Int16ArraySerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.Int16Array;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(System.Int16[]);
        [INLINE(256)] public void Pack(Packer packer, object obj) => Serializer.PackBlittableArrayPrimitives(packer, (System.Int16[])obj);
        [INLINE(256)] public object Unpack(Packer packer) => Serializer.UnpackBlittableArrayPrimitives<System.Int16>(packer);
        
    }

    public struct Int32ArraySerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.Int32Array;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(System.Int32[]);
        [INLINE(256)] public void Pack(Packer packer, object obj) => Serializer.PackBlittableArrayPrimitives(packer, (System.Int32[])obj);
        [INLINE(256)] public object Unpack(Packer packer) => Serializer.UnpackBlittableArrayPrimitives<System.Int32>(packer);
        
    }

    public struct Int64ArraySerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.Int64Array;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(System.Int64[]);
        [INLINE(256)] public void Pack(Packer packer, object obj) => Serializer.PackBlittableArrayPrimitives(packer, (System.Int64[])obj);
        [INLINE(256)] public object Unpack(Packer packer) => Serializer.UnpackBlittableArrayPrimitives<System.Int64>(packer);
        
    }

    public struct UInt16ArraySerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.UInt16Array;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(System.UInt16[]);
        [INLINE(256)] public void Pack(Packer packer, object obj) => Serializer.PackBlittableArrayPrimitives(packer, (System.UInt16[])obj);
        [INLINE(256)] public object Unpack(Packer packer) => Serializer.UnpackBlittableArrayPrimitives<System.UInt16>(packer);
        
    }

    public struct UInt32ArraySerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.UInt32Array;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(System.UInt32[]);
        [INLINE(256)] public void Pack(Packer packer, object obj) => Serializer.PackBlittableArrayPrimitives(packer, (System.UInt32[])obj);
        [INLINE(256)] public object Unpack(Packer packer) => Serializer.UnpackBlittableArrayPrimitives<System.UInt32>(packer);
        
    }

    public struct UInt64ArraySerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.UInt64Array;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(System.UInt64[]);
        [INLINE(256)] public void Pack(Packer packer, object obj) => Serializer.PackBlittableArrayPrimitives(packer, (System.UInt64[])obj);
        [INLINE(256)] public object Unpack(Packer packer) => Serializer.UnpackBlittableArrayPrimitives<System.UInt64>(packer);
        
    }

    public struct FloatArraySerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.FloatArray;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(float[]);
        [INLINE(256)] public void Pack(Packer packer, object obj) => Serializer.PackBlittableArrayPrimitives(packer, (float[])obj);
        [INLINE(256)] public object Unpack(Packer packer) => Serializer.UnpackBlittableArrayPrimitives<float>(packer);
        
    }

    public struct DoubleArraySerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.DoubleArray;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(double[]);
        [INLINE(256)] public void Pack(Packer packer, object obj) => Serializer.PackBlittableArrayPrimitives(packer, (double[])obj);
        [INLINE(256)] public object Unpack(Packer packer) => Serializer.UnpackBlittableArrayPrimitives<double>(packer);
        
    }

    public struct SByteArraySerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.SByteArray;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(sbyte[]);
        [INLINE(256)] public void Pack(Packer packer, object obj) => Serializer.PackBlittableArrayPrimitives(packer, (sbyte[])obj);
        [INLINE(256)] public object Unpack(Packer packer) => Serializer.UnpackBlittableArrayPrimitives<sbyte>(packer);
        
    }

    public struct GenericListSerializer : ITypeSerializer, ITypeSerializerInherit {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.GenericList;

        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(IList);

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            var arr = (IList)obj;
            var type = arr.GetType();
            Int32Serializer.PackDirect(packer, arr.Count);

            if (type.IsArray == true) {

                var rank = type.GetArrayRank();
                if (rank > 1) {
                
                    packer.WriteByte(2);
                    
                } else {
                    
                    packer.WriteByte(1);
                    
                }

                Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(type.GetElementType()));

                if (rank > 1) {

                    Int32Serializer.PackDirect(packer, rank);
                    var arrData = (System.Array)arr;
                    
                    for (int j = 0; j < rank; ++j) {
                        Int32Serializer.PackDirect(packer, arrData.GetLength(j));
                    }

                    void WrapDimension(int[] ids, int currentDimension) {
                        if (currentDimension == rank) {
                            packer.PackInternal(arrData.GetValue(ids));
                        } else {
                            for (int i = 0, length = arrData.GetLength(currentDimension); i < length; i++) {
                                ids[currentDimension] = i;
                                WrapDimension(ids, currentDimension + 1);
                            }
                        }
                    }

                    WrapDimension(new int[rank], 0);
                    
                } else {

                    for (int i = 0; i < arr.Count; ++i) {

                        packer.PackInternal(arr[i]);

                    }

                }

            } else {

                packer.WriteByte(0);
                Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(arr.GetType().GenericTypeArguments[0]));
                Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(arr.GetType().GetGenericTypeDefinition()));

                for (int i = 0; i < arr.Count; ++i) {

                    packer.PackInternal(arr[i]);

                }

            }

        }

        [INLINE(256)] public object Unpack(Packer packer) {

            var length  = Int32Serializer.UnpackDirect(packer);
            var isArray = packer.ReadByte();
            var typeId  = Int32Serializer.UnpackDirect(packer);
            var typeIn  = packer.GetMetaType(typeId);

            IList arr = null;
            if (isArray == 2) {
                
                var rank = Int32Serializer.UnpackDirect(packer);
                if (rank > 1) {
                    var lengthArray = new int[rank];
                    for (int j = 0; j < rank; ++j) {
                        lengthArray[j] = Int32Serializer.UnpackDirect(packer);
                    }
                    
                    var arrData = System.Array.CreateInstance(typeIn, lengthArray);
                    arr = arrData;

                    void WrapDimension(int[] ids, int currentDimension) {
                        if (currentDimension == rank) {
                            arrData.SetValue(packer.UnpackInternal(), ids);
                        } else {
                            for (int i = 0, len = arrData.GetLength(currentDimension); i < len; i++) {
                                ids[currentDimension] = i;
                                WrapDimension(ids, currentDimension + 1);
                            }
                        }
                    }

                    WrapDimension(new int[rank], 0);

                }
                
            } else if (isArray == 1) {

                arr = System.Array.CreateInstance(typeIn, length);
                for (int i = 0; i < length; ++i) {

                    arr[i] = packer.UnpackInternal();

                }

            } else {

                var type  = packer.GetMetaType(Int32Serializer.UnpackDirect(packer));
                var t    = type.MakeGenericType(typeIn);

                arr = (IList)System.Activator.CreateInstance(t);

                for (int i = 0; i < length; ++i) {

                    arr.Add(packer.UnpackInternal());

                }

            }

            return arr;

        }

    }

    public struct GenericDictionarySerializer : ITypeSerializer, ITypeSerializerInherit {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.GenericDictionary;

        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(IDictionary);

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            var dict = (IDictionary)obj;
            var type = dict.GetType();
            Int32Serializer.PackDirect(packer, dict.Count);

            Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(type.GenericTypeArguments[0]));
            Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(type.GenericTypeArguments[1]));
            Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(type.GetGenericTypeDefinition()));

            foreach (DictionaryEntry entry in dict) {
                
                packer.PackInternal(entry.Key);
                packer.PackInternal(entry.Value);
                
            }

        }

        [INLINE(256)] public object Unpack(Packer packer) {

            var length = Int32Serializer.UnpackDirect(packer);
            var typeIdKey = Int32Serializer.UnpackDirect(packer);
            var typeIdValue = Int32Serializer.UnpackDirect(packer);
            var typeKey = packer.GetMetaType(typeIdKey);
            var typeValue = packer.GetMetaType(typeIdValue);

            var type = packer.GetMetaType(Int32Serializer.UnpackDirect(packer));
            var t = type.MakeGenericType(typeKey, typeValue);

            var dict = (IDictionary)System.Activator.CreateInstance(t);

            for (int i = 0; i < length; ++i) {
                
                dict.Add(packer.UnpackInternal(), packer.UnpackInternal());
                
            }

            return dict;
        }

    }

    public struct ObjectArraySerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() {
            return (byte)TypeValue.ObjectArray;
        }

        [INLINE(256)] public System.Type GetTypeSerialized() {
            return typeof(object[]);
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            var arr = (System.Array)obj;

            Int32Serializer.PackDirect(packer, arr.Length);
            for (int i = 0; i < arr.Length; ++i) {

                packer.PackInternal(arr.GetValue(i));

            }
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            var length = Int32Serializer.UnpackDirect(packer);

            var arr = new object[length];
            for (int i = 0; i < length; ++i) {

                arr[i] = packer.UnpackInternal();

            }

            return arr;

        }


    }

    public struct ByteArraySerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() {
            return (byte)TypeValue.ByteArray;
        }

        [INLINE(256)] public System.Type GetTypeSerialized() {
            return typeof(byte[]);
        }

        [INLINE(256)] public static void PackDirect(Packer packer, byte[] arr) {
            
            Int32Serializer.PackDirect(packer, arr.Length);
            packer.WriteBytes(arr);

        }

        [INLINE(256)] public static byte[] UnpackDirect(Packer packer) {
            
            var length = Int32Serializer.UnpackDirect(packer);
            var arr = new byte[length];
            packer.ReadBytes(arr);
            return arr;

        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            ByteArraySerializer.PackDirect(packer, (byte[])obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return ByteArraySerializer.UnpackDirect(packer);

        }

    }

}