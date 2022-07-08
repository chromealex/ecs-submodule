namespace ME.ECS {

    public static class E {

        [System.Diagnostics.Conditional("WORLD_STATE_CHECK")]
        public static void IS_NOT_LOGIC_STEP(World world) {
            
            if (world.isActive == true && world.HasStep(WorldStep.LogicTick) == true) {
                
                OutOfStateException.ThrowWorldStateCheckVisual();
                
            }
            
        }

        [System.Diagnostics.Conditional("WORLD_STATE_CHECK")]
        public static void IS_LOGIC_STEP(World world) {
            
            if (world.isActive == true && world.HasStep(WorldStep.LogicTick) == false && world.HasResetState() == true) {

                OutOfStateException.ThrowWorldStateCheck();
                
            }
            
        }

        [System.Diagnostics.Conditional("WORLD_EXCEPTIONS")]
        public static void IS_TAG<TComponent>(in Entity entity) {
            
            if (AllComponentTypes<TComponent>.isTag == true) {

                TagComponentException.Throw(entity);

            }

        }

        [System.Diagnostics.Conditional("WORLD_EXCEPTIONS")]
        public static void IS_ALIVE(in Entity entity) {
            
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            
        }

        [System.Diagnostics.Conditional("WORLD_THREAD_CHECK")]
        public static void IS_WORLD_THREAD([System.Runtime.CompilerServices.CallerMemberName] string methodName = "") {
            
            if (WorldUtilities.IsWorldThread() == false) {

                WrongThreadException.Throw(methodName);
                
            }
            
        }

    }
    
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

    public class EmptyDataException : System.Exception {

        private EmptyDataException(Entity entity) : base("[ME.ECS] You are trying to read null data.") {}

        public static void Throw(Entity entity) {

            throw new EmptyDataException(entity);

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

    public class TagComponentException : System.Exception {

        private TagComponentException() : base("[ME.ECS] You are trying to read tag component.") {}

        private TagComponentException(Entity entity) : base("[ME.ECS] You are trying to read tag component: " + entity) {}

        public static void Throw() {

            throw new TagComponentException();

        }

        public static void Throw(Entity entity) {

            if (entity.generation == 0) TagComponentException.Throw();
            
            throw new TagComponentException(entity);

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

        public static void ThrowWorldStateCheckVisual() {

            throw new OutOfStateException("Visual state is required. You can disable this check by turning off WORLD_STATE_CHECK define.");

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