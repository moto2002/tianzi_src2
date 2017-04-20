using System;
using UnityEngine;

public class League_time_configDataList : BaseVoList
{
	[SerializeField]
	public League_time_configDataVo[] list;

	public override void Destroy()
	{
		this.list = new League_time_configDataVo[0];
	}
}
