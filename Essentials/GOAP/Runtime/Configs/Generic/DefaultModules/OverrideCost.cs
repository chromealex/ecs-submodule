#if FIXED_POINT_MATH
using math = ME.ECS.Mathematics.math;
using float3 = ME.ECS.Mathematics.float3;
using tfloat = sfloat;
#else
using math = Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;
using tfloat = System.Single;
#endif

namespace ME.ECS.Essentials.GOAP.DefaultModules {

	public interface ITargetCost : IComponentBase {

		tfloat GetFactor(in Entity agent);

	}
	
	[SerializedReferenceCaption("Default/Override Cost")]
	public class OverrideCost : GOAPActionModule {
        
		public override ActionEvent requiredEvents => ActionEvent.GetCost;

		[ComponentDataType(ComponentDataTypeAttribute.Type.NoData)]
		public ComponentData<ITargetCost> readCostFromTarget;

		public override tfloat GetCost(in Entity agent) {

			if (this.readCostFromTarget.TryRead(in agent, out var component) == true) {

				return component.GetFactor(in agent) * base.GetCost(in agent);
                
			}

			return base.GetCost(in agent);
            
		}

	}

}