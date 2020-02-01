using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct RobotMovementData : IComponentData
{
	public Vector2 direction;
	public float speed;
}
