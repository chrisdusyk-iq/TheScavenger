using Unity;
using Unity.Entities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager main;

	EntityManager entityManager;

	private void Awake()
	{
		main = this;
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	// BLAH BLAH BLAH CODE IS HERE
}
