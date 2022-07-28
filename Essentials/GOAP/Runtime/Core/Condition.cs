namespace ME.ECS.Essentials.GOAP {

    using ME.ECS.Collections;
    using Collections.V3;
    using Collections.MemoryAllocator;

    [System.Serializable]
    public struct ConditionsData {

        [FilterDataTypesFoldoutAttribute(false)]
        [Description("Data that entity should have before this action has been started.")]
        public FilterDataTypesOptional filter;

    }

    public struct ConditionBuilder {

        internal ListCopyable<FilterDataItem> with;
        internal ListCopyable<FilterDataItem> without;

        public ConditionBuilder With<T>() where T : struct, IComponent {

            this.Validate();
            this.with.Add(FilterDataItem.Create(AllComponentTypes<T>.typeId, 0, default));
            return this;

        }

        public ConditionBuilder Without<T>() where T : struct, IComponent {

            this.Validate();
            this.without.Add(FilterDataItem.Create(AllComponentTypes<T>.typeId, 0, default));
            return this;

        }

        public Condition Push(Unity.Collections.Allocator allocator) {

            var result = new Condition() {
                hasComponents = new SpanArray<FilterDataItem>(this.with, allocator),
                hasNoComponents = new SpanArray<FilterDataItem>(this.without, allocator),
            };
            this.Dispose();
            return result;
            
        }

        private void Dispose() {
            
            if (this.with != null) PoolListCopyable<FilterDataItem>.Recycle(ref this.with);
            if (this.without != null) PoolListCopyable<FilterDataItem>.Recycle(ref this.without);
            
        }
        
        internal ConditionBuilder Validate() {

            if (this.with == null) this.with = PoolListCopyable<FilterDataItem>.Spawn(10);
            if (this.without == null) this.without = PoolListCopyable<FilterDataItem>.Spawn(10);
            return this;

        }

    }

    public struct Condition {

        internal SpanArray<FilterDataItem> hasComponents;
        internal SpanArray<FilterDataItem> hasNoComponents;

        public Condition(Condition other, Unity.Collections.Allocator allocator) {
            
            this.hasComponents = new SpanArray<FilterDataItem>(other.hasComponents, allocator);
            this.hasNoComponents = new SpanArray<FilterDataItem>(other.hasNoComponents, allocator);
            
        }
        
        public static ConditionBuilder CreateFromData(ConditionsData data) {

            var builder = Condition.Create();
            for (var i = 0; i < data.filter.with.Length; ++i) {
                
                var component = data.filter.with[i];
                var type = component.data.GetType();
                if (ComponentTypesRegistry.allTypeId.TryGetValue(type, out var index) == true) {

                    var withData = component.optional;
                    UnsafeData unsafeData = default;
                    if (withData == true) {
                        var obj = new UnsafeData();
                        var setMethod = UnsafeData.setMethodInfo.MakeGenericMethod(type);
                        unsafeData = (UnsafeData)setMethod.Invoke(obj, new object[] { component.data });
                    }
                    builder.with.Add(FilterDataItem.Create(index, (byte)(withData == true ? 1 : 0), unsafeData));
                    
                }
            }

            for (var i = 0; i < data.filter.without.Length; ++i) {

                var component = data.filter.without[i];
                var type = component.data.GetType();
                if (ComponentTypesRegistry.allTypeId.TryGetValue(type, out var index) == true) {

                    var withData = component.optional;
                    UnsafeData unsafeData = default;
                    if (withData == true) {
                        var obj = new UnsafeData();
                        var setMethod = UnsafeData.setMethodInfo.MakeGenericMethod(type);
                        unsafeData = (UnsafeData)setMethod.Invoke(obj, new object[] { component.data });
                    }
                    builder.without.Add(FilterDataItem.Create(index, (byte)(withData == true ? 1 : 0), unsafeData));
                    
                }

            }
            
            return builder;

        }

        public static ConditionBuilder Create() {
            
            return new ConditionBuilder().Validate();
            
        }

        internal bool HasData(in MemoryAllocator allocator, NativeHashSet<UnsafeData> entityStateData) {
            
            for (int i = 0; i < this.hasComponents.Length; ++i) {

                if (this.hasComponents[i].hasData == 1) {

                    var ptr = this.hasComponents[i].data;
                    if (entityStateData.Contains(in allocator, ptr) == false) {

                        return false;

                    }

                }
                
            }

            return true;

        }

        internal bool HasNoData(in MemoryAllocator allocator, NativeHashSet<UnsafeData> entityStateData) {
            
            for (int i = 0; i < this.hasNoComponents.Length; ++i) {

                if (this.hasNoComponents[i].hasData == 1) {

                    var ptr = this.hasNoComponents[i].data;
                    if (entityStateData.Contains(in allocator, ptr) == true) {

                        return false;

                    }

                }
                
            }

            return true;

        }

        internal bool Has(in MemoryAllocator allocator, EquatableHashSet<int> entityState) {

            // we need to check if we have all components in hasComponents array in entityState
            for (int i = 0; i < this.hasComponents.Length; ++i) {

                var component = this.hasComponents[i].typeId;
                if (entityState.Contains(in allocator, component) == false) return false;

            }

            for (int i = 0; i < this.hasNoComponents.Length; ++i) {

                var component = this.hasNoComponents[i].typeId;
                if (entityState.Contains(in allocator, component) == true) return false;

            }

            return true;

        }

        internal readonly bool HasInAction(Unity.Collections.NativeArray<Action.Data> temp, in Action.Data parentAction, int component) {

            var action = parentAction;
            if (action.effects.Has(component) == true) return true;
            while (action.parent != -1) {

                action = temp[action.parent];
                if (action.effects.Has(component) == true) return true;

            }

            return false;

        }

        internal readonly bool Has(in MemoryAllocator allocator, Unity.Collections.NativeArray<Action.Data> temp, in Action.Data parentAction, EquatableHashSet<int> entityState) {

            // burst runtime check if we can traverse this node
            // we need to check if we have all components in hasComponents array somewhere in runtimeState or in entityState
            // and hasNoComponents not contained in entityState or in parent action's effects
            
            for (int i = 0; i < this.hasComponents.Length; ++i) {

                var component = this.hasComponents[i].typeId;
                if (entityState.Contains(in allocator, component) == false &&
                    this.HasInAction(temp, in parentAction, component) == false) {

                    return false;
                    
                }

            }
            
            for (int i = 0; i < this.hasNoComponents.Length; ++i) {

                var component = this.hasNoComponents[i].typeId;
                if (entityState.Contains(in allocator, component) == true ||
                    this.HasInAction(temp, in parentAction, component) == true) {

                    return false;
                    
                }

            }
            
            return true;

        }

        internal void Dispose() {
            this.hasComponents.Dispose();
            this.hasNoComponents.Dispose();
        }

    }

}