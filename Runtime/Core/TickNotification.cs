namespace ME.ECS {

    public struct TickNotification : System.IEquatable<TickNotification> {

        public Entity entity;
        public UnsafeData data;

        public void Dispose(ref ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator) {
            this.data.Dispose(ref allocator);
        }

        public bool Equals(TickNotification other) {
            return this.entity.Equals(other.entity) && this.data.Equals(other.data);
        }

        public override bool Equals(object obj) {
            return obj is TickNotification other && Equals(other);
        }

        public override int GetHashCode() {
            return this.entity.GetHashCode() ^ this.data.GetHashCode();
        }

    }

}