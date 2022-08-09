namespace ME.ECS {

    public abstract class ConfigBase : UnityEngine.ScriptableObject {

        public abstract void Apply(in Entity entity, bool overrideIfExist = true);

        public abstract void Prewarm(bool forced = false);

        #if !ENTITIES_GROUP_DISABLED
        public abstract void Apply(in EntitiesGroup group);
        #endif

    }

}