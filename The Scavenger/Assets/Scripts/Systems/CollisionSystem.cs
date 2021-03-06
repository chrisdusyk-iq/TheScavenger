﻿using Unity.Jobs;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Physics.Systems;
using Unity.Physics;

[UpdateAfter(typeof(MoveForwardSystem))]
[UpdateBefore(typeof(TimedDestroySystem))]
public class CollisionSystem : JobComponentSystem
{
	EntityQuery enemyGroup;
	EntityQuery bulletGroup;
	EntityQuery playerGroup;
	EntityQuery scrapGroup;

	private BuildPhysicsWorld buildPhysicsWorld;
	private StepPhysicsWorld stepPhysicsWorld;

	protected override void OnCreate()
	{
		playerGroup = GetEntityQuery(typeof(Health), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<PlayerTag>());
		enemyGroup = GetEntityQuery(typeof(Health), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<EnemyTag>());
		bulletGroup = GetEntityQuery(typeof(TimeToLive), ComponentType.ReadOnly<Translation>());
		scrapGroup = GetEntityQuery(typeof(ScrapTag), ComponentType.ReadOnly<Translation>());

		buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
		stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
	}

	private struct NewCollisionJob : ITriggerEventsJob
	{
		public ComponentDataFromEntity<Health> enemyGroup;

		[ReadOnly]
		public ComponentDataFromEntity<TimeToLive> bulletGroup;

		public void Execute(TriggerEvent triggerEvent)
		{
			if (bulletGroup.HasComponent(triggerEvent.Entities.EntityA))
			{
				if (enemyGroup.HasComponent(triggerEvent.Entities.EntityB))
				{
					Health health = enemyGroup[triggerEvent.Entities.EntityB];
					health.Value -= 1;
					enemyGroup[triggerEvent.Entities.EntityB] = health;
				}
			}

			if (bulletGroup.HasComponent(triggerEvent.Entities.EntityB))
			{
				if (enemyGroup.HasComponent(triggerEvent.Entities.EntityA))
				{
					Health health = enemyGroup[triggerEvent.Entities.EntityA];
					health.Value -= 1;
					enemyGroup[triggerEvent.Entities.EntityA] = health;
				}
			}
		}
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

	struct CollisionWithScrapJob : IJobChunk
	{
		public float radiusSquared;

		public ArchetypeChunkComponentType<ScrapTag> scrapType;

		[ReadOnly]
		public ArchetypeChunkComponentType<Translation> translationType;

		[DeallocateOnJobCompletion]
		[ReadOnly]
		public NativeArray<Translation> translationsToTestAgainst;

		public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndext)
		{
			var chunkScrapTag = chunk.GetNativeArray(scrapType);
			var chunkTranslations = chunk.GetNativeArray(translationType);

			for (int i = 0; i < chunk.Count; i++)
			{
				bool playerPickedUp = false;
				ScrapTag scrapTag = chunkScrapTag[i];
				Translation position = chunkTranslations[i];

				for (int j = 0; j < translationsToTestAgainst.Length; j++)
				{
					Translation position2 = translationsToTestAgainst[j];

					if (CheckCollision(position.Value, position2.Value, radiusSquared))
					{
						playerPickedUp = true;
					}
				}

				if (playerPickedUp)
				{
					Debug.Log("Scrap picked up");
					scrapTag.pickedUp = true;
					chunkScrapTag[i] = scrapTag;
				}
			}
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		var healthType = GetArchetypeChunkComponentType<Health>(false);
		var translationType = GetArchetypeChunkComponentType<Translation>(true);
		var scrapType = GetArchetypeChunkComponentType<ScrapTag>(false);

		float enemyRadius = 1f;
		float playerRadius = 2.5f;
		float scrapRadius = 1f;

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

		var playerVersusEnemiesHandle = jobPvE.Schedule(playerGroup, jobHandle);

		var jobScrapOnPlayer = new CollisionWithScrapJob()
		{
			radiusSquared = scrapRadius * scrapRadius,
			scrapType = scrapType,
			translationType = translationType,
			translationsToTestAgainst = playerGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
		};

		return jobScrapOnPlayer.Schedule(scrapGroup, playerVersusEnemiesHandle);

		//var enemyBulletsCollisionJob = new NewCollisionJob()
		//{
		//	bulletGroup = GetComponentDataFromEntity<TimeToLive>(),
		//	enemyGroup = GetComponentDataFromEntity<Health>()
		//};

		//return enemyBulletsCollisionJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
	}

	static bool CheckCollision(float3 posA, float3 posB, float radiusSqr)
	{
		float3 delta = posA - posB;
		float distanceSquare = delta.x * delta.x + delta.y * delta.y;

		return distanceSquare <= radiusSqr;
	}
}
