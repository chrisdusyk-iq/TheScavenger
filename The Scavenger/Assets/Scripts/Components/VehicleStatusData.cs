using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct VehicleStatusData : IComponentData
{
	public float health;
}
