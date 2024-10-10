namespace ME.ECS.Collections.LowLevel.Unsafe {

    public unsafe partial struct MemoryAllocator {

        public long GetHashCode() {

            long hash = 0;

            // First 12 bits stores zones count (3 last HEX digits) (enough for 2^10 = 1024 zones, when maximum error free defined as 1000)
            hash ^= this.zonesListCount;

            long blockCounter = 0;

            for (int i = 0; i < this.zonesListCount; ++i) {

                var zone = this.zonesList[i];

                if (zone == null) {
                    continue;
                }

                for (var block = zone->blocklist.next.Ptr(zone);; block = block->next.Ptr(zone)) {
                    if (block->next.Ptr(zone) == &zone->blocklist) {
                        // all blocks have been hit
                        break;
                    }

                    var memBlockOffset = new MemBlockOffset(block, zone);
                    var memBlockHash = (long) memBlockOffset.value.GetHashCode();
                    hash ^= memBlockHash << 32;

                    ++blockCounter;

                }

            }

            // Second 20 bits stores blocks count (5 HEX digits) (should be enough for 2^20 = 1048576 blocks total)
            hash ^= blockCounter << 12;

            return hash;

        }

        public void PrintAllocatorHash(string infoPrefix) {

            var hash = this.GetHashCode();
            UnityEngine.Debug.Log($"AllocatorHash: {infoPrefix}: HEX hash (hash-blocks-zones): {System.Convert.ToString(hash, 16).PadLeft(16, '0').Insert(8 + 5, "-").Insert(8, "-")}");

        }

    }

}
