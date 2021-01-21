using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Serializer {

    public struct TickSerializer : ITypeSerializer {

        public byte GetTypeValue() => 151;
        public System.Type GetTypeSerialized() => typeof(Tick);

        public void Pack(Packer packer, object obj) {

            Int64Serializer.PackDirect(packer, (uint)(Tick)obj);
            
        }

        public object Unpack(Packer packer) {

            return Int64Serializer.UnpackDirect(packer);
            
        }

    }

    public struct ViewIdSerializer : ITypeSerializer {

        public byte GetTypeValue() => 152;
        public System.Type GetTypeSerialized() => typeof(ViewId);

        public void Pack(Packer packer, object obj) {

            UInt32Serializer.PackDirect(packer, (uint)(ViewId)obj);
            
        }

        public object Unpack(Packer packer) {

            return UInt32Serializer.UnpackDirect(packer);
            
        }

    }

    public struct RPCIdSerializer : ITypeSerializer {

        public byte GetTypeValue() => 153;
        public System.Type GetTypeSerialized() => typeof(RPCId);

        public void Pack(Packer packer, object obj) {

            Int32Serializer.PackDirect(packer, (int)(RPCId)obj);
            
        }

        public object Unpack(Packer packer) {

            return Int32Serializer.UnpackDirect(packer);
            
        }

    }

    public static class ECSSerializers {

        public static Serializers GetSerializers() {

            var ser = new Serializers();
            ser.Add(new RPCIdSerializer());
            ser.Add(new ViewIdSerializer());
            ser.Add(new TickSerializer());
            ser.Add(new HistoryEventSerializer());
            return ser;

        }

    }

}
