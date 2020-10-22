namespace ME.ECS {

    public class ComponentHelpAttribute : System.Attribute {

        public string comment;

        public ComponentHelpAttribute(string comment) {

            this.comment = comment;

        }

    }

}