using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.BlackBox {

    public class BoxLinkAttribute : PropertyAttribute {

        public string caption;

        public BoxLinkAttribute(string caption = null) {

            this.caption = caption;

        }

    }

}