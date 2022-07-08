#if FIXED_POINT_MATH
using math = ME.ECS.Mathematics.math;
using float3 = ME.ECS.Mathematics.float3;
using tfloat = sfloat;
#else
using math = Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;
using tfloat = System.Single;
#endif

namespace ME.ECS.Essentials.GOAP {

    using Collections;
    
    public enum PathStatus {

        NotCalculated,
        Processing,
        Failed,
        Success,

    }

    public struct Plan {

        public PathStatus planStatus;
        public tfloat cost;
        public BufferArray<Action> actions;

        public void Dispose() {

            this.planStatus = PathStatus.NotCalculated;
            PoolArray<Action>.Recycle(ref this.actions);
            
        }

        public override string ToString() {

            var str = "Plan ";
            tfloat cost = 0f;
            foreach (var action in this.actions) {
                cost += action.data.cost;
            }

            str += " Cost(" + cost + "): ";

            foreach (var action in this.actions) {
                str += " => [" + action.data.id + "]";
            }

            return str;

        }

    }

}