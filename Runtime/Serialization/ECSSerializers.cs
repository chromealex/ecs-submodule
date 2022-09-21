#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif
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

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.Tick;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(Tick);

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            Int64Serializer.PackDirect(packer, (Tick)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return (Tick)Int64Serializer.UnpackDirect(packer);
            
        }

    }

    public struct ViewIdSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.ViewId;
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

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.RPCId;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(RPCId);

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            Int32Serializer.PackDirect(packer, (RPCId)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return (RPCId)Int32Serializer.UnpackDirect(packer);
            
        }

    }

    public struct NextTickTaskSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.NextTickTask;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(StructComponentsContainer.NextTickTask);

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            var task = (StructComponentsContainer.NextTickTask)obj;
            Int32Serializer.PackDirect(packer, task.entity.id);
            UInt16Serializer.PackDirect(packer, task.entity.generation);
            ByteSerializer.PackDirect(packer, (byte)task.lifetime);
            ByteSerializer.PackDirect(packer, (byte)task.storageType);
            #if FIXED_POINT_MATH
            FPSerializer.PackDirect(packer, task.secondsLifetime);
            #else
            FloatSerializer.PackDirect(packer, task.secondsLifetime);
            #endif
            UnsafeDataSerializer.PackDirect(packer, task.data);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            var task = new StructComponentsContainer.NextTickTask();
            var entityId = Int32Serializer.UnpackDirect(packer);
            var generation = UInt16Serializer.UnpackDirect(packer);
            task.entity = new Entity(entityId, generation);
            task.lifetime = (ComponentLifetime)ByteSerializer.UnpackDirect(packer);
            task.storageType = (StorageType)ByteSerializer.UnpackDirect(packer);
            #if FIXED_POINT_MATH
            task.secondsLifetime = FPSerializer.UnpackDirect(packer);
            #else
            task.secondsLifetime = FloatSerializer.UnpackDirect(packer);
            #endif
            task.data = UnsafeDataSerializer.UnpackDirect(packer);
            return task;
            
        }

    }

    public struct UnsafeDataSerializer : ITypeSerializer {

        [INLINE(256)] public byte GetTypeValue() => (byte)TypeValue.UnsafeData;
        [INLINE(256)] public System.Type GetTypeSerialized() => typeof(UnsafeData);

        [INLINE(256)] public void Pack(Packer packer, object obj) {

            UnsafeDataSerializer.PackDirect(packer, (UnsafeData)obj);
            
        }

        [INLINE(256)] public object Unpack(Packer packer) {

            return UnsafeDataSerializer.UnpackDirect(packer);
            
        }

        [INLINE(256)] public static void PackDirect(Packer packer, UnsafeData data) {
            
            Int32Serializer.PackDirect(packer, data.sizeOf);
            if (data.sizeOf == 0) return;
            
            Int32Serializer.PackDirect(packer, data.alignOf);
            Int32Serializer.PackDirect(packer, data.typeId);
            Int64Serializer.PackDirect(packer, data.data);
            /*var buffer = packer.GetBufferToWrite(data.sizeOf);
            var pos = packer.GetPositionAndMove(data.sizeOf);
            System.Runtime.InteropServices.Marshal.Copy(data.data, buffer, pos, data.sizeOf);
            */

        }

        [INLINE(256)] public static UnsafeData UnpackDirect(Packer packer) {
            
            var data = new UnsafeData();
            data.sizeOf = Int32Serializer.UnpackDirect(packer);
            if (data.sizeOf == 0) return data;
            
            data.alignOf = Int32Serializer.UnpackDirect(packer);
            data.typeId = Int32Serializer.UnpackDirect(packer);
            data.data = Int64Serializer.UnpackDirect(packer);
            /*var buffer = packer.GetBuffer();
            var pos = packer.GetPositionAndMove(data.sizeOf);
            var intPtrV = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.Malloc(data.sizeOf, data.alignOf, Unity.Collections.Allocator.Persistent);//System.Runtime.InteropServices.Marshal.AllocHGlobal(data.sizeOf);
            data.data = (System.IntPtr)intPtrV;
            System.Runtime.InteropServices.Marshal.Copy(buffer, pos, data.data, data.sizeOf);
            */
            return data;

        }

    }

    public static class ECSSerializers {

        public delegate void RegisterSerializerCallback(ref Serializers serializers);
        private static RegisterSerializerCallback registerSerializerCallback;

        public static void RegisterSerializer(RegisterSerializerCallback callback) {

            ECSSerializers.registerSerializerCallback += callback;

        }
        
        public static Serializers GetSerializers() {

            var ser = new Serializers();
            ser.Add(new ViewSerializer());
            ser.Add(new RPCIdSerializer());
            ser.Add(new ViewIdSerializer());
            ser.Add(new TickSerializer());
            ser.Add(new HistoryEventSerializer());
            ser.Add(new BufferArraySerializer());
            ser.Add(new MemoryAllocatorSerializer());
            ser.Add(new GenericIntDictionarySerializer());
            ser.Add(new GenericULongDictionarySerializer());
            ser.Add(new NextTickTaskSerializer());
            ser.Add(new UnsafeDataSerializer());
            
            registerSerializerCallback?.Invoke(ref ser);
            
            return ser;

        }

    }

}
