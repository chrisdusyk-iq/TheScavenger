using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PlayerMovementData : IComponentData
{
	public Vector2 direction;
	public float speed;
}
