#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if STATES_HISTORY_MODULE_SUPPORT && NETWORK_MODULE_SUPPORT

namespace ME.ECS {

    public partial interface IWorldBase {

        void SetNetworkModule(Network.INetworkModuleBase module);

    }

    public partial class World {

        private Network.INetworkModuleBase networkModule;

        public void SetNetworkModule(Network.INetworkModuleBase module) {

            this.networkModule = module;

        }

    }

}

namespace ME.ECS.Network {

    [System.Flags]
    public enum NetworkType : byte {

        None = 0x0,
        SendToNet = 0x1,
        RunLocal = 0x2,

    }

    public interface ITransporter {

        bool IsConnected();
        void Send(byte[] bytes);
        void SendSystem(byte[] bytes);
        byte[] Receive();

        int GetEventsSentCount();
        int GetEventsBytesSentCount();
        int GetEventsReceivedCount();
        int GetEventsBytesReceivedCount();


    }

    public interface ISerializer {

        ME.ECS.StatesHistory.HistoryStorage DeserializeStorage(byte[] bytes);
        byte[] SerializeStorage(ME.ECS.StatesHistory.HistoryStorage historyStorage);

        byte[] Serialize(StatesHistory.HistoryEvent historyEvent);
        StatesHistory.HistoryEvent Deserialize(byte[] bytes);

        byte[] SerializeWorld(World.WorldState data);
        World.WorldState DeserializeWorld(byte[] bytes);

    }

    public interface INetworkModuleBase : IModuleBase {

        ME.ECS.StatesHistory.HistoryEvent GetCurrentHistoryEvent();

        void SetAsyncMode(bool state);
        void SetReplayMode(bool state);

        void LoadHistoryStorage(ME.ECS.StatesHistory.HistoryStorage storage);

        void SetTransporter(ITransporter transporter);
        void SetSerializer(ISerializer serializer);
        ISerializer GetSerializer();

        int GetRPCOrder();

        bool IsReverting();

        bool UnRegisterRPC(RPCId rpcId);

        RPCId RegisterRPC(System.Reflection.MethodInfo methodInfo, bool runLocalOnly = false);
        bool RegisterRPC(RPCId rpcId, System.Reflection.MethodInfo methodInfo, bool runLocalOnly = false);

        bool RegisterObject(object obj, int objId = 0, int groupId = 0);
        bool UnRegisterObject(object obj, int objId);
        bool UnRegisterGroup(int groupId);

        int GetRegistryCount();

        double GetPing();

        int GetEventsSentCount();
        int GetEventsBytesSentCount();
        int GetEventsReceivedCount();
        int GetEventsBytesReceivedCount();

        Tick GetSyncTick();
        Tick GetSyncSentTick();

        void RPC<T1>(object instance, RPCId rpcId, T1 p1);
        void RPC<T1, T2>(object instance, RPCId rpcId, T1 p1, T2 p2);
        void RPC<T1, T2, T3>(object instance, RPCId rpcId, T1 p1, T2 p2, T3 p3);
        void RPC<T1, T2, T3, T4>(object instance, RPCId rpcId, T1 p1, T2 p2, T3 p3, T4 p4);
        void RPC<T1, T2, T3, T4, T5>(object instance, RPCId rpcId, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5);

    }

    public interface INetworkModule<TState> : INetworkModuleBase, IModule where TState : State, new() { }

    public struct Key {

        public int objId;
        public int groupId;

    }

    public class RegisterObjectMissingException : System.Exception {

        public RegisterObjectMissingException(object instance, RPCId rpcId) : base("[NetworkModule] Object " + instance + " could not send RPC with id " + rpcId +
                                                                                   " because RegisterObject() call should run before this call.") { }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract class NetworkModule<TState> : INetworkModule<TState>, IUpdatePreLate, IUpdatePost, StatesHistory.IEventRunner, IModuleValidation where TState : State, new() {

        private static readonly RPCId CANCEL_EVENT_RPC_ID = -11;
        private static readonly RPCId PING_RPC_ID = -1;
        private static readonly RPCId SYNC_RPC_ID = -2;

        public World world { get; set; }

        private RPCId rpcId;
        internal System.Collections.Generic.Dictionary<int, System.Reflection.MethodInfo> registry;
        private System.Collections.Generic.HashSet<int> runLocalOnly;
        private System.Collections.Generic.Dictionary<long, object> keyToObjects;
        private System.Collections.Generic.Dictionary<object, Key> objectToKey;
        private int currentObjectRegistryId;

        private StatesHistory.IStatesHistoryModule<TState> statesHistoryModule;
        protected ITransporter transporter { get; private set; }
        protected ISerializer serializer { get; private set; }
        private int localOrderIndex;

        private double ping;

        private float pingTime;
        private float syncTime;
        internal Tick syncedTick;
        internal int syncHash;
        private Tick syncTickSent;

        private bool isReverting;
        private Tick revertingTo;

        private bool asyncMode;
        private bool replayMode;

        void IModuleBase.OnConstruct() {

            this.replayMode = false;
            this.asyncMode = false;

            this.localOrderIndex = 0;
            this.rpcId = 0;
            this.ping = 0d;

            this.pingTime = 0f;
            this.syncTime = 0f;
            this.syncedTick = 0;
            this.syncHash = 0;
            this.syncTickSent = 0;
            this.revertingTo = 0;
            this.isReverting = false;

            this.registry = PoolDictionary<int, System.Reflection.MethodInfo>.Spawn(100);
            this.objectToKey = PoolDictionary<object, Key>.Spawn(100);
            this.keyToObjects = PoolDictionary<long, object>.Spawn(100);
            this.runLocalOnly = PoolHashSet<int>.Spawn(100);
            this.currentObjectRegistryId = 1000;

            this.statesHistoryModule = this.world.GetModule<StatesHistory.IStatesHistoryModule<TState>>();
            this.statesHistoryModule.SetEventRunner(this);

            this.world.SetNetworkModule(this);

            this.RegisterObject(this, -1, -1);
            this.RegisterRPC(NetworkModule<TState>.CANCEL_EVENT_RPC_ID, new System.Action<byte[]>(this.CancelEvent_RPC).Method);
            this.RegisterRPC(NetworkModule<TState>.PING_RPC_ID, new System.Action<double, bool>(this.Ping_RPC).Method);
            this.RegisterRPC(NetworkModule<TState>.SYNC_RPC_ID, new System.Action<Tick, int>(this.Sync_RPC).Method);
            
            this.OnInitialize();

        }

        void IModuleBase.OnDeconstruct() {

            this.OnDeInitialize();

            this.replayMode = default;
            this.asyncMode = default;

            this.localOrderIndex = default;
            this.rpcId = default;
            this.ping = default;

            this.pingTime = default;
            this.syncTime = default;
            this.syncedTick = default;
            this.syncHash = default;
            this.syncTickSent = default;
            this.revertingTo = default;
            this.isReverting = default;
            
            this.currentObjectRegistryId = default;
            
            this.world.SetNetworkModule(null);
            this.statesHistoryModule = null;

            this.UnRegisterObject(this, -1);
            this.currentObjectRegistryId = 1000;

            PoolHashSet<int>.Recycle(ref this.runLocalOnly);
            PoolDictionary<long, object>.Recycle(ref this.keyToObjects);
            PoolDictionary<object, Key>.Recycle(ref this.objectToKey);
            PoolDictionary<int, System.Reflection.MethodInfo>.Recycle(ref this.registry);

        }

        public ME.ECS.Network.ISerializer GetSerializer() {

            return this.serializer;

        }

        public double GetPing() {

            return this.ping;

        }

        private void CancelEvent_RPC(byte[] array) {

            try {

                var cancelEvent = this.serializer.Deserialize(array);
                this.CancelEvent(cancelEvent);

            } catch (System.Exception exception) {

                UnityEngine.Debug.LogException(exception);

            }

        }

        private void Sync_RPC(Tick tick, int hash) {

            this.statesHistoryModule.SetSyncHash(this.GetCurrentHistoryEvent().order, tick, hash);

        }

        private void Ping_RPC(double t, bool forward) {

            if (forward == true) {

                this.SystemRPC(this, NetworkModule<TState>.PING_RPC_ID, t, false);

            } else {

                // Measure ping client to client
                var dt = this.world.GetTimeSinceStart() - t;
                this.ping = dt;
                //UnityEngine.Debug.Log(this.world.id + ", ping c2c: " + dt + "secs");

            }

        }

        protected virtual void OnInitialize() { }

        protected virtual void OnDeInitialize() { }

        void INetworkModuleBase.SetTransporter(ITransporter transporter) {

            this.transporter = transporter;

        }

        void INetworkModuleBase.SetSerializer(ISerializer serializer) {

            this.serializer = serializer;

        }

        public int GetEventsSentCount() {

            if (this.transporter == null) return 0;
            return this.transporter.GetEventsSentCount();

        }

        public int GetEventsBytesSentCount() {

            if (this.transporter == null) return 0;
            return this.transporter.GetEventsBytesSentCount();

        }

        public int GetEventsReceivedCount() {

            if (this.transporter == null) return 0;
            return this.transporter.GetEventsReceivedCount();

        }

        public int GetEventsBytesReceivedCount() {

            if (this.transporter == null) return 0;
            return this.transporter.GetEventsBytesReceivedCount();

        }

        public int GetRegistryCount() {

            return this.registry.Count;

        }

        public virtual bool CouldBeAdded() {

            return this.world.GetTickTime() > 0f && this.world.GetModule<StatesHistory.IStatesHistoryModule<TState>>() != null;

        }

        public bool RegisterObject(object obj, int objId = 0, int groupId = 0) {

            if (objId == 0) objId = this.currentObjectRegistryId + 1;

            var key = MathUtils.GetKey(groupId, objId);
            if (this.keyToObjects.ContainsKey(key) == false) {

                ++this.currentObjectRegistryId;
                this.keyToObjects.Add(key, obj);
                this.objectToKey.Add(obj, new Key() { objId = objId, groupId = groupId });
                return true;

            }

            return false;

        }

        public bool UnRegisterObject(object obj, int objId) {

            foreach (var item in this.objectToKey) {

                if (item.Key == obj) {

                    var keyData = item.Value;
                    if (objId == keyData.objId) {

                        var key = MathUtils.GetKey(keyData.groupId, keyData.objId);
                        var found = this.keyToObjects.Remove(key);
                        if (found == true) {

                            this.objectToKey.Remove(obj);
                            return true;

                        }

                    }

                }

            }

            return false;

        }

        public bool UnRegisterGroup(int groupId) {

            var foundAny = false;
            var newObjectToKey = PoolDictionary<object, Key>.Spawn(100);
            foreach (var item in this.objectToKey) {

                if (item.Value.groupId == groupId) {

                    var keyData = item.Value;
                    var key = MathUtils.GetKey(keyData.groupId, keyData.objId);
                    var foundInside = false;
                    object obj;
                    if (this.keyToObjects.TryGetValue(key, out obj) == true) {

                        var found = this.keyToObjects.Remove(key);
                        if (found == true) {

                            foundInside = true;
                            foundAny = true;

                        }

                    }

                    if (foundInside == false) newObjectToKey.Add(item.Key, item.Value);

                }

            }

            PoolDictionary<object, Key>.Recycle(ref this.objectToKey);
            this.objectToKey = newObjectToKey;

            return foundAny;

        }

        public bool UnRegisterRPC(RPCId rpcId) {

            return this.registry.Remove(rpcId);

        }

        int INetworkModuleBase.GetRPCOrder() {

            return this.GetRPCOrder();

        }

        protected virtual int GetRPCOrder() {

            return 0;

        }

        protected virtual NetworkType GetNetworkType() {

            return NetworkType.RunLocal | NetworkType.SendToNet;

        }

        private void CallRPC(object instance, RPCId rpcId, bool storeInHistory, object[] parameters) {

            if (this.world.HasStep(WorldStep.LogicTick) == true) {

                InStateException.ThrowWorldStateCheck();

            }

            if (this.objectToKey.TryGetValue(instance, out var key) == true) {

                var evt = new ME.ECS.StatesHistory.HistoryEvent();
                evt.tick = this.world.GetStateTick() + this.statesHistoryModule.GetEventForwardTick(); // Call RPC on next N tick
                evt.parameters = parameters;
                evt.rpcId = rpcId;
                evt.objId = key.objId;
                evt.groupId = key.groupId;
                evt.order = this.GetRPCOrder();

                // If event run only on local machine
                // we need to write data through all states and run this event immediately on each
                // then return current state back
                // so we don't need to store this event in history, we just need to rewrite reset data
                var runLocalOnly = this.runLocalOnly.Contains(rpcId);
                if (runLocalOnly == true) {

                    { // Apply data to current state
                        this.statesHistoryModule.RunEvent(evt);
                    }

                    var currentState = this.world.GetState();
                    var resetState = this.world.GetResetState();
                    this.world.SetStateDirect(resetState);
                    {
                        this.statesHistoryModule.RunEvent(evt);
                    }

                    foreach (var entry in this.statesHistoryModule.GetDataStates().GetEntries()) {

                        if (entry.isEmpty == false) {

                            this.world.SetStateDirect(entry.state);
                            this.statesHistoryModule.RunEvent(evt);

                        }

                    }

                    this.world.SetStateDirect(currentState);
                    return;

                }

                // Set up other event data
                evt.localOrder = ++this.localOrderIndex;
                evt.storeInHistory = storeInHistory;

                var storedInHistory = false;
                if (this.GetNetworkType() == NetworkType.RunLocal && storeInHistory == true) {

                    this.statesHistoryModule.AddEvent(evt);
                    storedInHistory = true;

                }

                if (storedInHistory == false && storeInHistory == true && (this.GetNetworkType() & NetworkType.RunLocal) != 0) {

                    //var dEvt = this.serializer.Deserialize(this.serializer.Serialize(evt));
                    this.statesHistoryModule.AddEvent(evt);
                    storedInHistory = true;

                }

                if (this.transporter != null && this.transporter.IsConnected() == true) {

                    if (runLocalOnly == false && (this.GetNetworkType() & NetworkType.SendToNet) != 0) {

                        if (this.transporter != null && this.serializer != null) {

                            if (storeInHistory == false) {

                                this.transporter.SendSystem(this.serializer.Serialize(evt));

                            } else {

                                this.transporter.Send(this.serializer.Serialize(evt));

                            }

                        }

                    }

                }

                if (storedInHistory == false && parameters != null) {

                    // Return parameters into pool if we are not storing them locally
                    PoolArray<object>.Recycle(ref parameters);

                }

            } else {

                throw new RegisterObjectMissingException(instance, rpcId);

            }

        }

        public System.Reflection.MethodInfo GetMethodInfo(RPCId rpcId) {

            System.Reflection.MethodInfo methodInfo;
            if (this.registry.TryGetValue(rpcId, out methodInfo) == true) {

                return methodInfo;

            }

            return null;

        }

        private ME.ECS.StatesHistory.HistoryEvent runCurrentEvent;

        void StatesHistory.IEventRunner.RunEvent(StatesHistory.HistoryEvent historyEvent) {

            System.Reflection.MethodInfo methodInfo;
            if (this.registry.TryGetValue(historyEvent.rpcId, out methodInfo) == true) {

                var key = MathUtils.GetKey(historyEvent.groupId, historyEvent.objId);
                if (this.keyToObjects.TryGetValue(key, out var instance) == true) {

                    this.runCurrentEvent = historyEvent;
                    methodInfo.Invoke(instance, historyEvent.parameters);
                    this.runCurrentEvent = default;

                }

            }

        }

        public ME.ECS.StatesHistory.HistoryEvent GetCurrentHistoryEvent() {

            return this.runCurrentEvent;

        }

        public Tick GetSyncTick() {

            return this.syncedTick;

        }

        public Tick GetSyncSentTick() {

            return this.syncTickSent;

        }

        void INetworkModuleBase.LoadHistoryStorage(ME.ECS.StatesHistory.HistoryStorage storage) {

            this.statesHistoryModule.BeginAddEvents();
            foreach (var item in storage.events) {

                this.ApplyEvent(item);

            }

            this.statesHistoryModule.EndAddEvents();

        }

        private bool ApplyEvent(ME.ECS.StatesHistory.HistoryEvent historyEvent) {

            /*if (historyEvent.storeInHistory == true) {
                        
                System.Reflection.MethodInfo methodInfo;
                if (this.registry.TryGetValue(historyEvent.rpcId, out methodInfo) == true) {

                    UnityEngine.Debug.LogWarning("Received. evt.objId: " + historyEvent.objId + ", evt.rpcId: " + historyEvent.rpcId + ", evt.order: " + historyEvent.order + ", method: " + methodInfo.Name);

                }

            }*/

            if ((this.GetNetworkType() & NetworkType.RunLocal) != 0 && historyEvent.order == this.GetRPCOrder()) {

                // Skip events from local owner is it was run already
                //UnityEngine.Debug.LogWarning("Skipped event: " + historyEvent.objId + ", " + historyEvent.rpcId);
                return false;

            }

            if (historyEvent.storeInHistory == true) {

                // Run event normally on certain tick
                this.statesHistoryModule.AddEvent(historyEvent);

            } else {

                // Run event immediately
                this.statesHistoryModule.RunEvent(historyEvent);

            }

            return true;

        }

        protected void CancelEvent(ME.ECS.StatesHistory.HistoryEvent historyEvent) {

            this.statesHistoryModule.CancelEvent(historyEvent);

        }

        protected virtual void SendPing(float deltaTime) {

            this.pingTime += deltaTime;
            if (this.pingTime >= 1f) {

                this.SystemRPC(this, NetworkModule<TState>.PING_RPC_ID, this.world.GetTimeSinceStart(), true);
                this.pingTime -= 1f;

            }

        }

        protected virtual void SendSync(float deltaTime) {

            this.syncTime += deltaTime;
            if (this.syncTime >= 2f) {

                if (this.syncTickSent != this.syncedTick) {

                    this.SystemRPC(this, NetworkModule<TState>.SYNC_RPC_ID, this.syncedTick, this.syncHash);
                    this.syncTickSent = this.syncedTick;

                }

                this.syncTime -= 2f;

            }

        }

        protected virtual void ReceiveEventsAndApply() {

            if (this.transporter != null && this.serializer != null) {

                this.statesHistoryModule.BeginAddEvents();
                byte[] bytes = null;
                do {

                    bytes = this.transporter.Receive();
                    if (bytes == null) break;
                    if (bytes.Length == 0) continue;

                    var evt = this.serializer.Deserialize(bytes);
                    this.ApplyEvent(evt);

                } while (true);

                this.statesHistoryModule.EndAddEvents();

            }

        }

        public bool IsReverting() {

            return this.isReverting == true && this.world.GetCurrentTick() < this.revertingTo;

        }

        public void SetAsyncMode(bool state) {
            
            this.asyncMode = state;
            
        }

        public void SetReplayMode(bool state) {
            
            this.replayMode = state;
            
        }

        protected virtual void OnRevertingBegin(Tick sourceTick) {}
        protected virtual void OnRevertingEnd() {}

        protected virtual void ApplyTicksByState() {

            var tick = this.world.GetCurrentTick();

            var timeSinceGameStart = (long)(this.world.GetTimeSinceStart() * 1000L);
            var targetTick = (Tick)System.Math.Floor(timeSinceGameStart / (this.world.GetTickTime() * 1000d));
            var oldestEventTick = this.statesHistoryModule.GetAndResetOldestTick(tick);
            //UnityEngine.Debug.LogError("Tick: " + tick + ", timeSinceGameStart: " + timeSinceGameStart + ", targetTick: " + targetTick + ", oldestEventTick: " + oldestEventTick);
            if (oldestEventTick == Tick.Invalid || oldestEventTick >= tick) {

                // No events found
                this.world.SetFromToTicks(tick, targetTick);
                return;

            }

            if (oldestEventTick > targetTick) oldestEventTick = targetTick;

            var sourceState = this.statesHistoryModule.GetStateBeforeTick(oldestEventTick, out var sourceTick, lookupAll: this.replayMode);
            if (sourceState == null) {

                if (this.replayMode == true && (oldestEventTick == Tick.Invalid || oldestEventTick >= tick)) {

                    this.world.SetFromToTicks(tick, targetTick);
                    return;

                }
                sourceState = this.world.GetResetState<TState>();

            }
            //UnityEngine.Debug.LogWarning("Rollback. Oldest: " + oldestEventTick + ", sourceTick: " + sourceTick + " (hash: " + sourceState.GetHash() + " rnd: " + sourceState.randomState + "), targetTick: " + targetTick + ", currentTick: " + tick + ", timeSinceGameStart: " + timeSinceGameStart);
            
            this.statesHistoryModule.InvalidateEntriesAfterTick(sourceTick);

            /*
            if (this.replayMode == false) {
                
                this.statesHistoryModule.InvalidateEntriesAfterTick(sourceTick);
                
            } else {

                this.statesHistoryModule.SetLastSavedTick(sourceTick);

            }*/

            this.OnRevertingBegin(sourceTick);
            // Applying old state.
            this.isReverting = true;
            {
                var currentState = this.world.GetState();
                this.revertingTo = tick;
                currentState.CopyFrom(sourceState);
                currentState.Initialize(this.world, freeze: false, restore: true);
                if (this.asyncMode == false) this.world.Simulate(sourceTick, tick, 0f);
            }
            this.isReverting = false;
            this.OnRevertingEnd();

            if (this.asyncMode == true) {
                
                this.world.SetFromToTicks(sourceTick, targetTick);
                
            } else {

                this.world.SetFromToTicks(tick, targetTick);

            }

        }

        public virtual void UpdatePost(in float deltaTime) {
            
            if (this.GetNetworkType() != NetworkType.RunLocal) {

                this.SendPing(deltaTime);
                this.SendSync(deltaTime);

            }

        }

        public virtual void UpdatePreLate(in float deltaTime) {

            this.ReceiveEventsAndApply();
            this.ApplyTicksByState();

        }

        public RPCId RegisterRPC(System.Reflection.MethodInfo methodInfo, bool runLocalOnly = false) {

            if (this.registry.ContainsValue(methodInfo) == true) {

                foreach (var reg in this.registry) {

                    if (reg.Value == methodInfo) {

                        return reg.Key;

                    }

                }

            }

            this.RegisterRPC(++this.rpcId, methodInfo, runLocalOnly);
            return this.rpcId;

        }

        public bool RegisterRPC(RPCId rpcId, System.Reflection.MethodInfo methodInfo, bool runLocalOnly = false) {

            if (this.registry.ContainsKey(rpcId) == false) {

                this.registry.Add(rpcId, methodInfo);
                if (runLocalOnly == true) this.runLocalOnly.Add(rpcId);
                return true;

            }

            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SystemRPC(object instance, RPCId rpcId, params object[] parameters) {

            this.CallRPC(instance, rpcId, false, parameters);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RPC(object instance, RPCId rpcId) {

            this.CallRPC(instance, rpcId, true, null);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RPC<T1>(object instance, RPCId rpcId, T1 p1) /*where T1 : struct*/ {

            var arr = new object[1];
            arr[0] = p1;
            this.CallRPC(instance, rpcId, true, arr);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RPC<T1, T2>(object instance, RPCId rpcId, T1 p1, T2 p2) /*where T1 : struct where T2 : struct*/ {

            var arr = new object[2];
            arr[0] = p1;
            arr[1] = p2;
            this.CallRPC(instance, rpcId, true, arr);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RPC<T1, T2, T3>(object instance, RPCId rpcId, T1 p1, T2 p2, T3 p3) /*where T1 : struct where T2 : struct where T3 : struct*/ {

            var arr = new object[3];
            arr[0] = p1;
            arr[1] = p2;
            arr[2] = p3;
            this.CallRPC(instance, rpcId, true, arr);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RPC<T1, T2, T3, T4>(object instance, RPCId rpcId, T1 p1, T2 p2, T3 p3, T4 p4) /*where T1 : struct where T2 : struct where T3 : struct where T4 : struct*/ {

            var arr = new object[4];
            arr[0] = p1;
            arr[1] = p2;
            arr[2] = p3;
            arr[3] = p4;
            this.CallRPC(instance, rpcId, true, arr);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void
            RPC<T1, T2, T3, T4, T5>(object instance, RPCId rpcId, T1 p1, T2 p2, T3 p3, T4 p4,
                                    T5 p5) /*where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct*/ {

            var arr = new object[5];
            arr[0] = p1;
            arr[1] = p2;
            arr[2] = p3;
            arr[3] = p4;
            arr[4] = p5;
            this.CallRPC(instance, rpcId, true, arr);

        }

    }

}
#endif