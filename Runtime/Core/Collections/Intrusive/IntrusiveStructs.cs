namespace ME.ECS.Collections {

    public static class IntrusiveComponents {

        public static void Initialize() {
            
            WorldUtilities.InitComponentTypeId<IntrusiveData>();
            ComponentInitializer.Init(ref Worlds.currentWorld.GetStructComponents());
            
        }

        private static class ComponentInitializer {

            public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {

                structComponentsContainer.Validate<IntrusiveData>(false);

            }

        }
        
    }
    
    public struct IntrusiveData : IComponent {

        public Entity root;
        public Entity head;
        public int count;

    }

    public struct IntrusiveHashSetBucket : IComponent {

        public IntrusiveList list;

    }

    public struct IntrusiveHashSetData : IComponent {

        public StackArray10<Entity> buckets;
        public int count;

    }

    public struct IntrusiveHashSetBucketGeneric<T> : IComponent where T : struct, System.IEquatable<T> {

        public IntrusiveListGeneric<T> list;

    }

    public struct IntrusiveListNode : IComponent {

        public Entity next;
        public Entity prev;
        public Entity data;

    }

    public struct IntrusiveListGenericNode<T> : IComponent {

        public Entity next;
        public Entity prev;
        public T data;

    }

    public struct IntrusiveRingBufferGenericData<T> : IComponent where T : struct, System.IEquatable<T> {

        public IntrusiveListGeneric<T> list;
        public int capacity;

    }

    public struct IntrusiveSortedListGenericNode<T> : IComponent {

        public Entity next;
        public Entity prev;
        public T data;

    }

    public struct IntrusiveSortedListData : IComponent {

        public bool descending;

    }

}