using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct ProjectileMovementData : IComponentData
{
	public Vector2 direction;
	public float speed;
}
