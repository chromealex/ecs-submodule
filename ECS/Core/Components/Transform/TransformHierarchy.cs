namespace ME.ECS {
    
    using Transform;
    
    public static class ECSTransformHierarchy {

        public static void OnEntityDestroy(in Entity entity) {

            var childs = entity.GetData<Childs>(false);
            for (int i = 0; i < childs.childs.Length; ++i) {

                if (childs.childs[i].IsAlive() == true) {

                    childs.childs[i].Destroy();

                }

            }

        }
        
        public static void SetParent(this in Entity child, in Entity root, in bool worldPositionStays) {
            
            child.SetParent(root, worldPositionStays, registerChilds: true);
            
        }

        public static void SetParent(this in Entity child, in Entity root, in bool worldPositionStays, in bool registerChilds) {

            if (worldPositionStays == true) {
                
                var pos = child.GetPosition();
                var rot = child.GetRotation();
                ECSTransformHierarchy.SetParent_INTERNAL(child, root, registerChilds);
                child.SetPosition(pos);
                child.SetRotation(rot);
                
            } else {

                ECSTransformHierarchy.SetParent_INTERNAL(child, root, registerChilds);

            }

        }

        public static void SetParent(this in Entity child, in Entity root) {
            
            ECSTransformHierarchy.SetParent_INTERNAL(child, root, registerChilds: true);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void SetParent_INTERNAL(in Entity child, in Entity root, bool registerChilds) {

            if (child == root) return;
            
            var container = child.GetData<Container>(false);
            if (container.entity == root && root.IsAlive() == false) return;

            if (root.IsAlive() == false && (root.HasData<Childs>() == false || root.GetData<Childs>().childs.Length == 0)) {

                if (registerChilds == true) root.SetData(new Childs() { childs = new ME.ECS.Collections.StackArray50<Entity>(ME.ECS.Collections.StackArray50<int>.MAX_LENGTH) });

            }

            if (container.entity.IsAlive() == false) {

                if (registerChilds == true) {

                    // Remove child from previous container
                    var childs = root.GetData<Childs>();
                    for (int i = 0; i < childs.childs.Length; ++i) {

                        if (childs.childs[i] == child) {

                            childs.childs[i] = Entity.Empty;
                            break;

                        }

                    }

                    root.SetData(childs);

                }
                
            }

            if (root.IsAlive() == true) {

                child.RemoveData<Container>();
                //child.SetData(new Container() { entity = Entity.Empty });
                /*child.SetData(new TraverseGraph() {
                    graphsMask = PathfindingUtils.GetGraphMask(AstarPath.active.graphs[0] as Pathfinding.GridGraph)
                });*/

            } else {
                
                //UnityEngine.Debug.LogWarning("SET CHILD " + child + " TO ROOT " + root);
                child.SetData(new Container() { entity = root });

                if (registerChilds == true) {

                    var set = false;
                    var childs = root.GetData<Childs>();
                    for (int i = 0; i < childs.childs.Length; ++i) {

                        if (childs.childs[i].IsAlive() == true) {

                            childs.childs[i] = child;
                            set = true;
                            break;

                        }

                    }
                    root.SetData(childs);

                    if (set == false) {

                        throw new System.Exception("SetParent failed because max childs reached (" + ME.ECS.Collections.StackArray50<int>.MAX_LENGTH + ")");

                    }

                }
                
                /*child.SetData(new TraverseGraph() {
                    graphsMask = PathfindingUtils.GetGraphMask(child.GetData<Container>().entity.GetData<InnerGraph>().graphs)
                });*/

            }

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Entity GetRoot(this in Entity child) {

            var root = child;
            var container = child;
            do {

                root = container;
                container = container.GetData<Container>(false).entity;
                
            } while (container.IsAlive() == true);
            
            return root;

        }

    }

}