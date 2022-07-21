using System;
using System.Collections.Generic;
using System.Reflection;

namespace ME.ECS.Essentials.GOAP {

    public class SerializedReferenceCaptionAttribute : Attribute {

        public string value;

        public SerializedReferenceCaptionAttribute(string value) {
            this.value = value;
        }

    }

}