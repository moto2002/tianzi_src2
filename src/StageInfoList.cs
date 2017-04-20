using System;
using UnityEngine;

public class StageInfoList : BaseVoList
{
	[SerializeField]
	public StageInfo[] list;

	public override void Destroy()
	{
		this.list = new StageInfo[0];
	}
}
