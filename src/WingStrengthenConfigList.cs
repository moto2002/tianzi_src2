using System;
using UnityEngine;

public class WingStrengthenConfigList : BaseVoList
{
	[SerializeField]
	public WingStrengthenConfig[] list;

	public override void Destroy()
	{
		this.list = new WingStrengthenConfig[0];
	}
}
