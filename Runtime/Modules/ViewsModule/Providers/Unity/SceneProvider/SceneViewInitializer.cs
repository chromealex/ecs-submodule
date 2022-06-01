using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Views.Providers {

    public interface ISceneView {

        void Initialize(World world);

    }

    public abstract class SceneViewInitializer : MonoBehaviour, ISceneView {
        
        public EntityFlag entityFlags;
        public ME.ECS.DataConfigs.DataConfig applyDataConfig;
        public DestroyViewBehaviour destroyViewBehaviour;
        
        void ISceneView.Initialize(World world) { 
            
            var entity = new Entity(this.entityFlags);
            if (this.applyDataConfig != null) this.applyDataConfig.Apply(entity);

            this.OnInitialize(world, in entity);
            
        }

        protected abstract void OnInitialize(World world, in Entity entity);

    }

}