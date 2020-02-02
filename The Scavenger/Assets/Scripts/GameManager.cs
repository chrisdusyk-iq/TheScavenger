using Unity;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager main;
	public Transform player;

	EntityManager entityManager;
	

	[Header("Scrap")]
	public GameObject scrapPrefab;

	public Entity scrapEntityPrefab;
	private BlobAssetStore blobAssetStore;

	private void Awake()
	{
		main = this;
		blobAssetStore = new BlobAssetStore();
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		scrapEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(scrapPrefab, GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore));
	}

	private void OnDestroy()
	{
		blobAssetStore.Dispose();
	}

	public static void PlayerDied()
	{
		if (main.player == null)
			return;

		main.player = null;
		
		SceneManager.LoadScene("EndScene");
	}

	public static bool IsPlayerDead()
	{
		return main.player == null;
	}

	public static Vector3 PlayerPosition
	{
		get { return main.player.position; }
	}

	public static Vector3 GetPositionAroundPlayer(float radius)
	{
		Vector3 playerPos = main.player.position;

		float angle = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
		float s = Mathf.Sin(angle);
		float c = Mathf.Cos(angle);

		return new Vector3(c * radius, s * radius, 1.1f) + playerPos;
	}
}
