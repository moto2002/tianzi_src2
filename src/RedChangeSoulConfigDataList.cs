using System;
using UnityEngine;

public class RedChangeSoulConfigDataList : BaseVoList
{
	[SerializeField]
	public RedChangeSoulConfigData[] list;

	public override void Destroy()
	{
		this.list = new RedChangeSoulConfigData[0];
	}
}
