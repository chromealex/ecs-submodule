
namespace ME.ECSBurst {
    
    public interface IArrayElementCopy<T> {

        void Copy(T from, ref T to);
        void Recycle(T item);

    }

    public interface IArrayElementCopyWithIndex<T> {

        void Copy(int index, T from, ref T to);
        void Recycle(int index, ref T item);

    }

}