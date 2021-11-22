using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.BlackBox {

    public class CategoryAttribute : System.Attribute {

        public string path;

        public CategoryAttribute(string path = null) {

            this.path = path;

        }

    }

}