using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct MovementData : IComponentData
{
	public Vector2 direction;
	public float speed;
}
