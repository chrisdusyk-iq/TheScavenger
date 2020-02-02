using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class RemoveDeadSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.ForEach((Entity entity, ref Health health, ref Translation pos) =>
		{
			if (health.Value <= 0)
			{
				if (EntityManager.HasComponent(entity, typeof(PlayerTag)))
				{
					GameManager.PlayerDied();
				}

				else if (EntityManager.HasComponent(entity, typeof(EnemyTag)))
				{
					SpawnScrap(entity);
					PostUpdateCommands.DestroyEntity(entity);
				}

				else if (EntityManager.HasComponent(entity, typeof(ScrapTag)))
				{
					PostUpdateCommands.DestroyEntity(entity);
				}
			}
		});
	}

	private void SpawnScrap(Entity robot)
	{
		Entity scrap = EntityManager.Instantiate(GameManager.main.scrapEntityPrefab);
		EntityManager.SetComponentData(scrap, new Translation { Value = EntityManager.GetComponentData<Translation>(robot).Value });
	}
}