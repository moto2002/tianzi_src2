using System;
using UnityEngine;

public class Guild_building_devil_configDataList : BaseVoList
{
	[SerializeField]
	public Guild_building_devil_configDataVo[] list;

	public override void Destroy()
	{
		this.list = new Guild_building_devil_configDataVo[0];
	}
}
