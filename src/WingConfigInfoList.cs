using System;
using UnityEngine;

public class WingConfigInfoList : BaseVoList
{
	[SerializeField]
	public WingConfigInfo[] list;

	public override void Destroy()
	{
		this.list = new WingConfigInfo[0];
	}
}
