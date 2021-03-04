#if STATES_HISTORY_MODULE_SUPPORT
namespace ME.ECSEditor {

    using ME.ECS;
    using ME.ECS.StatesHistory;
    using UnityEngine;
    using UnityEditor;
    
    [ComponentCustomEditor(typeof(IStatesHistoryModuleBase))]
    public class StatesHistoryModuleEditor : ME.ECSEditor.IGUIEditor<IStatesHistoryModuleBase> {

        private bool syncTableFoldState {
            get {
                return EditorPrefs.GetBool("ME.ECS.StatesHistoryModuleEditor.foldouts.syncTableFoldState", false);
            }
            set {
                EditorPrefs.SetBool("ME.ECS.StatesHistoryModuleEditor.foldouts.syncTableFoldState", value);
            }
        }
        
        private bool statesHistoryFoldState {
            get {
                return EditorPrefs.GetBool("ME.ECS.StatesHistoryModuleEditor.foldouts.statesHistoryFoldState", false);
            }
            set {
                EditorPrefs.SetBool("ME.ECS.StatesHistoryModuleEditor.foldouts.statesHistoryFoldState", value);
            }
        }
        
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

            GUILayoutExt.Box(2f, 2f, () => {

                UnityEngine.GUILayout.Label("<b>Events:</b> " + dataCount.ToString(), style);
                UnityEngine.GUILayout.Label("<b>Events Added:</b> " + this.target.GetEventsAddedCount().ToString(), style);
                UnityEngine.GUILayout.Label("<b>Events Played:</b> " + this.target.GetEventsPlayedCount().ToString(), style);

            });

            GUILayoutExt.Separator();
            var val = this.syncTableFoldState;
            GUILayoutExt.FoldOut(ref val, "Sync Table", () => {

                var padding = 2f;
                var margin = 2f;
                var col1 = 60f;
                var col2 = 70f;
                var cellHeight = 18f;
                var tableStyle = (GUIStyle)"Box";

                GUILayout.BeginHorizontal();
                {
                    GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.TableCaption("Tick", EditorStyles.miniBoldLabel); }, tableStyle,
                                     GUILayout.Width(col1),
                                     GUILayout.Height(cellHeight));
                    GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.TableCaption("Player", EditorStyles.miniBoldLabel); }, tableStyle,
                                     GUILayout.Width(col2),
                                     GUILayout.Height(cellHeight));
                    GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.TableCaption("Hash", EditorStyles.miniBoldLabel); }, tableStyle,
                                     GUILayout.ExpandWidth(true),
                                     GUILayout.Height(cellHeight));
                }
                GUILayout.EndHorizontal();

                var syncHashTable = this.target.GetSyncHashTable();
                foreach (var item in syncHashTable) {

                    GUILayout.BeginHorizontal();
                    {
                        GUILayoutExt.DataLabel(item.Key.ToString(), GUILayout.Width(col1));
                    }
                    GUILayout.EndHorizontal();
                    foreach (var kv in item.Value) {

                        GUILayout.BeginHorizontal();
                        {
                            GUILayoutExt.DataLabel(string.Empty, GUILayout.Width(col1));
                            GUILayoutExt.DataLabel(kv.Key.ToString(), GUILayout.Width(col2));
                            GUILayoutExt.DataLabel(kv.Value.ToString(), GUILayout.ExpandWidth(true));
                        }
                        GUILayout.EndHorizontal();

                    }

                }

            });
            this.syncTableFoldState = val;
            
            GUILayoutExt.Separator();
            val = this.statesHistoryFoldState;
            GUILayoutExt.FoldOut(ref val, "States History", () => {

                UnityEngine.GUILayout.BeginHorizontal();
                {

                    if (UnityEngine.GUILayout.Button("Print Entities", UnityEditor.EditorStyles.miniButtonLeft) == true) {

                        var world = Worlds.currentWorld;
                        this.PrintEntities(world.currentState);

                    }

                    if (UnityEngine.GUILayout.Button("Print Events", UnityEditor.EditorStyles.miniButtonMid) == true) {

                        foreach (System.Collections.DictionaryEntry ren in this.target.GetData()) {

                            var entry = (ME.ECS.Collections.SortedList<long, HistoryEvent>)ren.Value;
                            for (int i = 0; i < entry.Count; ++i) {

                                UnityEngine.Debug.Log(entry.GetByIndex(i).ToString());

                            }

                        }

                    }

                    if (UnityEngine.GUILayout.Button("Recalc from Reset State", UnityEditor.EditorStyles.miniButtonRight) == true) {

                        this.target.RecalculateFromResetState();

                    }

                }
                UnityEngine.GUILayout.EndHorizontal();

            });
            this.statesHistoryFoldState = val;
            GUILayoutExt.Separator();

            var dataStates = this.target.GetDataStates();
            var entries = dataStates.GetEntries();
            foreach (var entryData in entries) {

                var entry = entryData as ME.ECS.Network.IStatesHistoryEntry;
                var state = entry.GetData() as State;
                UnityEngine.GUILayout.BeginHorizontal();
                {
                    UnityEngine.GUILayout.Label(entry.isEmpty == true ? "None" : "Tick: " + state.tick + ", Hash: " + state.GetHash());
                    if (entry.isEmpty == false) {

                        if (UnityEngine.GUILayout.Button("Print Entities", UnityEngine.GUILayout.Width(80f)) == true) {

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