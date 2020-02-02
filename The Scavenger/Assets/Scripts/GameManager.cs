using Unity;
using Unity.Entities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager main;
	public Transform player;

	EntityManager entityManager;

	private void Awake()
	{
		main = this;
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	public static void PlayerDied()
	{
		if (main.player == null)
			return;

		main.player = null;
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
