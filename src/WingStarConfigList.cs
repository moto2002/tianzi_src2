using System;
using UnityEngine;

public class WingStarConfigList : BaseVoList
{
	[SerializeField]
	public WingStarConfig[] list;

	public override void Destroy()
	{
		this.list = new WingStarConfig[0];
	}
}
