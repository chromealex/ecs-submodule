namespace ME.ECS.Essentials.GOAP {

    using ME.ECS.Collections;
    using Unity.Collections;

    [System.Serializable]
    public struct PreconditionsData {

        public FilterDataTypesOptional filter;

    }
    
    public struct PreconditionBuilder {

        internal ListCopyable<int> with;
        internal ListCopyable<int> without;
        internal ListCopyable<byte> withData;
        internal ListCopyable<UnsafeData> withDataValues;

        public PreconditionBuilder With<T>() where T : struct, IComponent {

            this.Validate();
            this.with.Add(AllComponentTypes<T>.typeId);
            this.withData.Add(0);
            this.withDataValues.Add(default);
            return this;

        }

        public PreconditionBuilder Without<T>() where T : struct, IComponent {

            this.Validate();
            this.without.Add(AllComponentTypes<T>.typeId);
            return this;

        }

        public Precondition Push(Allocator allocator) {

            var result = new Precondition() {
                hasComponents = new SpanArray<int>(this.with, allocator),
                hasNoComponents = new SpanArray<int>(this.without, allocator),
                hasComponentsData = new SpanArray<byte>(this.withData, allocator),
                hasComponentsDataValues = new SpanArray<UnsafeData>(this.withDataValues, allocator),
            };
            this.Dispose();
            return result;
            
        }

        private void Dispose() {
            
            if (this.with != null) PoolListCopyable<int>.Recycle(ref this.with);
            if (this.without != null) PoolListCopyable<int>.Recycle(ref this.without);
            if (this.withData != null) PoolListCopyable<byte>.Recycle(ref this.withData);
            if (this.withDataValues != null) PoolListCopyable<UnsafeData>.Recycle(ref this.withDataValues);
            
        }
        
        internal PreconditionBuilder Validate() {

            if (this.with == null) this.with = PoolListCopyable<int>.Spawn(10);
            if (this.without == null) this.without = PoolListCopyable<int>.Spawn(10);
            if (this.withData == null) this.withData = PoolListCopyable<byte>.Spawn(10);
            if (this.withDataValues == null) this.withDataValues = PoolListCopyable<UnsafeData>.Spawn(10);
            return this;

        }

    }

    public struct Precondition {

        internal SpanArray<int> hasComponents;
        internal SpanArray<int> hasNoComponents;
        internal SpanArray<byte> hasComponentsData;
        internal SpanArray<UnsafeData> hasComponentsDataValues;

        public Precondition(Precondition other, Allocator allocator) {
            
            this.hasComponents = new SpanArray<int>(other.hasComponents, allocator);
            this.hasNoComponents = new SpanArray<int>(other.hasNoComponents, allocator);
            this.hasComponentsData = new SpanArray<byte>(other.hasComponentsData, allocator);
            this.hasComponentsDataValues = SpanArray<UnsafeData>.Copy<UnsafeDataCopy>(other.hasComponentsDataValues, allocator);
            
        }
        
        public static PreconditionBuilder CreateFromData(PreconditionsData data) {

            var builder = Precondition.Create();
            for (var i = 0; i < data.filter.with.Length; ++i) {
                
                var component = data.filter.with[i];
                var type = component.data.GetType();
                if (ComponentTypesRegistry.allTypeId.TryGetValue(type, out var index) == true) {

                    builder.with.Add(index);
                    var withData = component.optional;
                    builder.withData.Add((byte)(withData == true ? 1 : 0));
                    if (withData == true) {
                        var obj = new UnsafeData();
                        var setMethod = UnsafeData.setMethodInfo.MakeGenericMethod(type);
                        obj = (UnsafeData)setMethod.Invoke(obj, new object[] { component.data });
                        builder.withDataValues.Add(obj);
                    } else {
                        builder.withDataValues.Add(default);
                    }

                }
            }

            foreach (var component in data.filter.without) {

                if (ComponentTypesRegistry.allTypeId.TryGetValue(component.GetType(), out var index) == true) {

                    builder.without.Add(index);

                }

            }
            
            return builder;

        }

        public static PreconditionBuilder Create() {
            
            return new PreconditionBuilder().Validate();
            
        }

        internal bool HasData(NativeHashSet<UnsafeData> entityStateData) {
            
            for (int i = 0; i < this.hasComponentsData.Length; ++i) {

                if (this.hasComponentsData[i] == 1) {

                    var ptr = this.hasComponentsDataValues[i];
                    if (entityStateData.Contains(ptr) == false) {

                        return false;

                    }

                }
                
            }

            return true;

        }

        internal bool Has(NativeHashSet<int> entityState) {

            // we need to check if we have all components in hasComponents array in entityState
            for (int i = 0; i < this.hasComponents.Length; ++i) {

                var component = this.hasComponents[i];
                if (entityState.Contains(component) == false) return false;

            }

            for (int i = 0; i < this.hasNoComponents.Length; ++i) {

                var component = this.hasNoComponents[i];
                if (entityState.Contains(component) == true) return false;

            }

            return true;

        }

        internal readonly bool HasInAction(NativeArray<Action.Data> temp, in Action.Data parentAction, int component) {

            var action = parentAction;
            if (action.effects.Has(component) == true) return true;
            while (action.parent != -1) {

                action = temp[action.parent];
                if (action.effects.Has(component) == true) return true;

            }

            return false;

        }

        internal readonly bool Has(NativeArray<Action.Data> temp, in Action.Data parentAction, NativeHashSet<int> entityState) {

            // burst runtime check if we can traverse this node
            // we need to check if we have all components in hasComponents array somewhere in runtimeState or in entityState
            // and hasNoComponents not contained in entityState or in parent action's effects
            
            for (int i = 0; i < this.hasComponents.Length; ++i) {

                var component = this.hasComponents[i];
                if (entityState.Contains(component) == false &&
                    this.HasInAction(temp, in parentAction, component) == false) {

                    return false;
                    
                }

            }
            
            for (int i = 0; i < this.hasNoComponents.Length; ++i) {

                var component = this.hasNoComponents[i];
                if (entityState.Contains(component) == true ||
                    this.HasInAction(temp, in parentAction, component) == true) {

                    return false;
                    
                }

            }
            
            return true;

        }

        internal void Dispose() {
            this.hasComponents.Dispose();
            this.hasNoComponents.Dispose();
            this.hasComponentsData.Dispose();
            this.hasComponentsDataValues.Dispose();
        }

    }

}