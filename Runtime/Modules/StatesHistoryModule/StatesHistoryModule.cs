﻿#if STATES_HISTORY_MODULE_SUPPORT
using System.Collections.Generic;

#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS {
    
    public partial interface IWorldBase {

        Tick GetCurrentTick();
        //void Simulate(double time);
        //void Simulate(Tick toTick);
        void SetStatesHistoryModule(StatesHistory.IStatesHistoryModuleBase module);

    }

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed partial class World {

        private StatesHistory.IStatesHistoryModuleBase statesHistoryModule;
        public void SetStatesHistoryModule(StatesHistory.IStatesHistoryModuleBase module) {

            this.statesHistoryModule = module;

        }

        public Tick GetCurrentTick() {

            return this.GetState().tick;

        }

        partial void PlayPlugin1ForTickPre(Tick tick) {
            
            if (this.statesHistoryModule != null) this.statesHistoryModule.PlayEventsForTickPre(tick);
            
        }

        partial void PlayPlugin1ForTickPost(Tick tick) {
            
            if (this.statesHistoryModule != null) this.statesHistoryModule.PlayEventsForTickPost(tick);
            
        }

        /*void IWorldBase.Simulate(double time) {

            if (this.statesHistoryModule != null) {

                this.timeSinceStart = time;
                this.statesHistoryModule.Simulate(this.GetCurrentTick(), this.statesHistoryModule.GetTickByTime(time));
                
            }

        }

        void IWorldBase.Simulate(Tick toTick) {

            if (this.statesHistoryModule != null) {

                this.timeSinceStart = toTick * this.tickTime;
                this.statesHistoryModule.Simulate(this.GetCurrentTick(), toTick);
                
            }

        }*/

    }

}

namespace ME.ECS.StatesHistory {

    [System.Serializable]
#if MESSAGE_PACK_SUPPORT
    [MessagePack.MessagePackObjectAttribute]
#endif
    public struct HistoryStorage {

#if MESSAGE_PACK_SUPPORT
        [MessagePack.KeyAttribute(0)]
#endif
        public HistoryEvent[] events;

    }

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    [System.Serializable]
#if MESSAGE_PACK_SUPPORT
    [MessagePack.MessagePackObjectAttribute]
#endif
    public struct HistoryEvent {

        // Header
        /// <summary>
        /// Event tick
        /// </summary>
#if MESSAGE_PACK_SUPPORT
        [MessagePack.KeyAttribute(0)]
#endif
        [ME.ECS.Serializer.Attributes.OrderAttribute]
        public long tick;
        /// <summary>
        /// Global event order (for example: you have 30 players on the map, each has it's own index)
        /// </summary>
#if MESSAGE_PACK_SUPPORT
        [MessagePack.KeyAttribute(1)]
#endif
        [ME.ECS.Serializer.Attributes.OrderAttribute]
        public int order;
        /// <summary>
        /// Rpc Id is a method Id (see NetworkModule::RegisterRPC) 
        /// </summary>
#if MESSAGE_PACK_SUPPORT
        [MessagePack.KeyAttribute(5)]
#endif
        [ME.ECS.Serializer.Attributes.OrderAttribute]
        public int rpcId;
        /// <summary>
        /// Local event order (order would be the first, then localOrder applies)
        /// </summary>
#if MESSAGE_PACK_SUPPORT
        [MessagePack.KeyAttribute(2)]
#endif
        [ME.ECS.Serializer.Attributes.OrderAttribute]
        public int localOrder;
        
        // Data
        /// <summary>
        /// Object Id to be called on (see NetworkModule::RegisterObject)
        /// </summary>
#if MESSAGE_PACK_SUPPORT
        [MessagePack.KeyAttribute(3)]
#endif
        [ME.ECS.Serializer.Attributes.OrderAttribute]
        public int objId;
        /// <summary>
        /// Group Id of objects (see NetworkModule::RegisterObject).
        /// One object could be registered in different groups at the same time.
        /// 0 by default (Common group)
        /// </summary>
#if MESSAGE_PACK_SUPPORT
        [MessagePack.KeyAttribute(4)]
#endif
        [ME.ECS.Serializer.Attributes.OrderAttribute]
        public int groupId;
        
#if MESSAGE_PACK_SUPPORT
        [MessagePack.KeyAttribute(7)]
#endif
        [ME.ECS.Serializer.Attributes.OrderAttribute]
        public bool storeInHistory;

        /// <summary>
        /// Parameters of method
        /// </summary>
#if MESSAGE_PACK_SUPPORT
        [MessagePack.KeyAttribute(6)]
#endif
        [ME.ECS.Serializer.Attributes.OrderAttribute]
        public object[] parameters;

        public override string ToString() {
            
            return "Tick: " + this.tick + ", Order: " + this.order + ", LocalOrder: " + this.localOrder + ", objId: " + this.objId + ", groupId: " + this.groupId + ", rpcId: " + this.rpcId + ", parameters: " + (this.parameters != null ? this.parameters.Length : 0);
            
        }
        
        public bool IsEqualsTo(HistoryEvent historyEvent) {

            return this.tick == historyEvent.tick &&
                   this.order == historyEvent.order &&
                   this.localOrder == historyEvent.localOrder &&
                   this.groupId == historyEvent.groupId &&
                   this.objId == historyEvent.objId &&
                   this.rpcId == historyEvent.rpcId &&
                   this.storeInHistory == historyEvent.storeInHistory &&
                   ((historyEvent.parameters == null && this.parameters == null) || (this.parameters != null && historyEvent.parameters != null && this.parameters.Length == historyEvent.parameters.Length));

        }

    }

    public interface IEventRunner {

        void RunEvent(HistoryEvent historyEvent);

    }

    public interface IStatesHistoryModuleBase : IModuleBase {

        void PauseStoreStateSinceTick(Tick tick);
        void ResumeStoreState();

        SortedDictionary<Tick, Dictionary<int, int>> GetSyncHashTable();
        
        Tick GetCacheSize();
        Tick GetTicksPerState();
        
        Tick GetLastSavedTick();
        
        uint GetEventForwardTick();
        uint GetEventForwardReceiveTick();

        void BeginAddEvents();
        void EndAddEvents();

        void InvalidateEntriesAfterTick(Tick tick);
        
        void HardResetTo(Tick tick);

        void GetResultEntries(List<ME.ECS.Network.ResultEntry<State>> states);
        
        State GetOldestState();
        State GetLatestState();
        HistoryStorage GetHistoryStorage();
        HistoryStorage GetHistoryStorage(Tick from, Tick to);
        
        System.Collections.IDictionary GetData();
        
        void PlayEventsForTickPre(Tick tick);
        void PlayEventsForTickPost(Tick tick);
        void RunEvent(HistoryEvent historyEvent);

        void SetEventRunner(IEventRunner eventRunner);
        
        void SetSyncHash(int orderId, Tick tick, int hash);

        int GetEventsAddedCount();
        int GetEventsPlayedCount();

        int GetStateHash(State state);

        void Reset();
        void AddEvents(IList<HistoryEvent> historyEvents);
        void AddEvent(HistoryEvent historyEvent);
        void CancelEvent(HistoryEvent historyEvent);
        void CancelEvents(Tick from, Tick to);

        HistoryEvent[] GetEvents();
        
        void RecalculateFromResetState();
        State GetStateBeforeTick(Tick tick);

    }

    public interface IStatesHistoryModule<TState> : IStatesHistoryModuleBase, IModule where TState : State, new() {

        public void GetEntries(List<TState> states);
        public void GetResultEntries(List<ME.ECS.Network.ResultEntry<TState>> states);

        Tick GetAndResetOldestTick(Tick tick);
        void SetLastSavedTick(Tick tick);
        
        Tick GetTickByTime(double seconds);
        TState GetStateBeforeTick(Tick tick, out Tick targetTick, bool lookupAll = false);

        void SetPlayersCount(int count);

    }

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public abstract class StatesHistoryModule<TState> : IStatesHistoryModule<TState>, IUpdate, IModuleValidation where TState : State, new() {

        private const int OLDEST_TICK_THRESHOLD = 1;
        
        private const int POOL_EVENTS_CAPACITY = 1000;
        private const int POOL_HISTORY_CAPACITY = 2;
        private const int POOL_SYNCHASH_CAPACITY = 10;

        private const uint DEFAULT_QUEUE_CAPACITY = 10u;
        private const uint DEFAULT_TICKS_PER_STATE = 100u;
        
        private ME.ECS.Network.StatesHistoryStorage<TState> statesHistory;
        private Dictionary<Tick, ME.ECS.Collections.SortedList<long, HistoryEvent>> events;
        private SortedDictionary<Tick, Dictionary<int, int>> syncHashTable;
        
        private bool prewarmed;
        //private int beginAddEventsCount;
        //private bool beginAddEvents;
        private int statEventsAdded;
        private int statPlayedEvents;
        private Tick oldestTick;
        private Tick lastSavedStateTick;
        private Tick pauseStoreStateSinceTick;
        
        private IEventRunner eventRunner;
        public World world { get; set; }

        public System.Action<Tick> onRemoteHashFail;
        public System.Action<Tick> gotValidState;

        private int playersCount;
        
        public virtual void OnConstruct() {

            this.oldestTick = Tick.Invalid;
            this.lastSavedStateTick = Tick.Invalid;
            this.pauseStoreStateSinceTick = Tick.Invalid;
            
            this.prewarmed = false;
            //this.beginAddEventsCount = 0;
            //this.beginAddEvents = false;
            this.statEventsAdded = 0;
            this.statPlayedEvents = 0;
            
            this.statesHistory = new ME.ECS.Network.StatesHistoryStorage<TState>(this.world, this.GetQueueCapacity());
            this.events = PoolDictionary<Tick, ME.ECS.Collections.SortedList<long, HistoryEvent>>.Spawn(StatesHistoryModule<TState>.POOL_EVENTS_CAPACITY);
            this.syncHashTable = new SortedDictionary<Tick, Dictionary<int, int>>();
            
            this.world.SetStatesHistoryModule(this);

        }

        public virtual void OnDeconstruct() {

            this.eventRunner = default;

            this.prewarmed = false;
            //this.beginAddEventsCount = 0;
            //this.beginAddEvents = false;
            this.statEventsAdded = 0;
            this.statPlayedEvents = 0;
            this.oldestTick = Tick.Invalid;
            this.lastSavedStateTick = Tick.Invalid;
            this.pauseStoreStateSinceTick = Tick.Invalid;
            
            this.statesHistory.DiscardAll();
            this.statesHistory = default;
            
            this.world.SetStatesHistoryModule(null);
            
            foreach (var item in this.events) {

                var values = item.Value.Values;
                for (int i = 0, cnt = values.Count; i < cnt; ++i) {
                    
                    var val = values[i];
                    if (val.parameters != null) PoolArray<object>.Recycle(ref val.parameters);

                }
                item.Value.Clear();
                PoolSortedList<long, HistoryEvent>.Recycle(item.Value);

            }
            PoolDictionary<Tick, ME.ECS.Collections.SortedList<long, HistoryEvent>>.Recycle(ref this.events);

            foreach (var kv in this.syncHashTable) {
                
                PoolDictionary<int, int>.Recycle(kv.Value);
                
            }
            this.syncHashTable.Clear();

        }

        uint IStatesHistoryModuleBase.GetEventForwardTick() {

            var next = this.GetTicksForInput();
            if (next <= 0) next = 1;
            return next;

        }

        uint IStatesHistoryModuleBase.GetEventForwardReceiveTick() {

            return this.GetTicksForReceive();

        }
        
        void IStatesHistoryModuleBase.SetEventRunner(IEventRunner eventRunner) {

            this.eventRunner = eventRunner;

        }

        HistoryStorage IStatesHistoryModuleBase.GetHistoryStorage() {

            return ((IStatesHistoryModuleBase)this).GetHistoryStorage(Tick.Invalid, Tick.Invalid);

        }

        public State GetOldestState() {

            return this.statesHistory.GetOldestState();

        }

        public State GetLatestState() {

            return this.statesHistory.GetLatestState();

        }

        Tick IStatesHistoryModuleBase.GetTicksPerState() {

            return this.GetTicksPerState();

        }
        
        public virtual Tick GetCacheSize() {

            return this.GetQueueCapacity() * this.GetTicksPerState() * 4;

        }

        /// <summary>
        /// Returns all events by tick range [from..to] (including from and to)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        HistoryStorage IStatesHistoryModuleBase.GetHistoryStorage(Tick from, Tick to) {

            var list = PoolListCopyable<HistoryEvent>.Spawn(100);
            foreach (var data in this.events) {

                var tick = data.Key;
                if ((from != Tick.Invalid && tick < from) ||
                    (to != Tick.Invalid && tick > to)) {
                    
                    continue;
                    
                }

                var values = data.Value.Values;
                for (int i = 0, cnt = values.Count; i < cnt; ++i) {

                    var evt = values[i];
                    if (evt.storeInHistory == true) {

                        list.Add(evt);

                    }

                }

            }

            var storage = new HistoryStorage();
            storage.events = list.ToArray();
            PoolListCopyable<HistoryEvent>.Recycle(ref list);
            return storage;

        }

        /// <summary>
        /// Here you can set up history states capacity
        /// </summary>
        /// <returns></returns>
        protected virtual uint GetQueueCapacity() {

            return StatesHistoryModule<TState>.DEFAULT_QUEUE_CAPACITY;

        }

        /// <summary>
        /// System will copy current state every N ticks
        /// </summary>
        /// <returns></returns>
        protected virtual uint GetTicksPerState() {

            return StatesHistoryModule<TState>.DEFAULT_TICKS_PER_STATE;

        }

        /// <summary>
        /// Event should run on N ticks forward (after receive it from server)
        /// </summary>
        /// <returns></returns>
        protected virtual uint GetTicksForReceive() {
            
            return Tick.Zero;
            
        }

        /// <summary>
        /// Event should run on N ticks forward (before send it to server)
        /// </summary>
        /// <returns></returns>
        protected virtual uint GetTicksForInput() {

            return Tick.One;

        }

        public int GetEventsAddedCount() {

            return this.statEventsAdded;

        }

        public int GetEventsPlayedCount() {

            return this.statPlayedEvents;

        }

        public void ResetEventsPlayedCount() {

            this.statPlayedEvents = 0;

        }

        public void BeginAddEvents() {

            //this.beginAddEventsCount = 0;
            //this.beginAddEventsTick = this.currentTick;
            //this.beginAddEvents = true;

        }

        public void EndAddEvents() {

            /*if (this.beginAddEvents == true && this.beginAddEventsCount > 0) {
                
                //this.Simulate(this.beginAddEventsTick, this.currentTick);

                var module = this.world.GetModule<ME.ECS.Network.NetworkModule<TState>>();
                var st = this.GetStateBeforeTick(this.oldestTick, out var syncTick);
                if (st == null || syncTick == Tick.Invalid) st = this.world.GetResetState<TState>();

                if (st.tick > module.syncedTick) {

                    module.syncedTick = st.tick;
                    module.syncHash = this.GetStateHash(st);

                }

            }*/
            
            //this.beginAddEvents = false;
            
        }

        public void AddEvents(IList<HistoryEvent> historyEvents) {
            
            this.BeginAddEvents();
            for (int i = 0, count = historyEvents.Count; i < count; ++i) this.AddEvent(historyEvents[i]);
            this.EndAddEvents();
            
        }

        public bool HasEvent(HistoryEvent historyEvent) {

            ME.ECS.Collections.SortedList<long, HistoryEvent> list;
            if (this.events.TryGetValue(historyEvent.tick, out list) == true) {

                var key = MathUtils.GetKey(historyEvent.order, historyEvent.localOrder);
                return list.ContainsKey(key);
                
            }

            return false;

        }

        public void RecalculateFromResetState() {

            this.statesHistory.DiscardAllAndReinitialize(this.world);
            
            var targetTick = this.world.GetCurrentTick();
            this.world.RewindTo(Tick.Zero, doVisualUpdate: false);
            this.world.RewindTo(targetTick, doVisualUpdate: true);
            
        }
        
        public void Reset() {

            this.oldestTick = Tick.Invalid;
            this.lastSavedStateTick = Tick.Invalid;

            foreach (var item in this.syncHashTable) {
                
                PoolDictionary<int, int>.Recycle(item.Value);
                
            }
            this.syncHashTable.Clear();
            this.InvalidateEntriesAfterTick(Tick.Invalid);
            this.statesHistory.DiscardAllAndReinitialize(this.world);
            
            foreach (var item in this.events) {
                
                PoolSortedList<long, HistoryEvent>.Recycle(item.Value);
                
            }
            this.events.Clear();
            
        }

        public void HardResetTo(Tick tick) {

            this.oldestTick = tick;
            if (this.oldestTick < Tick.Zero) this.oldestTick = Tick.Invalid;

        }

        public HistoryEvent[] GetEvents() {

            var count = 0;
            foreach (var item in this.events) {

                foreach (var k in item.Value.Values) {

                    ++count;

                }
                
            }
            
            var arr = new HistoryEvent[count];
            count = 0;
            foreach (var item in this.events) {

                foreach (var k in item.Value.Values) {

                    arr[count++] = k;

                }
                
            }
            return arr;

        }
        
        public void AddEvent(HistoryEvent historyEvent) {

            if (historyEvent.tick <= Tick.Zero) {

                // Tick fix if it is zero
                historyEvent.tick = Tick.One;

            }

            if (this.HasEvent(historyEvent) == true) {

                using (NoStackTrace.All) {
                    UnityEngine.Debug.LogWarning($"Duplicate event: {historyEvent}. Skip.");
                }
                return;
                
            }
            
            ++this.statEventsAdded;
            
            this.ValidatePrewarm();

            var key = MathUtils.GetKey(historyEvent.order, historyEvent.localOrder);
            if (this.events.TryGetValue(historyEvent.tick, out var list) == true) {
                
                list.Add(key, historyEvent);

            } else {

                list = PoolSortedList<long, HistoryEvent>.Spawn(StatesHistoryModule<TState>.POOL_HISTORY_CAPACITY);
                list.Add(key, historyEvent);
                this.events.Add(historyEvent.tick, list);

            }

            this.oldestTick = (this.oldestTick == Tick.Invalid || historyEvent.tick < this.oldestTick ? (Tick)historyEvent.tick : this.oldestTick);
            
            //++this.beginAddEventsCount;
            
            /*
            if (this.currentTick >= historyEvent.tick) {

                if (this.beginAddEvents == false) {

                    this.Simulate(historyEvent.tick, this.currentTick);

                } else {

                    if (this.beginAddEventsTick > historyEvent.tick) {

                        this.beginAddEventsTick = historyEvent.tick;

                    }

                }

            }*/

        }

        /// <summary>
        /// Remove all events from [tick..to)
        /// </summary>
        /// <param name="from">Include</param>
        /// <param name="to">Exclude</param>
        public void CancelEvents(Tick from, Tick to) {

            for (var tick = from; tick < to; ++tick) {

                ME.ECS.Collections.SortedList<long, HistoryEvent> list;
                if (this.events.TryGetValue(tick, out list) == true) {

                    var keys = PoolList<long>.Spawn(list.Count);
                    foreach (var evt in list) {

                        keys.Add(evt.Key);

                    }

                    for (int i = 0; i < keys.Count; ++i) {

                        if (list.Remove(keys[i]) == true) {
                            
                            --this.statEventsAdded;
                            this.oldestTick = (this.oldestTick == Tick.Invalid || tick < this.oldestTick ? tick : this.oldestTick);

                        }
                        
                    }
                    PoolList<long>.Recycle(ref keys);
                    
                }
                
            }
            
        }

        public void CancelEvent(HistoryEvent historyEvent) {

            if (historyEvent.storeInHistory == false) {

                return;

            }

            if (historyEvent.tick <= Tick.Zero) {

                // Tick fix if it is zero
                historyEvent.tick = Tick.One;

            }

            ME.ECS.Collections.SortedList<long, HistoryEvent> list;
            if (this.events.TryGetValue(historyEvent.tick, out list) == true) {

                var key = MathUtils.GetKey(historyEvent.order, historyEvent.localOrder);
                if (list.Remove(key) == true) {

                    --this.statEventsAdded;
                    this.oldestTick = (this.oldestTick == Tick.Invalid || historyEvent.tick < this.oldestTick ? (Tick)historyEvent.tick : this.oldestTick);
                    
                }

            }

        }

        public SortedDictionary<Tick, Dictionary<int, int>> GetSyncHashTable() {

            return this.syncHashTable;

        }
        
        public void SetSyncHash(int orderId, Tick tick, int hash) {

            //UnityEngine.Debug.Log("SetSyncHash: " + orderId + " :: " + tick + " :: " + hash);
            Dictionary<int, int> dic;
            if (this.syncHashTable.TryGetValue(tick, out dic) == false) {

                dic = PoolDictionary<int, int>.Spawn(4);
                this.syncHashTable.Add(tick, dic);
                
            }
            
            if (dic.ContainsKey(orderId) == true) {

                dic[orderId] = hash;

            } else {
                    
                dic.Add(orderId, hash);
                    
            }
            
            this.CleanUpHashTable(tick - this.GetCacheSize());
            
        }

        private void CleanUpHashTable(Tick beforeTick) {

            var list = PoolList<Tick>.Spawn(this.syncHashTable.Count);
            foreach (var kv in this.syncHashTable) {

                if (kv.Key <= beforeTick) {
                    
                    list.Add(kv.Key);
                    
                }
                
            }

            for (int i = 0; i < list.Count; ++i) {

                var key = list[i];
                var arr = this.syncHashTable[key];
                PoolDictionary<int, int>.Recycle(arr);
                this.syncHashTable.Remove(key);

            }
            
            PoolList<Tick>.Recycle(ref list);
            
        }
        
        private void CheckHash(Tick currentTick) {

            var greatestValidTick = Tick.Invalid;

            // syncHashTable is in Desc order, so we can notify each checked hash as valid
            foreach (var sync in this.syncHashTable) {

                var tick = sync.Key;
                var dic = sync.Value;
                if (dic.Count > 0) {

                    var hash = 0;
                    foreach (var kv in dic) {
                        
                        var remoteHash = kv.Value;
                        if (hash != 0 && hash != remoteHash) {
                    
                            var orderId = kv.Key;
                            using (NoStackTrace.All) {
                                
                                this.onRemoteHashFail?.Invoke(tick);
                            
                                UnityEngine.Debug.LogError($"[World #{this.world.id}] Remote Hash (Client Id: {orderId}): {tick}:{remoteHash}, Local Hash: {tick}:{hash}");
                                this.CleanUpHashTable(currentTick);
                                return;

                            }

                        }

                        hash = remoteHash;
                        
                    }
                    
                }

                if (this.playersCount > 0 && dic.Count == this.playersCount) {

                    // all client's hashes got & they are the same, so tell that state is valid
                    this.gotValidState?.Invoke(tick);

                    if (tick > greatestValidTick) {
                        greatestValidTick = tick;
                    }

                }

            }

            if (greatestValidTick != Tick.Invalid) this.CleanUpHashTable(greatestValidTick);
            
        }

        public int GetStateHash(State state) {

            return state.GetHash();

        }

        public virtual void InvalidateEntriesAfterTick(Tick tick) {
            
            this.statesHistory.InvalidateEntriesAfterTick(tick);
            this.SetLastSavedTick(tick);
            
        }

        public void SetLastSavedTick(Tick tick) {
            
            this.lastSavedStateTick = tick;
            
        }

        public Tick GetLastSavedTick() {

            return this.lastSavedStateTick;

        }

        public virtual State GetStateBeforeTick(Tick tick) {

            this.ValidatePrewarm();

            if (this.statesHistory.FindClosestEntry(tick, out var state, out _, lookupAll: true) == true) {

                return state;

            }

            return default;

        }

        public virtual TState GetStateBeforeTick(Tick tick, out Tick targetTick, bool lookupAll = false) {

            this.ValidatePrewarm();

            if (this.statesHistory.FindClosestEntry(tick, out var state, out targetTick, lookupAll) == true) {

                return state;

            }

            return default;

        }

        public Tick GetTickByTime(double seconds) {

            var tick = (seconds / (float)this.world.GetTickTime());
            return (Tick)math.floor((float)tick);

        }

        public virtual bool CanBeAdded() {

            return this.world.GetTickTime() > 0f;

        }

        public void Update(in float deltaTime) {

            this.ValidatePrewarm();
            
            /*var tick = this.GetTickByTime(this.world.GetTimeSinceStart());
            //if (tick != this.currentTick) {

                this.currentTick = tick;
                
            //}
            
            state.tick = this.currentTick;*/
            
        }

        public Tick GetAndResetOldestTick(Tick tick) {

            if (tick % StatesHistoryModule<TState>.OLDEST_TICK_THRESHOLD != 0L) return Tick.Invalid;

            var result = this.oldestTick;
            this.oldestTick = Tick.Invalid;
            return result;

        }

        public virtual void PlayEventsForTickPre(Tick tick) {
            
            if (tick > this.lastSavedStateTick && tick > Tick.Zero && tick % this.GetTicksPerState() == 0L) {

                this.StoreState(tick);
                this.SetLastSavedTick(tick);

            }

            if (this.events.TryGetValue(tick, out var list) == true) {

                var values = list.Values;
                for (int i = 0, count = values.Count; i < count; ++i) {

                    this.RunEvent(values[i]);

                }

            }

            this.CheckHash(tick);

        }

        public void PlayEventsForTickPost(Tick tick) {

        }

        public virtual void GetEntries(List<TState> states) {
            this.statesHistory.GetEntries(states);
        }

        public void GetResultEntries(List<ME.ECS.Network.ResultEntry<TState>> states) {
            this.statesHistory.GetResultEntries(states);
        }

        public void GetResultEntries(List<ME.ECS.Network.ResultEntry<State>> states) {
            this.statesHistory.GetResultEntries(states);
        }

        public System.Collections.IDictionary GetData() {

            return this.events;

        }

        public void RunEvent(HistoryEvent historyEvent) {

            if (historyEvent.storeInHistory == true) {
                
                ++this.statPlayedEvents;
                //UnityEngine.Debug.LogError("Run event tick: " + historyEvent.tick + ", method: " + historyEvent.localOrder + ", currentTick: " + this.world.currentState.tick);
                
            }
            
            if (this.eventRunner != null) this.eventRunner.RunEvent(historyEvent);
            
        }

        private void ValidatePrewarm() {
            
            if (this.prewarmed == false) {
                
                this.Prewarm(this.world.GetCurrentTick());
                this.prewarmed = true;

            }

        }

        private void Prewarm(Tick tick) {

            //this.states.BeginSet();
            for (uint i = 0; i < this.GetQueueCapacity(); ++i) {
                
                this.StoreState(((uint)tick + 1u + i) * this.GetTicksPerState(), isPrewarm: true);
                
            }
            
            this.statesHistory.InvalidateEntriesAfterTick(tick);
            //this.states.EndSet();

        }

        public void PauseStoreStateSinceTick(Tick tick) {
            
            this.pauseStoreStateSinceTick = tick;
            
        }

        public void ResumeStoreState() {
            
            this.pauseStoreStateSinceTick = Tick.Invalid;
            
        }

        private void StoreState(Tick tick, bool isPrewarm = false) {

            if (tick < this.pauseStoreStateSinceTick) {

                return;

            }
            
            /*if (isPrewarm == false) {
                
                UnityEngine.Debug.LogWarning("StoreState: " + this.world.id + ", tick: " + tick);
                var state = this.world.GetState();
                //UnityEngine.Debug.Log("State tick: " + state.tick);
                //UnityEngine.Debug.Log("State entityId: " + state.entityId);
                //UnityEngine.Debug.Log("State random: " + state.randomState);
                UnityEngine.Debug.Log("State: " + state.ToString());
                
            }*/

            {
                //var newState = WorldUtilities.CreateState<TState>();
                //newState.Initialize(this.world, freeze: true, restore: false);
                /*var state = this.world.GetState();
                newState.CopyFrom(state);
                newState.tick = tick;
                this.states.Set(tick, newState);*/
                
                var overwritedStateTick = this.statesHistory.Store(tick, this.world.GetState<TState>(), out var overwritedStateHash);
                if (isPrewarm == false && overwritedStateHash > 0) {

                    var module = this.world.GetModule<ME.ECS.Network.NetworkModule<TState>>();
                    if (module != null && module.IsReverting() == false && overwritedStateTick > module.syncedTick) {
    
                        module.syncedTick = overwritedStateTick;
                        module.syncHash = overwritedStateHash;
                        
#if NETWORK_SYNC_QUEUE_SUPPORT
                        module.syncedTickQueue.Enqueue(overwritedStateTick);
                        module.syncHashQueue.Enqueue(overwritedStateHash);
#endif
    
                    }

                }

            }
            
        }

        public void SetPlayersCount(int count) {

            this.playersCount = count;

        }

    }

}
#endif