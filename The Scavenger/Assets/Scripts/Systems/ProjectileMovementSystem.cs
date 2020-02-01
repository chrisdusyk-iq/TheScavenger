using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class ProjectileMovementSystem : JobComponentSystem
{
	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		JobHandle robotMovementHandle = Entities.ForEach((ref PhysicsVelocity velocity, in MovementData projectileMovementData) =>
		{
			//translation.Value = 0f;
		}).Schedule(inputDeps);

		return robotMovementHandle;
	}
}
