namespace ME.ECS.DebugUtils {
    
    using Collections.LowLevel.Unsafe;

    public unsafe class MemoryAllocatorProxyDebugger {

        public struct Dump {

            public string[] blocks;

        }
        
        private readonly MemoryAllocator allocator;
        
        public MemoryAllocatorProxyDebugger(MemoryAllocator allocator) {

            this.allocator = allocator;

        }

        public Dump[] dump {
            get {
                var list = new System.Collections.Generic.List<Dump>();
                for (int i = 0; i < this.allocator.zonesListCount; ++i) {
                    var zone = this.allocator.zonesList[i];

                    if (zone == null) {
                        list.Add(new Dump() {
                            blocks = null,
                        });
                        continue;
                    }

                    var blocks = new System.Collections.Generic.List<string>();
                    MemoryAllocator.ZmDumpHeap(zone, blocks);
                    var item = new Dump() {
                        blocks = blocks.ToArray(),
                    };
                    list.Add(item);
                }
                
                return list.ToArray();
            }
        }

        public Dump[] checks {
            get {
                var list = new System.Collections.Generic.List<Dump>();
                for (int i = 0; i < this.allocator.zonesListCount; ++i) {
                    var zone = this.allocator.zonesList[i];

                    if (zone == null) {
                        list.Add(new Dump() {
                            blocks = null,
                        });
                        continue;
                    }
                    
                    var blocks = new System.Collections.Generic.List<string>();
                    MemoryAllocator.ZmCheckHeap(zone, blocks);
                    var item = new Dump() {
                        blocks = blocks.ToArray(),
                    };
                    list.Add(item);
                }
                
                return list.ToArray();
            }
        }

    }

}