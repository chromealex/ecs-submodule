﻿namespace ME.ECS.Transform {

    [ComponentGroup(typeof(TransformComponentConstants.GroupInfo))]
    [ComponentOrder(4)]
    public struct Container : IComponent, IVersioned, IFilterConnect {

        public Entity entity;

        Entity IFilterConnect.entity => this.entity;

    }

    [ComponentGroup(typeof(TransformComponentConstants.GroupInfo))]
    [ComponentOrder(5)]
    public struct Nodes : IComponent, IVersioned, IComponentDisposable {

        public ME.ECS.Collections.IntrusiveList items;
        
        void IComponentDisposable.OnDispose() {

            this.items.Dispose();

        }

    }

}