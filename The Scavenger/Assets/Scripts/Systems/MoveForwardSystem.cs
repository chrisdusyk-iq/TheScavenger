﻿using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Unity.Transforms
{
	public class MoveForwardSystem : JobComponentSystem
	{
		[RequireComponentTag(typeof(MoveForward))]
		struct MoveForwardRotation : IJobForEach<Translation, Rotation, MoveSpeed>
		{
			public float dt;

			public void Execute(ref Translation pos, [ReadOnly] ref Rotation rot, [ReadOnly] ref MoveSpeed speed)
			{
				float3 forward = math.forward(rot.Value);
				forward.z = 0f;
				pos.Value += dt * speed.Value * forward;
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