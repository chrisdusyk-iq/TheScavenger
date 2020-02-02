using Unity.Jobs;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

[UpdateAfter(typeof(MoveForwardSystem))]
[UpdateBefore(typeof(TimedDestroySystem))]
public class CollisionSystem : JobComponentSystem
{
	EntityQuery enemyGroup;
	EntityQuery bulletGroup;
	EntityQuery playerGroup;

	protected override void OnCreate()
	{
		playerGroup = GetEntityQuery(typeof(Health), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<PlayerTag>());
		enemyGroup = GetEntityQuery(typeof(Health), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<EnemyTag>());
		bulletGroup = GetEntityQuery(typeof(TimeToLive), ComponentType.ReadOnly<Translation>());
	}

	struct CollisionJob : IJobChunk
	{
		public float radiusSquared;

		public ArchetypeChunkComponentType<Health> healthType;

		[ReadOnly]
		public ArchetypeChunkComponentType<Translation> translationType;

		[DeallocateOnJobCompletion]
		[ReadOnly]
		public NativeArray<Translation> translationsToTestAgainst;

		public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
		{
			var chunkHealths = chunk.GetNativeArray(healthType);
			var chunkTranslations = chunk.GetNativeArray(translationType);

			for (int i = 0; i < chunk.Count; i++)
			{
				float damage = 0f;
				Health health = chunkHealths[i];
				Translation pos = chunkTranslations[i];

				for (int j = 0; j < translationsToTestAgainst.Length; j++)
				{
					Translation pos2 = translationsToTestAgainst[j];

					if (CheckCollision(pos.Value, pos2.Value, radiusSquared))
					{
						damage += 1;
					}
				}

				if (damage > 0)
				{
					health.Value -= damage;
					chunkHealths[i] = health;
				}
			}
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		var healthType = GetArchetypeChunkComponentType<Health>(false);
		var translationType = GetArchetypeChunkComponentType<Translation>(true);

		float enemyRadius = 0.5f;
		float playerRadius = 2.5f;

		var jobEvB = new CollisionJob()
		{
			radiusSquared = enemyRadius * enemyRadius,
			healthType = healthType,
			translationType = translationType,
			translationsToTestAgainst = bulletGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
		};

		JobHandle jobHandle = jobEvB.Schedule(enemyGroup, inputDeps);

		// Handle player death
		if (GameManager.IsPlayerDead())
			return jobHandle;

		var jobPvE = new CollisionJob()
		{
			radiusSquared = playerRadius * playerRadius,
			healthType = healthType,
			translationType = translationType,
			translationsToTestAgainst = enemyGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
		};

		return jobPvE.Schedule(playerGroup, jobHandle);
	}

	static bool CheckCollision(float3 posA, float3 posB, float radiusSqr)
	{
		float3 delta = posA - posB;
		float distanceSquare = delta.x * delta.x + delta.y * delta.y;

		return distanceSquare <= radiusSqr;
	}
}
