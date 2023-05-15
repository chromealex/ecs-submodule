//#define MEMORY_ALLOCATOR_BOUNDS_CHECK
//#define MEMORY_ALLOCATOR_LOGS

using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;
using BURST = Unity.Burst.BurstCompileAttribute;

namespace ME.ECS.Collections.LowLevel.Unsafe {

    public struct TSize<T> where T : struct {

        public static readonly int size = UnsafeUtility.SizeOf<T>();

    }

    public struct TAlign<T> where T : struct {

        public static readonly int align = UnsafeUtility.AlignOf<T>();

    }

    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    public unsafe partial struct MemoryAllocator : IDisposable {

        #if MEMORY_ALLOCATOR_LOGS && UNITY_EDITOR
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
        
        internal const int MIN_ZONE_SIZE = 512 * 1024;
        private const int MIN_ZONES_LIST_CAPACITY = 20;

        [NativeDisableUnsafePtrRestriction]
        internal MemZone** zonesList;
        internal int zonesListCount;
        internal int zonesListCapacity;
        internal long maxSize;
        
        public bool isValid => this.zonesList != null;

        public static MemoryAllocatorContext CreateContext() {

            return new MemoryAllocatorContext() {
                allocator = Worlds.current.currentState.allocator,
            }.Create();

        }

        public static MemoryAllocatorContext CreateContext(in MemoryAllocator allocator) {

            return new MemoryAllocatorContext() {
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
                    size -= MemoryAllocator.ZmGetFreeMemory(zone);
                }
            }

            return size;

        }

        public int GetFreeSize() {

            var size = 0;
            for (int i = 0; i < this.zonesListCount; i++) {
                var zone = this.zonesList[i];
                if (zone != null) {
                    size += MemoryAllocator.ZmGetFreeMemory(zone);
                }
            }

            return size;

        }

        /// 
        /// Constructors
        /// 
        [INLINE(256)]
        public MemoryAllocator Initialize(long initialSize, long maxSize = -1L) {

            if (maxSize < initialSize) maxSize = initialSize;
            
            this.AddZone(MemoryAllocator.ZmCreateZone((int)Math.Max(initialSize, MemoryAllocator.MIN_ZONE_SIZE)));
            
            this.maxSize = maxSize;

            return this;
        }

        [INLINE(256)]
        public void Dispose() {

            this.FreeZones();
            
            if (this.zonesList != null) {
                UnsafeUtility.Free(this.zonesList, Allocator.Persistent);
				this.zonesList = null;
			}

            this.zonesListCapacity = 0;
            this.maxSize = default;

        }

        [INLINE(256)]
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

        [INLINE(256)]
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

        [INLINE(256)]
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
        public MemPtr ReAlloc(MemPtr ptr, int size) {
            
            if (ptr == MemPtr.Null) return this.Alloc(size);

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
        public readonly void MemCopy(MemPtr dest, int destOffset, MemPtr source, int sourceOffset, int length) {

            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            var destMaxOffset = dest.offset + destOffset + length;
            var sourceMaxOffset = source.offset + sourceOffset + length;
            
            if (dest.zoneId >= this.zonesListCount || source.zoneId >= this.zonesListCount) {
                throw new OutOfBoundsException();
            }
            
            if (this.zonesList[dest.zoneId]->size < destMaxOffset || this.zonesList[source.zoneId]->size < sourceMaxOffset) {
                throw new OutOfBoundsException();
            }
            #endif
            
            UnsafeUtility.MemCpy(this.GetUnsafePtr(dest + destOffset), this.GetUnsafePtr(source + sourceOffset), length);
            
        }

        [INLINE(256)]
        public readonly void MemMove(MemPtr dest, int destOffset, MemPtr source, int sourceOffset, int length) {
            
            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            var destMaxOffset = dest.offset + destOffset + length;
            var sourceMaxOffset = source.offset + sourceOffset + length;
            
            if (dest.zoneId >= this.zonesListCount || source.zoneId >= this.zonesListCount) {
                throw new OutOfBoundsException();
            }
            
            if (this.zonesList[dest.zoneId]->size < destMaxOffset || this.zonesList[source.zoneId]->size < sourceMaxOffset) {
                throw new OutOfBoundsException();
            }
            #endif
            
            UnsafeUtility.MemMove(this.GetUnsafePtr(dest + destOffset), this.GetUnsafePtr(source + sourceOffset), length);
            
        }

        [INLINE(256)]
        public readonly void MemClear(MemPtr dest, int destOffset, int length) {
            
            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            if (dest.zoneId >= this.zonesListCount || this.zonesList[dest.zoneId]->size < (dest.offset + destOffset + length)) {
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

            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            if (ptr.zoneId < this.zonesListCount && this.zonesList[ptr.zoneId] != null && this.zonesList[ptr.zoneId]->size < ptr.offset) {
                throw new OutOfBoundsException();
            }
            #endif

            return (byte*)this.zonesList[ptr.zoneId] + ptr.offset;
        }

        [INLINE(256)]
        internal readonly MemPtr GetSafePtr(void* ptr, int zoneIndex) {
            
            #if MEMORY_ALLOCATOR_BOUNDS_CHECK
            if (zoneIndex < this.zonesListCount && this.zonesList[zoneIndex] != null) {
                throw new OutOfBoundsException();
            }
            #endif
            
            var offset = ((byte*)ptr - (byte*)this.zonesList[zoneIndex]);

            return new MemPtr(zoneIndex, (int)offset);
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
