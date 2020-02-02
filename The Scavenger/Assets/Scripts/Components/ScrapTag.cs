using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct ScrapTag : IComponentData
{
	public bool pickedUp;
}
