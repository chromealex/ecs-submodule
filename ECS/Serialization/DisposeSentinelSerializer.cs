using ME.ECS.Collections;

namespace ME.ECS.Serializer {

    public struct DisposeSentinelSerializer : ITypeSerializer, ITypeSerializerInherit {

        public byte GetTypeValue() => (byte)TypeValue.DisposeSentinel;

        public System.Type GetTypeSerialized() => typeof(IDisposeSentinel);

        public void Pack(Packer packer, object obj) {

            var sentinel = (IDisposeSentinel)obj;
            var type = sentinel.GetType();

            Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(type.GetGenericTypeDefinition()));
            Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(type.GenericTypeArguments[0]));
            Int32Serializer.PackDirect(packer, packer.GetMetaTypeId(type.GenericTypeArguments[1]));

            packer.PackInternal(sentinel.GetData());
            Int64Serializer.PackDirect(packer, sentinel.GetTick());
            
        }

        public object Unpack(Packer packer) {

            var type = packer.GetMetaType(Int32Serializer.UnpackDirect(packer));
            var typeValue = packer.GetMetaType(Int32Serializer.UnpackDirect(packer));
            var typeProvider = packer.GetMetaType(Int32Serializer.UnpackDirect(packer));

            var sentinelType = type.MakeGenericType(typeValue, typeProvider);

            var poolClassType = typeof(PoolClass<>).MakeGenericType(sentinelType);
            var spawnMethod = poolClassType.GetMethod("Spawn", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var sentinel = (IDisposeSentinel)spawnMethod.Invoke(null, null);
            sentinel.SetData(packer.UnpackInternal());
            sentinel.SetTick(Int64Serializer.UnpackDirect(packer));

            return sentinel;

        }

    }

}
