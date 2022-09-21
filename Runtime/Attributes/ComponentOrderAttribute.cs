namespace ME.ECS {

    public class ComponentOrderAttribute : System.Attribute {

        public int order;

        public ComponentOrderAttribute(int order) {

            this.order = order;

        }

    }

}