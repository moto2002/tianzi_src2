using System;
using UnityEngine;

public class ObjectInfoList : BaseVoList
{
	[SerializeField]
	public ObjectInfo[] list;

	public override void Destroy()
	{
		this.list = new ObjectInfo[0];
	}
}
