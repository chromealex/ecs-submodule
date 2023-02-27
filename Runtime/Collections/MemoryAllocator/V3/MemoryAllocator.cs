//#define MEMORY_ALLOCATOR_BOUNDS_CHECK
//#define LOGS_ENABLED

using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;
using BURST = Unity.Burst.BurstCompileAttribute;

namespace ME.ECS.Collections.V3 {

    using MemPtr = System.Int64;
    
    public struct TSize<T> where T : struct {

        public static readonly int size = UnsafeUtility.SizeOf<T>();

    }

    public struct TAlign<T> where T : struct {

        public static readonly int align = UnsafeUtility.AlignOf<T>();

    }

    #if !LOGS_ENABLED && !MEMORY_ALLOCATOR_BOUNDS_CHECK
    //[BURST(CompileSynchronously = true)]
    #endif
    public static unsafe class MemoryAllocatorExt {

        #if !LOGS_ENABLED && !MEMORY_ALLOCATOR_BOUNDS_CHECK
        //[BURST(CompileSynchronously = true)]
        #endif
        public static MemPtr Alloc(this ref MemoryAllocator allocator, long size) {

            void* ptr = null;

            for (int i = 0; i < allocator.zonesListCount; i++) {
                var zone = allocator.zonesList[i];
                
                if (zone == null) continue;
                
                ptr = MemoryAllocator.ZmMalloc(zone, (int)size);

                if (ptr != null) {
                    var memPtr = allocator.GetSafePtr(ptr, i);
                    #if LOGS_ENABLED
                    MemoryAllocator.LogAdd(memPtr, size);
                    #endif
                    return memPtr;
                }
            }

            {
                var zone = MemoryAllocator.ZmCreateZone((int)Math.Max(size, MemoryAllocator.MIN_ZONE_SIZE));
                var zoneIndex = allocator.AddZone(zone);

                ptr = MemoryAllocator.ZmMalloc(zone, (int)size);

                var memPtr = allocator.GetSafePtr(ptr, zoneIndex);
                #if LOGS_ENABLED
                MemoryAllocator.LogAdd(memPtr, size);
                #endif
                return memPtr;
            }

        }

        #if !LOGS_ENABLED && !MEMORY_ALLOCATOR_BOUNDS_CHECK
        //[BURST(CompileSynchronously = true)]
        #endif
        public static bool Free(this ref MemoryAllocator allocator, MemPtr ptr) {

            if (ptr == 0) return false;
            
            var zoneIndex = ptr >> 32;
            
            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            if (zoneIndex >= allocator.zonesListCount || allocator.zonesList[zoneIndex] == null || allocator.zonesList[zoneIndex]->size < (ptr & MemoryAllocator.OFFSET_MASK)) {
                throw new OutOfBoundsException();
            }
            #endif
            
            var zone = allocator.zonesList[zoneIndex];

            #if LOGS_ENABLED
            if (startLog == true) {
                MemoryAllocator.LogRemove(ptr);
            }
            #endif

            var success = false;

            if (zone != null) {
                success = MemoryAllocator.ZmFree(zone, allocator.GetUnsafePtr(ptr));

                if (MemoryAllocator.IsEmptyZone(zone) == true) {
                    MemoryAllocator.ZmFreeZone(zone);
                    allocator.zonesList[zoneIndex] = null;
                }
            }

            return success;
        }

    }

    public struct AllocatorContext : IDisposable {

        public static readonly Unity.Burst.SharedStatic<MemoryAllocator> burstAllocator = Unity.Burst.SharedStatic<MemoryAllocator>.GetOrCreate<AllocatorContext, MemoryAllocator>();
        
        public MemoryAllocator allocator;

        public AllocatorContext Create() {
            
            AllocatorContext.burstAllocator.Data = this.allocator;
            return this;

        }

        public void Dispose() {

            AllocatorContext.burstAllocator.Data = default;

        }

    }

    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    public unsafe partial struct MemoryAllocator : IDisposable {

        #if LOGS_ENABLED && UNITY_EDITOR
        [Unity.Burst.BurstDiscardAttribute]
        public static void LogAdd(MemPtr memPtr, long size) {
            if (startLog == true) {
                var str = "ALLOC: " + memPtr + ", SIZE: " + size;
                strList.Add(memPtr, str + "\n" + UnityEngine.StackTraceUtility.ExtractStackTrace());
            }
        }

        [Unity.Burst.BurstDiscardAttribute]
        public static void LogRemove(MemPtr memPtr) {
            strList.Remove(memPtr);
        }

        public static bool startLog;
        public static System.Collections.Generic.Dictionary<MemPtr, string> strList = new System.Collections.Generic.Dictionary<MemPtr, string>();
        [UnityEditor.MenuItem("ME.ECS/Debug/Allocator: Start Log")]
        public static void StartLog() {
            startLog = true;
        }
        
        [UnityEditor.MenuItem("ME.ECS/Debug/Allocator: End Log")]
        public static void EndLog() {
            startLog = false;
            MemoryAllocator.strList.Clear();
        }
        
        [UnityEditor.MenuItem("ME.ECS/Debug/Allocator: Print Log")]
        public static void PrintLog() {
            foreach (var item in MemoryAllocator.strList) {
                UnityEngine.Debug.Log(item.Key + "\n" + item.Value);
            }
        }
        #endif

        private const long OFFSET_MASK = 0xFFFFFFFF;
        internal const long MIN_ZONE_SIZE = 512 * 1024;
        private const int MIN_ZONES_LIST_CAPACITY = 20;

        [NativeDisableUnsafePtrRestriction]
        internal MemZone** zonesList;
        internal int zonesListCount;
        internal int zonesListCapacity;
        internal long maxSize;
        
        public bool isValid => this.zonesList != null;

        public static AllocatorContext CreateContext() {

            return new AllocatorContext() {
                allocator = Worlds.current.currentState.allocator,
            }.Create();

        }

        public static AllocatorContext CreateContext(in MemoryAllocator allocator) {

            return new AllocatorContext() {
                allocator = allocator,
            }.Create();

        }

        public int GetReservedSize() {

            var size = 0;
            for (int i = 0; i < this.zonesListCount; i++) {
                var zone = this.zonesList[i];
                if (zone != null) {
                    size += zone->size;
                }
            }

            return size;

        }

        public int GetUsedSize() {

            var size = 0;
            for (int i = 0; i < this.zonesListCount; i++) {
                var zone = this.zonesList[i];
                if (zone != null) {
                    size += zone->size;
                    size -= MemoryAllocator.GetZmFreeMemory(zone);
                }
            }

            return size;

        }

        public int GetFreeSize() {

            var size = 0;
            for (int i = 0; i < this.zonesListCount; i++) {
                var zone = this.zonesList[i];
                if (zone != null) {
                    size += MemoryAllocator.GetZmFreeMemory(zone);
                }
            }

            return size;

        }

        /// 
        /// Constructors
        /// 
        public MemoryAllocator Initialize(long initialSize, long maxSize = -1L) {

            if (maxSize < initialSize) maxSize = initialSize;
            
            this.AddZone(MemoryAllocator.ZmCreateZone((int)Math.Max(initialSize, MemoryAllocator.MIN_ZONE_SIZE)));
            
            this.maxSize = maxSize;

            return this;
        }

        public void Dispose() {

            this.FreeZones();
            
            if (this.zonesList != null) {
                UnsafeUtility.Free(this.zonesList, Allocator.Persistent);
				this.zonesList = null;
			}

            this.zonesListCapacity = 0;
            this.maxSize = default;

        }

        public void CopyFrom(in MemoryAllocator other) {

            if (other.zonesList == null && this.zonesList == null) {
                
            } else if (other.zonesList == null && this.zonesList != null) {
                this.FreeZones();
            } else {
	    
		        var areEquals = true;
                
                if (this.zonesListCount == other.zonesListCount) {

                    for (int i = 0; i < other.zonesListCount; ++i) {
                        ref var curZone = ref this.zonesList[i];
                        var otherZone = other.zonesList[i];
                        {
                            if (curZone == null && otherZone == null) continue;
                            
                            if (curZone == null) {
                                curZone = MemoryAllocator.ZmCreateZone(otherZone->size);
                                UnsafeUtility.MemCpy(curZone, otherZone, otherZone->size);
                            } else if (otherZone == null) {
                                MemoryAllocator.ZmFreeZone(curZone);
                                curZone = null;
                            } else {
                                // resize zone
                                curZone = MemoryAllocator.ZmReallocZone(curZone, otherZone->size);
                                UnsafeUtility.MemCpy(curZone, otherZone, otherZone->size);
                            }
                        }
                    }

                } else {
                    areEquals = false;
                }

                if (areEquals == false) {
		    
		            this.FreeZones();

		            for (int i = 0; i < other.zonesListCount; i++) {
		                var otherZone = other.zonesList[i];

                        if (otherZone != null) {
                            var zone = MemoryAllocator.ZmCreateZone(otherZone->size);
                            UnsafeUtility.MemCpy(zone, otherZone, otherZone->size);
                            this.AddZone(zone);
                        } else {
                            this.AddZone(null);
                        }

                    }
                    
                }

            }
            
            this.maxSize = other.maxSize;
	    
        }

        private void FreeZones() {
            if (this.zonesListCount > 0 && this.zonesList != null) {
                for (int i = 0; i < this.zonesListCount; i++) {
                    var zone = this.zonesList[i];
                    if (zone != null) {
                        MemoryAllocator.ZmFreeZone(zone);
                    }
                }
            }

            this.zonesListCount = 0;
        }

        internal int AddZone(MemZone* zone) {
            
            for (int i = 0; i < this.zonesListCount; i++) {
                if (this.zonesList[i] == null) {
                    this.zonesList[i] = zone;
                    return i;
                }
            }

            if (this.zonesListCapacity <= this.zonesListCount) {
                var capacity = Math.Max(MemoryAllocator.MIN_ZONES_LIST_CAPACITY, this.zonesListCapacity * 2);
                var list = (MemZone**)UnsafeUtility.Malloc(capacity * sizeof(MemZone*), UnsafeUtility.AlignOf<byte>(), Allocator.Persistent);

                if (this.zonesList != null) {
                    for (int i = 0; i < this.zonesListCount; i++) {
                        list[i] = this.zonesList[i];
                    }
                    
                    UnsafeUtility.Free(this.zonesList, Allocator.Persistent);
                }
                
                this.zonesList = list;
                this.zonesListCapacity = capacity;
            }

            this.zonesList[this.zonesListCount++] = zone;

            return this.zonesListCount - 1;
        }

        /// 
        /// Base
        ///
        
        [INLINE(256)]
        public readonly ref T Ref<T>(MemPtr ptr) where T : struct {
            return ref UnsafeUtility.AsRef<T>(this.GetUnsafePtr(ptr));
        }

        [INLINE(256)]
        public MemPtr AllocData<T>(T data) where T : struct {
            var ptr = this.Alloc<T>();
            this.Ref<T>(ptr) = data;
            return ptr;
        }

        [INLINE(256)]
        public MemPtr Alloc<T>() where T : struct {
            var size = TSize<T>.size;
            var alignOf = TAlign<T>.align;
            return this.Alloc(size + alignOf);
        }

        [INLINE(256)]
        public MemPtr ReAlloc(MemPtr ptr, long size) {
            
            if (ptr == 0L) return this.Alloc(size);

            var blockSize = ((MemBlock*)((byte*)this.GetUnsafePtr(ptr) - TSize<MemBlock>.size))->size;
            var blockDataSize = blockSize - TSize<MemBlock>.size;
            if (blockDataSize > size) {
                return ptr;
            }

            if (blockDataSize < 0) {
                throw new Exception();
            }

            var newPtr = this.Alloc(size);
            this.MemMove(newPtr, 0, ptr, 0, blockDataSize);
            this.Free(ptr);

            return newPtr;
            
        }

        [INLINE(256)]
        public readonly void MemCopy(MemPtr dest, long destOffset, MemPtr source, long sourceOffset, long length) {
            
            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            var destZoneIndex = dest >> 32;
            var sourceZoneIndex = source >> 32;
            var destMaxOffset = (dest & MemoryAllocator.OFFSET_MASK) + destOffset + length;
            var sourceMaxOffset = (source & MemoryAllocator.OFFSET_MASK) + sourceOffset + length;
            
            if (destZoneIndex >= this.zonesListCount || sourceZoneIndex >= this.zonesListCount) {
                throw new OutOfBoundsException();
            }
            
            if (this.zonesList[destZoneIndex]->size < destMaxOffset || this.zonesList[sourceZoneIndex]->size < sourceMaxOffset) {
                throw new OutOfBoundsException();
            }
            #endif
            
            UnsafeUtility.MemCpy(this.GetUnsafePtr(dest + destOffset), this.GetUnsafePtr(source + sourceOffset), length);
            
        }

        [INLINE(256)]
        public readonly void MemMove(MemPtr dest, long destOffset, MemPtr source, long sourceOffset, long length) {
            
            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            var destZoneIndex = dest >> 32;
            var sourceZoneIndex = source >> 32;
            var destMaxOffset = (dest & MemoryAllocator.OFFSET_MASK) + destOffset + length;
            var sourceMaxOffset = (source & MemoryAllocator.OFFSET_MASK) + sourceOffset + length;
            
            if (destZoneIndex >= this.zonesListCount || sourceZoneIndex >= this.zonesListCount) {
                throw new OutOfBoundsException();
            }
            
            if (this.zonesList[destZoneIndex]->size < destMaxOffset || this.zonesList[sourceZoneIndex]->size < sourceMaxOffset) {
                throw new OutOfBoundsException();
            }
            #endif
            
            UnsafeUtility.MemMove(this.GetUnsafePtr(dest + destOffset), this.GetUnsafePtr(source + sourceOffset), length);
            
        }

        [INLINE(256)]
        public readonly void MemClear(MemPtr dest, long destOffset, long length) {
            
            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            var zoneIndex = dest >> 32;
            
            if (zoneIndex >= this.zonesListCount || this.zonesList[zoneIndex]->size < ((dest & MemoryAllocator.OFFSET_MASK) + destOffset + length)) {
                throw new OutOfBoundsException();
            }
            #endif

            UnsafeUtility.MemClear(this.GetUnsafePtr(dest + destOffset), length);
        }

        [INLINE(256)]
        public void Prepare(long size) {

            for (int i = 0; i < this.zonesListCount; i++) {
                var zone = this.zonesList[i];
                
                if (zone == null) continue;

                if (MemoryAllocator.ZmHasFreeBlock(zone, (int)size) == true) {
                    return;
                }
            }
 
            this.AddZone(MemoryAllocator.ZmCreateZone((int)Math.Max(size, MemoryAllocator.MIN_ZONE_SIZE)));
                
        }

        [INLINE(256)]
        public readonly void* GetUnsafePtr(in MemPtr ptr) {

            var zoneIndex = ptr >> 32;
            var offset = (ptr & MemoryAllocator.OFFSET_MASK);
            
            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            if (zoneIndex < this.zonesListCount && this.zonesList[zoneIndex] != null && this.zonesList[zoneIndex]->size < offset) {
                throw new OutOfBoundsException();
            }
            #endif

            return (byte*)this.zonesList[zoneIndex] + offset;
        }

        [INLINE(256)]
        internal readonly MemPtr GetSafePtr(void* ptr, int zoneIndex) {
            
            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            if (zoneIndex < this.zonesListCount && this.zonesList[zoneIndex] != null) {
                throw new OutOfBoundsException();
            }
            #endif
            
            var index = (long)zoneIndex << 32;
            var offset = ((byte*)ptr - (byte*)this.zonesList[zoneIndex]);

            return index | offset;
        }

        /// 
        /// Arrays
        /// 
        public readonly MemPtr RefArrayPtr<T>(MemPtr ptr, int index) where T : struct {
            var size = TSize<T>.size;
            return ptr + index * size;
        }
        
        [INLINE(256)]
        public readonly ref T RefArray<T>(MemPtr ptr, int index) where T : struct {
            var size = TSize<T>.size;
            return ref UnsafeUtility.AsRef<T>(this.GetUnsafePtr(ptr + index * size));
        }

        [INLINE(256)]
        public MemPtr ReAllocArray<T>(MemPtr ptr, int newLength) where T : struct {
            var size = TSize<T>.size;
            return this.ReAlloc(ptr, size * newLength);
        }

        [INLINE(256)]
        public MemPtr ReAllocArray(int sizeOf, MemPtr ptr, int newLength) {
            var size = sizeOf;
            return this.ReAlloc(ptr, size * newLength);
        }

        [INLINE(256)]
        public MemPtr AllocArray<T>(int length) where T : struct {
            var size = TSize<T>.size;
            return this.Alloc(size * length);
        }

        [INLINE(256)]
        public MemPtr AllocArray(int length, int sizeOf) {
            var size = sizeOf;
            return this.Alloc(size * length);
        }

    }

}
