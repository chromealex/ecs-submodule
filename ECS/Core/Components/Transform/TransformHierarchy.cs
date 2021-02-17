#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Transform;

    public static class ECSTransformHierarchy {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void OnEntityDestroy(in Entity entity) {

            if (entity.HasData<Childs>() == true) {

                ref var childs = ref entity.GetData<Childs>();
                childs.childs.Clear(destroyData: true);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetParent(this in Entity child, in Entity root) {

            child.SetParent(in root, worldPositionStays: true);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetParent(this in Entity child, in Entity root, bool worldPositionStays) {

            if (worldPositionStays == true) {

                var pos = child.GetPosition();
                var rot = child.GetRotation();
                ECSTransformHierarchy.SetParent_INTERNAL(in child, in root);
                child.SetPosition(pos);
                child.SetRotation(rot);

            } else {

                ECSTransformHierarchy.SetParent_INTERNAL(in child, in root);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private static void SetParent_INTERNAL(in Entity child, in Entity root) {

            if (child == root) return;

            ref var container = ref child.GetData<Container>();
            if (root == Entity.Empty) {

                ref var childs = ref container.entity.GetData<Childs>();
                childs.childs.Remove(child);
                child.RemoveData<Container>();
                return;

            }

            if (container.entity == root || root.IsAlive() == false) {

                return;

            }

            if (ECSTransformHierarchy.FindInHierarchy(in child, in root) == true) return;

            if (container.entity.IsAlive() == true) {

                child.SetParent(Entity.Empty);

            }

            container.entity = root;
            ref var rootChilds = ref root.GetData<Childs>();
            rootChilds.childs.Add(child);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity GetRoot(this in Entity child) {

            var root = child;
            var container = child;
            do {

                root = container;
                container = container.GetData<Container>(false).entity;

            } while (container.IsAlive() == true);

            return root;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private static bool FindInHierarchy(in Entity child, in Entity root) {

            ref var childChilds = ref child.GetData<Childs>(createIfNotExists: false);
            if (childChilds.childs.Contains(root) == true) {

                return true;

            }

            foreach (var cc in childChilds.childs) {

                if (ECSTransformHierarchy.FindInHierarchy(in cc, in root) == true) return true;

            }

            return false;

        }

    }

}