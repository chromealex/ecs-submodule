namespace ME.ECS.Essentials.GOAP {
    
    using ME.ECS.Collections;
    using Unity.Collections;

    [System.Serializable]
    public struct EffectsData {

        [FilterDataTypesFoldoutAttribute(false)]
        [Description("Data that entity should have after this action has been complete.")]
        public FilterDataTypesOptional filter;

    }

    public struct EffectBuilder {

        internal ListCopyable<int> with;
        internal ListCopyable<int> without;

        public EffectBuilder With<T>() where T : struct, IComponent {

            this.Validate();
            this.with.Add(AllComponentTypes<T>.typeId);
            return this;

        }

        public EffectBuilder Without<T>() where T : struct, IComponent {

            this.Validate();
            this.without.Add(AllComponentTypes<T>.typeId);
            return this;

        }

        public Effect Push(Allocator allocator) {

            var result = new Effect() {
                hasComponents = new SpanArray<int>(this.with, allocator),
                hasNoComponents = new SpanArray<int>(this.without, allocator),
            };
            this.Dispose();
            return result;
            
        }

        private void Dispose() {
            
            if (this.with != null) PoolListCopyable<int>.Recycle(ref this.with);
            if (this.without != null) PoolListCopyable<int>.Recycle(ref this.without);
            
        }
        
        internal EffectBuilder Validate() {

            if (this.with == null) this.with = PoolListCopyable<int>.Spawn(10);
            if (this.without == null) this.without = PoolListCopyable<int>.Spawn(10);
            return this;

        }

    }

    public struct Effect {
        
        internal SpanArray<int> hasComponents;
        internal SpanArray<int> hasNoComponents;

        public Effect(Effect other, Allocator allocator) {
            
            this.hasComponents = new SpanArray<int>(other.hasComponents, allocator);
            this.hasNoComponents = new SpanArray<int>(other.hasNoComponents, allocator);
            
        }

        public static EffectBuilder CreateFromData(EffectsData data) {

            var builder = Effect.Create();
            foreach (var component in data.filter.with) {

                if (ComponentTypesRegistry.allTypeId.TryGetValue(component.data.GetType(), out var index) == true) {

                    builder.with.Add(index);

                }

            }

            foreach (var component in data.filter.without) {

                if (ComponentTypesRegistry.allTypeId.TryGetValue(component.data.GetType(), out var index) == true) {

                    builder.without.Add(index);

                }

            }
            
            return builder;

        }

        public static EffectBuilder Create() {
            
            return new EffectBuilder().Validate();
            
        }
        
        internal bool HasAny(Condition conditions) {

            for (int i = 0; i < this.hasComponents.Length; ++i) {

                for (int j = 0; j < conditions.hasComponents.Length; ++j) {

                    if (this.hasComponents[i] == conditions.hasComponents[j].typeId) return true;

                }

            }

            return false;

        }

        internal bool Has(int component) {

            for (int i = 0; i < this.hasComponents.Length; ++i) {

                if (this.hasComponents[i] == component) return true;

            }

            return false;

        }

        internal void Dispose() {
            this.hasComponents.Dispose();
            this.hasNoComponents.Dispose();
        }

    }

}