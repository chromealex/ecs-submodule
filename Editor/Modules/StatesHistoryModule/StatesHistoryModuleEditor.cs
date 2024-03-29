using System.Linq;

#if STATES_HISTORY_MODULE_SUPPORT
namespace ME.ECSEditor {

    using ME.ECS;
    using ME.ECS.StatesHistory;
    using UnityEngine;
    using UnityEditor;
    
    [ComponentCustomEditor(typeof(IStatesHistoryModuleBase))]
    public class StatesHistoryModuleEditor : ME.ECSEditor.IGUIEditor<IStatesHistoryModuleBase> {

        public static readonly GUIStyle syncBoxStyle = "OL ToggleWhite";

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

        private bool eventsFoldState {
            get {
                return EditorPrefs.GetBool("ME.ECS.StatesHistoryModuleEditor.foldouts.eventsFoldState", false);
            }
            set {
                EditorPrefs.SetBool("ME.ECS.StatesHistoryModuleEditor.foldouts.eventsFoldState", value);
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

                UnityEngine.GUILayout.Label("<b>Memory Usage:</b> " + ME.ECS.MathUtils.BytesCountToString(WorldEditor.current.stateSize * (this.target.GetCacheSize() / this.target.GetTicksPerState())), style);
                UnityEngine.GUILayout.Label("<b>Events:</b> " + dataCount.ToString(), style);
                UnityEngine.GUILayout.Label("<b>Events Added:</b> " + this.target.GetEventsAddedCount().ToString(), style);
                UnityEngine.GUILayout.Label("<b>Events Played:</b> " + this.target.GetEventsPlayedCount().ToString(), style);

            });

            GUILayoutExt.Separator();
            var val = this.syncTableFoldState;
            GUILayoutExt.FoldOut(ref val, "Sync Table", () => {

                const float padding = 2f;
                const float margin = 2f;
                const float col1 = 60f;
                const float col2 = 50f;
                const float col3 = 22f;
                const float cellHeight = 22f;
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
                    GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.TableCaption(string.Empty, EditorStyles.miniBoldLabel); }, tableStyle,
                                     GUILayout.Width(col3),
                                     GUILayout.Height(cellHeight));
                }
                GUILayout.EndHorizontal();

                var syncHashTable = this.target.GetSyncHashTable();
                /*if (syncHashTable.ContainsKey(20) == false) syncHashTable.Add(20, new System.Collections.Generic.Dictionary<int, int>() {
                    { 100, 1234 }
                });
                if (syncHashTable.ContainsKey(100) == false) syncHashTable.Add(100, new System.Collections.Generic.Dictionary<int, int>() {
                    { 100, 1902832914 },
                    { 101, 1902832914 },
                    { 102, 1902832915 },
                });
                if (syncHashTable.ContainsKey(2000) == false) syncHashTable.Add(2000, new System.Collections.Generic.Dictionary<int, int>() {
                    { 100, 2345 }
                });*/
                foreach (var item in syncHashTable) {

                    var tick = item.Key;
                    int localHash = 0;
                    
                    GUILayout.BeginHorizontal();
                    {
                        GUILayoutExt.DataLabel(tick.ToString(), GUILayout.Width(col1));
                    }
                    GUILayout.EndHorizontal();
                    var stateHashResult = this.GetHashResult(item.Value);
                    
                    foreach (var kv in item.Value) {
                        
                        var playerId = kv.Key;
                        GUILayout.BeginHorizontal();
                        {
                            GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.DataLabel(string.Empty); }, tableStyle, GUILayout.Width(col1), GUILayout.Height(cellHeight));
                            GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.DataLabel(playerId.ToString()); }, tableStyle, GUILayout.Width(col2), GUILayout.Height(cellHeight));
                            GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.DataLabel(kv.Value.ToString()); }, tableStyle, GUILayout.ExpandWidth(true), GUILayout.Height(cellHeight));
                            GUILayoutExt.Box(padding, margin, () => {

                                GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();

                                this.DrawSyncCheckbox(stateHashResult, playerId);
                                
                                GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                                
                            }, tableStyle, GUILayout.Width(col3), GUILayout.Height(cellHeight));
                        }
                        GUILayout.EndHorizontal();

                    }

                }

            });
            this.syncTableFoldState = val;
            
            GUILayoutExt.Separator();
            val = this.statesHistoryFoldState;
            GUILayoutExt.FoldOut(ref val, "States History", () => {

                var padding = 2f;
                var margin = 2f;
                var col1 = 60f;
                var col2 = 30f;
                var col3 = 70f;
                var cellHeight = 22f;
                var tableStyle = (GUIStyle)"Box";
                
                UnityEngine.GUILayout.BeginHorizontal();
                {

                    if (UnityEngine.GUILayout.Button("Entities", UnityEditor.EditorStyles.miniButtonLeft) == true) {

                        var world = Worlds.currentWorld;
                        this.PrintEntities(world.currentState);

                    }

                    if (UnityEngine.GUILayout.Button("Events", UnityEditor.EditorStyles.miniButtonMid) == true) {

                        foreach (System.Collections.DictionaryEntry ren in this.target.GetData()) {

                            var entry = (ME.ECS.Collections.SortedList<long, HistoryEvent>)ren.Value;
                            for (int i = 0; i < entry.Count; ++i) {

                                UnityEngine.Debug.Log(entry.GetByIndex(i).ToString());

                            }

                        }

                    }

                    if (UnityEngine.GUILayout.Button("Reset State", UnityEditor.EditorStyles.miniButtonRight) == true) {

                        this.target.RecalculateFromResetState();

                    }

                }
                UnityEngine.GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.TableCaption("Tick", EditorStyles.miniBoldLabel); }, tableStyle,
                                     GUILayout.Width(col1),
                                     GUILayout.Height(cellHeight));
                    GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.TableCaption("Hash", EditorStyles.miniBoldLabel); }, tableStyle,
                                     GUILayout.ExpandWidth(true),
                                     GUILayout.Height(cellHeight));
                    GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.TableCaption("S", EditorStyles.miniBoldLabel); }, tableStyle,
                                     GUILayout.Width(col2),
                                     GUILayout.Height(cellHeight));
                    GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.TableCaption("Actions", EditorStyles.miniBoldLabel); }, tableStyle,
                                     GUILayout.Width(col3),
                                     GUILayout.Height(cellHeight));
                }
                GUILayout.EndHorizontal();

                var list = PoolList<ME.ECS.Network.ResultEntry<State>>.Spawn(10);
                this.target.GetResultEntries(list);
                var minTick = long.MaxValue;
                var maxTick = 0L;
                foreach (var entry in list) {
                    if (entry.isEmpty == false) {
                        if (minTick > entry.data.tick) minTick = entry.data.tick;
                        if (maxTick < entry.data.tick) maxTick = entry.data.tick;
                    }
                }

                foreach (var entry in list) {

                    var state = entry.data;
                    UnityEngine.GUILayout.BeginHorizontal();
                    {

                        GUI.color = Color.white;
                        GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.DataLabel(entry.isEmpty == true ? "-" : state.tick.ToString()); }, tableStyle, GUILayout.Width(col1), GUILayout.Height(cellHeight));
                        if (entry.isEmpty == false) {
                            GUI.color = Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minTick, maxTick, entry.data.tick));
                        }
                        GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.DataLabel(entry.isEmpty == true ? "-" : state.GetHash().ToString()); }, tableStyle, GUILayout.ExpandWidth(true), GUILayout.Height(cellHeight));
                        GUI.color = Color.white;
                        if (entry.isEmpty == false) {
                            GUILayoutExt.Box(padding, margin, () => {
                                var syncHashTable = this.target.GetSyncHashTable();
                                var stateHashResult = this.GetHashResult(syncHashTable.FirstOrDefault(x => x.Key == entry.data.tick).Value);
                                this.DrawSyncCheckbox(stateHashResult, 0);
                            }, tableStyle, GUILayout.Width(col2), GUILayout.Height(cellHeight));
                        }
                        GUILayoutExt.Box(padding, margin, () => {

                            EditorGUI.BeginDisabledGroup(entry.isEmpty == true);
                            if (UnityEngine.GUILayout.Button("Entities") == true) {

                                this.PrintEntities(state);

                            }
                            if (UnityEngine.GUILayout.Button("Rollback") == true) {

                                var targetTick = Worlds.current.currentState.tick;
                                Worlds.current.currentState.CopyFrom(state);
                                Worlds.current.currentState.Initialize(Worlds.current, false, true);
                                this.target.InvalidateEntriesAfterTick(state.tick);
                                Worlds.current.SetFromToTicks(state.tick, targetTick);

                            }
                            EditorGUI.EndDisabledGroup();

                        }, tableStyle, GUILayout.Width(col3), GUILayout.Height(cellHeight));

                    }
                    UnityEngine.GUILayout.EndHorizontal();
                
                }
                PoolList<ME.ECS.Network.ResultEntry<State>>.Recycle(ref list);

            });
            this.statesHistoryFoldState = val;
            GUILayoutExt.Separator();

            GUILayoutExt.Separator();
            val = this.eventsFoldState;
            GUILayoutExt.FoldOut(ref val, "Events Table", () => {

                const float padding = 2f;
                const float margin = 2f;
                const float col1 = 60f;
                const float col2 = 50f;
                const float cellHeight = 22f;
                var tableStyle = (GUIStyle)"Box";

                GUILayout.BeginHorizontal();
                {
                    GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.TableCaption("Tick", EditorStyles.miniBoldLabel); }, tableStyle,
                                     GUILayout.Width(col1),
                                     GUILayout.Height(cellHeight));
                    GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.TableCaption("Player", EditorStyles.miniBoldLabel); }, tableStyle,
                                     GUILayout.Width(col2),
                                     GUILayout.Height(cellHeight));
                    GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.TableCaption("Rpc ID", EditorStyles.miniBoldLabel); }, tableStyle,
                                     GUILayout.ExpandWidth(true),
                                     GUILayout.Height(cellHeight));
                }
                GUILayout.EndHorizontal();

                var events = this.target.GetEvents();
                foreach (var item in events) {

                    var tick = item.tick;
                    GUILayout.BeginHorizontal();
                    {
                        GUILayoutExt.DataLabel(tick.ToString(), GUILayout.Width(col1));
                    }
                    GUILayout.EndHorizontal();
                    
                    var playerId = item.order;
                    GUILayout.BeginHorizontal();
                    {
                        GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.DataLabel(tick.ToString()); }, tableStyle, GUILayout.Width(col1), GUILayout.Height(cellHeight));
                        GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.DataLabel(playerId.ToString()); }, tableStyle, GUILayout.Width(col2), GUILayout.Height(cellHeight));
                        GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.DataLabel(item.rpcId.ToString()); }, tableStyle, GUILayout.ExpandWidth(true), GUILayout.Height(cellHeight));
                    }
                    GUILayout.EndHorizontal();

                }

            });
            this.eventsFoldState = val;
            
            return false;

        }

        private void DrawSyncCheckbox(int stateHashResult, int playerId) {
            
            if (stateHashResult == 1) {

                using (new GUILayoutExt.GUIColorUsing(Color.green)) {

                    GUILayout.Toggle(true, new GUIContent(string.Empty, $"Local hash synced with player #{playerId}."), StatesHistoryModuleEditor.syncBoxStyle);

                }

            } else if (stateHashResult == -1) {
                                    
                using (new GUILayoutExt.GUIColorUsing(Color.red)) {

                    GUILayout.Toggle(true, new GUIContent(string.Empty, $"Local hash is not the same as player #{playerId} has, your server must resync that player."), StatesHistoryModuleEditor.syncBoxStyle);

                }

            } else {

                using (new GUILayoutExt.GUIColorUsing(Color.yellow)) {

                    GUILayout.Toggle(false, new GUIContent(string.Empty, $"Local hash is not sync yet with player #{playerId}, current tick is less than remote."), StatesHistoryModuleEditor.syncBoxStyle);

                }

            }
            
        }

        private int GetHashResult(System.Collections.Generic.Dictionary<int, int> item) {

            if (item == null) return 0;
            
            var stateHashResult = 0;
            var localHash = 0;
            foreach (var kv in item) {

                var hash = kv.Value;
                if (localHash != 0 && localHash != hash) {

                    stateHashResult = -1;
                    break;

                } else if (localHash != 0) {

                    stateHashResult = 1;

                }
                localHash = hash;

            }

            return stateHashResult;

        }

        private void PrintEntities(State state) {
            
            var str = "Tick: " + state.tick + ", hash: " + state.GetHash() + "\n";
            for (int i = 0; i < state.storage.cache.Length; ++i) {

                var entity = state.storage.cache[state.allocator, i];
                str += entity.ToStringNoVersion() + " (Ver: " + state.storage.versions.Get(state.allocator, entity).ToString() + ")\n";
                
            }
            UnityEngine.Debug.Log(str);
            
        }

    }

}
#endif