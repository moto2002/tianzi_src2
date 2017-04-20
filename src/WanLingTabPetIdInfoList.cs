using System;
using UnityEngine;

public class WanLingTabPetIdInfoList : BaseVoList
{
	[SerializeField]
	public WanLingTabPetIdInfo[] list;

	public override void Destroy()
	{
		this.list = new WanLingTabPetIdInfo[0];
	}
}
