using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public class RobotMovementSystem : JobComponentSystem
{
	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		JobHandle robotMovementHandle = Entities.ForEach((ref Translation translation, in RobotMovementData robotMovementData) =>
		{
			translation.Value = 0f;
		}).Schedule(inputDeps);

		return robotMovementHandle;
	}
}
