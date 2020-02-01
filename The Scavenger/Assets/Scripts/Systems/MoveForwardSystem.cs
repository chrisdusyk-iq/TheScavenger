using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Unity.Transforms
{
	public class MoveForwardSystem : JobComponentSystem
	{
		[RequireComponentTag(typeof(MoveForward))]
		struct MoveForwardRotation : IJobForEach<Translation, Rotation, MoveSpeed, LocalToWorld>
		{
			public float dt;

			public void Execute(ref Translation pos, [ReadOnly] ref Rotation rot, [ReadOnly] ref MoveSpeed speed, [ReadOnly] ref LocalToWorld localToWorld)
			{
				pos.Value += (dt * speed.Value * new float3(0f, 1f, 0f));
			}
		}

		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			var moveForwardRotationJob = new MoveForwardRotation
			{
				dt = Time.DeltaTime
			};

			return moveForwardRotationJob.Schedule(this, inputDeps);
		}
	}
}