using System;
using UnityEngine;

public class DailyActivityList : BaseVoList
{
	[SerializeField]
	public DailyActivityVo[] list;

	public override void Destroy()
	{
		this.list = new DailyActivityVo[0];
	}
}
