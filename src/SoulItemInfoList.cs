using System;
using UnityEngine;

public class SoulItemInfoList : BaseVoList
{
	[SerializeField]
	public SoulItemInfo[] list;

	public override void Destroy()
	{
		this.list = new SoulItemInfo[0];
	}
}
