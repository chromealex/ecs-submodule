using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Serializer {

    public struct HistoryEventSerializer : ITypeSerializer {

        public byte GetTypeValue() {
            return (byte)TypeValue.HistoryEvent;
        }

        public System.Type GetTypeSerialized() {
            return typeof(ME.ECS.StatesHistory.HistoryEvent);
        }
        
        public void Pack(Packer packer, object obj) {

            var pack = (ME.ECS.StatesHistory.HistoryEvent)obj;
            Int64Serializer.PackDirect(packer, pack.tick);
            Int32Serializer.PackDirect(packer, pack.order);
            Int32Serializer.PackDirect(packer, pack.rpcId);
            Int32Serializer.PackDirect(packer, pack.localOrder);
            Int32Serializer.PackDirect(packer, pack.objId);
            Int32Serializer.PackDirect(packer, pack.groupId);
            BooleanSerializer.PackDirect(packer, pack.storeInHistory);

            if (pack.parameters != null) {

                packer.WriteByte(1);
                var serializer = new ObjectArraySerializer();
                serializer.Pack(packer, pack.parameters);

            } else {
                
                packer.WriteByte(0);
                
            }

        }

        public object Unpack(Packer packer) {

            var pack = new ME.ECS.StatesHistory.HistoryEvent();
            pack.tick = Int64Serializer.UnpackDirect(packer);
            pack.order = Int32Serializer.UnpackDirect(packer);
            pack.rpcId = Int32Serializer.UnpackDirect(packer);
            pack.localOrder = Int32Serializer.UnpackDirect(packer);
            pack.objId = Int32Serializer.UnpackDirect(packer);
            pack.groupId = Int32Serializer.UnpackDirect(packer);
            pack.storeInHistory = BooleanSerializer.UnpackDirect(packer);

            var hasParams = packer.ReadByte();
            if (hasParams == 1) {

                var serializer = new ObjectArraySerializer();
                pack.parameters = (object[])serializer.Unpack(packer);

            }

            return pack;

        }

    }

}
