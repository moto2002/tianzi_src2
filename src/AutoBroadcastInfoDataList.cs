using System;
using UnityEngine;

public class AutoBroadcastInfoDataList : BaseVoList
{
	[SerializeField]
	public AutoBroadcastInfoDataVo[] list;

	public override void Destroy()
	{
		this.list = new AutoBroadcastInfoDataVo[0];
	}
}
