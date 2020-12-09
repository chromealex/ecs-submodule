using System;
using System.Runtime.CompilerServices;

namespace ME.ECS.Serializer.Attributes {

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class OrderAttribute : Attribute {

        public int order;

        public OrderAttribute([CallerLineNumber] int order = 0) {
            
            this.order = order;
            
        }

    }

}