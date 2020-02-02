using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class EnemyBehaviour : MonoBehaviour
{
	[Header("Movement")]
	public float speed = 5f;
	[Header("Life Settings")]
	public int enemyHealth = 1;

	Rigidbody2D rigidBody;

	void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if (!GameManager.IsPlayerDead())
		{
			Vector3 heading = GameManager.PlayerPosition - transform.position;
			heading.z = 0f;
			transform.rotation = Quaternion.LookRotation(heading);
		}

		Vector3 movement = transform.forward * speed * Time.deltaTime;
		rigidBody.MovePosition(transform.position + movement);
	}
	void OnTriggerEnter(Collider theCollider)
	{
		if (!theCollider.CompareTag("Bullet"))
			return;

		enemyHealth--;

		if (enemyHealth <= 0)
		{
			Destroy(gameObject);
		}
	}

	//public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
	//{
	//	manager.AddComponent(entity, typeof(EnemyTag));
	//	manager.AddComponent(entity, typeof(MoveForward));

	//	MoveSpeed moveSpeed = new MoveSpeed { Value = speed };
	//	manager.AddComponentData(entity, moveSpeed);

	//	Health health = new Health { Value = enemyHealth };
	//	manager.AddComponentData(entity, health);
	//}
}