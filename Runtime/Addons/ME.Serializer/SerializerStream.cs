using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

namespace ME.ECS.Serializer {

    public class SerializerStream : System.IDisposable {

        private int capacity;
        private int position;
        private byte[] buffer;
        
        public int Capacity {
            set {
                if (value > this.capacity) {
                    if (this.capacity > 0) value *= 2;
                    this.capacity = value;
                    System.Array.Resize(ref this.buffer, this.capacity);
                }
            }
            get => this.capacity;
        }
        
        public int Position {
            set => this.position = value;
            get => this.position;
        }

        public byte[] GetBuffer() => this.buffer;

        public SerializerStream(byte[] buffer) {

            this.buffer = buffer;
            this.capacity = buffer.Length;
            this.position = 0;

        }

        public SerializerStream(int capacity) {

            this.capacity = capacity;
            this.buffer = new byte[capacity];
            this.position = 0;
            
        }

        [INLINE(256)] public byte[] ToArray() {
            
            var arr = new byte[this.position];
            System.Buffer.BlockCopy(this.buffer, 0, arr, 0, arr.Length);
            return arr;
            
        }

        [INLINE(256)] public void WriteByte(byte b) {

            this.Capacity = this.position + 1;
            this.buffer[this.position] = b;
            ++this.position;

        }

        [INLINE(256)] public void Write(byte[] bytes, int offset, int length) {

            this.Capacity = this.position + length;
            System.Buffer.BlockCopy(bytes, offset, this.buffer, this.position, length);
            this.position += length;
            
        }

        [INLINE(256)] public byte ReadByte() {

            var b = this.buffer[this.position];
            ++this.position;
            return b;

        }

        [INLINE(256)] public void Read(byte[] bytes, int offset, int length) {

            System.Buffer.BlockCopy(this.buffer, this.position, bytes, offset, length);
            this.position += length;
            
        }

        public void Dispose() {}

    }

}