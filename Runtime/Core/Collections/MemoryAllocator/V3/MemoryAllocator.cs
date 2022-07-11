﻿using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace ME.ECS.Collections.V3 {

    using MemPtr = System.Int64;

    public unsafe partial struct MemoryAllocator : IMemoryAllocator<MemoryAllocator, MemPtr> {

        private MemZone* zone;
        private long maxSize;

        /// 
        /// Constructors
        /// 
        public MemoryAllocator Initialize(long initialSize, long maxSize = -1L) {

            this.zone = MemoryAllocator.ZmCreateZone((int)initialSize);
            this.maxSize = maxSize;

            return this;
        }

        public void Dispose() {

            MemoryAllocator.ZmFreeZone(this.zone);

            this.zone = null;
        }

        public void CopyFrom(in MemoryAllocator other) {

            if (this.zone == null) {
                this.zone = MemoryAllocator.ZmCreateZone(other.zone->size);
            } else if (this.zone->size != other.zone->size) {
                MemoryAllocator.ZmFreeZone(this.zone);
                this.zone = MemoryAllocator.ZmCreateZone(other.zone->size);
            }

            UnsafeUtility.MemCpy(this.zone, other.zone, other.zone->size);

            this.maxSize = other.maxSize;
        }

        /// 
        /// Base
        /// 
        public readonly ref T Ref<T>(MemPtr ptr) where T : unmanaged {
            return ref this.RefUnmanaged<T>(ptr);
        }

        public readonly ref T RefUnmanaged<T>(MemPtr ptr) where T : struct {
            return ref UnsafeUtility.AsRef<T>(this.GetUnsafePtr(ptr));
        }

        public MemPtr Alloc<T>() where T : unmanaged {
            var size = sizeof(T);
            var alignOf = UnsafeUtility.AlignOf<T>();
            return this.Alloc(size + alignOf);
        }

        public MemPtr AllocUnmanaged<T>() where T : struct {
            var size = UnsafeUtility.SizeOf<T>();
            var alignOf = UnsafeUtility.AlignOf<T>();
            return this.Alloc(size + alignOf);
        }

        public MemPtr Alloc(long size) {
            return this.Alloc(size, ClearOptions.UninitializedMemory);
        }

        public MemPtr Alloc(long size, ClearOptions options) {
            var ptr = MemoryAllocator.ZmMalloc(this.zone, (int)size, null);

            if (ptr == null) {
                var newSize = System.Math.Max(this.zone->size * 2, this.zone->size + MemoryAllocator.ZmGetMemBlockSize((int)size));
                this.zone = MemoryAllocator.ZmReallocZone(this.zone, newSize);
                return this.Alloc(size, options);
            }

            if (options == ClearOptions.ClearMemory) {
                UnsafeUtility.MemClear(ptr, (int)size);
            }

            return this.GetSafePtr(ptr);
        }

        public bool Free(MemPtr ptr) {
            return ptr == 0 ? false : MemoryAllocator.ZmFree(this.zone, this.GetUnsafePtr(ptr));
        }

        public MemPtr ReAlloc(MemPtr ptr, long size, ClearOptions options) {
            if (ptr == 0L) {

                return this.Alloc(size, options);

            }

            var blockDataSize = ((MemBlock*)((byte*)this.GetUnsafePtr(ptr) - sizeof(MemBlock)))->size - sizeof(MemBlock);

            if (blockDataSize > size) {
                return ptr;
            }

            var newPtr = this.Alloc((int)size, ClearOptions.UninitializedMemory);

            if (options == ClearOptions.ClearMemory) {
                this.MemClear(newPtr, 0, size);
            }

            this.MemCopy(newPtr, 0, ptr, 0, blockDataSize);

            this.Free(ptr);

            return newPtr;
        }

        public void MemCopy(MemPtr dest, long destOffset, MemPtr source, long sourceOffset, long length) {
            UnsafeUtility.MemCpy(this.GetUnsafePtr(dest + destOffset), this.GetUnsafePtr(source + sourceOffset), length);
        }

        public void MemClear(MemPtr dest, long destOffset, long length) {
            UnsafeUtility.MemClear(this.GetUnsafePtr(dest + destOffset), length);
        }

        public void Prepare(long size) {
            if (MemoryAllocator.ZmHasFreeBlock(this.zone, (int)size) == false) {
                this.zone = MemoryAllocator.ZmReallocZone(this.zone, this.zone->size + MemoryAllocator.ZmGetMemBlockSize((int)size));
            }
        }

        public readonly void* GetUnsafePtr(in MemPtr ptr) {
            return (byte*)this.zone + ptr;
        }

        private readonly MemPtr GetSafePtr(void* ptr) {
            return (MemPtr)((byte*)ptr - (byte*)this.zone);
        }

        /// 
        /// Arrays
        /// 
        public ref T RefArray<T>(MemPtr ptr, int index) where T : unmanaged {
            var size = sizeof(T);
            return ref UnsafeUtility.AsRef<T>(this.GetUnsafePtr(ptr + index * size));
        }

        public MemPtr ReAllocArray<T>(MemPtr ptr, int newLength, ClearOptions options) where T : unmanaged {
            var size = sizeof(T);
            return this.ReAlloc(ptr, size * newLength, options);
        }

        public MemPtr AllocArray<T>(int length) where T : unmanaged {
            var size = sizeof(T);
            return this.Alloc(size * length);
        }

        public ref T RefArrayUnmanaged<T>(MemPtr ptr, int index) where T : struct {
            var size = UnsafeUtility.SizeOf<T>();
            return ref UnsafeUtility.AsRef<T>(this.GetUnsafePtr(ptr + index * size));
        }

        public MemPtr ReAllocArrayUnmanaged<T>(MemPtr ptr, int newLength, ClearOptions options) where T : struct {
            var size = UnsafeUtility.SizeOf<T>();
            return this.ReAlloc(ptr, size * newLength, options);
        }

        public MemPtr AllocArrayUnmanaged<T>(int length) where T : struct {
            var size = UnsafeUtility.SizeOf<T>();
            return this.Alloc(size * length);
        }

    }

}