using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.BlackBox {
    
    [CreateAssetMenu(menuName = "ME.ECS/BlackBox/Container")]
    public class Container : FeatureBase {

        #region Construct
        private sealed class InnerSystem : ISystem, IAdvanceTick {

            public System.Action<float> onExecute;

            public World world { get; set; }
        
            public void OnConstruct() {}
            public void OnDeconstruct() {}

            public void AdvanceTick(in float deltaTime) {
                
                this.onExecute.Invoke(deltaTime);
                
            }

        }
        
        protected override void OnConstruct() {

            this.systemGroup.AddSystem(new InnerSystem() { onExecute = this.Execute });
            this.OnCreate();

        }

        protected override void OnDeconstruct() {
            
            
            
        }
        #endregion

        [HideInInspector]
        public Blueprint blueprint;
        
        public FilterDataTypes inputFilter;
        private Filter inputFilterData;

        public void OnCreate() {

            Filter.CreateFromData(this.inputFilter).Push(ref this.inputFilterData);

            this.blueprint.OnCreate();
            
        }
        
        public void Execute(float deltaTime) {

            foreach (var entity in this.inputFilterData) {

                this.blueprint.Execute(in entity, deltaTime);

            }
            
        }
        
    }
    
}