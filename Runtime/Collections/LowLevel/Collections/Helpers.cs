namespace ME.ECS.Collections.LowLevel {

    using Unsafe;

    public interface IIsCreated {

        bool isCreated { get; }

    }
    
    public interface IEquatableAllocator<T> {

        bool Equals(in MemoryAllocator allocator, T obj);
        int GetHash(in MemoryAllocator allocator);

    }

    public static class Helpers {

        public static int NextPot(int n) {

            --n;
            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            ++n;
            return n;

        }

    }

}