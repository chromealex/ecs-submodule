namespace ME.ECS.BlackBox {

    [Category("Variables")]
    public class BoxVariable : Box {

        public override float width => 120f;
        public override float padding => 3f;

        public RefType type = RefType.Undefined;
        [UnityEngine.TextAreaAttribute(1, 1)]
        public string caption;
        [UnityEngine.HideInInspector]
        public FieldData[] data;
        [UnityEngine.HideInInspector]
        public int dataIndex;

        public void Save(FieldDataContainer container, int index) {

            this.data = container.data;
            this.dataIndex = index;

        }

        public object Load() {

            var item = this.data[this.dataIndex];
            if (item.isArray == true) return item.valueArr;
            return item.value;

        }
        
        public override void OnCreate() {
            
        }
        
        public override Box Execute(in Entity entity, float deltaTime) {

            

            return null;
            
        }

    }

}