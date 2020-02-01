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
}
