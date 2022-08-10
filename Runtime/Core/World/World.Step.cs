namespace ME.ECS {
    
    using Collections;

    public partial class World {

        public delegate ListCopyable<T> GetGroupItems<T>(in SystemGroup group);

        public void StepGroup<T, TState>(TState state, BufferArray<SystemGroup> groups, float deltaTime, WorldStep step, GetGroupItems<T> getGroupItems, System.Action<TState, T, float> stepAction) {
            
            ////////////////
            this.currentStep |= step;
            ////////////////
            try {

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.systemGroups.arr, step);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"VisualTick-Pre [All Systems]");
                #endif

                for (int i = 0, count = groups.Count; i < count; ++i) {

                    ref var group = ref groups.arr[i];
                    var list = getGroupItems.Invoke(in group);
                    if (list == null) continue;
                    
                    for (int j = 0; j < list.Count; ++j) {

                        var system = list[j];

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(system, step);
                        #endif

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample(system.GetType().FullName);
                        #endif

                        stepAction.Invoke(state, system, deltaTime);

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(system, step);
                        #endif

                    }

                }

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.systemGroups.arr, step);
                #endif

            } catch (System.Exception ex) {

                UnityEngine.Debug.LogException(ex);

            }
            ////////////////
            this.currentStep &= ~step;
            ////////////////

        }
        
        public void StepElement<T, TInterface, TState>(TState state, ListCopyable<T> list, float deltaTime, WorldStep step, System.Func<TState, T, int, bool> checkActive, System.Action<TInterface, float> stepAction) where T : IModuleBase {
            
            ////////////////
            this.currentStep |= step;
            ////////////////
            try {

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules, step);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"VisualTick-Pre [All Modules]");
                #endif

                for (int i = 0, count = list.Count; i < count; ++i) {

                    var item = list[i];
                    if (checkActive.Invoke(state, item, i) == true) {

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(item, step);
                        #endif

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample(item.GetType().FullName);
                        #endif

                        if (item is TInterface moduleBase) {

                            stepAction.Invoke(moduleBase, deltaTime);

                        }

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(item, step);
                        #endif

                    }

                }

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules, step);
                #endif
                
            } catch (System.Exception ex) {

                UnityEngine.Debug.LogException(ex);

            }
            ////////////////
            this.currentStep &= ~step;
            ////////////////

        }

    }

}