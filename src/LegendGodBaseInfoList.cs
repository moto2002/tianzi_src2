using System;
using UnityEngine;

public class LegendGodBaseInfoList : BaseVoList
{
	[SerializeField]
	public LegendGodBaseInfoClass[] list;

	public override void Destroy()
	{
		this.list = new LegendGodBaseInfoClass[0];
	}
}
