using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Serializer {

    public struct ViewSerializer : ITypeSerializer , ITypeSerializerInherit {

        public byte GetTypeValue() {
            return (byte)TypeValue.View;
        }

        public System.Type GetTypeSerialized() {
            return typeof(ME.ECS.Views.IView);
        }

        public static void PackDirect(Packer packer, ME.ECS.Views.IView view) {
            
            var viewId = Worlds.currentWorld.GetModule<ME.ECS.Views.ViewsModule>().GetViewSourceId(view);
            ViewIdSerializer.PackDirect(packer, viewId);
            
        }

        public static ME.ECS.Views.IView UnpackDirect(Packer packer) {

            var viewId = ViewIdSerializer.UnpackDirect(packer);
            return Worlds.currentWorld.GetModule<ME.ECS.Views.ViewsModule>().GetViewSource(viewId);
            
        }

        public void Pack(Packer packer, object obj) {

            ViewSerializer.PackDirect(packer, (ME.ECS.Views.IView)obj);

        }

        public object Unpack(Packer packer) {

            return ViewSerializer.UnpackDirect(packer);

        }

    }

    public struct TickSerializer : ITypeSerializer {

        public byte GetTypeValue() => 151;
        public System.Type GetTypeSerialized() => typeof(Tick);

        public void Pack(Packer packer, object obj) {

            Int64Serializer.PackDirect(packer, (Tick)obj);
            
        }

        public object Unpack(Packer packer) {

            return (Tick)Int64Serializer.UnpackDirect(packer);
            
        }

    }

    public struct ViewIdSerializer : ITypeSerializer {

        public byte GetTypeValue() => 152;
        public System.Type GetTypeSerialized() => typeof(ViewId);

        public static void PackDirect(Packer packer, ViewId obj) {

            UInt32Serializer.PackDirect(packer, obj);
            
        }

        public static ViewId UnpackDirect(Packer packer) {

            return UInt32Serializer.UnpackDirect(packer);
            
        }

        public void Pack(Packer packer, object obj) {

            ViewIdSerializer.PackDirect(packer, (ViewId)obj);
            
        }

        public object Unpack(Packer packer) {

            return (ViewId)ViewIdSerializer.UnpackDirect(packer);
            
        }

    }

    public struct RPCIdSerializer : ITypeSerializer {

        public byte GetTypeValue() => 153;
        public System.Type GetTypeSerialized() => typeof(RPCId);

        public void Pack(Packer packer, object obj) {

            Int32Serializer.PackDirect(packer, (RPCId)obj);
            
        }

        public object Unpack(Packer packer) {

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
            return ser;

        }

    }

}
