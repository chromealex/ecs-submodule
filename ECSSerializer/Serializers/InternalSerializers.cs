using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Serializer {

    public struct MetaTypeSerializer : ITypeSerializer {

        public byte GetTypeValue() {
            return (byte)TypeValue.MetaType;
        }

        public System.Type GetTypeSerialized() {
            return typeof(Packer.MetaType);
        }

        public static void PackDirect(Packer packer, Packer.MetaType meta) {
            
            Int32Serializer.PackDirect(packer, meta.id);
            StringSerializer.PackDirect(packer, meta.type);
            
        }

        public static Packer.MetaType UnpackDirect(Packer packer) {
            
            var meta = new Packer.MetaType();
            meta.id = Int32Serializer.UnpackDirect(packer);
            meta.type = StringSerializer.UnpackDirect(packer);
            return meta;

        }

        public void Pack(Packer packer, object obj) {

            var meta = (Packer.MetaType)obj;
            MetaTypeSerializer.PackDirect(packer, meta);

        }

        public object Unpack(Packer packer) {

            return MetaTypeSerializer.UnpackDirect(packer);

        }

    }

    public struct MetaTypeArraySerializer : ITypeSerializer {

        public byte GetTypeValue() {
            return (byte)TypeValue.MetaTypeArray;
        }

        public System.Type GetTypeSerialized() {
            return typeof(Packer.MetaType[]);
        }

        public static void PackDirect(Packer packer, Packer.MetaType[] meta) {
            
            Int32Serializer.PackDirect(packer, meta.Length);
            for (int i = 0; i < meta.Length; ++i) {

                MetaTypeSerializer.PackDirect(packer, meta[i]);

            }
            
        }

        public static Packer.MetaType[] UnpackDirect(Packer packer) {
            
            var length = Int32Serializer.UnpackDirect(packer);
            var meta = new Packer.MetaType[length];
            for (int i = 0; i < length; ++i) {

                meta[i] = MetaTypeSerializer.UnpackDirect(packer);

            }

            return meta;

        }

        public void Pack(Packer packer, object obj) {

            var meta = (Packer.MetaType[])obj;
            MetaTypeArraySerializer.PackDirect(packer, meta);

        }

        public object Unpack(Packer packer) {

            return MetaTypeArraySerializer.UnpackDirect(packer);
            
        }

    }

    public struct MetaSerializer : ITypeSerializer {

        public byte GetTypeValue() {
            return (byte)TypeValue.Meta;
        }

        public System.Type GetTypeSerialized() {
            return typeof(Packer.Meta);
        }
        
        public static void PackDirect(Packer packer, Packer.Meta meta) {

            var arr = new Packer.MetaType[meta.meta.Count];
            var i = 0;
            foreach (var kv in meta.meta) {

                arr[i++] = kv.Value;

            }

            MetaTypeArraySerializer.PackDirect(packer, arr);

        }
        
        public static Packer.Meta UnpackDirect(Packer packer) {

            var meta = new Packer.Meta();
            meta.metaTypeId = 0;
            meta.meta = new Dictionary<System.Type, Packer.MetaType>();

            var asms = System.AppDomain.CurrentDomain.GetAssemblies();
            var arr = MetaTypeArraySerializer.UnpackDirect(packer);
            for (int i = 0; i < arr.Length; ++i) {

                var data = arr[i];
                for (int j = 0; j < asms.Length; ++j) {

                    var type = asms[j].GetType(data.type);
                    if (type != null) {

                        meta.meta.Add(type, data);
                        break;

                    }

                }

            }

            return meta;

        }

        public void Pack(Packer packer, object obj) {

            var meta = (Packer.Meta)obj;
            MetaSerializer.PackDirect(packer, meta);

        }

        public object Unpack(Packer packer) {

            return MetaSerializer.UnpackDirect(packer);

        }

    }

    public struct PackerObjectSerializer : ITypeSerializer {

        public byte GetTypeValue() {
            return (byte)TypeValue.PackerObject;
        }

        public System.Type GetTypeSerialized() {
            return typeof(Packer.PackerObject);
        }

        public static void PackDirect(Packer packer, Packer.PackerObject packerObject) {
            
            ByteArraySerializer.PackDirect(packer, packerObject.data);
            MetaSerializer.PackDirect(packer, packerObject.meta);
            
        }

        public static Packer.PackerObject UnpackDirect(Packer packer) {
            
            var packerObject = new Packer.PackerObject();
            packerObject.data = ByteArraySerializer.UnpackDirect(packer);
            packerObject.meta = MetaSerializer.UnpackDirect(packer);

            return packerObject;

        }

        public void Pack(Packer packer, object obj) {

            var packerObject = (Packer.PackerObject)obj;
            PackerObjectSerializer.PackDirect(packer, packerObject);

        }

        public object Unpack(Packer packer) {

            return PackerObjectSerializer.UnpackDirect(packer);

        }

    }

}
