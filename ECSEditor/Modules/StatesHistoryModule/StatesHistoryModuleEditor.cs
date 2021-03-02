#if STATES_HISTORY_MODULE_SUPPORT
namespace ME.ECSEditor {

    using ME.ECS;
    using ME.ECS.StatesHistory;
    
    [ComponentCustomEditor(typeof(IStatesHistoryModuleBase))]
    public class StatesHistoryModuleEditor : ME.ECSEditor.IGUIEditor<IStatesHistoryModuleBase> {

        public IStatesHistoryModuleBase target { get; set; }
        public IStatesHistoryModuleBase[] targets { get; set; }

        public T GetTarget<T>() {

            return (T)(object)this.target;

        }

        bool IGUIEditorBase.OnDrawGUI() {
                
            var style = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label);
            style.richText = true;

            var dataCount = 0;
            foreach (System.Collections.DictionaryEntry ren in this.target.GetData()) {

                dataCount += ((ME.ECS.Collections.SortedList<long, HistoryEvent>)ren.Value).Count;

            }
            
            UnityEngine.GUILayout.Label("<b>Events:</b> " + dataCount.ToString(), style);
            UnityEngine.GUILayout.Label("<b>Events Added:</b> " + this.target.GetEventsAddedCount().ToString(), style);
            UnityEngine.GUILayout.Label("<b>Events Played:</b> " + this.target.GetEventsPlayedCount().ToString(), style);

            if (UnityEngine.GUILayout.Button("Print Entities") == true) {

                var world = Worlds.currentWorld;
                this.PrintEntities(world.currentState);

            }

            if (UnityEngine.GUILayout.Button("Print Events") == true) {

                foreach (System.Collections.DictionaryEntry ren in this.target.GetData()) {
                    
                    var entry = (ME.ECS.Collections.SortedList<long, HistoryEvent>)ren.Value;
                    for (int i = 0; i < entry.Count; ++i) {

                        UnityEngine.Debug.Log(entry.GetByIndex(i).ToString());

                    }
                    
                }

            }

            if (UnityEngine.GUILayout.Button("Recalculate from Reset State") == true) {

                this.target.RecalculateFromResetState();

            }

            var dataStates = this.target.GetDataStates();
            var entries = dataStates.GetEntries();
            foreach (var entryData in entries) {

                var entry = entryData as ME.ECS.Network.IStatesHistoryEntry;
                var state = entry.GetData() as State;
                UnityEngine.GUILayout.BeginHorizontal();
                {
                    UnityEngine.GUILayout.Label(entry.isEmpty == true ? "None" : "Tick: " + state.tick + ", Hash: " + state.GetHash());
                    if (entry.isEmpty == false) {

                        if (UnityEngine.GUILayout.Button("Print Entities") == true) {

                            this.PrintEntities(state);

                        }

                    }
                }
                UnityEngine.GUILayout.EndHorizontal();
                
            }

            return false;

        }

        private void PrintEntities(State state) {
            
            var str = "Tick: " + state.tick + ", hash: " + state.GetHash() + "\n";
            for (int i = 0; i < state.storage.cache.Length; ++i) {

                var entity = state.storage.cache.arr[i];
                str += entity.ToStringNoVersion() + " (Ver: " + state.storage.versions.Get(entity).ToString() + ")\n";
                
            }
            UnityEngine.Debug.Log(str);
            
        }

    }

}
#endif