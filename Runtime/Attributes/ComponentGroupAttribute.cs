using System.Reflection;

namespace ME.ECS {

    public enum GroupColor {

        Default = 0, 
        Red,
        Green,
        Yellow,
        Cyan,
        Grey,
        Magenta,
        White,

    }
    
    public class ComponentGroupAttribute : System.Attribute {

        public string name;
        public UnityEngine.Color color;
        public int order = -1;

        public ComponentGroupAttribute(System.Type referenceType) {

            var attr = referenceType.GetCustomAttribute<ComponentGroupAttribute>();
            if (attr != null) {

                this.name = attr.name;
                this.color = attr.color;
                this.order = attr.order;

            }

        }

        public ComponentGroupAttribute(string name, int order = -1) {

            this.name = name;
            this.order = order;
            this.color = default;

        }

        public ComponentGroupAttribute(string name, byte r, byte g, byte b, int order = -1) {

            this.name = name;
            this.order = -1;
            this.color = new UnityEngine.Color32(r, g, b, 0);

        }

        public ComponentGroupAttribute(string name, GroupColor color, int order = -1) {

            this.name = name;
            this.order = order;
            
            switch (color) {
                case GroupColor.Default:
                    this.color = default;
                    break;
                case GroupColor.Red:
                    this.color = UnityEngine.Color.red;
                    break;
                case GroupColor.Green:
                    this.color = UnityEngine.Color.green;
                    break;
                case GroupColor.Yellow:
                    this.color = UnityEngine.Color.yellow;
                    break;
                case GroupColor.Cyan:
                    this.color = UnityEngine.Color.cyan;
                    break;
                case GroupColor.Grey:
                    this.color = UnityEngine.Color.grey;
                    break;
                case GroupColor.Magenta:
                    this.color = UnityEngine.Color.magenta;
                    break;
                case GroupColor.White:
                    this.color = UnityEngine.Color.white;
                    break;
            }

        }

    }

}