using System.Collections.Generic;

namespace ME.ECS.Network {

    public class PingStorage {

        private List<double> storedPings;
        private int storageSize;
        private int medianSize;
        
        public double avg { get; private set; }
        
        public PingStorage(int storageSize = 21, int medianSize = 11) {

            this.storageSize = storageSize;
            this.medianSize = medianSize;
            this.storedPings = new List<double>();

        }
        
        public void AddValue(double ping) {

            // Insert new ping value in the sorted order
            int firstBiggestIndex = this.storedPings.Count;
            for (int i = 0; i < this.storedPings.Count; ++i) {
                
                if (this.storedPings[i] > ping) {
                    firstBiggestIndex = i;
                    break;
                }

            }
            
            this.storedPings.Insert(firstBiggestIndex, ping);

            // Remove side values
            while (this.storedPings.Count >= this.storageSize) {
                if (firstBiggestIndex > this.storageSize * 0.5f) {
                    // Remove from the beginning
                    this.storedPings.RemoveAt(0);
                }
                else {
                    // Remove from the end
                    this.storedPings.RemoveAt(this.storedPings.Count - 1);
                }
            }

            if (this.storedPings.Count == 0) {
                this.avg = 0;
                return;
            }

            var sum = 0d;
            // Calculate average if there is less then median size
            if (this.storedPings.Count <= this.medianSize) {
                
                for (int i = 0; i < this.storedPings.Count; ++i) {
                    sum += this.storedPings[i];
                }
                this.avg = sum / this.storedPings.Count;
                return;

            }

            sum = 0d;
            var cutSize = (this.storedPings.Count - this.medianSize) / 2;
            for (int i = cutSize; i < this.storedPings.Count - cutSize; ++i) {
                sum += this.storedPings[i];
            }
        
            this.avg = sum / (this.storedPings.Count - cutSize * 2);
        
        }

    }

}
