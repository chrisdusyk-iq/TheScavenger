using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class RobotMovementSystem : JobComponentSystem
{
	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		JobHandle robotMovementHandle = Entities.ForEach((ref Transform transform, in RobotMovementData robotMovementData) =>
		{

		}).Schedule(inputDeps);

		return robotMovementHandle;
	}
}
