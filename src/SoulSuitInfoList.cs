using System;
using UnityEngine;

public class SoulSuitInfoList : BaseVoList
{
	[SerializeField]
	public SoulSuitInfo[] list;

	public override void Destroy()
	{
		this.list = new SoulSuitInfo[0];
	}
}
