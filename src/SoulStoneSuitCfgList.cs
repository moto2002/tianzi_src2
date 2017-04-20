using System;
using UnityEngine;

public class SoulStoneSuitCfgList : BaseVoList
{
	[SerializeField]
	public SoulStoneSuitCfg[] list;

	public override void Destroy()
	{
		this.list = new SoulStoneSuitCfg[0];
	}
}
