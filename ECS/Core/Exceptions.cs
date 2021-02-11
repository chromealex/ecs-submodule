namespace ME.ECS {

    public class OutOfBoundsException : System.Exception {

        public OutOfBoundsException() : base("ME.ECS Exception") { }
        public OutOfBoundsException(string message) : base(message) { }

    }
    
    public class StateNotFoundException : System.Exception {

        public StateNotFoundException() : base("ME.ECS Exception") { }
        public StateNotFoundException(string message) : base(message) { }

    }

    public class ViewSourceIsNullException : System.Exception {

        public ViewSourceIsNullException() : base("ME.ECS Exception") { }
        public ViewSourceIsNullException(string message) : base(message) { }

        public static void Throw() {

            throw new ViewSourceIsNullException("Prefab you want to use is null.");

        }
        
    }

    public class DeprecatedException : System.Exception {

        public DeprecatedException() : base("ME.ECS Exception") { }
        public DeprecatedException(string message) : base(message) { }
        
        public static void Throw() {

            throw new DeprecatedException("Deprecated");

        }

    }

    public class WrongThreadException : System.Exception {

        public WrongThreadException() : base("ME.ECS Exception") { }
        public WrongThreadException(string message) : base(message) { }

        public static void Throw(string methodName, string description = null) {

            throw new WrongThreadException("Can't use " + methodName + " method from non-world thread" + (string.IsNullOrEmpty(description) == true ? string.Empty : ", " + description) + ".\nTurn off this check by disabling WORLD_THREAD_CHECK.");

        }
        
    }

    public class EmptyEntityException : System.Exception {

        private EmptyEntityException() : base("[ME.ECS] You are trying to change empty entity.") {}

        private EmptyEntityException(Entity entity) : base("[ME.ECS] You are trying to change entity that has been destroyed already: " + entity) {}

        public static void Throw() {

            throw new EmptyEntityException();

        }

        public static void Throw(Entity entity) {

            if (entity.generation == 0) EmptyEntityException.Throw();
            
            throw new EmptyEntityException(entity);

        }

    }

    public class InStateException : System.Exception {

        public InStateException() : base("[ME.ECS] Could not perform action because current step is in state (" + Worlds.currentWorld.GetCurrentStep().ToString() + ").") {}

        public static void ThrowWorldStateCheck() {

            throw new InStateException();

        }

    }

    public class OutOfStateException : System.Exception {

        public OutOfStateException(string description = "") : base("[ME.ECS] Could not perform action because current step is out of state (" + Worlds.currentWorld.GetCurrentStep().ToString() + "). This could cause out of sync state. " + description) {}

        public static void ThrowWorldStateCheck() {

            throw new OutOfStateException("LogicTick state is required. You can disable this check by turning off WORLD_STATE_CHECK define.");

        }
        
    }

    public class AllocationException : System.Exception {

        public AllocationException() : base("Allocation not allowed!") {}

    }

    public class SystemGroupRegistryException : System.Exception {

        private SystemGroupRegistryException() {}
        private SystemGroupRegistryException(string caption) : base(caption) {}

        public static void Throw() {

            throw new SystemGroupRegistryException("SystemGroup was not registered in world. Be sure you use constructor with parameters (new SystemGroup(name)).");

        }

    }

}