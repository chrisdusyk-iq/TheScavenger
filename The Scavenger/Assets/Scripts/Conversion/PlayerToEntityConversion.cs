using Unity.Entities;
using UnityEngine;

public class PlayerToEntityConversion : MonoBehaviour, IConvertGameObjectToEntity
{
	public float healthValue = 100f;

	public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
	{
		entityManager.AddComponent(entity, typeof(PlayerTag));

		Health health = new Health { Value = healthValue };
		entityManager.AddComponentData(entity, health);

		InventoryComponent inventory = new InventoryComponent { TotalScrap = 0 };
		entityManager.AddComponentData(entity, inventory);
	}
}