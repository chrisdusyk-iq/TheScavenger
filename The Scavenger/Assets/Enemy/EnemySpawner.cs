using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[Header("Enemy Spawn Info")]
	public bool spawnEnemies = false;
	public float enemySpawnRadius = 25f;

	[Header("Enemy Spawn Timing")]
	[Range(1, 100)] public int spawnsPerInterval = 1;
	[Range(.1f, 2f)] public float spawnInterval = 1f;

	public GameObject enemyPrefab;
	EntityManager manager;
	Entity enemyEntityPrefab;

	BlobAssetStore blobAssetStore;

	float cooldown;

	void Start()
	{
		blobAssetStore = new BlobAssetStore();
		manager = World.DefaultGameObjectInjectionWorld.EntityManager;
		enemyEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(enemyPrefab, GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore));
	}

	private void OnDestroy()
	{
		blobAssetStore.Dispose();
	}

	private void Update()
	{
		if (!spawnEnemies || GameManager.IsPlayerDead())
			return;

		cooldown -= Time.deltaTime;

		if (cooldown <= 0f)
		{
			cooldown += spawnInterval;
			Spawn();
		}
	}
	void Spawn()
	{
		for (int i = 0; i < spawnsPerInterval; i++)
		{
			Vector3 pos = GameManager.GetPositionAroundPlayer(enemySpawnRadius);
			Entity enemy = manager.Instantiate(enemyEntityPrefab);
			manager.SetComponentData(enemy, new Translation { Value = pos });
		}
	}
}
