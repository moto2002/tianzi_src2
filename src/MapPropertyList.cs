using System;
using UnityEngine;

public class MapPropertyList : BaseVoList
{
	[SerializeField]
	public MapProperty[] list;

	public override void Destroy()
	{
		this.list = new MapProperty[0];
	}
}
