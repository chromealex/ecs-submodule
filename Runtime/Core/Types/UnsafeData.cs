using Unity.Collections.LowLevel.Unsafe;

namespace ME.ECS {

    public struct UnsafeData {

        public abstract class ItemBase {

            public abstract void Recycle();
            public abstract ItemBase Clone();
            public abstract void CopyFrom(ItemBase other);

        }
        
        public class Item<T> : ItemBase {

            public T data;

            public T Read() {

                return this.data;

            }

            public override ItemBase Clone() {

                var item = PoolClass<Item<T>>.Spawn();
                item.CopyFrom(this);
                return item;

            }

            public override void CopyFrom(ItemBase other) {

                var _other = (Item<T>)other;
                this.data = _other.data;
                
            }

            public override void Recycle() {

                this.data = default;
                PoolClass<Item<T>>.Recycle(this);
                
            }

        }
        
        public ItemBase data;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public UnsafeData Set<T>(T data) where T : struct {

            if (this.data != null) {

                this.data.Recycle();

            }

            var item = PoolClass<Item<T>>.Spawn();
            item.data = data;
            this.data = item;
            
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public T Read<T>() where T : struct {

            return ((Item<T>)this.data).Read();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void CopyFrom(in UnsafeData other) {

            if (other.data == null && this.data == null) {
                return;
            } else if (other.data != null && this.data == null) {
                this.data = other.data.Clone();
                return;
            } else if (other.data == null && this.data != null) {
                this.data.Recycle();
                this.data = null;
                return;
            }

            this.data.CopyFrom(other.data);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {
            
            if (this.data != null) this.data.Recycle();
            this.data = null;

        }

    }

    /*
    public unsafe struct UnsafeData {

        public System.IntPtr data;
        public int sizeOf;
        public int alignOf;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public UnsafeData Set<T>(T data) where T : struct {

            if (this.data != System.IntPtr.Zero) {
                
                NativeArrayUtils.Dispose(ref this.data);
                
            }

            this.sizeOf = UnsafeUtility.SizeOf<T>();
            this.alignOf = UnsafeUtility.AlignOf<T>();
            this.data = (System.IntPtr)UnsafeUtility.Malloc(this.sizeOf, this.alignOf, Unity.Collections.Allocator.Persistent);
            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.WriteArrayElement((void*)this.data, 0, data);
            
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public T Read<T>() where T : struct {

            return Unity.Collections.LowLevel.Unsafe.UnsafeUtility.ReadArrayElement<T>((void*)this.data, 0);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void CopyFrom(in UnsafeData other) {

            this.sizeOf = other.sizeOf;
            this.alignOf = other.alignOf;
            NativeArrayUtils.Copy(other.data, ref this.data, this.sizeOf, this.alignOf);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {
            
            if (this.data != System.IntPtr.Zero) {

                this.sizeOf = default;
                this.alignOf = default;
                NativeArrayUtils.Dispose(ref this.data);
                
            }
            
        }

    }
    */

}