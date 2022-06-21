using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

namespace ME.ECS.Serializer {

    public struct GenericSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() { return (byte)TypeValue.Generic; }
        [INLINE(256)] public System.Type GetTypeSerialized() { return typeof(GenericSerializer); }

        [INLINE(256)] public static void PackDirect(Packer packer, object obj) {
            new GenericSerializer().Pack(packer, obj);
        }

        [INLINE(256)] public static object UnpackDirect(Packer packer) {
            return new GenericSerializer().Unpack(packer);
        }

        [INLINE(256)] public void Pack(Packer packer, object obj, System.Type rootType) {
            
            var fields = rootType.GetCachedFields();
            for (int i = 0; i < fields.Length; ++i) {

                var val = fields[i].GetValue(obj);
                if (val == null) {
                    
                    packer.WriteByte((byte)TypeValue.Null);
                    continue;
                    
                }
                
                var type = fields[i].GetFieldType();
                if (packer.serializers.TryGetValue(type, out var ser) == true) {

                    packer.WriteByte(ser.typeValue);
                    ser.pack.Invoke(packer, val);

                } else {

                    packer.PackInternal(val);

                }

            }

        }
        
        [INLINE(256)] public void Pack(Packer packer, object obj) {

            var rootType = obj.GetType();
            var typeId = packer.GetMetaTypeId(rootType);
            Int32Serializer.PackDirect(packer, typeId);
            
            this.Pack(packer, obj, rootType);
            
        }

        [INLINE(256)] public object Unpack(Packer packer, System.Type rootType) {
            
            var instance = System.Activator.CreateInstance(rootType);
            var fields = rootType.GetCachedFields();
            for (int i = 0; i < fields.Length; ++i) {

                var type = packer.ReadByte();
                if (type == (byte)TypeValue.Null) {
                    
                    fields[i].SetValue(instance, null);
                    continue;
                    
                }
                
                if (packer.serializers.TryGetValue(type, out var ser) == true) {

                    fields[i].SetValue(instance, ser.unpack.Invoke(packer));

                } else {

                    packer.UnpackInternal();

                }

            }
            
            return instance;

        }
        
        [INLINE(256)] public void Unpack<T>(Packer packer, T objectToOverwrite) where T : class {

            var fields   = typeof(T).GetCachedFields();
            for (int i = 0; i < fields.Length; ++i) {

                var type = packer.ReadByte();
                if (type == (byte)TypeValue.Null) {
                    
                    fields[i].SetValue(objectToOverwrite, null);
                    continue;
                    
                }

                if (packer.serializers.TryGetValue(type, out var ser) == true) {

                    fields[i].SetValue(objectToOverwrite, ser.unpack.Invoke(packer));

                } else {

                    packer.UnpackInternal();

                }

            }
        }
        
        [INLINE(256)] public object Unpack(Packer packer) {

            var typeId = Int32Serializer.UnpackDirect(packer);
            var rootType = packer.GetMetaType(typeId);

            return this.Unpack(packer, rootType);

        }

    }
}
