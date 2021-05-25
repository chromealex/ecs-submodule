
namespace ME.ECSBurst {
    
    using Collections;

    public struct SystemAdvanceTick {

        public void AdvanceTick(float deltaTime) {}

    }

    public unsafe struct World {

        public StateStruct resetState;
        public NativeBufferArray<System.IntPtr> advanceTickSystems;
        public NativeBufferArray<System.IntPtr> updateInputSystems;
        public NativeBufferArray<System.IntPtr> updateVisualSystems;

        public void Update(float deltaTime) {

            for (int i = 0; i < this.advanceTickSystems.Length; ++i) {

                var adv = this.advanceTickSystems[i];
                //System.Runtime.InteropServices.Marshal.GetComInterfaceForObject<SystemAdvanceTick, IAdvanceTick>();
                ref var advTick = ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<SystemAdvanceTick>((void*)adv);
                advTick.AdvanceTick(deltaTime);

            }
            
        }
        
        public void AddSystem<T>(T system) where T : struct {

            var ptr = (System.IntPtr)Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AddressOf(ref system);
            if (system is IAdvanceTick) {
                
                ArrayUtils.Resize(this.advanceTickSystems.Length, ref this.advanceTickSystems);
                this.advanceTickSystems[this.advanceTickSystems.Length - 1] = ptr;

            }

            if (system is IUpdateInput) {
                
                ArrayUtils.Resize(this.updateInputSystems.Length, ref this.updateInputSystems);
                this.updateInputSystems[this.updateInputSystems.Length - 1] = ptr;

            }

            if (system is IUpdateVisual) {
                
                ArrayUtils.Resize(this.updateVisualSystems.Length, ref this.updateVisualSystems);
                this.updateVisualSystems[this.updateVisualSystems.Length - 1] = ptr;

            }

        }

    }

    public interface IAdvanceTick {

        void AdvanceTick(float deltaTime);

    }

    public interface IUpdateInput {

        void UpdateInput(float deltaTime);

    }

    public interface IUpdateVisual {

        void UpdateVisual(float deltaTime);

    }

}
