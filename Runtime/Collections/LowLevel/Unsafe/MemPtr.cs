
using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

namespace ME.ECS.Collections.LowLevel.Unsafe {

    public readonly struct MemPtr {

        public static readonly MemPtr Null = new(0, 0);

        public readonly int zoneId;
        public readonly int offset;

        [INLINE(256)]
        public MemPtr(int zoneId, int offset) {
            this.zoneId = zoneId;
            this.offset = offset;
        }
        
        [INLINE(256)]
        public static MemPtr operator +(MemPtr ptr, int offset) => new (ptr.zoneId, ptr.offset + offset);
        
        [INLINE(256)]
        public static MemPtr operator -(MemPtr ptr, int offset) => new (ptr.zoneId, ptr.offset - offset);
        
        [INLINE(256)]
        public static bool operator ==(MemPtr a, MemPtr b) => a.zoneId == b.zoneId && a.offset == b.offset;

        [INLINE(256)]
        public static bool operator !=(MemPtr a, MemPtr b) => !(a == b);

        public override string ToString() => $"(zone id {this.zoneId}; offset {this.offset})";

        public bool Equals(MemPtr other) {
            return this.zoneId == other.zoneId && this.offset == other.offset;
        }

        public override bool Equals(object obj) {
            return obj is MemPtr other && Equals(other);
        }

        public override int GetHashCode() {
            var value = (long)this.zoneId << 32;
            value |= (long)this.offset;
            
            return value.GetHashCode();
        }

    }

}