using System.Collections;
using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

namespace ME.ECS.Serializer {
    
    public struct GenericULongDictionarySerializer : ITypeSerializer, ITypeSerializerInherit {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.GenericULongDictionary;

        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(ME.ECS.Collections.IDictionaryULong);

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            var dict = (ME.ECS.Collections.IDictionaryULong)obj;
            var type = dict.GetType();
            Int32Serializer.PackDirect(packer, dict.Count);

            Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(type.GenericTypeArguments[0]));
            Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(type.GetGenericTypeDefinition()));

            foreach (DictionaryEntry entry in dict) {
                
                packer.PackInternal(entry.Key);
                packer.PackInternal(entry.Value);
                
            }

        }

        [INLINE(256)] public object Unpack(Packer packer) {

            var length = Int32Serializer.UnpackDirect(packer);
            var typeIdValue = Int32Serializer.UnpackDirect(packer);
            var typeValue = packer.GetMetaType(typeIdValue);

            var type = packer.GetMetaType(Int32Serializer.UnpackDirect(packer));
            var t = type.MakeGenericType(typeValue);

            var dict = (ME.ECS.Collections.IDictionaryULong)System.Activator.CreateInstance(t);

            for (int i = 0; i < length; ++i) {
                
                dict.Add(packer.UnpackInternal(), packer.UnpackInternal());
                
            }

            return dict;
        }

    }

    public struct GenericIntDictionarySerializer : ITypeSerializer, ITypeSerializerInherit {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.GenericIntDictionary;

        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(ME.ECS.Collections.IDictionaryInt);

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            var dict = (ME.ECS.Collections.IDictionaryInt)obj;
            var type = dict.GetType();
            Int32Serializer.PackDirect(packer, dict.Count);

            Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(type.GenericTypeArguments[0]));
            Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(type.GetGenericTypeDefinition()));

            foreach (DictionaryEntry entry in dict) {
                
                packer.PackInternal(entry.Key);
                packer.PackInternal(entry.Value);
                
            }

        }

        [INLINE(256)] public object Unpack(Packer packer) {

            var length = Int32Serializer.UnpackDirect(packer);
            var typeIdValue = Int32Serializer.UnpackDirect(packer);
            var typeValue = packer.GetMetaType(typeIdValue);

            var type = packer.GetMetaType(Int32Serializer.UnpackDirect(packer));
            var t = type.MakeGenericType(typeValue);

            var dict = (ME.ECS.Collections.IDictionaryInt)System.Activator.CreateInstance(t);

            for (int i = 0; i < length; ++i) {
                
                dict.Add(packer.UnpackInternal(), packer.UnpackInternal());
                
            }

            return dict;
        }

    }

    public struct ViewSerializer : ITypeSerializer , ITypeSerializerInherit {

        [INLINE(256)] public byte GetTypeValue() {
            return (byte)TypeValue.View;
        }

        [INLINE(256)] public System.Type GetTypeSerialized() {
            return typeof(ME.ECS.Views.IView);
        }

        [INLINE(256)] public static void PackDirect(Packer packer, ME.ECS.Views.IView view) {
            
            var viewId = Worlds.currentWorld.GetModule<ME.ECS.Views.ViewsModule>().GetViewSourceId(view);
            ViewIdSerializer.PackDirect(packer, viewId);
            
        }

        [INLINE(256)] public static ME.ECS.Views.IView UnpackDirect(Packer packer) {

            var viewId = ViewIdSerializer.UnpackDirect(packer);
            return Worlds.currentWorld.GetModule<ME.ECS.Views.ViewsModule>().GetViewSource(viewId);
            
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            ViewSerializer.PackDirect(packer, (ME.ECS.Views.IView)obj);

        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return ViewSerializer.UnpackDirect(packer);

        }

    }

    public struct TickSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => 151;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(Tick);

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            Int64Serializer.PackDirect(packer, (Tick)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return (Tick)Int64Serializer.UnpackDirect(packer);
            
        }

    }

    public struct ViewIdSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => 152;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(ViewId);

        [INLINE(256)] public static void PackDirect(Packer packer, ViewId obj) {

            UInt32Serializer.PackDirect(packer, obj);
            
        }

        [INLINE(256)] public static ViewId UnpackDirect(Packer packer) {

            return UInt32Serializer.UnpackDirect(packer);
            
        }

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            ViewIdSerializer.PackDirect(packer, (ViewId)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return (ViewId)ViewIdSerializer.UnpackDirect(packer);
            
        }

    }

    public struct RPCIdSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => 153;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(RPCId);

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            Int32Serializer.PackDirect(packer, (RPCId)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return (RPCId)Int32Serializer.UnpackDirect(packer);
            
        }

    }

    public static class ECSSerializers {

        public static Serializers GetSerializers() {

            var ser = new Serializers();
            ser.Add(new ViewSerializer());
            ser.Add(new RPCIdSerializer());
            ser.Add(new ViewIdSerializer());
            ser.Add(new TickSerializer());
            ser.Add(new HistoryEventSerializer());
            ser.Add(new BufferArraySerializer());
            ser.Add(new DisposeSentinelSerializer());
            ser.Add(new GenericIntDictionarySerializer());
            ser.Add(new GenericULongDictionarySerializer());
            return ser;

        }

    }

}
