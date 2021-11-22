using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.BlackBox {
    
    public abstract class Box : ScriptableObject, IValidateEditor {

        public virtual float width => 300f;
        public virtual float padding => 20f;

        public abstract void OnCreate();
        
        public abstract Box Execute(in Entity entity, float deltaTime);

        public virtual void OnValidateEditor() { }

    }

    public abstract class BoxNode : Box {

        [BoxLink("Next")]
        public Box next;

    }

}