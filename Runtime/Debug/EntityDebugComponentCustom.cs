using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.DebugUtils {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class EntityDebugComponentCustom : MonoBehaviour {

        public int id;
        public int generation;

        [ContextMenu("Get Entity")]
        public void GetEntity() {

            var comp = this.gameObject.AddComponent<EntityDebugComponent>();
            comp.entity = new Entity(this.id, (ushort)this.generation);
            comp.world = Worlds.current;

        }

    }

}
