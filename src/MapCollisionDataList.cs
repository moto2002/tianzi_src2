using System;
using UnityEngine;

public class MapCollisionDataList : BaseVoList
{
	[SerializeField]
	public MapCollisionData[] list;

	public override void Destroy()
	{
		this.list = new MapCollisionData[0];
	}
}
