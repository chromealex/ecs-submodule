using ME.ECS.Collections;
using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

namespace ME.ECS.Serializer {
    
    using System.Runtime.InteropServices;
    
    using ME.ECS.Collections.V3;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;

    public unsafe struct MemoryAllocatorSerializer : ITypeSerializer, ITypeSerializerInherit {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.MemoryAllocator;

        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(ME.ECS.Collections.V3.MemoryAllocator);

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            var allocator = (MemoryAllocator)obj;
            Int64Serializer.PackDirect(packer, allocator.maxSize);
            Int32Serializer.PackDirect(packer, allocator.zonesListCount);

            for (int i = 0; i < allocator.zonesListCount; ++i) {
                var zone = allocator.zonesList[i];

                Int32Serializer.PackDirect(packer, zone->size);
            
                if (zone->size == 0) continue;
            
                var writeSize = zone->size;
                var pos = packer.GetPositionAndMove(writeSize);
                var buffer = packer.GetBuffer();
                Marshal.Copy((System.IntPtr)zone, buffer, pos, writeSize);
            }

        }

        [INLINE(256)] public object Unpack(Packer packer) {

            var allocator = new MemoryAllocator();
            allocator.maxSize = Int64Serializer.UnpackDirect(packer);
            allocator.zonesListCount = Int32Serializer.UnpackDirect(packer);
            allocator.zonesListCapacity = allocator.zonesListCount;
            allocator.zonesList = (MemoryAllocator.MemZone**)UnsafeUtility.Malloc(allocator.zonesListCount * sizeof(MemoryAllocator.MemZone*), UnsafeUtility.AlignOf<byte>(), Allocator.Persistent);

            for (int i = 0; i < allocator.zonesListCount; ++i) {
                
                var length = Int32Serializer.UnpackDirect(packer);
                if (length == 0) continue;

                var zone = MemoryAllocator.ZmCreateZone(length);
            
                allocator.zonesList[i] = zone;
                var readSize = length;
                var pos = packer.GetPositionAndMove(readSize);
                var buffer = packer.GetBuffer();
                Marshal.Copy(buffer, pos, (System.IntPtr)zone, readSize);
                
            }

            return allocator;

        }

    }

}
