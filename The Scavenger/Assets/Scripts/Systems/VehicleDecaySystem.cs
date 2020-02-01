using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class VehicleDecaySystem : JobComponentSystem
{
	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		var timeDelta = Time.DeltaTime;
		Entities.ForEach((ref VehicleStatusData status) =>
		{
			status.health -= .5f * timeDelta;
		}).Run();

		return default;
	}
}
