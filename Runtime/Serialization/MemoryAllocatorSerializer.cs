using ME.ECS.Collections;
using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

namespace ME.ECS.Serializer {

    public unsafe struct MemoryAllocatorSerializer : ITypeSerializer, ITypeSerializerInherit {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.MemoryAllocator;

        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(ME.ECS.Collections.V3.MemoryAllocator);

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            var allocator = (ME.ECS.Collections.V3.MemoryAllocator)obj;
            Int64Serializer.PackDirect(packer, allocator.maxSize);
            var size = allocator.zone->size;
            Int32Serializer.PackDirect(packer, size);
            
            if (size == 0) return;
            
            var writeSize = size;
            var pos = packer.GetPositionAndMove(writeSize);
            var buffer = packer.GetBuffer();
            System.Runtime.InteropServices.Marshal.Copy((System.IntPtr)allocator.zone, buffer, pos, writeSize);

        }

        [INLINE(256)] public object Unpack(Packer packer) {

            var allocator = new ME.ECS.Collections.V3.MemoryAllocator();
            allocator.maxSize = Int64Serializer.UnpackDirect(packer);
            
            var length = Int32Serializer.UnpackDirect(packer);
            if (length == 0) return allocator;
            
            allocator.zone = ME.ECS.Collections.V3.MemoryAllocator.ZmCreateZone(length);
            var readSize = length;
            var pos = packer.GetPositionAndMove(readSize);
            var buffer = packer.GetBuffer();
            System.Runtime.InteropServices.Marshal.Copy(buffer, pos, (System.IntPtr)allocator.zone, readSize);

            return allocator;

        }

    }

}
