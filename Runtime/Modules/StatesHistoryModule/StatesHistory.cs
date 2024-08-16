namespace ME.ECS.Network {
    
    using System.Collections;
    using System.Collections.Generic;

    public interface IStatesHistoryEntry {

        Tick tick { get; }
        bool isEmpty { get; }
        T GetData<T>() where T : State;

    }

    public interface IStatesHistory {

    }

    public interface IStatesHistory<TState> : IStatesHistory where TState : State, new() {

        void GetResultEntries(List<ResultEntry<TState>> states);
        void GetEntries(List<TState> states);
        Tick Store(Tick tick, TState state, out int overwritedStateHash);
        bool FindClosestEntry(Tick maxTick, out TState state, out Tick tick, bool lookupAll = false);
        void InvalidateEntriesAfterTick(Tick tick);
        TState GetLatestState();
        TState GetOldestState();
        void DiscardAllAndReinitialize(World world);

    }

    public struct ResultEntry<T> {

        public T data;
        public bool isEmpty;

    }

    public struct StatesHistoryStorage<TState> : IStatesHistory<TState> where TState : State, new() {

        public struct Entry : IStatesHistoryEntry {
            
            public bool isEmpty { get; private set; }
            public Tick tick {
                get {
                    if (this.isEmpty == true) return Tick.Invalid;
                    return this.state.tick;
                }
            }
            public TState state;

            public Entry(World world) {
                
                this.isEmpty = true;
                this.state = WorldUtilities.CreateState<TState>();
                this.state.Initialize(world, freeze: true, restore: false);
                
            }

            public T GetData<T>() where T : State {
                
                return (T)(object)this.state;
                
            }
            
            public void SetEmpty() {
                
                this.isEmpty = true;
                
            }
            
            public Tick Store(Tick tick, TState state) {

                var overwritedTick = this.tick;
                this.state.CopyFrom(state);
                this.state.tick = tick;
                this.isEmpty = false;

                return overwritedTick;

            }
            
            public void Discard() {
                
                this.isEmpty = true;
                WorldUtilities.ReleaseState(ref this.state);
                
            }

            public override string ToString() {

                return string.Format("{0}: {1}", this.tick, this.state.ToString());

            }

        }
        
        private Entry[] entries;
        private readonly int capacity;
        private int headPointer;
        private Tick oldestTick;

        public StatesHistoryStorage(World world, uint capacity) {

            this.capacity = (int)capacity;
            this.entries = new Entry[capacity];
            this.headPointer = -1;
            this.oldestTick = Tick.Invalid;
            this.Initialize(world);

        }

        private void Initialize(World world) {

            for (var i = 0; i < this.capacity; ++i) {

                this.entries[i] = new Entry(world);

            }

            this.headPointer = 0;

        }

        public void GetEntries(List<TState> states) {
            foreach (var entry in this.entries) {
                if (entry.isEmpty == false) {
                    states.Add(entry.state);
                }
            }
        }

        public void GetResultEntries(List<ResultEntry<TState>> states) {
            foreach (var entry in this.entries) {
                states.Add(new ResultEntry<TState>() {
                    isEmpty = entry.isEmpty,
                    data = entry.state,
                });
            }
        }

        public void GetResultEntries(List<ResultEntry<State>> states) {
            foreach (var entry in this.entries) {
                states.Add(new ResultEntry<State>() {
                    isEmpty = entry.isEmpty,
                    data = entry.state,
                });
            }
        }

        public StatesHistoryStorage<TState>.Entry[] GetEntries() {
            return this.entries;
        }
        
        public Tick Store(Tick tick, TState state, out int overwritedStateHash) {

            ref var entry = ref this.GetEntry(this.headPointer);
            
            overwritedStateHash = 0;
            if (tick > entry.tick && entry.isEmpty == false) {
                
                overwritedStateHash = entry.state.GetHash();
                
            }
            var overwritedTick = entry.Store(tick, state);
            this.headPointer = this.IterateForward(this.headPointer);
            this.oldestTick = this.GetEntry(this.headPointer).tick;

            return overwritedTick;

        }

        private int IterateForward(int marker) {

            ++marker;
            if (marker >= this.capacity) {
                marker = 0;
            }

            return marker;

        }

        private int IterateBackward(int marker) {

            --marker;
            if (marker < 0) {
                marker = this.capacity - 1;
            }

            return marker;

        }

        private ref Entry GetEntry(int index) {

            if (index >= this.capacity) {
                index %= this.capacity;
            }

            return ref this.entries[index];

        }

        public bool FindClosestEntry(Tick maxTick, out TState state, out Tick tick, bool lookupAll = false) {
            
            state = null;
            tick = Tick.Invalid;

            if (this.headPointer == -1) return false;

            var marker = this.headPointer;
            marker = this.IterateBackward(marker);

            if (lookupAll == true) {

                while (marker != this.headPointer) {

                    ref var entry = ref this.GetEntry(marker);
                    if (entry.tick >= tick && entry.tick <= maxTick) {

                        state = entry.state;
                        tick = entry.tick;

                    }

                    marker = this.IterateBackward(marker);

                }
                
                return tick != Tick.Invalid;

            } else {
                
                while (marker != this.headPointer) {

                    ref var entry = ref this.GetEntry(marker);
                    if (entry.isEmpty == true) {

                        return false;

                    }
                    
                    if (entry.tick <= maxTick) {

                        state = entry.state;
                        tick = entry.tick;
                        return true;

                    }

                    marker = this.IterateBackward(marker);

                }

            }
            
            return false;
            
        }
        
        public void InvalidateEntriesAfterTick(Tick tick) {

            if (this.headPointer == -1) return;

            var prev = this.IterateBackward(this.headPointer);
            var marker = prev;

            do {

                ref var entry = ref this.GetEntry(marker);
                if (entry.tick <= tick) break;

                entry.SetEmpty();
                marker = this.IterateBackward(marker);

            } while (marker != prev);

            this.headPointer = this.IterateForward(marker);

        }

        public TState GetLatestState() {

            if (this.headPointer == -1) return null;

            TState state = null;
            var maxTick = Tick.Zero;
            foreach (var entry in this.entries) {

                if (entry.tick >= maxTick && entry.isEmpty == false) {

                    state = entry.state;
                    maxTick = entry.tick;

                }
                
            }
            
            return state;

        }

        public TState GetOldestState() {

            if (this.headPointer == -1) return null;

            TState state = null;
            var minTick = Tick.MaxValue;
            foreach (var entry in this.entries) {

                if (entry.tick < minTick && entry.isEmpty == false) {

                    state = entry.state;
                    minTick = entry.tick;

                }
                
            }
            
            return state;

        }

		public Tick GetOldestEntryTick() {

            if (this.headPointer == -1) return Tick.Invalid;

            var marker = this.headPointer;
            marker = this.IterateForward(marker);

            while (marker != this.headPointer) {

                var tick = this.GetEntry(marker).tick;
                if (tick != Tick.Invalid) return tick;

                marker = this.IterateForward(marker);

            }

            return Tick.Invalid;

        }

        public void DiscardAllAndReinitialize(World world) {

            for (int i = 0; i < this.entries.Length; ++i) {
                this.entries[i].Discard();
            } 
            
            this.headPointer = -1;
            this.oldestTick = Tick.Zero;
            this.Initialize(world);

        }

        public void DiscardAll() {

            for (int i = 0; i < this.entries.Length; ++i) {
                this.entries[i].Discard();
            } 
            
            this.headPointer = -1;
            this.oldestTick = Tick.Zero;
            
        }

    }

    /*
    public class StatesHistory<TState> : IStatesHistory<TState> where TState : State, new() {

        public class Entry : IStatesHistoryEntry {

            public bool isEmpty { get; private set; }
            public Tick tick {
                get {
                    if (this.isEmpty == true) return Tick.Invalid;
                    return this.state.tick;
                }
            }
            public TState state;

            public Entry(World world) {

                this.isEmpty = true;
                this.state = WorldUtilities.CreateState<TState>();
                this.state.Initialize(world, freeze: true, restore: false);

            }

            public void SetEmpty() {
                
                this.isEmpty = true;
                
            }

            public object GetData() {

                return this.state;

            }

            public Tick Store(Tick tick, TState state) {

                var overwritedTick = this.tick;
                this.state.CopyFrom(state);
                this.state.tick = tick;
                this.isEmpty = false;

                return overwritedTick;

            }

            public void Discard() {
                
                this.isEmpty = true;
                WorldUtilities.ReleaseState(ref this.state);
                
            }

            public override string ToString() {

                return string.Format("{0}: {1}", this.tick, this.state.ToString());

            }

        }

        private readonly LinkedList<Entry> entries = new LinkedList<Entry>();
        private LinkedListNode<Entry> currentEntryNode;
        public Tick oldestTick;
        public readonly long capacity;
        private World world;

        public StatesHistory(World world, long capacity) {

            this.world = world;
            this.capacity = capacity;
            this.Clear();

        }

        public LinkedList<Entry> GetEntries() {

            return this.entries;

        }

        public void GetEntries(List<TState> states) {
            foreach (var entry in this.entries) {
                if (entry.isEmpty == false) {
                    states.Add(entry.state);
                }
            }
        }

        public void GetResultEntries(List<ResultEntry<TState>> states) {
            foreach (var entry in this.entries) {
                states.Add(new ResultEntry<TState>() {
                    isEmpty = entry.isEmpty,
                    data = entry.state,
                });
            }
        }
        
        public void Clear() {

            this.entries.Clear();

            for (var i = 0; i < this.capacity; ++i) {

                this.entries.AddLast(new Entry(this.world));

            }

            this.currentEntryNode = this.entries.First;

        }

		public Tick Store(Tick tick, TState state, out int overwritedStateHash) {

            overwritedStateHash = 0;
            if (tick > this.currentEntryNode.Value.tick && this.currentEntryNode.Value.isEmpty == false) {
                
                overwritedStateHash = this.currentEntryNode.Value.state.GetHash();
                
            }
            var overwritedTick = this.currentEntryNode.Value.Store(tick, state);
            this.currentEntryNode = this.IterateForward(this.currentEntryNode);
            this.oldestTick = this.currentEntryNode.Value.tick;

            return overwritedTick;

        }

        public bool GetStateHash(long tick, out int hash, out long foundTick) {

            hash = 0;
            foundTick = 0L;

            if (tick <= 0L) return false;

            TState state;
            if (this.FindClosestEntry(tick, out state, out foundTick) == true) {

                if (state.tick > 0L && state.tick < tick) {

                    hash = state.GetHash();
                    return true;

                }

            }

            return false;

        }

        private LinkedListNode<Entry> IterateForward(LinkedListNode<Entry> entryNode) {

            entryNode = entryNode.Next ?? this.entries.First;

            return entryNode;

        }

        private LinkedListNode<Entry> IterateBackward(LinkedListNode<Entry> entryNode) {

            entryNode = entryNode.Previous ?? this.entries.Last;

            return entryNode;

        }


		public bool FindClosestEntry(Tick maxTick, out TState state, out Tick tick, bool lookupAll = false) {

            state = null;
            tick = Tick.Invalid;

            if (this.currentEntryNode == null) return false;
            
            var marker = this.currentEntryNode;
            marker = this.IterateBackward(marker);

            if (lookupAll == true) {

                while (marker != this.currentEntryNode) {

                    var entry = marker.Value;
                    if (entry.tick >= tick && entry.tick <= maxTick) {

                        state = entry.state;
                        tick = entry.tick;

                    }

                    marker = this.IterateBackward(marker);

                }
                
                return tick != Tick.Invalid;

            } else {
                
                while (marker != this.currentEntryNode) {

                    var entry = marker.Value;
                    if (entry.isEmpty == true) {

                        return false;

                    }
                    
                    if (entry.tick <= maxTick) {

                        state = entry.state;
                        tick = entry.tick;
                        return true;

                    }

                    marker = this.IterateBackward(marker);

                }

            }
            
            return false;

        }

		public void InvalidateEntriesAfterTick(Tick tick) {

            if (this.currentEntryNode == null) return;

            var prev = this.IterateBackward(this.currentEntryNode);
            var marker = prev;

            do {

                var entry = marker.Value;
                if (entry.tick <= tick) break;

                entry.SetEmpty();
                marker = this.IterateBackward(marker);

            } while (marker != prev);

            this.currentEntryNode = this.IterateForward(marker);

        }

        public TState GetLatestState() {

            TState state = null;
            var maxTick = Tick.Zero;
            foreach (var entry in this.entries) {

                if (entry.tick >= maxTick && entry.isEmpty == false) {

                    state = entry.state;
                    maxTick = entry.tick;

                }
                
            }
            
            return state;

        }

        public TState GetOldestState() {

            TState state = null;
            var minTick = Tick.MaxValue;
            foreach (var entry in this.entries) {

                if (entry.tick < minTick && entry.isEmpty == false) {

                    state = entry.state;
                    minTick = entry.tick;

                }
                
            }
            
            return state;

        }

		public Tick GetOldestEntryTick() {

            if (this.currentEntryNode == null) return Tick.Invalid;

            var marker = this.currentEntryNode;
            marker = this.IterateForward(marker);

            while (marker != this.currentEntryNode) {

                var tick = marker.Value.tick;
                if (tick != Tick.Invalid) return tick;

                marker = this.IterateForward(marker);

            }

            return Tick.Invalid;

        }

        public void DiscardAllAndReinitialize(World world) {

            foreach (var entry in this.entries) {

                entry.Discard();

            }

            this.entries.Clear();
            this.currentEntryNode = null;
            this.oldestTick = Tick.Zero;
            this.Clear();

        }

        public void DiscardAll() {

            foreach (var entry in this.entries) {

                entry.Discard();

            }

            this.currentEntryNode = null;
            this.oldestTick = Tick.Zero;

        }

    }*/

}