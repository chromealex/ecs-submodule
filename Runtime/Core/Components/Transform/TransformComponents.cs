namespace ME.ECS.Transform {

    public struct Container : IComponent {

        public Entity entity;

    }

    public struct Nodes : IStructCopyable<Nodes> {

        public ME.ECS.Collections.ListCopyable<Entity> items;

        public void CopyFrom(in Nodes other) {
            ArrayUtils.Copy(other.items, ref this.items);
        }

        public void OnRecycle() {
            PoolListCopyable<Entity>.Recycle(ref this.items);
        }

    }

}