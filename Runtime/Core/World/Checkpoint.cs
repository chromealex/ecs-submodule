namespace ME.ECS {

    public struct Checkpoint : System.IDisposable {

        private object interestObj;
        private WorldStep step;
        
        public Checkpoint(string caption, object interestObj, WorldStep step) {

            this.interestObj = interestObj;
            this.step = step;
            
            #if CHECKPOINT_COLLECTOR
            if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(interestObj, step);
            #endif

            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample(caption);
            #endif

        }

        public Checkpoint(string caption) {

            this.interestObj = null;
            this.step = WorldStep.None;

            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample(caption);
            #endif

        }

        public void Dispose() {
            
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif

            #if CHECKPOINT_COLLECTOR
            if (this.interestObj != null) {
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.interestObj, this.step);
            }
            #endif

        }

    }

}